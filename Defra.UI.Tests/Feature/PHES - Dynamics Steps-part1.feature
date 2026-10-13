Feature: PHES-Dynamics-part1

End-to-end PHES regression tests
	
@SPS-4289
Scenario: Dynamics 1 - SPS-4289
	# ---------- Dynamics Portal - Process the seed sampling inspection tasks ----------
	When I am logged in to the 'IDCOMS' app as 'Inspector'
	And I click on 'Work Orders' under the 'Common' area
	And I change the Work Orders view to 'All Work Orders - Exports'	
	Then the 'All Work Orders - Exports' view is displayed on the Work Orders page	
	When I search Work Orders for the application created in the Exporter Portal	
	Then the work order submitted in the Exporter Portal should be returned	
	And the Application Type is set to Pre-Export Application
	When I open the record in the grid
	Then the 'Pre-Export' tab is displayed and selected
	And the Substatus of the Work Order should be 'Unassigned'
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button	
	Then the Substatus of the Work Order should be 'Assigned'
	And the Owner of the Work Order should be me
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Seed Sampling Inspection Task' 'Issue a Report Task'
	When I click on the 'Seed Sampling Inspection Task' task
	And I maximise the popup
	Then I verify the 'Seed Sampling Inspection Task' popup is displayed
	And the 'Summary' tab is displayed and selected	
	And the 'Summary' 'Inspection Results' 'Notes' 'Related' tabs are displayed for the Work Order Task 
	When I select the 'Inspection Results' tab
	Then the 'Inspection Results' tab is displayed and selected	
	When I select the Visit field on the Work Order Task page	
	And I select the New button on the Visits dropdown	
	Then the Quick Create Visit form is displayed
	When I add my name to the Quick Create form
	And I add a valid date scheduled to the Quick Create form at least 2 days in the future
	And I add a valid time scheduled to the Quick Create form
	And I click the Save and Close button on the Quick Create form
	Then the Quick Create Visit form is closed
	And the 'Inspection Results' tab is displayed and selected
	When I scroll down to the Inspection Results grid on the Work Order Task page
	Then I can see 1 row in the Inspection Results grid	
	And I can see the following columns are present in the Inspection Results grid 'Status Reason' 'Sample ID (Consignment Item)' 'Lot Reference (Consignment Item)' 'Genus (Consignment Item)' 'Species (Consignment Item)' 'Variety (Consignment Item)' 'Description (Consignment Item)' 'Total Quantity (Consignment Item)' 'Unit of Measurement (Consignment Item)' 'Inspection Result' 'Reason' 'Task Type'
	When I select the row in the Inspection Results grid
	Then I can only edit the 'Inspection Result' column in the Inspection Results grid 
	When I expand the row in the Inspection Results grid	
	Then a nested row is displayed under the Inspection Results grid	
	And I can see the following columns are present in the nested row 'Country' 'Commodity Line' 'Passed Destination Countries' 'Reason for Country Not Valid'
	When I contract row in the Inspection Results grid
	Then the nested row is hidden under the Inspection Results grid
	When I select the row in the Inspection Results grid
	And I click the Pass Selected button above the Inspection Results grid	
	Then the Inspection Result is 'Pass'
	When I select the row in the Inspection Results grid	
	And I click the Submit Results button above the Inspection Results grid	
	Then the Status Reason is 'Submitted'
	When I select the 'Summary' tab
	Then the 'Summary' tab is displayed and selected	
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field	
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	When I enter 5 in the 'Travel' column
	And I enter 5 in the 'Inspection' column
	And I enter 5 in the 'Admin' column
	And I enter 5 in the 'Training given (Client)' column
	And I enter 5 in the 'Training given (Inspector)' column
	And I enter 5 in the 'Training received' column
	When I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the 'Summary' tab is displayed and selected
	And the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Issue a Report Task'
	And the 'Seed Sampling Inspection Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	When I click on the 'Issue a Report Task' task
	And I maximise the popup
	Then I verify the 'Issue a Report Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	When I enter 5 in the 'Admin' column
	And I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the 'Summary' tab is displayed and selected
	And the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task'
	And the 'Issue a Report Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	When I select the 'Pre-Export' tab
	Then the 'Pre-Export' tab is displayed and selected
	When I click the Export Application link in the Application & Inspection Address section	
	Then the Export Application page is displayed for the application created in the Exporter Portal	
	And the Status of the Export Application should be 'Inactive'
	And the Status Reason of the Export Application should be 'Completed'
	When I select the 'Related' tab
	And I select the 'Stocks' tab from the Related tab dropdown
	Then the 'Stocks' tab is displayed and selected
	And I can see 1 record displayed within the Stock subgrid
	And I make a note of the stock name