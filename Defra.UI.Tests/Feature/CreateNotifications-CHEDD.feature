@Regression
Feature: CreateNotification CHEDD

Create a notification for CHEDD type

Scenario: User creates and submits a B2C consignment notification - CHEDD Happy path
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Australia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When I click the back button in the browser
	Then the Origin of the import page should be displayed
	When I click the forward button in the browser
	Then the Description of the goods/Commodity page should be displayed
	When the user searches for first commodity code '12024200'
	Then the commodity details should be populated '12024200' 'Shelled, whether or not broken' for first commodity
	When the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	When the user populates Net weight as '19000' for first commodity
	And the user populates Number of packages as '1' for first commodity
	And the user selects type of package as 'Case' for the commodity '12024200' for first commodity
	And the user clicks the Update total button
	And the user clicks the Add commodity link
	And the user clicks the 'CEREALS' in the parent commodity tree
	And the sub commodity list expands
	And the user clicks '1006' 'Rice' under the parent commodity
	And the sub commodity list expands
	And the user selects the second commodity '10064000' 'Broken rice' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then the Commodity page should be displayed
	When the user populates Net weight as '18000' for the second commodity '10064000'
	And the user populates Number of packages as '1' for the second commodity '10064000'
	And the user selects type of package as 'Box' for the second commodity '10064000'
	When the user clicks the Update total button after adding all the commodities
	Then the total gross weight should be greater than the net weight '40000'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	When the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user download the document attached in accompanying documents
	Then the user switch to next tab and open the browser downloads
	And verifies the document 'IPAFFS Test Document' downloaded successfully
	When the user closes the newly opened tab
	Then the browser tab is closed
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "New"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "New" to "In Progress"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user clicks Save and continue without entering the local reference number data
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks on Save and continue button on the Checks page
	Then the Seal numbers page should be displayed
	When the user select 'No' radio button on the Seal numbers page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select 'No' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Decision page should be displayed
	And the main radio option 'Internal market' and the sub radio option 'Human consumption' are selected by default
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added for CHED D
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "12024200", description "Shelled, whether or not broken", quantity "19000", authority "FNAO" and decision "Acceptable for Internal Market" for commodity "1" after the decision is given
	And the user validates the commodity code "10064000", description "Broken rice", quantity "18000", authority "FNAO" and decision "Acceptable for Internal Market" for commodity "2" after the decision is given	
	When the user logs out of BTMS
	Then the user should be logged out successfully

