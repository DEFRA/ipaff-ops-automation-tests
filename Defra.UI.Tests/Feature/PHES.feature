@PHES
Feature: PHES

End-to-end PHES regression tests

@PHES-Happy-Path
Scenario: New exporter application for a phytosanitary certificate - PHES Happy Path
	Given that I navigate to the Exporter Portal
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should be redirected to the Sign in using Government Gateway page
	When I have provided the Exporter Trader credentials and signin
	Then the Exporter Portal homepage should be displayed
	When the user clicks the Start a new application button
	Then the What are you exporting? page is displayed
	When the user selects 'Bulbs' and clicks the Continue button on the What are you exporting? page
	Then the What do you need to do? page is displayed
	When the user selects 'Apply for a phytosanitary certificate' and clicks the Continue button on the What do you need to do? page
	Then the Destination country page is displayed
	When the user searches for destination country 'Norway' and clicks the Continue button
	Then the Regulations for exporting 'bulbs' to 'Norway' page is displayed
	When the user ticks the exporting regulations checkbox 
	And the user clicks the Continue button on the Regulations for exporting page
	Then the Create a reference page is displayed
	When the user enters a reference number and clicks the Save and continue button
	Then the Your application page is displayed
	When the user clicks the What is the exporter address? link
	Then the Is this the exporter address? page is displayed
	When the user selects 'Yes' and clicks the Save and continue button on the Is this the exporter address? page
	Then the Your application page is displayed
	When the user clicks the What's in your consignment? link
	Then the How do you want to add commodities to your consignment? page is displayed
	When the user selects 'Manually, by adding one at a time' and clicks the Continue button on the How do you want to add commodities to your consignment? page
	Then the Add 'bulbs' to your consignment page is displayed
	When the user describes the commodity with botanical name 'TULYA', commodity type 'Bulbs', quantity '10', unit of measurement 'Units', number of packages '10', type of packaging 'Box', countries of origin 'Netherlands'
	And the user clicks the Save and continue button on the Add 'bulbs' to your consignment page
	Then the Your commodities page is displayed with the commodity line just entered
	When the user selects 'No, I have finished adding commodities' and clicks the Save and continue button on the Your commodities page
	Then the Your application page is displayed
	When the user clicks the What are the inspection details? link
	Then the What are the inspection details? page is displayed
	When the user selects an inspection address and continues
	Then the Do you want to select this inspection address? page is displayed
	When the user selects 'Yes, select this inspection address' and clicks the Continue button on the Do you want to select this inspection address? page
	Then the Who is the contact for this application? page is displayed
	When the user enters a contact name for the inspector to contact about the application
	And the user enters a phone number for the inspector to contact about the application
	And the user clicks the Save and continue button on the Who is the contact for this application? page
	Then the When will the consignment be ready? page is displayed
	When the user enters a valid date for when the consignment will be ready for inspection at least 5 days in the future
	And the user clicks the Save and continue button on the When will the consignment be ready? page
	Then the Your application page is displayed
	When the user clicks the Who are you sending this consignment to? link
	Then the Who are you sending this consignment to? page is displayed
	When the user enters the conisgnee details and clicks the Continue button on the Who are you sending this consignment to? page
	Then the Your application page is displayed
	When the user clicks the How will this consignment be transported? link
	Then the How will this consignment be transported? page is displayed
	When the user enters the value of the consignment in pounds 1200.00
	And the user selects 'Yes' for Do you know the point of entry?
	And the user enters the point of entry 'CA NOY'
	And the user selects a transport method 'Road or ferry'
	And the user clicks the Save and continue button on the How will this consignment be transported? page
	Then the Your application page is displayed
	When the user clicks the Supporting documents link
	Then the Supporting documents page is displayed
	When the user uploads a supporting document and clicks the Save and continue button
	Then the Your application page is displayed
	When the user clicks the Check your answers and submit your application link
	Then the Review your answers page is displayed
	When the user clicks the Continue button on the Review your answers page
	Then the When do you need the certificate? page is displayed
	When the user enters a valid date for when the certificate is needed at least 5 days in the future 
	And the user clicks the Continue button on the When do you need the certificate? page
	Then the Exporter declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	Then the You have successfully submitted your application for a phytosanitary certificate page is displayed
	And the user records the APHA reference number

