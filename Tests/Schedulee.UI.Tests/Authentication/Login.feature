Feature: Login

Scenario Outline: Trying to log in while entering invalid email
	Given I have navigated to login page
	And I have entered invalid email '<email>'
	When I try to log in
	Then I should see '<validation_error>' detailing the email validation issue
	Examples: 
		| email  | validation_error |
		| Empty  | FieldRequired    |
		| john@ | InvalidEmail     |

Scenario Outline: Trying to log in while entering invalid password
	Given I have navigated to login page
	And I have entered invalid password '<password>'
	When I try to log in
	Then I should see '<validation_error>' detailing the password validation issue
	Examples: 
		| password | validation_error |
		| Empty    | FieldRequired    |

Scenario Outline: Login stale properties
	Given I have navigated to login page
	When I modify '<field>'
	Then When I try to dismiss the screen I should see a warning notifying if I would like to discard pending changes
	Examples:
		| field    |
		| Email    |
		| Password |

Scenario: Login completes successfully 
	Given I have navigated to login page
	And I have entered valid email and password
	When I try to log in and the process completes successfully
	Then the application should be notified about successful login