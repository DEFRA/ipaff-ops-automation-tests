@RiskEngine @RiskEngine-CHEDD
Feature: Risk Engine CHEDD

Bulk upload, Update and Test rules with end-to-end validation for a CHEDD notification

@SPS-9442
Scenario: Bulk upload initial load for CHEDD - SPS-9442
	# ---------- Iteration 1: Start ----------
	# Upload the rules
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the View all CHED-D (Import) Commodity Rules report page
	Then the count of CHED-D import commodity rules is recorded as 'InitialRuleCount'
	When the user clicks the 'CHED-D' link from the Risk Engine header menu
	Then the CHED-D imports page should be displayed
	When the user clicks the Bulk upload commodity rules link on the CHED-D imports page
	Then the Bulk upload commodity rules for CHED-D page should be displayed
	When the user clicks the Choose file button on the CHED-D bulk upload page
	And the user navigates to and selects the CHED-D bulk upload file 'SPS-8804_CHED-D_BulkUploadRules.csv'
	Then the selected file name is displayed within the CHED-D File added box
	When the user clicks the Continue button on the CHED-D bulk upload page
	Then the Commodity rules for CHED-D page should be displayed with the File submission in progress section showing status 'Ready to submit'
	When the user clicks the Confirm and submit link for the file submission in progress on the CHED-D page
	Then the Check and submit commodity rules for CHED-D page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 0     |
		| Add new rules          | 5     |
	And the Check and submit commodity rules for CHED-D page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the Confirm and submit rules button on the CHED-D page
	Then the File submission in progress section should be removed and the first Previous submission should have status 'Completed' on the CHED-D page
	When the user clicks the View summary link for the first record in the Previous submissions section on the CHED-D page
	Then the Commodity rules status for CHED-D page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 0     |
		| Add new rules          | 5     |
	And the Commodity rules status for CHED-D page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the View all CHED-D imports commodity rules link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the View all CHED-D (Import) Commodity Rules report page
	Then the count of CHED-D import commodity rules should be 5 more than 'InitialRuleCount'
	When the user enters '0910' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                                                                          |
		| Description         | Ginger, saffron, turmeric (curcuma), thyme, bay leaves, curry and other spices |
		| Commodity code      | 0910                                                                           |
		| Rate %              | 50                                                                             |
		| Previous rate %     | 0                                                                              |
		| Permanent           | Yes                                                                            |
		| End date            |                                                                                |
		| Countries           | ALL                                                                            |
		| Country groups      | None                                                                           |
		| Country exceptions  | Djibouti                                                                       |
		| Purpose             | Non-Internal Market                                                            |
		| Border Control Post | All                                                                            |
		| Reason              | Reason 1                                                                       |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Democratic Republic of the Congo / 0910 / Non-Internal Market)
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF 'Trader 1' credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Democratic Republic of the Congo" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0910' commodity code
	And the user selects the commodity code '0910' and description 'Ginger, saffron, turmeric (curcuma), thyme, bay leaves, curry and other spices' in the commodity tree
	Then the commodity details should be populated '0910' 'Ginger, saffron, turmeric (curcuma), thyme, bay leaves, curry and other spices'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Non-internal market' radio option
	And the user enters 'London' as the Point of exit
	And the user enters the date and time the consignment will leave Great Britain
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_1'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 50            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 0910
	When the user updates the bulk update CSV 'SPS-8832_CHED-D_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '0910' to the recorded 'Iteration_1_RuleId'
	Then the bulk update CSV row for commodity code '0910' should contain the recorded 'Iteration_1_RuleId'
	And 'Iteration_1' is complete
	# ---------- Iteration 1: Complete ----------
	# ---------- Iteration 2: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '1006' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                   |
		| Description         | Rice                    |
		| Commodity code      | 1006                    |
		| Rate %              | 0                       |
		| Previous rate %     | 0                       |
		| Permanent           | Yes                     |
		| End date            |                         |
		| Countries           | Montserrat              |
		| Country groups      | Euro-Mediterranean Area |
		| Country exceptions  | None                    |
		| Purpose             | All                     |
		| Border Control Post | All                     |
		| Reason              |                         |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Montserrat / 1006)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Montserrat" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '1006' commodity code
	And the user selects the commodity code '1006' and description 'Rice' in the commodity tree
	Then the commodity details should be populated '1006' 'Rice'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_2'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 0             |
		| Total          | 1             |
		| Triggered      | 0             |
		| IsTriggered    | false         |
	# Update bulk-update CSV with the new rule Id for commodity 1006
	When the user updates the bulk update CSV 'SPS-8832_CHED-D_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '1006' to the recorded 'Iteration_2_RuleId'
	Then the bulk update CSV row for commodity code '1006' should contain the recorded 'Iteration_2_RuleId'
	And 'Iteration_2' is complete
	# ---------- Iteration 2: Complete ----------
	# ---------- Iteration 3: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '13023290' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                                            |
		| Description         | Of guar seeds                                    |
		| Commodity code      | 13023290                                         |
		| Rate %              | 75                                               |
		| Previous rate %     | 0                                                |
		| Permanent           | Yes                                              |
		| End date            |                                                  |
		| Countries           | Afghanistan, Zimbabwe                            |
		| Country groups      | None                                             |
		| Country exceptions  | None                                             |
		| Purpose             | All                                              |
		| Border Control Post | Felixstowe - GBFXT1, Manchester airport - GBMNC1 |
		| Reason              | Reason 3                                         |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Afghanistan / 13023290 / Felixstowe)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Afghanistan" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '13023290' commodity code
	Then the commodity details should be populated '13023290' 'Of guar seeds'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'FELIXSTOWE (GBFXT)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_3'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 75            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 13023290
	When the user updates the bulk update CSV 'SPS-8832_CHED-D_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '13023290' to the recorded 'Iteration_3_RuleId'
	Then the bulk update CSV row for commodity code '13023290' should contain the recorded 'Iteration_3_RuleId'
	And 'Iteration_3' is complete
	# ---------- Iteration 3: Complete ----------
	# ---------- Iteration 4: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '2008191930' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                                                            |
		| Description         | Hazelnuts, otherwise prepared or preserved, including mixtures   |
		| Commodity code      | 2008191930                                                       |
		| Rate %              | 100                                                              |
		| Previous rate %     | 0                                                                |
		| Permanent           | Yes                                                              |
		| End date            |                                                                  |
		| Countries           | None                                                             |
		| Country groups      | EU Member States, The Americas                                   |
		| Country exceptions  | Antigua and Barbuda                                              |
		| Purpose             | All                                                              |
		| Border Control Post | All                                                              |
		| Reason              |                                                                  |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Mexico / 2008191930)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Mexico" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '2008191930' commodity code
	Then the commodity details should be populated '2008191930' 'Hazelnuts, otherwise prepared or preserved, including mixtures'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_4'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 100           |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 2008191930
	When the user updates the bulk update CSV 'SPS-8832_CHED-D_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '2008191930' to the recorded 'Iteration_4_RuleId'
	Then the bulk update CSV row for commodity code '2008191930' should contain the recorded 'Iteration_4_RuleId'
	And 'Iteration_4' is complete
	# ---------- Iteration 4: Complete ----------
	# ---------- Iteration 5: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '*' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value           |
		| Description         | ALL             |
		| Commodity code      | *               |
		| Rate %              | 25              |
		| Previous rate %     | 0               |
		| Permanent           | No              |
		| End date            | 01/01/2027      |
		| Countries           | Togo            |
		| Country groups      | None            |
		| Country exceptions  | None            |
		| Purpose             | Internal Market |
		| Border Control Post | All             |
		| Reason              |                 |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Togo / 200850 / Internal Market)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Togo" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '200850' commodity code
	Then the commodity details should be populated '200850' 'Apricots, otherwise prepared or preserved'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_5'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 25            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity *
	When the user updates the bulk update CSV 'SPS-8832_CHED-D_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '*' to the recorded 'Iteration_5_RuleId'
	Then the bulk update CSV row for commodity code '*' should contain the recorded 'Iteration_5_RuleId'
	And the user copies the updated bulk update CSV back to the source data directory
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------

