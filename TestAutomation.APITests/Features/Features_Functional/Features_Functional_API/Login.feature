Feature: Bookstore login
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@api @functional
Scenario: Bookstore application authorization
	Given I provide valid credentials
	Then I am authorized to access bookstore application

@api @functional
Scenario: Bookstore application token generation
	Given I provide valid credentials
	Then I am provided with valid token
	
@api @functional
Scenario: Login to bookstore application
	Given I provide valid credentials
	Then I am logged in to bookstore application
	And I am provided with Username, UserId and token

@api @functional
Scenario: Add new user
	Given I add new user details
	Then I am provided with user details

@api @functional
Scenario: Get user details
	Given I add new user
	Then I am provided with user details
	When I request for user details
	Then I am provided with requested user details

@api @functional
Scenario: Delete user
	Given I add new user
	Then I am provided with user details
	When I delete added user
	Then User details should not be available



