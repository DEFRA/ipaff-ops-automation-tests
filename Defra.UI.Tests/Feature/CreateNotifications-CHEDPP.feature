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
	And Confirmation to declare GMS page should be loaded
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
	
Scenario: Create a new import notification through clone a health or phytosanitary certificate process - SPS-9272
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
	And the user searches for the notification for cloning CHEDPP which is not more than 90 days from creation
	And the user provided CHED PP notification details in the search input fields
	When the user Clicks on Search button
	Then the Phytosanitary certificate details page should be displayed
	And the user verified all the details on Phytosanitary certificate details page
	When the user Clicks on Clone button
	Then the Who are you creating this notification for page should be displayed
	And the user selects the option of creating notification for as "Agent1"
	When the user Clicks on Save and review button
	Then the DRAFT 'CHEDPP' notification code page should be displayed
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
	When I have provided the IPAFF 'Trader 1' credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Manage trade partners
	Then the Are you a plants importer or agency? page should be displayed
	When the user selects Yes and clicks Continue
	Then the Manage your authorisations page should be displayed
	When the user clicks the change settings link
	Then the Change organisation settings page should be displayed
	When the user unticks 'I want to authorise an agent to act for my business' checkbox
	And the user ticks 'I am an agent who wants authority to act on behalf of other businesses' checkbox
	And the user ticks 'I confirm that I have read and accepted the above statement/s.' checkbox
	And the user clicks Save on the Change organisation settings page
	Then the Change organisation settings page should be displayed with 'Settings saved'
	When the user clicks Continue on the Change organisation settings page
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
	And the 'Agent 1' name should be listed under Agents acting on your behalf
	When the user clicks Add an agent
	Then the Add an agent page should be displayed
	When the user enters 'Agent 2' agent code
	And the user clicks Save and continue
	Then the Add an agent page should be displayed
	When the user clicks Yes for Is this the agent?
	And the user ticks the Confirm delegation checkbox
	And the user clicks Save and continue
	Then the Set permissions page should be displayed
	When the user toggles all permissions to Yes and clicks Finish
	Then the Manage your authorisations page should be displayed
	And the 'Agent 2' name should be listed under Agents acting on your behalf
	When the user clicks on the Back link above the Manage your authorisations header
	Then the dashboard page should be displayed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF 'Agent 2' credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Manage trade partners
	Then the Are you a plants importer or agency? page should be displayed
	When the user selects Yes and clicks Continue
	Then the Manage your authorisations page should be displayed
	And the business name should be displayed as the page header for 'Agent 2'
	And the change settings link should be displayed
	And the Businesses you are authorised to represent header should be displayed
	And the agent code and helper text should be displayed for 'Agent 2'
	And the Automatically accept delegation requests from Importers/Exporters should be toggled to Yes
	And the Companies section should be displayed with permissions message
	And 'Trader 1' and 'Trader 2' should be listed as companies
	When the user clicks on the Back link above the Manage your authorisations header
	Then the dashboard page should be displayed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF 'Agent 2' credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Plants, plant products and other objects' option
	And the user clicks Save and continue
	Then About the consignment - Who are you creating this notification for? page should be displayed
	When the user selects 'A different organisation' option in about the consignment page
	And the user clicks Save and continue
	Then About the consignment - Which company is this notification for page should be displayed
	When the user waits upto 10 minutes to select the 'Trader 1' radio button option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses 'France' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'France' as the Country of origin and Country from where consigned
	When the user enters a reference number '12345' in the Add a reference number for this consignment (optional) field
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
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Phytosanitary certificate'
	And the user enters Document reference 'INV12345'
	And the user selects a previous date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And Importer, Packer, Delivery address and Consignor page should be displayed
	When the user verifies Importer details 'Trader 1' is pre-filled
	And the user clicks Add a delivery address link
	Then Search for an existing delivery address page should be displayed
	When the user selects one of the displayed delivery address 'Manshead Court'
	Then the chosen delivery address 'Manshead Court' should be displayed on the Traders page
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'Isle of Anglesey'
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
	And the notification status should include 'TRADE PARTNER'

	
