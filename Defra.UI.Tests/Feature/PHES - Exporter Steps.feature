Feature: PHES-Exporter

End-to-end PHES regression tests

@SPS-4289
Scenario: Exporter 1 - SPS-4289
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
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	Then the Your application for seed sampling page is displayed
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	And the 'Create a reference' task is marked as 'COMPLETE'
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	When the user clicks the Seeds to be sampled link
	# @agent: New step, page and interface files ExporterHowDoYouWantToAddCommoditiesToThisApplication
	Then the How do you want to add commodities to this application? page is displayed
	# @agent: New step, page and interface files ExporterHowDoYouWantToAddCommoditiesToThisApplication
	When the user selects 'All at once, by uploading a file' and clicks the Continue button on the How do you want to add commodities to this application? page
	# @agent: New step, page and interface files ExporterUploadAFile
	Then the Upload a file page is displayed
	# @agent: Use this step definition to help implement the step below: [When("the user clicks Download the CSV template and read the guide on how to complete this \\(opens in new tab)")]
	# @agent: New step, page and interface files ExporterUploadAFile
	When the user downloads the CSV template
	# @agent: New step, page and interface files ExporterUploadAFile
	Then the CSV template is downloaded
	# @agent: New step, page and interface files ExporterUploadAFile
	# @agent: Reference file for CSV validation is 'template-seed-sample.csv'
	And the only units of measurements listed in the template are 'grams' 'kilograms' 'tonnes'
	# @agent: New step, page and interface files ExporterUploadAFile
	When the user clicks the Back link on the Upload a file page
	Then the How do you want to add commodities to this application? page is displayed
	When the user selects 'Manually, by adding one at a time' and clicks the Continue button on the How do you want to add commodities to this application? page
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
	Then the Add seeds to your consignment page is displayed
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
 	When the user describes the seed commodity with botanical name 'FLUTI', variety 'special', lot reference '12345', quantity '10000', unit of measurement 'Grams', chemically treated 'Yes', chemicals used 'Pesticide', country where grown 'United Kingdom', destination countries 'Netherlands'
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
	And the only options available for Unit of measurement are 'Grams' 'Kilograms' 'Tonnes'
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
	Then the seed commodity details are displayed with botanical name 'FLUTI', variety 'special', lot reference '12345', quantity '10000', unit of measurement 'Grams', chemically treated 'Yes', chemicals used 'Pesticide', country where grown 'United Kingdom', destination countries 'Netherlands'
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
	When the user clicks the Save and continue button on the Add seeds to your consignment page
	# @agent: New step on existing files ExporterYourCommoditiesSteps
	Then the Your commodities page is displayed
	# @agent: New step on existing files ExporterYourCommoditiesSteps
	And the commodity table is displayed for arranging for seed sampling to export seeds for planting
	# @agent: New step on existing files ExporterYourCommoditiesSteps
	And the 'Do you want to add more seeds to be sampled?' question is displayed on the Your commodities page
	# @agent: New step on existing files ExporterYourCommoditiesSteps
	And there are radio buttons present on the Your commodities page
	# @agent: New step on existing files ExporterYourCommoditiesSteps
	And there is no option to Save and continue later on the Your commodities page
	When the user selects 'No, I have finished adding commodities' and clicks the Save and continue button on the Your commodities page
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	Then the Your application for seed sampling page is displayed
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	And the 'Seeds to be sampled' task is marked as 'COMPLETE'
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	When the user clicks the Inspection details link
	# @agent: New step, page and interface files ExporterWhereAreTheSeeds
	Then the Where are the seeds? page is displayed
	When the user selects an inspection address and continues
	Then the Do you want to select this inspection address? page is displayed
	When the user selects 'Yes, select this inspection address' and clicks the Continue button on the Do you want to select this inspection address? page
	Then the Who is the contact for this application? page is displayed
	# @agent: New step on existing files ExporterWhoIsTheContactForThisApplicationSteps
	And the fields available on the Who is the contact for this application? page are 'Contact name, Phone number, Email address'
	When the user enters a contact name for the inspector to contact about the application
	And the user enters a phone number for the inspector to contact about the application
	# @agent: New step on existing files ExporterWhoIsTheContactForThisApplicationSteps
	And the user enters an email address for the inspector to contact about the application
	And the user clicks the Save and continue button on the Who is the contact for this application? page
	# @agent: New step, page and interface files ExporterWhenWillTheSeedsBeReadyForSampling
	Then the When will the seeds be ready for sampling? page is displayed
	# @agent: New step, page and interface files ExporterWhenWillTheSeedsBeReadyForSampling
	When the user enters a valid date for when the seeds will be ready for sampling at least 4 days in the future
	# @agent: New step, page and interface files ExporterWhenWillTheSeedsBeReadyForSampling
	And the user clicks the Save and continue button on the When will the seeds be ready for sampling? page
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	Then the Your application for seed sampling page is displayed
	# @agent: New step, page and interface files ExporterYourApplicationForSeedSampling
	And the 'Inspection details' task is marked as 'COMPLETE'
	When the user clicks the Check your answers and submit your application link
	Then the Review your answers page is displayed
	When the user clicks the Continue button on the Review your answers page
	# @agent: New step on existing files ExporterDeclarationSteps
	Then the Declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	# @agent: New step on existing files ExporterYouHaveSuccessfullySubmittedYourApplicationSteps
	Then the You have successfully submitted your seed sampling application page is displayed
	And the user records the APHA reference number for 'Seed Sampling Application'
	# ---------- Exporter Portal - Search for the Phytosanitary certificate ----------
	When I navigate to the Exporter Portal	
	Then the Exporter Portal homepage should be displayed
	# @agent: Search by scenario context
	# @agent: New step on existing files ExporterYourApplicationsSteps
	When the user enters the recorded APHA reference for 'Seed Sampling Application' in the search box
	# @agent: New step on existing files ExporterYourApplicationsSteps
	Then the Export Application portal search returns one matching record
	# @agent: New step on existing files ExporterYourApplicationsSteps
	When the user clicks the APHA reference link for 'Seed Sampling Application'
	# @agent: New step, page and interface files ExporterManageApplication
	Then the Manage application page should be displayed
	# @agent: New step, page and interface files ExporterManageApplication
	When the user clicks the Sampling report (csv) link on the Manage application page
	# @agent: Use this step definition to help implement the step below: [When("the user clicks Download the CSV template and read the guide on how to complete this \\(opens in new tab)")]
	# @agent: New step, page and interface files ExporterManageApplication
	Then the Sampling report (csv) file is downloaded
	# @agent: Use this step definition to help implement the step below: [When("the user updates the bulk update CSV {string} setting the Id for commodity code {string} to the recorded {string}")]
	# @agent: New step, page and interface files ExporterManageApplication
	# @agent: Reference file for CSV validation is 'seedSamplingReport_2026-10-09-095850.csv'
	And the Remaining Approved Stock quantity is '3000.0'
	