@SPS-4289
Scenario: Exporter application for seed sampling followed by a phytosanitary certificate application using the stock - SPS-4289
	# ---------- Exporter Portal - Arrange seed sampling ----------
	Given that I navigate to the Exporter Portal
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should be redirected to the Sign in using Government Gateway page
	When I have provided the Exporter Trader credentials and signin
	Then the Exporter Portal homepage should be displayed
	When the user clicks the Start a new application button
	Then the What are you exporting? page is displayed
	When the user selects 'Seeds for planting' and clicks the Continue button on the What are you exporting? page
	Then the What do you need to do? page is displayed
	When the user selects 'Arrange seed sampling' and clicks the Continue button on the What do you need to do? page
	Then the Create a reference page is displayed
	When the user enters a reference number and clicks the Save and continue button
	Then the Your application for seed sampling page is displayed
	And the 'Create a reference' task is marked as 'COMPLETE'
	When the user clicks the Seeds to be sampled link
	Then the How do you want to add commodities to this application? page is displayed
	When the user selects 'All at once, by uploading a file' and clicks the Continue button on the How do you want to add commodities to this application? page
	Then the Upload a file page is displayed
	When the user downloads the CSV template
	Then the CSV template is downloaded
	And the only units of measurements listed in the template are 'grams' 'kilograms' 'tonnes'
	When the user clicks the Back link on the Upload a file page
	Then the How do you want to add commodities to this application? page is displayed
	When the user selects 'Manually, by adding one at a time' and clicks the Continue button on the How do you want to add commodities to this application? page
	Then the Add 'seeds' to your consignment page is displayed
 	When the user describes the seed commodity with botanical name 'FLUTI', variety 'special', lot reference '12345', quantity '10000', unit of measurement 'Grams', chemically treated 'Yes', chemicals used 'Pesticide', country where grown 'United Kingdom', destination countries 'Netherlands'
	And the only options available for Unit of measurement are 'Grams' 'Kilograms' 'Tonnes'
	Then the seed commodity details are displayed with botanical name 'FLUTI', variety 'special', lot reference '12345', quantity '10000', unit of measurement 'Grams', chemically treated 'Yes', chemicals used 'Pesticide', country where grown 'United Kingdom', destination countries 'Netherlands'
	When the user clicks the Save and continue button on the Add 'seeds' to your consignment page
	Then the Your commodities page is displayed
	And the commodity table is displayed for arranging for seed sampling to export seeds for planting
	And the 'Do you want to add more seeds to be sampled?' question is displayed on the Your commodities page
	And there are radio buttons present on the Your commodities page
	And there is no option to Save and continue later on the Your commodities page
	When the user selects 'No, I have finished adding commodities' and clicks the Save and continue button on the Your commodities page
	Then the Your application for seed sampling page is displayed
	And the 'Seeds to be sampled' task is marked as 'COMPLETE'
	When the user clicks the Inspection details link
	Then the Where are the seeds? page is displayed
	When the user selects an inspection address and continues
	Then the Do you want to select this inspection address? page is displayed
	When the user selects 'Yes, select this inspection address' and clicks the Continue button on the Do you want to select this inspection address? page
	Then the Who is the contact for this application? page is displayed
	And the fields available on the Who is the contact for this application? page are 'Contact name, Phone number, Email address'
	When the user enters a contact name for the inspector to contact about the application
	And the user enters a phone number for the inspector to contact about the application
	And the user enters an email address for the inspector to contact about the application
	And the user clicks the Save and continue button on the Who is the contact for this application? page
	Then the When will the seeds be ready for sampling? page is displayed
	When the user enters a valid date for when the seeds will be ready for sampling at least 4 days in the future
	And the user clicks the Save and continue button on the When will the seeds be ready for sampling? page
	Then the Your application for seed sampling page is displayed
	And the 'Inspection details' task is marked as 'COMPLETE'
	When the user clicks the Check your answers and submit your application link
	Then the Review your answers page is displayed
	When the user clicks the Continue button on the Review your answers page
	Then the Declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	Then the You have successfully submitted your seed sampling application page is displayed
	And the user records the APHA reference number for 'Seed Sampling Application'
	# ---------- Dynamics Portal - Process the seed sampling inspection tasks ----------
	When I am logged in to the 'IDCOMS' app as 'Inspector'
	And I click on 'Work Orders' under the 'Common' area
	And I change the Work Orders view to 'All Work Orders - Exports'	
	Then the 'All Work Orders - Exports' view is displayed on the Work Orders page	
	When I search Work Orders for the 'Seed Sampling Application' created in the Exporter Portal
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
	Then the Export Application page is displayed for the 'Seed Sampling Application' created in the Exporter Portal	
	And the Status of the Export Application should be 'Inactive'
	And the Status Reason of the Export Application should be 'Completed'
	When I select the 'Related' tab
	And I select the 'Stocks' tab from the Related tab dropdown
	Then the 'Stocks' tab is displayed and selected
	And I can see 1 record displayed within the Stock subgrid
	And I make a note of the stock name
	# ---------- Exporter Portal - Apply for a phytosanitary certificate using the stock ----------
	When I switch to the Exporter Portal window
	And I navigate to the Exporter Portal
	Then the Exporter Portal homepage should be displayed
	When the user clicks the Start a new application button
	Then the What are you exporting? page is displayed
	When the user selects 'Seeds for planting' and clicks the Continue button on the What are you exporting? page
	Then the What do you need to do? page is displayed
	When the user selects 'Apply for a phytosanitary certificate' and clicks the Continue button on the What do you need to do? page
	Then the Destination country page is displayed
	When the user searches for destination country 'Netherlands' and clicks the Continue button
	Then the Regulations for exporting 'seeds' to 'Netherlands' page is displayed
	And the guidance needs you to have a phytosanitary certificate to export seeds to 'Netherlands'
	And the guidance needs you to describe your consignment using the botanical name (genus and species)
	When the user ticks the exporting regulations checkbox 
	And the user clicks the Continue button on the Regulations for exporting page
	Then the Create a reference page is displayed
	When the user enters a reference number and clicks the Save and continue button
	Then the Your application page is displayed
	And the 'Create a reference' task is marked as 'COMPLETE'
	When the user clicks the What is the exporter address? link
	Then the Is this the exporter address? page is displayed
	And the guidance is This is the address saved to your Defra account.
	And the 'Yes' and 'No' radio buttons are displayed and unselected by default
	When the user selects 'Yes' and clicks the Save and continue button on the Is this the exporter address? page
	Then the Your application page is displayed
	And the 'What is the exporter address?' task is marked as 'COMPLETE'
	When the user clicks the What's in your consignment? link
	Then the How do you want to add commodities to your consignment? page is displayed
	When the user selects 'Manually, by adding one at a time' and clicks the Continue button on the How do you want to add commodities to your consignment? page
	Then the Add 'seeds' to your consignment page is displayed
	When the user describes the seed commodity with botanical name 'FLUTI', variety 'special', lot reference '12345', sample reference, quantity '7', unit of measurement 'Kilograms', number of packages '7', type of packaging 'Box', countries of origin 'United Kingdom'
	And the user clicks the Save and continue button on the Add 'seeds' to your consignment page
	Then the Your commodities page is displayed
	And the commodity table is displayed for applying for a phytosanitary certificate to export seeds for planting
	When the user selects 'No, I have finished adding commodities' and clicks the Save and continue button on the Your commodities page
	Then the Your application page is displayed
	And the "What's in your consignment" task is marked as 'COMPLETE'
	When the user clicks the Who are you sending this consignment to? link
	Then the Who are you sending this consignment to? page is displayed
	When the user enters the conisgnee details and clicks the Continue button on the Who are you sending this consignment to? page
	Then the Your application page is displayed
	And the 'Who are you sending this consignment to?' task is marked as 'COMPLETE'
	When the user clicks the How will this consignment be transported? link
	Then the How will this consignment be transported? page is displayed
	When the user enters the value of the consignment in pounds 1200.00
	And the user selects 'Yes' for Do you know the point of entry?
	And the user enters the point of entry 'NL AMS'
	And the user selects a transport method 'Road or ferry'
	And the user clicks the Save and continue button on the How will this consignment be transported? page
	Then the Your application page is displayed
	And the 'How will this consignment be transported?' task is marked as 'COMPLETE'
	When the user clicks the Check your answers and submit your application link
	Then the Review your answers page is displayed
	And the data presented for review matches the data entered into the Exporter Portal for a phytosanitary certificate application
	When the user clicks the Continue button on the Review your answers page
	Then the When do you need the certificate? page is displayed
	When the user enters a valid date for when the certificate is needed at least 4 days in the future 
	And the user clicks the Continue button on the When do you need the certificate? page
	Then the Exporter declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	Then the Application complete page is displayed
	And the user records the APHA reference number for 'Phytosanitary Certificate Application'
	# ---------- Dynamics Portal - Process the phytosanitary certificate export application ----------
	When I switch back to the Dynamics window
	And I click on 'Export Applications' under the 'Exports' area
	And I click the Discard changes button on the Unsaved changes popup 
	Then the 'Active Export Applications' view is displayed on the Export Applications page
	When I search Export Applications for the 'Phytosanitary Certificate Application' created in the Exporter Portal
	Then the export application submitted in the Exporter Portal should be returned
	When I open the record in the grid
	Then the Export Application page is displayed for the 'Phytosanitary Certificate Application' created in the Exporter Portal	
	And the 'General' tab is displayed and selected
	And the Status of the Export Application should be 'Active'
	And the Status Reason of the Export Application should be 'Submitted'
	And the Commodity Group should be 'Seeds'
	And the Application Method should be 'Online'
	And the Application Type should be 'Export Application'
	And the 'Stock Requests' tab is displayed
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	And I can see 1 row in the Stock Requests subgrid
	When I select the 'General' tab
	Then the 'General' tab is displayed and selected
	When I click the Work Order link in the Application Details section
	Then the Work Order page is displayed for the 'Phytosanitary Certificate Application' created in the Exporter Portal
	And the 'Export' tab is displayed and selected
	And the 'Stock Requests' tab is displayed
	And the Application Type should be 'Export Application' in the General section
	And the Commodity Group should be 'Seeds' in the General section
	And the Substatus of the Work Order should be 'Unassigned'
	And the Owner of the Work Order should be 'CIT Triage Team'
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button
	Then the Substatus of the Work Order should be 'Assigned'
	And the Owner of the Work Order should be me
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	And I can see 1 row in the Stock Requests subgrid
	And the Status Reason of the Stock Request in the Stock Requests subgrid should be 'Pending'
	When I select the record for the stock reference created in the Exporter Portal
	Then the Accept Request button should be displayed
	When I click the Accept Request button
	Then the Status Reason of the Stock Request in the Stock Requests subgrid should be 'Accepted'
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the 'Summary' tab is displayed and selected
	And the 4 Stock Draw Down Quantities fields are displayed to three decimal places
	When I click the Back button in the command bar
	Then the 'Stock Requests' tab is displayed and selected
	When I select the 'Phyto Commodities' tab
	Then the 'Phyto Commodities' tab is displayed and selected
	And I can see 1 row in the Commodity Lines subgrid
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Issue Phyto Task'
	When I click on the 'Pre Inspection Task' task
	And I maximise the popup
	Then I verify the 'Pre Inspection Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
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
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
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
	And the 'Issue Phyto Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	And the Substatus of the Work Order should be 'Certificate Issued'
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	And I cannot see any row in the Stock Requests subgrid
	When I change the view to 'All Stock Requests' in the Stock Requests subgrid
	Then the 'All Stock Requests' view is displayed in the Stock Requests subgrid
	And I can see 1 row in the Stock Requests subgrid
	When I open the record for the stock reference created in the Exporter Portal
	Then the Stock Request page is displayed
	And the 'General' tab is displayed and selected
	And the Status of the Stock Request should be 'Inactive'
	And the Status Reason of the Stock Request should be 'Certificate Issued'
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the 'Phytosanitary Certificate Application' created in the Exporter Portal
	And the 'Stock Requests' tab is displayed and selected
	Then the 'Active Stock Requests' view is displayed in the Stock Requests subgrid
	When I change the view to 'All Stock Requests' in the Stock Requests subgrid
	Then the 'All Stock Requests' view is displayed in the Stock Requests subgrid
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the 'Summary' tab is displayed and selected
	And the Total Draw Down quantity is '7,000.000'
	And the Total Draw Down Including Pending quantity is '7,000.000'
	And the Remaining Approved Stock quantity is '3,000.000'
	And the Remaining Approved Stock Including Pending quantity is '3,000.000'
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the 'Phytosanitary Certificate Application' created in the Exporter Portal
	And the 'Stock Requests' tab is displayed and selected
	When I select the 'Export' tab
	Then the 'Export' tab is displayed and selected
	When I click the Export Application link in the Application & Inspection Address section
	Then the Export Application page is displayed for the 'Phytosanitary Certificate Application' created in the Exporter Portal
	And the Status of the Export Application should be 'Inactive'
	And the Status Reason of the Export Application should be 'Completed'
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the 'Phytosanitary Certificate Application' created in the Exporter Portal
	And the 'Export' tab is displayed and selected
	When I click View Phyto command from the header ribbon
	Then the Phytosanitary certificate for 'Phytosanitary Certificate Application' is displayed in a new window
	When I download the Phytosanitary certificate PDF for 'Phytosanitary Certificate Application'
	Then the Phytosanitary Certificate displays the details of 'Phytosanitary Certificate Application' submitted
	When I close the PDF window
	Then the window is closed
	# ---------- Exporter Portal - Search for the Phytosanitary certificate ----------
	When I switch to the Exporter Portal window
	And I navigate to the Exporter Portal	
	Then the Exporter Portal homepage should be displayed
	When the user enters the recorded APHA reference for 'Seed Sampling Application' in the search box
	Then the Export Application portal search returns one matching record
	When the user clicks the APHA reference link for 'Seed Sampling Application'
	Then the Manage application page should be displayed
	When the user clicks the Sampling report (csv) link on the Manage application page
	Then the Sampling report (csv) file is downloaded
	And the Remaining Approved Stock quantity in the CSV is '3000.0'

