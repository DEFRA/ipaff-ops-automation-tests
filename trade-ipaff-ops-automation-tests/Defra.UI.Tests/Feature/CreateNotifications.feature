Feature: Create Notification

Create a notification with the different types

Scenario: User creates and submits a B2C consignment notification
	Given the user logs on as a B2C notifier
	Then the user should be logged into Part 1 Notification page
	When the user clicks Create a new notification
	Then the About the consignment/What are you importing? page should be displayed with the following options:
		| Live animals                                                       |
		| Products of animal origin, germinal products or animal by-products |
		| High risk food and feed of non-animal origin                       |
		| Plants, plant products and other objects                           |
	When the user chooses Products of animal origin, germinal products or animal by-products
	Then the selection should be made
	When the user clicks Save and continue
	Then the Origin of the animal or product page should be displayed
	When the user chooses "France" from the dropdown for Country of origin
	Then "France" should be displayed as the chosen country
	When the user clicks Save and continue
	Then the Origin of the import page should be displayed, showing "China" as the Country of origin and Country from where consigned
	When the user chooses "No" for Does your consignment require a region code?
	Then the "No" radio button should be selected
	When the user chooses "Yes" for Does this consignment conform to regulatory regulations?
	Then the "Yes" radio button should be selected
	When the user chooses "No" for Will the consignment change vehicles or means of transport after the Border Control Post (BCP)?
	Then the "No" radio button should be selected
	When the user enters a reference number in the Add a reference number for this consignment (optional) field
	Then the reference number should be populated
	When the user clicks Save and continue
	Then the Description of the goods/Commodity page should be displayed
	When the user populates the commodity details:
		| Type of commodity | Domestic                       |
		| Species           | Bison bison                    |
		| Commodity code    | 41015050 (Dried or dry-salted) |
	Then the commodity details should be populated
	When the user selects "No" for Do you want to add another commodity?
	Then the user should not be able to add another commodity
	When the user clicks Save and continue
	Then the "What is the main reason for importing the consignment?" page should be displayed with the following options:
		| Internal market               |
		| Transhipment or onward travel |
		| Transit                       |
		| Re-entry                      |
	When the user chooses "Internal market" and the sub-option "Animal feedingstuff"
	Then the "Internal market" and "Animal feedingstuff" options should be selected
	When the user clicks Save and continue
	Then the "Select the highest risk category for the commodities in this consignment" page should be displayed with options:
		| Medium risk |
		| Low risk    |
	When the user chooses "Low risk"
	Then the "Low risk" radio button should be selected
	When the user clicks Save and continue
	Then the Notification Hub page should be displayed
	When the user clicks the "Commodity" hyperlink
	Then the Commodity page should be displayed with the commodity and description entered
	When the user populates the following details on the "Commodity" screen:
		| Net weight         | 1000 kg |
		| Number of packages |      10 |
		| Type of package    | Box     |
	Then the Commodity screen should be updated with the correct details
	When the user clicks the Update total button
	Then the total gross weight should be greater than the net weight
	When the user clicks Save and continue
	Then the Additional details page should be displayed
	When the user selects any radio button on the Additional details page
	Then the corresponding radio button should be selected
	When the user clicks Save and continue
	Then the Accompanying documents page should be displayed
	When the user enters the following details:
		| Document type      | Invoice    |
		| Document reference | INV12345   |
		| Date of issue      | 2025-11-24 |
	And the user adds an attachment
	Then the user should be able to click Save and continue
	When the user clicks Search for an approved establishment
	Then the list of establishments should be displayed, filtered by Country of origin to China
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
	When the user clicks Same as consignee for the "Importer"
	Then the importer should be populated with the same details as the consignee
	When the user clicks Add a place of destination
	Then the Search for an existing place of destination page should be displayed
	When the user selects a place of destination with a UK country
	Then the chosen place of destination should be displayed on the Addresses page
	When the user clicks Save and continue
	Then the Transport to the port of entry page should be displayed
	When the user populates the transport details and clicks Save and continue
	Then the Goods movement services page should be displayed
	When the user selects "No" for Are you using the Common Transit Convention (CTC)?
	Then the "No" radio button should be selected
	When the user clicks Save and continue
	Then the Contact details page should be displayed, pre-populated with the user's details
	When the user clicks Save and continue
	Then the Nominated contacts page should be displayed
	When the user clicks Save and continue
	Then the Contact address for consignment page should be displayed
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
	When the user logs in to BTMS
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier and checks all details match
	Then the CHED details should match each commodity line's:
		| Commodity code  |            41015050 |
		| Description     | Dried or dry-salted |
		| Quantity/Weight | 1000 kg             |
		| Authority       | POAO                |
		| Decision        | Decision not given  |
	When the user logs out of BTMS
	Then the user should be logged out successfully
	When the user logs into IPAFFS Part 2 Inspector for the appropriate BCP
	Then the user should be logged into IPAFFS Part 2
	When the user searches for the newly created notification on the Import notifications page
	Then the notification should be found with status "NEW"
	When the user opens the notification under test
	Then the Decision Hub page should be displayed
	When the user clicks Save and set as in progress
	Then the notification status should change from "NEW" to "IN PROGRESS"
	When the user enters a local reference number and clicks Save and continue
	Then the Documentary check page should be displayed
	When the user selects Satisfactory for the documentary check and clicks Save and continue
	Then the Identity and physical checks page should be displayed
	When the user selects "Satisfactory" for both identity and physical checks
	And the user clicks Save and continue
	Then the Seal numbers page should be displayed
	When the user clicks Save and continue
	Then the Laboratory tests page should be displayed
	When the user clicks Save and continue
	Then the Decision page should be displayed
	When the user clicks Save and continue
	Then the Review outcome decision page should be displayed
	When the user populates the Date and time of checks
	And user clicks Submit decision
	Then the Your checks have been submitted page should be displayed
	When the user logs out of IPAFFS Part 2
	Then the user should be logged out successfully
	When the user logs in to BTMS again
	Then the BTMS search screen should be displayed
	When the user searches for the CHED created earlier and checks all details match
	Then the CHED details should match each commodity line's:
		| Commodity code  |                       41015050 |
		| Description     | Dried or dry-salted            |
		| Quantity/Weight | 1000 kg                        |
		| Authority       | POAO                           |
		| Decision        | Acceptable for Internal Market |
	When the user logs out of BTMS
	Then the user should be logged out successfully