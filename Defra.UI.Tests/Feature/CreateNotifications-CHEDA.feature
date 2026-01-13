@Regression
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
	And the user clicks on Add attachment link on the Latest Health Certificate page
	And the user uploads the Veterinary Health Certificate 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the Veterinary Health Certificate 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Commercial invoice'
	And the user enters Document reference 'INV12345'
	And the user enters date of issue '24''11''2025'
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
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
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF'
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
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Heathrow Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notificaiton found with status 'NEW'
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from 'NEW' to 'IN PROGRESS'
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects 'Satisfactory' for the Documentary check and clicks Save and continue
	Then the Identity, physical and welfare checks page should be displayed
	When the user selects 'Satisfactory' for Identity check
	And the user selects 'Satisfactory' for Physical check
	And the user selects '4' for Number of animals checked
	And the user selects 'Satisfactory' for Welfare check
	And the user selects '25' '%' for Number of dead animals
	And the user selects '3' 'unit' for Number of unfit animals
	And the user selects '0' for Number of births or abortions
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Approved bodies'
	And the user clicks Save and continue
	Then the Select a controlled destination page should be displayed
	When the user clicks Add a controlled destination
	Then the Search for an existing controlled destination page should be displayed
	When the user selects a controlled destination
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When I am logged in to the 'PIMS' app as 'Caseworker'
	And I open the sub area 'Importer Notifications' under the 'Case Management' area
	And I search Importer Notifications for the notification created in IPAFFS
	And I open the record at position '0' in the grid

