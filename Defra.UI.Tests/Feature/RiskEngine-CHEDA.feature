@RiskEngine
Feature: Risk Engine CHEDA

Bulk upload, Update and Test rules with end-to-end validation for a CHEDA notification

Scenario: Bulk upload initial load for CHEDA - SPS-9427
	# ---------- Iteration 1: Start ----------
	# Upload the rules
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the CHED-A rules report page
	Then the count of CHED-A rules is recorded as 'InitialRuleCount'
	When the user clicks the 'CHED-A' link from the Risk Engine header menu
	Then the CHED-A imports page should be displayed
	When the user clicks the Bulk upload commodity rules link on the CHED-A imports page
	Then the Bulk upload commodity rules for CHED-A page should be displayed
	When the user clicks the Choose file button on the CHED-A bulk upload page
	And the user navigates to and selects the CHED-A bulk upload file 'SPS-8800_CHED-A_BulkUploadRules.csv'
	Then the selected file name is displayed within the File added box
	When the user clicks the Continue button on the CHED-A bulk upload page
	Then the Commodity rules for CHED-A page should be displayed with the File submission in progress section showing status 'Ready to submit'
	When the user clicks the Confirm and submit link for the file submission in progress on the CHED-A page
	Then the Check and submit commodity rules for CHED-A page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 0     |
		| Add new rules          | 5     |
	And the Check and submit commodity rules for CHED-A page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the Confirm and submit rules button on the CHED-A page
	Then the File submission in progress section should be removed and the first Previous submission should have status 'Completed' on the CHED-A page
	When the user clicks the View summary link for the first record in the Previous submissions section on the CHED-A page
	Then the Commodity rules status for CHED-A page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 0     |
		| Add new rules          | 5     |
	And the Commodity rules status for CHED-A page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the View all CHED-A imports commodity rules link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the CHED-A rules report page
	Then the count of CHED-A rules should be 5 more than 'InitialRuleCount'
	When the user enters '*' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                                                                           |
		| Description         | ALL                                                                             |
		| Commodity code      | *                                                                               |
		| Rate %              | 100                                                                             |
		| Previous rate %     | 0                                                                               |
		| Permanent           | Yes                                                                             |
		| End date            |                                                                                 |
		| Countries           | Djibouti                                                                        |
		| Country groups      | None                                                                            |
		| Country exceptions  | None                                                                            |
		| Certified For       | Approved bodies                                                                 |
		| Purpose             | All                                                                             |
		| Border Control Post | Manchester Airport IC1 (animals) - GBMNC4, Stansted Airport (animals) - GBSTN4A |
		| Reason              | Reason 1                                                                        |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Djibouti / 04071100 / Approved bodies / GBMNC4)
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
	When the user randomly selects a purpose from 'Any'
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
	Then the Add the County Parish Holding number (CPH) page should be displayed
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_1' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_1'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_1_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 100           |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity *
	When the user updates the bulk update CSV 'SPS-8807_CHED-A_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '*' to the recorded 'Iteration_1_RuleId'
	Then the bulk update CSV row for commodity code '*' should contain the recorded 'Iteration_1_RuleId'
	And 'Iteration_1' is complete
	# ---------- Iteration 1: Complete ----------
	# ---------- Iteration 2: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '03074290' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                |
		| Description         | Other                |
		| Commodity code      | 03074290             |
		| Rate %              | 40                   |
		| Previous rate %     | 0                    |
		| Permanent           | Yes                  |
		| End date            |                      |
		| Countries           | None                 |
		| Country groups      | European Countries   |
		| Country exceptions  | None                 |
		| Certified For       | All                  |
		| Purpose             | All internal markets |
		| Border Control Post | All                  |
		| Reason              |                      |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Monaco / 03074290 / Internal market)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user randomly selects a purpose from 'Internal market'
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
	When the user randomly selects a certification from 'Any'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_2' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_2'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 40            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	When the user updates the bulk update CSV 'SPS-8807_CHED-A_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '03074290' to the recorded 'Iteration_2_RuleId'
	Then the bulk update CSV row for commodity code '03074290' should contain the recorded 'Iteration_2_RuleId'
	And 'Iteration_2' is complete
	# ---------- Iteration 2: Complete ----------
	# ---------- Iteration 3: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '0101' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                                       |
		| Description         | Live horses, asses, mules and hinnies       |
		| Commodity code      | 0101                                        |
		| Rate %              | 0                                           |
		| Previous rate %     | 0                                           |
		| Permanent           | Yes                                         |
		| End date            |                                             |
		| Countries           | None                                        |
		| Country groups      | Third Countries                             |
		| Country exceptions  | Montserrat                                  |
		| Certified For       | Pets, Slaughter, Fattening, Registered      |
		| Purpose             | Breeding, Re-entry, Transit, Slaughter      |
		| Border Control Post | Heathrow Airport - HARC (animals) - GBLHR4A |
		| Reason              | Reason 3                                    |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Australia / 0101 / Slaughter / GBLHR4A)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user randomly selects a purpose from 'Internal market: Breeding, Slaughter | Transit | Re-entry'
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
	When the user randomly selects a certification from 'Fattening, Registered, Pets, Slaughter'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_3' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_3'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_3_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 0             |
		| Total          | 1             |
		| Triggered      | 0             |
		| IsTriggered    | false         |
	When the user updates the bulk update CSV 'SPS-8807_CHED-A_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '0101' to the recorded 'Iteration_3_RuleId'
	Then the bulk update CSV row for commodity code '0101' should contain the recorded 'Iteration_3_RuleId'
	And 'Iteration_3' is complete
	# ---------- Iteration 3: Complete ----------
	# ---------- Iteration 4: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '05119190' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                             |
		| Description         | Other                             |
		| Commodity code      | 05119190                          |
		| Rate %              | 99                                |
		| Previous rate %     | 0                                 |
		| Permanent           | Yes                               |
		| End date            |                                   |
		| Countries           | Djibouti, Germany, Togo           |
		| Country groups      | None                              |
		| Country exceptions  | None                              |
		| Certified For       | Approved bodies, Slaughter, Other |
		| Purpose             | All                               |
		| Border Control Post | All                               |
		| Reason              |                                   |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Togo / 05119190 / Other)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user randomly selects a purpose from 'Any'
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
	When the user randomly selects a certification from 'Approved bodies, Slaughter, Other'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_4' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_4'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_4_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 99            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	When the user updates the bulk update CSV 'SPS-8807_CHED-A_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '05119190' to the recorded 'Iteration_4_RuleId'
	Then the bulk update CSV row for commodity code '05119190' should contain the recorded 'Iteration_4_RuleId'
	And 'Iteration_4' is complete
	# ---------- Iteration 4: Complete ----------
	# ---------- Iteration 5: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '950810' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                                               |
		| Description         | Travelling circuses and travelling menageries       |
		| Commodity code      | 950810                                              |
		| Rate %              | 10                                                  |
		| Previous rate %     | 0                                                   |
		| Permanent           | No                                                  |
		| End date            | 01/01/2030                                          |
		| Countries           | ALL                                                 |
		| Country groups      | None                                                |
		| Country exceptions  | None                                                |
		| Certified For       | Breeding or production, Circus or exhibition        |
		| Purpose             | All internal markets, Transhipment or onward travel |
		| Border Control Post | All                                                 |
		| Reason              |                                                     |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Zimbabwe / 950810 / Internal market / Circus)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user randomly selects a purpose from 'Internal market | Transhipment or onward travel'
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
	When the user randomly selects a certification from 'Breeding and/or production, Circus/exhibition'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_5' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_5'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_5_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 10            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	When the user updates the bulk update CSV 'SPS-8807_CHED-A_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '950810' to the recorded 'Iteration_5_RuleId'
	Then the bulk update CSV row for commodity code '950810' should contain the recorded 'Iteration_5_RuleId'
	And the user copies the updated bulk update CSV back to the source data directory
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------

