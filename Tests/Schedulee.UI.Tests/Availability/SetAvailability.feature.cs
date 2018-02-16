﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.2.0.0
//      SpecFlow Generator Version:2.2.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Schedulee.UI.Tests.Availability
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Set Availability")]
    public partial class SetAvailabilityFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SetAvailability.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Set Availability", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Loading user availability data")]
        public virtual void LoadingUserAvailabilityData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Loading user availability data", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "DaysOfWeek",
                        "TimePeriods"});
            table1.AddRow(new string[] {
                        "Mon, Wed",
                        "10:00 - 11:00, 13:00 - 16:00"});
            table1.AddRow(new string[] {
                        "Tue, Thu, Sun",
                        "09:00 - 18:00"});
#line 5
 testRunner.Given("I have some availability data already stored", ((string)(null)), table1, "Given ");
#line 9
 testRunner.And("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.When("the page completes loading", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
 testRunner.Then("I should see the correct availability information", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Starting adding availability process")]
        public virtual void StartingAddingAvailabilityProcess()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Starting adding availability process", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
 testRunner.When("I start adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
 testRunner.Then("I should no longer be able to start another add availability process until this o" +
                    "ne finishes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "weekdays"});
            table2.AddRow(new string[] {
                        "Mon, Tue, Wed, Thu, Fri, Sat, Sun"});
#line 17
 testRunner.And("I should see the week days, and none should be selected", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Operation"});
            table3.AddRow(new string[] {
                        "AddTimePeriodCommand"});
            table3.AddRow(new string[] {
                        "DeleteTimePeriodCommand"});
            table3.AddRow(new string[] {
                        "ToggleDayCommand"});
            table3.AddRow(new string[] {
                        "CancelCommand"});
            table3.AddRow(new string[] {
                        "SaveCommand"});
#line 20
 testRunner.And("I Should already be able to perform following availability manipulation operation" +
                    "s", ((string)(null)), table3, "And ");
#line 27
 testRunner.And("view should be marked as non-stale", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Trying to start adding availability process while there\'s another in progress")]
        public virtual void TryingToStartAddingAvailabilityProcessWhileTheresAnotherInProgress()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Trying to start adding availability process while there\'s another in progress", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.When("I try to start adding availability process one more time", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Message"});
            table4.AddRow(new string[] {
                        "You\'re already in mid of adding new time available. Please complete the process b" +
                            "efore trying to add a new one."});
#line 33
 testRunner.Then("I should see the following message", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Trying to perform availability relating operation while availability process isn\'" +
            "t started")]
        [NUnit.Framework.TestCaseAttribute("AddTimePeriodCommand", null)]
        [NUnit.Framework.TestCaseAttribute("DeleteTimePeriodCommand", null)]
        [NUnit.Framework.TestCaseAttribute("ToggleDayCommand", null)]
        [NUnit.Framework.TestCaseAttribute("CancelCommand", null)]
        [NUnit.Framework.TestCaseAttribute("SaveCommand", null)]
        public virtual void TryingToPerformAvailabilityRelatingOperationWhileAvailabilityProcessIsntStarted(string operation, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Trying to perform availability relating operation while availability process isn\'" +
                    "t started", exampleTags);
#line 37
this.ScenarioSetup(scenarioInfo);
#line 38
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
 testRunner.When(string.Format("I try to perform availability related \'{0}\'", operation), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Message"});
            table5.AddRow(new string[] {
                        "Please start adding time available before trying to continue."});
#line 40
 testRunner.Then("I should see the following message", ((string)(null)), table5, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Adding new time period")]
        public virtual void AddingNewTimePeriod()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Adding new time period", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 52
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 53
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "TimePeriod"});
            table6.AddRow(new string[] {
                        "10:00 - 11:00"});
#line 54
 testRunner.When("I try to add following time period", ((string)(null)), table6, "When ");
#line 57
 testRunner.Then("the time period should be added to the list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deleting time period")]
        public virtual void DeletingTimePeriod()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deleting time period", ((string[])(null)));
#line 59
this.ScenarioSetup(scenarioInfo);
#line 60
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 61
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "TimePeriod"});
            table7.AddRow(new string[] {
                        "10:00 - 11:00"});
#line 62
 testRunner.And("I have added following time period", ((string)(null)), table7, "And ");
#line 65
 testRunner.When("I try to delete that time period", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 66
 testRunner.Then("the time period should be removed from the list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Selecting day of week")]
        public virtual void SelectingDayOfWeek()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Selecting day of week", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line 69
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 70
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "DayOfWeek"});
            table8.AddRow(new string[] {
                        "Wed"});
