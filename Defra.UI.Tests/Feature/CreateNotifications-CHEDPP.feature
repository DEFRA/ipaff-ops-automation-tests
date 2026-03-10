@Regression
Feature: Create Notification CHEDPP

Create a notification for CHEDPP type

Scenario: Delegation of Authority Agent submits CHEDPP notification on behalf of trader and status becomes valid after auto clear SLA - SPS - 7394
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Agent credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then About the consignment - Who are you creating this notification for? page should be displayed
	When the user selects 'A different organisation' option in about the consignment page
	And the user clicks Save and continue
	Then About the consignment - Which company is this notification for page should be displayed
	When the user selects company name as 'Trader 5'
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Manual entry' option to add commodity details
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Commodity code search tab
	And the user searches for first commodity code '12092980'
	Then the CHED PP commodity details should be populated '12092980' 'Other' for first commodity
	When the user searchs for EPPO code '1AIEG' and clicks add link
	Then Genus(and Species) 'x Aliceara' and EPPO code '1AIEG' should be populated in commodity page
	And the user clicks Save and continue
	And What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	And the user clicks the Add commodity link
	And the user clicks the 'EDIBLE FRUIT AND NUTS; PEEL OF CITRUS FRUIT OR MELONS' in the parent commodity tree
	And the sub commodity list expands
	And the user clicks '0810' 'Other fruit, fresh' under the parent commodity
	And the sub commodity list expands
	And the user selects the second commodity '08105000' 'Kiwifruit' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects EPPO code 'ATICH' checkbox
	And the user clicks Save and continue
	Then the Description of the goods Variety and class of commodity should be displayed
	When the user selects 'Hayward (Yellow flesh)' variety of EPPO code 'ATICH'
	And the user select 'Class I' class of EPPO code 'ATICH'
	And the user clicks Save and continue
	Then the selected commodities 'Other' 'Kiwifruit' should be displayed in commodity page with code '12092980' '08105000' EPPO code '1AIEG' 'ATICH' and Genus 'x Aliceara' 'Actinidia chinensis'
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user selects check boxes for the commodity codes '12092980' '08105000'
	And the user populates Net weight as '100' for CHED PP commodity
	And the user populates Number of packages as '10' for CHED PP commodity
	And the user selects type of package as 'Box' for CHED PP commodity
	And the user populates Quantity as '10' for CHED PP commodity
	And the user selects Quantity type as 'Kilograms' for CHED PP commodity
	Then the user clicks Apply Button
	And the user clicks Save and continue
	And the Additional details page should be displayed
	And the total gross weight should be greater than the net weight '300'
	When the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' '123456' 'No' 'Doc1234'
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
	When the user selects Document type "Phytosanitary certificate"
	And the user enters Document reference "INV12345"
	And the user selects a previous date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'Trader 5' is pre-filled
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
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	And the notification status should be 'NEW TRADE PARTNER'
	When the users opens a new tab
	And the user navigate to the IPAFFS Internal Plants Inspector application
	And I have provided the IPAFFS Internal Plants Inspector credentials and signin
	Then the user should be logged into Notification page
	When user searches for the import notification
	Then the notification should be found with the status "NEW"
	When the user waits and searches for the notification should be found with the status "IN PROGRESS"
	And the user switches to the part 1 tab
	And the user searches for the import notification
	Then the notification should be found with the status "IN PROGRESS"
	When the user waits and searches for the notification should be found with the status "VALID"
	And the user switches to the part 2 tab
	And the user clicks Record decision from the header
	And the user searches for the CHED number
	Then the user clicks the notification found with status "VALID"
	And the CHED overview page should be displayed
	When the user switches to 'Checks' tab in CHED Overview page
	Then verifies Risk decision PHSI is set to 'NO INSPECTION'
	And verifies Document check is set to 'AUTO CLEARED'
	And verifies Risk decision HMI is set to 'NO INSPECTION'
	And verifies 'Decision recorded by' field is set to 'Auto Cleared'
	When the user logs out of IPAFFS Part 2
	And the user closes the newly opened tab
	Then the browser tab is closed
	When the user switches to the part 1 tab
	And the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully

