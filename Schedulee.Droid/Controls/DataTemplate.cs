using System;
using Android.Content;

namespace Schedulee.Droid.Controls
{
    public class DataTemplate : IDataTemplate
    {
        internal DataTemplate()
        {
        }

        internal DataTemplate(Func<Context, object> loadTemplate) : this()
        {
            LoadTemplate = loadTemplate ?? throw new ArgumentNullException(nameof(loadTemplate));
        }

		public Func<Context, object> LoadTemplate { get; set; }

		public object CreateContent(Context context)
        {
            if(LoadTemplate == null)
                throw new InvalidOperationException("LoadTemplate should not be null");
            if(this is DataTemplateSelector)
                throw new InvalidOperationException("Cannot call CreateContent directly on a DataTemplateSelector");

            object item = LoadTemplate(context);
            SetupContent(item);

            return item;
        }

        internal virtual void SetupContent(object item)
        {
        }
    }
}