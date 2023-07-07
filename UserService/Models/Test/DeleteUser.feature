Feature: DeleteUser

A short summary of the feature

@tag1
Scenario: Delete a not existing user and the status response is not found
	Given  a new user is created
	And the user is deleted
	When a new request to delete the user is made
	Then the delete user response status code is 'InternalServerError'
	And the message response was 'cannot find user with this id'

Scenario: Delete a not active user 
	Given a new user is created
	When the user is deleted
	Then the delete user response status code is 'OK'
	And the message response was ''


