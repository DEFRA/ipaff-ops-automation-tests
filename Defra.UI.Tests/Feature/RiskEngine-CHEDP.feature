@RiskEngine
Feature: Risk Engine CHEDP

Bulk upload, Update and Test rules with end-to-end validation for a CHEDP notification

Scenario: Bulk upload initial load for CHEDP - SPS-9419
	# ---------- Iteration 1: Start ----------
	# Upload the rules
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the CHED-P rules report page
	Then the count of CHED-P rules is recorded as 'InitialRuleCount'
	When the user clicks the 'CHED-P' link from the Risk Engine header menu
	Then the CHED-P imports page should be displayed
	When the user clicks the Bulk upload commodity rules link on the CHED-P imports page
	Then the Upload multiple commodity rules using a CSV file page should be displayed
	When the user clicks the Choose file button
	And the user navigates to and selects the bulk upload file 'SPS-8805_CHED-P_BulkUploadRules.csv'
	Then the selected file name is displayed next to the Choose file button
	When the user clicks the Upload button on the bulk upload page
	Then the Check your file processing status page should be displayed
	When the user clicks the 'view the processing status of your file here' link
	Then the Submit multiple commodity rules using a CSV file page is displayed with the first record status 'Ready to submit'
	When the user clicks the Confirm and submit link for the first record in the list
	Then the 'Are the commodity rule changes correct?' page is displayed with the following upload details
		| Field                           | Value   |
		| CHED file type                  | CHED-P  |
		| Replace existing rules?         | No      |
		| Incoming rules                  | 5       |
		| Existing rule IDs to be deleted | 0       |
		| Number of rules to be updated   | 0       |
		| Incoming rules to be added      | 5       |
	And the 'Are the commodity rule changes correct?' page should show Existing rules equal to 'InitialRuleCount'
	And the 'Are the commodity rule changes correct?' page should show Total rules 5 more than 'InitialRuleCount'
	When the user selects the 'Yes, submit changes to the risk engine' radio option
	And the user clicks the Submit button on the rule changes page
	Then the Uploading rule changes to the risk engine page should be displayed
	When the user clicks the Check file status link
	Then the Submit multiple commodity rules using a CSV file page is displayed with the first record status 'Completed'
	When the user clicks the View summary link for the first record in the list
	Then the CSV file details and status page is displayed with the following upload details
		| Field                           | Value   |
		| CHED file type                  | CHED-P  |
		| Replace existing rules?         | No      |
		| Incoming rules                  | 5       |
		| Existing rule IDs to be deleted | 0       |
		| Number of rules to be updated   | 0       |
		| Incoming rules to be added      | 5       |
	And the CSV file details and status page should show Existing rules equal to 'InitialRuleCount'
	And the CSV file details and status page should show Total rules 5 more than 'InitialRuleCount'
	When the user clicks the EU CHED-P reporting link at the bottom of the summary page
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed in a new browser tab
	When the user scrolls to the bottom of the CHED-P rules report page
	Then the count of CHED-P rules should be 5 more than 'InitialRuleCount'
	When the user enters '*' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                            |
		| Description                | ALL                                                              |
		| Commodity code             | *                                                                |
		| Rate %                     | 50                                                               |
		| Previous rate %            | 0                                                                |
		| Permanent                  | No                                                               |
		| End date                   | 23/10/2028                                                       |
		| Countries                  | ALL                                                              |
		| Country groups             | None                                                             |
		| Country exceptions         | Austria, Belgium, Bulgaria, Croatia, Cyprus, Czechia, Denmark    |
		| Purpose                    | All                                                              |
		| Border Control Post        | All                                                              |
		| Risk categorisation        | High                                                             |
		| Allow multiple inspections | Yes                                                              |
		| Reason                     | Reason 1                                                         |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Djibouti / 51021100 / Risk categorisation: High)
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Djibouti' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Djibouti' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '51021100' commodity code
	Then the commodity details should be populated '51021100' 'Of Kashmir (cashmere) goats'
	When the user selects species of commodity 'Capra spp.'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Internal market'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'High risk' risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'INV12345'
	And the user enters Latest Health Certificate date of issue from yesterday
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_1' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_1'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_1_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 50            |
		| Total         | 1             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	# Update bulk-update CSV with the new rule Id for commodity *
	When the user updates the bulk update CSV 'SPS-8833_CHED-P_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '*' to the recorded 'Iteration_1_RuleId'
	Then the bulk update CSV row for commodity code '*' should contain the recorded 'Iteration_1_RuleId'
	And 'Iteration_1' is complete
	# ---------- Iteration 1: Complete ----------
	# ---------- Iteration 2: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '51021100' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                                   |
		| Description                | Of Kashmir (cashmere) goats                                             |
		| Commodity code             | 51021100                                                                |
		| Rate %                     | 1                                                                       |
		| Previous rate %            | 0                                                                       |
		| Permanent                  | Yes                                                                     |
		| End date                   |                                                                         |
		| Countries                  | None                                                                    |
		| Country groups             | CHED-P EU Countries                                                     |
		| Country exceptions         | Hungary, Ireland, Romania                                               |
		| Purpose                    | Internal Market - Animal Feedingstuff, Internal Market - Human Consumption, Internal Market - Technical use |
		| Border Control Post        | All                                                                     |
		| Risk categorisation        | Medium                                                                  |
		| Allow multiple inspections | No                                                                      |
		| Reason                     |                                                                         |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (France / Internal Market - Technical use / 51021100 / Risk categorisation: Medium) - APP-A
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'France' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'France' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '51021100' commodity code
	Then the commodity details should be populated '51021100' 'Of Kashmir (cashmere) goats'
	When the user selects species of commodity 'Capra spp.'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Internal market: Animal Feedingstuff, Human Consumption, Technical use'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Medium risk' risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'INV12345'
	And the user enters Latest Health Certificate date of issue from yesterday
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_2A'
	# Submit a 2nd matching CHED-P notification in IPAFFS (France / Internal Market - Technical use / 51021100 / Risk categorisation: Medium) - APP-B
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'France' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'France' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '51021100' commodity code
	Then the commodity details should be populated '51021100' 'Of Kashmir (cashmere) goats'
	When the user selects species of commodity 'Capra spp.'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Internal market: Animal Feedingstuff, Human Consumption, Technical use'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Medium risk' risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'INV12345'
	And the user enters Latest Health Certificate date of issue from yesterday
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_2B'
	# Validate via Risk Decision Report - APP-A (Total=1, Triggered=0)
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_2A' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_2A'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 1             |
		| Total         | 1             |
		| Triggered     | 0             |
		| IsTriggered   | false         |
	# Validate via Risk Decision Report - APP-B (Total=2, Triggered=1)
	When the user enters the recorded CHED Reference for 'Iteration_2B' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_2B'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 1             |
		| Total         | 2             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	# Update bulk-update CSV with the new rule Id for commodity 51021100
	When the user updates the bulk update CSV 'SPS-8833_CHED-P_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '51021100' to the recorded 'Iteration_2_RuleId'
	Then the bulk update CSV row for commodity code '51021100' should contain the recorded 'Iteration_2_RuleId'
	And 'Iteration_2' is complete
	# ---------- Iteration 2: Complete ----------
	# ---------- Iteration 3: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '1603' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                                                              |
		| Description                | Extracts and juices of meat, fish or crustaceans, molluscs or other aquatic invertebrates           |
		| Commodity code             | 1603                                                                                               |
		| Rate %                     | 100                                                                                                |
		| Previous rate %            | 0                                                                                                  |
		| Permanent                  | Yes                                                                                                |
		| End date                   |                                                                                                    |
		| Countries                  | Djibouti                                                                                           |
		| Country groups             | None                                                                                               |
		| Country exceptions         | None                                                                                               |
		| Purpose                    | All                                                                                                |
		| Border Control Post        | All                                                                                                |
		| Risk categorisation        | Any                                                                                                |
		| Allow multiple inspections | Yes                                                                                                |
		| Reason                     | Reason 3                                                                                           |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Djibouti / 1603)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Djibouti' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Djibouti' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '1603' commodity code
	Then the commodity details should be populated '1603' 'Extracts and juices of meat, fish or crustaceans, molluscs or other aquatic invertebrates'
	When the user selects the type of commodity 'Composite products'
	When the user selects species of commodity 'Other'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Any'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Medium risk' risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'INV12345'
	And the user enters Latest Health Certificate date of issue from yesterday
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_3' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_3'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_3_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 100           |
		| Total         | 1             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	# Update bulk-update CSV with the new rule Id for commodity 1603
	When the user updates the bulk update CSV 'SPS-8833_CHED-P_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '1603' to the recorded 'Iteration_3_RuleId'
	Then the bulk update CSV row for commodity code '1603' should contain the recorded 'Iteration_3_RuleId'
	And 'Iteration_3' is complete
	# ---------- Iteration 3: Complete ----------
	# ---------- Iteration 4: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '31051000' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                                                                   |
		| Description                | Goods of this chapter in tablets or similar forms or in packages of a gross weight not exceeding 10kg   |
		| Commodity code             | 31051000                                                                                                |
		| Rate %                     | 0                                                                                                       |
		| Previous rate %            | 0                                                                                                       |
		| Permanent                  | Yes                                                                                                     |
		| End date                   |                                                                                                         |
		| Countries                  | Brazil, Canada, Chile, China, Ecuador, India, Thailand, Turkey, Ukraine                                 |
		| Country groups             | EU Member States                                                                                        |
		| Country exceptions         | None                                                                                                    |
		| Purpose                    | Transhipment / Onward travel, Transit                                                                   |
		| Border Control Post        | All                                                                                                     |
		| Risk categorisation        | Low                                                                                                     |
		| Allow multiple inspections | No                                                                                                      |
		| Reason                     |                                                                                                         |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Brazil / Transhipment / Onward travel / 31051000 / Risk categorisation: Low)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Brazil' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Brazil' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '31051000' commodity code
	Then the commodity details should be populated '31051000' 'Goods of this chapter in tablets or similar forms or in packages of a gross weight not exceeding 10kg'
	When the user selects the type of commodity 'processed manure'
	When the user selects species of commodity 'Aves'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Transit | Transhipment or onward travel'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Low risk' risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_4' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_4'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_4_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 0             |
		| Total         | 1             |
		| Triggered     | 0             |
		| IsTriggered   | false         |
	# Update bulk-update CSV with the new rule Id for commodity 31051000
	When the user updates the bulk update CSV 'SPS-8833_CHED-P_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '31051000' to the recorded 'Iteration_4_RuleId'
	Then the bulk update CSV row for commodity code '31051000' should contain the recorded 'Iteration_4_RuleId'
	And 'Iteration_4' is complete
	# ---------- Iteration 4: Complete ----------
	# ---------- Iteration 5: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '020130' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                                                                                          |
		| Description                | Boneless                                                                                                                       |
		| Commodity code             | 020130                                                                                                                         |
		| Rate %                     | 15                                                                                                                             |
		| Previous rate %            | 0                                                                                                                              |
		| Permanent                  | Yes                                                                                                                            |
		| End date                   |                                                                                                                                |
		| Countries                  | Australia, Canada, Japan, Singapore, South Africa, United States of America (the)                                              |
		| Country groups             | None                                                                                                                           |
		| Country exceptions         | None                                                                                                                           |
		| Purpose                    | Transit                                                                                                                        |
		| Border Control Post        | Dover Port - GBDOV1P, East Midlands Airport - GBEMA4, Grimsby and Immingham - GBGSY1, Hull - GBHUL1H, London Gateway - GBLGP1 |
		| Risk categorisation        | Any                                                                                                                            |
		| Allow multiple inspections | Yes                                                                                                                            |
		| Reason                     |                                                                                                                                |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Singapore / 020130 / Transit / London Gateway)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Singapore' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Singapore' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '020130' commodity code
	Then the commodity details should be populated '020130' 'Boneless'
	When the user selects species of commodity 'Bison bison'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Transit'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Low risk' risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_5' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_5'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_5_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 15            |
		| Total         | 1             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	# Update bulk-update CSV with the new rule Id for commodity 020130
	When the user updates the bulk update CSV 'SPS-8833_CHED-P_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '020130' to the recorded 'Iteration_5_RuleId'
	Then the bulk update CSV row for commodity code '020130' should contain the recorded 'Iteration_5_RuleId'
	And the user copies the updated bulk update CSV back to the source data directory
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------