@SPS-3951
Scenario: Exporter phytosanitary certificate application creating stock, followed by draw down applications - SPS-3951
	# ---------- Exporter Portal - Application 1 (qualifying application) ----------
	Given that I navigate to the Exporter Portal
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should be redirected to the Sign in using Government Gateway page
	When I have provided the Exporter Trader credentials and signin
	Then the Exporter Portal homepage should be displayed
	When the user clicks the Start a new application button
	Then the What are you exporting? page is displayed
	When the user selects 'Plants and fresh produce' and clicks the Continue button on the What are you exporting? page
	Then the What do you need to do? page is displayed
	When the user selects 'Apply for a phytosanitary certificate' and clicks the Continue button on the What do you need to do? page
	Then the Destination country page is displayed
	When the user searches for destination country 'France' and clicks the Continue button
	Then the Regulations for exporting 'plants and fresh produce' to 'France' page is displayed
	When the user ticks the exporting regulations checkbox
	And the user clicks the Continue button on the Regulations for exporting page
	Then the Create a reference page is displayed
	When the user enters a reference number and clicks the Save and continue button
	Then the Your application page is displayed
	When the user clicks the What is the exporter address? link
	Then the Is this the exporter address? page is displayed
	When the user selects 'Yes' and clicks the Save and continue button on the Is this the exporter address? page
	Then the Your application page is displayed
	When the user clicks the What's in your consignment? link
	Then the How do you want to add commodities to your consignment? page is displayed
	When the user selects 'Manually, by adding one at a time' and clicks the Continue button on the How do you want to add commodities to your consignment? page
	Then the Add 'plants and fresh produce' to your consignment page is displayed
	When the user describes the commodity with botanical name 'LAVVI', commodity type 'Plants', quantity '10', unit of measurement 'Kilograms', number of packages '10', type of packaging 'Box', countries of origin 'United Kingdom'
	And the user clicks the Save and continue button on the Add 'plants and fresh produce' to your consignment page
	Then the Your commodities page is displayed with the commodity line just entered
	When the user selects 'No, I have finished adding commodities' and clicks the Save and continue button on the Your commodities page
	Then the Your application page is displayed
	When the user clicks the What are the inspection details? link
	Then the Do you want to apply under the plant health exports audited trader scheme (PHEATS)? page is displayed
	When the user selects 'No' and clicks the Save and continue button on the Do you want to apply under the plant health exports audited trader scheme (PHEATS)? page
	Then the What are the inspection details? page is displayed
	When the user selects an inspection address and continues
	Then the Do you want to select this inspection address? page is displayed
	When the user selects 'Yes, select this inspection address' and clicks the Continue button on the Do you want to select this inspection address? page
	Then the Who is the contact for this application? page is displayed
	When the user enters a contact name for the inspector to contact about the application
	And the user enters a phone number for the inspector to contact about the application
	And the user clicks the Save and continue button on the Who is the contact for this application? page
	Then the When will the consignment be ready? page is displayed
	When the user enters a valid date for when the consignment will be ready for inspection at least 5 days in the future
	And the user clicks the Save and continue button on the When will the consignment be ready? page
	Then the Your application page is displayed
	When the user clicks the Who are you sending this consignment to? link
	Then the Who are you sending this consignment to? page is displayed
	When the user enters the conisgnee details and clicks the Save and continue button on the Who are you sending this consignment to? page
	Then the Your application page is displayed
	When the user clicks the How will this consignment be transported? link
	Then the How will this consignment be transported? page is displayed
	When the user enters the value of the consignment in pounds 2000.00
	And the user selects 'Yes' for Do you know the point of entry?
	And the user enters the point of entry 'FR CQF'
	And the user selects a transport method 'Road or ferry'
	And the user clicks the Save and continue button on the How will this consignment be transported? page
	Then the Your application page is displayed
	When the user clicks the Supporting documents link
	Then the Supporting documents page is displayed
	When the user uploads a supporting document and clicks the Save and continue button
	Then the Your application page is displayed
	When the user clicks the Check your answers and submit your application link
	Then the Review your answers page is displayed
	And the data presented for review matches the data entered into the Exporter Portal for a phytosanitary certificate application
	When the user clicks the Continue button on the Review your answers page
	Then the When do you need the certificate? page is displayed
	When the user enters a valid date for when the certificate is needed at least 5 days in the future
	And the user clicks the Continue button on the When do you need the certificate? page
	Then the Exporter declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	Then the You have successfully submitted your application for a phytosanitary certificate page is displayed
	And the user records the APHA reference number for 'Application 1'
	# ---------- Dynamics Portal - Process Application 1 and create stock ----------
	When I am logged in to the 'IDCOMS' app as 'Inspector'
	And I click on 'Export Applications' under the 'Exports' area
	Then the 'Active Export Applications' view is displayed on the Export Applications page
	When I search Export Applications for the 'Application 1' created in the Exporter Portal
	Then the export application submitted in the Exporter Portal should be returned
	When I open the record in the grid
	Then the Export Application page is displayed for the 'Application 1' created in the Exporter Portal
	And the 'General' tab is displayed and selected
	When I click the Work Order link in the Application Details section
	Then the Work Order page is displayed for the 'Application 1' created in the Exporter Portal
	And the 'Export' tab is displayed and selected
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button
	Then the Substatus of the Work Order should be 'Assigned'
	And the Owner of the Work Order should be me
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	When I click on the 'Plants Inspection Task' task
	And I maximise the popup
	Then I verify the 'Plants Inspection Task' popup is displayed
	And the 'Summary' tab is displayed and selected
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
	When I click the Save button on the Work Order Task page
	And I select the 'Summary' tab
	Then the 'Summary' tab is displayed and selected
	And the Status of the Work Order should be 'Scheduled'
	When I select the 'Inspection Results' tab
	Then the 'Inspection Results' tab is displayed and selected
	When I scroll down to the Inspection Results grid on the Work Order Task page
	Then I can see 1 row in the Inspection Results grid	
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
	When I enter 5 in the 'Travel' column
	And I enter 5 in the 'Inspection' column
	And I enter 5 in the 'Admin' column
	And I enter 5 in the 'Training given (Client)' column
	And I enter 5 in the 'Training given (Inspector)' column
	And I enter 5 in the 'Training received' column
	And I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then the 'Work Order Tasks' tab is displayed and selected
	When I select the 'Export' tab
	Then the 'Export' tab is displayed and selected
	When I select a row in the Commodity Lines subgrid
	And I click the Create Stock button
	Then the Quick Create Stock form is displayed
	When I enter the Site / Batch ID to the Quick Create form
	And I enter the Approved Stock 100 on the Quick Create form
	And I click the Save and Close button on the Quick Create form
	Then the Quick Create Visit form is closed
	And the Stock record created confirmation is displayed
	And the 'Export' tab is displayed and selected
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	When I click on the 'Issue Phyto Task' task
	And I maximise the popup
	Then I verify the 'Issue Phyto Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	When I enter 5 in the 'Admin' column
	And I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then the Substatus of the Work Order should be 'Certificate Issued'
	When I click View Phyto command from the header ribbon
	Then the Phytosanitary certificate for 'Application 1' is displayed in a new window
	When I download the Phytosanitary certificate PDF for 'Application 1'
	Then the Phytosanitary Certificate displays the details of 'Application 1' submitted
	When I close the PDF window
	Then the window is closed
	When I select the 'Export' tab
	Then the 'Export' tab is displayed and selected
	When I click the Export Application link in the Application & Inspection Address section	
	Then the Export Application page is displayed for the 'Application 1' created in the Exporter Portal	
	When I select the 'Related' tab
	And I select the 'Stocks' tab from the Related tab dropdown
	Then the 'Stocks' tab is displayed and selected
	And I can see 1 record displayed within the Stock subgrid
	And I make a note of the stock name
	# ---------- Exporter Portal - Application 2 (draw down from stock) ----------
	When I switch to the Exporter Portal window
	And the user clicks the View your applications button
	Then the Your applications page is displayed
	When the user enters the recorded APHA reference for 'Application 1' in the search box
	Then the Export Application portal search returns one matching record
	When the user clicks the APHA reference link for 'Application 1'
	Then the Manage application page should be displayed
	And the 'Draw down stock is available' message is displayed on the Manage application page
	When the user clicks the 'Draw down or view stock' link on the Manage application page
	Then the Stock details page is displayed
	And the stock request name is displayed on the Stock details page
	And the stock details show the commodity description, variety, location, inspected on, valid until, quantity inspected and amount drawn down of 0
	When the user clicks the Draw down button
	Then the Do you want to reuse the same application reference? page is displayed
	When the user selects 'No, a different reference' and enters a new reference
	And the user clicks the Save and continue button on the Do you want to reuse the same application reference? page
	Then the How much stock are you exporting? page is displayed
	And the Quantity, Inspected on date and Valid until date are displayed on the How much stock are you exporting? page
	When the user enters the draw down quantity 20 on the How much stock are you exporting? page 
	And the user clicks the Save and continue button on the How much stock are you exporting? page
	Then the Upload supporting photographs page is displayed
	When the user uploads a JPEG supporting photograph with a file description
	Then the photograph is added to the supporting photographs table with the Download and Delete actions
	When the user clicks the Save and continue button on the Upload supporting photographs page
	Then the Make changes to your application page is displayed
	When the user clicks the Change link next to the 'Destination' field on the Make changes to your application page
	Then the Destination country page is displayed
	When the user searches for destination country 'Germany' and clicks the Save and continue button
	Then the Review your answers page is displayed
	When the user clicks the Continue button on the Review your answers page
	Then the When do you need the certificate? page is displayed
	When the user enters a valid date for when the certificate is needed at least 5 days in the future
	And the user clicks the Continue button on the When do you need the certificate? page
	Then the Exporter declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	Then the You have successfully submitted your application for a phytosanitary certificate page is displayed
	And the user records the APHA reference number for 'Application 2'
	# ---------- Dynamics Portal - Process Application 2 ----------
	When I switch back to the Dynamics window
	And I click on 'Export Applications' under the 'Exports' area
	Then the 'Active Export Applications' view is displayed on the Export Applications page
	When I search Export Applications for the 'Application 2' created in the Exporter Portal
	Then the export application submitted in the Exporter Portal should be returned
	When I open the record in the grid
	Then the Export Application page is displayed for the 'Application 2' created in the Exporter Portal
	And the 'General' tab is displayed and selected
	When I click the Work Order link in the Application Details section
	Then the Work Order page is displayed for the 'Application 2' created in the Exporter Portal
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button
	Then the Substatus of the Work Order should be 'Assigned'
	And the Owner of the Work Order should be me
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	And the 'Active Stock Requests' view is displayed in the Stock Requests subgrid
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the 'Summary' tab is displayed and selected
	And the Total Draw Down quantity is '0.000'
	And the Total Draw Down Including Pending quantity is '20.000'
	And the Remaining Approved Stock quantity is '100.000'
	And the Remaining Approved Stock Including Pending quantity is '80.000'
	When I click the Back button in the command bar
	Then the 'Stock Requests' tab is displayed and selected
	And the 'Active Stock Requests' view is displayed in the Stock Requests subgrid
	And the Status Reason of the Stock Request in the Stock Requests subgrid should be 'Pending'
	When I select the record for the stock reference created in the Exporter Portal
	Then the Accept Request button should be displayed
	When I click the Accept Request button
	Then the Status Reason of the Stock Request in the Stock Requests subgrid should be 'Accepted'
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the Total Draw Down quantity is '20.000'
	And the Total Draw Down Including Pending quantity is '20.000'
	And the Remaining Approved Stock quantity is '80.000'
	And the Remaining Approved Stock Including Pending quantity is '80.000'
	When I click the Back button in the command bar
	Then the 'Stock Requests' tab is displayed and selected
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Issue Phyto Task'
	And the 'Plants Inspection Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	When I change the view to 'All Work Order Service Tasks' in the Work Order Tasks subgrid
	Then I can see following Work Order Tasks 'Pre Inspection Task' 'Plants Inspection Task' 'Issue Phyto Task'
	And the Work Order Task 'Plants Inspection Task' Status is 'Inactive'
	When I click on the 'Issue Phyto Task' task
	And I maximise the popup
	Then I verify the 'Issue Phyto Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	When I enter 5 in the 'Admin' column
	And I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then the Substatus of the Work Order should be 'Certificate Issued'
	When I click View Phyto command from the header ribbon
	Then the Phytosanitary certificate for 'Application 2' is displayed in a new window
	When I download the Phytosanitary certificate PDF for 'Application 2'
	Then the Phytosanitary Certificate displays the details of 'Application 2' submitted
	When I close the PDF window
	Then the window is closed
	# ---------- Exporter Portal - Application 3 (copy of Application 2) ----------
	When I switch to the Exporter Portal window
	And the user clicks the View your applications button
	Then the Your applications page is displayed
	When the user enters the recorded APHA reference for 'Application 2' in the search box
	Then the Export Application portal search returns one matching record
	When the user clicks the APHA reference link for 'Application 2'
	Then the Manage application page should be displayed
	And the 'This application has been drawn down from draw down stock' message is displayed on the Manage application page
	When the user clicks the Copy as new application link on the Manage application page
	Then the Are you sure you want to copy this application? page is displayed
	When the user selects 'Yes, copy this application' and clicks the Continue button on the Are you sure you want to copy this application? page
	Then the Do you want to reuse the same application reference? page is displayed
	When the user selects 'No, a different reference' and enters a new reference
	And the user clicks the Save and continue button on the Do you want to reuse the same application reference? page
	Then the Make changes to your application page is displayed
	When the user clicks the Change link next to the 'Destination' field on the Make changes to your application page
	Then the Destination country page is displayed
	When the user searches for destination country 'Spain' and clicks the Save and continue button
	Then the Review your answers page is displayed
	When the user clicks the Change link next to the 'File name' field on the Review your answers page
	Then the Upload supporting photographs page is displayed
	When the user uploads a JPEG supporting photograph with a file description
	Then the photograph is added to the supporting photographs table with the Download and Delete actions
	When the user clicks the Save and continue button on the Upload supporting photographs page
	Then the Review your answers page is displayed
	When the user clicks the Continue button on the Review your answers page
	Then the When do you need the certificate? page is displayed
	When the user enters a valid date for when the certificate is needed at least 5 days in the future
	And the user clicks the Continue button on the When do you need the certificate? page
	Then the Exporter declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	Then the You have successfully submitted your application for a phytosanitary certificate page is displayed
	And the user records the APHA reference number for 'Application 3'
	# ---------- Dynamics Portal - Process Application 3 ----------
	When I switch back to the Dynamics window
	And I click on 'Export Applications' under the 'Exports' area
	Then the 'Active Export Applications' view is displayed on the Export Applications page
	When I search Export Applications for the 'Application 3' created in the Exporter Portal
	Then the export application submitted in the Exporter Portal should be returned
	When I open the record in the grid
	Then the Export Application page is displayed for the 'Application 3' created in the Exporter Portal
	When I click the Work Order link in the Application Details section
	Then the Work Order page is displayed for the 'Application 3' created in the Exporter Portal
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button
	Then the Substatus of the Work Order should be 'Assigned'
	And the Owner of the Work Order should be me
	When I select the 'Stock Requests' tab
	Then the 'Stock Requests' tab is displayed and selected
	And the 'Active Stock Requests' view is displayed in the Stock Requests subgrid
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the 'Summary' tab is displayed and selected
	And the Total Draw Down quantity is '20.000'
	And the Total Draw Down Including Pending quantity is '40.000'
	And the Remaining Approved Stock quantity is '80.000'
	And the Remaining Approved Stock Including Pending quantity is '60.000'
	When I click the Back button in the command bar
	Then the 'Stock Requests' tab is displayed and selected
	And the 'Active Stock Requests' view is displayed in the Stock Requests subgrid
	And the Status Reason of the Stock Request in the Stock Requests subgrid should be 'Pending'
	When I select the record for the stock reference created in the Exporter Portal
	Then the Accept Request button should be displayed
	When I click the Accept Request button
	Then the Status Reason of the Stock Request in the Stock Requests subgrid should be 'Accepted'
	When I click the link in the 'Stock' column
	Then the Stock page is displayed for the stock reference created in the Exporter Portal
	And the Total Draw Down quantity is '40.000'
	And the Total Draw Down Including Pending quantity is '40.000'
	And the Remaining Approved Stock quantity is '60.000'
	And the Remaining Approved Stock Including Pending quantity is '60.000'
	When I click the Back button in the command bar
	Then the 'Stock Requests' tab is displayed and selected
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Issue Phyto Task'
	And the 'Plants Inspection Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	When I change the view to 'All Work Order Service Tasks' in the Work Order Tasks subgrid
	Then I can see following Work Order Tasks 'Pre Inspection Task' 'Plants Inspection Task' 'Issue Phyto Task'
	And the Work Order Task 'Plants Inspection Task' Status is 'Inactive'
	When I click on the 'Issue Phyto Task' task
	And I maximise the popup
	Then I verify the 'Issue Phyto Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	When I enter 5 in the 'Admin' column
	And I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then the Substatus of the Work Order should be 'Certificate Issued'
	When I click View Phyto command from the header ribbon
	Then the Phytosanitary certificate for 'Application 3' is displayed in a new window
	When I download the Phytosanitary certificate PDF for 'Application 3'
	Then the Phytosanitary Certificate displays the details of 'Application 3' submitted
	When I close the PDF window
	Then the window is closed

@SPS-3991
Scenario: Exporter phytosanitary certificate application for potatoes, inspection and certificate verification - SPS-3991
	# ---------- Exporter Portal - Apply for a phytosanitary certificate for potatoes ----------
	Given that I navigate to the Exporter Portal
	Then I should see type of Gateway login page
	And I have selected 'Sign in with Government Gateway' as login type
	When I click Continue button from How do you want to sign in page
	Then I should be redirected to the Sign in using Government Gateway page
	When I have provided the Exporter Trader credentials and signin
	Then the Exporter Portal homepage should be displayed
	When the user clicks the Start a new application button
	Then the What are you exporting? page is displayed
	When the user selects 'Potatoes' and clicks the Continue button on the What are you exporting? page
	Then the What do you need to do? page is displayed
	When the user selects 'Apply for a phytosanitary certificate' and clicks the Continue button on the What do you need to do? page
	Then the Destination country page is displayed
	When the user searches for destination country 'France' and clicks the Continue button
	Then the Regulations for exporting 'potatoes' to 'France' page is displayed
	When the user ticks the exporting regulations checkbox
	And the user clicks the Continue button on the Regulations for exporting page
	Then the Create a reference page is displayed
	When the user enters a reference number and clicks the Save and continue button
	Then the Your application page is displayed
	When the user clicks the What is the exporter address? link
	Then the Is this the exporter address? page is displayed
	When the user selects 'Yes' and clicks the Save and continue button on the Is this the exporter address? page
	Then the Your application page is displayed
	When the user clicks the What's in your consignment? link
	Then the How do you want to add commodities to your consignment? page is displayed
	When the user selects 'Manually, by adding one at a time' and clicks the Continue button on the How do you want to add commodities to your consignment? page
	Then the Add 'potatoes' to your consignment page is displayed
	And the 'Ware' and 'Seed' radio buttons are displayed under Type of potato
	And the 'Variety' and 'Other' radio buttons are displayed under What variety are your potatoes?
	And the 'Unit of measurement' dropdown is displayed under Unit of measurement
	And the following textbox fields are displayed on the Add 'potatoes' to your consignment page
		| Field                              |
		| Growing season inspection number   |
		| Chemical used on tubers (optional) |
		| Quantity                           |
		| Number of packages                 |
		| Type of packaging                  |
		| Packing material (optional)        |
		| Distinguishing marks (optional)    |
		| Countries of origin                |
	When the user describes the 'Seed' potato with stock number 'SPS3991-STOCK', variety 'King Edward', growing season inspection number 'SPS3991-GSI', chemical used on tubers 'Fungicide', quantity '100', unit of measurement 'Kilograms', number of packages '10', type of packaging 'Box', packing material 'Paper', countries of origin 'United Kingdom'
	Then the potato commodity details are displayed with type of potato 'Seed', stock number 'SPS3991-STOCK', variety 'King Edward', growing season inspection number 'SPS3991-GSI', chemical used on tubers 'Fungicide', quantity '100', unit of measurement 'Kilograms', number of packages '10', type of packaging 'Box', packing material 'Paper', countries of origin 'United Kingdom'
	When the user clicks the Save and continue button on the Add 'potatoes' to your consignment page
	Then the Your commodities page is displayed
	And the commodity table is displayed for applying for a phytosanitary certificate to export potatoes
	When the user selects 'No, I have finished adding commodities' and clicks the Save and continue button on the Your commodities page
	Then the Your application page is displayed
	When the user clicks the What are the inspection details? link
	Then the What are the inspection details? page is displayed
	When the user selects an inspection address and continues
	Then the Do you want to select this inspection address? page is displayed
	When the user selects 'Yes, select this inspection address' and clicks the Continue button on the Do you want to select this inspection address? page
	Then the Who is the contact for this application? page is displayed
	When the user enters a contact name for the inspector to contact about the application
	And the user enters a phone number for the inspector to contact about the application
	And the user clicks the Save and continue button on the Who is the contact for this application? page
	Then the When will the consignment be ready? page is displayed
	When the user enters a valid date for when the consignment will be ready for inspection at least 5 days in the future
	And the user enters a valid time for when the consignment will be ready for inspection at least 2 hours in the future
	And the user clicks the Save and continue button on the When will the consignment be ready? page
	Then the Your application page is displayed
	When the user clicks the Who are you sending this consignment to? link
	Then the Who are you sending this consignment to? page is displayed
	When the user enters the conisgnee details and clicks the Save and continue button on the Who are you sending this consignment to? page
	Then the Your application page is displayed
	When the user clicks the How will this consignment be transported? link
	Then the How will this consignment be transported? page is displayed
	When the user enters the value of the consignment in pounds 2000.00
	And the user selects 'Yes' for Do you know the point of entry?
	And the user enters the point of entry 'FR CQF'
	And the user selects a transport method 'Road or ferry'
	And the user enters container number 'SPS3991-CONTAINER' on the How will this consignment be transported? page
	And the user clicks the Save and continue button on the How will this consignment be transported? page
	Then the Your application page is displayed
	When the user clicks the Supporting documents link
	When the user uploads a supporting document and clicks the Save and continue button
	Then the Your application page is displayed
	When the user clicks the Check your answers and submit your application link
	Then the Review your answers page is displayed
	And the data presented for review matches the data entered into the Exporter Portal for a phytosanitary certificate application
	When the user clicks the Continue button on the Review your answers page
	Then the When do you need the certificate? page is displayed
	When the user enters a valid date for when the certificate is needed at least 6 days in the future
	And the user enters a valid time for when the certificate is needed at least 2 hours in the future
	And the user clicks the Continue button on the When do you need the certificate? page
	Then the Exporter declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	Then the You have successfully submitted your application for a phytosanitary certificate page is displayed
	And the user records the APHA reference number for 'Potatoes Application'
	And the guidance 'We will:' is displayed on the Application complete page
	And the guidance 'arrange an inspection of your consignment within 7 working days' is displayed on the Application complete page
	And the guidance 'issue your certificate, if there are no problems' is displayed on the Application complete page
	And the guidance 'send you an invoice' is displayed on the Application complete page
	And the guidance 'You can reduce the final weight of your consignment later.' is displayed on the Application complete page
	# ---------- Dynamics Portal - Review and assign the potatoes export application ----------
	When I am logged in to the 'IDCOMS' app as 'Inspector'
	And I click on 'Export Applications' under the 'Exports' area
	Then the 'Active Export Applications' view is displayed on the Export Applications page
	When I search Export Applications for the 'Potatoes Application' created in the Exporter Portal
	Then the export application submitted in the Exporter Portal should be returned
	When I open the record in the grid
	Then the Export Application page is displayed for the 'Potatoes Application' created in the Exporter Portal
	And the 'General' tab is displayed and selected
	And the Status of the Export Application should be 'Active'
	And the Status Reason of the Export Application should be 'Submitted'
	And the Commodity Group should be 'Potatoes'
	When I click the Work Order link in the Application Details section
	Then the Work Order page is displayed for the 'Potatoes Application' created in the Exporter Portal
	And the 'Export' tab is displayed and selected
	And the Substatus of the Work Order should be 'Unassigned'
	And the Application Type should be 'Export Application' in the General section
	And the Commodity Group should be 'Potatoes' in the General section
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button
	Then the Substatus of the Work Order should be 'Assigned'
	And the Owner of the Work Order should be me
	And I can see 1 row in the Commodity Lines subgrid
	# @agent: New step in: WorkOrderSteps
	# data-id="dataSetRoot_Commoditylines"
	# Use HTML file: HTML for 'And I can see the following columns in the Commodity Lines subgrid' step.txt
	And I can see the following columns in the Commodity Lines subgrid 'Potato Type' 'Variety' 'Quantity Exported' 'Lot Reference' 'Soil Sampling/Stock Number' 'GSI Number' 'Stock Number' 'Chemicals Used'
	# @agent: New step in: WorkOrderSteps
	# reference step: [When(@"I double click on a Commodity Line with Regulatory Authority set to '(.*)'")]
	When I double click on a commodity line in the Commodity Lines subgrid
	# @agent: New step in new step file: CommodityLineSteps
	# <span data-id="entity_name_span" class="pa-er pa-ru pa-rv pa-rw pa-br pa-nk pa-j pa-cr pa-jy ">Commodity Line</span>
	Then the Commodity Line page is displayed
	And the 'Potatoes' tab is displayed and selected
	# @agent: New step in new step file: CommodityLineSteps 
	# Use HTML file: HTML for new Commodity Line page.txt
	And the following fields are displayed on the Commodity Line page 'Container number' 'Potato Type' 'Packaging Material' 'Distinguishing Marks' 'Quantity Exported' 'Total Quantity'
	# @agent: New step in new step file: CommodityLineSteps
	# Use HTML file: HTML for new Commodity Line page.txt
	And the Quantity Exported field is directly below the Total Quantity field
	# @agent: New step in new step file: CommodityLineSteps
	# Validate against scenario context
	# Use HTML file: HTML for new Commodity Line page.txt
	# data-id="trd_containernumber.fieldControl-text-box-text"
	# data-id="trd_stocknumber.fieldControl-text-box-text"
	# data-id="trd_packagingmaterialused.fieldControl-text-box-text"
	# data-id="trd_gsinumber.fieldControl-text-box-text"
	And the 'Container number' 'Stock Number' 'Packaging Material' 'GSI Number' values match those entered in the Exporter Portal for 'Potatoes Application'
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the 'Potatoes Application' created in the Exporter Portal
	And the 'Export' tab is displayed and selected
	# ---------- Dynamics Portal - Complete the pre-inspection task ----------
	When I select the 'Work Order Tasks' tab
	Then the 'Work Order Tasks' tab is displayed and selected
	And I can see following Work Order Tasks 'Pre Inspection Task' 'Potatoes Inspection Task' 'Issue Phyto Task'
	When I click on the 'Pre Inspection Task' task
	And I maximise the popup
	Then I verify the 'Pre Inspection Task' popup is displayed
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
	And the 'Pre Inspection Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	# ---------- Dynamics Portal - Inspect the potatoes and select a declaration ----------
	When I click on the 'Potatoes Inspection Task' task
	And I maximise the popup
	Then I verify the 'Potatoes Inspection Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	# @agent: New step in: WorkOrderTasksSteps
	# data-id="msdyn_tasktype.fieldControl-LookupResultsDropdown_msdyn_tasktype_selected_tag_text"
	# data-id="QV_Work_Order_Sub_Status.msdyn_substatus.fieldControl-LookupResultsDropdown_msdyn_substatus_selected_tag_text"
	# data-id="trd_dateassigned.fieldControl._datecontrol-date-container"
	# data-id="msdyn_percentcomplete.fieldControl-pcf-container-id"
	# data-id="trd_chargeid.fieldControl-Lookup_trd_chargeid"
	# data-id="statuscode.fieldControl-pcf-container-id"
	# Date Assigned format is DD/MM/YYYY should just validate against today's date
	# Charge 'Blank' value just means empty.
	And the following fields are displayed and read only on the Work Order Task page
		| Field             | Expected value           |
		| Task Type         | Potatoes Inspection Task |
		| Work Order Status | Assigned                 |
		| Date Assigned     | Work order assigned date |
		| % Complete        | 0.00                     |
		| Charge            | Blank                    |
		| Status Reason     | Active                   |
	When I select the 'Inspection Results' tab
	Then the 'Inspection Results' tab is displayed and selected
	# New step in: WorkOrderTasksSteps
	# data-id="trd_visitid.fieldControl-LookupResultsDropdown_trd_visitid_InputSearch"
	# data-id="trd_visitid.fieldControl-Lookup_trd_visitid_microsoftIcon_searchButton"
	# data-id="trd_inspectorassignedid.fieldControl-LookupResultsDropdown_trd_inspectorassignedid_SelectedRecordList"
	# data-id="trd_inspectorassignedid.fieldControl-Lookup_trd_inspectorassignedid_microsoftIcon_searchButton"
	And the 'Visit' and 'Inspector Assigned' fields are displayed and editable on the Work Order Task page
	When I scroll down to the Inspection Results grid on the Work Order Task page
	Then I can see the following columns are present in the Inspection Results grid 'Status Reason' 'Potato Type (Consignment Item)' 'Variety (Consignment Item)' 'Stock Number (Consignment Item)' 'Chemicals Used (Consignment Item)' 'Quantity Applied For' 'Quantity Inspected' 'Quantity Passed' 'Quantity Exported (Consignment Item)' 'Inspection Result' 'Reason' 'Lab Sample' 'Task Type'
	When I select the Visit field on the Work Order Task page
	And I select the New button on the Visits dropdown
	Then the Quick Create Visit form is displayed
	When I add my name to the Quick Create form
	And I add a valid date scheduled to the Quick Create form at least 2 days in the future
	And I add a valid time scheduled to the Quick Create form
	And I click the Save and Close button on the Quick Create form
	Then the Quick Create Visit form is closed
	And the 'Inspection Results' tab is displayed and selected
	When I click the Save button on the Work Order Task page
	And I scroll down to the Inspection Results grid on the Work Order Task page
	Then I can see 1 row in the Inspection Results grid
	# @agent: New step in: WorkOrderTasksSteps
	# data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_potatotype"
	# data-id="trd_quantityappliedfor"
	# data-id="trd_quantityinspected"
	# data-id="trd_quantitypassed"
	# data-id="a_f4855dca5a0deb11a813000d3a2c32cb.trd_finalweight"
	# data-id="trd_fminspectionresult"
	And the Potato Type displayed in the Inspection Results grid matches the Exporter Portal 'Potatoes Application'
	# @agent: New step in: WorkOrderTasksSteps
	# Note that the Quantity in dynamics is displayed to 3 decimal places, while the Exporter Portal displays to 2 decimal places. The values are considered equal if they are equal when rounded to 2 decimal places.	
	And the Quantity Applied For in the Inspection Results grid matches the Exporter Portal 'Potatoes Application'
	# @agent: New step in: WorkOrderTasksSteps
	And the 'Quantity Inspected' 'Quantity Passed' 'Quantity Exported' 'Inspection Result' fields are blank in the Inspection Results grid
	# @agent: New step in: WorkOrderTasksSteps
	When I enter the Quantity Applied For into the Quantity Inspected field in the Inspection Results grid
	# @agent: New step in: WorkOrderTasksSteps
	Then the Quantity Inspected matches the Quantity Applied For
	# @agent: New step in: WorkOrderTasksSteps
	When I enter the Quantity Applied For into the Quantity Passed field in the Inspection Results grid
	# @agent: New step in: WorkOrderTasksSteps
	Then the Quantity Passed matches the Quantity Applied For
	# @agent: New step in: WorkOrderTasksSteps
	And the Quantity Exported matches the Quantity Passed
	And the Inspection Result is 'Pass'
	When I select the row in the Inspection Results grid
	And I click the Submit Results button above the Inspection Results grid
	Then the Status Reason is 'Submitted'
	When I select the 'Phyto Commodities' tab
	Then the 'Phyto Commodities' tab is displayed and selected
	And I can see 1 row in the Commodity Lines subgrid
	# @agent: New step in: WorkOrderTasksSteps
	# data-id="dataSetRoot_subgrid_selected_commodities"
	# Use HTML in: HTML For Selected Commodity Lines subgrid.txt
	And the following columns are displayed and read only in the Selected Commodity Lines subgrid 'Include on Phyto' 'Potato Type (Commodity Item)' 'Variety (Commodity Item)' 'Soil Sampling/Stock Number (Commodity Item)' 'Lot Reference (Commodity Item)' 'Stock Number (Commodity Item)'
	When I select the 'Declarations' tab
	Then the 'Declarations' tab is displayed and selected
	# @agent: New step in: WorkOrderTasksSteps
	# data-id="dataSetRoot_declarations_subgrid"
	# Use HTML in: HTML for Declarations subgrid.txt
	And the following columns are displayed in the Declarations subgrid 'Declaration ID' 'Declaration Short Name' 'Declaration Description' 'Country Name' 'TAP Approved' 'Created On'
	# @agent: New step in: WorkOrderTasksSteps
	# store the Declaration Short Name in scenario context too col-id="trd_shortname"
	# store the Declaration Description in scenario context too col-id="trd_declaration"
	When I randomly select a declaration from the Declarations subgrid
	# @agent: New step in: WorkOrderTasksSteps
	# col-id="trd_declaration"
	And I record the selected Declaration Description for 'Potatoes Application'
	# @agent: New step in: WorkOrderTasksSteps
	# data-id="trd_declaration|NoRelationship|SubGridStandard|trd.trd_declaration.Button.AddDeclarations"
	And I click the Add Declarations button
	# @agent: New step in: WorkOrderTasksSteps
	# data-id="dataSetRoot_SelectedDeclarations_grid"
	# Validate short name appears under column with header data-id="trd_name"
	# Validate the description under column with header data-id="trd_declaration"
	# Declaration Description cell value is <div tabindex="0" role="gridcell" aria-selected="true" aria-required="false" class="wj-cell wj-state-selected wj-state-active" data-id="cell-0-3" id="Grid2b707bb4-b286-a6d6-8735-001e70326183-id-cell-0-3" aria-colindex="3" title="The consignment complies with Commission Implementing Decision 2012/270, item (a) the potato tubers have been grown in a pest-free area established by the national plant protection organisation in accordance with relevant international standards for phytosanitary measures; the potato tubers have been found free from the specified organisms concerned and from the signs of infestation by those organisms on potato tubers, and do not contain more than 0,1 % of soil in an official examination carried out immediately prior to export; the packaging material in which potato tubers are imported is clean. [state area in â€œplace of origin]"
	# Use HTML in: HTML for Selected Declaration subgrid.txt
	Then the selected declaration is displayed in the Selected Declarations subgrid
	# @agent: New step in: WorkOrderTasksSteps
	And the following columns are displayed in the Selected Declarations subgrid 'Declaration Short Name' 'Declaration Description' 'Country Name' 'TAP Approved' 'Include on Phyto' 'Modified On'
	When I select the 'Summary' tab
	Then the 'Summary' tab is displayed and selected
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	# @agent: New step in: WorkOrderTasksSteps
	# Date format is DD/MM/YYYY
	# Column header data-id="trd_date"
	# <div tabindex="-1" role="gridcell" aria-selected="false" aria-required="false" class="wj-cell" data-id="cell-0-3" id="Gridc107d03e-eed5-4a55-8c8e-734ef1f83229-id-cell-0-3" aria-colindex="3" title="28/10/2026"
	And the date is today's date
	# @agent: New step in: WorkOrderTasksSteps
	# Use HTML in: HTML for Time Recording subgrid.txt
	And the following columns are displayed in the Time Recording grid 'Travel' 'Inspection' 'Admin' 'Training given (Client)' 'Training given (Inspector)' 'Training received'
	When I enter 5 in the 'Travel' column
	And I enter 5 in the 'Inspection' column
	And I enter 5 in the 'Admin' column
	And I enter 5 in the 'Training given (Client)' column
	And I enter 5 in the 'Training given (Inspector)' column
	And I enter 5 in the 'Training received' column
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
	And the 'Potatoes Inspection Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	# ---------- Dynamics Portal - Issue the phytosanitary certificate ----------
	When I click on the 'Issue Phyto Task' task
	And I maximise the popup
	Then I verify the 'Issue Phyto Task' popup is displayed
	And the 'Summary' tab is displayed and selected
	And the following fields are displayed and read only on the Work Order Task page
		| Field             | Expected value           |
		| Task Type         | Issue Phyto Task         |
		| Work Order Status | Inspection Complete      |
		| Date Assigned     | Work order assigned date |
		| % Complete        | 0.00                     |
		| Charge            | Blank                    |
		| Status Reason     | Active                   |
	When I select the 'Phyto Commodities' tab
	Then the 'Phyto Commodities' tab is displayed and selected
	And I can see 1 row in the Commodity Lines subgrid
	And the following columns are displayed and read only in the Selected Commodity Lines subgrid 'Include on Phyto' 'Potato Type (Commodity Item)' 'Variety (Commodity Item)' 'Soil Sampling/Stock Number (Commodity Item)' 'Lot Reference (Commodity Item)' 'Stock Number (Commodity Item)'
	When I select the 'Declarations' tab
	Then the 'Declarations' tab is displayed and selected
	And the following columns are displayed in the Declarations subgrid 'Declaration ID' 'Declaration Short Name' 'Declaration Description' 'Country Name' 'TAP Approved' 'Created On'
	And the Declarations subgrid is filtered by the Country Name 'France'
	And the following columns are displayed in the Selected Declarations subgrid 'Declaration Short Name' 'Declaration Description' 'Country Name' 'TAP Approved' 'Include on Phyto' 'Modified On'
	When I select the 'Summary' tab
	Then the 'Summary' tab is displayed and selected
	When I select the Time Recording Inspectors field on the Work Order Task page
	And I search for my name in the Time Recording Inspectors field
	Then a new row appears in the Time Recording grid containing my name
	And the entry status is 'Draft'
	And the date is today's date
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
	And the 'Issue Phyto Task' is no longer visible in the Active Work Order Service Tasks - Exports subgrid
	And the Substatus of the Work Order should be 'Certificate Issued'
	When I select the 'Export' tab
	Then the 'Export' tab is displayed and selected
	When I click the Export Application link in the Application & Inspection Address section
	Then the Export Application page is displayed for the 'Potatoes Application' created in the Exporter Portal
	And the Status of the Export Application should be 'Inactive'
	And the Status Reason of the Export Application should be 'Completed'
	When I click the Back button in the command bar
	Then the Work Order page is displayed for the 'Potatoes Application' created in the Exporter Portal
	And the 'Export' tab is displayed and selected
	# ---------- Phytosanitary certificate - Verify the potatoes application details ----------
	When I click View Phyto command from the header ribbon
	Then the Phytosanitary certificate for 'Potatoes Application' is displayed in a new window
	When I download the Phytosanitary certificate PDF for 'Potatoes Application'
	Then the Phytosanitary Certificate displays the details of 'Potatoes Application' submitted
	When I close the PDF window
	Then the window is closed