Scenario: User creates and submits a notification, override the risk decision, reject the notification and creates border notification - CHEDD - SPS-9107
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed
	When country of origin and Country from where consigned fields are pre-populated with previously selected country
	And 'No' is pre-selected for the Does your consignment require a region code? radio option
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '12024200' commodity code
	Then the commodity details should be populated '12024200' 'Shelled, whether or not broken'
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Non-internal market' radio option
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
	And the Commodity intended for field displays the radio options 'Feedingstuff' 'Further process' 'Human consumption' and 'Other'
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	When the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document - Exceeds size' in the format '.png' that exceeds size limit
	Then the error message for exceeding file size should be displayed
	When the user clicks on Cancel link
	Then the Accompanying documents page should be displayed
	When the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the document 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	And the user clicks Save and continue
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
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "12024200", description "Shelled, whether or not broken", quantity "1000", authority "FNAO" and decision "Decision not given" for commodity "1" after the decision is given
	When the user logs out of BTMS
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "New"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "New" to "In Proress"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Not satisfactory' sub radio button under the Physical check main radio
	And the user clicks on Save and continue button on the Checks page
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	And 'Non-internal market' radio is pre-selected under Acceptable for
	When the user selects 'Use for other purpose' 'Not acceptable' in decision page
	And the user enters currendate in decision page
	And the user clicks Save and continue
	Then the Select a controlled destination page should be displayed
	When the user clicks Add a controlled destination
	Then the Search for an existing controlled destination page should be displayed
	When the user selects a controlled destination
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user selects "Other, Create Border Notification" as reason for refusal
	And the user selects "Chemical Contamination" as another reason for refusal
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And a border notification banner displaying the reason for refusal 'Refused for chemical contamination, other border notification reasons' under the title 'The result of this decision requires a border notification to be created' is displayed
	When the user clicks on Create border notification button
	Then Enter the details of the border notification page should be displayed
	When the user selects "Food" as the Notification type
	And the user selects "border control - consignment detained" as the Notification basis
	And the user selects "Alcoholic Beverages" as the Product category
	And the user enters "Food" as the Product name
	And the user enters "Flipflop" as the Brand name
	And the user enters "Ingredients" in the Other labelling field
	And the user enters "Destroy" in the Other information field
	And the user selects "Use by date" under the Durability date radio options
	And the user selects 'not serious' as Risk decision
	And the user selects 'animal health' as Impact on
	And the user selects 'GMO / novel food' as Hazard category
	And the user selects 'chemical treatment' as Measure taken
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed on the Inspector application
	When the user selects Document type "Air Waybill" for creating border notification
	And the user enters Document reference "INV12345" for creating border notification
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx' for creating border notification
	Then the Accompanying documents page should be displayed on the Inspector application
	And the user clicks Save and continue
	Then the Review border notification page should be displayed
	And the border notification details reflect the information added
	When the user clicks on the Submit button
	Then your border notification has been submitted page should be displayed
	When the user records the BN number
	And the user clicks Return to dashboard button
	Then Border notifications dashboard page should be displayed
	When the user searches for the newly created border notification
	Then the border notification found with status "New"
	When the user logs out of Border notifications in IPAFFS Part 2 
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the notification is displayed on the inspector dashboard
	When the user clicks View CHED link
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "12024200", description "Shelled, whether or not broken", quantity "1000", authority "FNAO" and decision "Non Acceptable" for commodity "1" after the decision is given
	When the user logs out of BTMS
	Then the user should be logged out successfully

Scenario: User verifies Address book page search, submits notification for 'Non-Internal market' reason,  override the risk decision, reject the notification and creates border notification - CHEDD - SPS-9106
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user is taken to the Your import notifications page
	When the user clicks the Address book link on the Your import notifications page
	Then the Address book page should be displayed
	When the user searches by selecting 'Importer' in the Type dropdown
	Then the type of every address listed is 'Importer'
	When the user searches by selecting 'Transporter' in the Type dropdown
	Then the type of every address listed is 'Transporter'
	When the user searches by selecting 'Exporter' in the Type dropdown
	Then the type of every address listed is 'Exporter'
	When the user clicks on the Dashboard link on the top left
	Then the user is taken to the Your import notifications page
	And the Search notifications by section displays all the fields on the Your import notifications page
	When the user clicks Contact link on the footer
	Then the Contacting us by phone or by email page is displayed
	And the Contacting us by phone or by email page displays the text to contact APHA service desk
	When the user clicks on the back link
	Then the Your notifications page is displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Australia' as the Country of origin and Country from where consigned
	When the user changes the consigned country to 'Belgium'
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks the 'CEREALS' in the parent commodity tree
	And the sub commodity list expands
	And the user clicks '1006' 'Rice' under the parent commodity
	And the sub commodity list expands
	And the user selects the '10064000' 'Broken rice' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Non-internal market' radio option
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
	And the Commodity intended for field displays the radio options 'Feedingstuff' 'Further process' 'Human consumption' and 'Other'
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	When the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Sea waybill"
	And the user enters Document reference "INV67890"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document with a long file name to check the limit of filename length exceeding 100 characters' in the format '.docx'
	Then the document 'IPAFFS Test Document with a long file name to check the limit of filename length exceeding 100 characters' '.docx' is uploaded successfully
	And the file name should contain 27 characters including the file type
	And the user clicks Save and continue
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
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks the View details link
	Then the Review your notification page should be displayed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "10064000", description "Broken rice", quantity "1000", authority "FNAO" and decision "Decision not given" for commodity "1" after the decision is given
	When the user logs out of BTMS
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks on Save and continue button on the Checks page
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	And the user clicks Save and continue
	Then the Decision page should be displayed
	And 'Non-internal market' radio is pre-selected under Acceptable for
	When the user changes the selection to "Re-dispatching" "Not acceptable" in the decision page
	And the user enters currendate in decision page
	And the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user selects "Other, Create Border Notification" as reason for refusal
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And a border notification banner displaying the reason for refusal 'Refused for other border notification' under the title 'The result of this decision requires a border notification to be created' is displayed
	When the user clicks on Create border notification button
	Then Enter the details of the border notification page should be displayed
	When the user selects "Food" as the Notification type
	And the user selects "border control - consignment detained" as the Notification basis
	And the user selects "Alcoholic Beverages" as the Product category
	And the user enters "Food" as the Product name
	And the user enters "Flipflop" as the Brand name
	And the user enters "Ingredients" in the Other labelling field
	And the user enters "Destroy" in the Other information field
	And the user selects "Use by date" under the Durability date radio options
	And the user selects 'not serious' as Risk decision
	And the user selects 'animal health' as Impact on
	And the user selects 'GMO / novel food' as Hazard category
	And the user selects 'chemical treatment' as Measure taken
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed on the Inspector application
	When the user selects Document type "Air Waybill" for creating border notification
	And the user enters Document reference "INV12345" for creating border notification
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx' for creating border notification
	Then the Accompanying documents page should be displayed on the Inspector application
	And the user clicks Save and continue
	Then the Review border notification page should be displayed
	And the border notification details reflect the information added
	When the user clicks on the Submit button
	Then your border notification has been submitted page should be displayed
	When the user records the BN number
	And the user clicks Return to dashboard button
	Then Border notifications dashboard page should be displayed
	When the user searches for the newly created border notification
	Then the border notification found with status "NEW"
	When the user logs out of Border notifications in IPAFFS Part 2 
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the notification is displayed on the inspector dashboard
	When the user clicks View CHED link
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "10064000", description "Broken rice", quantity "1000", authority "FNAO" and decision "Non Acceptable" for commodity "1" after the decision is given
	When the user logs out of BTMS
	Then the user should be logged out successfully

Scenario: User submits B2C consignment notification, inspector rejects and creates border notification - CHEDD - SPS-7378
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Australia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches for first commodity code '12024200'
	Then the commodity details should be populated '12024200' 'Shelled, whether or not broken' for first commodity
	When the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	When the user populates Net weight as '100'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '100'
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	When the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "12024200", description "Shelled, whether or not broken", quantity "100", authority "FNAO" and decision "Decision not given" for commodity "1" after the decision is given
	When the user logs out of BTMS
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Checks link in Record checks
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks Save and Return
	When the user clicks Laboratory tests link
	Then the Laboratory tests page should be displayed
	When the user select 'No' radio button on the Laboratory tests page
	And the user clicks Save and Return
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user selects '' 'Not acceptable' in decision page
	And the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user selects "Other, Create Border Notification" as reason for refusal
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user Clicks the change link under 'Additional documents'
	Then the Documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV54322"
	And the user enters date of issue "04/12/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Inspector Document' in the format '.docx'
	Then the document 'IPAFFS Inspector Document' '.docx' is uploaded successfully
	And the user verifies the entered document information is displayed correctly
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And a border notification banner displaying the reason for refusal 'Refused for other border notification reasons' under the title 'The result of this decision requires a border notification to be created' is displayed
	When the user clicks on Create border notification button
	Then Enter the details of the border notification page should be displayed
	When the user selects "Food" as the Notification type
	And the user selects "border control - consignment detained" as the Notification basis
	And the user selects "Alcoholic Beverages" as the Product category
	And the user enters "Food" as the Product name
	And the user enters "Flipflop" as the Brand name
	And the user enters "Ingredients" in the Other labelling field
	And the user enters "Destroy" in the Other information field
	And the user selects "Use by date" under the Durability date radio options
	And the user selects 'not serious' as Risk decision
	And the user selects 'animal health' as Impact on
	And the user selects 'GMO / novel food' as Hazard category
	And the user selects 'chemical treatment' as Measure taken
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed on the Inspector application
	When the user selects Document type "Air Waybill" for creating border notification
	And the user enters Document reference "INV12345" for creating border notification
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx' for creating border notification
	Then the Accompanying documents page should be displayed on the Inspector application
	And the user clicks Save and continue
	Then the Review border notification page should be displayed
	And the border notification details reflect the information added
	When the user clicks on the Submit button
	Then your border notification has been submitted page should be displayed
	When the user records the BN number
	And the user clicks Return to dashboard button
	Then Border notifications dashboard page should be displayed
	When the user logs out of Border notifications in IPAFFS Part 2 
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When user searches for the import notification after decision submission
	Then the notification should be present in the list of part 2 dashboard
	When the user clicks View CHED link
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	Then the user clicks the notification found with status "REJECTED"
	And the CHED overview page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "12024200", description "Shelled, whether or not broken", quantity "100", authority "FNAO" and decision "Non Acceptable" for commodity "1" after the decision is given
	When the user logs out of BTMS

