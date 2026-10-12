Feature: PHES-Dynamics-part1

End-to-end PHES regression tests
	
@SPS-4289
Scenario: Dynamics 1 - SPS-4289
	# ---------- Dynamics Portal - Process the seed sampling inspection tasks ----------
	When I am logged in to the 'IDCOMS' app as 'Inspector'
	And I click on 'Work Orders' under the 'Common' area
	# @agent: Add to new step file WorkOrdersSteps.cs
	# @agent: Use this step definition to help implement the step below: [When(@"I change the Import Notifications view to '(.*)'")]
	# id="ViewSelector_3_c72cf1ad-7817-eb11-a813-000d3a2c32cb"
	And I change the Work Orders view to 'All Work Orders - Exports'
	# @agent: Add to new step file WorkOrdersSteps.cs
	# @agent: Use this step definition to help implement the step below: [Then(@"the '(.*)' view is displayed")] 
	# <label class="ms-Label viewName root-408">All Work Orders - Exports</label>
	# aria-label="All Work Orders - Exports" 
	Then the 'All Work Orders - Exports' view is displayed on the Work Orders page
	# @agent: Add to new step file WorkOrdersSteps.cs	
	# @agent: Use this step definition to help implement the step below: [When("I search Importer Notifications for the notification created in IPAFFS")]
	# aria-label="Work Order Filter by keyword"
	When I search Work Orders for the application created in the Exporter Portal
	# @agent: Add to new step file WorkOrdersSteps.cs	
	# @agent: Use this step definition to help implement the step below: [Then("the notification created in IPAFFS should be returned")]
	# col-id="trd_applicationreference"
	Then the work order submitted in the Exporter Portal should be returned
	# @agent: Add to new step file WorkOrdersSteps.cs	
	# @agent: Use this method to help implement the step below since it may need a similar helper to scroll untill the Application Type is visible: private void ScrollGridUntilBothColumnsVisible()
	# col-id="msdyn_workordertype"
	And the Application Type is set to Pre-Export Application
	When I open the record in the grid
	Then the 'Pre-Export' tab is displayed and selected
	# @agent: Add to existing step file WorkOrderSteps.cs
	# @agent: Use this step definition to help implement the step below: [Then("the Substatus of the Work Order should be Assigned")]
	# data-name="header_msdyn_substatus"
	#And the Substatus of the Work Order should be 'Unassigned'
	#When I click the Assign command
	#Then I can see the Assign Work Order popup is displayed
	#When I click the Assign button
	# @agent: Use this step definition to help implement the step below: [Then("the Substatus of the Work Order should be Assigned")]
	# @agent: Add to existing step file WorkOrderSteps.cs
	# data-name="header_msdyn_substatus"
	#Then the Substatus of the Work Order should be 'Assigned'
	#And the Owner of the Work Order should be me
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Seed Sampling Inspection Task' 'Issue a Report Task'
	When I click on the 'Seed Sampling Inspection Task' task
	And I maximise the popup
	Then I verify the 'Seed Sampling Inspection Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## @agent: Use this step definition to help implement the step below but remember that for this new step we only want to check for visibility/displayed not selected too: [Then(@"the '(.*)' tab is displayed and selected")]
	## aria-label="Work Order Task Form" 
	#And the 'Summary' 'Inspection Results' 'Notes' 'Related' tabs are displayed for the Work Order Task 
	#When I select the 'Inspection Results' tab
	#Then the 'Inspection Results' tab is displayed and selected
	## @agent: Add to WorkOrderTasksSteps
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## data-id="trd_visitid.fieldControl-Lookup_trd_visitid"
	#When I select the Visit field on the Work Order Task page
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## data-id="trd_visitid.fieldControl-LookupResultsPopup_trd_visitid_addNewBtn_0"
	#And I select the New button on the Visits dropdown
	## @agent: Add to existing step file QuickCreateFormSteps.cs
	## <h1 data-id="quickHeaderTitle" class="pa-vd pa-bq pa-k pa-ix pa-iy pa-vb pa-ye pa-fj pa-afs ">Quick Create: Visit</h1>
	#Then the Quick Create Visit form is displayed
	## @agent: Add to existing step file QuickCreateFormSteps.cs
	## data-id="trd_name.fieldControl-text-box-text"
	#When I add my name to the Quick Create form
	## @agent: Add to existing step file QuickCreateFormSteps.cs
	## data-id="trd_datescheduled.fieldControl._datecontrol-date-container"
	## id="datePicker-popupSurface1"
	#And I add a valid date scheduled to the Quick Create form at least 2 days in the future
	#And I add a valid time scheduled to the Quick Create form
	## @agent: Add to existing step file QuickCreateFormSteps.cs
	## data-id="quickCreateSaveAndCloseBtn"
	#And I click the Save and Close button on the Quick Create form
	## @agent: Add to existing step file QuickCreateFormSteps.cs
	#Then the Quick Create Visit form is closed
	#And the 'Inspection Results' tab is displayed and selected
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## data-id="dataSetRoot_InspectionResults"
	#When I scroll down to the Inspection Results grid on the Work Order Task page
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	#Then I can see 1 row in the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## data-id="statuscode"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_samplereference"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_lotreference"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_botanicalgenus"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_botanicalspecies"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_variety"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_description"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_quantity"
	## data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_unitofmeasurement"
	## data-id="trd_fminspectionresult"
	## data-id="trd_reasonforfailure"
	## data-id="trd_tasktype"
	#And I can see the following columns are present in the Inspection Results grid 'Status Reason' 'Sample ID (Consignment Item)' 'Lot Reference (Consignment Item)' 'Genus (Consignment Item)' 'Species (Consignment Item)' 'Variety (Consignment Item)' 'Description (Consignment Item)' 'Total Quantity (Consignment Item)' 'Unit of Measurement (Consignment Item)' 'Inspection Result' 'Reason' 'Task Type'
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## @agent: Use this step definition to help implement the step below: [When(@"I select the new row in the grid")]
	#When I select the row in the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	#Then I can only edit the 'Inspection Result' column in the Inspection Results grid 
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## class="cc-grid-nested-expander cc-grid-nested-expander-expand"
	#When I expand the row in the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## aria-label="(Deprecated) Editable Grid"
	#Then a nested row is displayed under the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## data-id="trd_countryid"
	## data-id="trd_consignmentitemid"
	## data-id="trd_passeddestinationcountries"
	## data-id="trd_reasonforcountrynotvalid"
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	#And I can see the following columns are present in the nested row 'Country' 'Commodity Line' 'Passed Destination Countries' 'Reason for Country Not Valid'
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	#When I contract row in the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	#Then the nested row is hidden under the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	#When I select the row in the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## @agent: Use this step definition to help implement the step below: [When(@"I click Submit Time")]
	## data-id="trd_inspectionresult|NoRelationship|SubGridStandard|trd.trd_inspectionresult.PassSelected"
	#And I click the Pass Selected button above the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## @agent: Use this step definition to help implement the step below: [Then(@"the entry status is '(.*)'")]
	#Then the Inspection Result is 'Pass'
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	#When I select the row in the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## @agent: Use this step definition to help implement the step below: [When(@"I click Submit Time")]
	## data-id="trd_inspectionresult|NoRelationship|SubGridStandard|trd.trd_inspectionresult.SubmitResults"
	#And I click the Submit Results button above the Inspection Results grid
	## @agent: Add to existing step file WorkOrderTasksSteps.cs
	## @agent: Use this step definition to help implement the step below: [Then(@"the entry status is '(.*)'")]
	#Then the Status Reason is 'Submitted'
	#When I select the 'Summary' tab
	Then the 'Summary' tab is displayed and selected
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# data-id="trd_timerecordinginspectorids.fieldControl_container"
	When I select the Time Recording Inspectors field on the Work Order Task page
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	And I search for my name in the Time Recording Inspectors field
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# @agent: Use this step definition to help implement the step below: [Then(@"a new row appears in the grid containing my name")]
	# data-lp-id="e8e2a536-5e0c-4473-96df-abbf37d83162|timerecordings_subgrid|msdyn_workorderservicetask|cc-grid|grid"
	# data-id="trd_bookableresource"
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# @agent: Use this step definition to help implement the step below: [When(@"I enter '(.*)' in the (Admin|Travel|Inspection) column")]
	# data-id="trd_travel"
	When I enter 5 in the 'Travel' column
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# data-id="trd_inspection"
	And I enter 5 in the 'Inspection' column
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# data-id="trd_admin"
	And I enter 5 in the 'Admin' column
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# data-id="trd_trainingprovidedtotrade"
	And I enter 5 in the 'Training given (Client)' column
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# data-id="trd_trainingprovidedtoinspectors"
	And I enter 5 in the 'Training given (Inspector)' column
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# data-id="trd_trainingreceived"
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
	# @agent: Add to existing step file WorkOrderSteps.cs
	# @agent: Use this step definition to help implement the step below: [Then(@"I can see following Work Order Tasks
	And the 'Seed Sampling Inspection Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	When I click on the 'Issue a Report Task' task
	And I maximise the popup
	Then I verify the 'Issue a Report Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	When I select the Time Recording Inspectors field on the Work Order Task page
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	And I search for my name in the Time Recording Inspectors field
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# @agent: Use this step definition to help implement the step below: [Then(@"a new row appears in the grid containing my name")]
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	# @agent: Add to existing step file WorkOrderTasksSteps.cs
	# @agent: Use this step definition to help implement the step below: [When(@"I enter '(.*)' in the (Admin|Travel|Inspection) column")]
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
	# @agent: Add to existing step file WorkOrderSteps.cs
	# @agent: Use this step definition to help implement the step below: [Then(@"I can see following Work Order Tasks
	And the 'Issue a Report Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	When I select the 'Pre-Export' tab
	Then the 'Pre-Export' tab is displayed and selected
	# @agent: Add to existing step file WorkOrderSteps.cs
	# @agent: Use this step definition to help implement the step below: [When("I click the reference number in the Work Order field for the notification created in IPAFFS")]
	# data-id="trd_exportapplicationid.fieldControl-LookupResultsDropdown_trd_exportapplicationid_selected_tag_text"
	When I click the Export Application link in the Application & Inspection Address section
	# @agent: Add to new step file ExportApplicationSteps.cs
	# @agent: Use this step definition to help implement the step below: [Then("I verify the Import Notification page is displayed for the notification created in IPAFFS")]
	# data-id="header_title"
	# title="1762431094198"
	Then the Export Application page is displayed for the application created in the Exporter Portal
	# @agent: Add to new step file ExportApplicationSteps.cs
	# @agent: Use this step definition to help implement the 2 steps below: And the Substatus of the Work Order should be Assigned
	# data-name="header_statecode"
	And the Status of the Export Application should be 'Inactive'
	# @agent: Add to new step file ExportApplicationSteps.cs
	And the Status Reason of the Export Application should be 'Completed'
	When I select the 'Related' tab
	And I select the 'Stocks' tab from the Related tab dropdown
	Then the 'Stocks' tab is displayed and selected
	# @agent: Add to new step file ExportApplicationSteps.cs
	# data-lp-id="Microsoft.PowerApps.PowerAppsOneGrid|entity_control|trd_exportapplication|0a9b57f2-df26-4ed1-9cff-e817e668fad8|trd_stock|trd_trd_exportapplication_trd_stock_exportapplication"
	And I can see 1 record displayed within the Stock subgrid
	# @agent: Add to new step file ExportApplicationSteps.cs
	# data-testid="columnHeader"
	# data-testid="trd_name"
	And I make a note of the stock name