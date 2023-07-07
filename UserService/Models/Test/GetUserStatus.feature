Feature: GetUserStatus

A short summary of the feature

@tag1
Scenario: Get status from not existing user
	Given a new ramdon Id
	When a request is made it to check user status
	Then the check status response is 'NotFound'

Scenario: Create a user and and check the status
	Given a new user is created
	When a request is made it to check user status
	Then the user status is 'False'

Scenario: Create user and then check changed true status
	Given a new user is created
	When a request to change the user status to 'true' is made 
	Then the user status is 'True'

Scenario: Set status for not existing user the status response is not found
	Given a new ramdon Id
	When a request to change the user status to 'true' is made 
	Then the set user status request is 'NotFound'

Scenario: Create an user and change to active status and then to inactive 
	Given a new user is created
	And a request to change the user status to 'true' is made 
	When a request to change the user status to 'false' is made 
	Then the user status is 'False'

Scenario: Create a user and change the status to active then change to inactive and then to active the status result is active
	Given a new user is created
	And a request to change the user status to 'true' is made 
	And a request to change the user status to 'false' is made 
	When a request to change the user status to 'true' is made 
	Then the user status is 'True'

Scenario: Create a new user and change the status to inactive the status is inactive
	Given a new user is created
	When a request to change the user status to 'false' is made 
	Then the user status is 'False'

Scenario: Create a new user and chenge the status to active twice
	Given a new user is created
	And a request to change the user status to 'true' is made 
	When a request to change the user status to 'rue' is made 
	Then the user status is 'True'
 
