@Regression
Feature: Create Notification CHEDP

Create a notification for CHEDP type

Scenario: User creates and submits a B2C consignment notification - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '41015050' commodity code
	Then the commodity details should be populated '41015050' 'Dried or dry-salted'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bison bison'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	When the user clicks the Update total button
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
	Then I should see type of Gateway login page
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
	Then the Decision Hub page should be displayed
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
	Then I should see type of Gateway login page
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

Scenario: User creates and submits a B2C consignment notification for Transit Reason - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '16042005' commodity code
	Then the commodity details should be populated '16042005' 'Preparations of surimi'
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Other'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	When the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '1500'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks Save and continue
	Then the Catch cerificates page should be displayed
	When the user selects "No" option
	And the user clicks Save and continue 
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Document' in the format '.docx'
	Then the document 'IPAFFS Test Document' '.docx' is uploaded successfully
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

Scenario: User creates and submits a B2C consignment notification for Transhipment or onward travel Reason - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '41015050' commodity code
	Then the commodity details should be populated '41015050' 'Dried or dry-salted'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bison bison'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	When the user clicks the Update total button
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
	Then the Accompanying documents page should be displayed
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
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User creates and submits 2 B2C consignment notification with existing Billing details - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '97052200' commodity code
	Then the commodity details should be populated '97052200' 'Extinct or endangered species and parts thereof'
	When the user selects the type of commodity 'Game trophies'
	And the user selects species of commodity 'Cervidae'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '97052200' commodity code
	Then the commodity details should be populated '97052200' 'Extinct or endangered species and parts thereof'
	When the user selects the type of commodity 'Game trophies'
	And the user selects species of commodity 'Cervidae'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	Then the Latest Health Certificate page should be displayed
	When the user enters Latest Health Certificate Document reference "INV12345"
	And the user enters Latest Health Certificate date of issue "24""10""2025"
	And the user clicks Latest Health Certificate add attachment link
	And the user uploads the Latest Health Certificate document 'IPAFFS Test Document' in the format '.docx'
	Then the Latest Health Certificate document 'IPAFFS Test Document' '.docx' is uploaded successfully
	And the user clicks Save and continue
	Then the Accompanying documents page should be displayed
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
	Then the Confirm billing details page should be displayed
	When the user clicks Save and continue
	Then the Review your notification page should be displayed
	#And the user verifies all the data displayed in review page for commodity code "160"
	When the user clicks Save and continue
	Then the Declaration page should be displayed
	When the user clicks Submit notification
	Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully

Scenario: Admin submits a notification and records decision and validate cookies page as normal user - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '16051000' commodity code
	Then the commodity details should be populated '16051000' 'Crab'
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Geryon maritae'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	When the user selects "No" option
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
	Then the Decision Hub page should be displayed
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
	When the user select 'Random' reason radio button on the Laboratory tests page
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

Scenario: User creates a B2C consignment notification, updates it from the review page, submits it, amends the notification, and sends it for laboratory tests - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '020110' commodity code
	Then the commodity details should be populated '020110' 'Carcases and half-carcases'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bos taurus'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	And the user should see an error message 'Temperature of the consignment' in review page
	And the user should see an error message 'Total gross weight' in review page
	When the user Clicks the change link under 'Description of the goods'
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '2106'
	And the user populates Number of packages as '10'
	And the user selects type of package as 'Box'
	When the user clicks the Update total button
	Then the total gross weight should be greater than the net weight '2200'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Frozen' radio button on the Additional details page
	And the user clicks on Save and review
	Then the Review your notification page should be displayed
	Then the user verifies all the data displayed in review page for commodity code "160"
	When the user Clicks the change link under 'Transport'
	When the user populates the transport details "BRISTOL (GBBRS)" "No" "Road vehicle" "123456" "Doc1234"
	And the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under '1 additional document'
	Then the Accompanying documents page should be displayed
	When the user clicks the Add a document link
	When the user selects Document type "Veterinary health certificate"
	And the user enters Document reference "INV54321"
	And the user enters date of issue "04/12/2025"
	And the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under 'Approved establishment'
	Then the Approved establishment of origin page should be displayed
	When the user removes the establishment of origin
	When the user clicks Search for an approved establishment
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
	When user searches for the import notification
	Then the notification should be present in the list
	When the user clicks Amend
	Then the Notification Hub page should be displayed
	When the user clicks on 'Origin of the import' link
	When the user chooses "Finland" from the dropdown for Country of origin
	And the user clicks on Save and return to hub
	When the user clicks on 'Addresses' link
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
	When the user clicks on 'Review and submit' link
	When the user clicks Save and continue
    Then the Declaration page should be displayed
	When the user clicks Submit notification           
    Then the Confirmation page should be displayed with the initial risk assessment
	And the user records the IPAFFS User details and CHED Reference
	When the user clicks Return to your dashboard   
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
	When the user selects "Satisfactory" under "Seal check only" in identity check
	And the user selects "Satisfactory" for physical check
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
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
	When the user clicks Save and continue
	And the user clicks Save and Return
	Then the Decision Hub page should be displayed
	When the user clicks Decision link
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	And the user should see an error message 'Lab results pending for this consignment' in review page
	When the user Clicks the change link under 'Laboratory tests'
	And the user clicks Save and continue
	And the user clicks Save and continue
	And the user clicks on the Test 'Anaplasma marginale'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	When the user enters Released date as '16''12''2025'
	When the user selects "Satisfactory" for Conclusion
	And the user clicks Save and continue
	And the user clicks Save and Return
	When the user clicks Review And Submit link
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

