# Multithreading
          1. Wat is multithreading? Het tegelijkertijd(asynchroon) uitvoeren van verschillende taken door ze te verdelen over verschillende threads
          2. Wanneer gebruik je meerdere threads? Het is bijvoorbeeld handig om multithreading te gebruiken bij grote operaties. Deze kunnen van
               bijvoorbeeld op de achtergrond uitgevoerd worden zonder dat de mainthread verstoord wordt.
          3.  - Het delen van data tussen verschillende threads kan voor problemen zorgen.
              - Het afhandelen van foutmeldingen
              - Het gebruik van locks kan zorgen voor de verslechtering van de performance
          4.  - Objecten worden in de heap geplaatst. 
              - Bij multithreading wil je voorkomen dat verschillende threads gebruik maken van dezelfde resources.
          5.  Deze worden op de heap geplaatst
          6. Een racing condition is wanneer verschillende threads eenzelfde resource gebruiken en/of aanpassen. Het is dan moeilijk te bepalen
               of dat in de juiste volgorde gebeurt. Om dit te voorkomen zou je een lock kunnen gebruiken. Ook zou je ervoor kunnen zorgen dat de 
               verschillende threads niet hetzelfde object gebruiken.