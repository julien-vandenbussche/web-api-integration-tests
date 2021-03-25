Feature: api-infirmary
  Allows you to perform medical procedures, list veterinarians etc.
  
  @get-veterinaries @ok
    Scenario: Return all veterinary, contact information 
    Given the referential have any veterinaries
        | Name          | Address                | Postal Code | City  | Country | Phone      | Website                    | EMail                       |
        | Michel Klein  | 39 rue de chambor      | 75015       | Paris | France  | 0908070605 | michelklein.veterinaire.fr | michelklein@veterinaire.fr  |
        | John Dolittle | 57 chemin de la grotte | 39400       | Morez | France  | 0102030405 | dolittle.veterinaire.fr    | johndolittle@veterinaire.fr |
    When i call the http resource 'api/v1/infirmary/veterinaries' with GET http method
    Then the http status code of response is 200
    And the content have veterinary informations
      | Name          | Address                                         | Phone      | Website                    | EMail                       |
      | Michel Klein  | 39 rue de chambor\r\n75015 Paris\r\nFrance     | 0908070605 | michelklein.veterinaire.fr | michelklein@veterinaire.fr  |
      | John Dolittle | 57 chemin de la grotte\r\n39400 Morez\r\nFrance | 0102030405 | dolittle.veterinaire.fr    | johndolittle@veterinaire.fr |