@SPS-9443
Scenario: Bulk upload update existing rules for CHEDD - SPS-9443
	# ---------- Iteration 1: Start ----------
	# Upload the update rules
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the View all CHED-D (Import) Commodity Rules report page
	Then the count of CHED-D import commodity rules is recorded as 'InitialRuleCount'
	When the user clicks the 'CHED-D' link from the Risk Engine header menu
	Then the CHED-D imports page should be displayed
	When the user clicks the Bulk upload commodity rules link on the CHED-D imports page
	Then the Bulk upload commodity rules for CHED-D page should be displayed
	When the user clicks the Choose file button on the CHED-D bulk upload page
	And the user navigates to and selects the CHED-D bulk upload file 'SPS-8832_CHED-D_BulkUploadRules_UpdatedRules.csv'
	Then the selected file name is displayed within the CHED-D File added box
	When the user clicks the Continue button on the CHED-D bulk upload page
	Then the Commodity rules for CHED-D page should be displayed with the File submission in progress section showing status 'Ready to submit'
	When the user clicks the Confirm and submit link for the file submission in progress on the CHED-D page
	Then the There are rules in your CSV file that already exist page should be displayed
	When the user selects Yes to replace existing rules and clicks Continue
	Then the Check and submit commodity rules for CHED-D page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 5     |
		| Add new rules          | 0     |
	And the Check and submit commodity rules for CHED-D page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the Confirm and submit rules button on the CHED-D page
	Then the File submission in progress section should be removed and the first Previous submission should have status 'Completed' on the CHED-D page
	When the user clicks the View summary link for the first record in the Previous submissions section on the CHED-D page
	Then the Commodity rules status for CHED-D page is displayed with the following upload details
		| Field                  | Value |
		| Replace existing rules | 5     |
		| Add new rules          | 0     |
	And the Commodity rules status for CHED-D page should show Existing rules equal to 'InitialRuleCount'
	When the user clicks the View all CHED-D imports commodity rules link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the View all CHED-D (Import) Commodity Rules report page
	Then the count of CHED-D import commodity rules should equal the recorded 'InitialRuleCount'
	When the user enters '0910' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                                                                          |
		| Description         | Ginger, saffron, turmeric (curcuma), thyme, bay leaves, curry and other spices |
		| Commodity code      | 0910                                                                           |
		| Rate %              | 95                                                                             |
		| Previous rate %     | 50                                                                             |
		| Permanent           | Yes                                                                            |
		| End date            |                                                                                |
		| Countries           | ALL                                                                            |
		| Country groups      | None                                                                           |
		| Country exceptions  | Djibouti, Montserrat, Togo                                                     |
		| Purpose             | Non-Internal Market                                                            |
		| Border Control Post | All                                                                            |
		| Reason              | Reason 1b                                                                      |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Democratic Republic of the Congo / 0910 / Non-Internal Market)
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF 'Trader 1' credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Democratic Republic of the Congo" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0910' commodity code
	And the user selects the commodity code '0910' and description 'Ginger, saffron, turmeric (curcuma), thyme, bay leaves, curry and other spices' in the commodity tree
	Then the commodity details should be populated '0910' 'Ginger, saffron, turmeric (curcuma), thyme, bay leaves, curry and other spices'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Non-internal market' radio option
	And the user enters 'London' as the Point of exit
	And the user enters the date and time the consignment will leave Great Britain
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_1'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 95            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_1' is complete
	# ---------- Iteration 1: Complete ----------
	# ---------- Iteration 2: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '1006' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                   |
		| Description         | Rice                    |
		| Commodity code      | 1006                    |
		| Rate %              | 0                       |
		| Previous rate %     | 0                       |
		| Permanent           | Yes                     |
		| End date            |                         |
		| Countries           | Montserrat              |
		| Country groups      | Euro-Mediterranean Area |
		| Country exceptions  | None                    |
		| Purpose             | Internal Market         |
		| Border Control Post | All                     |
		| Reason              | Reason 2b               |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Montserrat / 1006 / Internal Market)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Montserrat" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '1006' commodity code
	And the user selects the commodity code '1006' and description 'Rice' in the commodity tree
	Then the commodity details should be populated '1006' 'Rice'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_2'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 0             |
		| Total          | 1             |
		| Triggered      | 0             |
		| IsTriggered    | false         |
	And 'Iteration_2' is complete
	# ---------- Iteration 2: Complete ----------
	# ---------- Iteration 3: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '13023290' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                          |
		| Description         | Of guar seeds                  |
		| Commodity code      | 13023290                       |
		| Rate %              | 50                             |
		| Previous rate %     | 75                             |
		| Permanent           | Yes                            |
		| End date            |                                |
		| Countries           | Afghanistan, Zimbabwe          |
		| Country groups      | None                           |
		| Country exceptions  | None                           |
		| Purpose             | All                            |
		| Border Control Post | Manchester airport - GBMNC1    |
		| Reason              |                                |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Afghanistan / 13023290 / Manchester airport)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Afghanistan" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '13023290' commodity code
	Then the commodity details should be populated '13023290' 'Of guar seeds'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'MANCHESTER AIRPORT (GBMAN)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_3'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 50            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_3' is complete
	# ---------- Iteration 3: Complete ----------
	# ---------- Iteration 4: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '2008191930' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                                                          |
		| Description         | Hazelnuts, otherwise prepared or preserved, including mixtures |
		| Commodity code      | 2008191930                                                     |
		| Rate %              | 100                                                            |
		| Previous rate %     | 0                                                              |
		| Permanent           | Yes                                                            |
		| End date            |                                                                |
		| Countries           | None                                                           |
		| Country groups      | Euro-Mediterranean Area, EU Member States, The Americas        |
		| Country exceptions  | Antigua and Barbuda                                            |
		| Purpose             | All                                                            |
		| Border Control Post | All                                                            |
		| Reason              |                                                                |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Mexico / 2008191930)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Mexico" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '2008191930' commodity code
	Then the commodity details should be populated '2008191930' 'Hazelnuts, otherwise prepared or preserved, including mixtures'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_4'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 100           |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_4' is complete
	# ---------- Iteration 4: Complete ----------
	# ---------- Iteration 5: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user enters '*' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value           |
		| Description         | ALL             |
		| Commodity code      | *               |
		| Rate %              | 30              |
		| Previous rate %     | 25              |
		| Permanent           | No              |
		| End date            | 01/01/2027      |
		| Countries           | Togo            |
		| Country groups      | None            |
		| Country exceptions  | None            |
		| Purpose             | Internal Market |
		| Border Control Post | All             |
		| Reason              |                 |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Togo / 200850 / Internal Market)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Togo" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '200850' commodity code
	Then the commodity details should be populated '200850' 'Apricots, otherwise prepared or preserved'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_5'
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Risk decision report link
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
		| Rate           | 30            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------
	# Remove the 5 rules created by the update bulk upload
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user clicks the Remove rule link for CHED-D import commodity rule Id recorded as 'Iteration_1_RuleId'
	Then the CHED-D import commodity rule Id recorded as 'Iteration_1_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D import commodity rule Id recorded as 'Iteration_2_RuleId'
	Then the CHED-D import commodity rule Id recorded as 'Iteration_2_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D import commodity rule Id recorded as 'Iteration_3_RuleId'
	Then the CHED-D import commodity rule Id recorded as 'Iteration_3_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D import commodity rule Id recorded as 'Iteration_4_RuleId'
	Then the CHED-D import commodity rule Id recorded as 'Iteration_4_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D import commodity rule Id recorded as 'Iteration_5_RuleId'
	Then the CHED-D import commodity rule Id recorded as 'Iteration_5_RuleId' should no longer be present in the rules table
	When the user scrolls to the bottom of the View all CHED-D (Import) Commodity Rules report page
	Then the count of CHED-D import commodity rules should be 5 less than 'InitialRuleCount'