Scenario: User creates and submits a notification, Copy the notification and submit, Records decision as Inspector and Validate in BTMS - CHEDD - SPS-9111
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Australia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When I click the back button in the browser
	Then the Origin of the import page should be displayed
	When I click the forward button in the browser
	Then the Description of the goods/Commodity page should be displayed
	When the user searches for first commodity code '12024200'
	Then the commodity details should be populated '12024200' 'Shelled, whether or not broken' for first commodity
	When the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	When the user populates Net weight as '19000' for first commodity
	And the user populates Number of packages as '1' for first commodity
	And the user selects type of package as 'Case' for the commodity '12024200' for first commodity
	And the user clicks the Add commodity link
	And the user clicks the 'CEREALS' in the parent commodity tree
	And the sub commodity list expands
	And the user clicks '1006' 'Rice' under the parent commodity
	And the sub commodity list expands
	And the user selects the second commodity '10064000' 'Broken rice' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	When the user populates Net weight as '18000' for the second commodity '10064000'
	And the user populates Number of packages as '1' for the second commodity '10064000'
	And the user selects type of package as 'Box' for the second commodity '10064000'
	When the user clicks the Update total button after adding all the commodities
	Then the total gross weight should be greater than the net weight '40000'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	When the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	When the user download the document attached in accompanying documents
	Then the user switch to next tab and open the browser downloads
	And verifies the document 'IPAFFS Test Document' downloaded successfully
	When the user closes the newly opened tab
	Then the browser tab is closed
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	And the user records the Draft CHED number
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When the user logs back into IPAFFS application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	And the user searches for the Draft CHED reference on the dashboard
	Then the draft notification should be present in the list
	When the user clicks the Amend link
	Then the Notification Hub page should be displayed
	When the user clicks Contact address for consignment link
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D	
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks return to your dashboard link
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks the Show notification link
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	And the dashboard page should be displayed
	And the user is taken back to the dashboard page
	When the user clicks the Copy as new link for the notification
	Then the Notification Hub page of a new draft notification should be displayed
	When the user clicks on 'Origin of the import' link
	Then the user verifies and enters any missing data on the Origin of the import page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Main reason for importing the consignment' link
	Then the user verifies and enters any missing data on the Main reason for importing the consignment page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Commodity' link
	Then the user verifies and enters any missing data on the Commodity page
	And the user clicks on Save and return to hub on the Commodity page
	When the user clicks on 'Additional details' link
	Then the user verifies and enters any missing data on the Additional details page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Accompanying documents' link
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx' as no document is attached by the copy
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks the Save and return to hub button
	When the user clicks on 'Addresses' link
	Then the user verifies and enters any missing data on the Addresses page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Transport to the port of entry' link
	Then the user verifies and enters any missing data on the Transport to the port of entry page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Goods movement services' link
	Then the user verifies and enters any missing data on the Goods movement services page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Contact details' link
	Then the user verifies and enters any missing data on the Contact details page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Contact address for consignment' link
	Then the user verifies and enters any missing data on the Contact address for consignment page
	And the user clicks the Save and return to hub button
	When the user clicks on 'Review and submit' link
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user clicks Save and continue without entering the local reference number data
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks on Save and continue button on the Checks page
	Then the Seal numbers page should be displayed
	When the user select 'No' radio button on the Seal numbers page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select 'No' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Decision page should be displayed
	And the main radio option 'Internal market' and the sub radio option 'Human consumption' are selected by default
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added for CHED D
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user validates the commodity code "12024200", description "Shelled, whether or not broken", quantity "19000", authority "FNAO" and decision "Acceptable for Internal Market" for commodity "1" after the decision is given
	And the user validates the commodity code "10064000", description "Broken rice", quantity "18000", authority "FNAO" and decision "Acceptable for Internal Market" for commodity "2" after the decision is given	
	When the user logs out of BTMS
	Then the user should be logged out successfully