Scenario: Bulk upload update existing rules for CHEDP - SPS-9420
	# ---------- Iteration 1: Start ----------
	# Upload the update rules
	Given that I navigate to the Risk Engine application
	When I have provided the Risk Engine admin credentials and signed in
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user scrolls to the bottom of the CHED-P rules report page
	Then the count of CHED-P rules is recorded as 'InitialRuleCount'
	When the user clicks the 'CHED-P' link from the Risk Engine header menu
	Then the CHED-P imports page should be displayed
	When the user clicks the Bulk upload commodity rules link on the CHED-P imports page
	Then the Upload multiple commodity rules using a CSV file page should be displayed
	When the user clicks the Choose file button
	And the user navigates to and selects the bulk upload file 'SPS-8833_CHED-P_BulkUploadRules_UpdatedRules.csv'
	Then the selected file name is displayed next to the Choose file button
	When the user clicks the Upload button on the bulk upload page
	Then the Check your file processing status page should be displayed
	When the user clicks the 'view the processing status of your file here' link
	Then the Submit multiple commodity rules using a CSV file page is displayed with the first record status 'Ready to submit'
	When the user clicks the Confirm and submit link for the first record in the list
	Then the 'Are the commodity rule changes correct?' page is displayed with the following upload details
		| Field                           | Value   |
		| CHED file type                  | CHED-P  |
		| Replace existing rules?         | No      |
		| Incoming rules                  | 5       |
		| Existing rule IDs to be deleted | 0       |
		| Number of rules to be updated   | 5       |
		| Incoming rules to be added      | 0       |
	And the 'Are the commodity rule changes correct?' page should show Existing rules equal to 'InitialRuleCount'
	And the 'Are the commodity rule changes correct?' page should show Total rules 0 more than 'InitialRuleCount'
	When the user selects the 'Yes, submit changes to the risk engine' radio option
	And the user clicks the Submit button on the rule changes page
	Then the Uploading rule changes to the risk engine page should be displayed
	When the user clicks the Check file status link
	Then the Submit multiple commodity rules using a CSV file page is displayed with the first record status 'Completed'
	When the user clicks the View summary link for the first record in the list
	Then the CSV file details and status page is displayed with the following upload details
		| Field                           | Value   |
		| CHED file type                  | CHED-P  |
		| Replace existing rules?         | No      |
		| Incoming rules                  | 5       |
		| Existing rule IDs to be deleted | 0       |
		| Number of rules to be updated   | 5       |
		| Incoming rules to be added      | 0       |
	And the CSV file details and status page should show Existing rules equal to 'InitialRuleCount'
	And the CSV file details and status page should show Total rules 0 more than 'InitialRuleCount'
	When the user clicks the EU CHED-P reporting link at the bottom of the summary page
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed in a new browser tab
	When the user scrolls to the bottom of the CHED-P rules report page
	Then the count of CHED-P rules should equal the recorded 'InitialRuleCount'
	When the user enters '*' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                            |
		| Description                | ALL                                                              |
		| Commodity code             | *                                                                |
		| Rate %                     | 50                                                               |
		| Previous rate %            | 0                                                                |
		| Permanent                  | No                                                               |
		| End date                   | 23/10/2027                                                       |
		| Countries                  | ALL                                                              |
		| Country groups             | None                                                             |
		| Country exceptions         | Austria, Belgium, Bulgaria, Croatia, Cyprus, Czechia, Denmark    |
		| Purpose                    | All                                                              |
		| Border Control Post        | All                                                              |
		| Risk categorisation        | High                                                             |
		| Allow multiple inspections | Yes                                                              |
		| Reason                     | Reason 1b                                                        |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Djibouti / 51021100 / Risk categorisation: High)
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Djibouti' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Djibouti' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '51021100' commodity code
	Then the commodity details should be populated '51021100' 'Of Kashmir (cashmere) goats'
	When the user selects species of commodity 'Capra spp.'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Internal market'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'High risk' risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'INV12345'
	And the user enters Latest Health Certificate date of issue from yesterday
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_1' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_1'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_1_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 50            |
		| Total         | 1             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	And 'Iteration_1' is complete
	# ---------- Iteration 1: Complete ----------
	# ---------- Iteration 2: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '51021100' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                            |
		| Description                | Of Kashmir (cashmere) goats                      |
		| Commodity code             | 51021100                                         |
		| Rate %                     | 1                                                |
		| Previous rate %            | 0                                                |
		| Permanent                  | Yes                                              |
		| End date                   |                                                  |
		| Countries                  | None                                             |
		| Country groups             | Third Countries, CHED-P EU Countries             |
		| Country exceptions         | Djibouti, Hungary, Ireland, Romania              |
		| Purpose                    | All                                              |
		| Border Control Post        | All                                              |
		| Risk categorisation        | Any                                              |
		| Allow multiple inspections | No                                               |
		| Reason                     | Reason 2                                         |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Zambia / 51021100) - APP-A
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Zambia' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Zambia' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '51021100' commodity code
	Then the commodity details should be populated '51021100' 'Of Kashmir (cashmere) goats'
	When the user selects species of commodity 'Capra spp.'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Any'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Low risk' risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_2A'
	# Submit a 2nd matching CHED-P notification in IPAFFS (Zambia / 51021100) - APP-B
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Zambia' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Zambia' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '51021100' commodity code
	Then the commodity details should be populated '51021100' 'Of Kashmir (cashmere) goats'
	When the user selects species of commodity 'Capra spp.'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Any'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Low risk' risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the CHED Reference for 'Iteration_2B'
	# Validate via Risk Decision Report - APP-A (Total=1, Triggered=0)
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_2A' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_2A'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 1             |
		| Total         | 1             |
		| Triggered     | 0             |
		| IsTriggered   | false         |
	# Validate via Risk Decision Report - APP-B (Total=2, Triggered=1)
	When the user enters the recorded CHED Reference for 'Iteration_2B' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_2B'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_2_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 1             |
		| Total         | 2             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	And 'Iteration_2' is complete
	# ---------- Iteration 2: Complete ----------
	# ---------- Iteration 3: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '1603' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                                                                                          |
		| Description                | Extracts and juices of meat, fish or crustaceans, molluscs or other aquatic invertebrates                                       |
		| Commodity code             | 1603                                                                                                                           |
		| Rate %                     | 100                                                                                                                            |
		| Previous rate %            | 0                                                                                                                              |
		| Permanent                  | Yes                                                                                                                            |
		| End date                   |                                                                                                                                |
		| Countries                  | Djibouti, Togo                                                                                                                 |
		| Country groups             | None                                                                                                                           |
		| Country exceptions         | None                                                                                                                           |
		| Purpose                    | All                                                                                                                            |
		| Border Control Post        | Dover Port - GBDOV1P, East Midlands Airport - GBEMA4, Grimsby and Immingham - GBGSY1, Hull - GBHUL1H, London Gateway - GBLGP1 |
		| Risk categorisation        | Any                                                                                                                            |
		| Allow multiple inspections | Yes                                                                                                                            |
		| Reason                     |                                                                                                                                |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Togo / 1603 / London Gateway)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Togo' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Togo' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '1603' commodity code
	Then the commodity details should be populated '1603' 'Extracts and juices of meat, fish or crustaceans, molluscs or other aquatic invertebrates'
	When the user selects the type of commodity 'Composite products'
	When the user selects species of commodity 'Other'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Any'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Low risk' risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_3' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_3'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_3_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 100           |
		| Total         | 1             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	And 'Iteration_3' is complete
	# ---------- Iteration 3: Complete ----------
	# ---------- Iteration 4: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '31051000' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                                                                   |
		| Description                | Goods of this chapter in tablets or similar forms or in packages of a gross weight not exceeding 10kg   |
		| Commodity code             | 31051000                                                                                                |
		| Rate %                     | 100                                                                                                     |
		| Previous rate %            | 0                                                                                                       |
		| Permanent                  | Yes                                                                                                     |
		| End date                   |                                                                                                         |
		| Countries                  | Brazil, Canada, Chile, China, Ecuador, India, Thailand, Turkey, Ukraine                                 |
		| Country groups             | EU Member States                                                                                        |
		| Country exceptions         | None                                                                                                    |
		| Purpose                    | Transhipment / Onward travel, Transit                                                                   |
		| Border Control Post        | All                                                                                                     |
		| Risk categorisation        | Low                                                                                                     |
		| Allow multiple inspections | No                                                                                                      |
		| Reason                     |                                                                                                         |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Brazil / Transhipment or Onward travel OR Transit / 31051000 / Risk categorisation: Low)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Brazil' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Brazil' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '31051000' commodity code
	Then the commodity details should be populated '31051000' 'Goods of this chapter in tablets or similar forms or in packages of a gross weight not exceeding 10kg'
	When the user selects the type of commodity 'processed manure'
	When the user selects species of commodity 'Aves'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Transit | Transhipment or onward travel'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Low risk' risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	When the user populates the transport details 'BRISTOL (GBBRS)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_4' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_4'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_4_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 100           |
		| Total         | 1             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	And 'Iteration_4' is complete
	# ---------- Iteration 4: Complete ----------
	# ---------- Iteration 5: Start ----------
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user enters '020130' in the CHED-P rules search field
	Then the top CHED-P rule row should match the following details
		| Field                      | Value                                                                                                                          |
		| Description                | Boneless                                                                                                                       |
		| Commodity code             | 020130                                                                                                                         |
		| Rate %                     | 15                                                                                                                             |
		| Previous rate %            | 0                                                                                                                              |
		| Permanent                  | Yes                                                                                                                            |
		| End date                   |                                                                                                                                |
		| Countries                  | Australia                                                                                                                      |
		| Country groups             | None                                                                                                                           |
		| Country exceptions         | None                                                                                                                           |
		| Purpose                    | Transit                                                                                                                        |
		| Border Control Post        | Dover Port - GBDOV1P, East Midlands Airport - GBEMA4, Grimsby and Immingham - GBGSY1, Hull - GBHUL1H, London Gateway - GBLGP1 |
		| Risk categorisation        | Any                                                                                                                            |
		| Allow multiple inspections | Yes                                                                                                                            |
		| Reason                     |                                                                                                                                |
	And the top CHED-P rule row should have Start date as today's date
	And the user records the Id of the top CHED-P rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-P notification in IPAFFS (Australia / 020130 / Transit / London Gateway)
	When I navigate to the IPAFF application
	Then the Your import notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'Australia' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Australia' as the Country of origin and Country from where consigned 
	When the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '020130' commodity code
	Then the commodity details should be populated '020130' 'Boneless'
	When the user selects species of commodity 'Bison bison'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed
	When the user randomly selects a purpose from 'Transit'
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses 'Low risk' risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
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
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference for 'Iteration_5' in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference of 'Iteration_5'
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'Iteration_5_RuleId' with the following values
		| Field         | Value         |
		| RuleType      | CommodityRule |
		| RegulatorType | EUImport      |
		| Rate          | 15            |
		| Total         | 1             |
		| Triggered     | 1             |
		| IsTriggered   | true          |
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------
	# Delete the 5 rules created by the update bulk upload
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-P reports link
	Then the CHED-P reports page should be displayed
	When the user clicks the CHED-P imports commodity rules report link
	Then the View all CHED-P (Import) Commodity Rules report page should be displayed
	When the user clicks the Remove rule link for CHED-P rule Id recorded as 'Iteration_1_RuleId'
	Then the CHED-P rule Id recorded as 'Iteration_1_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-P rule Id recorded as 'Iteration_2_RuleId'
	Then the CHED-P rule Id recorded as 'Iteration_2_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-P rule Id recorded as 'Iteration_3_RuleId'
	Then the CHED-P rule Id recorded as 'Iteration_3_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-P rule Id recorded as 'Iteration_4_RuleId'
	Then the CHED-P rule Id recorded as 'Iteration_4_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-P rule Id recorded as 'Iteration_5_RuleId'
	Then the CHED-P rule Id recorded as 'Iteration_5_RuleId' should no longer be present in the rules table
	When the user scrolls to the bottom of the CHED-P rules report page
	Then the count of CHED-P rules should be 5 less than 'InitialRuleCount'