Scenario: Delegation of Authority Agent submits CHEDPP notification by uploading commodity details in CSV file - SPS - 5092
	Given that I navigate to the IPAFF application
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
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then Description of the goods How do you want to add your commodity details page should be displayed
	When the user selects 'Upload from a CSV file' option to add commodity details
	And the user clicks Save and continue
	Then Upload commodity details using a CSV file page should be displayed
	When the user clicks Download the CSV template and read the guide on how to complete this (opens in new tab)
	Then the guidance to upload commodity details using a CSV file page will open in new tab
	When user downloads the template by clicking the link 'IPAFFS commodity details CSV template (xlsx format)'
	And verifies the document 'IPAFFS-commodity-details-CSV-template-xls' downloaded successfully
	Then validates the document 'IPAFFS-commodity-details-CSV-template-xls.xlsx' should have column headers 'Commodity code' 'Genus and Species' 'EPPO code' 'Variety' 'Class' 'Intended for final users (or commercial flower production)' 'Number of packages' 'Type of package' 'Quantity' 'Quantity type' 'Net weight (kg)' 'Controlled atmosphere container'
	When the user closes the newly opened tab
	Then the browser tab is closed
	When the user creates a CSV file from the downloaded template 'IPAFFS-commodity-details-CSV-template-xls.xlsx' with below data and uploads the CSV file
		| Commodity code | Genus and Species   | EPPO code | Variety                | Class       | Intended for final users (or commercial flower production) | Number of packages | Type of package | Quantity | Quantity type | Net weight (kg) | Controlled atmosphere container |
		|       12092980 | x Aliceara          | 1AIEG     |                        |             |                                                            |                 10 | BX              |       10 | KGM           |             100 |                                 |
		|       08105000 | Actinidia chinensis | ATICH     | Hayward (Yellow flesh) | Class I     |                                                            |                 10 | CS              |       20 | KGM           |           200.0 |                                 |
		|       08105000 | Actinidia chinensis | ATICH     | Jintao (Yellow flesh)  | Class I     |                                                            |                 10 | BG              |       20 | BLB           |          200.00 | Yes                             |
		|       08105000 | Actinidia chinensis | ATICH     | None (Yellow flesh)    | Class I     |                                                            |                 10 | BL              |       20 | KGM           |         200.000 |                                 |
		|       08105000 | Actinidia chinensis | ATICH     | Hayward (Yellow flesh) | Class II    |                                                            |                 10 | VR              |       30 | CRZ           |          300.05 |                                 |
		|       08105000 | Actinidia chinensis | ATICH     | Jintao (Yellow flesh)  | Class II    |                                                            |                 10 | CA              |       30 | PCS           |           300.1 | Yes                             |
		|       08105000 | Actinidia chinensis | ATICH     | None (Yellow flesh)    | Class II    |                                                            |                 10 | CT              |       30 | PTC           |         300.324 |                                 |
		|       08105000 | Actinidia chinensis | ATICH     | Hayward (Yellow flesh) | Extra Class |                                                            |                 10 | PK              |       40 | SDS           |         400.001 |                                 |
		|       08105000 | Actinidia chinensis | ATICH     | Jintao (Yellow flesh)  | Extra Class |                                                            |                 10 | PX              |       40 | STM           |          400.53 | No                              |
		|       08105000 | Actinidia chinensis | ATICH     | None (Yellow flesh)    | Extra Class |                                                            |                 10 | PU              |       40 | KGM           |             400 |                                 |
		|       08106000 | Durio zibethinus    | DURZI     |                        |             |                                                            |                 10 | BE              |       40 | CRZ           |             400 | No                              |
	Then CSV file should be uploaded successfully with 'Upload successful' message and validate the count of commodity
	And all the displayed commodity data in all tables should be validated with the values given in the input
	And checks the information message heading 'If details are incorrect or missing' and content 'You will need to upload commodity details CSV file again. This will replace your previous upload.'
	When the user clicks Confirm and continue button
	Then What is the main reason for importing the consignment? page should be displayed
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then Check uploaded commodity details page should be displayed
	And all the displayed commodity data in all tables should be validated with the values given in the input
	And checks the information message heading 'If details are incorrect or missing' and content 'You will need to upload commodity details CSV file again. This will replace your previous upload.'
	When the user clicks Confirm and continue button
	Then the Additional details page should be displayed
	And user enters the total gross weight greater than the net weight '3500'
	And the user clicks Save and continue
	Then Confirmation to declare GMS page should be loaded
	When the user selects 'Yes' confirmation option
	And the user clicks Save and continue
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'London Gateway - GBLGP1PP' 'London Gateway' 'Road vehicle' '123456' 'No' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "Yes – add MRN now" for Are you using the Common Transit Convention (CTC)?
	And the user can provide Movement Reference Number as '24GB123456789AB013'
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Phytosanitary certificate"
	And the user enters Document reference "INV12345"
	And the user selects a previous date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And Importer, Packer, Delivery address and Consignor page should be displayed
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
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	And the dashboard page should be displayed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
		
