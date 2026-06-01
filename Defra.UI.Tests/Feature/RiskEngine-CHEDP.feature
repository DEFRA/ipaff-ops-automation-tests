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
	# TODO: IPAFFS notification submission steps to be added
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
	# TODO: IPAFFS notification submission steps to be added
	And the user records the CHED Reference for 'Iteration_2A'
	# Submit a 2nd matching CHED-P notification in IPAFFS (France / Internal Market - Technical use / 51021100 / Risk categorisation: Medium) - APP-B
	# TODO: IPAFFS notification submission steps to be added
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
	# TODO: IPAFFS notification submission steps to be added
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
	# TODO: IPAFFS notification submission steps to be added
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
	# TODO: IPAFFS notification submission steps to be added
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
	# TODO: IPAFFS notification submission steps to be added
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
	# Submit a matching CHED-P notification in IPAFFS (Portugal / Transit / 51021100 / Risk categorisation: Low) - APP-A
	# TODO: IPAFFS notification submission steps to be added
	And the user records the CHED Reference for 'Iteration_2A'
	# Submit a 2nd matching CHED-P notification in IPAFFS (Portugal / Transit / 51021100 / Risk categorisation: Low) - APP-B
	# TODO: IPAFFS notification submission steps to be added
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
	# TODO: IPAFFS notification submission steps to be added
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
	# Submit a matching CHED-P notification in IPAFFS (Brazil / Transhipment / Onward travel / 31051000 / Risk categorisation: Low)
	# TODO: IPAFFS notification submission steps to be added
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
	# TODO: IPAFFS notification submission steps to be added
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