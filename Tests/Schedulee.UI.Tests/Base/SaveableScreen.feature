Feature: Saveable Screen 
	All arround the app there many screens which allow users to edit and save data.
	In this type of screens users should be informed if there are unfixed validation errors.
	Users should presented with loading indicator to show that saving is in progress.
	And if saving wasn't successful users should be notified about the error.


Scenario: User tries to perform save operation while there are validation errors
	Given I have navigated to a saveable screen
	And I have entered invalid data at least in one of the fields
	When I try to save the data
	Then a message is shown warning the user to fix validation errors before trying to save

Scenario: User triggers save process
	Given I have navigated to a saveable screen
	When I try to save the data
	Then a loading indicator is shown while saving is in progress

Scenario: Saving process completes successfully
	Given I have navigated to a saveable screen
	And I've already triggered saving process
	When the saving process completes successfully
	Then the saving loading indicator is hidden
	And the saveable screen should be dismissed

Scenario: Saving process fails
	Given I have navigated to a saveable screen
	And I've already triggered saving process
	When the saving process fails
	Then the saving loading indicator is hidden
	And a message is shown detailing the failure reason

Scenario:  Saving process is canceled
	Given I have navigated to a saveable screen
	And I've already triggered saving process
	When the saving is canceled
	Then the saving is stopped and the loading indicator is hidden