#line 71
 testRunner.When("I try to select following day of week", ((string)(null)), table8, "When ");
#line 74
 testRunner.Then("That day of week should be selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delecting day of week")]
        public virtual void DelectingDayOfWeek()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delecting day of week", ((string[])(null)));
#line 76
this.ScenarioSetup(scenarioInfo);
#line 77
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 78
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "DayOfWeek"});
            table9.AddRow(new string[] {
                        "Wed"});
#line 79
 testRunner.And("I have already selected following day of week", ((string)(null)), table9, "And ");
#line 82
 testRunner.When("I try to deselect that day", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 83
 testRunner.Then("That day of week should be deselected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Try to cancel adding availability process")]
        public virtual void TryToCancelAddingAvailabilityProcess()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Try to cancel adding availability process", ((string[])(null)));
#line 85
this.ScenarioSetup(scenarioInfo);
#line 86
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 87
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "DayOfWeek"});
            table10.AddRow(new string[] {
                        "Wed"});
#line 88
 testRunner.And("I have already selected following day of week", ((string)(null)), table10, "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "TimePeriod"});
            table11.AddRow(new string[] {
                        "10:00 - 11:00"});
#line 91
 testRunner.And("I have added following time period", ((string)(null)), table11, "And ");
#line 94
 testRunner.When("I cancel the adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Message"});
            table12.AddRow(new string[] {
                        "There are some pending changes, are you sure you want to discard them?"});
#line 95
 testRunner.Then("I should see the following message", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Confirming the cancellation of adding availability process")]
        public virtual void ConfirmingTheCancellationOfAddingAvailabilityProcess()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Confirming the cancellation of adding availability process", ((string[])(null)));
#line 99
this.ScenarioSetup(scenarioInfo);
#line 100
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 101
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "DayOfWeek"});
            table13.AddRow(new string[] {
                        "Wed"});
#line 102
 testRunner.And("I have already selected following day of week", ((string)(null)), table13, "And ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "TimePeriod"});
            table14.AddRow(new string[] {
                        "10:00 - 11:00"});
#line 105
 testRunner.And("I have added following time period", ((string)(null)), table14, "And ");
#line 108
 testRunner.When("I cancel the adding availability process and I confirm that I want to discard the" +
                    " changes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 109
 testRunner.Then("all added time periods should be cleared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 110
 testRunner.And("all days of week should be deselected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
 testRunner.And("view should be marked as non-stale", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
 testRunner.And("I should be able to start another adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Saving the availability information")]
        public virtual void SavingTheAvailabilityInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Saving the availability information", ((string[])(null)));
#line 114
this.ScenarioSetup(scenarioInfo);
#line 115
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 116
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "DayOfWeek"});
            table15.AddRow(new string[] {
                        "Wed"});
#line 117
 testRunner.And("I have already selected following day of week", ((string)(null)), table15, "And ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "TimePeriod"});
            table16.AddRow(new string[] {
                        "10:00 - 11:00"});
#line 120
 testRunner.And("I have added following time period", ((string)(null)), table16, "And ");
#line 123
 testRunner.When("I save the availability information", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 124
 testRunner.Then("Availability should be saved in database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 125
 testRunner.And("The newly created availability should be added to the list of availabilities", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
 testRunner.And("all added time periods should be cleared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
 testRunner.And("all days of week should be deselected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
 testRunner.And("view should be marked as non-stale", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
 testRunner.And("I should be able to start another adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Trying to add time availability while entering invalid data")]
        [NUnit.Framework.TestCaseAttribute("DaysOfWeek", "NoDateSelected", "Please select  at least one day of week before adding new time available.", null)]
        [NUnit.Framework.TestCaseAttribute("TimePeriods", "TimePeriods", "Please add at least one time period before adding new time available.", null)]
        public virtual void TryingToAddTimeAvailabilityWhileEnteringInvalidData(string field, string data, string validation_Error, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Trying to add time availability while entering invalid data", exampleTags);
#line 131
this.ScenarioSetup(scenarioInfo);
#line 132
 testRunner.Given("I have navigated to set availability page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 133
 testRunner.And("I have started adding availability process", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
 testRunner.And(string.Format("I have entered invalid \'{0}\' into \'{1}\'", data, field), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
 testRunner.When("I try to save changes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 136
 testRunner.Then(string.Format("I should see \'{0}\' detailing the \'{1}\' validation issue for \'{2}\'", validation_Error, data, field), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
