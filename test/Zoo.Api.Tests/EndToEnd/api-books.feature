Feature: api-books
  allows you to collect books by type of animal

  Background:
    Given the referential have any books
      | ID     | Title                                          | Author         |
      | X345D  | Little Brown Bear loves his daddy              | Marie Aubinais |
      | G465H  | The Giraffe                                    | Marie Nimier   |
      | D827G  | Nanuk: The Polar Bear Ledger                   | Michel Rawicki |
      | P0243  | The Giraffe Stroke: Scientists in the Savannah | Léo Grasset    |

  @books @ok
  Scenario: return all books of bear when call api/bears/books and referential have any bears
    When i call the http resource 'api/v1/bears/books' with GET http method
    Then the http status code of response is 200
    And the content have books of bear
      | Id     | Title                                      | Author         |
      | X345D  | Little Brown Bear loves his daddy          | Marie Aubinais |
      | D827G  | Nanuk: The Polar Bear Ledger               | Michel Rawicki |