Scenario: Bulk upload update existing rules for CHEDA - SPS-9428
	# ---------- Iteration 1: Start ----------
	# Upload the update rules
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the CHED-A rules report page
	Then the count of CHED-A rules is recorded as 'InitialRuleCount'
	When the user clicks the 'CHED-A' link from the Risk Engine header menu
	Then the CHED-A imports page should be displayed
	When the user clicks the Bulk upload commodity rules link on the CHED-A imports page
	Then the Bulk upload commodity rules for CHED-A page should be displayed
	When the user clicks the Choose file button on the CHED-A bulk upload page
	And the user navigates to and selects the CHED-A bulk upload file 'SPS-8807_CHED-A_BulkUploadRules_UpdatedRules.csv'
	Then the selected file name is displayed within the File added box
	When the user clicks the Continue button on the CHED-A bulk upload page
	Then the Commodity rules for CHED-A page should be displayed with the File submission in progress section showing status 'Ready to submit'
	When the user clicks the Confirm and submit link for the file submission in progress on the CHED-A page
	Then the There are rules in your CSV file that already exist page should be displayed for CHED-A
	When the user selects Yes to replace existing rules and clicks Continue for CHED-A
	Then the Check and submit commodity rules for CHED-A page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 5     |
		| Add new rules          | 0     |
	And the Check and submit commodity rules for CHED-A page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the Confirm and submit rules button on the CHED-A page
	Then the File submission in progress section should be removed and the first Previous submission should have status 'Completed' on the CHED-A page
	When the user clicks the View summary link for the first record in the Previous submissions section on the CHED-A page
	Then the Commodity rules status for CHED-A page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 5     |
		| Add new rules          | 0     |
	And the Commodity rules status for CHED-A page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the View all CHED-A imports commodity rules link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the CHED-A rules report page
	Then the count of CHED-A rules should equal the recorded 'InitialRuleCount'
	When the user enters '*' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                                                                           |
		| Description         | ALL                                                                             |
		| Commodity code      | *                                                                               |
		| Rate %              | 70                                                                              |
		| Previous rate %     | 100                                                                             |
		| Permanent           | Yes                                                                             |
		| End date            |                                                                                 |
		| Countries           | Djibouti                                                                        |
		| Country groups      | None                                                                            |
		| Country exceptions  | None                                                                            |
		| Certified For       | Approved bodies                                                                 |
		| Purpose             | All                                                                             |
		| Border Control Post | Manchester Airport IC1 (animals) - GBMNC4, Stansted Airport (animals) - GBSTN4A |
		| Reason              | Reason 1b                                                                       |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Djibouti / 04071100 / Approved bodies / GBMNC4)
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
	When the user randomly selects a purpose from 'Any'
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
	Then the Add the County Parish Holding number (CPH) page should be displayed
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_1' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_1'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_1_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 70            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_1' is complete
	# ---------- Iteration 1: Complete ----------
	# ---------- Iteration 2: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '03074290' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                      |
		| Description         | Other                      |
		| Commodity code      | 03074290                   |
		| Rate %              | 100                        |
		| Previous rate %     | 40                         |
		| Permanent           | Yes                        |
		| End date            |                            |
		| Countries           | Djibouti, Germany, Italy   |
		| Country groups      | European Countries         |
		| Country exceptions  | None                       |
		| Certified For       | All                        |
		| Purpose             | All internal markets       |
		| Border Control Post | All                        |
		| Reason              | Reason 2                   |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Djibouti / 03074290 / Internal market)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user searches '03074290' commodity code
	Then the commodity details should be populated '03074290' 'Other'
	When the user selects species of commodity 'Afrololigo spp.'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user randomly selects a purpose from 'Internal market'
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
	When the user randomly selects a certification from 'Any'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_2' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_2'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 100           |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_2' is complete
	# ---------- Iteration 2: Complete ----------
	# ---------- Iteration 3: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '0101' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                                                                                                                                                            |
		| Description         | Live horses, asses, mules and hinnies                                                                                                                            |
		| Commodity code      | 0101                                                                                                                                                             |
		| Rate %              | 0                                                                                                                                                                |
		| Previous rate %     | 0                                                                                                                                                                |
		| Permanent           | Yes                                                                                                                                                              |
		| End date            |                                                                                                                                                                  |
		| Countries           | None                                                                                                                                                             |
		| Country groups      | Third Countries                                                                                                                                                  |
		| Country exceptions  | Montserrat                                                                                                                                                       |
		| Certified For       | Pets, Fattening, Registered                                                                                                                                      |
		| Purpose             | Breeding, Re-entry, Transit, Slaughter                                                                                                                           |
		| Border Control Post | Heathrow Airport - Airpets Limited (animals) - GBLHR022, Heathrow Airport - Animal Aircare Ltd (animals) - GBLHR067, Heathrow Airport - HARC (animals) - GBLHR4A |
		| Reason              |                                                                                                                                                                  |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Australia / 0101 / Slaughter / GBLHR4A)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user randomly selects a purpose from 'Internal market: Breeding, Slaughter | Transit | Re-entry'
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
	When the user randomly selects a certification from 'Fattening, Registered, Pets'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_3' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_3'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_3_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 0             |
		| Total          | 1             |
		| Triggered      | 0             |
		| IsTriggered    | false         |
	And 'Iteration_3' is complete
	# ---------- Iteration 3: Complete ----------
	# ---------- Iteration 4: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '05119190' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                      |
		| Description         | Other                      |
		| Commodity code      | 05119190                   |
		| Rate %              | 50                         |
		| Previous rate %     | 99                         |
		| Permanent           | Yes                        |
		| End date            |                            |
		| Countries           | Djibouti, Germany, Togo    |
		| Country groups      | None                       |
		| Country exceptions  | None                       |
		| Certified For       | All                        |
		| Purpose             | All                        |
		| Border Control Post | All                        |
		| Reason              |                            |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Togo / 05119190 / Any)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user randomly selects a purpose from 'Any'
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
	When the user randomly selects a certification from 'Any'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_4' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_4'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_4_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 50            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_4' is complete
	# ---------- Iteration 4: Complete ----------
	# ---------- Iteration 5: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user enters '950810' in the CHED-A rules search field
	Then the top CHED-A rule row should match the following details
		| Field               | Value                                               |
		| Description         | Travelling circuses and travelling menageries       |
		| Commodity code      | 950810                                              |
		| Rate %              | 100                                                 |
		| Previous rate %     | 10                                                  |
		| Permanent           | No                                                  |
		| End date            | 01/01/2030                                          |
		| Countries           | ALL                                                 |
		| Country groups      | None                                                |
		| Country exceptions  | None                                                |
		| Certified For       | Breeding or production, Circus or exhibition        |
		| Purpose             | All internal markets, Transhipment or onward travel |
		| Border Control Post | All                                                 |
		| Reason              |                                                     |
	And the top CHED-A rule row should have Start date as today's date
	And the user records the Id of the top CHED-A rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-A notification in IPAFFS (Zimbabwe / 950810 / Internal market / Circus)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
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
	When the user randomly selects a purpose from 'Internal market | Transhipment or onward travel'
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
	When the user randomly selects a certification from 'Breeding and/or production, Circus/exhibition'
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
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the CHED-A Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_5' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_5'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_5_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 100           |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------
	# Delete the 5 rules created by the update bulk upload
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-A reports link
	Then the CHED-A reports page should be displayed
	When the user clicks the Imports commodity rules report link
	Then the View all CHED-A (Import) Commodity Rules report page should be displayed
	When the user clicks the Remove rule link for CHED-A rule Id recorded as 'Iteration_1_RuleId'
	Then the CHED-A rule Id recorded as 'Iteration_1_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-A rule Id recorded as 'Iteration_2_RuleId'
	Then the CHED-A rule Id recorded as 'Iteration_2_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-A rule Id recorded as 'Iteration_3_RuleId'
	Then the CHED-A rule Id recorded as 'Iteration_3_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-A rule Id recorded as 'Iteration_4_RuleId'
	Then the CHED-A rule Id recorded as 'Iteration_4_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-A rule Id recorded as 'Iteration_5_RuleId'
	Then the CHED-A rule Id recorded as 'Iteration_5_RuleId' should no longer be present in the rules table
	When the user scrolls to the bottom of the CHED-A rules report page
	Then the count of CHED-A rules should be 5 less than 'InitialRuleCount'