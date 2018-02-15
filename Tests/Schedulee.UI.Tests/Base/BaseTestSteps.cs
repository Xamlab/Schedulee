using System.Linq;
using Grace.DependencyInjection;
using Schedulee.Core;
using Schedulee.Core.DI;
using Schedulee.Core.DI.Implementation;
using Schedulee.UI.Services;
using Schedulee.UI.Tests.Services.Implementation;
using Schedulee.UI.ViewModels.Base;
using Shouldly;
using TechTalk.SpecFlow;

namespace Schedulee.UI.Tests.Base
{
    public class BaseTestSteps
    {
        protected IDependencyContainer Container { get; private set; }

        protected MockDialogService DialogService { get; private set; }

        [BeforeScenario]
        public virtual void BeforeScenarioSetup()
        {
            SetupContainer();
        }

        [AfterScenario]
        public virtual void AfterScenarioCleanup()
        {
            Container?.Dispose();
        }

        private void SetupContainer()
        {
            var graceContainer = new DependencyInjectionContainer();
            Container = new DependencyContainer(graceContainer);
            Container.RegisterCoreDependencies()
                     .RegisterUIDependencies();
            DialogService = new MockDialogService();
            Container.RegisterSingleton<IDialogService>(DialogService);
            ConfigureServices();
        }

        protected virtual void ConfigureServices()
        {
        }

        protected void CheckValidationError(IViewModelValidator validator, string property, string expectedMessage)
        {
            var stringError = validator.GetErrorsInString(property)
                                       .FirstOrDefault(error => error == expectedMessage);

            stringError.ShouldNotBeNull();
            DialogService.DialogMessage.Contains(stringError).ShouldBeTrue();
        }
    }
}