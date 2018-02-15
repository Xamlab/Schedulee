using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Schedulee.UI.ViewModels.Base
{
    public interface IViewModelValidator : INotifyDataErrorInfo
    {
        bool Validate();
        IList GetAllErrors(params string[] propertyNames);
        List<string> GetAllErrorsInString();
        List<string> GetErrorsInString(string propertyName);
    }
}