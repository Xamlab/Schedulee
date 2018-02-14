Feature: Loadable Screen 
	All arround the app there is one thing common for a many screens - 
	they perform some kind of background operation to load data.
	And it's common functionality to show loading indicator and 
	if something fails notify the user with description of the failure.
	Besides that loadable screens should be able to differentiate if the
	data is loaded or no, so the main content will be hidden/shown.


Scenario: Background operation is in progress
	Given I have navigated to a loadable screen
	When the screen is loaded and the background operation is in progress
	Then a loading indicator is shown while operation is in progress

Scenario: Background operation completes successfully
	Given I have navigated to a loadable screen
	And the screen is loaded and the background operation is in progress
	When the background operation completes successfully
	Then the loading indicator is hidden
	And the content is shown

Scenario: Background operation fails
	Given I have navigated to a loadable screen
	And the screen is loaded and the background operation is in progress
	When the background operation fails
	Then an error specific to the failure is displayed detailing the issue
	And the content is hidden

Scenario: Background operation is canceled
	Given I have navigated to a loadable screen
	And the screen is loaded and the background operation is in progress
	When the background operation is canceled
	Then the background operation is stopped
	And the loading indicator is hidden
	And the content is hidden
