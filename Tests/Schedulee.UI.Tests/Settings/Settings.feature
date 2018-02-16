Feature: Settings
	
Scenario: Initial loading of settings page
	Given I am logged in as
		| FirstName | LastName | Email               | ProfilePictureUrl                          | Location          | SetTravelTime |
		| John      | Smith    | john.smith@mail.com | http://some.url.com?image=jhon.smith.image | New York, 31, USA | 20            |
	And I have navigated to settings page
	Then I should see the user data populated correctly

Scenario Outline: Trying to save settings while entering invalid data
	Given I am logged in as
		| FirstName | LastName | Email               | ProfilePictureUrl                          | Location          | SetTravelTime |
		| John      | Smith    | john.smith@mail.com | http://some.url.com?image=jhon.smith.image | New York, 31, USA | 20            |

	Given I have navigated to settings page
	And I have entered invalid '<data>' into '<field>'
	When I try to save changes
	Then I should see '<validation_error>' detailing the '<data>' validation issue for '<field>'
	Examples: 
		| field         | data  | validation_error                          |
		| FirstName     | Empty | FIRST NAME is required.                   |
		| LastName      | Empty | LAST NAME is required.                    |
		| Location      | Empty | LOCATION is required.                     |
		| SetTravelTime | 0     | Set Travel Time should be greater than 0. |

Scenario Outline: Settings stale properties
	Given I am logged in as
		| FirstName | LastName | Email               | ProfilePictureUrl                          | Location          | SetTravelTime |
		| John      | Smith    | john.smith@mail.com | http://some.url.com?image=jhon.smith.image | New York, 31, USA | 20            |
	Given I have navigated to settings page
	When I modify '<field>' with '<value>'
	Then When I try to dismiss the screen I should see a warning notifying if I would like to discard pending changes
	Examples:
		| field         | value       |
		| FirstName     | Marie       |
		| LastName      | Johnson     |
		| Location      | Delaware 81 |
		| SetTravelTime | 80          |

Scenario: Saving completes successfully 
	Given I am logged in as
		| FirstName | LastName | Email               | ProfilePictureUrl                          | Location          | SetTravelTime |
		| John      | Smith    | john.smith@mail.com | http://some.url.com?image=jhon.smith.image | New York, 31, USA | 20            |
	Given I have navigated to settings page
	And I have updated my user information with this data
		| FirstName | LastName | Email                  | ProfilePictureUrl                             | Location          | SetTravelTime |
		| Marie     | Johnson  | marie.johnson@mail.com | http://some.url.com?image=marie.johnson.image | Delaware, 83, USA | 40            |
	When I save the data and saving completes successfully
	Then the current user information should be updated