Scenario: User submits B2C consignment notification, reason for refusal and creates border notification - CHED D SPS-7381
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Australia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed	
	When the user clicks the 'OIL SEEDS AND OLEAGINOUS FRUITS; MISCELLANEOUS GRAINS, SEEDS AND FRUIT; INDUSTRIAL OR MEDICINAL PLANTS; STRAW AND FODDER' in the parent commodity tree
	And the sub commodity list expands
	And the user clicks '' 'Groundnuts, not roasted or otherwise cooked, whether or not shelled or broken' under the parent commodity
	And the user clicks '' 'Other' under the parent commodity
	And the user selects the '12024200' 'Shelled, whether or not broken' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	And the user populates Net weight as '100'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '100'
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user clicks Save and continue without entering the local reference number data
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks Save and Return
	When the user clicks Laboratory tests link
	Then the Laboratory tests page should be displayed
	When the user select 'No' radio button on the Laboratory tests page
	And the user clicks Save and Return
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user selects 'Destruction' 'Not acceptable' in decision page
	And the user provides the reason as 'Refusal reason' for destruction option in decision page
	And the user enters currendate in decision page
	And the user clicks Save and continue
	Then the Select a controlled destination page should be displayed
	When the user clicks Add a controlled destination
	Then the Search for an existing controlled destination page should be displayed
	When the user selects a controlled destination
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user selects "Microbiological Contamination" as reason for refusal
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And the user verified the banner message 'The result of this decision requires a border notification to be created'
	And the user verified the banner message 'Refused for microbiological contamination reasons'
	When the user clicks Return to your dashboard link
	Then the Import notifications dashboard page should be displayed	
	When the user searches for the newly created notification on the Import notifications page
	And the user clicks View details for the notification
	Then the CHED overview page should be displayed
	When the user clicks Raise border notification button
	Then Enter the details of the border notification page should be displayed
	And the user enters the details as 'Food' 'official control on the market' 'Fats & oils' 'Product' 'Brand' 'Label' 'Other' 'Use by date' 'undecided' 'environment' 'Allergens' 'destruction'
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Review border notification page should be displayed
	And the border notification details reflect the information added
	When the user downloads the document attached in accompanying documents
	Then the user switch to next tab and open the browser downloads
	And verifies the document 'IPAFFS Test Document' downloaded successfully
	When the user closes the newly opened tab
	Then the browser tab is closed
	When the user clicks on the Submit button
	Then your border notification has been submitted page should be displayed
	When the user records the BN number
	And the user clicks Return to dashboard button
	Then Border notifications dashboard page should be displayed
	When the user logs out of Border notifications in IPAFFS Part 1
	Then the user should be logged out successfully
	When the user deletes all the stored values
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Australia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches for first commodity code '12024200'
	Then the commodity details should be populated '12024200' 'Shelled, whether or not broken' for first commodity
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	And the user populates Net weight as '100'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '100'
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a future date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user clicks Save and continue without entering the local reference number data
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks Save and Return
	When the user clicks Laboratory tests link
	Then the Laboratory tests page should be displayed
	When the user select 'No' radio button on the Laboratory tests page
	And the user clicks Save and Return
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user selects 'Destruction' 'Not acceptable' in decision page
	And the user provides the reason as 'Refusal reason' for destruction option in decision page
	And the user enters currendate in decision page
	And the user clicks Save and continue
	Then the Select a controlled destination page should be displayed
	When the user clicks Add a controlled destination
	Then the Search for an existing controlled destination page should be displayed
	When the user selects a controlled destination
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user selects "Other" as reason for refusal
	And the user provides the reason as 'other reason' in Reason for refusal page
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And the user verified the banner message 'The result of this decision requires a border notification to be created'
	And the user verified the banner message 'Refused for other reasons'
	When the user clicks Return to your dashboard link
	Then the Import notifications dashboard page should be displayed	
	When the user searches for the newly created notification on the Import notifications page
	And the user clicks View details for the notification
	Then the CHED overview page should be displayed
	When the user clicks Raise border notification button
	Then Enter the details of the border notification page should be displayed
	And the user enters the details as 'Food' 'official control on the market' 'Fats & oils' 'Product' 'Brand' 'Label' 'Other' 'Use by date' 'undecided' 'environment' 'Allergens' 'destruction'
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Review border notification page should be displayed
	And the border notification details reflect the information added
	When the user downloads the document attached in accompanying documents
	Then the user switch to next tab and open the browser downloads
	And verifies the document 'IPAFFS Test Document' downloaded successfully
	When the user closes the newly opened tab
	Then the browser tab is closed
	When the user clicks on the Submit button
	Then your border notification has been submitted page should be displayed
	When the user records the BN number
	And the user clicks Return to dashboard button
	Then Border notifications dashboard page should be displayed
	When the user logs out of Border notifications in IPAFFS Part 1
	Then the user should be logged out successfully

