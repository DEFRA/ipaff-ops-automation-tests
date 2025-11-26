# nipts-pts-ops-automation-tests

# Introduction 
This solution/repo holds the automation tests of Charge, Customer Extension, Delegated Authority, Developer portal, ETrade, Notify, Risk Assessment and Service Status modules

# Getting Started -> Build and Test
Original Run Settings file should contain the below values
1.	Runtime parameters in appsettings.json
	{
	  "AppSettings": {

		"TestConfiguration": {

		  "Environment": "https://dev-uk.trade.azure.defra.cloud",
		  "EnvPassword": "OmRWeuOb2eqwioiJeUCwhpBLeIkws9dV", -> Password on the environment used
		  "Headless": false, [Values -> true(hide browser)]
		  "GlobalWaitsInSeconds": 10,
		  "DeviceName": "Windows", [Values -> check from Browserstack]
		  "BSOSVersion": "11",
		  "Project": "Etrade_UI_Tests",
		  "Build": "Local_Etrade_UI_Tests",
		  "Platform": "Desktop", [Values -> Desktop/Mobile]
		  "IsEmulationEnabled": false, [Values -> to execute tests in chrome Emulation]
		  "EmulateDeviceInfo": "iPad Air" [Values -> check from 'https://chromedriver.chromium.org/mobile-emulation'],
		  "GB_Trader_TST": "", [Values -> "139273110608/paxxxxxword"] -> Override here to run from your local,
      	  "NI_Trader_TST": "", [Values -> "139273110608/paxxxxxword"]-> Override here to run from your local,
      	  "GB_Trader_PRE": "", [Values -> "139273110608/paxxxxxword"]-> Override here to run from your local,
      	  "NI_Trader_PRE": "" [Values -> "139273110608/paxxxxxword"]-> Override here to run from your local,

		},

		"UiFrameworkConfiguration": {

		  "SeleniumGrid": "", [Values -> "https://hub.browserstack.com/wd/hub/"], [Values -> "http://localhost:4444/wd/hub/"]
		  "Target": "Chrome", 
		  "IsDebug": false [Values -> true to enable all logs]
		},
		"BrowserStackConfiguration": {
		  "CloudDeviceTarget": true,
		  "CloudCountryCode": "",
		  "CloudDeviceUserName": "nesa_Mz3Bsv",
		  "CloudDeviceUserKey": "zWNskRdAsuJtz2yi57oW",
		  "CloudDeviceIdentifier": "Test123_Nesa" [to expose local environments via BS proxy]
		}
	  }
	}

2. How to execute test from local commandline 
   
   >dotnet test C:\Working\Defra.Trade.EEHC.Automation\Defra.UI.Tests\bin\Debug\net6.0\Defra.UI.Tests.dll --settings C:\Working\Defra.Trade.EEHC.Automation\Defra.UI.Tests\testrun.runsettings --filter TestCategory="EnvCheck"
   
3. Test
