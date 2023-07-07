Feature: ChargeUser

A short summary of the feature

@tag1
Scenario: 43_Create a new user and charge an amount 
	Given a new user is created
	When charge a ramdon  amount 
	Then the charge response status is <codeResponse>
	And the message charge response is "<messageResponse>"
	Examples:
		| codeResponse			| messageResponse		|
		|	InternalServerError	|	not active user		|

Scenario: 44_Create a ramdon Id and charge an amount the status response is InternalServerError
	Given a new ramdon Id
	When charge a ramdon  amount
	Then the charge response status is <codeResponse>
	And the message charge response is "<messageResponse>"
	Examples:
		| codeResponse			| messageResponse		|
		|	InternalServerError	|	not active user		|

	#Test T47
Scenario Outline: 47_Create new user and charge amount over ten million - Status code is InternalServerError
	Given a new user is created
	And change to active user status to <boolstate> is made
	When an <amount> is charged to the user 
	Then the charge response status is <codeResponse>
	And the message charge response is "<messageResponse>"
	Examples:
			| boolstate	   | amount			 | codeResponse				|	messageResponse																			|
			| true		   | 10000000.01     |		InternalServerError	|	After this charge balance could be '10000000.01', maximum user balance is '10000000'	|

			#Test T48-46
Scenario Outline: 48_Create new user then active the user and charge amount - Status code is InternalServerError
	Given a new user is created
	And change to active user status to <boolstate> is made
	When an <amount> is charged to the user 
	Then the charge response status is <codeResponse>
	And the message charge response is "<messageResponse>"

	Examples:
			| boolstate	   | amount			 | codeResponse				| messageResponse											|
			| true		   | 0.001			 | InternalServerError		|	Amount value must have precision 2 numbers after dot	|
			| true		   | 0				 | InternalServerError		|	Amount cannot be '0'									|
			| true		   | -0.01			 | InternalServerError		|	User have '0', you try to charge '-0.01'.				|

Scenario Outline: 56_Create new user then active the user and the balance is n+10, charge n (n>0) - Status code is OK
	Given a new user is created
	And change to active user status to <boolstate> is made
	And an charge of <amount> plus <value> is charged 
	When  an <amount> is charged to the user 
	Then the charge response status is <codeResponse>

	Examples:
			| boolstate	   | amount		| value				 | codeResponse	    |
			| true		   | 20			| 10				 | OK				|
			| true		   | 60			| 10				 | OK				|

Scenario Outline: 45_Create new user then active the user and thebalance = n, charge -n- 0.01 (n>0) - Status code is OK
	Given a new user is created
	And change to active user status to <boolstate> is made
	And  charge <amount> minus <value> to the user 
	When  an <amount> is charged to the user 
	Then the charge response status is <codeResponse>

	Examples:
			| boolstate	   | amount		| value				 | codeResponse	    |
			| true		   | 20			| 0.01				 | OK				|
			| true		   | 100		| 0.01				 | OK				|

Scenario Outline: 49_55_Create new user and charge a valid amount - Status code is OK
	Given a new user is created
	And change to active user status to <boolstate> is made
	When an <amount> is charged to the user 
	Then the charge response status is <codeResponse>
	Examples:	
			| boolstate	   | amount			 | codeResponse				|
			| true		   | 0.01			 |		OK					|
			| true		   | -30			 |		InternalServerError	|

Scenario Outline: 54_53_Create new user and charge -n and Status code is OK
	Given a new user is created
	And change to active user status to <boolstate> is made
	When an minus <amount> is charged
	Then the charge response status is <codeResponse>
	Examples:	
			| boolstate	   | amount			 | codeResponse				|
			| true		   | -20			 |		OK					|
			| true		   | 20				 |		InternalServerError	|