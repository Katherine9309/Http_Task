Feature: User Service test

A short summary of the feature


Scenario: Set a new user with fields empty
	Given a empty first and last name
	When sending a request to create a new user
	Then the user response status code is OK

Scenario: Set a new user with Null fields
	Given a null firts and last name
	When sending a request to create a new user
	Then the user response status code is OK

Scenario: Create a user with digits and Upper case on fields
	Given a digits and upper case on first name and last name 
	When sending a request to create a new user
	Then the user response status code is OK

Scenario: Create a user with Hundred Length On Fields 
	Given a first name and last name with hundred length on fields 
	When sending a request to create a new user
	Then the user response status code is OK

Scenario: Create a user and check the Id is Autoincremented 
	Given a new user is created
	When a second user is created
	Then the Id of the second user is greater

