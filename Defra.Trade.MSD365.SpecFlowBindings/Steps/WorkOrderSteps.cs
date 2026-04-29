// <copyright file="AlertSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.Trade.Plants.Specs.Steps;
using FluentAssertions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Reqnroll;

/// <summary>
/// Step bindings relating to the work order functional area.
/// </summary>
[Binding]
public sealed class WorkOrderSteps : PowerAppsStepDefiner
{
    private readonly SessionContext sessionContext;
    private readonly ScenarioContext scenarioContext;

    public WorkOrderSteps(SessionContext sessionContext, ScenarioContext scenarioContext = null)
    {
        this.sessionContext = sessionContext;
        this.scenarioContext = scenarioContext;
    }

    [Then("I verify the Work Order page is displayed for the notification created in IPAFFS")]
    public void ThenIVerifyTheWorkOrderPageIsDisplayedForTheNotificationCreatedInIPAFFS()
    {
        Driver.WaitForTransaction();

        var expectedChedReference = scenarioContext.Get<string>("CHEDReference");

        var pageHeader = Driver.WaitUntilAvailable(
            By.XPath("//span[@data-id='entity_name_span']"),
            "Work Order page header could not be found.");

        var actualPageHeader = pageHeader.Text.Trim();
        actualPageHeader.Should().Be("Work Order",
            $"Expected page header to be 'Work Order' but found '{actualPageHeader}'.");

        var chedReferenceHeader = Driver.WaitUntilAvailable(
            By.XPath("//h1[@data-id='header_title']"),
            $"CHED reference header could not be found on the Work Order page.");

        var actualChedReference = chedReferenceHeader.Text
            .Replace("- Saved", string.Empty)
            .Trim();

        actualChedReference.Should().Be(expectedChedReference,
            $"Expected CHED reference header to be '{expectedChedReference}' but found '{actualChedReference}'.");
    }

    [When("I click the Assign command")]
    public void WhenIClickTheAssignCommand()
    {
        Driver.WaitForTransaction();

        // Determine whether the Assign command should be clicked inside a maximised popup
        // or on the main Work Order page command bar.
        //
        // When called after "I click on the '<task>' task" + "I maximise the popup", the popup
        // container is present in the DOM and its own command bar may not yet have fully rendered.
        // Clicking via CommandSteps.WhenISelectTheCommand in this state hits the Work Order page's
        // overflow button instead, causing ElementClickInterceptedException.
        //
        // When called from the Work Order page (no active popup), fall through to the standard helper.
        var popupContainers = Driver.FindElements(
            By.XPath("//section[contains(@id,'popupContainer')]"));

        if (popupContainers.Count > 0)
        {
            // Wait for the popup's own Assign button to be present and visible before clicking.
            // Scope to the popup container to avoid matching the Work Order command bar.
            var popupAssignButton = Driver.WaitUntilAvailable(
                By.XPath("//section[contains(@id,'popupContainer')]//button[@aria-label='Assign' or @title='Assign']"),
                TimeSpan.FromSeconds(30),
                "Assign button could not be found in the popup command bar within 30 seconds.");

            Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", popupAssignButton);
            Driver.WaitForTransaction();

            popupAssignButton.Click();
        }
        else
        {
            CommandSteps.WhenISelectTheCommand("Assign");
        }

        Driver.WaitForTransaction();
    }

