@RiskEngine
Feature: Risk Engine CHEDA

Bulk upload, Update and Test rules with end-to-end validation for a CHEDA notification

Scenario: Bulk upload initial load for CHEDA - SPS-9427 - Iteration 1
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Live animals' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses 'Djibouti' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Djibouti' as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '04071100' commodity code
	Then the commodity details should be populated '04071100' 'Of fowls of the species Gallus domesticus'
	When the user selects species of commodity 'Gallus gallus'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Internal market' and the sub-option 'Approved premises or body'
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '3'
	And the user populates Number of packages as '3'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates the Egg mark as 'EGG1234'
	And the user populates the Collection date as 7 days ago
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Approved bodies' for What are the animals certified for?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter should be displayed
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee 'DEF'
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee
	When the user clicks Save and continue
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user enters BCP or Port of entry 'Manchester Airport (animals) - GBMNC4'
	And the user selects means of transport to BCP or Port of entry 'Airplane'
	And the user enters transport identification 'BA1234'
	And the user selects 'No' for Are any road trailers or shipping containers being used to transport the consignment
	And the user enters transport document reference 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	And the user enters estimated arrival time at BCP with future time
	And the user enters estimated total journey time of the animals '8' hours
	And the user clicks Save and continue
	Then the Transport after the BCP or Port of entry page should be displayed
	When the user selects means of transport after BCP 'Road vehicle'
	And the user enters transport identification after BCP 'ER58 AUT'
	And the user enters transport document reference after BCP 'Doc5678'
	And the user enters departure date from BCP '2' days later than arrival date
	And the user enters departure time from BCP or Port of entry with future time
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Should we notify any transport contacts about inspections? page should be displayed
	When the user selects 'No' for Should we notify any transport contacts about inspections?
	And the user clicks Save and continue
	Then the Contact address for consignment page should be displayed without the secondary title
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_1'
	And 'Iteration_1' is complete

Scenario: Bulk upload initial load for CHEDA - SPS-9427 - Iteration 2
Used Research instead of Technical Use, but the rule is still triggered
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Live animals' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses 'Monaco' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Monaco' as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '03074290' commodity code
	Then the commodity details should be populated '03074290' 'Other'
	When the user selects species of commodity 'Afrololigo spp.'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Internal market' and the sub-option 'Research'
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '3'
	And the user populates Number of packages as '3'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates Idenitification details as '1234'
	And the user populates the Description as 'Other'
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Other' for What are the animals certified for?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter should be displayed
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee 'DEF'
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee
	When the user clicks Save and continue
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user enters BCP or Port of entry 'Manchester Airport (animals) - GBMNC4'
	And the user selects means of transport to BCP or Port of entry 'Airplane'
	And the user enters transport identification 'BA1234'
	And the user selects 'No' for Are any road trailers or shipping containers being used to transport the consignment
	And the user enters transport document reference 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	And the user enters estimated arrival time at BCP with future time
	And the user enters estimated total journey time of the animals '8' hours
	And the user clicks Save and continue
	Then the Transport after the BCP or Port of entry page should be displayed
	When the user selects means of transport after BCP 'Road vehicle'
	And the user enters transport identification after BCP 'ER58 AUT'
	And the user enters transport document reference after BCP 'Doc5678'
	And the user enters departure date from BCP '2' days later than arrival date
	And the user enters departure time from BCP or Port of entry with future time
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Should we notify any transport contacts about inspections? page should be displayed
	When the user selects 'No' for Should we notify any transport contacts about inspections?
	And the user clicks Save and continue
	Then the Contact address for consignment page should be displayed without the secondary title
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_2'
	And 'Iteration_2' is complete