Scenario: Create a new import notification through clone a health or phytosanitary certificate process - SPS-9272 - CHED PP
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Agent credentials and signin
	Then the user should be logged into Notification page
	When the user Clicks on Clone a certificate button
	Then the Clone a health or phytosanitary certificate page should be displayed
	And the user verifies all the content in Clone a health or phytosanitary certificate page
	And the user selected the importing option as 'Plants, plant products and other objects'
	When the user clicks on continue button
	Then the Certificate details page should be displayed
	And the user searches for the notification for cloning which is not more than 90 days from creation
	And the user provided notification details in the search input fields
	When the user Clicks on Search button
	Then the Phytosanitary certificate details page should be displayed
	And the user verified all the details on Phytosanitary certificate details page
	When the user Clicks on Clone button
	Then the Who are you creating this notification for page should be displayed
	And the user selects the option of creating notification for as "Agent1"
	When the user Clicks on Save and review button
	Then the DRAFT CHEDPP notification code page should be displayed
	And the user records the Draft notification number
	And the user verifies the following information is displayed within a red outlined box
		| MissingOrIncorrect                                                         |
		| Add the estimated arrival date at BCP                                      |
		| Add the estimated arrival time at BCP                                      |
		| Enter missing commodity details                                            |
		| Add the total gross weight                                                 |
		| Check your details on the 'Contact details' page and save them to continue |
		| Select if using the Goods Vehicle Movement Service (GVMS)                  |
		| Add document details                                                       |
		| Add a delivery address                                                     |
		| Add the entry Border Control Post                                          |
		| Select if using the Common Transit Convention (CTC)                        |
	When the user clicks on Check or update commodity details link
	Then the Add intended use of bulbs page should be displayed
	And the user selects the Commodity from the list appeared
	And the user selects "Yes" for Are the commodity lines you selected intended for final users or commercial flower production?
	When the user clicks on Apply button
	Then the user can see the success message "1 commodity line has been updated"
	When the user clicks on Save and continue button
	Then the Check or update commodity details page should be displayed
	When the user clicks on Save and continue button
	Then the Additional details page should be displayed
	And the user verifies Total gross volume is displayed but it is marked as optional with the value of "Total gross volume (optional)"
	And the user enter Total Gross Weight as "1100"
	When the user Clicks on Save and review button from Additional details page
	Then the Review your notification page should be displayed
	When the user Clicks on Change link for Transport to the Border Control Post
	Then Transport to the Border Control Post (BCP) page should be dislayed
	When the user populates the transport to the BCP details 'Heathrow Airport - GBLHR4PP' 'Eurobip' 'Road vehicle' 'YY10 KTP' 'No' 'Doc23456'
	And the user Clicks on Save and review button from Border Control Post page
	Then the Review your notification page should be displayed
	When the user Clicks on Change link for Contact details
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user Clicks on Save and review button from Contact details page
	Then the Review your notification page should be displayed
	When the user Clicks on Change link for Goods movement services
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user Clicks on Save and review button from Goods movement services page
	Then the Review your notification page should be displayed
	When the user Clicks on Change link for Add a delivery address
	Then Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'Agent 1' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address "DEFRA"
	Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	When the user Clicks on Save and review button from Importer, Packer, Delivery address and Consignor page
	Then the Review your notification page should be displayed
	And the user verifies the following information is displayed within a red outlined box
		| MissingOrIncorrect |
	When the user clicks on Save and continue button
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	And the notification returned in the search has the status 'NEW'
	When the user clicks View details for the notification
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED PP
	When the user clicks View CHED grey button
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user clicks on the Dashboard link
	Then the dashboard page should be displayed