    [Then("I can see the Assign Work Order popup is displayed")]
    public void ThenICanSeeTheAssignWorkOrderPopupIsDisplayed()
    {
        Driver.WaitForTransaction();

        var assignDialog = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='Assign' and @data-uci-dialog='true']"),
            "Assign Work Order dialog could not be found.");

        var dialogHeader = assignDialog.FindElement(By.XPath(".//h1[@data-id='assignheader_id']"));

        dialogHeader.Text.Trim().Should().Be("Assign Work Order",
            $"Expected dialog header to be 'Assign Work Order' but found '{dialogHeader.Text.Trim()}'.");
    }

    [When("I click the Assign button")]
    public void WhenIClickTheAssignButton()
    {
        Driver.WaitForTransaction();
        var assignButton = Driver.WaitUntilAvailable(
            By.XPath("//button[@data-id='ok_id' and @title='Assign']"),
            "Assign button could not be found in the Assign Work Order dialog.");

        assignButton.Click();
        Driver.WaitForTransaction();
    }

    [Then("the Substatus of the Work Order should be Assigned")]
    public void ThenTheSubstatusOfTheWorkOrderShouldBeAssigned()
    {
        Driver.WaitForTransaction();

        var substatusLink = Driver.WaitUntilAvailable(
            By.XPath("//a[@aria-label='Assigned']"),
            "Substatus 'Assigned' could not be found in the Work Order header.");

        substatusLink.Text.Trim().Should().Be("Assigned",
            $"Expected Substatus to be 'Assigned' but found '{substatusLink.Text.Trim()}'.");
    }

    [Then("the Owner of the Work Order should be me")]
    public void ThenTheOwnerOfTheWorkOrderShouldBeMe()
    {
        Driver.WaitForTransaction();

        var currentUser = TestConfig.GetUser("Inspector",useCurrentUser: true);
        var localPart = currentUser.Username.Split('@')[0];
        var expectedOwner = string.Join(" ", localPart.Split('.')
            .Select(p => char.ToUpper(p[0]) + p.Substring(1)));

        var ownerLink = Driver.WaitUntilAvailable(
            By.XPath($"//a[@aria-label='{expectedOwner}']"),
            $"Owner '{expectedOwner}' could not be found in the Work Order header.");

        ownerLink.Text.Trim().Should().Be(expectedOwner,
            $"Expected Owner to be '{expectedOwner}' but found '{ownerLink.Text.Trim()}'.");
    }

    [When(@"I check that the Commodity Lines frame shows '(.*)'")]
    public void WhenICheckThatTheCommodityLinesFrameShows(string expectedViewName)
    {
        Driver.WaitForTransaction();

        var viewLabel = Driver.WaitUntilAvailable(
            By.XPath($"//span[contains(@id,'ViewSelector_') and contains(@id,'_text-value') and normalize-space(text())='{expectedViewName}']"),
            $"Commodity Lines frame view '{expectedViewName}' could not be found.");
        
        viewLabel.Text.Trim().Should().Be(expectedViewName,
            $"Expected Commodity Lines frame to show '{expectedViewName}' but found '{viewLabel.Text.Trim()}'.");
    }

    private static readonly string[] ValidRegulatoryAuthorities = ["PHSI", "HMI", "Joint"];
    private const string EppoCodeColumnDataId = "trd_eppocode";
    private const string RegulatoryAuthorityColumnDataId = "trd_regulatoryauthoritycode";

    /// <summary>
    /// Verifies that all Import Commodity Lines on the Work Order match the EPPO codes
    /// provided in the test input, and that each line has a valid Regulatory Authority.
    /// </summary>
    [Then(@"all the Commodity Lines should be validated with the values given in the input")]
    public void ThenAllTheCommodityLinesShouldBeValidatedWithTheValuesGivenInTheInput()
    {
        if (!scenarioContext.ContainsKey("AllCommodityDetails"))
        {
            var tempTable = new Table("Commodity code", "Genus and Species", "EPPO code");
            tempTable.AddRow("06012030", "Calanthe biloba", "CLPBI");
            tempTable.AddRow("06012030", "Brassavola sp.", "BSVSS");
            tempTable.AddRow("06011010", "Albuca bracteata", "ABWBR");
            tempTable.AddRow("06029045", "Abelia engleriana", "ABEEN");
            tempTable.AddRow("06029045", "Abelia graebneriana", "ABEGR");
            tempTable.AddRow("06029045", "Abelia integrifolia", "ABEIN");
            tempTable.AddRow("06029045", "Abelia ionandra", "ABEIO");
            tempTable.AddRow("06029045", "Abelia schumannii", "ABESC");
            tempTable.AddRow("06029045", "Abelia sp.", "ABESS");
            tempTable.AddRow("06029045", "Abelia triflora", "ABETR");
            tempTable.AddRow("06029045", "Abelia umbellata", "ABEUM");
            tempTable.AddRow("06029045", "Abelia uniflora", "ABEUN");
            tempTable.AddRow("06029045", "Abelia x grandiflora", "ABEGF");
            tempTable.AddRow("06029045", "Abelia zanderi", "ABEZA");
            tempTable.AddRow("06029045", "Abeliophyllum distichum", "ABLDI");
            tempTable.AddRow("06029045", "Abeliophyllum sp.", "ABLSS");
            tempTable.AddRow("06029045", "Abelmoschus caillei", "ABMCA");
            tempTable.AddRow("06011010", "Albuca fibrotunicata", "ABWFI");
            tempTable.AddRow("06029045", "Abelmoschus ficulneus", "HIBFC");
            tempTable.AddRow("06029045", "Abelmoschus glutino-textilis", "ABMGT");
            tempTable.AddRow("06029045", "Abelmoschus manihot", "HIBMA");
            tempTable.AddRow("06029045", "Abelmoschus mindanensis", "ABMMI");
            tempTable.AddRow("06029045", "Abelmoschus moschatus", "ABMMO");
            tempTable.AddRow("06029045", "Abelmoschus sp.", "ABMSS");
            tempTable.AddRow("06029045", "Abies alba", "ABIAL");
            tempTable.AddRow("06029045", "Abies amabilis", "ABIAM");
            tempTable.AddRow("06029045", "Abies balsamea", "ABIBA");
            tempTable.AddRow("06029045", "Abies balsamea var. phanerolepis", "ABIPH");
            tempTable.AddRow("06029045", "Abies borisii-regis", "ABIBO");
            tempTable.AddRow("06029045", "Abies bracteata", "ABIBT");
            tempTable.AddRow("06029045", "Abies cephalonica", "ABICE");
            tempTable.AddRow("06029045", "Abies chengii", "ABICN");
            tempTable.AddRow("06029045", "Abies chensiensis", "ABICH");
            tempTable.AddRow("06029045", "Abies cilicica", "ABICI");
            tempTable.AddRow("06029045", "Abies concolor", "ABICO");
            tempTable.AddRow("06029045", "Abies concolor var. lowiana", "ABICL");
            tempTable.AddRow("06029045", "Abies delavayi", "ABIDE");
            tempTable.AddRow("06029045", "Abies densa", "ABIDN");
            tempTable.AddRow("06029045", "Abies durangensis", "ABIDU");
            tempTable.AddRow("06029045", "Abies fabri", "ABIFA");
            tempTable.AddRow("06029045", "Abies fabri subsp. minensis", "ABIMI");
            tempTable.AddRow("06029045", "Abies fargesii", "ABIFG");
            tempTable.AddRow("06029045", "Abies firma", "ABIFI");
            tempTable.AddRow("06029045", "Abies forrestii", "ABIFO");
            tempTable.AddRow("06029045", "Abies fraseri", "ABIFR");
            tempTable.AddRow("06029045", "Abies grandis", "ABIGR");
            tempTable.AddRow("06029045", "Abies guatemalensis", "ABIGU");
            tempTable.AddRow("06029045", "Abies guatemalensis var. jaliscana", "ABIFL");
            tempTable.AddRow("06029045", "Abies hickelii", "ABIHI");
            tempTable.AddRow("06029045", "Abies holophylla", "ABIHL");
            tempTable.AddRow("06029045", "Abies homolepis", "ABIHO");
            tempTable.AddRow("06029045", "Abies kawakamii", "ABIKA");
            tempTable.AddRow("06029045", "Abies koreana", "ABIKO");
            tempTable.AddRow("06029045", "Abies lasiocarpa", "ABILA");
            tempTable.AddRow("06029045", "Abies lasiocarpa var. arizonica", "ABILZ");
            tempTable.AddRow("06029045", "Abies magnifica", "ABIMA");
            tempTable.AddRow("06029045", "Abies mariesii", "ABIMR");
            tempTable.AddRow("06029045", "Abies nebrodensis", "ABINB");
            tempTable.AddRow("06029045", "Abies nephrolepis", "ABINE");
            tempTable.AddRow("06029045", "Abies nordmanniana", "ABINO");
            tempTable.AddRow("06029045", "Abies nordmanniana subsp. equitrojani", "ABIBR");
            tempTable.AddRow("06029045", "Abies numidica", "ABINU");
            tempTable.AddRow("06029045", "Abies pindrow", "ABIPI");
            tempTable.AddRow("06029045", "Abies pindrow var. brevifolia", "ABIGA");
            tempTable.AddRow("06029045", "Abies pinsapo", "ABIPN");
            tempTable.AddRow("06029045", "Abies pinsapo var. marocana", "ABIMC");
            tempTable.AddRow("06029045", "Abies procera", "ABIPR");
            tempTable.AddRow("06029045", "Abies recurvata", "ABIRE");
            tempTable.AddRow("06029045", "Abies recurvata var. ernestii", "ABIER");
            tempTable.AddRow("06029045", "Abies religiosa", "ABIRG");
            tempTable.AddRow("06029045", "Abies sachalinensis", "ABISA");
            tempTable.AddRow("06029045", "Abies sachalinensis var. gracilis", "ABISG");
            tempTable.AddRow("06029045", "Abies sachalinensis var. mayriana", "ABISM");
            tempTable.AddRow("06029045", "Abies sachalinensis var. nemorensis", "ABISN");
            tempTable.AddRow("06029045", "Abies sibirica", "ABISB");
            tempTable.AddRow("06029045", "Abies sibirica subsp. semenovii", "ABISE");
            tempTable.AddRow("06029045", "Abies sp.", "ABISS");
            tempTable.AddRow("06029045", "Abies spectabilis", "ABISP");
            tempTable.AddRow("06029045", "Abies squamata", "ABISQ");
            tempTable.AddRow("06029045", "Abies veitchii", "ABIVE");
            tempTable.AddRow("06029045", "Abies vejarii", "ABIVJ");
            tempTable.AddRow("06029045", "Abies vejarii subsp. mexicana", "ABIME");
            tempTable.AddRow("06029045", "Abies x arnoldiana", "ABIAR");
            tempTable.AddRow("06029045", "Abies x insignis", "ABIIN");
            tempTable.AddRow("06029045", "Abies x vasconcellosiana", "ABIVA");
            tempTable.AddRow("06029045", "Abies x vilmorinii", "ABIVI");
            tempTable.AddRow("06029045", "Abrus precatorius", "ABRPR");
            tempTable.AddRow("06029045", "Abrus precatorius subsp. africanus", "ABRPA");
            tempTable.AddRow("06029045", "Abrus melanospermus", "ABRPP");
            tempTable.AddRow("06029045", "Abrus melanospermus subsp. Suffruticosus", "ABRPS");
            tempTable.AddRow("06029045", "Abrus sp.", "ABRSS");
            tempTable.AddRow("06029045", "Abutilon angulatum", "ABUAN");
            tempTable.AddRow("06029045", "Abutilon asiaticum", "ABUAS");
            tempTable.AddRow("06029045", "Abutilon fruticosum", "ABUFR");
            tempTable.AddRow("06029045", "Abutilon grandifolium", "ABUMO");
            tempTable.AddRow("06029045", "Abutilon graveolens", "ABUGR");
            tempTable.AddRow("06029045", "Abutilon guineense", "ABUGU");
            tempTable.AddRow("06029045", "Abutilon hemsleyanum", "ABUHE");
            tempTable.AddRow("06029045", "Abutilon hybrids", "ABUHY");
            tempTable.AddRow("06029045", "Abutilon indicum", "ABUIN");
            tempTable.AddRow("06029045", "Abutilon inflatum", "ABUIF");
            tempTable.AddRow("06029045", "Abutilon ochsenii", "ABUOC");
            tempTable.AddRow("06029045", "Abutilon oxycarpum", "ABUOX");
            tempTable.AddRow("06029045", "Abutilon pannosum", "ABUGL");
            tempTable.AddRow("06029045", "Abutilon pauciflorum", "ABUPF");
            tempTable.AddRow("06029045", "Abutilon ramosum", "ABURA");
            tempTable.AddRow("06029045", "Abutilon sonneratianum", "ABUSO");
            tempTable.AddRow("06029045", "Pseudabutilon sp.", "PSDSS");
            tempTable.AddRow("06029045", "Abutilon theophrasti", "ABUTH");
            tempTable.AddRow("06029045", "Abutilon trisulcatum", "ABUTR");
            tempTable.AddRow("06029045", "Abutilon vitifolium", "ABUVI");
            tempTable.AddRow("06029045", "Acacia alata", "ACAAL");
            tempTable.AddRow("06029045", "Acacia anceps", "ACAGL");
            tempTable.AddRow("06029045", "Acacia aneura", "ACAAN");
            tempTable.AddRow("06029045", "Acacia argyrodendron", "ACAAD");
            tempTable.AddRow("06029045", "Acacia aulacocarpa", "ACAAU");
            tempTable.AddRow("06029045", "Acacia auriculiformis", "ACAAF");
            tempTable.AddRow("06029045", "Acacia baileyana", "ACABA");
            tempTable.AddRow("06029045", "Acacia brachystachya", "ACABR");
            tempTable.AddRow("06029045", "Acacia burrowii", "ACABU");
            tempTable.AddRow("06029045", "Acacia cambagei", "ACACB");
            tempTable.AddRow("06029045", "Acacia cana", "ACACN");
            tempTable.AddRow("06029045", "Acacia cardiophylla", "ACACD");
            tempTable.AddRow("06029045", "Acacia catenulata", "ACACE");
            tempTable.AddRow("06029045", "Acacia concurrens", "ACACH");
            tempTable.AddRow("06029045", "Acacia confusa", "ACACU");
            tempTable.AddRow("06029045", "Acacia coriacea", "ACACR");
            tempTable.AddRow("06029045", "Acacia cultriformis", "ACACL");
            tempTable.AddRow("06029045", "Acacia curvinervia", "ACACQ");
            tempTable.AddRow("06029045", "Acacia cyclops", "ACACC");
            tempTable.AddRow("06029045", "Acacia cyperophylla", "ACACP");
            tempTable.AddRow("06029045", "Acacia dealbata", "ACADA");
            tempTable.AddRow("06029045", "Acacia deanei", "ACADN");
            tempTable.AddRow("06029045", "Acacia decurrens", "ACADC");
            tempTable.AddRow("06029045", "Acacia doratoxylon", "ACADO");
            tempTable.AddRow("06029045", "Acacia ericifolia", "ACAER");
            tempTable.AddRow("06029045", "Acacia excelsa", "ACAEX");
            tempTable.AddRow("06029045", "Acacia flavescens", "ACAFL");
            tempTable.AddRow("06029045", "Acacia genistifolia", "ACADI");
            tempTable.AddRow("06029045", "Acacia georginae", "ACAGG");
            tempTable.AddRow("06029045", "Acacia glaucoptera", "ACAGP");
            tempTable.AddRow("06029045", "Acacia harpophylla", "ACAHA");
            tempTable.AddRow("06029045", "Acacia homalophylla", "ACAHM");
            tempTable.AddRow("06029045", "Acacia implexa", "ACAIM");
            tempTable.AddRow("06029045", "Acacia ixiophylla", "ACAIX");
            tempTable.AddRow("06029045", "Acacia koa", "ACAKO");
            tempTable.AddRow("06029045", "Acacia leiocalyx", "ACALE");
            tempTable.AddRow("06029045", "Acacia leptocarpa", "ACALC");
            tempTable.AddRow("06029045", "Acacia longifolia", "ACALO");
            tempTable.AddRow("06029045", "Acacia macracantha", "ACAMA");
            tempTable.AddRow("06029045", "Acacia maidenii", "ACAMN");
            tempTable.AddRow("06029045", "Acacia mangium", "ACAMG");
            tempTable.AddRow("06029045", "Acacia mearnsii", "ACAMR");
            tempTable.AddRow("06029045", "Acacia melanoxylon", "ACAME");
            tempTable.AddRow("06029045", "Acacia mellifera subsp. detinens", "ACAMD");
            tempTable.AddRow("06029045", "Acacia mucronata", "ACAMU");
            tempTable.AddRow("06029045", "Acacia oswaldii", "ACAOS");
            tempTable.AddRow("06029045", "Acacia paniculata", "ACAPA");
            tempTable.AddRow("06029045", "Acacia paradoxa", "ACAAR");
            tempTable.AddRow("06029045", "Acacia pendula", "ACAPD");
            tempTable.AddRow("06029045", "Acacia penninervis", "ACAPN");
            tempTable.AddRow("06029045", "Acacia permixta", "ACAPX");
            tempTable.AddRow("06029045", "Acacia peuce", "ACAPC");
            tempTable.AddRow("06029045", "Acacia plumosa", "ACAPL");
            tempTable.AddRow("06029045", "Acacia podalyriifolia", "ACAPF");
            tempTable.AddRow("06029045", "Acacia polyphylla", "ACAPO");
            tempTable.AddRow("06029045", "Acacia pravissima", "ACAPR");
            tempTable.AddRow("06029045", "Acacia pulchella", "ACAPU");
            tempTable.AddRow("06029045", "Acacia pycnantha", "ACAPY");
            tempTable.AddRow("06029045", "Acacia redolens", "ACARD");
            tempTable.AddRow("06029045", "Acacia reficiens", "ACARF");
            tempTable.AddRow("06029045", "Acacia retinodes", "ACART");
            tempTable.AddRow("06029045", "Acacia riceana", "ACARC");
            tempTable.AddRow("06029045", "Acacia rigidula", "ACARI");
            tempTable.AddRow("06029045", "Acacia salicina", "ACASC");
            tempTable.AddRow("06029045", "Acacia saligna", "ACASA");
            tempTable.AddRow("06029045", "Acacia senegal var. leiorhachis", "ACASL");
            tempTable.AddRow("06029045", "Acacia senegal var. rostrata", "ACASO");
            tempTable.AddRow("06029045", "Acacia seyal var. fistula", "ACASF");
            tempTable.AddRow("06029045", "Acacia shirleyi", "ACASH");
            tempTable.AddRow("06029045", "Vachellia sieberiana var. woodii", "ACASW");
            tempTable.AddRow("06029045", "Acacia sp.", "ACASS");
            tempTable.AddRow("06029045", "Acacia sparsiflora", "ACASQ");
            tempTable.AddRow("06029045", "Acacia spirocarpa", "ACASR");
            tempTable.AddRow("06029045", "Acacia stenophylla", "ACAST");
            tempTable.AddRow("06029045", "Acacia stuhlmannii", "ACASM");
            tempTable.AddRow("06029045", "Acacia sutherlandii", "ACASU");
            tempTable.AddRow("06029045", "Acacia tenuifolia", "ACATF");
            tempTable.AddRow("06029045", "Acacia tenuispina", "ACATS");
            tempTable.AddRow("06029045", "Acacia terminalis", "ACATM");
            tempTable.AddRow("06029045", "Acacia tetragonophylla", "ACATE");
            tempTable.AddRow("06029045", "Acacia tortilis subsp. raddiana", "ACATR");
            tempTable.AddRow("06029045", "Acacia truncata", "ACADE");
            tempTable.AddRow("06029045", "Acacia unijuga", "ACAUN");
            tempTable.AddRow("06029045", "Acacia vernicosa", "ACAVC");
            tempTable.AddRow("06029045", "Acacia verticillata", "ACAVE");
            tempTable.AddRow("06029045", "Acacia victoriae", "ACAVI");
            tempTable.AddRow("06029045", "Acacia welwitschii subsp. delagoensis", "ACAWD");
            tempTable.AddRow("06029045", "Acacia wrightii", "ACAWR");
            tempTable.AddRow("06029045", "Acaciella angustissima", "ACAAG");
            tempTable.AddRow("06029045", "Acalypha guatemalensis", "ACCGU");
            tempTable.AddRow("06029045", "Acalypha havanensis", "ACCHA");
            tempTable.AddRow("06029045", "Acalypha hispida", "ACCHI");
            tempTable.AddRow("06029045", "Acalypha indica", "ACCIN");
            tempTable.AddRow("06029045", "Acalypha macrostachya", "ACCMA");
            tempTable.AddRow("06029045", "Acalypha malabarica", "ACCMB");
            tempTable.AddRow("06029045", "Acalypha neomexicana", "ACCNE");
            tempTable.AddRow("06029045", "Acalypha persimilis", "ACCOS");
            tempTable.AddRow("06029045", "Acalypha pendula", "ACCPN");
            tempTable.AddRow("06029045", "Acalypha poirettii", "ACCPO");
            tempTable.AddRow("06029045", "Acalypha polystachya", "ACCPY");
            tempTable.AddRow("06029045", "Acalypha pseudoalopecuroides", "ACCPS");
            tempTable.AddRow("06029045", "Acalypha racemosa", "ACCRA");
            tempTable.AddRow("06029045", "Acalypha rhomboidea", "ACCRH");
            tempTable.AddRow("06029045", "Acalypha schiedana", "ACCSC");
            tempTable.AddRow("06029045", "Acalypha segetalis", "ACCSE");
            tempTable.AddRow("06029045", "Acalypha setosa", "ACCST");
            tempTable.AddRow("06029045", "Acalypha sp.", "ACCSS");
            tempTable.AddRow("06029045", "Acalypha villicaulis", "ACCPE");
            tempTable.AddRow("06029045", "Acalypha virginica", "ACCVI");
            tempTable.AddRow("06029045", "Acalypha wilkesiana", "ACCWI");
            tempTable.AddRow("06029045", "Acantholimon acerosum", "ACLAC");
            tempTable.AddRow("06029045", "Acantholimon armenum", "ACLAR");
            tempTable.AddRow("06029045", "Acantholimon caryophyllaceum", "ACLCA");
            tempTable.AddRow("06029045", "Acantholimon diapensioides", "ACLDI");
            tempTable.AddRow("06029045", "Acantholimon glumaceum", "ACLGL");
            tempTable.AddRow("06029045", "Acantholimon kotschyi", "ACLKO");
            tempTable.AddRow("06029045", "Acantholimon libanoticum", "ACLLI");
            tempTable.AddRow("06029045", "Acantholimon melananthum", "ACLME");
            tempTable.AddRow("06029045", "Acantholimon olivieri", "ACLOL");
            tempTable.AddRow("06029045", "Acantholimon sp.", "ACLSS");
            tempTable.AddRow("06029045", "Acantholimon ulicinum", "ACLAN");
            tempTable.AddRow("06029045", "Acanthoprasium frutescens", "BLLFR");
            tempTable.AddRow("06029045", "Acanthosicyos horridus", "ACWHO");
            tempTable.AddRow("06029045", "Acanthosicyos sp.", "ACWSS");
            tempTable.AddRow("06029045", "Acanthospermum australe", "ACNAU");
            tempTable.AddRow("06029045", "Acanthospermum glabratum", "ACNGL");
            tempTable.AddRow("06011010", "Bellevalia ciliata", "BLVCI");
            tempTable.AddRow("06029045", "Acanthospermum humile", "ACNHU");
            tempTable.AddRow("06029045", "Acanthospermum sp.", "ACNSS");
            tempTable.AddRow("06029045", "Acanthostyles buniifolius", "EUPBU");
            tempTable.AddRow("06029045", "Acanthosyris faclata", "AHSFA");
            tempTable.AddRow("06029045", "Acanthosyris paulo-alvimii", "AHSPA");
            tempTable.AddRow("06029045", "Acanthosyris sp.", "AHSSS");
            tempTable.AddRow("06029045", "Acca sellowiana", "FEJSE");
            tempTable.AddRow("06029045", "Acer acuminatum", "ACRAC");
            tempTable.AddRow("06029045", "Acer albopurpurascens", "ACRAL");
            tempTable.AddRow("06029045", "Acer amplum", "ACRAM");
            tempTable.AddRow("06029045", "Acer argutum", "ACRAR");
            tempTable.AddRow("06029045", "Acer barbatum", "ACRBA");
            tempTable.AddRow("06029045", "Acer barbinerve", "ACRBB");
            tempTable.AddRow("06029045", "Acer buergerianum", "ACRBU");
            tempTable.AddRow("06029045", "Acer caesium", "ACRCE");
            tempTable.AddRow("06029045", "Acer caesium subsp. giraldii", "ACRCI");
            tempTable.AddRow("06029045", "Acer campbellii", "ACRCB");
            tempTable.AddRow("06029045", "Acer campestre", "ACRCA");
            tempTable.AddRow("06029045", "Acer capillipes", "ACRCL");
            tempTable.AddRow("06029045", "Acer cappadocicum", "ACRCP");
            tempTable.AddRow("06029045", "Acer carpinifolium", "ACRCR");
            tempTable.AddRow("06029045", "Acer catalpifolium", "ACRCT");
            tempTable.AddRow("06029045", "Acer caudatifolium", "ACRCF");
            tempTable.AddRow("06029045", "Acer caudatum", "ACRCD");
            tempTable.AddRow("06029045", "Acer caudatum subsp. ukurunduense", "ACRUK");
            tempTable.AddRow("06029045", "Acer caudatum var. multiserratum", "ACRCM");
            tempTable.AddRow("06029045", "Acer circinatum", "ACRCJ");
            tempTable.AddRow("06029045", "Acer cissifolium", "ACRCS");
            tempTable.AddRow("06029045", "Acer cordatum", "ACRCO");
            tempTable.AddRow("06029045", "Acer coriaceifolium", "ACRCC");
            tempTable.AddRow("06029045", "Acer crataegifolium", "ACRCG");
            tempTable.AddRow("06029045", "Acer davidii", "ACRDA");
            tempTable.AddRow("06029045", "Acer davidii subsp. grosseri", "ACRGO");
            tempTable.AddRow("06029045", "Acer diabolicum", "ACRDI");
            tempTable.AddRow("06029045", "Acer discolor", "ACRDS");
            tempTable.AddRow("06029045", "Acer distylum", "ACRDT");
            tempTable.AddRow("06029045", "Acer divergens", "ACRDV");
            tempTable.AddRow("06029045", "Acer erianthum", "ACRER");
            tempTable.AddRow("06029045", "Acer fabri", "ACRFA");
            tempTable.AddRow("06029045", "Acer fargesii", "ACRFG");
            tempTable.AddRow("06029045", "Acer flabellatum", "ACRFL");
            tempTable.AddRow("06029045", "Acer forrestii", "ACRFO");
            tempTable.AddRow("06029045", "Acer franchetii", "ACRFR");
            tempTable.AddRow("06029045", "Acer glabrum", "ACRGL");
            tempTable.AddRow("06029045", "Acer granatense", "ACRGR");
            tempTable.AddRow("06029045", "Acer griseum", "ACRGS");
            tempTable.AddRow("06029045", "Acer heldreichii", "ACRHE");
            tempTable.AddRow("06029045", "Acer henryi", "ACRHN");
            tempTable.AddRow("06029045", "Acer hypoleucum", "ACRHL");
            tempTable.AddRow("06029045", "Acer hyrcanum", "ACRHR");
            tempTable.AddRow("06029045", "Acer japonicum", "ACRJA");
            tempTable.AddRow("06029045", "Acer laevigatum", "ACRLA");
            tempTable.AddRow("06029045", "Acer laevigatum var. salweense", "ACRLS");
            tempTable.AddRow("06029045", "Acer laurinum", "ACRGA");
            tempTable.AddRow("06029045", "Acer laxiflorum", "ACRLX");
            tempTable.AddRow("06029045", "Acer leucoderme", "ACRLE");
            tempTable.AddRow("06029045", "Acer litseifolium", "ACRLI");
            tempTable.AddRow("06029045", "Acer lobelii", "ACRLB");
            tempTable.AddRow("06029045", "Acer longipes", "ACRLO");
            tempTable.AddRow("06029045", "Acer macrophyllum", "ACRMA");
            tempTable.AddRow("06029045", "Acer mandshuricum", "ACRMN");
            tempTable.AddRow("06029045", "Acer maximowiczii", "ACRMX");
            tempTable.AddRow("06029045", "Acer mayrii", "ACRMY");
            tempTable.AddRow("06029045", "Acer micranthum", "ACRMR");
            tempTable.AddRow("06029045", "Acer miyabei", "ACRMI");
            tempTable.AddRow("06029045", "Acer monspessulanum", "ACRMS");
            tempTable.AddRow("06029045", "Acer monspessulanum subsp. cinerascens", "ACRMC");
            tempTable.AddRow("06029045", "Acer multiserratum", "ACRMU");
            tempTable.AddRow("06029045", "Acer negundo", "ACRNE");
            tempTable.AddRow("06029045", "Acer negundo var. californicum", "ACRNC");
            tempTable.AddRow("06029045", "Acer nikoense", "ACRNK");
            tempTable.AddRow("06029045", "Acer nipponicum", "ACRNP");
            tempTable.AddRow("06029045", "Acer oblongum", "ACROB");
            tempTable.AddRow("06029045", "Acer obtusatum", "ACROT");
            tempTable.AddRow("06029045", "Acer obtusifolium", "ACROF");
            tempTable.AddRow("06029045", "Acer okamotoanum", "ACROK");
            tempTable.AddRow("06029045", "Acer oliverianum", "ACROL");
            tempTable.AddRow("06029045", "Acer opalus", "ACROP");
            tempTable.AddRow("06029045", "Acer osmastonii", "ACROS");
            tempTable.AddRow("06029045", "Acer palmatum", "ACRPA");
            tempTable.AddRow("06029045", "Acer paxii", "ACRPX");
            tempTable.AddRow("06029045", "Acer pectinatum", "ACRPC");
            tempTable.AddRow("06029045", "Acer pensylvanicum", "ACRPE");
            tempTable.AddRow("06029045", "Acer pentapomicum", "ACRPT");
            tempTable.AddRow("06029045", "Acer pictum subsp. mono", "ACRMO");
            tempTable.AddRow("06029045", "Acer pilosum", "ACRPI");
            tempTable.AddRow("06029045", "Acer platanoides", "ACRPL");
            tempTable.AddRow("06029045", "Acer pseudoplatanus", "ACRPP");
            tempTable.AddRow("06029045", "Acer pseudosieboldianum", "ACRPS");
            tempTable.AddRow("06029045", "Acer pubipalmatum", "ACRPB");
            tempTable.AddRow("06029045", "Acer pycnanthum", "ACRPY");
            tempTable.AddRow("06029045", "Acer ramosum", "ACRRA");
            tempTable.AddRow("06029045", "Acer robustum", "ACRRO");
            tempTable.AddRow("06029045", "Acer rubrum", "ACRRB");
            tempTable.AddRow("06029045", "Acer rufinerve", "ACRRU");
            tempTable.AddRow("06029045", "Acer saccharinum", "ACRSA");
            tempTable.AddRow("06029045", "Acer saccharum", "ACRSC");
            tempTable.AddRow("06029045", "Acer saccharum subsp. grandidentatum", "ACRSG");
            tempTable.AddRow("06029045", "Acer saccharum subsp. nigrum", "ACRSN");
            tempTable.AddRow("06029045", "Acer schneiderianum", "ACRSD");
            tempTable.AddRow("06029045", "Acer semenovii", "ACRSM");
            tempTable.AddRow("06029045", "Acer sempervirens", "ACRSV");
            tempTable.AddRow("06029045", "Acer shirasawanum", "ACRSH");
            tempTable.AddRow("06029045", "Acer sieboldianum", "ACRSB");
            tempTable.AddRow("06029045", "Acer sikkimense", "ACRHO");
            tempTable.AddRow("06029045", "Acer sinense", "ACRSI");
            tempTable.AddRow("06029045", "Acer sino-oblongum", "ACRSO");
            tempTable.AddRow("06029045", "Acer sino-purpurascens", "ACRSR");
            tempTable.AddRow("06029045", "Acer sp.", "ACRSS");
            tempTable.AddRow("06029045", "Acer spicatum", "ACRSP");
            tempTable.AddRow("06029045", "Acer stachyophyllum", "ACRST");
            tempTable.AddRow("06029045", "Acer sterculiaceum", "ACRSQ");
            tempTable.AddRow("06029045", "Acer sutchuense", "ACRSU");
            tempTable.AddRow("06029045", "Acer taronense", "ACRTN");
            tempTable.AddRow("06029045", "Acer tataricum", "ACRTA");
            tempTable.AddRow("06029045", "Acer tataricum subsp. ginnala", "ACRGN");
            tempTable.AddRow("06029045", "Acer tegmentosum", "ACRTG");
            tempTable.AddRow("06029045", "Acer thomsonii", "ACRTH");
            tempTable.AddRow("06029045", "Acer tibetense", "ACRTI");
            tempTable.AddRow("06029045", "Acer tonkinense", "ACRTO");
            tempTable.AddRow("06029045", "Acer trautvetteri", "ACRTR");
            tempTable.AddRow("06029045", "Acer triflorum", "ACRTF");
            tempTable.AddRow("06029045", "Acer truncatum", "ACRTU");
            tempTable.AddRow("06029045", "Acer tschonoskii", "ACRTS");
            tempTable.AddRow("06029045", "Acer platanoides subsp. Turkestanicum", "ACRTK");
            tempTable.AddRow("06029045", "Acer tutcheri", "ACRTC");
            tempTable.AddRow("06029045", "Acer velutinum", "ACRVL");
            tempTable.AddRow("06029045", "Acer wardii", "ACRWA");
            tempTable.AddRow("06029045", "Acer wilsonii", "ACRWI");
            tempTable.AddRow("06029045", "Acer x bornmuelleri", "ACRBO");
            tempTable.AddRow("06029045", "Acer x boscii", "ACRBS");
            tempTable.AddRow("06029045", "Acer x coriaceum", "ACRCU");
            tempTable.AddRow("06029045", "Acer x dieckii", "ACRDK");
            tempTable.AddRow("06029045", "Acer x freemanii", "ACRFE");
            tempTable.AddRow("06029045", "Acer x hybridum", "ACRHY");
            tempTable.AddRow("06029045", "Acer x rotundilobum", "ACRRT");
            tempTable.AddRow("06029045", "Acer x senecaense", "ACRSE");
            tempTable.AddRow("06029045", "Acer x sericeum", "ACRSJ");
            tempTable.AddRow("06029045", "Acer x veitchii", "ACRVE");
            tempTable.AddRow("06029045", "Acer x zoeschense", "ACRZO");
            tempTable.AddRow("06029045", "Acer yuii", "ACRYU");
            tempTable.AddRow("06011010", "Bellevalia sp.", "BLVSS");
            tempTable.AddRow("06029045", "Achillea alpina", "ACHSI");
            tempTable.AddRow("06029045", "Achillea borealis", "ACHBO");
            tempTable.AddRow("06029045", "Achillea distans", "ACHDI");
            tempTable.AddRow("06011010", "Bowiea sp.", "BWASS");
            tempTable.AddRow("06029045", "Achillea micrantha", "ACHMC");
            tempTable.AddRow("06011010", "Camassia cusickii", "CDSCU");
            tempTable.AddRow("06029045", "Achillea millefolium var. occidentalis", "ACHLA");
            tempTable.AddRow("06011010", "Dipcadi bakeriana", "DPDBA");
            tempTable.AddRow("06011010", "Dipcadi erythraeum", "DPDER");
            tempTable.AddRow("06029045", "Achillea santolina", "ACHSA");
            tempTable.AddRow("06029045", "Achillea setacea", "ACHSE");
            tempTable.AddRow("06029045", "Achillea sp.", "ACHSS");
            tempTable.AddRow("06011010", "Drimia maritima", "URGMA");
            tempTable.AddRow("06029045", "Achyrocline satureioides", "ACOSA");
            tempTable.AddRow("06029045", "Achyrocline sp.", "ACOSS");
            tempTable.AddRow("06029045", "Acilepis divergens", "VENDV");
            tempTable.AddRow("06029045", "Acis autumnalis", "LEJAU");
            tempTable.AddRow("06029045", "Acmella alba", "SPLOC");
            tempTable.AddRow("06029045", "Acmella brachyglossa", "SPLLI");
            tempTable.AddRow("06029045", "Acmella caulirhiza", "SPLMR");
            tempTable.AddRow("06029045", "Acmella ciliata", "SPLCI");
            tempTable.AddRow("06029045", "Acmella oleracea", "SPLOL");
            tempTable.AddRow("06029045", "Acmella paniculata", "SPLPA");
            tempTable.AddRow("06029045", "Acmopyle pancheri", "ACMPA");
            tempTable.AddRow("06029045", "Acmopyle sp.", "ACMSS");
            tempTable.AddRow("06029045", "Acnistus arborescens", "AKSAR");
            tempTable.AddRow("06029045", "Acnistus sp.", "AKSSS");
            tempTable.AddRow("06029045", "Acoelorraphe sp.", "AEQSS");
            tempTable.AddRow("06029045", "Acokanthera oblongifolia", "CISSP");
            tempTable.AddRow("06029045", "Acokanthera oppositifolia", "CISAK");
            tempTable.AddRow("06029045", "Acradenia frankliniae", "ARNFR");
            tempTable.AddRow("06029045", "Acradenia sp.", "ARNSS");
            tempTable.AddRow("06029045", "Acrocarpus fraxinifolius", "AOCFR");
            tempTable.AddRow("06029045", "Acrocarpus sp.", "AOCSS");
            tempTable.AddRow("06029045", "Acrocomia aculeata", "AARSC");
            tempTable.AddRow("06029045", "Acrocomia sp.", "AARSS");
            tempTable.AddRow("06029045", "Acroptilon repens", "CENRE");
            tempTable.AddRow("06029045", "Acrostichum aureum", "AOHAU");
            tempTable.AddRow("06029045", "Acrostichum sp.", "AOHSS");
            tempTable.AddRow("06029045", "Acrotome inflata", "AFTIN");
            tempTable.AddRow("06029045", "Acrotome sp.", "AFTSS");
            tempTable.AddRow("06029045", "Actaea rubra", "AATSR");
            tempTable.AddRow("06029045", "Actaea sp.", "AATSS");
            tempTable.AddRow("06029045", "Actaea spicata", "AATSP");
            tempTable.AddRow("06029045", "Actinodaphne sp.", "AHDSS");
            tempTable.AddRow("06029045", "Actinostemma lobatum", "ACVLO");
            tempTable.AddRow("06029045", "Actinostemma sp.", "ACVSS");
            tempTable.AddRow("06029045", "Actinostrobus acuminatus", "ACJAC");
            tempTable.AddRow("06029045", "Actinostrobus pyramidalis", "ACJPY");
            tempTable.AddRow("06029045", "Actinostrobus sp.", "ACJSS");
            tempTable.AddRow("06029045", "Adansonia digitata", "AADDI");
            tempTable.AddRow("06029045", "Adansonia sp.", "AADSS");
            tempTable.AddRow("06029045", "Adenandra fragrans", "ADDFR");
            tempTable.AddRow("06029045", "Adenandra sp.", "ADDSS");
            tempTable.AddRow("06029045", "Adenandra umbellata", "ADDUM");
            tempTable.AddRow("06029045", "Adenandra uniflora", "ADDUN");
            tempTable.AddRow("06029045", "Adenanthera pavonina", "ADEPA");
            tempTable.AddRow("06029045", "Adenanthera sp.", "ADESS");
            tempTable.AddRow("06029045", "Adenia cissampeloides", "ADJCI");
            tempTable.AddRow("06029045", "Adenia digitata", "ADJDI");
            tempTable.AddRow("06029045", "Adenia gracilis", "ADJGR");
            tempTable.AddRow("06029045", "Adenia mannii", "ADJMA");
            tempTable.AddRow("06029045", "Adenia rumicifolia", "ADJRU");
            tempTable.AddRow("06029045", "Adenia rumicifolia var. miegei", "ADJRM");
            tempTable.AddRow("06029045", "Adenia rumicifolia var. rumicifolia", "ADJRR");
            tempTable.AddRow("06029045", "Adenia sp.", "ADJSS");
            tempTable.AddRow("06029045", "Adenia staudtii", "ADJST");
            tempTable.AddRow("06029045", "Adenium obesum", "ADFOB");
            tempTable.AddRow("06029045", "Adenium sp.", "ADFSS");
            tempTable.AddRow("06029045", "Adenocalymma alliaceum", "ADNAL");
            tempTable.AddRow("06029045", "Adenocalymma bracteatum", "ADNBR");
            tempTable.AddRow("06029045", "Adenocalymma sp.", "ADNSS");
            tempTable.AddRow("06029045", "Adenocarpus anagyrifolius", "ADCAN");
            tempTable.AddRow("06029045", "Adenocarpus complicatus", "ADCCO");
            tempTable.AddRow("06029045", "Adenocarpus decorticans", "ADCDE");
            tempTable.AddRow("06029045", "Adenocarpus foliosus", "ADCFO");
            tempTable.AddRow("06029045", "Adenocarpus hispanicus", "ADCHI");
            tempTable.AddRow("06029045", "Adenocarpus sp.", "ADCSS");
            tempTable.AddRow("06029045", "Adenocarpus telonensis", "ADCTE");
            tempTable.AddRow("06029045", "Adenocarpus viscosus", "ADCVI");
            tempTable.AddRow("06029045", "Adenocaulon bicolor", "ADKBI");
            tempTable.AddRow("06029045", "Adenocaulon sp.", "ADKSS");
            tempTable.AddRow("06029045", "Adenodolichos rhomboideus", "AOORH");
            tempTable.AddRow("06029045", "Adenodolichos sp.", "AOOSS");
            tempTable.AddRow("06029045", "Adenostemma brasilianum", "AOSBR");
            tempTable.AddRow("06029045", "Adenostemma sp.", "AOSSS");
            tempTable.AddRow("06029045", "Adenostemma viscosum", "AOSLA");
            tempTable.AddRow("06029045", "Adenostoma fasciculatum", "ADSFA");
            tempTable.AddRow("06029045", "Adenostoma sp.", "ADSSS");
            tempTable.AddRow("06029045", "Adenostoma sparsifolium", "ADSSP");
            tempTable.AddRow("06029045", "Adenostyles alliariae", "ADTAL");
            tempTable.AddRow("06029045", "Adenostyles sp.", "ADTSS");
            tempTable.AddRow("06029045", "Adesmia muricata", "AIMMU");
            tempTable.AddRow("06029045", "Adesmia sp.", "AIMSS");
            tempTable.AddRow("06029045", "Adiantum capillus-veneris", "ADICV");
            tempTable.AddRow("06029045", "Adiantum concinnum", "ADICO");
            tempTable.AddRow("06029045", "Adiantum cristatum", "ADICR");
            tempTable.AddRow("06029045", "Adiantum fulvum", "ADIFU");
            tempTable.AddRow("06029045", "Adiantum hispidulum", "ADIHI");
            tempTable.AddRow("06029045", "Adiantum pedatum", "ADIPE");
            tempTable.AddRow("06029045", "Adiantum raddianum", "ADIRA");
            tempTable.AddRow("06029045", "Adiantum sp.", "ADISS");
            tempTable.AddRow("06029045", "Adiantum tenerum", "ADITE");
            tempTable.AddRow("06029045", "Adiantum trapeziforme", "ADITR");
            tempTable.AddRow("06029045", "Adiantum venustum", "ADIVE");
            tempTable.AddRow("06029045", "Adonidia merrillii", "VTHME");
            tempTable.AddRow("06029045", "Adriana acerifola", "ADBAC");
            tempTable.AddRow("06029045", "Adriana sp.", "ADBSS");
            tempTable.AddRow("06029045", "Aechmea candida", "AEMCA");
            tempTable.AddRow("06029045", "Aechmea chantinii", "AEMCH");
            tempTable.AddRow("06029045", "Aechmea fasciata", "AEMFA");
            tempTable.AddRow("06029045", "Aechmea fulgens", "AEMFU");
            tempTable.AddRow("06029045", "Aechmea magdalenae", "AEMMA");
            tempTable.AddRow("06029045", "Aechmea mexicana", "AEMME");
            tempTable.AddRow("06029045", "Aechmea servitensis", "AEMSE");
            tempTable.AddRow("06029045", "Aechmea sp.", "AEMSS");
            tempTable.AddRow("06011010", "Scilla nana", "CIXNA");
            tempTable.AddRow("06011010", "Scilla peruviana", "SLLPE");
            tempTable.AddRow("0709999020", "Abelmoschus esculentus", "ABMES");
            tempTable.AddRow("0810907590", "Ardisia crenata", "ADACN");
            scenarioContext["AllCommodityDetails"] = tempTable;
        }

        Driver.WaitForTransaction();

        var inputTable = scenarioContext["AllCommodityDetails"] as Table;
        inputTable.Should().NotBeNull("AllCommodityDetails was not found in the scenario context.");

        var expectedRowCount = inputTable.Rows.Count;

        // Step 1: Poll until all commodity lines have loaded, refreshing between attempts.
        WaitForCommodityLinesToLoad(expectedRowCount, timeout: TimeSpan.FromMinutes(10));

        // Step 2: Sort the grid by EPPO code ascending so page order matches our in-memory sort.
        SortCommodityLinesByEppoCodeAscending();
        Driver.WaitForTransaction();
        Thread.Sleep(3000);

        // Step 3: Collect all rows across every page, retrying the entire collection if the
        // total falls short. Wijmo virtualises both columns AND rows — a single page may return
        // fewer rows than expected even after ExtractRowsFromCurrentPage retries, because the
        // DOM row count itself can be reduced by vertical virtualisation when the grid is
        // scrolled horizontally. Retrying the full collection from page 1 is the only reliable
        // way to recover from this, since each retry starts fresh after a sort reset.
        List<(string EppoCode, string RegulatoryAuthority)> collectedRows = null;
        var pageNumber = 1;
        const int maxCollectionAttempts = 3;

        for (var collectionAttempt = 1; collectionAttempt <= maxCollectionAttempts; collectionAttempt++)
        {
            collectedRows = new List<(string EppoCode, string RegulatoryAuthority)>();
            pageNumber = 1;

            while (true)
            {
                Driver.WaitForTransaction();
                Thread.Sleep(1500);

                // Ensure both EPPO Code (col 5) and Regulatory Authority (col 11) are in the DOM.
                ScrollGridUntilBothColumnsVisible();

                var pageRows = ExtractRowsFromCurrentPage();
                collectedRows.AddRange(pageRows);

                var nextButtons = Driver.FindElements(By.XPath("//button[contains(@id,'_nextPage')]"));

                if (!nextButtons.Any() || nextButtons[0].GetAttribute("disabled") != null)
                {
                    break;
                }

                Driver.ExecuteScript("arguments[0].click();", nextButtons[0]);
                pageNumber++;
            }

            if (collectedRows.Count >= expectedRowCount)
            {
                break;
            }

            Console.WriteLine(
                $"[COLLECT ROWS] Attempt {collectionAttempt}/{maxCollectionAttempts}: " +
                $"collected {collectedRows.Count} of {expectedRowCount} rows across {pageNumber} page(s). " +
                $"Refreshing and retrying from page 1.");

            // Refresh to reset pagination back to page 1, then re-sort before the next attempt.
            CommandSteps.WhenISelectTheCommand("Refresh");
            Driver.WaitForTransaction();
            SortCommodityLinesByEppoCodeAscending();
            Driver.WaitForTransaction();
            Thread.Sleep(3000);
        }

        // Step 4: Sort expected rows the same way as the grid.
        var sortedExpected = inputTable.Rows
            .OrderBy(r => r["EPPO code"])
            .ToList();

        // Step 5: Assert total count.
        collectedRows.Count.Should().Be(sortedExpected.Count,
            $"Expected {sortedExpected.Count} commodity lines but collected {collectedRows.Count} " +
            $"across {pageNumber} page(s) after {maxCollectionAttempts} collection attempt(s).");

        // Step 6: Validate each row's EPPO code and Regulatory Authority.
        for (var i = 0; i < sortedExpected.Count; i++)
        {
            var expectedEppo = sortedExpected[i]["EPPO code"].Trim();
            var actualEppo = collectedRows[i].EppoCode;

            actualEppo.Should().Be(expectedEppo,
                $"Row {i + 1}: Expected EPPO code '{expectedEppo}' but found '{actualEppo}'.");

            collectedRows[i].RegulatoryAuthority.Should().BeOneOf(ValidRegulatoryAuthorities,
                $"Row {i + 1} (EPPO: '{actualEppo}'): Regulatory Authority should be one of the valid values.");
        }
    }

    /// <summary>
    /// Extracts EPPO code and Regulatory Authority from every data row on the currently visible
    /// grid page. Uses the pagination label as the expected row count where available, falling
    /// back to the DOM row count. Resets vertical scroll to 0 on each retry to counteract
    /// Wijmo row virtualisation caused by a non-zero scrollTop.
    /// </summary>
    private List<(string EppoCode, string RegulatoryAuthority)> ExtractRowsFromCurrentPage()
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            try
            {
                var grid = Driver.WaitUntilAvailable(
                    By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                    "Commodity Lines grid could not be found while extracting rows.");

                var rows = grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));
                var domRowCount = rows.Count;

                if (domRowCount == 0)
                {
                    Thread.Sleep(1000);
                    Driver.WaitForTransaction();
                    continue;
                }

                // Prefer the pagination label count as ground truth over the DOM row count.
                // The DOM count can be reduced by vertical virtualisation; the label always
                // reflects the true page size regardless of what Wijmo has rendered.
                var expectedOnPage = TryReadExpectedRowsOnCurrentPage() ?? domRowCount;

                var results = new List<(string EppoCode, string RegulatoryAuthority)>();

                foreach (var row in rows)
                {
                    var eppoSpan = row.FindElements(
                        By.XPath(".//div[@role='gridcell'][@aria-colindex='5']//span[@role='presentation']"))
                        .FirstOrDefault();

                    var regAuthSpan = row.FindElements(
                        By.XPath(".//div[@role='gridcell'][@aria-colindex='11']//span[@role='presentation']"))
                        .FirstOrDefault();

                    if (eppoSpan != null && regAuthSpan != null)
                    {
                        results.Add((eppoSpan.Text.Trim(), regAuthSpan.Text.Trim()));
                    }
                }

                if (results.Count < expectedOnPage)
                {
                    Console.WriteLine(
                        $"[EXTRACT ROWS] Attempt {attempt + 1}: collected {results.Count} of " +
                        $"{expectedOnPage} expected rows (DOM count: {domRowCount}). " +
                        $"Re-scrolling and resetting vertical scroll before retrying.");

                    ScrollGridUntilBothColumnsVisible();
                    Driver.WaitForTransaction();
                    Thread.Sleep(500);
                    continue;
                }

                return results;
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(1000);
                Driver.WaitForTransaction();
            }
        }

        return new List<(string EppoCode, string RegulatoryAuthority)>();
    }

    /// <summary>
    /// Counts the total number of commodity line data rows across all pages
    /// without collecting cell values, purely to check whether loading is complete.
    /// Leaves the grid on the last visited page.
    /// </summary>
    private int CountCommodityLinesAcrossAllPages()
    {
        var total = 0;

        while (true)
        {
            Driver.WaitForTransaction();

            var grid = Driver.WaitUntilAvailable(
                By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                "Import Commodity Lines grid could not be found while counting rows.");

            // No horizontal scroll occurs here, so the Wijmo frozen column panel (wj-part='fcells')
            // is not rendered — the plain row query is safe and correct in this context.
            var dataRows = grid.FindElements(
                By.XPath(".//div[@role='row'][@aria-label='Data']"));

            total += dataRows.Count;

            var nextButton = Driver.WaitUntilAvailable(
                By.XPath("//button[contains(@id,'_nextPage')]"),
                "Pagination next button could not be found while counting rows.");

            if (nextButton.GetAttribute("disabled") != null)
            {
                break;
            }

            // Scroll the button into view and click via JavaScript to avoid
            // ElementClickInterceptedException when an overlay div covers the button.
            Driver.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", nextButton);
            Driver.WaitForTransaction();
            Driver.ExecuteScript("arguments[0].click();", nextButton);
        }

        return total;
    }

    /// <summary>
    /// Polls the Import Commodity Lines grid by counting rows across all pages,
    /// refreshing the form between attempts, until the expected count is reached
    /// or the timeout expires. Always refreshes on exit to reset the grid to page 1,
    /// since <see cref="CountCommodityLinesAcrossAllPages"/> leaves the grid on the
    /// last page after paginating through all pages.
    /// </summary>
    /// <param name="expectedRowCount">The total number of commodity lines expected.</param>
    /// <param name="timeout">How long to keep retrying before giving up.</param>
    private void WaitForCommodityLinesToLoad(int expectedRowCount, TimeSpan timeout)
    {
        var deadline = DateTime.UtcNow.Add(timeout);

        while (DateTime.UtcNow < deadline)
        {
            var currentCount = CountCommodityLinesAcrossAllPages();

            // Always refresh after counting — resets the grid to page 1 regardless of
            // whether the count was met, so callers always start from a known position.
            CommandSteps.WhenISelectTheCommand("Refresh");
            Driver.WaitForTransaction();

            if (currentCount >= expectedRowCount)
            {
                return;
            }
        }

        // Final count after timeout — let the assertion produce a clear failure message.
        var finalCount = CountCommodityLinesAcrossAllPages();
        CommandSteps.WhenISelectTheCommand("Refresh");
        Driver.WaitForTransaction();

        finalCount.Should().BeGreaterOrEqualTo(expectedRowCount,
            $"Timed out after {timeout.TotalMinutes} minutes waiting for {expectedRowCount} " +
            $"commodity lines to load. Final count across all pages was {finalCount}.");
    }
    
    private void SortCommodityLinesByEppoCodeAscending()
    {
        var eppoColumnHeaderButton = Driver.WaitUntilAvailable(
            By.XPath($"//div[contains(@id,'_headerButton{EppoCodeColumnDataId}')]"),
            "EPPO Code column header button could not be found.");

        eppoColumnHeaderButton.Click();
        Driver.WaitForTransaction();

        var sortAtoZMenuItem = Driver.WaitUntilAvailable(
            By.XPath("//button[@name='Sort A to Z' and @role='menuitemradio']"),
            "Sort A to Z menu item could not be found.");

        Driver.ExecuteScript("arguments[0].click();", sortAtoZMenuItem);
        Driver.WaitForTransaction();
    }

    private void ScrollGridUntilBothColumnsVisible()
    {
        const string findScrollRootAndCheckScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return null;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (!scrollRoot) return null;
            var eppoVisible = grid.querySelector('div[role=""gridcell""][aria-colindex=""5""]') !== null;
            var regAuthVisible = grid.querySelector('div[role=""gridcell""][aria-colindex=""11""]') !== null;
            return JSON.stringify({
                scrollLeft: scrollRoot.scrollLeft,
                scrollWidth: scrollRoot.scrollWidth,
                clientWidth: scrollRoot.clientWidth,
                eppoVisible: eppoVisible,
                regAuthVisible: regAuthVisible
            });";

        const string scrollIncrementScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollLeft += 300; }";

        // After achieving horizontal visibility, reset vertical scroll to 0 so Wijmo renders
        // rows from the top of the page. Without this reset, a non-zero scrollTop (caused by
        // prior horizontal scrolling or page navigation) clips the bottom row out of the
        // viewport — Wijmo de-virtualises it, reducing DOM row count from 50 to 49 on every
        // attempt, including outer collection retries.
        const string resetVerticalScrollScript = @"
            var grid = document.querySelector('div[role=""grid""][aria-label*=""Import Commodity Lines""]');
            if (!grid) return;
            var scrollRoot = grid.closest('[wj-part=""root""]') || grid.parentElement;
            if (scrollRoot) { scrollRoot.scrollTop = 0; }";

        const int gridReadyAttempts = 20;
        const int maxScrollAttempts = 40;

        string resultJson = null;
        for (var waitAttempt = 0; waitAttempt < gridReadyAttempts; waitAttempt++)
        {
            resultJson = Driver.ExecuteScript(findScrollRootAndCheckScript) as string;

            if (resultJson != null)
            {
                break;
            }

            Driver.WaitForTransaction();
        }

        if (resultJson == null)
        {
            throw new InvalidOperationException(
                $"The Import Commodity Lines grid scroll container could not be found after waiting " +
                $"{gridReadyAttempts} attempts. The grid may not have finished rendering.");
        }

        for (var scrollAttempt = 0; scrollAttempt < maxScrollAttempts; scrollAttempt++)
        {
            var eppoVisible = resultJson.Contains(@"""eppoVisible"":true");
            var regAuthVisible = resultJson.Contains(@"""regAuthVisible"":true");

            if (eppoVisible && regAuthVisible)
            {
                // Both columns are in the DOM. Reset vertical scroll to 0 so Wijmo renders
                // rows starting from the top of the page — prevents the bottom row being
                // clipped by a non-zero scrollTop accumulated during horizontal scrolling.
                Driver.ExecuteScript(resetVerticalScrollScript);
                Driver.WaitForTransaction();
                return;
            }

            Driver.ExecuteScript(scrollIncrementScript);
            Driver.WaitForTransaction();

            resultJson = Driver.ExecuteScript(findScrollRootAndCheckScript) as string
                ?? throw new InvalidOperationException(
                    "The Import Commodity Lines grid scroll container disappeared during scrolling.");
        }

        throw new InvalidOperationException(
            $"Could not scroll the Commodity Lines grid to show both EPPO Code (col 5) and " +
            $"Regulatory Authority (col 11) simultaneously after {maxScrollAttempts} scroll attempts. " +
            $"Consider reviewing the column layout or increasing the max attempts.");
    }

    /// <summary>
    /// Reads the expected number of data rows on the current grid page from the pagination
    /// label (e.g. "1 - 50 of 500"). Returns null if the label cannot be parsed, in which
    /// case callers should fall back to the DOM row count.
    /// </summary>
    private int? TryReadExpectedRowsOnCurrentPage()
    {
        // Dynamics renders the pagination range as a label like "1 - 50 of 500".
        // The containing element varies by grid type; search broadly by text pattern.
        var paginationLabels = Driver.FindElements(
            By.XPath("//*[contains(@aria-label,'of') and contains(@aria-label,' - ')]"));

        foreach (var label in paginationLabels)
        {
            var text = (label.GetAttribute("aria-label") ?? label.Text ?? string.Empty).Trim();

            // Expected format: "X - Y of Z"
            var match = System.Text.RegularExpressions.Regex.Match(
                text, @"(\d+)\s*-\s*(\d+)\s+of\s+(\d+)");

            if (match.Success &&
                int.TryParse(match.Groups[1].Value, out var from) &&
                int.TryParse(match.Groups[2].Value, out var to))
            {
                return to - from + 1;
            }
        }

        return null;
    }

    [When(@"I sort Commodity Lines by Regulatory Authority")]
    public void WhenISortCommodityLinesByRegulatoryAuthority()
    {
        ScrollGridUntilBothColumnsVisible();
        Driver.WaitForTransaction();

        var regulatoryAuthorityHeaderButton = Driver.WaitUntilAvailable(
            By.XPath($"//div[contains(@id,'_headerButton{RegulatoryAuthorityColumnDataId}')]"),
            "Regulatory Authority column header button could not be found.");

        regulatoryAuthorityHeaderButton.Click();
        Driver.WaitForTransaction();

        var sortAtoZMenuItem = Driver.WaitUntilAvailable(
            By.XPath("//button[@name='Sort A to Z' and @role='menuitemradio']"),
            "Sort A to Z menu item could not be found.");

        sortAtoZMenuItem.Click();
        Driver.WaitForTransaction();
    }

    [When(@"I double click on a Commodity Line with Regulatory Authority set to '(.*)'")]
    public void WhenIDoubleClickOnACommodityLineWithRegulatoryAuthoritySetTo(string regulatoryAuthority)
    {
        ScrollGridUntilBothColumnsVisible();
        Driver.WaitForTransaction();

        var isAgGrid = Driver.FindElements(
            By.XPath("//div[@id='dataSetRoot_Import_notification_commodity_lines_subgrid']")).Count > 0;

        if (isAgGrid)
        {
            var matchingIndex = -1;

            for (var attempt = 0; attempt < 5; attempt++)
            {
                try
                {
                    var grid = Driver.WaitUntilAvailable(
                        By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                        "Commodity Lines grid could not be found.");

                    var dataRows = grid.FindElements(By.XPath(".//div[@role='row'][@aria-label='Data']"));

                    for (var i = 0; i < dataRows.Count; i++)
                    {
                        var regulatoryCells = dataRows[i].FindElements(
                            By.XPath(".//div[@role='gridcell'][@aria-colindex='11']//span[@role='presentation']"));

                        if (regulatoryCells.Count > 0 &&
                            regulatoryCells[0].Text.Trim().Equals(regulatoryAuthority, StringComparison.OrdinalIgnoreCase))
                        {
                            matchingIndex = i;
                            break;
                        }
                    }

                    break;
                }
                catch (StaleElementReferenceException)
                {
                    Driver.WaitForTransaction();
                    Thread.Sleep(500);
                }
            }

            matchingIndex.Should().BeGreaterOrEqualTo(0,
                $"No Commodity Line with Regulatory Authority '{regulatoryAuthority}' could be found on the current page.");

            XrmApp.Entity.SubGrid.OpenSubGridRecord("Import_notification_commodity_lines_subgrid", matchingIndex);
        }
        else
        {
            // For the Wijmo cc-grid, re-query the matching cell and double-click within a retry
            // loop. The StaleElementReferenceException occurs because the grid re-renders after
            // the sort completes — cells found during ScrollGridUntilBothColumnsVisible are
            // detached from the DOM by the time the predicate reads their text. Re-finding the
            // cell on each attempt avoids referencing a detached element.
            for (var attempt = 0; attempt < 5; attempt++)
            {
                try
                {
                    var grid = Driver.WaitUntilAvailable(
                        By.XPath("//div[@role='grid'][contains(@aria-label,'Import Commodity Lines')]"),
                        "Commodity Lines grid could not be found.");

                    // Re-find all candidate cells on every attempt — never re-use a reference
                    // from a previous attempt as the DOM may have been replaced by a sort or
                    // page transition between the collection and the predicate evaluation.
                    var candidateCells = grid.FindElements(
                        By.XPath(".//div[@role='row'][@aria-label='Data']" +
                                 "//div[@role='gridcell'][@aria-colindex='11']" +
                                 "[.//span[@role='presentation']]"));

                    IWebElement matchingCell = null;

                    foreach (var cell in candidateCells)
                    {
                        // Read text inside a try/catch — an individual cell may go stale
                        // mid-iteration if Wijmo re-virtualises rows during the loop.
                        try
                        {
                            var cellText = cell.FindElement(
                                By.XPath(".//span[@role='presentation']")).Text.Trim();

                            if (cellText.Equals(regulatoryAuthority, StringComparison.OrdinalIgnoreCase))
                            {
                                matchingCell = cell;
                                break;
                            }
                        }
                        catch (StaleElementReferenceException)
                        {
                            // This cell went stale mid-read — break and retry the whole query.
                            matchingCell = null;
                            break;
                        }
                    }

                    if (matchingCell == null)
                    {
                        Driver.WaitForTransaction();
                        Thread.Sleep(500);
                        continue;
                    }

                    matchingCell.Should().NotBeNull(
                        $"No Commodity Line with Regulatory Authority '{regulatoryAuthority}' could be found on the current page.");

                    new Actions(Driver).DoubleClick(matchingCell).Perform();
                    Driver.WaitForTransaction();
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    Driver.WaitForTransaction();
                    Thread.Sleep(500);
                }
            }

            throw new InvalidOperationException(
                $"Could not double-click a Commodity Line with Regulatory Authority '{regulatoryAuthority}' " +
                $"after 5 attempts due to repeated StaleElementReferenceException.");
        }

        Driver.WaitForTransaction();
    }

    /// <summary>
    /// Asserts that the specified Work Order Task in the workorderservicetasksgrid has the expected Status.
    /// </summary>
    /// <param name="taskName">The name of the Work Order Task.</param>
    /// <param name="expectedStatus">The expected Status value (e.g., "Active", "Inactive").</param>
    [Then(@"the Work Order Task '(.*)' Status is '(.*)'")]
    public void ThenTheWorkOrderTaskStatusIs(string taskName, string expectedStatus)
    {
        Driver.WaitForTransaction();

        // Find the grid container for Work Order Service Tasks
        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Find all rows in the AG Grid
        var rows = grid.FindElements(By.XPath(".//div[contains(@class,'ag-center-cols-container')]/div[@role='row']"));
        rows.Should().NotBeEmpty("Expected at least one row in the Work Order Service Tasks grid.");

        // Find the row where the Name column (aria-colindex='2') matches the taskName
        IWebElement matchingRow = null;
        foreach (var row in rows)
        {
            var nameCell = row.FindElements(By.XPath(".//div[@role='gridcell'][@aria-colindex='2']//span[@role='presentation']"))
                .FirstOrDefault();
            if (nameCell != null && nameCell.Text.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull($"Could not find a Work Order Task row with Name '{taskName}'.");

        // Status is in aria-colindex='4'
        var statusCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='4']//label[@aria-label]")
        );

        var actualStatus = statusCell.GetAttribute("aria-label").Trim();

        actualStatus.Should().Be(expectedStatus,
            $"Expected Status for Work Order Task '{taskName}' to be '{expectedStatus}' but found '{actualStatus}'.");
    }

    /// <summary>
    /// Asserts that the specified Work Order Task in the workorderservicetasksgrid has the expected % Complete value.
    /// </summary>
    /// <param name="taskName">The name of the Work Order Task.</param>
    /// <param name="expectedPercentComplete">The expected % Complete value (e.g., "100.00").</param>
    [Then(@"the Work Order Task '(.*)' % Complete is '(.*)'")]
    public void ThenTheWorkOrderTaskPercentCompleteIs(string taskName, string expectedPercentComplete)
    {
        Driver.WaitForTransaction();

        // Find the grid container for Work Order Service Tasks
        var grid = Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='dataSetRoot_workorderservicetasksgrid']"),
            "Work Order Service Tasks grid could not be found.");

        // Find all rows in the AG Grid
        var rows = grid.FindElements(By.XPath(".//div[contains(@class,'ag-center-cols-container')]/div[@role='row']"));
        rows.Should().NotBeEmpty("Expected at least one row in the Work Order Service Tasks grid.");

        // Find the row where the Name column (aria-colindex='2') matches the taskName
        IWebElement matchingRow = null;
        foreach (var row in rows)
        {
            var nameCell = row.FindElements(By.XPath(".//div[@role='gridcell'][@aria-colindex='2']//span[@role='presentation']"))
                .FirstOrDefault();
            if (nameCell != null && nameCell.Text.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase))
            {
                matchingRow = row;
                break;
            }
        }

        matchingRow.Should().NotBeNull($"Could not find a Work Order Task row with Name '{taskName}'.");

        // % Complete is in aria-colindex='6'
        var percentCell = matchingRow.FindElement(
            By.XPath(".//div[@role='gridcell'][@aria-colindex='6']//label[@aria-label]")
        );

        var actualPercent = percentCell.GetAttribute("aria-label").Trim();

        actualPercent.Should().Be(expectedPercentComplete,
            $"Expected % Complete for Work Order Task '{taskName}' to be '{expectedPercentComplete}' but found '{actualPercent}'.");
    }

    /// <summary>
    /// Opens the Related tab dropdown and selects the specified item by scrolling it into view
    /// within the flyout before clicking. Required because the flyout container has a fixed
    /// max-height with overflow-y:auto — items below the fold (e.g. 'Charges') are present in
    /// the DOM but not clickable until scrolled into view.
    /// </summary>
    /// <param name="relatedTabName">The aria-label of the Related menu item e.g. 'Charges'.</param>
    [When(@"I select the '(.*)' tab from the Related tab dropdown")]
    public void WhenISelectTheTabFromTheRelatedTabDropdown(string relatedTabName)
    {
        Driver.WaitForTransaction();

        // Locate the Related tab in the tablist. aria-expanded indicates whether the flyout is open.
        var relatedTab = Driver.WaitUntilAvailable(
            By.XPath("//ul[@role='tablist']//li[@role='tab' and @aria-haspopup='true' and @aria-label='Related']"),
            "Related tab could not be found in the tablist.");

        // Open the flyout only if it is not already expanded.
        var isExpanded = relatedTab.GetAttribute("aria-expanded");
        if (!string.Equals(isExpanded, "true", StringComparison.OrdinalIgnoreCase))
        {
            relatedTab.Click();
            Driver.WaitForTransaction();

            // Wait until aria-expanded flips to "true" before proceeding.
            Driver.WaitUntilAvailable(
                By.XPath("//ul[@role='tablist']//li[@role='tab' and @aria-haspopup='true' and @aria-label='Related' and @aria-expanded='true']"),
                "Related tab flyout did not open (aria-expanded did not become 'true').");
        }

        // Wait for the flyout container to be present and visible.
        Driver.WaitUntilAvailable(
            By.XPath("//div[@data-id='relatedTabMenuList']"),
            $"Related tab flyout menu could not be found when selecting '{relatedTabName}'.");

        // Find the menu item by its aria-label inside the flyout.
        var menuItem = Driver.WaitUntilAvailable(
            By.XPath($"//div[@data-id='relatedTabMenuList']//div[@role='menuitem' and @aria-label='{relatedTabName}']"),
            $"Related tab menu item '{relatedTabName}' could not be found in the flyout.");

        // Scroll the item into view within the flyout — the container has a fixed max-height
        // with overflow-y:auto so items below the fold are not clickable without scrolling.
        Driver.ExecuteScript("arguments[0].scrollIntoView({block:'nearest'});", menuItem);
        Driver.WaitForTransaction();

        menuItem.Click();
        Driver.WaitForTransaction();
    }

    [Given(@"'(.*)' has updated the status of the work order associated to '(.*)' to '(.*)'")]
    public void GivenHasUpdatedTheStatusOfTheWorkOrderAssociatedToTo(string userAlias, string applicationAlias, string subStatusName)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);
        var user = TestConfig.GetUser(userAlias);

        using (var svcClient = SessionContext.GetServiceClient())
        using (var context = new PlantsContext(svcClient))
        {
            var substatus = context.msdyn_workordersubstatusSet.Where(s => s.msdyn_name == subStatusName).FirstOrDefault();
            if (substatus == null)
            {
                throw new ArgumentException($"Unable to find a work order sub status of {subStatusName}.");
            }

            var userObjectId = context.SystemUserSet
                .Where(u => u.DomainName == user.Username)
                .Select(u => u.AzureActiveDirectoryObjectId)
                .FirstOrDefault();

            if (!userObjectId.HasValue)
            {
                throw new ArgumentException($"Unable to find a system user with a domain name of {user.Username}.");
            }

            var workOrder = GetWorkOrder(svcClient, application);
            WaitForWorkServiceTasks(svcClient, workOrder);
            svcClient.CallerAADObjectId = userObjectId.Value;
            svcClient.Update(new msdyn_workorder { Id = workOrder.Id, msdyn_SubStatus = substatus.ToEntityReference() });
        }
    }

    [Given(@"the work order associated to '(.*)' is assigned to me")]
    public void GivenTheWorkOrderAssociatedToTheIsAssignedToMe(string applicationAlias)
    {
        var application = TestDriver.GetTestRecordReference(applicationAlias);

        using (var svcClient = SessionContext.GetServiceClient())
        {
            var workOrder = GetWorkOrder(svcClient, application);
            WaitForWorkServiceTasks(svcClient, workOrder);
            svcClient.AssignEntityToUser(this.sessionContext.UserId, msdyn_workorder.EntityLogicalName, workOrder.Id);
        }

        new AsyncSteps(this.sessionContext).WhenIWaitForAllServiceTasksToBeOwned(applicationAlias, applicationAlias.ToLower().Contains("import") ? SpecflowBindingsConstants.DefaultTaskCountOwnedByCITImportsTeam : 0);
    }

    [When(@"I select the '(.*)' tab on the work order task")]
    public void WhenISelectTheTabOnTheWorkOrderTask(string tabName)
    {
        this.ClickTab(tabName);
    }

    [When(@"I select inspector at position '(.*)' after using search criteria '(.*)'")]
    public void WhenISelectInspectorAtPositionAfterUsingSearchCriteria(int zeroBasedIndex, string searchCriteria)
    {
        this.SelectInspector(zeroBasedIndex, searchCriteria);
    }

    [When(@"I scroll into time recording inspectors")]
    public void WhenIScrollIntoTimeRecordingInspectors()
    {
        this.ScrollIntoElement();
    }

    [When(@"I refresh the time recordings until the '(.*)' grid contains '(.*)' row\(s\)")]
    public void WhenIRefreshTheTimeRecordingsUntilTheGridContainsRows(string gridName, int expectedRowCount)
    {
        if (this.RefreshCommands.Count.Equals(2))
        {
            Wait.Until(
                TimeSpan.FromSeconds(15),
                () => GridHelper.GetRows(Driver, gridName).Count.Equals(expectedRowCount),
                () => this.RefreshCommands[1].Click());
        }
        else
        {
            throw new AutomationException("Expected 2 refresh commands in work order task");
        }
    }

    [When(@"I save the work order task")]
    public void WhenISaveTheWorkOrderTask()
    {
        this.SaveWorkOrder();
    }

    [When(@"I assign workorder to myself")]
    public void WhenIAssignWorkorderToMyself()
    {
        new DataSteps(this.sessionContext).GivenIHaveOpened("WorkOrder");
        this.WhenIAssignTheWorkOrderToMe();
    }

    [Then(@"I can see the workorder status as '(.*)'")]
    public void ThenICanSeeTheWorkorderStatusAs(string status)
    {
        Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(5))
                .Execute(() =>
                {
                    new DataSteps(this.sessionContext).GivenIHaveOpened("WorkOrder");
                    ThenThereIsAValueOfInTheLookupHeaderField(status, "msdyn_substatus");
                });
    }

    [Given(@"I assign the work order to me")]
    [When(@"I assign the work order to me")]
    public void WhenIAssignTheWorkOrderToMe()
    {
        var formContext = Driver.WaitUntilAvailable(By.Id("mainContent"));
        CommandSteps.ClickCommand("Refresh");
        Driver.WaitForPageToLoad();
        CommandHelper.ClickCommand(Driver, formContext, "Assign");
        DialogSteps.WhenIAssignToMeOnTheAssignDialog();
    }

    [When(@"I close the work order task")]
    public void WhenICloseTheWorkOrderTask()
    {
        this.ClosePopupContainer.Click();
    }

    // TODO: Review if the binding in Capgemini.PowerApps.SpecFlowBindings doesn't work. If so, raise an issue on the repository.
    [Then(@"there is a value of '(.*)' in the '(.*)' lookup header field")]
    public void ThenThereIsAValueOfInTheLookupHeaderField(string expectedHeaderValue, string headerName)
    {
        Driver.WaitForTransaction();

        var headerValue = this.GetHeaderValue(headerName);

        headerValue.Should().Be(expectedHeaderValue);
    }

    [Then(@"there is a value in the '(.*)' lookup header field that does not contain (.*)")]
    public void ThenThereIsADifferentValueOfInTheLookupHeaderField(string headerName, string notMatching)
    {
        Driver.WaitForTransaction();

        var headerValue = this.GetHeaderValue(headerName);

        headerValue.Should().NotContain(notMatching);
    }

    [When(@"I click cancel on the assign work order dialog")]
    public void WhenIClickCancelOnTheAssignWorkOrderDialog()
    {
        this.ClickCancel();
    }

    [When(@"I select the '(.*)' tab on the popup dialog")]
    public void WhenITabOnTheWorkOrderPopupDialogTab(string popTabName)
    {
        this.ClickWorkOrderTab(popTabName);
    }

    [When(@"I click on '(.*)' button")]
    public void WhenIClickOnButtonOnTheTable(string buttonName)
    {
        this.ClickButtonOnTable(buttonName);
    }


    [Then(@"a new window with the phyto url field value is opened")]
    public void ThenANewWindowWithThePhytoUrlFieldValueIsOpened()
    {
        var expectedURL = this.GetPhytoURLValue();
        Driver.LastWindow();
        var actualURL = Driver.Url;
        actualURL.Should().Contain(expectedURL);
    }

    [Then(@"I open and view the phyto certificate")]
    public void ThenIOpenAndViewThePhytoCertificate()
    {
        PopupSteps.WhenICloseThePopup();
        CommandSteps.WhenISelectTheCommand("View Phyto");
        ThenANewWindowWithThePhytoUrlFieldValueIsOpened();
    }

    /// <summary>
    /// Gets the cancel button for the assign dialog.
    /// </summary>
    public IWebElement CancelButton => Driver.WaitUntilAvailable(By.XPath("//button[@data-id='cancel_id']"));

    /// <summary>
    /// Clicks the cancel button on the assign dialog.
    /// </summary>
    public void ClickCancel()
    {
        this.CancelButton.Click();
    }

    public IWebElement HeaderFieldsExpand => Driver.WaitUntilAvailable(By.XPath("//button[@data-id='header_overflowButton']"));

    public IWebElement SupportingDocumentURL => Driver.WaitUntilAvailable(By.XPath("//*[contains(@data-id,'trd_documentsviewurl.fieldControl-url-action-icon')]"));

    public IWebElement PhytoURL => Driver.WaitUntilAvailable(By.XPath(".//*[@data-id='trd_phytourl.fieldControl-text-box-text']"));

    public string GetHeaderValue(string headerName)
    {
        this.HeaderFieldsExpand.Click();
        var header = Driver.WaitUntilAvailable(By.XPath($"//ul[@data-id='header_{headerName}.fieldControl-LookupResultsDropdown_{headerName}_SelectedRecordList']"));
        var headerValue = header.Text;
        this.HeaderFieldsExpand.Click();
        return headerValue;
    }

    public void ClickWorkOrderTab(string popTabName)
    {
        var tab = Driver.WaitUntilAvailable(By.XPath($"//li[@data-id='tablist-tab_{popTabName}']"));
        tab.Click();
    }

    public void ClickButtonOnTable(string buttonName)
    {
        Driver.WaitUntilAvailable(By.XPath($"//button[@title='{buttonName}' or @aria-label='{buttonName}']")).Click();
    }

    public string GetPhytoURLValue()
    {
        return this.PhytoURL.GetAttribute("value");
    }

    public IWebElement PopupContainer => Driver.WaitUntilAvailable(By.XPath("//section[contains(@id, 'popupContainer')]"));

    public IWebElement ClosePopupContainer => this.PopupContainer.FindElement(By.XPath(".//button[@data-id='dialogCloseIconButton']"));

    public IWebElement Save => this.PopupContainer.FindElement(By.XPath(".//span[@aria-label='Save']"));

    public IWebElement Search => this.PopupContainer.FindElement(By.XPath(".//div[@class='   css-1wa3eu0-placeholder']"));

    public IWebElement Close => this.PopupContainer.FindElement(By.XPath(".//span[contains (@class, 'ms-Button-label') and contains(string(), 'Close')]"));

    public IList<IWebElement> RefreshCommands => this.PopupContainer.FindElements(By.XPath(".//span[@aria-label='Refresh']"));

    public IList<IWebElement> InspectorGridCells => this.PopupContainer.FindElements(By.XPath(".//div[@data-automationid='DetailsRowCell']"));

    public IWebElement ScrollTimeRecording => this.PopupContainer.FindElement(By.XPath(".//label[contains(string(), 'Time Recording Inspectors')]"));

    public void ClickTab(string tabName)
    {
        Driver.WaitForTransaction();
        var tab = Driver.FindElement(By.XPath($"//li[@title='{tabName}' and contains(@role, 'tab')]"));
        tab.Click();
        Driver.WaitForTransaction();
    }

    public void SelectInspector(int zeroBasedIndex, string searchCriteria)
    {
        this.Search.Click();
        this.Search.InputText(searchCriteria);
        Wait.Until(TimeSpan.FromSeconds(10), () => this.InspectorGridCells.Count > 0);
        this.InspectorGridCells[zeroBasedIndex].Click();
        this.SaveWorkOrder();
    }

    public void SaveWorkOrder()
    {
        this.Save.Click();
    }

    public void ScrollIntoElement()
    {
        Driver.ExecuteScript("arguments[0].scrollIntoView(true);", this.ScrollTimeRecording);
    }

    public static EntityReference GetWorkOrder(CrmServiceClient svcClient, EntityReference application)
    {
        return svcClient.WaitForFieldValue<EntityReference>(application.LogicalName, application.Id, "trd_workorderid", TimeSpan.FromSeconds(5));
    }

    public static EntityCollection WaitForWorkServiceTasks(CrmServiceClient svcClient, EntityReference workOrder)
    {
        return svcClient.WaitForRecords(
            new QueryByAttribute(msdyn_workorderservicetask.EntityLogicalName)
            {
                Attributes = { "msdyn_workorder" },
                Values = { workOrder.Id }
            },
            TimeSpan.FromSeconds(90));
    }

    [Then(@"I can view the following Business process stages")]
    public void ThenICanViewTheFollowingBusinessProcessStages(Table table)
    {
        foreach (var row in table.Rows)
        {
            Driver.FindElement(By.XPath("//*[@title='" + row["Stages"] + "']"));
        }
    }

    /// <summary>
    /// Opens work-order for the current application. Assuming that there will be only one work-order for the given test.
    /// </summary>
    public void OpenWorkOrder()
    {
        ApplicationSteps.OpenEntity(this.sessionContext.GetEntityReference(SpecflowBindingsConstants.WorkOrderAlias));
    }

    public void AddWorkOrderToEntityHolder(EntityReference workOrder)
    {
        if (!this.sessionContext.Entities.ContainsKey(SpecflowBindingsConstants.WorkOrderAlias + this.sessionContext.SessionId))
        {
            this.sessionContext.Entities.Add($"{SpecflowBindingsConstants.WorkOrderAlias}{this.sessionContext.SessionId}",
                new EntityHolder
                {
                    Alias = SpecflowBindingsConstants.WorkOrderAlias,
                    EntityName = workOrder.LogicalName,
                    EntityCollectionName = workOrder.LogicalName + "s",
                    Id = workOrder.Id,
                });
        }
    }


    [When(@"I navigate to the service tasks grid on the work order")]
    public void WhenINavigateToTheServiceTasksGridOnTheWorkorder()
    {
        FormSteps.StoreFormValueInVariable("trd_workorderid", "Lookup", "field", "plntworkorderid", this.sessionContext);
        Capgemini.PowerApps.SpecFlowBindings.Steps.LookupSteps.WhenISelectARelatedLookupInTheForm("trd_workorderid");
        Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySteps.ISelectTab("Work Order Tasks");
        Capgemini.PowerApps.SpecFlowBindings.Steps.EntitySubGridSteps.WhenISwitchToTheViewInTheSubgrid("All Work Order Service Tasks", "workorderservicetasksgrid");
    }
}