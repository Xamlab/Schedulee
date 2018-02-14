Feature: Item Collection Screen
	Item collection screen allows the users to view a list of objects

Scenario: Items are loaded
	Given I have navigated to an Item Collection Screen for the first time
	When the loading operation completes
	Then I should see a list of objects
