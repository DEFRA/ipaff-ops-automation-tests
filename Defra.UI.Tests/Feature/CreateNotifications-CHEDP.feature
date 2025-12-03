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
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination with a UK country
	Then the chosen place of destination should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details "BRISTOL (GBBRS)" "Road vehicle" "123456" "Doc1234"
	When the user clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
	When the user clicks Save and continue
	#Then the Confirm billing details page should be displayed
	#When the user clicks Save and continue
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
	And the user checks commodity code, description, quantity, authority and decision
	When the user logs out of BTMS
	Then the user should be logged out successfully
