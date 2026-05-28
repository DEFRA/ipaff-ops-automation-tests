@RiskEngine
Feature: Risk Engine CHEDD

Bulk upload, Update and Test rules with end-to-end validation for a CHEDD notification

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
	When the user scrolls to the bottom of the CHED-D rules report page
	Then the count of CHED-D rules is recorded as 'InitialRuleCount'
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
	When the user scrolls to the bottom of the CHED-D rules report page
	Then the count of CHED-D rules should be 5 more than 'InitialRuleCount'
	When the user enters '0910' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Democratic Republic of the Congo / 0910 / Non-Internal Market)
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
	When the user enters '1006' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
		| Field               | Value                 |
		| Description         | Rice                  |
		| Commodity code      | 1006                  |
		| Rate %              | 0                     |
		| Previous rate %     | 0                     |
		| Permanent           | Yes                   |
		| End date            |                       |
		| Countries           | Montserrat            |
		| Country groups      | Euro-Mediterranean Area |
		| Country exceptions  | None                  |
		| Purpose             | All                   |
		| Border Control Post | All                   |
		| Reason              |                       |
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Montserrat / 1006)
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
	When the user enters '13023290' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
		| Field               | Value                                    |
		| Description         | Of guar seeds                            |
		| Commodity code      | 13023290                                 |
		| Rate %              | 75                                       |
		| Previous rate %     | 0                                        |
		| Permanent           | Yes                                      |
		| End date            |                                          |
		| Countries           | Afghanistan, Zimbabwe                    |
		| Country groups      | None                                     |
		| Country exceptions  | None                                     |
		| Purpose             | All                                      |
		| Border Control Post | Felixstowe - GBFXT1, Manchester airport - GBMNC1 |
		| Reason              | Reason 3                                 |
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Afghanistan / 13023290 / Felixstowe)
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
	When the user enters '2008191930' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Mexico / 2008191930)
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
	When the user enters '*' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Togo / 200850 / Internal Market)
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
	#And the user copies the updated bulk update CSV back to the source data directory
	And 'Iteration_5' is complete
	# ---------- Iteration 5: Complete ----------

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
	When the user scrolls to the bottom of the CHED-D rules report page
	Then the count of CHED-D rules is recorded as 'InitialRuleCount'
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
	Then the There are rules in your CSV file that already exist page should be displayed for CHED-D
	When the user selects Yes to replace existing rules and clicks Continue for CHED-D
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
	When the user scrolls to the bottom of the CHED-D rules report page
	Then the count of CHED-D rules should equal the recorded 'InitialRuleCount'
	When the user enters '0910' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_1_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Democratic Republic of the Congo / 0910 / Non-Internal Market)
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
	When the user enters '1006' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_2_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Montserrat / 1006 / Internal Market)
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
	When the user enters '13023290' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_3_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Afghanistan / 13023290 / Manchester airport)
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
	When the user enters '2008191930' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_4_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Mexico / 2008191930)
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
	When the user enters '*' in the CHED-D rules search field
	Then the top CHED-D rule row should match the following details
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
	And the top CHED-D rule row should have Start date as today's date
	And the user records the Id of the top CHED-D rule row as 'Iteration_5_RuleId'
	# Submit a matching CHED-D notification in IPAFFS (Togo / 200850 / Internal Market)
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
	When the user clicks the Remove rule link for CHED-D rule Id recorded as 'Iteration_1_RuleId'
	Then the CHED-D rule Id recorded as 'Iteration_1_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D rule Id recorded as 'Iteration_2_RuleId'
	Then the CHED-D rule Id recorded as 'Iteration_2_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D rule Id recorded as 'Iteration_3_RuleId'
	Then the CHED-D rule Id recorded as 'Iteration_3_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D rule Id recorded as 'Iteration_4_RuleId'
	Then the CHED-D rule Id recorded as 'Iteration_4_RuleId' should no longer be present in the rules table
	When the user clicks the Remove rule link for CHED-D rule Id recorded as 'Iteration_5_RuleId'
	Then the CHED-D rule Id recorded as 'Iteration_5_RuleId' should no longer be present in the rules table
	When the user scrolls to the bottom of the CHED-D rules report page
	Then the count of CHED-D rules should be 5 less than 'InitialRuleCount'