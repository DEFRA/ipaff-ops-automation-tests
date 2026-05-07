@RiskEngine
Feature: Risk Engine

Bulk upload PHSI commodity rules and end-to-end validation via IPAFFS notification

Scenario: SPS-9414 - Bulk Upload CHED-PP - Initial Load - Iteration 1
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
		| Field                  | Value                                         |
		| Commodity code         | 07020099                                      |
		| Intended Use           | None                                          |
		| Type                   | None                                          |
		| Rate %                 | 25                                            |
		| Previous rate %        | 0                                             |
		| Permanent              | Yes                                           |
		| Countries              | Djibouti                                      |
		| Country groups         | None                                          |
		| Country exceptions     | None                                          |
		| Document check aligned | No                                            |
		| Reason                 | Reason 1                                      |
	And the user records the Id of the top PHSI rule row as 'NewRuleId'
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
	And the data presented for review matches the data entered into the notification for CHED PP
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	# Validate via Risk Decision Report
	When I navigate to the Risk Engine application
	Then the Risk Engine Home page should be displayed	
	When the user clicks the 'Reports' link from the Risk Engine header menu
	Then the Risk Engine Reports page should be displayed
	When the user clicks the CHED-PP reports link
	Then the CHED-PP reports page should be displayed
	When the user clicks the Risk decision report link
	Then the Risk decision report page should be displayed
	When the user enters the recorded CHED Reference in the Risk decision search box and clicks Search
	Then the Risk decision report returns one matching record
	When the user clicks the Expand button for the CHED Reference
	And the user clicks the Requests details link
	Then the Requests section is expanded with details from IPAFFS
	When the user clicks the Decision details link
	Then the Decision section contains a DecisionRule matching the recorded 'NewRuleId' with the following values
		| Field          | Value         |
		| RuleType       | CommodityRule |
		| RegulatorType  | PHSIImport    |
		| Rate           | 25            |
		| Total          | 1             |
		| Triggered      | 1             |
		| IsTriggered    | true          |
	# Update bulk-update CSV with the new rule Id for commodity '07020099
	When the user updates the bulk update CSV 'SPS-8834_CHED-PP_BulkUploadRules_UpdatedRules.csv' setting the Id for commodity code '07020099' to the recorded 'NewRuleId'
	Then the bulk update CSV row for commodity code '07020099' should contain the recorded 'NewRuleId'
	# ---------- Iteration 1: Complete ----------