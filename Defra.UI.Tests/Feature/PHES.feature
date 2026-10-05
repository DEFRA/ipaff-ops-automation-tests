Feature: PHES

End-to-end PHES regression Test

@PHES
Scenario: New exporter application for a phytosanitary certificate
	# Exporter Portal - New exporter application for a phytosanitary certificate
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
	Then the Add bulbs to your consignment page is displayed
	When the user describes the commodity with botanical name 'TULYA', commodity type 'Bulbs', quantity '10', unit of measurement 'Units', number of packages '10', type of packaging 'Box', countries of origin 'Netherlands'
	And the user clicks the Save and continue button on the Add bulbs to your consignment page
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
	# Dynamics Portal - Caseworker processes an application for a phytosanitary certificate
	When I am logged in to the 'IDCOMS' app as 'Inspector'
	And I click on 'Export Applications' under the 'Exports' area
	Then the 'Active Export Applications' view is displayed
	When I search Import Notifications for the notification created in IPAFFS
	Then the notification created in IPAFFS should be returned
	When I open the record in the grid
	Then I verify the Import Notification page is displayed for the notification created in IPAFFS
	And the 'Summary' tab is displayed and selected
	When I click the reference number in the Work Order field for the notification created in IPAFFS
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And the 'Import' tab is displayed and selected
	When I click the Assign command
	Then I can see the Assign Work Order popup is displayed
	When I click the Assign button
	Then the Substatus of the Work Order should be Assigned
	And the Owner of the Work Order should be me
	When I check that the Commodity Lines frame shows 'Active Import Commodity Lines'
	Then all the Commodity Lines should be validated with the values given in the input
	When I sort Commodity Lines by Regulatory Authority
	And I double click on a Commodity Line with Regulatory Authority set to 'PHSI'
	Then the Import Commodity Line page is displayed
	And the settings are displayed as HMI Inspection Required 'No', PHSI Inspection Required 'Yes' and Inspection Classification 'Mandatory / Controlled / Reduced / Not Notifiable'
	When I click the Back button in the command bar
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And the 'Import' tab is displayed and selected
	When I sort Commodity Lines by Regulatory Authority
	And I double click on a Commodity Line with Regulatory Authority set to 'HMI'
	Then the Import Commodity Line page is displayed
	And the settings are displayed as HMI Inspection Required 'Yes', PHSI Inspection Required 'No' and Inspection Classification ''
	When I click the Back button in the command bar
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And the 'Import' tab is displayed and selected
	When I sort Commodity Lines by Regulatory Authority
	And I double click on a Commodity Line with Regulatory Authority set to 'Joint'
	Then the Import Commodity Line page is displayed
	And the settings are displayed as HMI Inspection Required 'Yes / No', PHSI Inspection Required 'No' and Inspection Classification 'Mandatory / Controlled / Reduced / Not Notifiable'
	When I click the Back button in the command bar
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And the 'Import' tab is displayed and selected
	When I select the 'Work Order Tasks' tab
	Then I can see following Work Order Tasks 'HMI Check' 'Document Check' 'Imports Phyto Certificate Audit' 'Identity & Physical Check'
	When I click on the 'Document Check' task
	And I maximise the popup
	Then I verify the 'Document Check' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I click the Assign command
	Then I can see the Assign Work Order Task popup is displayed
	When I click the Assign button
	Then the Owner of the Work Order should be me
	When I click Add my time within the Time Recording section
	Then a new row appears in the grid containing my name
	And the entry status is 'Draft'
	When I enter '20' in the Admin column
	And I click the Save icon
	Then the details are saved 
	And the entry status is 'Draft'
	When I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And I can see following Work Order Tasks 'HMI Check' 'Document Check' 'Imports Phyto Certificate Audit' 'Identity & Physical Check'
	And the Work Order Task 'Document Check' Status is 'Inactive'
	And the Work Order Task 'Document Check' % Complete is '100.00'
	When I click on the 'Imports Phyto Certificate Audit' task
	And I maximise the popup
	Then I verify the 'Imports Phyto Certificate Audit' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I click the Assign command
	Then I can see the Assign Work Order Task popup is displayed
	When I click the Assign button
	Then the Owner of the Work Order should be me
	When I click Add my time within the Time Recording section
	Then a new row appears in the grid containing my name
	And the entry status is 'Draft'
	When I enter '599' in the Admin column
	And I click the Save icon
	Then the details are saved 
	And the entry status is 'Draft'
	When I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the entry status is 'Submitted'
	When I update the Audit status to 'Audited'
	And I update the Date documents received to today's date
	And I update the Documents match electronic copy? to 'Yes'
	Then the Audit status field is 'Audited'
	And the Date documents received field is today's date
	And the Documents match electronic copy? field is 'Yes'
	When I click the Save icon above the Accompanying Documents grid
	Then the Accompanying Documents grid details are saved
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And I can see following Work Order Tasks 'HMI Check' 'Document Check' 'Imports Phyto Certificate Audit' 'Identity & Physical Check'
	And the Work Order Task 'Imports Phyto Certificate Audit' Status is 'Inactive'
	And the Work Order Task 'Imports Phyto Certificate Audit' % Complete is '100.00'
	When I click on the 'Identity & Physical Check' task
	And I maximise the popup
	Then I verify the 'Identity & Physical Check' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I select the 'Samples' tab
	Then the 'Samples' tab is displayed and selected
	And the Import Commodity Lines grid is populated with the commodity lines from the notification created in IPAFFS
	And the Lab Samples grid is empty
	When I close the popup
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And the Work Order Task 'Identity & Physical Check' Status is 'Active'
	And the Work Order Task 'Identity & Physical Check' % Complete is '0.00'
	When I click on the 'Identity & Physical Check' task
	And I maximise the popup
	Then I verify the 'Identity & Physical Check' popup is displayed
	And the 'Summary' tab is displayed and selected
	When I click Add my time within the Time Recording section
	Then a new row appears in the grid containing my name
	And the entry status is 'Draft'
	When I enter '10' in the Admin column
	And I enter '120' in the Travel column
	And I enter '60' in the Inspection column
	And I click the Save icon
	Then the details are saved 
	And the entry status is 'Draft'
	When I select the new row in the grid
	And I click Submit Time
	Then I can see the Confirm Time Entries popup is displayed
	When I click the OK button
	Then the entry status is 'Submitted'
	When I click Mark Complete
	Then a grey banner is displayed 'Read-only This record’s status: Inactive'
	When I close the popup
	Then I verify the Work Order page is displayed for the notification created in IPAFFS
	And I can see following Work Order Tasks 'HMI Check' 'Document Check' 'Imports Phyto Certificate Audit' 'Identity & Physical Check'
	And the Work Order Task 'Identity & Physical Check' Status is 'Inactive'
	And the Work Order Task 'Identity & Physical Check' % Complete is '100.00'