@RiskEngine
Feature: Risk Engine CHEDPP

Bulk upload, Update and Test rules with end-to-end validation for a CHEDPP notification

Scenario: Bulk upload of new rules for CHEDPP - SPS-9414
	# ---------- Iteration 1: Start ----------
	# Upload the rules
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the PHSI imports commodity rules report link
	Then the View all PHSI (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the PHSI rules report page
	Then the count of rules is recorded as 'InitialRuleCount'
	When the user clicks the 'CHED-PP' link from the Risk Engine header menu
	Then the CHED-PP imports and exports page should be displayed
	When the user clicks the Bulk upload commodity rules link
	Then the Upload multiple commodity rules using a CSV file page should be displayed
	When the user clicks the Choose file button
	And the user navigates to and selects the bulk upload file 'SPS-8806_CHED-PP_BulkUploadRules.csv'
	Then the selected file name is displayed next to the Choose file button
	When the user clicks the Upload button on the bulk upload page
	Then the Check your file processing status page should be displayed
	When the user clicks the 'view the processing status of your file here' link
	Then the Submit multiple commodity rules using a CSV file page is displayed with the first record status 'Ready to submit'
	When the user clicks the Confirm and submit link for the first record in the list
	Then the 'Are the commodity rule changes correct?' page is displayed with the following upload details
		| Field                          | Value    |
		| CHED file type                 | CHED-PP  |
		| Replace existing rules?        | No       |
		| Incoming rules                 | 5        |
		| Existing rule IDs to be deleted| 0        |
		| Number of rules to be updated  | 0        |
		| Incoming rules to be added     | 5        |
	When the user selects the 'Yes, submit changes to the risk engine' radio option
	And the user clicks the Submit button on the rule changes page
	Then the Uploading rule changes to the risk engine page should be displayed
	When the user clicks the Check file status link
	Then the Submit multiple commodity rules using a CSV file page is displayed with the first record status 'Completed'
	When the user clicks the View summary link for the first record in the list
	Then the CSV file details and status page should be displayed with 5 incoming rules added
	And the CSV file details and status page should be displayed with the count of Total rules 5 more than 'InitialRuleCount'
	When the user clicks the PHSI reporting link at the bottom of the summary page
	Then the View all PHSI (Import) Commodity Rules report page should be displayed in a new browser tab
	When the user scrolls to the bottom of the PHSI rules report page
	Then the count of rules should be 5 more than 'InitialRuleCount'
	And the count of rules is recorded as 'FinalRuleCount'
	When the user enters '07020099' in the PHSI rules search field
	And the user sorts the PHSI rules table by Id descending
	Then the top PHSI rule row should match the following details
		| Field                  | Value    |
		| Commodity code         | 07020099 |
		| Intended Use           | None     |
		| Type                   | None     |
		| Rate %                 | 25       |
		| Previous rate %        | 0        |
		| Permanent              | Yes      |
		| Countries              | Djibouti |
		| Country groups         | None     |
		| Country exceptions     | None     |
		| Document check aligned | No       |
		| Reason                 | Reason 1 |
	And the user records the Id of the top PHSI rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-PP notification in IPAFFS (Djibouti / 0702009907)
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Trader credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Djibouti" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Djibouti" as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Manual entry' option to add commodity details
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Commodity code search tab
	And the user searches for the commodity code '0702009907'
	Then the CHED PP commodity details should be populated '0702009907' 'Cherry tomatoes'
	When the user selects EPPO code 'LYPSS' checkbox
	And the user clicks Save and continue
	Then the Description of the goods Variety and class of commodity should be displayed
	When the user select 'Class I' class of EPPO code 'LYPSS'
	And the user selects 'Other than Solanum lycopersicum' variety of EPPO code 'LYPSS'
	And the user clicks Save and continue
	Then the selected commodity 'Cherry tomatoes' should be displayed with Commodity code '0702009907' and Genus 'Lycopersicon sp.' and EPPO code 'LYPSS' and Class 'Class I' and Variety 'Other than Solanum lycopersicum'
	When the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user selects the check box for the commodity code '0702009907'
	And the user populates Number of packages as '10' for CHED PP commodity
	And the user selects type of package as 'Box' for CHED PP commodity
	And the user populates Quantity as '10' for CHED PP commodity
	And the user selects Quantity type as 'Kilograms' for CHED PP commodity
	And the user populates Net weight as '100' for CHED PP commodity
	And the user clicks Apply Button
	And the user clicks Save and continue
	Then the Additional details page should be displayed
	When the user enters the total gross weight '110'
	And the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' 'YY10 KTP' 'No' 'Doc23456'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Phytosanitary certificate'
	And the user enters Document reference 'PHYTOCERT123'
	And the user enters date of issue from last week
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'IPAFFS IDM Test' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address "DEFRA"
	Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "DEFRA"
	Then the chosen consignor or exporter should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference for notification 1
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for notification 1 in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of notification 1
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_1_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | PHSIImport    |
		| Rate           | 25            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 07020099
	When the user updates the bulk update CSV 'SPS-8834_CHED-PP_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '07020099' to the recorded 'Iteration_1_RuleId'
	Then the bulk update CSV row for commodity code '07020099' should contain the recorded 'Iteration_1_RuleId'
	And 'Iteration_1' is complete
	# ---------- Iteration 1: Complete ----------
	# ---------- Iteration 2: Start ----------
	# Locate and record the new rule for commodity 12099130
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the PHSI imports commodity rules report link
	Then the View all PHSI (Import) Commodity Rules report page should be displayed
	When the user enters '12099130' in the PHSI rules search field
	And the user sorts the PHSI rules table by Id descending
	Then the top PHSI rule row should match the following details
		| Field                  | Value            |
		| Commodity code         | 12099130         |
		| Name                   | Beta sp.         |
		| Eppo                   | BEASS            |
		| Intended Use           | Not Test/Trial   |
		| Type                   | Seed             |
		| Rate %                 | 1                |
		| Previous rate %        | 0                |
		| Permanent              | No               |
		| End date               | 01/01/2027       |
		| Countries              | None             |
		| Country groups         | EU Member States |
		| Country exceptions     | None             |
		| Document check aligned | Yes              |
	And the user records the Id of the top PHSI rule row as 'Iteration_2_RuleId'
	# Submit TWO matching CHED-PP notifications in IPAFFS (Italy / 12099130)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	# --- APP-A (notification 2) ---
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Italy" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Italy" as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Manual entry' option to add commodity details
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Commodity code search tab
	And the user searches for the commodity code '12099130'
	Then the CHED PP commodity details should be populated '12099130' 'Salad beet seed or beetroot seed (Beta vulgaris var. conditiva)'
	When the user selects EPPO code 'BEASS' checkbox
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user selects the check box for the commodity code '12099130'
	And the user populates Number of packages as '10' for CHED PP commodity
	And the user selects type of package as 'Box' for CHED PP commodity
	And the user populates Quantity as '10' for CHED PP commodity
	And the user selects Quantity type as 'Kilograms' for CHED PP commodity
	And the user populates Net weight as '100' for CHED PP commodity
	And the user clicks Apply Button
	And the user clicks Save and continue
	Then the Additional details page should be displayed
	When the user enters the total gross weight '110'
	And the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' 'YY10 KTP' 'No' 'Doc23456'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Phytosanitary certificate'
	And the user enters Document reference 'PHYTOCERT123'
	And the user enters date of issue from last week
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'IPAFFS IDM Test' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address "DEFRA"
	Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "DEFRA"
	Then the chosen consignor or exporter should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference for notification 2
	# --- APP-B (notification 3) ---
	When the user clicks Return to your dashboard
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Italy" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Italy" as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Manual entry' option to add commodity details
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Commodity code search tab
	And the user searches for the commodity code '12099130'
	Then the CHED PP commodity details should be populated '12099130' 'Salad beet seed or beetroot seed (Beta vulgaris var. conditiva)'
	When the user selects EPPO code 'BEASS' checkbox
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user selects the check box for the commodity code '12099130'
	And the user populates Number of packages as '10' for CHED PP commodity
	And the user selects type of package as 'Box' for CHED PP commodity
	And the user populates Quantity as '10' for CHED PP commodity
	And the user selects Quantity type as 'Kilograms' for CHED PP commodity
	And the user populates Net weight as '100' for CHED PP commodity
	And the user clicks Apply Button
	And the user clicks Save and continue
	Then the Additional details page should be displayed
	When the user enters the total gross weight '110'
	And the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' 'YY10 KTP' 'No' 'Doc23456'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Phytosanitary certificate'
	And the user enters Document reference 'PHYTOCERT123'
	And the user enters date of issue from last week
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'IPAFFS IDM Test' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address "DEFRA"
	Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "DEFRA"
	Then the chosen consignor or exporter should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference for notification 3
	# Validate via Risk Decision Report - APP-A first (Total=1, Triggered=0)
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for notification 2 in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of notification 2
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | PHSIImport    |
		| Rate           | 1             |
		| Total          | 1             |
		| Triggered      | 0             |
		| IsTriggered    | false         |
	# Validate via Risk Decision Report - APP-B (Total=2, Triggered=1)
	When the user enters the recorded CHED Reference for notification 3 in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of notification 3
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | PHSIImport    |
		| Rate           | 1             |
		| Total          | 2             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 12099130
	When the user updates the bulk update CSV 'SPS-8834_CHED-PP_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '12099130' to the recorded 'Iteration_2_RuleId'
	Then the bulk update CSV row for commodity code '12099130' should contain the recorded 'Iteration_2_RuleId'
	And 'Iteration_2' is complete
	# ---------- Iteration 2: Complete ----------
	# ---------- Iteration 3: Start ----------
	# Locate and record the new rule for commodity 060120
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the PHSI imports commodity rules report link
	Then the View all PHSI (Import) Commodity Rules report page should be displayed
	When the user enters '060120' in the PHSI rules search field
	And the user sorts the PHSI rules table by Id descending
	Then the top PHSI rule row should match the following details
		| Field                  | Value                                                              |
		| Commodity code         | 060120                                                             |
		| Intended Use           | Intended for final users                                           |
		| Type                   | Plant                                                              |
		| Woody/Non-woody?       | Non-woody                                                          |
		| Indoor use/Outdoor use?| Outdoor use                                                        |
		| Rate %                 | 30                                                                 |
		| Previous rate %        | 0                                                                  |
		| Permanent              | Yes                                                                |
		| Countries              | None                                                               |
		| Country groups         | European Countries, Euro-Mediterranean Area, Third Countries       |
		| Country exceptions     | None                                                               |
		| Document check aligned | No                                                                 |
		| Reason                 | Reason 3                                                           |
	And the user records the Id of the top PHSI rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-PP notification in IPAFFS (Monaco / 06012010 / Intended for final users)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Monaco" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Monaco" as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Manual entry' option to add commodity details
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Commodity code search tab
	And the user searches for the commodity code '06012010'
	Then the CHED PP commodity details should be populated '06012010' 'Chicory plants and roots'
	When the user searchs for EPPO code 'CICCA' and clicks add link
	Then Genus (and Species) 'Cichorium calvum' and EPPO code 'CICCA' should be populated in commodity page
	When the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user selects the check box for the commodity code '06012010'
	And the user populates Number of packages as '10' for CHED PP commodity
	And the user selects type of package as 'Box' for CHED PP commodity
	And the user populates Quantity as '10' for CHED PP commodity
	And the user selects Quantity type as 'Kilograms' for CHED PP commodity
	And the user populates Net weight as '100' for CHED PP commodity
	And the user clicks Apply Button
	And the user selects Intended for final users as 'Yes' for CHED PP commodity '06012010'
	And the user clicks Save and continue
	Then the Additional details page should be displayed
	When the user enters the total gross weight '110'
	And the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' 'YY10 KTP' 'No' 'Doc23456'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Phytosanitary certificate'
	And the user enters Document reference 'PHYTOCERT123'
	And the user enters date of issue from last week
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'IPAFFS IDM Test' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address "DEFRA"
	Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "DEFRA"
	Then the chosen consignor or exporter should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference for notification 4
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for notification 4 in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of notification 4
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_3_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | PHSIImport    |
		| Rate           | 30            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 060120
	When the user updates the bulk update CSV 'SPS-8834_CHED-PP_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '060120' to the recorded 'Iteration_3_RuleId'
	Then the bulk update CSV row for commodity code '060120' should contain the recorded 'Iteration_3_RuleId'
	And 'Iteration_3' is complete
	# ---------- Iteration 3: Complete ----------
	# ---------- Iteration 4: Start ----------
	# Locate and record the new rule for commodity 08094090
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the PHSI imports commodity rules report link
	Then the View all PHSI (Import) Commodity Rules report page should be displayed
	When the user enters '08094090' in the PHSI rules search field
	And the user sorts the PHSI rules table by Id descending
	Then the top PHSI rule row should match the following details
		| Field                  | Value             |
		| Commodity code         | 08094090          |
		| Intended Use           | None              |
		| Type                   | None              |
		| Rate %                 | 5                 |
		| Previous rate %        | 0                 |
		| Permanent              | Yes               |
		| Countries              | Montserrat        |
		| Country groups         | EU Member States  |
		| Country exceptions     | France, Germany   |
		| Document check aligned | Yes               |
	And the user records the Id of the top PHSI rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-PP notification in IPAFFS (Montserrat / 08094090)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Montserrat" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Montserrat" as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Manual entry' option to add commodity details
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Commodity code search tab
	And the user searches for the commodity code '08094090'
	Then the CHED PP commodity details should be populated '08094090' 'Sloes'
	When the user searchs for EPPO code 'PRNAF' and clicks add link
	Then Genus (and Species) 'Prunus africana' and EPPO code 'PRNAF' should be populated in commodity page
	When the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user selects the check box for the commodity code '08094090'
	And the user populates Number of packages as '10' for CHED PP commodity
	And the user selects type of package as 'Box' for CHED PP commodity
	And the user populates Quantity as '10' for CHED PP commodity
	And the user selects Quantity type as 'Kilograms' for CHED PP commodity
	And the user populates Net weight as '100' for CHED PP commodity
	And the user clicks Apply Button
	And the user clicks Save and continue
	Then the Additional details page should be displayed
	When the user enters the total gross weight '110'
	And the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' 'YY10 KTP' 'No' 'Doc23456'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Phytosanitary certificate'
	And the user enters Document reference 'PHYTOCERT123'
	And the user enters date of issue from last week
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'IPAFFS IDM Test' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address "DEFRA"
	Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "DEFRA"
	Then the chosen consignor or exporter should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference for notification 5
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for notification 5 in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of notification 5
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_4_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | PHSIImport    |
		| Rate           | 5             |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 08094090
	When the user updates the bulk update CSV 'SPS-8834_CHED-PP_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '08094090' to the recorded 'Iteration_4_RuleId'
	Then the bulk update CSV row for commodity code '08094090' should contain the recorded 'Iteration_4_RuleId'
	And 'Iteration_4' is complete
	# ---------- Iteration 4: Complete ----------
	# ---------- Iteration 5: Start ----------
	# Locate and record the new rule for commodity 12040010
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the PHSI imports commodity rules report link
	Then the View all PHSI (Import) Commodity Rules report page should be displayed
	When the user enters '12040010' in the PHSI rules search field
	And the user sorts the PHSI rules table by Id descending
	Then the top PHSI rule row should match the following details
		| Field                  | Value               |
		| Commodity code         | 12040010            |
		| Name                   | Linum usitatissimum |
		| Eppo                   | LIUUT               |
		| Intended Use           | Not Test/Trial      |
		| Type                   | Seed                |
		| Rate %                 | 10                  |
		| Previous rate %        | 0                   |
		| Permanent              | Yes                 |
		| Countries              | Togo                |
		| Country groups         | None                |
		| Country exceptions     | None                |
		| Document check aligned | No                  |
	And the user records the Id of the top PHSI rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-PP notification in IPAFFS (Togo / 12040010 / LIUUT)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Togo" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Togo" as the Country of origin and Country from where consigned
	When the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Manual entry' option to add commodity details
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Commodity code search tab
	And the user searches for the commodity code '12040010'
	Then the CHED PP commodity details should be populated '12040010' 'For sowing'
	When the user selects EPPO code 'LIUUT' checkbox
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user selects the check box for the commodity code '12040010'
	And the user populates Number of packages as '10' for CHED PP commodity
	And the user selects type of package as 'Box' for CHED PP commodity
	And the user populates Quantity as '10' for CHED PP commodity
	And the user selects Quantity type as 'Kilograms' for CHED PP commodity
	And the user populates Net weight as '100' for CHED PP commodity
	And the user clicks Apply Button
	And the user clicks Save and continue
	Then the Additional details page should be displayed
	When the user enters the total gross weight '110'
	And the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' 'YY10 KTP' 'No' 'Doc23456'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Phytosanitary certificate'
	And the user enters Document reference 'PHYTOCERT123'
	And the user enters date of issue from last week
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'IPAFFS IDM Test' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address "DEFRA"
	Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "DEFRA"
	Then the chosen consignor or exporter should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference for notification 6
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for notification 6 in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of notification 6
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_5_RuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | PHSIImport    |
		| Rate           | 10            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity 12040010
	When the user updates the bulk update CSV 'SPS-8834_CHED-PP_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '12040010' to the recorded 'Iteration_5_RuleId'
	Then the bulk update CSV row for commodity code '12040010' should contain the recorded 'Iteration_5_RuleId'
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------