@SPS-9521
Scenario: New import commodity rule for CHEDD - SPS-9521
	# Create a new import commodity rule
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'CHED-D' link from the Risk Engine header menu
	Then the CHED-D imports page should be displayed
	When the user clicks the Individual commodity rules link under the CHED-D rules header
	Then the Select the CHED-D commodity you want to add a rule for page should be displayed
	When the user searches for commodity '070930' with name 'Aubergines (eggplants)' on the Select the CHED-D commodity you want to add a rule for page
	Then the You have selected a commodity page should be displayed showing commodity '070930' with description 'Aubergines (eggplants)'
	When the user clicks the Continue button on the You have selected a commodity page
	Then the Which countries will this commodity rule apply to? page should be displayed
	When the user selects 'Yemen' in the Countries field on the Which countries will this commodity rule apply to? page
	And the user clicks the Continue button on the Which countries will this commodity rule apply to? page
	Then the Will this commodity rule apply to all border control posts? page should be displayed
	When the user selects the 'Yes' radio button on the Will this commodity rule apply to all border control posts? page
	And the user clicks the Continue button on the Will this commodity rule apply to all border control posts? page
	Then the Which import purposes will this commodity rule apply to? page should be displayed
	When the user ticks both import purpose checkboxes on the Which import purposes will this commodity rule apply to? page
	And the user clicks the Continue button on the Which import purposes will this commodity rule apply to? page
	Then the Does this commodity rule have a start or end date? page should be displayed
	When the user selects the 'No' radio button on the Does this commodity rule have a start or end date? page
	And the user clicks the Continue button on the Does this commodity rule have a start or end date? page
	Then the Set the inspection rate page should be displayed
	When the user enters '50' as the inspection rate on the Set the inspection rate page
	And the user clicks the Continue button on the Set the inspection rate page
	Then the Check and submit commodity rule page should be displayed
	When the user clicks the Confirm and submit rule button on the Check and submit commodity rule page
	Then the Commodity rule submitted page should be displayed
	# Verify the rule in the imports commodity rules report
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the View all CHED-D (Import) Commodity Rules report page
	Then the count of CHED-D import commodity rules is recorded as 'CommodityRuleCount'
	When the user enters '070930' in the CHED-D import commodity rules search field
	Then the top CHED-D import commodity rule row should match the following details
		| Field               | Value                  |
		| Description         | Aubergines (eggplants) |
		| Commodity code      | 070930                 |
		| Rate %              | 50                     |
		| Previous rate %     | 0                      |
		| Permanent           | Yes                    |
		| End date            |                        |
		| Countries           | Yemen                  |
		| Country groups      | None                   |
		| Country exceptions  | None                   |
		| Purpose             | All                    |
		| Border Control Post | All                    |
		| Reason              |                        |
	And the top CHED-D import commodity rule row should have Start date as today's date
	And the user records the Id of the top CHED-D import commodity rule row as 'NewCommodityRuleId'
	# Submit 4 matching CHED-D notifications in IPAFFS (Yemen / 0709300005)
	# --- APP-A ---
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF 'Trader 1' credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Yemen" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0709300005' commodity code
	Then the commodity details should be populated '0709300005' 'Solanum melongena'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_A'
	# --- APP-B ---
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Yemen" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0709300005' commodity code
	Then the commodity details should be populated '0709300005' 'Solanum melongena'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_B'
	# --- APP-C ---
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Yemen" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0709300005' commodity code
	Then the commodity details should be populated '0709300005' 'Solanum melongena'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_C'
	# --- APP-D ---
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Yemen" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0709300005' commodity code
	Then the commodity details should be populated '0709300005' 'Solanum melongena'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_D'
	# Validate via Risk Decision Report - APP-A (Total=1, Triggered=1, IsTriggered=true)
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'APP_A' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_A'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCommodityRuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 50            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	And 'APP_A' is complete
	# Validate via Risk Decision Report - APP-B (Total=2, Triggered=1, IsTriggered=false)
	When the user enters the recorded CHED Reference for 'APP_B' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_B'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCommodityRuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 50            |
		| Total          | 2             |
		| Triggered      | 1             |
		| IsTriggered    | false         |
	And 'APP_B' is complete
	# Validate via Risk Decision Report - APP-C (Total=3, Triggered=1, IsTriggered=false)
	When the user enters the recorded CHED Reference for 'APP_C' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_C'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCommodityRuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 50            |
		| Total          | 3             |
		| Triggered      | 1             |
		| IsTriggered    | false         |
	And 'APP_C' is complete
	# Validate via Risk Decision Report - APP-D (Total=4, Triggered=2, IsTriggered=true)
	When the user enters the recorded CHED Reference for 'APP_D' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_D'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCommodityRuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | EUImport      |
		| Rate           | 50            |
		| Total          | 4             |
		| Triggered      | 2             |
		| IsTriggered    | true          |
	And 'APP_D' is complete
	# Delete the rule created by this test
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the CHED-D Imports commodity rules report link
	Then the View all CHED-D (Import) Commodity Rules report page should be displayed
	When the user clicks the Remove rule link for CHED-D import commodity rule Id recorded as 'NewCommodityRuleId'
	Then the CHED-D import commodity rule Id recorded as 'NewCommodityRuleId' should no longer be present in the rules table
	When the user scrolls to the bottom of the View all CHED-D (Import) Commodity Rules report page
	Then the count of CHED-D import commodity rules should be 1 less than 'CommodityRuleCount'

