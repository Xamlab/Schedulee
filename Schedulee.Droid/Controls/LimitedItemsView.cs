using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PropertyChanged;

namespace Schedulee.Droid.Controls
{
    [AddINotifyPropertyChangedInterface]
    public abstract class LimitedItemsView<T> : BindableLinearLayout
        where T : INotifyPropertyChanged
    {
        private ViewButton _seeMoreButton;
        private INotifyCollectionChanged _observedItems;
        private bool _isExpanded;
        private readonly Context _context;
        private bool _isSeeMoreButtonAdded;

        public LimitedItemsView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public LimitedItemsView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;
            Initialize(context);
        }

        public LimitedItemsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            _context = context;
            Initialize(context);
        }

        public LimitedItemsView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            _context = context;
            Initialize(context);
        }

        public LimitedItemsView(Context context) : base(context)
        {
            _context = context;
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            Orientation = Orientation.Vertical;
            var textView = new TextView(context)
                           {
                               Text = "See more"
                           };
            textView.SetPadding(20, 10, 20, 10);
            textView.SetBackgroundColor(Color.Purple);
            textView.SetTextColor(Color.White);
            _seeMoreButton = new ViewButton(context);

            _seeMoreButton.AddView(textView);
            _seeMoreButton.Clicked += SeeMoreButtonClicked;
        }

        public event EventHandler<EventArgs> ItemClicked;

        private IEnumerable<T> _items;

        public IEnumerable<T> Items
        {
            get => _items;
            set
            {
                if(_items != value)
                {
                    _items = value;
                    UpdateItems();
                }
            }
        }

        private DataTemplate _itemTemplate;

        public DataTemplate Itemtemplate
        {
            get => _itemTemplate;
            set
            {
                if(_itemTemplate != value)
                {
                    _itemTemplate = value;
                    LoadChildren();
                }
            }
        }

        private int _limit = -1;

        public int Limit
        {
            get => _limit;
            set
            {
                if(_limit != value)
                {
                    _limit = value;
                    UpdateItems();
                }
            }
        }

        private bool _itemClickable;

        public bool ItemClickable
        {
            get => _itemClickable;
            set
            {
                if(_itemClickable != value)
                {
                    _itemClickable = value;
                    UpdateItems();
                }
            }
        }

        public T SelectedItem { get; set; }

        public abstract void AddChildView(View view, int index);

        public abstract void RemoveChildView(int index);

        private void AddItem(T item, int index = -1)
        {
            DataTemplate template = Itemtemplate;
            if(Itemtemplate is DataTemplateSelector templateSelector)
            {
                template = templateSelector.SelectTemplate(item, this);
            }

            var content = template.CreateContent(_context);
            if(!(content is IBindableObject bindableObject)) throw new ArgumentException($"{content.GetType()} should be {typeof(IBindableObject)}");
            if(!(content is View view)) throw new ArgumentException($"{content.GetType()} should be {typeof(View)}");

            bindableObject.BindingContext = item;
            if(ItemClickable)
            {
                view.Clickable = true;
                view.Click += ViewOnClicked;
                AddChildView(view, index);
            }
            else
            {
                AddChildView(view, index);
            }
        }

        private void RemoveItem(int index)
        {
            RemoveChildView(index);
        }

        private void UpdateItems()
        {
            DisconnectEvents();
            _observedItems = null;
            _isExpanded = false;
            Clear();

            _observedItems = Items as INotifyCollectionChanged;
            ConnectEvents();
            LoadChildren();
        }

        private void SeeMoreButtonClicked(object sender, EventArgs e)
        {
            _isExpanded = true;
            RemoveViewAt(1);
            for(var i = Limit; i < Items.Count(); i++)
            {
                var item = Items.ElementAt(i);
                AddItem(item);
            }
        }

        private void ViewOnClicked(object sender, EventArgs eventArgs)
        {
            if(!(GetChildAt(0) is LinearLayout root)) return;
            var index = root.IndexOfChild((View) sender);
            SelectedItem = Items.ElementAt(index);
            ItemClicked?.Invoke(SelectedItem, EventArgs.Empty);
        }

        private void ConnectEvents()
        {
            if(_observedItems == null) return;
            _observedItems.CollectionChanged += OnItemsCollectionChanged;
        }

        protected virtual void Clear()
        {
            DisConnectButtonEvents();
            RemoveAllViews();
        }

        private void DisconnectEvents()
        {
            if(_observedItems == null) return;
            _observedItems.CollectionChanged -= OnItemsCollectionChanged;
        }

        private void DisConnectButtonEvents()
        {
            if(GetChildAt(0) is LinearLayout root && root.ChildCount > 0)
            {
                for(int i = 0; i < root.ChildCount; i++)
                {
                    var child = root.GetChildAt(i);
                    child.Click -= ViewOnClicked;
                }
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadChildren(e);
        }

        protected virtual void LoadChildren(NotifyCollectionChangedEventArgs args = null)
        {
            if(Itemtemplate == null) return;
            if(args == null)
            {
                LoadViews();
                return;
            }

            switch(args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    LoadAfterInsert(args);
                    break;
                case NotifyCollectionChangedAction.Move:
                    LoadAfterMove(args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    LoadAfterRemove(args);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    LoadAfterReplace(args);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    LoadAfterReset();
                    break;
            }
        }

        private void LoadAfterInsert(NotifyCollectionChangedEventArgs args)
        {
            if(!(GetChildAt(0) is LinearLayout root)) return;

            if(!_isExpanded && root.ChildCount >= Limit)
            {
                if(args.NewStartingIndex < Limit)
                {
                    if(root.ChildCount >= Limit)
                    {
                        RemoveItem(args.NewStartingIndex);
                    }

                    AddItem((T) args.NewItems[0], args.NewStartingIndex);
                }
            }
            else
            {
                AddItem((T) args.NewItems[0], args.NewStartingIndex);
            }

            SetupSeeMoreButton();
        }

        private void LoadAfterMove(NotifyCollectionChangedEventArgs args)
        {
            if(args.NewStartingIndex == args.OldStartingIndex) return;

            if(_isExpanded || args.NewStartingIndex < Limit && args.OldStartingIndex < Limit)
            {
                RemoveItem(args.OldStartingIndex);
                AddItem((T) args.NewItems[0], args.NewStartingIndex);
            }
            else
            {
                if(args.NewStartingIndex < Limit)
                {
                    RemoveItem(Limit - 1);
                    AddItem((T) args.NewItems[0], args.NewStartingIndex);
                }
                else
                {
                    RemoveItem(args.OldStartingIndex);
                    AddItem(Items.ElementAt(Limit - 1), Limit - 1);
                }
            }
        }

        private void LoadAfterRemove(NotifyCollectionChangedEventArgs args)
        {
            var count = Items?.Count() ?? 0;
            if(!(count > 0)) return;
            if(!_isExpanded)
            {
                if(args.OldStartingIndex <= Limit)
                {
                    RemoveItem(args.OldStartingIndex);
                    if(count >= Limit)
                    {
                        AddItem(Items.ElementAt(args.OldStartingIndex + 1), Limit - 1);
                    }
                }
            }
            else
            {
                RemoveItem(args.OldStartingIndex);
            }

            SetupSeeMoreButton();
        }

        private void LoadAfterReplace(NotifyCollectionChangedEventArgs args)
        {
            if(args.NewItems[0] == args.OldItems[0]) return;
            if(_isExpanded || args.NewStartingIndex < Limit)
            {
                RemoveItem(args.OldStartingIndex);
                AddItem((T) args.NewItems[0], args.NewStartingIndex);
            }
        }

        private void LoadAfterReset()
        {
            Clear();
        }

        private void LoadViews()
        {
            var count = Items?.Count() ?? 0;
            if(!(count > 0)) return;
            Clear();
            var i = 0;
            foreach(var item in Items)
            {
                if(!_isExpanded && Limit >= 0 && i >= Limit) break;
                AddItem(item);
                i++;
            }

            SetupSeeMoreButton();
        }

        private void SetupSeeMoreButton()
        {
            var count = Items?.Count() ?? 0;
            if(count > Limit && Limit != -1)
            {
                if(_seeMoreButton.GetChildAt(0) is TextView textView)
                {
                    textView.Text = $"See {count - Limit} more";
                }

                if(!_isSeeMoreButtonAdded)
                {
                    var layoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    AddView(_seeMoreButton, layoutParams);
                    _isSeeMoreButtonAdded = true;
                }
            }
            else
            {
                if(_isSeeMoreButtonAdded) RemoveView(_seeMoreButton);
            }
        }
    }
}