Scenario: Exporter 2 - SPS-4289
	# ---------- Exporter Portal - Apply for a phytosanitary certificate using the stock ----------
	# @agent: Use this step definition to help implement the step below: [When("I switch to the IPAFFS tab")]
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
	# @agent: New step on existing files ExporterRegulationsForExportingSteps
	And the guidance needs you to have a phytosanitary certificate to export seeds to 'Netherlands'
	# @agent: New step on existing files ExporterRegulationsForExportingSteps
	And the guidance needs you to describe your consignment using the botanical name (genus and species)
	When the user ticks the exporting regulations checkbox 
	And the user clicks the Continue button on the Regulations for exporting page
	Then the Create a reference page is displayed
	When the user enters a reference number and clicks the Save and continue button
	Then the Your application page is displayed
	# @agent: New step on existing files ExporterYourApplicationSteps
	And the 'Create a reference' task is marked as 'COMPLETE'
	When the user clicks the What is the exporter address? link
	Then the Is this the exporter address? page is displayed
	# @agent: New step on existing files ExporterIsThisTheExporterAddressSteps
	And the guidance is This is the address saved to your Defra account.
	# @agent: New step on existing files ExporterIsThisTheExporterAddressSteps
	And the 'Yes' and 'No' radio buttons are displayed and unselected by default
	When the user selects 'Yes' and clicks the Save and continue button on the Is this the exporter address? page
	Then the Your application page is displayed
	# @agent: New step on existing files ExporterYourApplicationSteps
	And the 'What is the exporter address?' task is marked as 'COMPLETE'
	When the user clicks the What's in your consignment? link
	Then the How do you want to add commodities to your consignment? page is displayed
	When the user selects 'Manually, by adding one at a time' and clicks the Continue button on the How do you want to add commodities to your consignment? page
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
	Then the Add seeds to your consignment page is displayed
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
	When the user describes the seed commodity with botanical name 'FLUTI', variety 'special', lot reference '12345', sample reference, quantity '7', unit of measurement 'Kilograms', number of packages '7', type of packaging 'Box', countries of origin 'United Kingdom'
	# @agent: New step, page and interface files ExporterAddSeedsToYourConsignment
	And the user clicks the Save and continue button on the Add seeds to your consignment page
	# @agent: New step on existing files ExporterYourCommoditiesSteps
	Then the Your commodities page is displayed
	# @agent: New step on existing files ExporterYourCommoditiesSteps
	And the commodity table is displayed for applying for a phytosanitary certificate to export seeds for planting
	When the user selects 'No, I have finished adding commodities' and clicks the Save and continue button on the Your commodities page
	Then the Your application page is displayed
	# @agent: New step on existing files ExporterYourApplicationSteps
	And the "What's in your consignment" task is marked as 'COMPLETE'
	When the user clicks the Who are you sending this consignment to? link
	Then the Who are you sending this consignment to? page is displayed
	When the user enters the conisgnee details and clicks the Continue button on the Who are you sending this consignment to? page
	Then the Your application page is displayed
	# @agent: New step on existing files ExporterYourApplicationSteps
	And the 'Who are you sending this consignment to?' task is marked as 'COMPLETE'
	When the user clicks the How will this consignment be transported? link
	Then the How will this consignment be transported? page is displayed
	When the user enters the value of the consignment in pounds 1200.00
	And the user selects 'Yes' for Do you know the point of entry?
	And the user enters the point of entry 'NL AMS'
	And the user selects a transport method 'Road or ferry'
	And the user clicks the Save and continue button on the How will this consignment be transported? page
	Then the Your application page is displayed
	# @agent: New step on existing files ExporterYourApplicationSteps
	And the 'How will this consignment be transported?' task is marked as 'COMPLETE'
	When the user clicks the Check your answers and submit your application link
	Then the Review your answers page is displayed
	# @agent: Use this step definition to help implement the step below: [Then("the data presented for review matches the data entered into the notification for CHED PP")]
	# @agent: New step on existing files ExporterReviewYourAnswersSteps
	And the data presented for review matches the data entered into the Exporter Portal for a phytosanitary certificate application
	When the user clicks the Continue button on the Review your answers page
	Then the When do you need the certificate? page is displayed
	When the user enters a valid date for when the certificate is needed at least 4 days in the future 
	And the user clicks the Continue button on the When do you need the certificate? page
	Then the Exporter declaration page is displayed
	When the user ticks the exporter declaration checkbox
	And the user clicks the Submit application button
	# @agent: New step on existing files ExporterYouHaveSuccessfullySubmittedYourApplicationSteps
	Then the Application complete page is displayed
	And the user records the APHA reference number for 'Phytosanitary Certificate Application'
	
Scenario: Exporter 3 - SPS-4289

	# ---------- Exporter Portal - Search for the Phytosanitary certificate ----------
	When I navigate to the Exporter Portal	
	Then the Exporter Portal homepage should be displayed
	# @agent: Search by scenario context
	# @agent: New step on existing files ExporterYourApplicationsSteps
	When the user enters the recorded APHA reference for 'Seed Sampling Application' in the search box
	# @agent: New step on existing files ExporterYourApplicationsSteps
	Then the Export Application portal search returns one matching record
	# @agent: New step on existing files ExporterYourApplicationsSteps
	When the user clicks the APHA reference link for 'Seed Sampling Application'
	# @agent: New step, page and interface files ExporterManageApplication
	Then the Manage application page should be displayed
	# @agent: New step, page and interface files ExporterManageApplication
	When the user clicks the Sampling report (csv) link on the Manage application page
	# @agent: Use this step definition to help implement the step below: [When("the user clicks Download the CSV template and read the guide on how to complete this \\(opens in new tab)")]
	# @agent: New step, page and interface files ExporterManageApplication
	Then the Sampling report (csv) file is downloaded
	# @agent: Use this step definition to help implement the step below: [When("the user updates the bulk update CSV {string} setting the Id for commodity code {string} to the recorded {string}")]
	# @agent: New step, page and interface files ExporterManageApplication
	# @agent: Reference file for CSV validation is 'seedSamplingReport_2026-10-09-095850.csv'
	And the Remaining Approved Stock quantity is '3000.0'