@SPS-9522
Scenario: New EU import country rule for CHEDD - SPS-9522
	# Create a new EU import country rule
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'CHED-D' link from the Risk Engine header menu
	Then the CHED-D imports page should be displayed
	When the user clicks the Country rules link under the CHED-D rules header
	Then the EU (Import) Country Rules page should be displayed
	When the user selects 'Mozambique' from the country dropdown on the EU import country rules page
	And the user sets the inspection rate to 50 on the EU import country rules page
	And the user ensures the Permanent checkbox is checked on the EU import country rules page
	And the user clicks the Confirm and send button on the EU import country rules page
	Then the Confirmation of country rate change page should be displayed with the following details
		| Field | Value |
		| From  | 0%    |
		| To    | 50%   |
	When the user clicks the Confirm and send button on the confirmation of country rate change page
	Then the Rule change complete page should be displayed
	# Verify the rule in the imports commodity rules report
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the Country rules report link under the CHED-D reports header
	Then the View rules for all countries page should be displayed
	When the user scrolls to the bottom of the View rules for all countries page
	Then the EU import country rule count is recorded as 'CountryRuleCount'
	When the user enters 'Mozambique' in the EU import country rules search field
	And the user sorts the EU import country rules table by Id descending
	Then the top EU import country rule row should match the following details
		| Field           | Value      |
		| Country         | Mozambique |
		| Rate %          | 50         |
		| Previous rate % | 0          |
		| Permanent rule  | YES        |
		| Start Date      |            |
		| End Date        |            |
	And the top EU import country rule row should have Last Updated date as today's date
	And the top EU import country rule row should have Created date as today's date
	And the user records the Id of the top EU import country rule row as 'NewCountryRuleId'
	# Submit 4 matching CHED-D notifications in IPAFFS (Mozambique / 11041999 / Internal Market)
	# --- APP-A ---
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF 'Trader 1' credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Mozambique" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '11041999' commodity code
	Then the commodity details should be populated '11041999' 'Rolled or flaked cereal grains (excluding grains of oats, wheat, rye, maize and barley, and flaked rice)'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_A'
	# --- APP-B ---
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Mozambique" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '11041999' commodity code
	Then the commodity details should be populated '11041999' 'Rolled or flaked cereal grains (excluding grains of oats, wheat, rye, maize and barley, and flaked rice)'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_B'
	# --- APP-C ---
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Mozambique" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '11041999' commodity code
	Then the commodity details should be populated '11041999' 'Rolled or flaked cereal grains (excluding grains of oats, wheat, rye, maize and barley, and flaked rice)'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_C'
	# --- APP-D ---
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Mozambique" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	And the Country of origin and Country from where consigned fields are pre-populated with the previously selected country
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '11041999' commodity code
	Then the commodity details should be populated '11041999' 'Rolled or flaked cereal grains (excluding grains of oats, wheat, rye, maize and barley, and flaked rice)'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When the user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
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
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'APP_D'
	# Validate via Risk Decision Report - APP-A (Total=1, Triggered=1, IsTriggered=true)
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'APP_A' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_A'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCountryRuleId' with the following values
		| Field         | Value       |
		| RuleType      | CountryRule |
		| RegulatorType | EUImport    |
		| Rate          | 50          |
		| Total         | 1           |
		| Triggered     | 1           |
		| IsTriggered   | true        |
	And 'APP_A' is complete
	# Validate via Risk Decision Report - APP-B (Total=2, Triggered=1, IsTriggered=false)
	When the user enters the recorded CHED Reference for 'APP_B' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_B'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCountryRuleId' with the following values
		| Field         | Value       |
		| RuleType      | CountryRule |
		| RegulatorType | EUImport    |
		| Rate          | 50          |
		| Total         | 2           |
		| Triggered     | 1           |
		| IsTriggered   | false       |
	And 'APP_B' is complete
	# Validate via Risk Decision Report - APP-C (Total=3, Triggered=1, IsTriggered=false)
	When the user enters the recorded CHED Reference for 'APP_C' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_C'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCountryRuleId' with the following values
		| Field         | Value       |
		| RuleType      | CountryRule |
		| RegulatorType | EUImport    |
		| Rate          | 50          |
		| Total         | 3           |
		| Triggered     | 1           |
		| IsTriggered   | false       |
	And 'APP_C' is complete
	# Validate via Risk Decision Report - APP-D (Total=4, Triggered=2, IsTriggered=true)
	When the user enters the recorded CHED Reference for 'APP_D' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'APP_D'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewCountryRuleId' with the following values
		| Field         | Value       |
		| RuleType      | CountryRule |
		| RegulatorType | EUImport    |
		| Rate          | 50          |
		| Total         | 4           |
		| Triggered     | 2           |
		| IsTriggered   | true        |
	And 'APP_D' is complete
	# Delete the rule created by this test
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-D reports link
	Then the CHED-D reports page should be displayed
	When the user clicks the Country rules report link under the CHED-D reports header
	Then the View rules for all countries page should be displayed
	When the user clicks the Remove rule link for EU import country rule Id recorded as 'NewCountryRuleId'
	Then the EU import country rule Id recorded as 'NewCountryRuleId' should no longer be present in the rules table
	When the user scrolls to the bottom of the View rules for all countries page
	Then the EU import country rule count should be 1 less than 'CountryRuleCount'