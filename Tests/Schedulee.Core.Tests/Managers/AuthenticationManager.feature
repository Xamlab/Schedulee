Feature: Authentication Manager

Scenario: Sign in process completes successfully
	When sign in process completes successfully
	Then The user account should be stored in the secure storage
	And session state should be changed to LoggedIn
	And application should be notified that session state was changed to LoggedIn

Scenario: Sign in process completes and no account is returned
	When Sign in process completes and no account is returned
	Then session state should be changed to LoggedOut

Scenario: Sign in process fails due to exception
	When Sign in process fails due to exception
	Then Exception should be re-thrown

Scenario: Signing out
	Given I am signed in
	When I try to sign out
	Then account information should be cleared
	Then session state should be changed to LoggedOut
	And application should be notified that session state was changed to LoggedOut

Scenario: Restoring session when there is stored session
	Given There is stored session
	When I try to restore the session
	Then session state should be changed to LoggedIn
	And application should be notified that session state was changed to LoggedIn

Scenario: Restoring session when there is no stored session
	Given There is no stored session
	When I try to restore the session
	Then session state should be changed to LoggedOut