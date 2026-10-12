Feature: PHES-Dynamics-part2

End-to-end PHES regression tests
	
@SPS-4289
Scenario: Dynamics 2 - SPS-4289
	# ---------- Dynamics Portal - Process the phytosanitary certificate export application ----------
	When I switch back to the Dynamics tab
	# @agent: Include dismissal of the sign in banner after clicking Discard changes if it's there
	And I click the Discard changes button on the Unsaved changes popup 
	And I click on 'Export Applications' under the 'Exports' area
	# @agent: Use this step definition to help implement the step below: Then the 'Inactive Import Notifications' view is displayed
	Then the 'Active Export Applications' view is displayed on the Export Applications page
	# @agent: Use this step definition to help implement the step below: When I search Import Notifications for the notification created in IPAFFS
	When I search Export Applications for the application created in the Exporter Portal
	# @agent: Use this step definition to help implement the step below: Then the notification created in IPAFFS should be returned
	Then the export application submitted in the Exporter Portal should be returned
	When I open the record in the grid
	Then the 'General' tab is displayed and selected
	# @agent: Use this step definition to help implement the step below: And the Substatus of the Work Order should be Assigned
	And the Status of the Export Applications should be 'Active'
	And the Status Reason of the Export Applications should be 'Submitted'
	And the Commodity Group should be 'Seeds'
	And the Application Method should be 'Online'
	And the Application Type should be 'Export Application'
	# @agent: Use this step definition to help implement the step below eventhough on this occasion we only want to check displayed not selected: [Then(@"the '(.*)' tab is displayed and selected")]
	And the 'Stock Requests' tab is displayed
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	# @agent: Add to ExportApplicationSteps
	And I can see 1 row in the Stock Requests subgrid
	When I select the 'General' tab
	Then the 'General' tab is displayed and selected
	# @agent: Use this step definition to help implement the step below: [When("I click the reference number in the Work Order field for the notification created in IPAFFS")]
	When I click the Work Order link in the Application Details section
	# @agent: Use this step definition to help implement the step below: [Then("I verify the Work Order page is displayed for the notification created in IPAFFS")]
	Then the Work Order page is displayed for the application created in the Exporter Portal
	And the 'Export' tab is displayed and selected
	# @agent: Use this step definition to help implement the step below eventhough on this occasion we only want to check displayed not selected: [Then(@"the '(.*)' tab is displayed and selected")]
	And the 'Stock Requests' tab is displayed
	And the Application Type should be 'Export Application'
	And the Commodity Group should be 'Seeds'
	And the Substatus of the Work Order should be 'Unassigned'
	And the Owner of the Work Order should be 'CIT Triage Team'
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button
	Then the Substatus of the Work Order should be 'Assigned'
	And the Owner of the Work Order should be me
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	# @agent: Add to ExportApplicationSteps
	And I can see 1 row in the Stock Requests subgrid
	And the Status Reason of the Stock Request should be 'Pending'
	When I select the record for the stock reference created in the Exporter Portal
	# @agent: Use this step definition to help implement the step below: [When(@"I click Add my time within the Time Recording section")]
	Then the Accept Request button should be displayed
	When I click the Accept Request button
	Then the Status Reason of the Stock Request should be 'Accepted'
	# @agent: Use this step definition to help implement the step below: [When(@"I double click on a Commodity Line with Regulatory Authority set to '(.*)'")]
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the 'Summary' tab is displayed and selected
	And the four Stock Draw Down Quantities fields are displayed to three decimal places
	When I click the Back button in the command bar
	Then the 'Stock Requests' tab is displayed and selected
	When I select the 'Phyto Commodities' tab
	Then the 'Phyto Commodities' tab is displayed and selected
	# @agent: Use this step definition to help implement the step below: [When(@"I double click on a Commodity Line with Regulatory Authority set to '(.*)'")]
	And I can see 1 row in the Commodity Lines subgrid
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Issue Phyto Task'
	When I click on the 'Pre Inspection Task' task
	And I maximise the popup
	Then I verify the 'Pre Inspection Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	# @agent: Add to WorkOrderTasksSteps
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	# @agent: Use this step definition to help implement the step below: [Then(@"a new row appears in the grid containing my name")]
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	# @agent: Use this step definition to help implement the step below: [When(@"I enter '(.*)' in the (Admin|Travel|Inspection) column")]
	When I enter 5 in the 'Admin' column
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
	And I can see following Work Order Tasks 'Issue Phyto Task'
	# @agent: Use this step definition to help implement the step below: [Then(@"I can see following Work Order Tasks
	And the 'Pre Inspection Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	When I click on the 'Issue Phyto Task' task
	And I maximise the popup
	Then I verify the 'Issue Phyto Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I select the 'Phyto Commodities' tab
	Then the 'Phyto Commodities' tab is displayed and selected
	And I can see 1 row in the Commodity Lines subgrid
	When I select the 'Summary' tab
	Then the 'Summary' tab is displayed and selected
	# @agent: Add to WorkOrderTasksSteps
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	# @agent: Use this step definition to help implement the step below: [Then(@"a new row appears in the grid containing my name")]
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	# @agent: Use this step definition to help implement the step below: [When(@"I enter '(.*)' in the (Admin|Travel|Inspection) column")]
	When I enter 5 in the 'Admin' column
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
	# @agent: Use this step definition to help implement the step below: [Then(@"I can see following Work Order Tasks
	And the 'Issue Phyto Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	And the Substatus of the Work Order should be 'Certificate Issued'
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	# @agent: Add to ExportApplicationSteps
	And I cannot see any row in the Stock Requests subgrid
	# @agent: Use this step definition to help implement the step below: [When(@"I change the Import Notifications view to '(.*)'")]
	When I change the view to 'All Stock Requests' in the Stock Requests subgrid
	# @agent: Use this step definition to help implement the step below: [Then(@"the '(.*)' view is displayed")]
	Then the 'All Stock Requests' view is displayed	in the Stock Rquests subgrid
	And I can see 1 row in the Stock Requests subgrid
	When I open the record for the stock reference created in the Exporter Portal
	Then the Stock Request page is displayed
	And the 'General' tab is displayed and selected
	And the Status of the Stock Request should be 'Inactive'
	And the Status Reason of the Stock Request should be 'Certificate Issued'
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the application created in the Exporter Portal
	And the 'Stock Requests' tab is displayed and selected
	And the 'Active Stock Requests' view is displayed in the Stock Rquests subgrid
	When I change the view to 'All Stock Requests' in the Stock Requests subgrid
	# @agent: Use this step definition to help implement the step below: [Then(@"the '(.*)' view is displayed")]
	Then the 'All Stock Requests' view is displayed	in the Stock Rquests subgrid
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the 'Summary' tab is displayed and selected
	And the Total Draw Down quantity is '7,000.000'
	And the Total Draw Down Including Pending quantity is '7,000.000'
	And the Remaining Approved Stock quantity is '3,000.000'
	And the Remaining Approved Stock Including Pending quantity is '3,000.000'
	When I click the Back button in the command bar
	Then the 'Stock Requests' tab is displayed and selected
	When I select the 'Exports' tab
	Then the 'Exports' tab is displayed and selected
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the application created in the Exporter Portal
	And the 'Stock Requests' tab is displayed and selected
	When I select the 'Exports' tab
	Then the 'Exports' tab is displayed and selected
	# @agent: Use this step definition to help implement the step below: [When("I click the reference number in the Work Order field for the notification created in IPAFFS")]
	When I click the Export Application link in the Application & Inspection Address section
	# @agent: Use this step definition to help implement the step below: [Then("I verify the Import Notification page is displayed for the notification created in IPAFFS")]
	Then the Export Application page is displayed for the application created in the Exporter Portal
	And the Status of the Export Application should be 'Inactive'
	And the Status Reason of the Export Application should be 'Completed'
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the application created in the Exporter Portal
	And the 'Exports' tab is displayed and selected
	# @agent: Use this step definition to help implement the step below: [When("I click the Assign command")]
	When I click View Phyto command from the header ribbon
	# @agent: Use this step definition to help implement the step below: [Then("the certificate should be displayed in a new browser tab")]
	Then the Phytosanitary certificate is displayed in a new window
	# @agent: Use this step definition to help implement the step below: [When("the user downloads the PDF for validation using the current browser session")]
	When I download the Phytosanitary certificate PDF
	# @agent: Use this step definition to help implement the step below:  [When("the user checks that the data in the certificate matches the data entered into the CHED PP notification")]
	Then the Phytosanitary certificate displays the details of the application submitted
	# @agent: Use this step definition to help implement the step below: [When("the user closes the PDF browser tab")]
	When I close the PDF window
	# @agent: Use this step definition to help implement the step below: [Then("the browser tab is closed")]
	Then the window is closed