Scenario: User creates and submits a notification, override the risk decision and reject the notification - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '41015050' commodity code
	Then the commodity details should be populated '41015050' 'Dried or dry-salted'
	When the user selects the type of commodity 'Domestic'
	And the user selects species of commodity 'Bison bison'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	When the user clicks the Update total button
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
	Then the Decision Hub page should be displayed
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
	When the user clicks Save and continue
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
	When the user select 'Suspicion' reason radio button on the Laboratory tests page
	And the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks on the Test 'Anaplasma marginale'
	Then the Record laboratory test information page should be displayed
	When the user enters Sample use by date as '15''12''2025'
	When the user enters Released date as '16''12''2025'
	When the user selects "Not satisfactory" for Conclusion
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
	And the user enters reason 'Not satisfactory' and selects By date
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

Scenario: User submits a notification as no inspection required, override the risk decision and checks Inspection required box - CHEDP
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '16051000' commodity code
	Then the commodity details should be populated '16051000' 'Crab'
	When the user selects the type of commodity 'Composite products'
	And the user selects species of commodity 'Geryon maritae'
	And the user selects "No" for Do you want to add another commodity?
	When the user clicks Save and continue
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
	When the user selects "No" option
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
	Then the Decision Hub page should be displayed
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
	Then the Decision page should be displayed
	When the user selects Acceptable for 'Internal market' 'Animal feedingstuff'
	And the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully

Scenario: User submits a notification, inspector copy it as replacement, update and submit decision - CHED P
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
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	And the user enters a reference number "12345" in the Add a reference number for this consignment (optional) field
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user searches '230910' commodity code
	Then the commodity details should be populated '230910' 'Dog or cat food, put up for retail sale'
	When the user selects the type of commodity 'By-products / feedingstuff'
	And the user selects species of commodity 'Ungulates'
	When the user selects "Yes" for Do you want to add another commodity?
	And the user clicks Save and continue
	When the user searches '42050090' commodity code
	Then the commodity details should be populated '42050090' 'Other'
	When the user selects the type of commodity 'By-products / feedingstuff'
	And the user selects species of commodity 'Suidae'
	When the user selects "No" for Do you want to add another commodity?
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
	When the user clicks the Update total button
	Then the Total Net weight should be populated as '10227'
	And the Total Number of packages should be populated as '25'
	Then the total gross weight should be greater than the net weight '15000'
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
	Then the Accompanying documents page should be displayed
	When the user selects Document type "Commercial invoice"
	And the user enters Document reference "INV12345"
	And the user enters date of issue "24/11/2025"
	And the user clicks on Add attachment link
	And the user uploads the document 'IPAFFS Test Health Certificate' in the format '.docx'
	Then the document 'IPAFFS Test Health Certificate' '.docx' is uploaded successfully
	Then the user clicks Save and continue
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
	Then I should see type of Gateway login page
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
	Then I should see type of Gateway login page
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
	Then the user records replaced CHED Reference number, Customs declaration reference and document code
	When the user clicks change in commodity section
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates Net weight as '1000,1000'
	And the user clicks the Update total button
	Then the Total Net weight should be populated as '2000'
	Then the total gross weight should be greater than the net weight '3000'
	When the user clicks Save and continue in commodity page
	Then the Additional details page should be displayed
	When the user selects 'Ambient' radio button on the Additional details page
	And the user clicks on Save and review
	Then the Notification overview page should be displayed
	And the status should be "MODIFY"
	Then the total net weight should be updated to '2000 kg/units'
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
	Then I should see type of Gateway login page
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
