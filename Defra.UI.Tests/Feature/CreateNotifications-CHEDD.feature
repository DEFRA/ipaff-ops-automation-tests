@Regression
Feature: CreateNotification CHEDD

Create a notification for CHEDD type

Scenario: User creates and submits a B2C consignment notification - CHED D
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
	Then the Origin of the plants plant product or other objects page should be displayed
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
	And the user selects the second commodity '100610' 'Rice in the husk (paddy or rough)' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	When the user populates Net weight as '18000' for the second commodity '100610'
	And the user populates Number of packages as '1' for the second commodity '100610'
	And the user selects type of package as 'Box' for the second commodity '100610'
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
	And the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects any one of the displayed consignors or exporters
	Then the chosen consignor or exporter should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee
	Then the chosen consignee should be displayed
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination
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
	Then the user clicks the notificaiton found with status "NEW"
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
	And the user validates the commodity code "12024200", description "Shelled, whether or not broken", quantity "19000", authority "FNAO" and decision "Acceptable for Internal Market" for commodity "1" after the decision is given
	And the user validates the commodity code "100610", description "Rice in the husk (paddy or rough)", quantity "18000", authority "FNAO" and decision "Acceptable for Internal Market" for commodity "2" after the decision is given	
	When the user logs out of BTMS
	Then the user should be logged out successfully