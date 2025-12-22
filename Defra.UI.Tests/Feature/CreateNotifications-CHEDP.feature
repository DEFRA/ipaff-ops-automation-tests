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
	Then the list of establishments should be displayed, filtered by Country of origin to "France"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects any one of the displayed consignors or exporters
	Then the chosen consignor or exporter should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee with a UK country
	Then the chosen consignee should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination with a UK country
	Then the chosen place of destination should be displayed on the Addresses page
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
	And the user verifies all the data displayed in review page
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
	Then the user clicks the notificaiton found with status "NEW"
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
	Then the list of establishments should be displayed, filtered by Country of origin to "France"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects any one of the displayed consignors or exporters
	Then the chosen consignor or exporter should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee with a UK country
	Then the chosen consignee should be displayed on the Addresses page
	When the user clicks Add an importer
	Then the Search for an existing importer page should be displayed
	When the user selects an importer "XYZ" with a UK country
	Then the chosen importer should be displayed on the Addresses page
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination with a UK country
	Then the chosen place of destination should be displayed on the Addresses page
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
	And the user verifies all the data displayed in review page
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
	Then the user clicks the notificaiton found with status "NEW"
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
	Then the user clicks the notificaiton found with status "Valid"

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
	Then the list of establishments should be displayed, filtered by Country of origin to "France"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects any one of the displayed consignors or exporters
	Then the chosen consignor or exporter should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee with a UK country
	Then the chosen consignee should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee on the Addresses page
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
	And the user verifies all the data displayed in review page
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
	Then the user clicks the notificaiton found with status "NEW"
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
	When the user clicks return to you dashboard link
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
	Then the list of establishments should be displayed, filtered by Country of origin to "France"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks Save and continue
	Then the Addresses page should be displayed
	When the user clicks Add a consignor or exporter
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects any one of the displayed consignors or exporters
	Then the chosen consignor or exporter should be displayed on the Addresses page
	When the user clicks Add a consignee
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee with a UK country
	Then the chosen consignee should be displayed on the Addresses page
	When the user clicks Same as consignee for the Importer
	Then the importer should be populated with the same details as the consignee on the Addresses page
	When the user clicks Same as consignee for Place of destination
	Then the place of destination should be populated with the same details as the consignee on the Addresses page
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
	Then the user verifies all the data displayed in review page
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
	Then the list of establishments should be displayed, filtered by Country of origin to "France"
	When the user clicks Select for one of the establishments in the list
	Then the Approved establishment of origin page should be displayed with the selected establishment
	When the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under 'Traders'
	Then the Addresses page should be displayed
	When the user clicks on Change link under 'Consignor or exporter'
	Then the Search for an existing consignor or exporter page should be displayed
	When the user selects any one of the displayed consignors or exporters
	Then the chosen consignor or exporter should be displayed on the Addresses page
	When the user clicks on Change link under 'Consignee'
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee with a UK country
	Then the chosen consignee should be displayed on the Addresses page
	When the user clicks on Change link under 'Importer'
	Then the Search for an existing importer page should be displayed
	When the user selects an importer "XYZ" with a UK country
	Then the chosen importer should be displayed on the Addresses page
	When the user clicks on Change link under 'Place of destination'
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination with a UK country
	Then the chosen place of destination should be displayed on the Addresses page
	When the user clicks on Save and review
	Then the Review your notification page should be displayed
	When the user Clicks the change link under 'Contact address for consignment'
	Then the Contact address for consignment page should be displayed
	And the user selects a contact address for the consignment
	When the user clicks on Save and review
	Then the user verifies all the data displayed in review page
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
	When the user selects any one of the displayed consignors or exporters
	Then the chosen consignor or exporter should be displayed on the Addresses page
	When the user clicks on Change link under 'Consignee'
	Then the Search for an existing consignee page should be displayed
	When the user selects a consignee with a UK country
	Then the chosen consignee should be displayed on the Addresses page
	When the user clicks on Change link under 'Importer'
	Then the Search for an existing importer page should be displayed
	When the user selects an importer "XYZ" with a UK country
	Then the chosen importer should be displayed on the Addresses page
	When the user clicks on Change link under 'Place of destination'
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination with a UK country
	Then the chosen place of destination should be displayed on the Addresses page
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
	Then the user clicks the notificaiton found with status "NEW"
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
	When the user selects "satisfactory" for Conclusion
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