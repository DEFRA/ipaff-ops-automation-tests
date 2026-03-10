@Regression
Feature: Create Notification CHEDP

Create a notification for CHEDP type

Scenario: User creates and submits a B2C consignment notification - CHEDP Happy Path
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '41015050' commodity code
	Then the commodity details should be populated '41015050' 'Dried or dry-salted'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bison bison'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Animal feedingstuff"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Low risk" risk category
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
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	Then the user should be able to click Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
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
	And I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user checks commodity code "41015050", description "Bison bison", quantity "1000", authority "POAO" and decision "Decision not given"
	When the user logs out of BTMS
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Full identity check" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user checks commodity code "41015050", description "Bison bison", quantity "1000", authority "POAO" and decision "Acceptable for Internal Market" after the decision given
	When the user logs out of BTMS
	Then the user should be logged out successfully

Scenario: User creates and submits a B2C consignment notification for Transit Reason - CHEDP 7369
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '16042005' commodity code
	Then the commodity details should be populated '16042005' 'Preparations of surimi'
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Other'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Transit" and the sub-option ""
	And the user chooses exit BCP "GATWICK (GBLGW)" transited country "Germany" and destination country "Qatar"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
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
	Then the Catch cerificates page should be displayed
	And the user selects "No" option for add catch certificate
	When the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the document 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user should be able to click Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Add an importer
	Then the Search for an existing importer page should be displayed
	When the user selects an importer "DEF" with a UK country
	Then the chosen importer should be displayed on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
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
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the IUU page should be displayed
	When the user selects "Yes" and sub-option as "No need to inspect - exempt or not applicable" for the IUU check
	And the user clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Full identity check" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Seal numbers link
	Then the Seal numbers page should be displayed
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Laboratory tests link
	Then the Laboratory tests page should be displayed
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Decision link
	Then the Decision page should be displayed
	And the user verifies the Transit radio button option is pre populated
	And verifies exit BCP Transited country and Destination country are pre populated from part 1
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED button
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "Valid"
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User creates and submits a B2C consignment notification for Transhipment or onward travel Reason - CHEDP 7370
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '41015050' commodity code
	Then the commodity details should be populated '41015050' 'Dried or dry-salted'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bison bison'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Transhipment or onward travel" and the sub-option ""
	And the user chooses destination country "Austria"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
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
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document(1)' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
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
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Not Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects "Re-dispatching" "Not acceptable" in decision page
	And the user enters currendate in decision page
	And the user clicks Save and continue
	Then the Reason for Refusal page should be displayed
	When the user selects "Invasive Alien Species" as reason for refusal
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks return to your dashboard link in decision submitted page
	Then the Import notifications dashboard page should be displayed
	When user searches for the import notification after decision submission
	Then the notification should be present in the list of part 2 dashboard
	When the user clicks View CHED link
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user clicks Record control in Dashboard page
	Then the Consignments requiring control page should be displayed
	When the user searches for the CHED number	
	Then the notification should be found with the status "Rejected"
	And the user verifies the control status is "CONTROL REQUIRED"
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User creates and submits 2 B2C consignment notification with existing Billing details - CHEDP 7365
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '97052200' commodity code
	Then the commodity details should be populated '97052200' 'Extinct or endangered species and parts thereof'
	When the user selects the type of commodity 'Game trophies'
	And the user selects species of commodity 'Cervidae'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
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
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document(1)' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "DOVER EAST (SEVINGTON BCP) (GBSEV)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Confirm billing details page should be displayed
	When the user clicks read about rates and eligibility (opens in new tab) link in the confirm billing details page
	Then the "Common user charge: rates, eligibility and invoices" page should be opened in new tab
	When the user closes the tab
	Then the new tab should be closed
	When the user clicks read the terms and conditions (opens in new tab) link in the confirm billing details page
	Then the "Paying the common user charge: terms and conditions" page should be opened in new tab
	When the user closes the tab
	Then the new tab should be closed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	When the user clicks Return to your dashboard
	Then the dashboard page should be displayed
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '97052200' commodity code
	Then the commodity details should be populated '97052200' 'Extinct or endangered species and parts thereof'
	When the user selects the type of commodity 'Game trophies'
	And the user selects species of commodity 'Cervidae'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
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
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Accompanying documents page should be displayed
	When the user selects Document type for the next notification "Commercial invoice"
	And the user enters Document reference for the next notification "INV12345"
	And the user enters date of issue for the next notification "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document(1)' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "FOLKESTONE - EUROTUNNEL (SEVINGTON BCP) (GBFOLS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Confirm billing details page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully

Scenario: Admin submits a notification and records decision and validate cookies page as normal user - CHEDP 7368
	Given that I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user clicks Create notification in Dashboard page header
	And the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Italy" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Italy" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '16051000' commodity code
	Then the commodity details should be populated '16051000' 'Crab'
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Geryon maritae'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
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
	Then the Catch cerificates page should be displayed
	And the user selects "No" option for add catch certificate
	When the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document(1)' '.docx' is uploaded successfully
	And the user should be able to click Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "Italy" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "abc"
	Then the chosen consignor or exporter "abc" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "Vera Importation Emporium" with a UK country
	Then the chosen consignee "Vera Importation Emporium" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "Vera Importation Emporium" on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee "Vera Importation Emporium" on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	When the user records the IPAFFS User details and CHED Reference
	Then the details should be recorded
	When the user clicks Record decision from the header
	And the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the IUU page should be displayed
	When the user selects "Yes" and sub-option as "No need to inspect - exempt or not applicable" for the IUU check
	And the user clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select "Yes" radio button on the Laboratory tests page
	And the user clicks Save and continue
	And the user select 'Random' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks the Select link for the '16051000' commodity code
	Then the Laboratory tests page should be displayed
	When the user clicks select link of one of the Laboratory test
	Then the Laboratory tests Commodity sampled page should be displayed
	When the user populates the commodity sample details 'Initial analysis' 'Campden BRI' '12345' '3' 'Blood' 'Chilled'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	And the user verifies the data in Laboratory tests review page
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED button
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Cookies link from the footer of the page
	Then the Cookies page should be displayed
	When the user clicks Import of products, animals, food and feed service link on the header
	Then the dashboard page should be displayed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully

Scenario: User creates a B2C consignment notification, updates it from the review page, submits it, amends the notification, and sends it for laboratory tests - CHEDP 7371
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '020110' commodity code
	Then the commodity details should be populated '020110' 'Carcases and half-carcases'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bos taurus'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
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
	Then the total gross weight should be greater than the net weight ''
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects '' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	Then the user should be able to click Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "LIVERPOOL (GBLIV)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user should see an error message 'Temperature of the consignment' in review page
	And the user should see an error message 'Total gross weight' in review page
	When the user Clicks the change link under 'Description of the goods'
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '2106'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '2200'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Frozen' radio button on the Additional details page
	And the user clicks on Save and review
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
	When the user Clicks the change link under 'Transport'
	And the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under '1 additional document'
	Then the Accompanying documents page should be displayed
	When the user clicks the Add a document link
	And the user selects Document type "Veterinary health certificate"
	And the user enters Document reference "INV54321"
	And the user enters date of issue "04/12/2025"
	And the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under 'Approved establishment'
	Then the Approved establishment of origin page should be displayed
	When the user removes the establishment of origin
	And the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under 'Traders'
	Then the Addresses page should be displayed
	When the user clicks on Change link under 'Consignor or exporter'
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks on Change link under 'Consignee'
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks on Change link under 'Importer'
	Then the Search for an existing importer page should be displayed
	When the user selects an importer "DEF" with a UK country
	Then the chosen importer should be displayed on the Addresses page
	When the user clicks on Change link under 'Place of destination'
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under 'Contact address for consignment'
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks on Save and review
	Then the user verifies all the data displayed in review page for commodity code "160"
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
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
	And user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Amend
	Then the Notification Hub page should be displayed
	When the user clicks on 'Origin of the import' link
	And the user chooses "Finland" from the dropdown for Country of origin
	And the user clicks on Save and return to hub
	And the user clicks on 'Addresses' link
	Then the Addresses page should be displayed
	When the user clicks on Change link under 'Consignor or exporter'
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks on Change link under 'Consignee'
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks on Change link under 'Importer'
	Then the Search for an existing importer page should be displayed
	When the user selects an importer "DEF" with a UK country
	Then the chosen importer should be displayed on the Addresses page
	When the user clicks on Change link under 'Place of destination'
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks on Save and return to hub
	And the user clicks on 'Review and submit' link
	And the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	When the user clicks Return to your dashboard
	And the user clicks Show notification
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And the user clicks Save and continue
	And the Laboratory tests page should be displayed
	When the user select 'Yes' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user select 'Suspicion' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks the Select link for the '020110' commodity code
	Then the Laboratory tests page should be displayed
	When the user selects 'Animal diseases' in Laboratory test category
	And the user selects 'Cattle diseases' in Laboratory test subcategory
	And the user clicks on Search
	And the user selects 'Anaplasma marginale' from the list of Laboratory tests
	And the user populates the commodity sample details 'Initial analysis' 'Concept Life Sciences' '987' '12' 'Cuttings' 'Frozen'
	And the user clicks Save and continue
	And the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the user should see an error message 'Lab results pending for this consignment' in review page
	When the user Clicks the change link under 'Laboratory tests'
	And the user clicks Save and continue
	And the user clicks Save and continue
	And the user clicks on the Test 'Anaplasma marginale'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	And the user enters Released date as '16''12''2025'
	And the user selects "Satisfactory" for Conclusion
	And the user clicks Save and continue
	And the user clicks Save and Return
	And the user clicks Review And Submit link
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED button
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User creates and submits a notification, override the risk decision and reject the notification - CHEDP 7372
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '41015050' commodity code
	Then the commodity details should be populated '41015050' 'Dried or dry-salted'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bison bison'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Animal feedingstuff"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Low risk" risk category
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
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	Then the user should be able to click Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
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
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks override the risk decision
	Then the Override risk decision page should be displayed
	When the user clicks Yes, override risk decision button
	Then the Decision Hub page should be displayed
	And the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user select 'Yes' radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests Reason for testing page should be displayed
	When the user select 'Suspicion' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks the Select link for the '41015050' commodity code
	Then the Laboratory tests page should be displayed
	When the user selects 'Animal diseases' in Laboratory test category
	And the user selects 'Cattle diseases' in Laboratory test subcategory
	And the user clicks on Search
	And the user selects 'Anaplasma marginale' from the list of Laboratory tests
	Then the Laboratory tests Commodity sampled page should be displayed
	When the user populates the commodity sample details 'Initial analysis' 'Campden BRI' '12345' '3' 'Blood' 'Chilled'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	And the user verifies the data in Laboratory tests review page
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the user checks the reason should be 'Suspicious' in the review page
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the user should see an error message 'Lab results pending for this consignment' in review page
	When the user Clicks the change link under 'Laboratory tests'
	Then the Laboratory tests page should be displayed
	When the user select 'Yes' radio button on the Laboratory tests page
	And the user clicks Save and continue
	And the user select 'Suspicion' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks on the Test 'Anaplasma marginale'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	And the user enters Released date as '16''12''2025'
	And the user selects "Not satisfactory" for Conclusion
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the user should not see an error message 'Lab results pending for this consignment' in review page
	When the user Clicks the change link under 'Decision information'
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Not acceptable' 'Destruction'
	And the user enters 'Not satisfactory' in reason under Destruction
	And the user enters 'today' date in By date
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
	Then the user should see an error message 'Refused for microbiological contamination reasons' under title 'The result of this decision requires a border notification to be created' in checks submitted page
	And the user should see an error message 'Refused for unsatisfactory laboratory test results' under title 'The result of this decision requires a border notification to be created' in checks submitted page
	And the message 'Due to the reason of refusal, a border notification is required to be created to alert the FSA to a possible issue.' should be displayed under Next steps
	When the user clicks return to your dashboard link in decision submitted page
	Then the Import notifications dashboard page should be displayed
	When user searches for the import notification after decision submission
	Then the notification should be present in the list of part 2 dashboard
	And the notification should be found with the status "Rejected"
	When the user clicks CHEDP reference number
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
	When the user download the document attached in accompanying documents
	Then the user switch to next tab and open the browser downloads
	And verifies the document 'IPAFFS Test Document' downloaded successfully
	When the user closes the newly opened tab
	Then the browser tab is closed
	When the user clicks Save and continue
	Then Reveiw border notification page should be displayed
	When the user clicks submit button
	Then your border notification has been submitted page should be displayed
	When the user records the BN number
	And the user clicks Return to dashboard button
	Then Border notifications dashboard page should be displayed
	When the user searches for the newly created border notification
	Then the border notification found with status "NEW"
	When the user clicks the view details of the border notification
	Then the Border notification overview page should be displayed
	When the user clicks Dashboard link
	Then Border notifications dashboard page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User submits a notification as no inspection required, override the risk decision and checks Inspection required box - CHEDP 8582
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Italy" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Italy" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '16051000' commodity code
	Then the commodity details should be populated '16051000' 'Crab'
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Geryon maritae'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
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
	Then the Catch cerificates page should be displayed
	And the user selects "No" option for add catch certificate
	When the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	Then the user should be able to click Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "Italy" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
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
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks override the risk decision
	Then the Override risk decision page should be displayed
	When the user clicks Yes, override risk decision button
	Then the Decision Hub page should be displayed
	And the user verifies 'Inspection required' box appears in the page
	And the text 'This risk decision was overridden from no inspection required to inspection required. The consignment will be brought to the BCP for inspection.' is displayed
	And the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the IUU page should be displayed
	When the user selects "Yes" and sub-option as "No need to inspect - exempt or not applicable" for the IUU check
	And the user clicks Save and continue
	Then the Documentary check page should be displayed
	And the user verifies 'Inspection required' box appears in the page
	And the text 'This risk decision was overridden from no inspection required to inspection required. The consignment will be brought to the BCP for inspection.' is displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	And the user verifies 'Inspection required' box appears in the page
	And the text 'This risk decision was overridden from no inspection required to inspection required. The consignment will be brought to the BCP for inspection.' is displayed
	When the user selects "Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	And the user clicks Save and continue
	And the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User submits a notification, inspector copy it as replacement, update and submit decision - CHEDP 9112
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "China" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "China" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '230910' commodity code
	Then the commodity details should be populated '230910' 'Dog or cat food, put up for retail sale'
	When the user selects the type of commodity 'By-products / feedingstuff'
	And the user selects species of commodity 'Ungulates'
	And the user selects "Yes" for Do you want to add another commodity?
	And the user clicks Save and continue
	And the user searches '42050090' commodity code
	Then the commodity details should be populated '42050090' 'Other'
	When the user selects the type of commodity 'By-products / feedingstuff'
	And the user selects species of commodity 'Suidae'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Animal feedingstuff"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '3114,7113'
	And the user populates Number of packages as '10,15'
	And the user selects type of package as 'Box,Case'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '10227'
	And the Total Number of packages should be populated as '25'
	And the total gross weight should be greater than the net weight '15000'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the document 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "China" type "Freezing Vessel" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF"
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
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
	And I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user checks commodity code "230910", description "Ungulates", quantity "3114", authority "POAO" and decision "Decision not given"
	And the user checks commodity code "42050090", description "Suidae", quantity "7113", authority "POAO" and decision "Decision not given"
	When the user logs out of BTMS
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Full identity check" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the users opens a new tab
	And the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user checks commodity code "230910", description "Ungulates", quantity "3114", authority "POAO" and decision "Acceptable for Internal Market" after the decision given
	And the user checks commodity code "42050090", description "Suidae", quantity "7113", authority "POAO" and decision "Acceptable for Internal Market" after the decision given
	When the user logs out of BTMS
	Then the user should be logged out successfully
	When the user closes the tab
	Then the new tab should be closed
	When the user clicks return to your dashboard link in decision submitted page
	Then the Import notifications dashboard page should be displayed
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "VALID"
	And the CHED overview page should be displayed
	When the user clicks Copy as replacement button
	Then the Replace CHED page should be displayed
	When the user clicks Yes, replace this CHED
	Then the Notification overview page should be displayed
	And the status should be "IN PROGRESS"
	And the user records replaced CHED Reference number, Customs declaration reference and document code
	When the user clicks change in commodity section
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000,1000'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '2000'
	And the total gross weight should be greater than the net weight '3000'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks on Save and review
	Then the Notification overview page should be displayed
	And the status should be "MODIFY"
	And the total net weight should be updated to '2000 kg/units'
	And the total gross weight should be updated to '3000 kg/units'
	When the user clicks Set to in Progress button
	Then the status should be "IN PROGRESS"
	When the user clicks Record checks button
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Full identity check" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Human consumption'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks return to your dashboard link in decision submitted page
	Then the Import notifications dashboard page should be displayed
	When the user searches for the replacement notification
	Then the 'replacement' notification should be displayed with status "VALID"
	When the user searches for the original notification
	Then the 'original' notification should be displayed with status "REPLACED"
	And the user clicks the notification found with status "REPLACED"
	And the CHED overview page should be displayed for the 'original' notification
	And link should be displayed as Replaced by along with 'replacement' notification number
	When the user clicks Replaced by link
	Then the CHED overview page should be displayed for the 'replacement' notification
	And link should be displayed as Replaced by along with 'original' notification number
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user navigate to the BTMS application
	Then I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Government Gateway" as login type
	And I click Sign in button
	And I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the CHED status should be "Replaced" in BTMS search result page
	When I click the back button in the browser
	Then the BTMS search screen should be displayed
	When the user searches for the replacement CHED reference
	Then the BTMS search result screen should be displayed for the replacement CHED reference
	And the CHED status should be "Valid" in BTMS search result page
	When the user logs out of BTMS
	Then the user should be logged out successfully

Scenario: Create and Submit B2C Consignment with Two Commodities and Catch Certificates, Submits decision and CHED Verification in BTMS CHEDP_SPS_9113
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Italy" from the dropdown for Country of origin
	And the user clicks Save and continue
	And the user changes the consigned country to 'Sweden'
	And the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches for first commodity code '03061792'
	Then the commodity details should be populated '03061792' 'Shrimps of the genus Penaeus' for first commodity
	When the user selects the type of commodity 'Farmed stock'
	And the user selects species of commodity 'Penaeus spp.'
	And the user selects "Yes" for Do you want to add another commodity?
	And the user clicks Save and continue
	And the user searches '16052190' commodity code
	Then the commodity details should be populated '16052190' 'Other' for second commodity
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Penaeus (Litopenaeus) vannamei'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	Then the user verifies 'Internal market' radio button exists with the sub-option 'Animal feedingstuff,Human consumption,Other'
	When the user verifies 'Transhipment or onward travel' radio button exists with 'Destination country' dropdown
	Then the user verifies 'Transit' radio button exists with the sub-option 'Exit border control post,When the consignment will leave Great Britain,Transited country,Destination country'
	When the user chooses "Internal market" and the sub-option "Animal feedingstuff"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed
	When the user populates Net weight as '13300' for first commodity
	And the user populates Number of packages as '1' for first commodity
	And the user selects type of package as 'Case' for the commodity '03061792' for first commodity
	And the user populates Net weight as '3240' for the second commodity '16052190'
	And the user populates Number of packages as '1' for the second commodity '16052190'
	And the user selects type of package as 'Box' for the second commodity '16052190'
	And the user clicks the Update total button after adding all the commodities
	Then the total gross weight should be greater than the net weight '400000'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Catch cerificates page should be displayed
	And the user selects "Yes" option for add catch certificate
	When the user clicks Save and continue
	Then Upload catch certificates page is displayed
	When the user uploads the document 'IPAFFS Test Document 1' in the format '.docx'
	Then Manage catch certificates page is displayed
	When the user selects the 'Yes' option for Do you need to upload more catch certificates?
	And the user clicks Save and continue
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then Manage catch certificates page is displayed
	And the user verifies there are '2' certificates attached
	When the user selects the 'No' option for Do you need to upload more catch certificates?
	And the user clicks Save and continue
	Then Add catch certificate details page should be displayed
	And 'Number of catch certificates in this attachment' is displayed in Add catch certificate page
	And 'Change' is displayed in Add catch certificate page
	When the user enters 'CatchRef123' in catch certificate reference 1
	And the user enters Data of issue 1 as '23''01''2026' in Add catch certificate page
	And the user enters 'United Kingdom of Great Britain and Northern Ireland' in Flag state of catching vessels 1
	Then the calendar icon is displayed in Add catch certificate page
	And 'Select species being imported under this catch certificate' is displayed in Add catch certificate page
	And 'Select all' is displayed in Add catch certificate page
	And 'Save and return to manage catch certificates' is displayed in Add catch certificate page
	And 'Save and return to hub' is displayed in Add catch certificate page
	When the user selects the 'Penaeus spp.,Penaeus (Litopenaeus) vannamei' species under Select species being imported under this catch certificate
	And the user clicks Save and continue
	Then Add catch certificate details page should be displayed
	And 'Number of catch certificates in this attachment' is displayed in Add catch certificate page
	And 'Change' is displayed in Add catch certificate page
	When the user enters 'CatchRef456' in catch certificate reference 1
	And the user enters Data of issue 1 as '22''01''2026' in Add catch certificate page
	And the user enters 'France' in Flag state of catching vessels 1
	Then the calendar icon is displayed in Add catch certificate page
	And 'Select species being imported under this catch certificate' is displayed in Add catch certificate page
	And 'Select all' is displayed in Add catch certificate page
	And 'Save and return to manage catch certificates' is displayed in Add catch certificate page
	And 'Save and return to hub' is displayed in Add catch certificate page
	When the user selects the 'Penaeus spp.,Penaeus (Litopenaeus) vannamei' species under Select species being imported under this catch certificate
	And the user clicks Save and continue
	Then Manage catch certificates page is displayed
	When the user selects the 'No' option for Do you need to upload more catch certificates?
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
	And the user enters date of issue '24/11/2025'
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document with a long file name to check the limit of filename length exceeding 100 characters' in the format '.docx'
	Then the document 'IPAFFS Test Document with a long file name to check the limit of filename length exceeding 100 characters' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "Italy" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
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
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "160"
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
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the IUU page should be displayed
	When the user selects "No" and sub-option as "" for the IUU check
	And the user clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Full identity check" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user clicks View or print CHED button
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
	And I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the BTMS Sign in using Government Gateway page
	When I have provided the BTMS credentials and signin
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier
	Then the BTMS search result screen should be displayed
	And the user checks commodity code "03061792", description "Penaeus spp.", quantity "13300", authority "POAO IUU" and decision "Acceptable for Internal Market Decision not given" after the decision given
	And the user checks commodity code "16052190", description "Penaeus (Litopenaeus) vannamei", quantity "3240", authority "POAO IUU" and decision "Acceptable for Internal Market Decision not given" after the decision given
	When the user logs out of BTMS
	Then the user should be logged out successfully

Scenario: Verify IPAFF Inspector application SPS-7391
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	And the user verifies 'Create notification' link in Dashboard header
	And the user verifies 'Record decision' link in Dashboard header
	And the user verifies 'Record control' link in Dashboard header
	When the user clicks Record control in Dashboard page
	Then the Consignments requiring control page should be displayed
	And the user verifies 'Show CHED' link in Consignments requiring control page
	When the user clicks Record decision from the header
	Then the Import notifications dashboard page should be displayed
	When the user clicks View CHED link
	Then the certificate should be displayed in a new browser tab
	When the user closes the PDF browser tab
	Then the browser tab is closed
	And the user validates 'Search CHEDs by' text in Import notifications Page
	And the user validates 'Keywords or CHED number' text in Import notifications Page
	And the user validates 'Commodity' text in Import notifications Page
	And the user validates 'CHED status' text in Import notifications Page
	And the user validates 'CHED type' text in Import notifications Page
	And the user validates 'Country of origin' text in Import notifications Page
	And the user validates 'Consignee' text in Import notifications Page
	And the user validates 'Decision' text in Import notifications Page
	And the user validates 'Risk outcome' text in Import notifications Page
	And the user validates 'Microchip number (CHED-A only)' text in Import notifications Page
	And the user validates 'Risk category (CHED-P only)' text in Import notifications Page
	And the user validates 'Filter by arrival date and time' text in Import notifications Page
	And the user validates 'Today' text in Import notifications Page
	And the user validates 'Tomorrow' text in Import notifications Page
	And the user validates 'Start date' text in Import notifications Page
	And the user validates 'End date' text in Import notifications Page
	And the user validates 'Start time (optional)' text in Import notifications Page
	And the user validates 'End time (optional)' text in Import notifications Page
	When the user clicks Record control in Dashboard page
	Then the Consignments requiring control page should be displayed
	And the user verifies 'All' is selected in 'BCP' field
	And the user verifies 'All' is selected in 'CHED status' field
	And the user verifies 'All' is selected in 'CHED type' field
	And the user verifies 'All' is selected in 'Country of origin' field
	And the user verifies 'All' is selected in 'Control status' field
	And the user verifies 'All' is selected in 'Decision' field
	And the user verifies 'All' is selected in 'Seal check required' field
	When the user selects 'Complete' from 'Control status' field
	And the user clicks on Search in Consignments requiring control page
	Then the user validates the Control status is 'CONTROL COMPLETE'
	When the user selects 'All' from 'Control status' field
	When the user enters the Start date as 'past30'
	And the user enters the End date as 'today'
	And the user clicks on Search in Consignments requiring control page
	Then the user validates the result is within the date range
	And the Date of decision is sorted by 'Date of decision (newest to oldest)'
	When the user clicks on 'Show CHED' in Consignment requiring control page
	Then the certificate should be displayed in a new browser tab
	When the user closes the PDF browser tab
	Then the browser tab is closed
	When the user clicks Record control in Dashboard page
	When the user selects 'Complete' from 'Control status' field
	And the user clicks on Search in Consignments requiring control page
	And the user searches for the CHED 'CHEDP.GB.2024.1037447' in Import notifications page
	And the user opens the first notification in the consignments requiring control page
	Then the CHED overview page should be displayed
	And the user verifies 'Notification' tab in CHED Overview page
	And the user verifies 'Checks' tab in CHED Overview page
	And the user verifies 'Control' tab in CHED Overview page
	And the user verifies the value is present for 'Import type'
	And the user verifies the value is present for 'Responsible person'
	And the user verifies the value is present for 'Telephone'
	And the user verifies the value is present for 'Email'
	And the user verifies the value is present for 'Organisation Name'
	And the user verifies the value is present for 'Organisation Addresss'
	And the user verifies the value is present for 'Organisation Telephone'
	And the user verifies the value is present for 'Country of origin'
	And the user verifies the value is present for 'Country from where consigned'
	And the user verifies the value is present for 'Consignment conforms to regulatory requirements'
	And the user verifies the value is present for 'Consignment reference number'
	And the user verifies the value is present for 'Main reason for importing the consignment'
	And the user verifies the value is present for 'Port of exit'
	And the user verifies the value is present for 'When the consignment will leave Great Britain?'
	And the user verifies the value is present for 'Transited country'
	And the user verifies the value is present for 'Destination country'
	And the user verifies the value is present for 'Commodity code'
	And the user verifies the value is present for 'Subtotal'
	And the user verifies the value is present for 'Total net weight'
	And the user verifies the value is present for 'Total packages'
	And the user verifies the value is present for 'Total gross weight'
	And the user verifies the value is present for 'Temperature'
	And the user verifies the value is present for 'Consignor or exporter'
	And the user verifies the value is present for 'Consignee'
	And the user verifies the value is present for 'Importer'
	And the user verifies the value is present for 'Place of destination'
	And the user verifies the value is present for 'Port of entry'
	And the user verifies the value is present for 'Means of transport to port of entry'
	And the user verifies the value is present for 'Transport identification'
	And the user verifies the value is present for 'Using road trailers or containers?'
	And the user verifies the value is present for 'Transport document reference'
	And the user verifies the value is present for 'Estimated arrival date at port of entry'
	And the user verifies the value is present for 'Estimated arrival time at port of entry'
	And the user verifies the value is present for 'Using the Goods Vehicle Movement Service (GVMS)'
	And the user verifies the value is present for 'Contact address'
	And the user verifies the value is present for 'Name'
	And the user verifies the value is present for 'Email address'
	And the user verifies the value is present for 'Phone number'
	And the user verifies the value is present for 'Notification submitted by'
	And the user verifies the value is present for 'Submission date'
	And the user verifies the value is present for 'Submission time'
	And the user verifies the value is present for 'Species' in '1' column
	And the user verifies the value is present for 'Net weight' in '2' column
	And the user verifies the value is present for 'Packages' in '3' column
	And the user verifies the value is present for 'Package type' in '4' column
	And the user verifies the value is present for 'Document type' in '1' column
	And the user verifies the value is present for 'Document reference' in '2' column
	And the user verifies the value is present for 'Date of issue' in '3' column
	And the user verifies the value is present for 'Attachments' in '4' column
	And the user verifies the value is present for 'Name' in '1' column
	And the user verifies the value is present for 'Country' in '2' column
	And the user verifies the value is present for 'Type' in '3' column
	And the user verifies the value is present for 'Approval number' in '4' column
	When the user switches to 'Checks' tab in CHED Overview page
	Then the user verifies the value is present for 'IUU'
	And the user verifies the value is present for 'Documentary check'
	And the user verifies the value is present for 'Seal check only'
	And the user verifies the value is present for 'Physical check'
	And the user verifies the value is present for 'Required'
	And the user verifies the value is present for 'Decision result'
	And the user verifies the value is present for 'Decision recorded by'
	And the user verifies the value is present for 'Decision recorded date'
	And the user verifies the value is present for 'Decision recorded time'
	And the user verifies the value is present for 'Declaration date'
	And the user verifies the value is present for 'Transited country' under 'Decision information'
	And the user verifies the value is present for 'Exit Border Control Post'
	And the user verifies the value is present for 'Destination country' under 'Decision information'
	When the user switches to 'Control' tab in CHED Overview page
	Then the user verifies the value is present for 'Has the consignment left the UK?'
	And the user verifies the value is present for 'Means of transport'
	And the user verifies the value is present for 'Transport identification' under 'Control'
	And the user verifies the value is present for 'Transport document reference' under 'Control'
	And the user verifies the value is present for 'Date of departure'
	And the user verifies the value is present for 'Exit BCP'
	And the user verifies the value is present for 'Destination country' under 'Control'
	And the user verifies the value is present for 'Name' under 'Recorded by'
	And the user verifies the value is present for 'Registered location'
	And the user verifies the value is present for 'Date control added'
	When the user clicks Record control in Dashboard page
	Then the Consignments requiring control page should be displayed
	When the user clicks Clear all in consinments requiring control page
	When the user selects 'Required' from 'Control status' field
	And the user clicks on Search in Consignments requiring control page
	Then the user validates the Control status is 'CONTROL REQUIRED'
	When the user searches for the CHED 'CHEDP.GB.2026.1064468' in Import notifications page
	And the user opens the first notification in the consignments requiring control page
	Then the CHED overview page should be displayed
	And the user verifies 'Notification' tab in CHED Overview page
	And the user verifies 'Checks' tab in CHED Overview page
	And the user verifies the value is present for 'Import type'
	And the user verifies the value is present for 'Responsible person'
	And the user verifies the value is present for 'Telephone'
	And the user verifies the value is present for 'Email'
	And the user verifies the value is present for 'Organisation Name'
	And the user verifies the value is present for 'Organisation Addresss'
	And the user verifies the value is present for 'Organisation Telephone'
	And the user verifies the value is present for 'Country of origin'
	And the user verifies the value is present for 'Country from where consigned'
	And the user verifies the value is present for 'Consignment conforms to regulatory requirements'
	And the user verifies the value is present for 'Consignment reference number'
	And the user verifies the value is present for 'Main reason for importing the consignment'
	And the user verifies the value is present for 'Purpose in the internal market'
	And the user verifies the value is present for 'Commodity code'
	And the user verifies the value is present for 'Type of commodity'
	And the user verifies the value is present for 'Subtotal'
	And the user verifies the value is present for 'Total net weight'
	And the user verifies the value is present for 'Total packages'
	And the user verifies the value is present for 'Total gross weight'
	And the user verifies the value is present for 'Temperature'
	And the user verifies the value is present for 'Consignor or exporter'
	And the user verifies the value is present for 'Consignee'
	And the user verifies the value is present for 'Importer'
	And the user verifies the value is present for 'Place of destination'
	And the user verifies the value is present for 'Port of entry'
	And the user verifies the value is present for 'Means of transport to port of entry'
	And the user verifies the value is present for 'Transport identification'
	And the user verifies the value is present for 'Using road trailers or containers?'
	And the user verifies the value is present for 'Transport document reference'
	And the user verifies the value is present for 'Estimated arrival date at port of entry'
	And the user verifies the value is present for 'Estimated arrival time at port of entry'
	And the user verifies the value is present for 'Using the Goods Vehicle Movement Service (GVMS)'
	And the user verifies the value is present for 'Contact address'
	And the user verifies the value is present for 'Name'
	And the user verifies the value is present for 'Email address'
	And the user verifies the value is present for 'Phone number'
	And the user verifies the value is present for 'Notification submitted by'
	And the user verifies the value is present for 'Submission date'
	And the user verifies the value is present for 'Submission time'
	And the user verifies the value is present for 'Document type' in '1' column
	And the user verifies the value is present for 'Document reference' in '2' column
	And the user verifies the value is present for 'Date of issue' in '3' column
	And the user verifies the value is present for 'Name' in '1' column
	And the user verifies the value is present for 'Country' in '2' column
	And the user verifies the value is present for 'Type' in '3' column
	And the user verifies the value is present for 'Approval number' in '4' column
	When the user switches to 'Checks' tab in CHED Overview page
	#Then the user verifies the value is present for 'IUU'
	Then the user verifies the value is present for 'Documentary check'
	And the user verifies the value is present for 'Seal check only'
	And the user verifies the value is present for 'Physical check'
	And the user verifies the value is present for 'Required'
	And the user verifies the value is present for 'Reason'
	And the user verifies the value is present for 'Analysis type'
	And the user verifies the value is present for 'Commodity code' under 'Laboratory tests'
	And the user verifies the value is present for 'Commodity description'
	And the user verifies the value is present for 'Species'
	And the user verifies the value is present for 'Laboratory test'
	And the user verifies the value is present for 'Sample date'
	And the user verifies the value is present for 'Sample time'
	And the user verifies the value is present for 'Conclusion'
	And the user verifies the value is present for 'Decision result'
	And the user verifies the value is present for 'Decision recorded by'
	And the user verifies the value is present for 'Decision recorded date'
	And the user verifies the value is present for 'Decision recorded time'
	And the user verifies the value is present for 'Declaration date'
	And the user verifies the value is present for 'Refusal decision'
	And the user verifies the value is present for 'Reason'
	And the user verifies the value is present for 'Refusal by date'
	And the user verifies the value is present for 'Reason(s) for refusal'
	And the user verifies the value is present for 'Controlled destination'
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: Create and submits a B2C consignment notification - SPS-6937 CHEDP
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "France" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	And the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '16054000' commodity code
	Then the commodity details should be populated '16054000' 'Other crustaceans'
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Jasus edwardsii'
	And the user selects species of commodity 'Metanephrops challengeri'
	And the user selects species of commodity 'Palaemonoidea'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Low risk" risk category
	And the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000,500,1500'
	And the user populates Number of packages as '10,20,15'
	And the user selects type of package as 'Box,Case,Drum'
	And the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '3100'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Catch cerificates page should be displayed
	And the user selects "No – all the wild fish in this consignment are exempt from IUU fishing controls" option for add catch certificate
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "France" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details 'LONDON GATEWAY (GBLGP)' 'No' 'Road vehicle' '123456' 'Doc1234'
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "Yes – add MRN now" for Are you using the Common Transit Convention (CTC)?
	And the user can provide Movement Reference Number as "24GB123456789AB012"
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "16054000"
	And the user should see the message "0 catch certificate" displayed in the certificates section
	And the user should see the message "No catch certificates attached" under Catch certificate reference
	When the user clicks on Change link under 'Documents'
	Then the Catch cerificates page should be displayed
	And the user selects "Yes" option for add catch certificate
	When the user clicks Save and continue
	Then Upload catch certificates page is displayed
	When the user uploads the document 'IPAFFS Test Document 1' in the format '.docx'
	Then Manage catch certificates page is displayed
	And the user verifies there are '1' certificates attached
	When the user clicks on Add details link
	Then Add catch certificate details page should be displayed
	And 'Number of catch certificates in this attachment' is displayed in Add catch certificate page
	And 'Change' is displayed in Add catch certificate page
	When the user clicks on Change link in Add catch certificate details page
	And the user enters "3" for Number of catch certificates in this attachment
	And the user clicks on Update button
	Then the user can see 3 Catch certificate reference details sections for input
	When the user enters 'CatchRef001' in catch certificate reference 1
	And the user enters Data of issue 1 as '01''02''2026' in Add catch certificate page
	And the user enters 'United Kingdom of Great Britain and Northern Ireland' in Flag state of catching vessels 1
	And the user Clicks on Update details 1
	And the user enters 'CatchRef002' in catch certificate reference 2
	And the user enters Data of issue 2 as '02''02''2026' in Add catch certificate page
	And the user enters 'United Kingdom of Great Britain and Northern Ireland' in Flag state of catching vessels 2
	And the user Clicks on Update details 2
	And the user enters 'CatchRef003' in catch certificate reference 3
	And the user enters Data of issue 3 as '03''02''2026' in Add catch certificate page
	And the user enters 'United Kingdom of Great Britain and Northern Ireland' in Flag state of catching vessels 3
	And the user clicks Save and continue
	Then Manage catch certificates page is displayed
	When the user selects the 'No' option for Do you need to upload more catch certificates?
	And the user clicks Save and continue
	Then Confirm exempt species page should be displayed
	And the user selected "None of the species are exempt" option
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies the 3 catch certificates reference details
	When the user clicks on 1 Change link of Catch Cerfitificate Document section
	Then Add catch certificate details page should be displayed
	When the user enters 'France' in Flag state of catching vessels 1
	And the user selects the "Jasus edwardsii,Metanephrops challengeri,Palaemonoidea" species under Select species being imported under this catch certificate
	And the user Clicks on Update details 1
	And the user clicks on Save and return to manage catch certificates link
	Then Manage catch certificates page is displayed
	When the user selects the 'No' option for Do you need to upload more catch certificates?
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies the 3 catch certificates reference details
	And the user verifies the updated catch certificates 3 species details
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
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks on Attachments button on Decision Hub page
	Then the Documents page should be displayed
	When the user clicks on Download all documents link
	Then Your download has started message should be displayed
	And the user verifies that Documents is downloaded into a .zip file with the title of the CHED certificates in Downloaded folder
	When the user clicks Return to documents button
	Then the Documents page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Save and Return	
	Then the Decision Hub page should be displayed
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the IUU page should be displayed
	When the user selects "Yes" and sub-option as "Compliant" for the IUU check
	And the user clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Full identity check" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Human consumption'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
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

Scenario: Verify IUU and Catch certificate details for EU and Non-EU countries CHEDP_SPS_6283
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Sweden" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Sweden" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '03063400' commodity code
	Then the commodity details should be populated '03063400' 'Norway lobsters (Nephrops norvegicus)'
	When the user selects the type of commodity 'Wild stock'
	When the user selects species of commodity 'Nephrops norvegicus'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	When the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Catch cerificates page should be displayed
	And the user selects "No" option for add catch certificate
	And the user clicks Save and continue 
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue '24/11/2025'
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "Sweden" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	And the user verifies all the data displayed in review page for commodity code "03063400"
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
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the IUU page should be displayed
	And the user clicks Save and continue
	And the user should see an error message '"Record IUU status" is required'
	When the user selects "Yes" and sub-option as "" for the IUU check
	And the user clicks Save and continue
	Then the user should see an error message 'Select an IUU check decision'
	And the user clicks Save and continue
	When the user selects "Yes" and sub-option as "Compliant" for the IUU check
	And the user clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed	
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user Clicks the change link under 'IUU checks'
	Then the IUU page should be displayed
	When the user selects "Yes" and sub-option as "Not compliant" for the IUU check
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Review And Submit link
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user Clicks the change link under 'IUU checks'
	Then the IUU page should be displayed
	When the user selects "No" and sub-option as "" for the IUU check
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Review And Submit link
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And the user verfies the decision outcome as 'Acceptable for Internal Market'
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	Then the Your checks have been submitted page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user deletes all the stored values
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Argentina" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Argentina" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '020210' commodity code
	Then the commodity details should be populated '020210' 'Carcases and half-carcases'
	When the user selects the type of commodity 'Domestic'
	When the user selects species of commodity 'Bubalus bubalis'
	And the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Internal market" and the sub-option "Human consumption"
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	When the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue 
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue '24/11/2025'
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "Argentina" type "Cold Stores" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects one of the displayed consignors or exporters "ABC"
	Then the chosen consignor or exporter "ABC" should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
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
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And 'No' is pre-selected for Are new seal numbers required?
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed	
	And 'No' is pre-selected for Would you like to record laboratory tests?
	When the user clicks Save and continue
	Then the Decision page should be displayed
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the details reflect the information added
	And the catch certificate details are not present
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	And the user verfies the decision outcome as 'Acceptable for Internal Market'
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	Then the Your checks have been submitted page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	Then the user should be logged out successfully

Scenario: User creates and submits a CHEDP consignment notification with multiple catch certificates and validates attachment management - SPS-6944
	Given that I navigate to the IPAFF application
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
	When the user chooses 'Austria' from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing 'Austria' as the Country of origin and Country from where consigned
	When the user chooses 'No' for Does your consignment require a region code?
	And the user chooses 'Yes' for Does this consignment conform to regulatory regulations?
	And the user chooses 'No' for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number '12345' in the Add a reference number for this consignment (optional) field
	And the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '03063400' commodity code
	Then the commodity details should be populated '03063400' 'Norway lobsters (Nephrops norvegicus)'
	When the user selects the type of commodity 'Wild stock'
	And the user selects species of commodity 'Nephrops norvegicus'
	And the user selects 'No' for Do you want to add another commodity?
	And the user clicks Save and continue
	Then the What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses 'Internal market' and the sub-option 'Human consumption'
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
	Then the total gross weight should be greater than the net weight '1010'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Frozen' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Catch cerificates page should be displayed
	And the user verifies 'Do you need to add catch certificates?' is displayed
	And the user verifies the radio buttons are 'Yes' and 'No – all the wild fish in this consignment are exempt from IUU fishing controls'
	When the user selects 'Yes' option for add catch certificate
	And the user clicks Save and continue
	Then Upload catch certificates page is displayed
	And the user verifies 'Select documents to upload' is displayed with drag and drop functionality
	When the user uploads the documents 'Catch Certificate 1' 'Catch Certificate 2' 'Catch Certificate 3' in the format '.docx'
	And the user verifies all 3 files are displayed
	And the user clicks Continue
	Then Manage catch certificates page is displayed
	And the user verifies success message displays the number of files added
	And the user verifies the number of attachments missing details based on added files
	And the user verifies 'Catch certificates uploaded' heading is displayed
	And the user verifies 'Number of catch certificates in this attachment' is displayed for each attachment
	And the user verifies each attachment is displayed as 'X of Y files' format
	And the user verifies each attachment has a populated input box with the number of catch certificates in that attachment
	And the user verifies each attachment has an 'Update' button
	And the user verifies each attachment has 'Add details' and 'Remove' links
	When the user selects the 'No' option for Do you need to upload more catch certificates?
	And the user clicks Save and continue
	Then Add catch certificate details page should be displayed
	And the user verifies Attachment 1 is displayed underneath Add catch certificate details
	And the user verifies 'Number of catch certificates in this attachment' with 'Change' link is displayed
	And the user verifies 'Catch certificate reference number' field is displayed
	And the user verifies 'Date of issue' with Day, Month, Year fields and calendar icon is displayed
	And the user verifies 'Flag state of catching vessel(s)' field is displayed
	And the user verifies 'Select species being imported under this catch certificate' with 'Select all' option is displayed
	And the user verifies 'Save and continue' button is displayed
	And the user verifies 'Save and return to manage catch certificates' link is displayed
	And the user verifies 'Save and return to hub' link is displayed
	When the user starts typing 'Nor' in Flag state field
	Then the user verifies multiple country options appear including 'Norway'
	When the user selects 'Norway' from the dropdown
	And the user clicks Save and continue
	Then the user should see error messages 'Enter the catch certificate reference' in Add catch certificate details page
	And the user verifies catch certificate reference field is highlighted
	When the user enters Catch certificate reference 'CATCH001'
	And the user clicks Save and continue
	Then the user should see error messages 'Enter the flag state of the catching vessel' in Add catch certificate details page
	And the user verifies flag state field is highlighted
	When the user clicks Save and continue
	Then the user should see error messages 'Enter the catch certificate reference, Enter the flag state of the catching vessel' in Add catch certificate details page
	And the user verifies catch certificate reference field is highlighted
	And the user verifies flag state field is highlighted
	When the user clicks the add the commodity link
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks Cancel link
	Then the Commodity page should be displayed
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user clicks Save and continue
	Then Manage catch certificates page is displayed
	When the user selects the 'No' option for Do you need to upload more catch certificates?
	And the user clicks Save and continue
	Then Add catch certificate details page should be displayed
	When the user enters Catch certificate reference 'CATCH001' for attachment 1
	And the user enters Date of issue '23''01''2026' for attachment 1
	And the user enters Flag state of catching vessels 'Norway' for attachment 1
	And the user selects the species 'Nephrops norvegicus' for attachment 1
	And the user clicks Save and continue
	Then Add catch certificate details page should be displayed
	And the user verifies Attachment 2 is displayed underneath Add catch certificate details
	When the user clicks Select all option
	Then all listed species are selected and displayed
	When the user enters Catch certificate reference 'CATCH002'
	And the user starts typing 'Norway' in Flag state field
	And the user enters Date of issue '30''02''2026'
	And the user clicks Save and continue
	Then the user should see error messages 'Date of issue must be a real date' in Add catch certificate details page
	And the user verifies date of issue field is highlighted
	When the user enters Catch certificate reference 'CATCH002' for attachment 2
	And the user enters Date of issue '03''02''2026' for attachment 2
	And the user enters Flag state of catching vessels 'France' for attachment 2
	And the user selects the species 'Nephrops norvegicus' for attachment 2
	And the user clicks Save and continue
	Then Add catch certificate details page should be displayed
	And the user verifies Attachment 3 is displayed underneath Add catch certificate details
	When the user enters Catch certificate reference 'CATCH003' for attachment 3
	And the user enters Date of issue '10''02''2026' for attachment 3
	And the user enters Flag state of catching vessels 'United Kingdom of Great Britain and Northern Ireland' for attachment 3
	And the user clicks Select all option
	And the user clicks on Save and return to manage catch certificates link
	Then Manage catch certificates page is displayed
	When the user clicks View or amend details link for attachment 3
	Then Add catch certificate details page should be displayed
	And the user verifies Attachment 3 is displayed underneath Add catch certificate details
	When the user clicks Save and return to hub link
	Then the Notification Hub page should be displayed
	When the user clicks on 'Latest health certificate' link
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference 'INV12345'
	And the user enters Latest Health Certificate date of issue '24''10''2025'
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the document 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user clicks Save and continue
	Then the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin 'Austria' type 'ABP Transport' status 'Approved'
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects a consignor or exporter 'ABC'
	Then the chosen consignor or exporter 'ABC' should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee 'DEF'
	Then the chosen consignee 'DEF' should be displayed on the Addresses page
	When the user clicks Add an importer
	Then the Search for an existing importer page should be displayed
	When the user selects an importer 'DEF' with a UK country
	Then the chosen importer should be displayed on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination 'DEF' with a UK country
	Then the chosen place of destination 'DEF' should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user enters BCP or Port of entry 'BRISTOL (GBBRS)'
	And the user selects means of transport to BCP or Port of entry 'Road vehicle'
	And the user enters transport identification 'EJ64 YGB'
	And the user selects 'No' for Are any road trailers or shipping containers being used to transport the consignment
	And the user enters transport document reference 'Doc1234'
	And the user enters arrival date at BCP or Port of entry as today's date
	And the user enters estimated arrival time at BCP with future time
	And the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects 'No' for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	And the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	When the user verifies the number of certificates displayed under the Documents heading
	And the user verifies the catch certificate table
	And the user verifies the catch certificate details
	And the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	And the details should be recorded

Scenario: SPS-7373
	Given that I navigate to the IPAFF application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF credentials and signin
	Then the user should be logged into Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with radio buttons
	When the user chooses 'Products of animal origin, germinal products or animal by-products' option
	And the user clicks Save and continue
	Then the Origin of the plants plant product or other objects page should be displayed
	When the user chooses "Sweden" from the dropdown for Country of origin
	And the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "Sweden" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	And the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user clicks the 'WOOL, FINE OR COARSE ANIMAL HAIR; HORSEHAIR YARN AND WOVEN FABRIC' in the parent commodity tree
	And the sub commodity list expands
	And the user clicks '5101' 'Wool, not carded or combed' under the parent commodity
	Then the Commodity page should be displayed
	When the user selects "No" for Do you want to add another commodity?
	And the user clicks Save and continue
	Then What is the main reason for importing the consignment? page should be displayed with radio buttons
	When the user chooses "Re-entry" and the sub-option ""
	And the user clicks Save and continue
	Then Select the highest risk category for the commodities in this consignment page should be displayed
	When the user chooses "Medium risk" risk category
	And the user clicks Save and continue
	Then the Health certificate required page should be displayed
	When the user clicks continue button
	Then the Notification Hub page should be displayed
	When the user clicks the Commodity hyperlink
	Then the Description of the goods/Commodity page should be displayed
	When the user populates Net weight as '100'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	When the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '150'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue '24/11/2025'
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	And the Approved establishment of origin page should be displayed
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin "Sweden" type "ABP Transport" status "Approved"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user clicks on Create a new consignor or exporter link
	Then Add consignee page should be displayed
	When the user enters Consignee name as 'Brook'
	And the user enters Consignee address as 'Covent garden'
	And the user enters Consignee City as 'Abingdon'
	And the user enters the Consignee Postcode as 'OX1 1AD'
	And the user enters the Consignee Telephone number as '07348764455'
	And the user enters the Consignee Country as 'England'
	And the user enters the Consignee Email as 'Brook@CG.com'
	And the user clicks Save and continue
	Then The consignee has been created page is displayed
	When the user clicks Add to notification button
	Then the Addresses page should be displayed
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee "DEF" with a UK country
	Then the chosen consignee "DEF" should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee "DEF" on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination "DEF" with a UK country
	Then the chosen place of destination "DEF" should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	And the user selects 'No' for Will the transport use the Goods Vehicle Movement Service (GVMS)?
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
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
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	When I navigate to the IPAFF Inspector application
	Then I should see type of Gateway login page
	And I have selected "Sign in with Government Gateway" as login type
	When I click Continue button from How do you want to sign in page
	Then I should redirected to the IPAFF Sign in using Government Gateway page
	When I have provided the IPAFF Inspector credentials and signin
	Then the user should be logged into Import notifications page
	When the user searches for the newly created notification on the Import notifications page
	Then the user clicks the notification found with status "NEW"
	And the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user clicks Local reference number link in Record checks
	Then Local reference number page should be displayed
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects "Satisfactory" for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" under "Full identity check" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Seal numbers link
	Then the Seal numbers page should be displayed
	When the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Laboratory tests link
	Then the Laboratory tests page should be displayed
	 When the user select "Yes" radio button on the Laboratory tests page
	And the user clicks Save and continue
	And the user select 'Random' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks the Select link for the '5101' commodity code
	Then the Laboratory tests page should be displayed
	When the user clicks select link of one of the Laboratory test
	Then the Laboratory tests Commodity sampled page should be displayed
	When the user populates the commodity sample details 'Initial analysis' 'Campden BRI' '12345' '3' 'Blood' 'Chilled'
	And the user clicks Save and continue
	Then the Laboratory tests Review page should be displayed
	And the user verifies the data in Laboratory tests review page
	When the user clicks Save and continue
	Then the Decision page should be displayed
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
	When the user clicks View or print CHED
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user clicks Record control in Dashboard page
	Then the Consignments requiring control page should be displayed
	When the user searches for the CHED number
	Then the notification should be found with the status "REJECTED"
	And the user verifies the control status is "CONTROL REQUIRED"
	And the user clicks the notification found with status "REJECTED"
	And the CHED overview page should be displayed
	When the user clicks Record control button
	Then Record control page should be displayed
	When the user selects 'yes' for Did the consignment leave the UK?
	And the user selects 'Airplane' as Means of transport
	And the user enters 'Test123' as Identification
	And the user enters 'TestDoc' in Documentaion
	And the user enters 'today' date in Record control page
	And the user selects 'Bristol - GBBRS1' as Exit BCP
	And the user selects 'Brazil' as Destination country
	And the user clicks the Submit control button
	Then Your control has been recorded page should be displayed
	And the CHED reference number is displayed
	And the outcome is recorded as 'Consignment has left the UK'
	When the user clicks View or print CHED button on Control recorded page
	Then the certificate should be displayed in a new browser tab
	When the user checks that the data in the certificate matches the data entered into the notification
	And the user closes the PDF browser tab
	Then the browser tab is closed
	When the user clicks Return to Your Dashboard link in Record control recorded page
	Then the Consignments requiring control page should be displayed
	When the user searches for the CHED number
	Then the notification should be found with the status "REJECTED"
	And the user verifies the control status is "CONTROL COMPLETE"
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	