Scenario: User submits consignment notification, inspector overrides the risk decision and rejects the notification - CHED D 7379
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Australia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches for first commodity code '12024200'
	Then the commodity details should be populated '12024200' 'Shelled, whether or not broken' for first commodity
	When the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	When the user populates Net weight as '100'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '100'
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	When the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a previous date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user clicks Record decision from the header
	And the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks override the risk decision
	Then the user selects 'Override risk decision as no inspection' option for override decision
	When the user clicks Yes, override risk decision button
	Then the Decision Hub page should be displayed
	And the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects 'Destruction' 'Not acceptable' in decision page
	And the user provides the reason as 'Refusal reason' for destruction option in decision page
	And the user enters currendate in decision page
	And the user clicks Save and continue
	Then the Select a controlled destination page should be displayed
	When the user clicks Add a controlled destination
	Then the Search for an existing controlled destination page should be displayed
	When the user selects a controlled destination
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user selects "Chemical Contamination" as reason for refusal
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks Record control in Dashboard page
	Then the Consignments requiring control page should be displayed
	When the user searches for the CHED number
	Then the notification should be found with the status "Rejected"
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User creates a notification and the inspector revises the decision throughout the Record Decision workflow CHEDD - SPS-7380
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Australia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Australia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks the 'CEREALS' in the parent commodity tree
	And the sub commodity list expands
	And the user clicks '1006' 'Rice' under the parent commodity
	And the sub commodity list expands
	And the user selects the '10064000' 'Broken rice' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	When the user populates Net weight as '100'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '100'
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	When the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user enters date of issue for the next notification "yesterday"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user clicks Record decision from the header
	And the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	Then the Decision Hub page should be displayed
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks on Save and continue button on the Checks page
	Then the Seal numbers page should be displayed
	When the user select 'No' radio button on the Seal numbers page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select 'No' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Decision page should be displayed
	And the main radio option 'Internal market' and the sub radio option 'Human consumption' are selected by default
	And the user clicks Save and continue
	And the Review outcome decision page should be displayed
	And the details reflect the information added for CHED D
	When the user Clicks the change link under 'Decision information'
	Then the Decision page should be displayed	
	When the user selects 'Other' 'Internal market' in decision page
	Then the user clicks Save and continue
	And the Review outcome decision page should be displayed
	And the details reflect the information added for CHED D
	When the user Clicks the change link under 'Decision information'
	Then the Decision page should be displayed
	When the user selects '' 'Non-internal market' in decision page
	Then the user clicks Save and continue
	And the Review outcome decision page should be displayed
	And the details reflect the information added for CHED D
	When the user Clicks the change link under 'Decision information'
	Then the Decision page should be displayed
	When the user selects '' 'Not acceptable' in decision page
	Then the user clicks Save and continue
	And the Reason for Refusal page should be displayed
	When the user selects "Chemical Contamination" as reason for refusal
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added for CHED D
	When the user Clicks the change link under 'Reason for refusal'
	Then the Reason for Refusal page should be displayed
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Decision link
	Then the Decision page should be displayed
	And 'Not acceptable' radio is pre-selected under Acceptable for
	When the user selects 'Destruction' 'Not acceptable' in decision page
	And the user enters 'Obsolete' in reason under Destruction
	And the user clicks Save and continue
	Then the Select a controlled destination page should be displayed
	When the user clicks on Cancel link
	And the user selects 'Re-dispatching' 'Not acceptable' in decision page
	Then the user clicks Save and continue
	And the Reason for Refusal page should be displayed
	When the user clicks on Cancel link
	Then the Decision Hub page should be displayed
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user selects 'Transformation' 'Not acceptable' in decision page
	Then the user clicks Save and continue
	And the Select a controlled destination page should be displayed
	When the user clicks Add a controlled destination
	Then the Search for an existing controlled destination page should be displayed
	When the user selects a controlled destination
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user clicks Save and continue
	When the user Clicks the change link under 'Decision information'
	Then the Decision page should be displayed
	When the user clicks Save and continue
	Then the chosen controlled destination should be displayed
	When the user clicks on Cancel link
	Then the Decision page should be displayed
	When the user selects 'Use for other purpose' 'Not acceptable' in decision page
	When the user enters 'future' date in By date
	When the user clicks Save and continue
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added for CHED D
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	When the user populates the Date and time of checks
	When user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And a border notification banner displaying the reason for refusal 'Refused for chemical contamination reasons' under the title 'The result of this decision requires a border notification to be created' is displayed
	And the user verfies the decision outcome as 'Not Acceptable'
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user downloads the PDF for validation
	And the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user clicks Record control in Dashboard page
	Then the Consignments requiring control page should be displayed
	When the user searches for the CHED number
	Then the notification should be found with the status "Rejected"
	And the user verifies the control status is 'CONTROL REQUIRED'
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User submits notification of no inspection required, inspector overrides the risk decision and make the notification acceptable - CHEDD 8581
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'High risk food and feed of non-animal origin' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "Zambia" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Zambia" as the Country of origin and Country from where consigned
	When the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches for first commodity code '39241000'
	Then the commodity details should be populated '39241000' 'Kitchenware containing polyamide or melamine' for first commodity
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons for CHEDD
	When The user selects 'Internal market' radio option
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	And the user populates Net weight as '100'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '100'
	And the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Human consumption' radio button under Commodity intended for on the Additional details page
	And the user selects 'Chilled' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Air waybill"
	And the user enters Document reference "INV12345"
	And the user selects a previous date from the date picker
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
	Then the chosen place of destination should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contacts - Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification for CHED D
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Gateway Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user clicks Record decision from the header
	And the user searches for the newly created notification on the Import notifications page
	Then the notification should be found with the status "NEW"
	And the notification should be found with risk outcome "NO INSPECTION"
	When the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks override the risk decision
	And the user clicks Yes, override risk decision button
	Then the Decision Hub page should be displayed
	And the user verifies 'Inspection required' box appears in the page
	And the text 'This risk decision was overridden from no inspection required to inspection required. The consignment will be brought to the BCP for inspection.' is displayed
	And the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Checks page should be displayed
	When the user selects 'Satisfactory' radio button under Documentary check on the Checks page
	And the user selects 'Yes' radio button under Identity check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Identity check main radio
	And the user selects 'Yes' radio button under Physical check on the Checks page
	And the user selects 'Satisfactory' sub radio button under the Physical check main radio
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects 'Human consumption' 'Internal market' in decision page
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks Record decision from the header
	And the user searches for the CHED number
	Then the notification should be found with the status "VALID"
	And the notification should be found with risk outcome "INSPECT"
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully