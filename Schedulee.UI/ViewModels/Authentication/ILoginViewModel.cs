﻿using System;
using Schedulee.Core.Models;
using Schedulee.UI.ViewModels.Base;

namespace Schedulee.UI.ViewModels.Authentication
{
    public interface ILoginViewModel : ISaveableViewModel
    {
        string Email { get; set; }
        string Password { get; set; }
        event EventHandler<Token> LoginCompleted;
    }
}