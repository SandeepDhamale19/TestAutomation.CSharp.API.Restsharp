Feature: Bookstore
	In order to access bookstore app
	As an registered user
	I want to be add/ edit/ remove books

@api @functional
Scenario Outline: Access books from bookstore app
	Given I access books from bookstore with <url>, <verb> and <data>
	Then I am provided with information about books
Examples: 
| url                 | verb | data |
| /BookStore/v1/Books | get  |      |


@api @functional
Scenario: Add List of books
	Given I add isbn for a book
	Then book is added to my bookstore