Scenario: Bulk upload initial load for CHEDA - SPS-9427 - Iteration 3
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Live animals' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses 'Australia' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Australia' as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0101' commodity code
	Then the commodity details should be populated '0101' 'Live horses, asses, mules and hinnies'
	When the user selects species of commodity 'Equus asinus'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Internal market' and the sub-option 'Slaughter'
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '3'
	And the user populates Number of packages as '3'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates the Microchip number as '1234'
	And the user populates the Passport number as '5678'
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Slaughter' for What are the animals certified for?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter should be displayed
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee 'DEF'
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee
	When the user clicks Save and continue
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user enters BCP or Port of entry 'Heathrow Airport - HARC (animals) - GBLHR4A'
	And the user selects means of transport to BCP or Port of entry 'Airplane'
	And the user enters transport identification 'BA1234'
	And the user selects 'No' for Are any road trailers or shipping containers being used to transport the consignment
	And the user enters transport document reference 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	And the user enters estimated arrival time at BCP with future time
	And the user enters estimated total journey time of the animals '8' hours
	And the user clicks Save and continue
	Then the Transport after the BCP or Port of entry page should be displayed
	When the user selects means of transport after BCP 'Road vehicle'
	And the user enters transport identification after BCP 'ER58 AUT'
	And the user enters transport document reference after BCP 'Doc5678'
	And the user enters departure date from BCP '2' days later than arrival date
	And the user enters departure time from BCP or Port of entry with future time
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Should we notify any transport contacts about inspections? page should be displayed
	When the user selects 'No' for Should we notify any transport contacts about inspections?
	And the user clicks Save and continue
	Then the Contact address for consignment page should be displayed without the secondary title
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_3'
	And 'Iteration_3' is complete

Scenario: Bulk upload initial load for CHEDA - SPS-9427 - Iteration 4
Certified for: Other, isn't available but the rule is still triggered

	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Live animals' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses 'Togo' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Togo' as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '05119190' commodity code
	Then the commodity details should be populated '05119190' 'Other'
	When the user selects species of commodity 'Acipenser spp.'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Transhipment or onward travel' and the sub-option ''
	And the user chooses destination country 'Togo'
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '3'
	And the user populates Number of packages as '3'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates Idenitification details as '1234'
	And the user populates the Description as 'Other'
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Approved bodies' for What are the animals certified for?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter should be displayed
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee 'DEF'
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee
	When the user clicks Save and continue
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user enters BCP or Port of entry 'Manchester Airport (animals) - GBMNC4'
	And the user selects means of transport to BCP or Port of entry 'Airplane'
	And the user enters transport identification 'BA1234'
	And the user selects 'No' for Are any road trailers or shipping containers being used to transport the consignment
	And the user enters transport document reference 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	And the user enters estimated arrival time at BCP with future time
	And the user enters estimated total journey time of the animals '8' hours
	And the user clicks Save and continue
	Then the Transport after the BCP or Port of entry page should be displayed
	When the user selects means of transport after BCP 'Road vehicle'
	And the user enters transport identification after BCP 'ER58 AUT'
	And the user enters transport document reference after BCP 'Doc5678'
	And the user enters departure date from BCP '2' days later than arrival date
	And the user enters departure time from BCP or Port of entry with future time
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Should we notify any transport contacts about inspections? page should be displayed
	When the user selects 'No' for Should we notify any transport contacts about inspections?
	And the user clicks Save and continue
	Then the Contact address for consignment page should be displayed without the secondary title
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_4'
	And 'Iteration_4' is complete

Scenario: Bulk upload initial load for CHEDA - SPS-9427 - Iteration 5
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Live animals' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses 'Zimbabwe' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Zimbabwe' as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '950810' commodity code
	Then the commodity details should be populated '950810' 'Travelling circuses and travelling menageries'
	When the user selects species of commodity 'Antilocapridae'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Internal market' and the sub-option 'Production'
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '3'
	And the user populates Number of packages as '3'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates Idenitification details as '1234'
	And the user populates the Description as 'Other'
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Circus/exhibition' for What are the animals certified for?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter should be displayed
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee 'DEF'
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee
	When the user clicks Save and continue
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user enters BCP or Port of entry 'Manchester Airport (animals) - GBMNC4'
	And the user selects means of transport to BCP or Port of entry 'Airplane'
	And the user enters transport identification 'BA1234'
	And the user selects 'No' for Are any road trailers or shipping containers being used to transport the consignment
	And the user enters transport document reference 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	And the user enters estimated arrival time at BCP with future time
	And the user enters estimated total journey time of the animals '8' hours
	And the user clicks Save and continue
	Then the Transport after the BCP or Port of entry page should be displayed
	When the user selects means of transport after BCP 'Road vehicle'
	And the user enters transport identification after BCP 'ER58 AUT'
	And the user enters transport document reference after BCP 'Doc5678'
	And the user enters departure date from BCP '2' days later than arrival date
	And the user enters departure time from BCP or Port of entry with future time
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Should we notify any transport contacts about inspections? page should be displayed
	When the user selects 'No' for Should we notify any transport contacts about inspections?
	And the user clicks Save and continue
	Then the Contact address for consignment page should be displayed without the secondary title
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_5'
	And 'Iteration_5' is complete