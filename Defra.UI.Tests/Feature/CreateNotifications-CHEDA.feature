Feature: Create Notification CHEDA

Create a notification for CHEDA type

Scenario: User creates and submits a B2C consignment notification - CHEDA
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Live animals' option
	And the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses 'Tanzania' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Tanzania' as the Country of origin and Country from where consigned
	When the user chooses 'No' for Does your consignment require a region code?
	And the user enters a reference number 'This is a CHED-A test' in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0103' commodity code
	Then the commodity details should be populated '0103' 'Live swine'
	When the user selects species of commodity 'Sus scrofa domesticus'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Internal market' and the sub-option 'Research'
	And the user clicks Save and continue	
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '3'
	And the user populates Number of packages as '3'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates the Ear tag as 'ET1234'
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Approved bodies' for What are the animals certified for?
	And the user selects 'No' for Does the consignment contain any unweaned animals?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'VHC12345'
	And the user enters Latest Health Certificate date of issue '01''12''2025'
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Commercial invoice'
	And the user enters Document reference 'INV12345'
	And the user enters date of issue '24''11''2025'
	And the user clicks Save and continue	
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter
	Then the chosen consignor or exporter should be displayed
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
	Then the Add the County Parish Holding number (CPH) page should be displayed
	When the user enters the CPH number '12/345/6789/0001'
	And the user clicks Save and continue
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user populates the transport details 'London Borough of Hillingdon Heathrow Airport Imported Food Office - ADADA' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Notification Hub page should be displayed
	When the user clicks the Countries the consignment will travel through hyperlink 
	Then the Which countries will the consignment travel through? page should be displayed
	When the user selects 'Kenya' for Will the consignment travel through any other countries before reaching the UK?
	And the user clicks Save and continue
	Then the Transport Contacts page should be displayed
	When the user selects 'No' for Should we notify any transport contacts about inspections?
	And the user clicks Save and continue
	Then the Contact address for consignment page should be displayed without the secondary title
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the data presented for review matches the data entered into the notification
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully