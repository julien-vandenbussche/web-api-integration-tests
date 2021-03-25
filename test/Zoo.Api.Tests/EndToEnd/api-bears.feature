Feature: api-bears
  allows to recover the bears of the zoo, it is possible to recover them all, to filter by id.
  the API allows the creation of animals in our referential.

  Background:
    Given The current context is Blue
    Given the referential have any animals
      | Id | Name      | Legs | FamilyId |
      | 1  | Winnie    | 4    | 3        |
      | 2  | Léo       | 4    | 2        |
      | 3  | Sophie    | 4    | 1        |
      | 4  | Martin    | 4    | 3        |
    Given the referential have any families
      | Id | Name    | ClassificationId|
      | 2  | Lion    |1                |
      | 3  | Bear    |1                |
      | 1  | Giraffe |1                |
    Given the referential have any foods
      | Id | Name     |
      | 1  | Honey    |
    Given the referential have any classification
      | Id | Name     |
      | 1  | Mammifère|
    
    
  @retrained-animals @ok
  Scenario: return all retrained bears when call api/bears and referential have any bears    
    When i call the http resource 'api/v1/bears' with GET http method
    Then the http status code of response is 200
    And the content have restrained bears
      | Id | Name      |
      | 1  | Winnie    |
      | 4  | Martin    |

  @create-animal @ok
  Scenario: return created value when call api/bears with valid creating bear
    Given I'm zoo-director
    And the animal can eats
      | Title               | FamilyId | FoodId |
      | Bears can eat honey | 3        | 1      |
    When i would like register bear
      | Title             | Name    | Legs | Foods |
      | Barnabé eat honey | Barnabé | 4    | 1     |
    And i call the http resource 'api/v1/bears' with POST http method    
    Then the http status code of response is 201
    And the content is
      | Title             | location       | Id | Name    | Legs | Foods |
      | Barnabé eat honey | api/v1/bears/5 | 5  | Barnabé | 4    | Honey |
