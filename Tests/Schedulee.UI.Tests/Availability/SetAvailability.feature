Feature: Set Availability


Scenario: Loading user availability data
	Given I have some availability data already stored
		| DaysOfWeek    | TimePeriods                  |
		| Mon, Wed      | 10:00 - 11:00, 13:00 - 16:00 |
		| Tue, Thu, Sun | 09:00 - 18:00                |
	And I have navigated to set availability page
	When the page completes loading
	Then I should see the correct availability information

Scenario: Starting adding availability process
	Given I have navigated to set availability page
	When I start adding availability process
	Then I should no longer be able to start another add availability process until this one finishes
	And I should see the week days, and none should be selected
		| weekdays                          |
		| Mon, Tue, Wed, Thu, Fri, Sat, Sun |
	And I Should already be able to perform following availability manipulation operations
		| Operation               |
		| AddTimePeriodCommand    |
		| DeleteTimePeriodCommand |
		| ToggleDayCommand        |
		| CancelCommand           |
		| SaveCommand             |
	And view should be marked as non-stale

Scenario: Trying to start adding availability process while there's another in progress
	Given I have navigated to set availability page
	And I have started adding availability process
	When I try to start adding availability process one more time
	Then I should see the following message
		| Message                                                                                                         |
		| You're already in mid of adding new time available. Please complete the process before trying to add a new one. |

Scenario Outline: Trying to perform availability relating operation while availability process isn't started
	Given I have navigated to set availability page
	When I try to perform availability related '<Operation>'
	Then I should see the following message
		| Message                                                       |
		| Please start adding time available before trying to continue. |
	Examples: 
		| Operation               |
		| AddTimePeriodCommand    |
		| DeleteTimePeriodCommand |
		| ToggleDayCommand        |
		| CancelCommand           |
		| SaveCommand             |

Scenario: Adding new time period
	Given I have navigated to set availability page
	And I have started adding availability process
	When I try to add following time period
		| TimePeriod    |
		| 10:00 - 11:00 |
	Then the time period should be added to the list

Scenario: Deleting time period
	Given I have navigated to set availability page
	And I have started adding availability process
	And I have added following time period
		| TimePeriod    |
		| 10:00 - 11:00 |
	When I try to delete that time period
	Then the time period should be removed from the list

Scenario: Selecting day of week
	Given I have navigated to set availability page
	And I have started adding availability process
	When I try to select following day of week
		| DayOfWeek |
		| Wed       |
	Then That day of week should be selected

Scenario: Delecting day of week
	Given I have navigated to set availability page
	And I have started adding availability process
	And I have already selected following day of week
		| DayOfWeek |
		| Wed       |
	When I try to deselect that day
	Then That day of week should be deselected

Scenario: Try to cancel adding availability process
	Given I have navigated to set availability page
	And I have started adding availability process
	And I have already selected following day of week
		| DayOfWeek |
		| Wed       |
	And I have added following time period
		| TimePeriod    |
		| 10:00 - 11:00 |
	When I cancel the adding availability process
	Then I should see the following message
		| Message                                                                |
		| There are some pending changes, are you sure you want to discard them? |

Scenario: Confirming the cancellation of adding availability process
	Given I have navigated to set availability page
	And I have started adding availability process
	And I have already selected following day of week
		| DayOfWeek |
		| Wed       |
	And I have added following time period
		| TimePeriod    |
		| 10:00 - 11:00 |
	When I cancel the adding availability process and I confirm that I want to discard the changes
	Then all added time periods should be cleared
	And all days of week should be deselected
	And view should be marked as non-stale
	And I should be able to start another adding availability process

Scenario: Saving the availability information
	Given I have navigated to set availability page
	And I have started adding availability process
	And I have already selected following day of week
		| DayOfWeek |
		| Wed       |
	And I have added following time period
		| TimePeriod    |
		| 10:00 - 11:00 |
	When I save the availability information
	Then Availability should be saved in database
	And The newly created availability should be added to the list of availabilities
	And all added time periods should be cleared
	And all days of week should be deselected
	And view should be marked as non-stale
	And I should be able to start another adding availability process
		
Scenario Outline: Trying to add time availability while entering invalid data
	Given I have navigated to set availability page
	And I have started adding availability process
	And I have entered invalid '<data>' into '<field>'
	When I try to save changes
	Then I should see '<validation_error>' detailing the '<data>' validation issue for '<field>'
	Examples: 
		| field       | data           | validation_error                                                          |
		| DaysOfWeek  | NoDateSelected | Please select  at least one day of week before adding new time available. |
		| TimePeriods | TimePeriods    | Please add at least one time period before adding new time available.     |
		#| TimePeriods | IntersectingTimePeriod | Selected time periods are intersecting with your current availability settings. Please make sure you add non-intersecting time periods and try again. |

#Scenario Outline: Settings stale properties
#	And I have navigated to set availability page
#	And the availability information is loaded
#		| DaysOfWeek    | TimePeriods                  |
#		| Mon, Wed      | 10:00 - 11:00, 13:00 - 16:00 |
#		| Tue, Thu, Sun | 09:00 - 18:00                |
#	And I have started adding availability process
#	When I modify '<field>'
#	Then When I try to dismiss the screen I should see a warning notifying if I would like to discard pending changes
#	Examples:
#		| field         |
#		| DaysOfWeek    |
#		| TimePeriods   |