Feature: PHES

End-to-end PHES regression Test

@PHES
Scenario: New exporter application for a phytosanitary certificate
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
	And the user records the APHA reference number for 'PHES'