Scenario: SPS-9104
	#Given that I navigate to the IPAFF application
	#Then I should see type of Gateway login page
	#And I have selected "Sign in with Government Gateway" as login type
	#When I click Continue button from How do you want to sign in page
	#Then I should redirected to the IPAFF Sign in using Government Gateway page
	#When I have provided the IPAFF Trader credentials and signin
	#Then the user should be logged into Notification page
	#When the user clicks Create a new notification
	#Then the About the consignment/What are you importing? page should be displayed with radio buttons
	#When the user chooses 'Plants, plant products and other objects' option
	#And the user clicks Save and continue
	#Then the Origin of the plants plant product or other objects page should be displayed
	#When the user chooses "France" from the dropdown for Country of origin
	#And the user clicks Save and continue
	#Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	#When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	#And the user clicks Save and continue
	#Then Description of the goods How do you want to add your commodity details page should be displayed
	#When the user selects 'Upload from a CSV file' option to add commodity details
	#And the user clicks Save and continue
	#Then Upload commodity details using a CSV file page should be displayed
	#When the user clicks Download the CSV template and read the guide on how to complete this (opens in new tab)
	#Then the guidance to upload commodity details using a CSV file page will open in new tab
	#When user downloads the template by clicking the link 'IPAFFS commodity details CSV template (xlsx format)'
	#And verifies the document 'IPAFFS-commodity-details-CSV-template-xls' downloaded successfully
	#Then validates the document 'IPAFFS-commodity-details-CSV-template-xls.xlsx' should have column headers 'Commodity code' 'Genus and Species' 'EPPO code' 'Variety' 'Class' 'Intended for final users (or commercial flower production)' 'Number of packages' 'Type of package' 'Quantity' 'Quantity type' 'Net weight (kg)' 'Controlled atmosphere container'
	#When the user closes the newly opened tab
	#Then the browser tab is closed
	#When the user creates a CSV file from the downloaded template 'IPAFFS-commodity-details-CSV-template-xls.xlsx' with below data and uploads the CSV file
	#	| Commodity code | Genus and Species                      | EPPO code | Variety               | Class   | Intended for final users (or commercial flower production) | Number of packages | Type of package | Quantity | Quantity type | Net weight (kg) | Controlled atmosphere container |
	#	|       08105000 | Actinidia chinensis                    | ATICH     | Jintao (Yellow flesh) | Class I |                                                            |                  1 | BL              |        2 | BLB           |               3 | No                              |
	#	|       06012030 | Brassavola sp.                         | BSVSS     |                       |         | No                                                         |                  1 | VR              |        2 | PTC           |               3 | No                              |
	#	|       07099990 | Atropa belladonna                      | ATRBE     |                       |         |                                                            |                  1 | VR              |        2 | PTC           |               3 | Yes                             |
	#	|       06029045 | Abelia engleriana                      | ABEEN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia graebneriana                    | ABEGR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia integrifolia                    | ABEIN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia ionandra                        | ABEIO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia schumannii                      | ABESC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia sp.                             | ABESS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia triflora                        | ABETR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia umbellata                       | ABEUM     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia uniflora                        | ABEUN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia x grandiflora                   | ABEGF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelia zanderi                         | ABEZA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abeliophyllum distichum                | ABLDI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abeliophyllum sp.                      | ABLSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelmoschus caillei                    | ABMCA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Albuca canadensis                      | ABWAL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelmoschus ficulneus                  | HIBFC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelmoschus glutino-textilis           | ABMGT     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelmoschus manihot                    | HIBMA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelmoschus mindanensis                | ABMMI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelmoschus moschatus                  | ABMMO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abelmoschus sp.                        | ABMSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies alba                             | ABIAL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies amabilis                         | ABIAM     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies balsamea                         | ABIBA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies balsamea var. phanerolepis       | ABIPH     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies borisii-regis                    | ABIBO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies bracteata                        | ABIBT     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies cephalonica                      | ABICE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies chengii                          | ABICN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies chensiensis                      | ABICH     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies cilicica                         | ABICI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies concolor                         | ABICO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies concolor var. lowiana            | ABICL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies delavayi                         | ABIDE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies densa                            | ABIDN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies durangensis                      | ABIDU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies fabri                            | ABIFA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies fabri subsp. minensis            | ABIMI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies fargesii                         | ABIFG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies firma                            | ABIFI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies forrestii                        | ABIFO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies fraseri                          | ABIFR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies grandis                          | ABIGR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies guatemalensis                    | ABIGU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies guatemalensis var. jaliscana     | ABIFL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies hickelii                         | ABIHI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies holophylla                       | ABIHL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies homolepis                        | ABIHO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies kawakamii                        | ABIKA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies koreana                          | ABIKO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies lasiocarpa                       | ABILA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies lasiocarpa var. arizonica        | ABILZ     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies magnifica                        | ABIMA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies mariesii                         | ABIMR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies nebrodensis                      | ABINB     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies nephrolepis                      | ABINE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies nordmanniana                     | ABINO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies nordmanniana subsp. equitrojani  | ABIBR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies numidica                         | ABINU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies pindrow                          | ABIPI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies pindrow var. brevifolia          | ABIGA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies pinsapo                          | ABIPN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies pinsapo var. marocana            | ABIMC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies procera                          | ABIPR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies recurvata                        | ABIRE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies recurvata var. ernestii          | ABIER     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies religiosa                        | ABIRG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies sachalinensis                    | ABISA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies sachalinensis var. gracilis      | ABISG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies sachalinensis var. mayriana      | ABISM     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies sachalinensis var. nemorensis    | ABISN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies sibirica                         | ABISB     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies sibirica subsp. semenovii        | ABISE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies sp.                              | ABISS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies spectabilis                      | ABISP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies squamata                         | ABISQ     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies veitchii                         | ABIVE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies vejarii                          | ABIVJ     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies vejarii subsp. mexicana          | ABIME     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies x arnoldiana                     | ABIAR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies x insignis                       | ABIIN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies x vasconcellosiana               | ABIVA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abies x vilmorinii                     | ABIVI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abrus precatorius                      | ABRPR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abrus precatorius subsp. africanus     | ABRPA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abrus pulchellus                       | ABRPP     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abrus pulchellus subsp. suffruticosus  | ABRPS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abrus sp.                              | ABRSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon angulatum                     | ABUAN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon asiaticum                     | ABUAS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon fruticosum                    | ABUFR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon grandifolium                  | ABUMO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon graveolens                    | ABUGR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon guineense                     | ABUGU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon hemsleyanum                   | ABUHE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon hybrids                       | ABUHY     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon indicum                       | ABUIN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon inflatum                      | ABUIF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon ochsenii                      | ABUOC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon oxycarpum                     | ABUOX     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon pannosum                      | ABUGL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon pauciflorum                   | ABUPF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon ramosum                       | ABURA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon sonneratianum                 | ABUSO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Pseudabutilon sp.                      | PSDSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon theophrasti                   | ABUTH     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon trisulcatum                   | ABUTR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Abutilon vitifolium                    | ABUVI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia alata                           | ACAAL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia anceps                          | ACAGL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia aneura                          | ACAAN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia argyrodendron                   | ACAAD     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia aulacocarpa                     | ACAAU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia auriculiformis                  | ACAAF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia baileyana                       | ACABA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia brachystachya                   | ACABR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia burrowii                        | ACABU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia cambagei                        | ACACB     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia cana                            | ACACN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia cardiophylla                    | ACACD     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia catenulata                      | ACACE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia concurrens                      | ACACH     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia confusa                         | ACACU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia coriacea                        | ACACR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia cultriformis                    | ACACL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia curvinervia                     | ACACQ     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia cyclops                         | ACACC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia cyperophylla                    | ACACP     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia dealbata                        | ACADA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia deanei                          | ACADN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia decurrens                       | ACADC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia doratoxylon                     | ACADO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia ericifolia                      | ACAER     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia excelsa                         | ACAEX     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia flavescens                      | ACAFL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia genistifolia                    | ACADI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia georginae                       | ACAGG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia glaucoptera                     | ACAGP     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia harpophylla                     | ACAHA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia homalophylla                    | ACAHM     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia implexa                         | ACAIM     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia ixiophylla                      | ACAIX     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia koa                             | ACAKO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia leiocalyx                       | ACALE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia leptocarpa                      | ACALC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia longifolia                      | ACALO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia macracantha                     | ACAMA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia maidenii                        | ACAMN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia mangium                         | ACAMG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia mearnsii                        | ACAMR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia melanoxylon                     | ACAME     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia mellifera subsp. detinens       | ACAMD     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia mucronata                       | ACAMU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia oswaldii                        | ACAOS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia paniculata                      | ACAPA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia paradoxa                        | ACAAR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia pendula                         | ACAPD     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia penninervis                     | ACAPN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia permixta                        | ACAPX     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia peuce                           | ACAPC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia plumosa                         | ACAPL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia podalyriifolia                  | ACAPF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia polyphylla                      | ACAPO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia pravissima                      | ACAPR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia pulchella                       | ACAPU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia pycnantha                       | ACAPY     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia redolens                        | ACARD     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia reficiens                       | ACARF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia retinodes                       | ACART     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia riceana                         | ACARC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia rigidula                        | ACARI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia salicina                        | ACASC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia saligna                         | ACASA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia senegal var. leiorhachis        | ACASL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia senegal var. rostrata           | ACASO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia seyal var. fistula              | ACASF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia shirleyi                        | ACASH     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia sieberiana var. woodii          | ACASW     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia sp.                             | ACASS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia sparsiflora                     | ACASQ     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia spirocarpa                      | ACASR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia stenophylla                     | ACAST     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia stuhlmannii                     | ACASM     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia sutherlandii                    | ACASU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia tenuifolia                      | ACATF     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia tenuispina                      | ACATS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia terminalis                      | ACATM     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia tetragonophylla                 | ACATE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia tortilis subsp. raddiana        | ACATR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia truncata                        | ACADE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia unijuga                         | ACAUN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia vernicosa                       | ACAVC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia verticillata                    | ACAVE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia victoriae                       | ACAVI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia welwitschii subsp. delagoensis  | ACAWD     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acacia wrightii                        | ACAWR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acaciella angustissima                 | ACAAG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha guatemalensis                 | ACCGU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha havanensis                    | ACCHA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha hispida                       | ACCHI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha indica                        | ACCIN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha macrostachya                  | ACCMA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha malabarica                    | ACCMB     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha neomexicana                   | ACCNE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha ostryifolia                   | ACCOS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha pendula                       | ACCPN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha poirettii                     | ACCPO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha polystachya                   | ACCPY     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha pseudoalopecuroides           | ACCPS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha racemosa                      | ACCRA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha rhomboidea                    | ACCRH     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha schiedana                     | ACCSC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha segetalis                     | ACCSE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha setosa                        | ACCST     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha sp.                           | ACCSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha villicaulis                   | ACCPE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha virginica                     | ACCVI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acalypha wilkesiana                    | ACCWI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon acerosum                  | ACLAC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon armenum                   | ACLAR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon caryophyllaceum           | ACLCA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon diapensioides             | ACLDI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon glumaceum                 | ACLGL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon kotschyi                  | ACLKO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon libanoticum               | ACLLI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon melananthum               | ACLME     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon olivieri                  | ACLOL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon sp.                       | ACLSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acantholimon ulicinum                  | ACLAN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthoprasium frutescens              | BLLFR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthosicyos horridus                 | ACWHO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthosicyos sp.                      | ACWSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthospermum australe                | ACNAU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthospermum glabratum               | ACNGL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Albuca fibrotunicata                   | ABWFI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthospermum humile                  | ACNHU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthospermum sp.                     | ACNSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthostyles buniifolius              | EUPBU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthosyris faclata                   | AHSFA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthosyris paulo-alvimii             | AHSPA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acanthosyris sp.                       | AHSSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acca sellowiana                        | FEJSE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer acuminatum                        | ACRAC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer albopurpurascens                  | ACRAL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer amplum                            | ACRAM     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer argutum                           | ACRAR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer barbatum                          | ACRBA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer barbinerve                        | ACRBB     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer buergerianum                      | ACRBU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer caesium                           | ACRCE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer caesium subsp. giraldii           | ACRCI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer campbellii                        | ACRCB     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer campestre                         | ACRCA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer capillipes                        | ACRCL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer cappadocicum                      | ACRCP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer carpinifolium                     | ACRCR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer catalpifolium                     | ACRCT     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer caudatifolium                     | ACRCF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer caudatum                          | ACRCD     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer caudatum subsp. ukurunduense      | ACRUK     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer caudatum var. multiserratum       | ACRCM     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer circinatum                        | ACRCJ     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer cissifolium                       | ACRCS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer cordatum                          | ACRCO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer coriaceifolium                    | ACRCC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer crataegifolium                    | ACRCG     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer davidii                           | ACRDA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer davidii subsp. grosseri           | ACRGO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer diabolicum                        | ACRDI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer discolor                          | ACRDS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer distylum                          | ACRDT     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer divergens                         | ACRDV     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer erianthum                         | ACRER     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer fabri                             | ACRFA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer fargesii                          | ACRFG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer flabellatum                       | ACRFL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer forrestii                         | ACRFO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer franchetii                        | ACRFR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer glabrum                           | ACRGL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer granatense                        | ACRGR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer griseum                           | ACRGS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer heldreichii                       | ACRHE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer henryi                            | ACRHN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer hypoleucum                        | ACRHL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer hyrcanum                          | ACRHR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer japonicum                         | ACRJA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer laevigatum                        | ACRLA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer laevigatum var. salweense         | ACRLS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer laurinum                          | ACRGA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer laxiflorum                        | ACRLX     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer leucoderme                        | ACRLE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer litseifolium                      | ACRLI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer lobelii                           | ACRLB     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer longipes                          | ACRLO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer macrophyllum                      | ACRMA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer mandshuricum                      | ACRMN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer maximowiczii                      | ACRMX     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer mayrii                            | ACRMY     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer micranthum                        | ACRMR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer miyabei                           | ACRMI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer monspessulanum                    | ACRMS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer monspessulanum subsp. cinerascens | ACRMC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer multiserratum                     | ACRMU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer negundo                           | ACRNE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer negundo var. californicum         | ACRNC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer nikoense                          | ACRNK     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer nipponicum                        | ACRNP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer oblongum                          | ACROB     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer obtusatum                         | ACROT     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer obtusifolium                      | ACROF     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer okamotoanum                       | ACROK     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer oliverianum                       | ACROL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer opalus                            | ACROP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer osmastonii                        | ACROS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer palmatum                          | ACRPA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer paxii                             | ACRPX     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pectinatum                        | ACRPC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pensylvanicum                     | ACRPE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pentapomicum                      | ACRPT     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pictum subsp. mono                | ACRMO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pilosum                           | ACRPI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer platanoides                       | ACRPL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pseudoplatanus                    | ACRPP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pseudosieboldianum                | ACRPS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pubipalmatum                      | ACRPB     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer pycnanthum                        | ACRPY     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer ramosum                           | ACRRA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer robustum                          | ACRRO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer rubrum                            | ACRRB     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer rufinerve                         | ACRRU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer saccharinum                       | ACRSA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer saccharum                         | ACRSC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer saccharum subsp. grandidentatum   | ACRSG     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer saccharum subsp. nigrum           | ACRSN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer schneiderianum                    | ACRSD     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer semenovii                         | ACRSM     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sempervirens                      | ACRSV     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer shirasawanum                      | ACRSH     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sieboldianum                      | ACRSB     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sikkimense                        | ACRHO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sinense                           | ACRSI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sino-oblongum                     | ACRSO     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sino-purpurascens                 | ACRSR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sp.                               | ACRSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer spicatum                          | ACRSP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer stachyophyllum                    | ACRST     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sterculiaceum                     | ACRSQ     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer sutchuense                        | ACRSU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer taronense                         | ACRTN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer tataricum                         | ACRTA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer tataricum subsp. ginnala          | ACRGN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer tegmentosum                       | ACRTG     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer thomsonii                         | ACRTH     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer tibetense                         | ACRTI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer tonkinense                        | ACRTO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer trautvetteri                      | ACRTR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer triflorum                         | ACRTF     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer truncatum                         | ACRTU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer tschonoskii                       | ACRTS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer turkestanicum                     | ACRTK     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer tutcheri                          | ACRTC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer velutinum                         | ACRVL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer wardii                            | ACRWA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer wilsonii                          | ACRWI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x bornmuelleri                    | ACRBO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x boscii                          | ACRBS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x coriaceum                       | ACRCU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x dieckii                         | ACRDK     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x freemanii                       | ACRFE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x hybridum                        | ACRHY     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x rotundilobum                    | ACRRT     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x senecaense                      | ACRSE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x sericeum                        | ACRSJ     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x veitchii                        | ACRVE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer x zoeschense                      | ACRZO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acer yuii                              | ACRYU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Barnardia japonica                     | SLLSC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea alpina                        | ACHSI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea borealis                      | ACHBO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea distans                       | ACHDI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Bellevalia ciliata                     | BLVCI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea micrantha                     | ACHMC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Bellevalia longipes                    | BLVLO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea millefolium var. occidentalis | ACHLA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Bellevalia romana                      | BLVRO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Bellevalia sp.                         | BLVSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea santolina                     | ACHSA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea setacea                       | ACHSE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achillea sp.                           | ACHSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06011010 | Bellevalia spicata                     | BLVSP     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achyrocline satureioides               | ACOSA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Achyrocline sp.                        | ACOSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acilepis divergens                     | VENDV     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acis autumnalis                        | LEJAU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmella alba                           | SPLOC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmella brachyglossa                   | SPLLI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmella caulirhiza                     | SPLMR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmella ciliata                        | SPLCI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmella oleracea                       | SPLOL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmella paniculata                     | SPLPA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmopyle pancheri                      | ACMPA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acmopyle sp.                           | ACMSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acnistus arborescens                   | AKSAR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acnistus sp.                           | AKSSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acoelorrhaphe sp.                      | AEQSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acokanthera oblongifolia               | CISSP     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acokanthera oppositifolia              | CISAK     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acradenia frankliniae                  | ARNFR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acradenia sp.                          | ARNSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrocarpus fraxinifolius               | AOCFR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrocarpus sp.                         | AOCSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrocomia aculeata                     | AARSC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrocomia sp.                          | AARSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acroptilon repens                      | CENRE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrostichum aureum                     | AOHAU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrostichum sp.                        | AOHSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrotome inflata                       | AFTIN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Acrotome sp.                           | AFTSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actaea rubra                           | AATSR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actaea sp.                             | AATSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actaea spicata                         | AATSP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actinodaphne sp.                       | AHDSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actinostemma lobatum                   | ACVLO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actinostemma sp.                       | ACVSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actinostrobus acuminatus               | ACJAC     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actinostrobus pyramidalis              | ACJPY     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Actinostrobus sp.                      | ACJSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adansonia digitata                     | AADDI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adansonia sp.                          | AADSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenandra fragrans                     | ADDFR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenandra sp.                          | ADDSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenandra umbellata                    | ADDUM     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenandra uniflora                     | ADDUN     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenanthera pavonina                   | ADEPA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenanthera sp.                        | ADESS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia cissampeloides                  | ADJCI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia digitata                        | ADJDI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia gracilis                        | ADJGR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia mannii                          | ADJMA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia rumicifolia                     | ADJRU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia rumicifolia var. miegei         | ADJRM     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia rumicifolia var. rumicifolia    | ADJRR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia sp.                             | ADJSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenia staudtii                        | ADJST     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenium obesum                         | ADFOB     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenium sp.                            | ADFSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocalymma alliaceum                 | ADNAL     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocalymma bracteatum                | ADNBR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocalymma sp.                       | ADNSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus anagyrifolius              | ADCAN     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus complicatus                | ADCCO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus decorticans                | ADCDE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus foliosus                   | ADCFO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus hispanicus                 | ADCHI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus sp.                        | ADCSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus telonensis                 | ADCTE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocarpus viscosus                   | ADCVI     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocaulon bicolor                    | ADKBI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenocaulon sp.                        | ADKSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenodolichos rhomboideus              | AOORH     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenodolichos sp.                      | AOOSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostemma brasilianum                | AOSBR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostemma sp.                        | AOSSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostemma viscosum                   | AOSLA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostoma fasciculatum                | ADSFA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostoma sp.                         | ADSSS     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostoma sparsifolium                | ADSSP     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostyles alliariae                  | ADTAL     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adenostyles sp.                        | ADTSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adesmia muricata                       | AIMMU     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adesmia sp.                            | AIMSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum capillus-veneris              | ADICV     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum concinnum                     | ADICO     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum cristatum                     | ADICR     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum fulvum                        | ADIFU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum hispidulum                    | ADIHI     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum pedatum                       | ADIPE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum raddianum                     | ADIRA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum sp.                           | ADISS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum tenerum                       | ADITE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum trapeziforme                  | ADITR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adiantum venustum                      | ADIVE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adonidia merrillii                     | VTHME     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adriana acerifola                      | ADBAC     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Adriana sp.                            | ADBSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea candida                        | AEMCA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea chantinii                      | AEMCH     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea fasciata                       | AEMFA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea fulgens                        | AEMFU     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea magdalenae                     | AEMMA     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea mexicana                       | AEMME     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea servitensis                    | AEMSE     |                       |         | No                                                         |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aechmea sp.                            | AEMSS     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aegiphila sellowiana                   | AEFSE     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Aegle marmelos                         | AELMA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Afrocarpus falcatus                    | PODFA     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#	|       06029045 | Afrocarpus gracilior                   | PODGR     |                       |         | Yes                                                        |                  1 | BG              |        2 | STM           |               3 | Yes                             |
	#Then CSV file should be uploaded successfully with 'Upload successful' message and validate the count of commodity
	#And all the displayed commodity data in all tables should be validated with the values given in the input
	#And checks the information message heading 'If details are incorrect or missing' and content 'You will need to upload commodity details CSV file again. This will replace your previous upload.'
	#When the user clicks Confirm and continue button
	#Then What is the main reason for importing the consignment? page should be displayed
	#When The user selects 'Internal market' radio option
	#And the user clicks Save and continue
	#Then the Notification Hub page should be displayed
	#When the user clicks the Commodity hyperlink
	#Then Check uploaded commodity details page should be displayed
	#And all the displayed commodity data in all tables should be validated with the values given in the input
	#And checks the information message heading 'If details are incorrect or missing' and content 'You will need to upload commodity details CSV file again. This will replace your previous upload.'
	#When the user clicks Confirm and continue button
	#Then the Additional details page should be displayed
	#And user enters the total gross weight greater than the net weight '3500'
	#And the user clicks Save and continue
	#Then Transport to the Border Control Post (BCP) page should be dislayed
	#When the user populates the transport to the BCP details 'London Gateway (plants) - GBLGP1PP' 'London Gateway' 'Road vehicle' '123456' 'No' 'Doc1234'
	#And the user clicks Save and continue
	#Then the Goods movement services page should be displayed
	#When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	#And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	#And the user clicks Save and continue
	#Then the Contact details page should be displayed, pre-populated with the user's details
	#When the user clicks Save and continue
	#Then the Nominated contacts page should be displayed
	#When the user clicks Save and continue
	#Then the Accompanying documents page should be displayed
	#When the user selects Document type "Phytosanitary certificate"
	#And the user enters Document reference "INV12345"
	#And the user selects a previous date from the date picker
	#And the user clicks on Add attachment link
	#And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	#Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	#And the user clicks Save and continue
	#And Importer, Packer, Delivery address and Consignor page should be displayed
	#When the user verifies Importer details 'IPAFFS IDM Test' is pre-filled
	#And the user clicks Add a delivery address link
	#Then Search for an existing delivery address page should be displayed
	#When the user selects one of the displayed delivery address "DEFRA"
	#Then the chosen delivery address "DEFRA" should be displayed on the Traders page
	#When the user clicks Add a consignor or exporter
	#Then the Search for an existing consignor or exporter page should be displayed
	#When the user selects a consignor or exporter "DEFRA"
	#Then the chosen consignor or exporter should be displayed
	#When the user clicks Save and continue
	#Then the Review your notification page should be displayed
	#And the data presented for review matches the data entered into the notification for CHED PP
	#When the user clicks Save and continue
	#Then the Declaration page should be displayed
	#When the user ticks the checkbox to declare that the information is true and correct
	#And the user clicks Submit notification
	#Then the Confirmation page should be displayed with the initial risk assessment
	#When the user records the IPAFFS User details and CHED Reference
	#Then the details should be recorded
	#When the user clicks Return to your dashboard
	#Then the dashboard page should be displayed
	#When user searches for the import notification
	#Then the notification should be present in the list
	#When the user clicks Show notification
	#Then the certificate should be displayed in a new browser tab
	#When the user checks that the data in the certificate matches the data entered into the notification
	#And the user closes the PDF browser tab
	#Then the browser tab is closed
	#And the dashboard page should be displayed
	#When the user logs out of IPAFFS Part 1
	#Then the user should be logged out successfully
	#When the user navigate to the BTMS application
	#Then I click Sign in button
	#And I should see type of Gateway login page
	#And I have selected "Government Gateway" as login type
	#And I click Sign in button
	#And I should see type of Gateway login page
	#And I have selected "Sign in with Government Gateway" as login type
	#When I click Continue button from How do you want to sign in page
	#Then I should redirected to the BTMS Sign in using Government Gateway page
	#When I have provided the BTMS credentials and signin
	#Then the BTMS search screen should be displayed
	#When the user searches for the CHED created earlier
	#Then the BTMS search result screen should be displayed
	#And the user checks commodity code "08105000", description "Actinidia chinensis", quantity "3.0", authority "HMI" and decision "Decision not given"
	#And the user checks commodity code "06029045", description "Abies holophylla", quantity "3.0", authority "PHSI" and decision "Decision not given"
	#And the user checks commodity code "06029045", description "Acacia truncata", quantity "3.0", authority "PHSI" and decision "Decision not given"
	#And the user checks commodity code "06029045", description "Acer capillipes", quantity "3.0", authority "PHSI" and decision "Decision not given"
	#And the user checks commodity code "06029045", description "Acer pictum subsp. mono", quantity "3.0", authority "PHSI" and decision "Decision not given"
	#And the user checks commodity code "06029045", description "Afrocarpus gracilior", quantity "3.0", authority "PHSI" and decision "Decision not given"
	#When the user logs out of BTMS
	#Then the user should be logged out successfully
	When I am logged in to the 'IDCOMS' app as 'Inspector'
	And I open the sub area 'Importer Notifications' under the 'Case Management' area
	And I search Importer Notifications for the notification created in IPAFFS
	And I open the record at position '0' in the grid
	Then I verify the Importer Notification Details reflect the information from the EU Import Notification
	When I select the Permanent Addresses tab
	Then I verify the permanent address displayed for each animal matches the address entered in IPAFFS