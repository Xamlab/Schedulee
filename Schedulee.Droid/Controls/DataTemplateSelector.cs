using System;
using System.ComponentModel;

namespace Schedulee.Droid.Controls
{
    public abstract class DataTemplateSelector : DataTemplate
    {
        public DataTemplate SelectTemplate(object item, IBindableObject container)
        {
            var dataTemplate = OnSelectTemplate(item, container);
            if(dataTemplate is DataTemplateSelector)
            {
                throw new NotSupportedException("DataTemplateSelector.OnSelectTemplate must not return another DataTemplateSelector");
            }

            return dataTemplate;
        }

        protected abstract DataTemplate OnSelectTemplate(object item, INotifyPropertyChanged container);
    }
}