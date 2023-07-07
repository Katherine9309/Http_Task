Feature: RevertTransaction

A short summary of the feature

@tag1
Scenario Outline: 33 Revert transaction from a wrong Id transaction
	Given a new ramdon Id transaction 
	When a request to revert the transaction is made
	Then  the reverse transaction response status is <codeResponse>
		Examples: 
			| codeResponse			|
			|		NotFound		|

Scenario Outline: 38 Create a new user and active the user then charge and amount and reverse the charge transaction and then the revert transaction
	Given a new user is created
	And a request to change the user status to <boolstate> is made
	And charge a ramdon amount 
	When a request to revert the transaction is made
	And a request to revert the last revert transaction
	Then the reverse transaction response status is <codeResponse>
	And the second reverse transaction response status is <secondCodeResponse>
	Examples:
		| boolstate	   | secondCodeResponse	    | codeResponse		|
		| true		   | OK						|		OK			|

		#Test T37
Scenario Outline: 37Create a new user and active it and then charge amount over ten million - status code is InternalError
	Given a new user is created
	And change to active user status to <boolstate> is made
	When an <amount> is charged to the user 
	And  a request to revert the transaction is made
	Then the reverse transaction response status is <codeResponse>
		Examples:
			| boolstate	   | amount			 | codeResponse		|
			| true		   | 10000000.01     |		NotFound	|


Scenario Outline:35-34-39-36 Create a new user and active the user then charge a valid amount and reverse the charge transaction and then the revert transaction
	Given a new user is created
	And a request to change the user status to <boolstate> is made
	And an <amount> is charged to the user 
	When a request to revert the transaction is made
	Then the reverse transaction response status is <codeResponse>
	Examples:
		| boolstate	    | codeResponse		| amount		|
		| true			|		OK			| 0.01			|
		| true			|		OK			| 9999999.99    |
		| true			|		OK			| 10000000      |
		| true			|		NotFound	| -0.01		    |