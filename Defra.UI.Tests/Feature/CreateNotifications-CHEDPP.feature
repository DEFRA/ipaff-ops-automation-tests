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
	Then What is the main reason for importing the consignment? page should be displayed
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
	Then the Additional details page should be displayed
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
	Then Importer, Packer, Delivery address and Consignor page should be displayed
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
	When the user logs out of IPAFFS Part 1
	Then the user should be logged out successfully
	
Scenario:  Create a new import notification through clone a health or phytosanitary certificate process - SPS-9272 - CHED PP
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
	And the user selected the importing option as ''
	When the user clicks continue button
	Then the Certificate details page should be displayed
	And the user searches for the notification for cloning which is not more  than 90 days from creation
	And the user provided notification details in the search input fields
	When the user Clicks on Search button
	
	