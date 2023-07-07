Feature: GetBalance

A short summary of the feature

		# Test T1 , T2
Scenario Outline: Create a new user there status is not active and get the balance the result code is internal server error

	Given a new user is created
	When a request to get the balance is made
	Then the balance response status is <codeResponse>
	Examples:
		|	codeResponse				|
		|	InternalServerError			|
		# Test T3
Scenario: Get balance from not existing user the result code is internal server error
	Given a new ramdon Id
	When a request to get the balance is made
	Then the balance response status is <codeResponse>
	Examples:
		|	 codeResponse				|
		|	InternalServerError			|

		#Test T15
Scenario Outline: Create a user and change to active user and get the balance the result code is ok
	Given a new user is created
	And change to active user status to <boolstate> is made
	When a request to get the balance is made
	Then the balance response status is <codeResponse>
	And the balance is <overalBalance>
	Examples:
		| boolstate	  | codeResponse	| 	overalBalance	|
		| true		  |	OK				| 		0			|

# Test T10, T11, T12,  T13,T14,
Scenario Outline:10 Create a new user and get balance after some charge transactions
	Given a new user is created
	And change to active user status to <boolstate> is made
	When an <amount> is charged to the user 
	And a request to get the balance is made
	Then the balance response status is <codeResponse>
	And the balance is <overalBalance>
	
	Examples:
		| boolstate	   | amount		  |	overalBalance		| codeResponse	|
		| true		   | 0.01		  |		0.01			|		OK		|
		| true		   | 9999999.99	  |		9999999.99		|		OK		|
		| true		   | 10000000	  |		10000000		|		OK		|
		| true		   | -0.01		  |		0				|		OK		|
		| true		   | -10000000.01 |		0				|		OK		|

#Test T4, T5, T6, T7, T8
Scenario Outline: T4_Create a user and set active then make some charge transactions and then get the balance 
	Given  a new user is created
	And change to active user status to <boolstate> is made
	When I made charge transactions for the following <amounts>
	And a request to get the balance is made
	Then the balance response status is <codeResponse>
	And the balance is <overalBalance>
	Examples:
		| boolstate	   | amounts							  |	overalBalance		| codeResponse	|
		| true		   | 10, 20, 30, -10, -20, -30			  |		0				|		OK		|
		| true		   | 10, 20, 30, -20, -30, -9.99		  |		 0.01			|		OK		|
		| true		   | 10, 20, 30, -20, -30, -10.01		  |		10				|		OK		|
		| true		   | 5000000, 4000000,1000000, -0.01	  |		 9999999.99		|		OK		|
		| true		   | 5000000, 4000000, 500000, 500000	  |		10000000		|		OK		|

#Test T16
Scenario Outline: 16_Create a user and set active then make a reverse transaction after this get the balance - status repsonse OK
	Given  a new user is created
	And change to active user status to <boolstate> is made
	And  an <amount> is charged to the user 
	When a request to revert the transaction is made
	And a request to get the balance is made
	Then the balance response status is <codeResponse>
	And the balance is <overalBalance> 

	Examples:
		| boolstate	   | amount  |	overalBalance		| codeResponse	|
		| true		   | 10		  |		0				|		OK		|

