using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using PropertyChanged;
using Schedulee.Core.Extensions;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class BaseStaleMonitor : IStaleMonitor
    {
        private readonly INotifyPropertyChanged _viewModel;
        private Dictionary<string, object> _values;
        private Dictionary<string, object[]> _collections;
        private IEnumerable<PropertyInfo> _collectionProperties;
        private IEnumerable<PropertyInfo> _properties;

        public BaseStaleMonitor(INotifyPropertyChanged viewModel)
        {
            _viewModel = viewModel;
        }

        public bool IsStale => GetIsStale();

        public virtual IEnumerable<string> Properties => null;
        public virtual IEnumerable<string> Collections => null;

        public void StartMonitoring()
        {
            CaptureProperties();
            CaptureCollectionProperties();
        }

        public void CaptureProperties(params string[] properties)
        {
            if(properties?.Any() != true)
            {
                _properties = _viewModel.GetType().GetRuntimeProperties().Where(p => Properties?.Contains(p.Name) == true);
                _values = _properties.ToDictionary(property => property.Name, property => _viewModel.GetPropertyValue(property.Name));
            }
            else
            {
                foreach(var property in properties)
                {
                    CaptureProperty(property);
                }
            }
        }

        public void CaptureCollectionProperties(params string[] properties)
        {
            if(properties?.Any() != true)
            {
                var enumerableInfo = typeof(IEnumerable).GetTypeInfo();

                _collectionProperties = _viewModel.GetType().GetRuntimeProperties()
                                                  .Where(p => Collections?.Contains(p.Name) == true && enumerableInfo.IsAssignableFrom(p.PropertyType.GetTypeInfo()));
                _collections = _collectionProperties.ToDictionary(property => property.Name, property =>
                                                                                             {
                                                                                                 var enumerable = (IEnumerable) property.GetValue(_viewModel);
                                                                                                 return enumerable.ToArray();
                                                                                             });
            }
            else
            {
                foreach(var property in properties)
                {
                    CaptureProperty(property, true);
                }
            }
        }

        public bool ArePropertiesStale(params string[] properties)
        {
            var propertiesToCheck = properties;
            if(propertiesToCheck?.Any() != true)
            {
                propertiesToCheck = (_properties?.Select(property => property.Name)).ToArray();
            }

            if(propertiesToCheck == null) return false;
            foreach(var property in propertiesToCheck)
            {
                if(CheckPropertyIsStale(property)) return true;
            }

            return false;
        }

        public bool AreCollectionsStale(params string[] properties)
        {
            var propertiesToCheck = properties;
            if(propertiesToCheck?.Any() != true)
            {
                propertiesToCheck = (_collectionProperties?.Select(property => property.Name)).ToArray();
            }

            if(propertiesToCheck == null) return false;
            foreach(var property in propertiesToCheck)
            {
                if(CheckCollectionIsState(property)) return true;
            }

            return false;
        }

        private void CaptureProperty(string property, bool isCollection = false)
        {
            if(isCollection)
            {
                var enumerable = (IEnumerable) _viewModel.GetPropertyValue(property);
                _collections = _collections ?? new Dictionary<string, object[]>();
                _collections[property] = enumerable.ToArray();
            }
            else
            {
                _values = _values ?? new Dictionary<string, object>();
                _values[property] = _viewModel.GetPropertyValue(property);
            }
        }

        private bool GetIsStale()
        {
            if(ArePropertiesStale()) return true;
            if(AreCollectionsStale()) return true;
            return false;
        }

        private bool CheckPropertyIsStale(string property)
        {
            object oldValue = null;
            var newValue = _viewModel?.GetPropertyValue(property);
            _values?.TryGetValue(property, out oldValue);
            var isStale = !newValue.ObjectsEqual(oldValue);
            return isStale;
        }

        private bool CheckCollectionIsState(string property)
        {
            object[] oldCollection = null;
            _collections?.TryGetValue(property, out oldCollection);
            var newCollection = (IEnumerable) _viewModel.GetPropertyValue(property);
            return !oldCollection.EnumerableEqual(newCollection);
        }
    }
}