Scenario: User creates and submits a B2C consignment notification for Temporary admission horses with laboratory tests - CHEDA
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
	When the user chooses 'Thailand' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Thailand' as the Country of origin and Country from where consigned
	When the user chooses 'No' for Does your consignment require a region code?
	And the user enters a reference number 'This is a CHED-A test' in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0101' commodity code
	Then the commodity details should be populated '0101' 'Live horses, asses, mules and hinnies'
	When the user selects species of commodity 'Equus caballus'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Temporary admission horses' as the main reason for importing the consignment
	And the user enters exit date '8' days from today
	And the user selects exit BCP 'London Borough of Hillingdon Heathrow Airport Imported Food Office - ADADA'
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '1'
	And the user populates Number of packages as '1'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates the Horse name as 'Thunder'
	And the user populates the Microchip number as 'MC123456789'
	And the user populates the Passport number as 'PP987654321'
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Approved bodies' for What are the animals certified for?
	And the user selects 'No' for Does the consignment contain any unweaned animals?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'VHC12345'
	And the user enters Latest Health Certificate date of issue '01''12''2025'
	And the user clicks on Add attachment link on the Latest Health Certificate page
	And the user uploads the Veterinary Health Certificate 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the Veterinary Health Certificate 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
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
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user populates the transport details 'London Borough of Hillingdon Heathrow Airport Imported Food Office - ADADA' 'Yes' 'Airplane' 'BA1234' 'Doc1234'
	And the user enters Container Number 'CONT123456'
	And the user enters Seal Number 'SEAL789012'
	And the user ticks the checkbox to confirm an official seal is affixed
	And the user clicks Save and continue
	Then the Transport after the BCP or Port of entry page should be displayed
	When the user selects means of transport after BCP 'Road vehicle'
	And the user enters transport identification after BCP 'TRANS456'
	And the user enters transport document reference after BCP 'TDOC789'
	And the user enters departure date from BCP '2' days later than arrival date
	And the user enters departure time from BCP '14:30'
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Notification Hub page should be displayed
	When the user clicks the Transport contacts hyperlink
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
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Heathrow Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notificaiton found with status 'NEW'
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from 'NEW' to 'IN PROGRESS'
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects 'Satisfactory' for the Documentary check and clicks Save and continue
	Then the Identity, physical and welfare checks page should be displayed
	When the user selects 'Satisfactory' for Identity check
	And the user selects 'Satisfactory' for Physical check
	And the user selects '1' for Number of animals checked
	And the user selects 'Satisfactory' for Welfare check
	And the user selects '0' '%' for Number of dead animals
	And the user selects '0' 'unit' for Number of unfit animals
	And the user selects '0' for Number of births or abortions
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select 'Yes' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Reason for testing page should be displayed
	When the user select 'Suspicion' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Select the commodity sampled page should be displayed
	When the user clicks the Select link for the '0101' commodity code
	Then the Laboratory tests Commodity to be tested page should be displayed
	When the user selects '.AMINOGLYCOSIDE/AMINOSIDE' from the list of Laboratory tests	
	Then the Laboratory tests Commodity sampled page should be displayed
	And the Sample date and time is todays date with the time the lab test was selected
	When the user populates the commodity sample details 'Initial analysis' 'Campden BRI' '12345' '3' 'Blood' 'Chilled'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	And the user verifies the data in Laboratory tests review page
	When the user clicks Save and continue
	Then the Decision page should be displayed
	And the 'Temporary admission horses' radio button option is pre populated
	And the exit date is pre populated
	And the exit BCP is correct
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the user should see an error message 'Lab results pending for this consignment' in review page
	When the user Clicks the change link under 'Laboratory tests'
	Then the Laboratory tests page should be displayed
	When the user select 'Yes' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Reason for testing page should be displayed
	When the user select 'Suspicion' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	When the user clicks on the Test '.AMINOGLYCOSIDE/AMINOSIDE'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	When the user enters Released date as '16''12''2025'
	When the user selects 'Not satisfactory' for Conclusion
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Review And Submit link
	Then the Review outcome decision page should be displayed
	And the user should not see an error message 'Lab results pending for this consignment' in review page
	And the details reflect the information added
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User creates and submits a B2C consignment notification with Transit through multiple countries - CHEDA
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
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0103' commodity code
	Then the commodity details should be populated '0103' 'Live swine'
	When the user selects species of commodity 'Sus scrofa domesticus'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user verifies 'Internal market' radio button exists with 11 sub-options for 'Purpose in the internal market'
	And the user verifies 'Transhipment or onward travel' radio button exists with 'Destination country' dropdown
	And the user verifies 'Transit' radio button exists with 'Exit border control post' dropdown and 'Destination country' dropdown
	And the user verifies 'Re-entry' radio button exists with no sub-options
	And the user verifies 'Temporary admission horses' radio button exists with 'Exit date' fields and 'Exit border control post' dropdown
	When the user chooses 'Transit' as the main reason for importing the consignment
	And the user selects exit BCP 'Manchester Airport (animals) - GBMNC4'
	And the user selects destination country 'Qatar'
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
	And the user clicks on Add attachment link on the Latest Health Certificate page
	And the user uploads the Veterinary Health Certificate 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the Veterinary Health Certificate 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type 'Commercial invoice'
	And the user enters Document reference 'INV12345'
	And the user enters date of issue from last week
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
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
	Then the Add the County Parish Holding number (CPH) page should be displayed
	When the user clicks Save and continue
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user populates the transport details 'London Borough of Hillingdon Heathrow Airport Imported Food Office - ADADA' 'Yes' 'Road vehicle' '123456' 'Doc1234'
	And the user enters Container Number 'CONT123456'
	And the user enters Seal Number 'SEAL789012'
	And the user ticks the checkbox to confirm an official seal is affixed
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
	And the user clicks Add another country
	And the user selects 'Malawi' for Will the consignment travel through any other countries before reaching the UK?
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
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Heathrow Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notificaiton found with status 'NEW'
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from 'NEW' to 'IN PROGRESS'
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects 'Satisfactory' for the Documentary check and clicks Save and continue
	Then the Identity, physical and welfare checks page should be displayed
	When the user selects 'Satisfactory' for Identity check
	And the user selects 'Satisfactory' for Physical check
	And the user selects '1' for Number of animals checked
	And the user selects 'Satisfactory' for Welfare check
	And the user selects '0' 'unit' for Number of dead animals
	And the user selects '100' '%' for Number of unfit animals
	And the user selects '0' for Number of births or abortions
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select 'Yes' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Reason for testing page should be displayed
	When the user select 'Random' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Select the commodity sampled page should be displayed
	When the user clicks the Select link for the '0103' commodity code
	Then the Laboratory tests Commodity to be tested page should be displayed
	When the user clicks select link of one of the Laboratory test
	Then the Laboratory tests Commodity sampled page should be displayed
	And the Sample date and time is todays date with the time the lab test was selected
	When the user populates the commodity sample details 'Second expert analysis' 'Concept Life Sciences' 'RAND1234' '1' 'Adult' 'Ambient'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	And the user verifies the data in Laboratory tests review page
	When the user clicks Save and continue
	Then the Decision page should be displayed
	And the 'Transit' radio button option is pre populated
	And the exit BCP is prepopulated with value entered in Part 1
	And the destination country is prepopulated with value entered in Part 1
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User creates and submits a B2C consignment notification with inspector workflow and lab test validations - CHEDA
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
	When the user chooses 'Thailand' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Thailand' as the Country of origin and Country from where consigned
	When the user chooses 'No' for Does your consignment require a region code?
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '0101' commodity code
	Then the commodity details should be populated '0101' 'Live horses, asses, mules and hinnies'
	When the user selects species of commodity 'Equus caballus'
	And the user clicks Save and continue
	Then What is the main reason for importing the animals? page should be displayed with radio buttons
	When the user chooses 'Internal market' and the sub-option 'Transfer of ownership – Sale/gift'
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Number of animals as '2'
	And the user populates Number of packages as '2'
	And the user clicks Save and continue in commodity page
	Then the Enter animal identification details page should be displayed
	When the user populates the Horse name as 'Spirit'
	And the user clicks Save and continue
	Then the Additional animal details page should be displayed
	When the user selects 'Approved bodies' for What are the animals certified for?
	And the user selects 'No' for Does the consignment contain any unweaned animals?
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
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
	Then the Transport to the BCP or Port of entry page should be displayed
	When the user populates the transport details 'London Borough of Hillingdon Heathrow Airport Imported Food Office - ADADA' 'No' 'Airplane' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Transport after the BCP or Port of entry page should be displayed
	When the user selects means of transport after BCP 'Road vehicle'
	And the user enters transport identification after BCP 'TRANS789'
	And the user enters transport document reference after BCP 'TDOC456'
	And the user enters departure date from BCP '2' days later than arrival date
	And the user enters departure time from BCP '15:00'
	And the user clicks Save and continue
	Then the Transporter page should be displayed
	When the user clicks Add a transporter
	Then the Search for an existing transporter page should be displayed
	When the user selects any one of the displayed transporters
	Then the chosen transporter should be displayed on the Transporter page
	When the user clicks Save and continue in Transporter page
	Then the Notification Hub page should be displayed
	When the user clicks the Transport contacts hyperlink
	Then the Transport Contacts page should be displayed
	When the user selects 'No' for Should we notify any transport contacts about inspections?
	And the user clicks Save and continue
	Then the Contact address for consignment page should be displayed without the secondary title
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user should not see any error messages in review page
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
	Then the notification returned in the search has the status 'NEW'
	And the Amend link should be available for the notification
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Heathrow Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notificaiton found with status 'NEW'
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from 'NEW' to 'IN PROGRESS'
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When user searches for the import notification
	Then the notification should be present in the list
	And the Amend link should not be available for the notification
	And the Copy as new link should be available for the notification
	And the View details link should be available for the notification
	And the Show notification link should be available for the notification
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Heathrow Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notificaiton found with status 'IN PROGRESS'
	And the Decision Hub page should be displayed
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects 'Satisfactory' for the Documentary check and clicks Save and continue
	Then the Identity, physical and welfare checks page should be displayed
	When the user selects 'Satisfactory' for Identity check
	And the user selects 'Satisfactory' for Physical check
	And the user selects '2' for Number of animals checked
	And the user selects 'Satisfactory' for Welfare check
	And the user selects '0' '%' for Number of dead animals
	And the user selects '0' 'unit' for Number of unfit animals
	And the user selects '0' for Number of births or abortions
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select 'Yes' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Reason for testing page should be displayed
	When the user select 'Suspicion' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Select the commodity sampled page should be displayed
	When the user clicks the Select link for the '0101' commodity code
	Then the Laboratory tests Commodity to be tested page should be displayed
	When the user selects '.AMINOGLYCOSIDE/AMINOSIDE' from the list of Laboratory tests
	Then the Laboratory tests Commodity sampled page should be displayed
	When the user populates the commodity sample details 'Initial analysis' 'Campden BRI' '12345' '1' 'Blood' 'Chilled'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	When the user clicks the Add a laboratory test link
	Then the Laboratory tests Select the commodity sampled page should be displayed
	When the user clicks the Select link for the '0101' commodity code
	Then the Laboratory tests Commodity to be tested page should be displayed
	When the user selects '.ANTITHYROID AGENTS' from the list of Laboratory tests
	Then the Laboratory tests Commodity sampled page should be displayed
	When the user populates the commodity sample details 'Initial analysis' 'Concept Life Sciences' '67890' '1' 'Blood' 'Frozen'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	When the user clicks the Add a laboratory test link
	Then the Laboratory tests Select the commodity sampled page should be displayed
	When the user clicks the Select link for the '0101' commodity code
	Then the Laboratory tests Commodity to be tested page should be displayed
	When the user selects '.CHEMICAL ELEMENTS' from the list of Laboratory tests
	Then the Laboratory tests Commodity sampled page should be displayed
	When the user populates the commodity sample details 'Initial analysis' 'Concept Life Sciences' '67890' '1' 'Blood' 'Ambient'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	And the user verifies multiple Laboratory tests are entered with Results 'Pending'
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Approved bodies'
	And the user clicks Save and continue
	Then the Select a controlled destination page should be displayed
	When the user clicks Add a controlled destination
	Then the Search for an existing controlled destination page should be displayed
	When the user selects a controlled destination
	Then the chosen controlled destination should be displayed
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the user should see an error message 'A document type must be entered' in review page
	And the user should see an error message 'Lab results pending for this consignment' in review page
	When the user Clicks the change link under 'Laboratory tests'
	Then the Laboratory tests page should be displayed
	When the user clicks Save and continue
	Then the Laboratory tests Reason for testing page should be displayed
	When the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	When the user clicks on the Test '.AMINOGLYCOSIDE/AMINOSIDE'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	When the user enters Released date as '16''12''2025'
	When the user selects 'Satisfactory' for Conclusion
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	When the user clicks on the Test '.ANTITHYROID AGENTS'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	When the user enters Released date as '16''12''2025'
	When the user selects 'Satisfactory' for Conclusion
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	When the user clicks on the Test '.CHEMICAL ELEMENTS'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	When the user enters Released date as '16''12''2025'
	When the user selects 'Satisfactory' for Conclusion
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	And the user verifies multiple Laboratory tests are displayed with Results 'Satisfactory'
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Review And Submit link
	Then the Review outcome decision page should be displayed
	And the user should not see an error message 'Lab results pending for this consignment' in review page
	And the user should see an error message 'A document type must be entered' in review page
	When the user Clicks the change link under 'Documents'
	Then the Documents page should be displayed
	And the Add another document link is displayed
	And the user verifies there are no documents in the Inspector section
	When the user selects Document type 'Commercial invoice'
	And the user enters Document reference 'INSP12345'
	And the user enters date of issue '05''12''2025'
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Inspector Document' in the format '.docx'
	Then the document 'IPAFFS Inspector Document' '.docx' is uploaded successfully
	And the user verifies the entered document information is displayed correctly
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the user should not see any error messages in review page
	When the user selects the radio button to declare that the checks have been carried out in accordance with EU law
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When user searches for the import notification
	Then the notification should be present in the list
	And the notification returned in the search has the status 'VALID'
	And the Amend link should not be available for the notification
	When the user clicks View details for the notification
	Then the Review your notification page should be displayed
	And the Copy as new button should be available
	And the View CHED button should be available
	And the Change links should not be available
	When the user clicks on the Dashboard link
	Then the dashboard page should be displayed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully

Scenario: User adds addresses to address book and amends a CHEDA notification to use new traders - SPS-7387
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Address book link
	Then the Address book page should be displayed
	When the user clicks Add an address
	Then the Choose address type page should be displayed with 'Operator address' and 'My organisation branch address' radio buttons
	When the user selects address type 'Operator address' and clicks Continue
	Then the Choose operator type page should be displayed with 'Importer' 'Exporter' 'Transporter' and 'Packer' radio buttons
	When the user selects operator type 'Importer' and clicks Continue
	Then the Add operator details page should be displayed
	When the user adds the operator 'Importer' details	
	And the user clicks Save and Continue
	Then the address has been added to your address book page should be displayed
	And Continue from here
	When the user clicks Return to Address Book
	Then the Address Book page should be displayed
	And the newly added address 'Test Importer SPS7387' should be displayed in the address book
	When the user clicks Add an address
	Then the Choose address type page should be displayed with 'Operator address' and 'My organisation branch address' radio buttons
	When the user selects 'Operator address' and clicks Continue
	Then the Choose Operator Type page should be displayed with 'Importer' 'Exporter' 'Transporter' and 'Packer' radio buttons
	When the user selects 'Exporter' radio button and clicks Continue
	Then the Add Operator Details page should be displayed
	When the user enters operator name 'Test Exporter SPS7387'
	And the user enters operator address line 1 'Exporter Address Line 1'
	And the user enters operator address line 2 'Exporter Address Line 2'
	And the user enters operator city or town 'Exporter City'
	And the user enters operator postcode 'EX1 1EX'
	And the user selects operator country 'France'
	And the user enters operator telephone number '07598765432'
	And the user enters operator email 'testexporter@test.com'
	And the user clicks Save and Continue
	Then the address has been added to your address book page should be displayed
	When the user clicks Return to Address Book
	Then the Address Book page should be displayed
	And the newly added address 'Test Exporter SPS7387' should be displayed in the address book
	When the user clicks Add an address
	Then the Choose address type page should be displayed with 'Operator address' and 'My organisation branch address' radio buttons
	When the user selects 'Operator address' and clicks Continue
	Then the Choose Operator Type page should be displayed with 'Importer' 'Exporter' 'Transporter' and 'Packer' radio buttons
	When the user selects 'Transporter' radio button and clicks Continue
	Then the Transporter type selection page should be displayed
	When the user selects 'Private transporter' and clicks Continue
	Then the Add Operator Details page should be displayed
	When the user enters operator name 'Test Transporter SPS7387'
	And the user enters operator address line 1 'Transporter Address Line 1'
	And the user enters operator address line 2 'Transporter Address Line 2'
	And the user enters operator city or town 'Transporter City'
	And the user enters operator postcode 'TR1 1TR'
	And the user selects operator country 'United Kingdom'
	And the user enters operator telephone number '07511223344'
	And the user enters operator email 'testtransporter@test.com'
	And the user clicks Save and Continue
	Then the address has been added to your address book page should be displayed
	When the user clicks Return to Address Book
	Then the Address Book page should be displayed
	And the newly added address 'Test Transporter SPS7387' should be displayed in the address book
	When the user clicks Dashboard link
	Then the dashboard page should be displayed
	When user searches for a CHEDA import notification with status 'NEW'
	Then a notification with status 'NEW' should be found
	When the user clicks Amend
	Then the Notification Hub page should be displayed
	And the notification reference number version should be incremented by 1
	When the user clicks on 'Addresses' link
	Then the Addresses page should be displayed
	When the user clicks on Change link under 'Consignor or exporter'
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects the consignor or exporter 'Test Exporter SPS7387' from address book
	Then the chosen consignor or exporter 'Test Exporter SPS7387' should be displayed on the Addresses page
	When the user clicks on Change link under 'Consignee'
	Then the Search for an existing consignee page should be displayed
	When the user selects the consignee 'Test Importer SPS7387' from address book
	Then the chosen consignee 'Test Importer SPS7387' should be displayed on the Addresses page
	When the user clicks on Change link under 'Importer'
	Then the Search for an existing importer page should be displayed
	When the user selects the importer 'Test Importer SPS7387' from address book
	Then the chosen importer 'Test Importer SPS7387' should be displayed on the Addresses page
	When the user clicks on Change link under 'Place of destination'
	Then the Search for an existing place of destination page should be displayed
	When the user selects the place of destination 'Test Importer SPS7387' from address book
	Then the chosen place of destination 'Test Importer SPS7387' should be displayed on the Addresses page
	When the user clicks on Save and return to hub
	Then the Notification Hub page should be displayed
	When the user clicks on 'Review and submit' link
	Then the Review your notification page should be displayed
	And the new traders should be displayed in the review page
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user ticks the checkbox to declare that the information is true and correct
	And the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	And the new traders 'Test Exporter SPS7387' 'Test Importer SPS7387' should be displayed in the certificate
	When the user closes the PDF browser tab
	Then the browser tab is closed
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully