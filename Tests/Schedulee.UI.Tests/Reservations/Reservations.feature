Feature: Reservations


Scenario: Loading of reservations grouped by the day of month
	Given I have navigated to reservations page
	When Reservations page is loaded
	Then I should see reservations grouped by the day of month