Scenario: Agent submits CHEDPP notification for Trader after being delegated authority - SPS-7364
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Trader 1 credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Manage trade partners
	Then the Are you a plants importer or agency? page should be displayed
	When the user selects Yes and clicks Continue
	Then the Manage your authorisations page should be displayed
	And the business name should be displayed as the page header for 'Trader 1'
	And the change settings link should be displayed
	And the Businesses you are authorised to represent header should be displayed
	And the agent code and helper text should be displayed for 'Trader 1'
	And the Automatically accept delegation requests from Importers/Exporters should be toggled to Yes
	And the Companies section should be displayed with no permissions message
	When the user clicks the change settings link
	Then the Change organisation settings page should be displayed
	And the instruction text should be displayed on the Change organisation settings page
	And the Select all that apply hint should be displayed
	And the 'I want to authorise an agent to act for my business' checkbox should be displayed
	And the 'I am an agent who wants authority to act on behalf of other businesses' checkbox should be displayed
	And the agent declaration text should be displayed
	And the 'I confirm that I have read and accepted the above statement/s.' checkbox should be displayed
	And the warning message should be displayed on the Change organisation settings page
	And the Save button should be displayed on the Change organisation settings page
	When the user ticks 'I want to authorise an agent to act for my business' checkbox
	And the user unticks 'I am an agent who wants authority to act on behalf of other businesses' checkbox
	And the user ticks 'I confirm that I have read and accepted the above statement/s.' checkbox
	And the user clicks Save on the Change organisation settings page
	Then the Change organisation settings page should be displayed with 'Settings saved'
	When the user clicks Continue on the Change organisation settings page
	Then the Manage your authorisations page should be displayed
	And the business name should be displayed as the page header for 'Trader 1'
	And the change settings link should be displayed
	And the Agents acting on your behalf header should be displayed
	And the no agents authorised message should be displayed
	And the Add an agent button should be displayed
	When the user clicks Add an agent
	Then the Add an agent page should be displayed
	When the user enters 'Agent 1' agent code 
	And the user clicks Save and continue
	Then the Add an agent page should be displayed
	When the user clicks Yes for Is this the agent?
	And the user ticks the Confirm delegation checkbox
	And the user clicks Save and continue
	Then the Set permissions page should be displayed
	When the user toggles all permissions to Yes and clicks Finish
	Then the Manage your authorisations page should be displayed
	#And the Agent 1 name should be listed under Agents acting on your behalf
	#When the user clicks Add an agent
	#Then the Add an agent page should be displayed
	#When the user enters Agent 2 agent code and clicks Save and continue
	#Then the Add an agent page should be displayed
	#When the user clicks Yes for Is this the agent, ticks the delegation checkbox and clicks Save and continue
	#Then the Set permissions page should be displayed
	#When the user toggles all permissions to Yes and clicks Finish
	#Then the Manage your authorisations page should be displayed
	#And the Agent 2 name should be listed under Agents acting on your behalf
	#When the user clicks the Back link
	#Then the dashboard page should be displayed
	#When the user logs out of IPAFFS Part 1
	#Then the user should be logged out successfully
	#Given that I navigate to the IPAFF application
	#Then I should see type of Gateway login page
	#And I have selected "Sign in with Government Gateway" as login type
	#When I click Continue button from How do you want to sign in page
	#Then I should redirected to the IPAFF Sign in using Government Gateway page
	#When I have provided the IPAFF Agent 2 credentials and signin
	#Then the user should be logged into Notification page
	#When the user clicks Manage trade partners
	#Then the Are you a plants importer or agency? page should be displayed
	#When the user selects 'Yes' and clicks Continue
	#Then the Manage your authorisations page should be displayed
	#And Trader 1 and Trader 2 should be listed as companies
	#When the user clicks the Back link
	#Then the dashboard page should be displayed
	#When the user logs out of IPAFFS Part 1
	#Then the user should be logged out successfully
	#Given that I navigate to the IPAFF application
	#Then I should see type of Gateway login page
	#And I have selected "Sign in with Government Gateway" as login type
	#When I click Continue button from How do you want to sign in page
	#Then I should redirected to the IPAFF Sign in using Government Gateway page
	#When I have provided the IPAFF Agent 2 credentials and signin
	#Then the user should be logged into Notification page
	#When the user clicks Create a new notification
	#Then the About the consignment/What are you importing? page should be displayed with radio buttons
	#When the user chooses 'Plants, plant products and other objects' option
	#And the user clicks Save and continue
	#Then About the consignment - Who are you creating this notification for? page should be displayed
	#When the user selects the Trader 1 radio button option
	#And the user clicks Save and continue
	#Then the Origin of the plants plant product or other objects page should be displayed
	#When user searches for the import notification
	#Then the notification should be present in the list
	#And the notification status should be 'TRADE PARTNER'