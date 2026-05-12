using Defra.UI.Framework.Driver;
using Defra.UI.Tests.Data.Users;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System.Diagnostics;
using System.Globalization;
using static UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor.ContentOrderTextExtractor;

namespace Defra.UI.Tests.Tools
{
    public static class Utils
    {
        public static string GenerateRandomName()
        {
            var size = 25;
            var random = new Random();
            var alphabets = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = alphabets[random.Next(alphabets.Length)];
            }
            return new string(chars);
        }

        public static DateTime ConvertToDate(string dateTime)
        {
            return DateTime.ParseExact(dateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static string GenerateRandomUKPhonenumber()
        {
            var randomDigits = new Random().Next(10000000, 99999999);
            var phoneNumber = "075" + randomDigits.ToString();
            return phoneNumber;
        }

        public static string GenerateRandomNumber()
        {
            return DateTime.Now.ToString("ddMMyyHHmmss");
        }

        public static DateTime GetCurrentTime()
        {
            DateTime currentDate = DateTime.Today;
            return currentDate;
        }

        public static string GetCurrentDate(string format)
        {
            return DateTime.Today.ToString(format);
        }

        public static string GetFutureDate(int daysInFuture)
        {
            DateTime currentDate = DateTime.Today;
            DateTime futureDate = currentDate.AddDays(daysInFuture);
            return futureDate.ToString("dd/MM/yyyy");
        }

        public static string GetPastDate(int daysInFuture)
        {
            DateTime currentDate = DateTime.Today;
            DateTime futureDate = currentDate.AddDays(-daysInFuture);
            return futureDate.ToString("dd/MM/yyyy");
        }

        public static void ChangePageView(this IWebDriver driver, int percentage)
        {
            var jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript($"document.body.style.zoom = '{percentage}%';");

            driver.Wait(2);
        }

        public static void ScrollAndClick(this IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView()", element);
            element.Click();
        }

        public static void ScrollToElement(this IWebElement element, IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView()", element);
        }

        public static (string day, string month, string year) GetDayMonthYear(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                throw new ArgumentException("Date string cannot be empty.", nameof(dateString));

            dateString = dateString.Trim().ToLowerInvariant();

            DateTime date =
                dateString == "today" ? DateTime.Today :
                dateString == "yesterday" ? DateTime.Today.AddDays(-1) :
                dateString == "tomorrow" ? DateTime.Today.AddDays(1) :
                dateString == "future" ? DateTime.Today.AddDays(10) :
                dateString == "past" ? DateTime.Today.AddDays(-10) :
                dateString.StartsWith("future") ? DateTime.Today.AddDays(int.Parse(dateString.Replace("future", ""))) :
                dateString.StartsWith("past") ? DateTime.Today.AddDays(-int.Parse(dateString.Replace("past", ""))) :
                DateTime.ParseExact(
                    dateString,
                    new[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "d-M-yyyy" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None
                );

            string day = date.ToString("dd");
            string month = date.ToString("MM");
            string year = date.Year.ToString();

            return (day, month, year);
        }

        public static bool IsDownloaded(string fileName, string extension)
        {
            var downloadedFilePath = Path.Combine(Path.GetTempPath(), "automation-downloads", $"{fileName}.{extension}");

            var timeout = TimeSpan.FromSeconds(30);

            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (File.Exists(downloadedFilePath))
                {
                    return true;
                }

                Thread.Sleep(500);
            }
            return false;
        }

        public static bool Equals(this string expected, string actual)
        {
            return expected.Equals(actual, StringComparison.OrdinalIgnoreCase);
        }

        public static bool TextEquals(this string actual, string expected) => string.Equals(actual, expected, StringComparison.Ordinal);

        public static bool CollectionsEqualIgnoreOrder(IEnumerable<string> actual, IEnumerable<string> expected)
        {
            return actual.OrderBy(x => x).SequenceEqual(expected.OrderBy(x => x));
        }

        public static string DownloadPDF(string fileName, string pdfUrl, IUserObject UserObject, string userRole)
        {
            var chromeOptions = new ChromeOptions();

            // ✅ Unique download directory
            var downloadDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(downloadDirectory);           

            Console.WriteLine("downloadDirectory: " + downloadDirectory);

            // ✅ Required for pipeline (Linux)
            chromeOptions.AddArgument("--headless=new");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");

            // ✅ Download settings
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("download.directory_upgrade", true);
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

            chromeOptions.EnableDownloads = true;

            var service = ChromeDriverService.CreateDefaultService("/usr/bin/");

            Console.WriteLine("Starting ChromeDriver...");

            using (var tempDriver = new ChromeDriver(service, chromeOptions))
            {
                Console.WriteLine("***********Inside Chome driver block*************");

                tempDriver.Navigate().GoToUrl(pdfUrl);
                var elements = tempDriver.WaitForElements(By.CssSelector(".govuk-label.govuk-radios__label.break-word")).ToList();                      
                elements[1].Click();
                
                tempDriver.FindElement(By.Id("continueReplacement")).Click();

                var jsonData = UserObject?.GetUser("IPAFF", userRole);
                var userObject = new User
                {
                    UserName = jsonData.UserName,
                    Credential = jsonData.Credential
                };
                
                tempDriver.WaitForElement(By.Id("user_id")).SendKeys(userObject.UserName);
                tempDriver.FindElement(By.Id("password")).SendKeys(userObject.Credential);
                Thread.Sleep(1000);
                tempDriver.WaitForElement(By.Id("continue")).Click();
                Thread.Sleep(1000);

                IsDownloaded(fileName, "pdf");

                tempDriver.Quit();
                tempDriver.Dispose();
            }
            return downloadDirectory;
        }

       



        #region WebDriver Extension Methods for Element Safety

        /// <summary>
        /// Checks if an element is displayed without throwing an exception.
        /// Returns false if the element is not found or not displayed.
        /// </summary>
        public static bool IsElementDisplayed(this IWebDriver driver, By locator)
        {
            try
            {
                var element = driver.FindElement(locator);
                return element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (ElementNotInteractableException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if an element is displayed without throwing an exception.
        /// Returns false if the element is not found or not displayed.
        /// </summary>
        public static bool IsElementDisplayed(this IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (ElementNotInteractableException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// Safely retrieves text from an element by locator, returning empty string if element not found.
        /// </summary>
        public static string SafelyGetText(this IWebDriver driver, By locator)
        {
            try
            {
                var element = driver.FindElement(locator);
                return element.Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
            catch (StaleElementReferenceException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Safely retrieves text from an element, returning empty string if element not accessible.
        /// </summary>
        public static string SafelyGetText(this IWebElement element)
        {
            try
            {
                return element.Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
            catch (StaleElementReferenceException)
            {
                return string.Empty;
            }
        }

        #endregion

        #region ScenarioContext Extension Methods

        /// <summary>
        /// Removes multiple keys from ScenarioContext if they exist.
        /// Useful for cleaning up context when workflow changes (e.g., transport mode changes).
        /// </summary>
        /// <param name="scenarioContext">The ScenarioContext instance</param>
        /// <param name="keys">Array of keys to remove from context</param>
        public static void RemoveContextKeys(this ScenarioContext scenarioContext, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (scenarioContext.ContainsKey(key))
                {
                    scenarioContext.Remove(key);
                }
            }
        }

        public static T GetFromContext<T>(this ScenarioContext context, string key, T defaultValue = default!)
        {
            if (context.ContainsKey(key))
            {
                return context.Get<T>(key);
            }

            return defaultValue;
        }

        public static void AppendStringToScenarioContextArray(this ScenarioContext context, string key, string value)
        {
            if (context.TryGetValue(key, out var existing) && existing is string[] current)
            {
                var updated = new string[current.Length + 1];
                Array.Copy(current, updated, current.Length);
                updated[current.Length] = value;
                context[key] = updated;
            }
            else
            {
                context[key] = new[] { value };
            }
        }

        /// <summary>
        /// Gets or creates the MultiSpeciesData from ScenarioContext.
        /// Provides a single structured store for all multi-species related data.
        /// </summary>
        public static MultiSpeciesData GetOrCreateMultiSpeciesData(this ScenarioContext context)
        {
            if (context.ContainsKey(nameof(MultiSpeciesData)))
            {
                return context.Get<MultiSpeciesData>(nameof(MultiSpeciesData));
            }

            var data = new MultiSpeciesData();
            context[nameof(MultiSpeciesData)] = data;
            return data;
        }

        #endregion

        #region Operator Details Generation

        /// <summary>
        /// Locale codes that use non-Latin scripts (Arabic, Cyrillic, Chinese, Japanese, Korean, Greek, Georgian, Persian, Nepali).
        /// Countries mapped to these locales are excluded from random selection.
        /// </summary>
        private static readonly HashSet<string> NonLatinLocales =
        [
            "ar",      // Arabic
            "ru",      // Cyrillic (Russian)
            "uk",      // Cyrillic (Ukrainian)
            "zh_CN",   // Chinese (Simplified)
            "zh_TW",   // Chinese (Traditional)
            "ja",      // Japanese
            "ko",      // Korean
            "el",      // Greek
            "ge",      // Georgian
            "fa",      // Persian
            "ne"       // Nepali
        ];

        /// <summary>
        /// Generates random operator details with realistic data using Bogus Faker library.
        /// Randomly selects a country from the available countries list.
        /// Only selects countries that use Latin scripts.
        /// </summary>
        public static OperatorDetails GenerateOperatorDetails()
        {
            return GenerateOperatorDetails(null);
        }

        /// <summary>
        /// Generates random operator details with realistic data using Bogus Faker library.
        /// For Importer operator type, restricts country selection to Great Britain (England, Scotland, Wales, Northern Ireland).
        /// For other types, randomly selects from all available countries that use Latin scripts.
        /// </summary>
        /// <param name="operatorType">The type of operator (e.g., "Importer", "Exporter", "Transporter"). Pass null for random selection from all Latin-script countries.</param>
        public static OperatorDetails GenerateOperatorDetails(string? operatorType)
        {
            // List of all countries from IPAFFS dropdown with their best-match Bogus locales
            // Exclude non-Latin script locales and countries whose names contain commas
            // (commas break ExtractCountryFromAddressText which splits on comma delimiters)
            var countryLocaleMap = GetCountryLocaleMapping()
                .Where(kvp => !NonLatinLocales.Contains(kvp.Value))
                .Where(kvp => !kvp.Key.Contains(','))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Great Britain countries
            var greatBritainCountries = new List<string> { "England", "Scotland", "Wales", "Northern Ireland" };

            // Randomly select a country based on operator type
            var random = new Random();
            KeyValuePair<string, string> selectedCountry;

            // Check if operator type is Importer (case-insensitive, trim whitespace)
            var isImporter = !string.IsNullOrWhiteSpace(operatorType) &&
                             operatorType.Trim().Equals("Importer", StringComparison.OrdinalIgnoreCase);

            if (isImporter)
            {
                // For Importer, select only from Great Britain countries
                var gbCountryName = greatBritainCountries[random.Next(greatBritainCountries.Count)];
                selectedCountry = new KeyValuePair<string, string>(gbCountryName, countryLocaleMap[gbCountryName]);
            }
            else
            {
                // For other types, select from all available Latin-script countries
                selectedCountry = countryLocaleMap.ElementAt(random.Next(countryLocaleMap.Count));
            }

            var countryName = selectedCountry.Key;
            var locale = selectedCountry.Value;

            // Create Faker instance with selected locale
            var faker = new Bogus.Faker(locale);

            // Generate postcode
            var postcode = GeneratePostcodeForCountry(faker, countryName);

            // Generate phone number
            var phoneNumber = GeneratePhoneNumberForCountry(faker, countryName);

            var operatorDetails = new OperatorDetails
            {
                OperatorName = faker.Company.CompanyName().Replace(",", ""),
                AddressLine1 = faker.Address.StreetAddress().Replace(",", ""),
                CityOrTown = faker.Address.City().Replace(",", ""),
                Postcode = postcode,
                Country = countryName,
                TelephoneNumber = phoneNumber.Replace(",", ""),
                Email = GenerateSafeEmail(faker)
            };

            return operatorDetails;
        }

        /// <summary>
        /// Maps country names from IPAFFS to their most appropriate Bogus locale codes.
        /// Uses regionally authentic locales including non-Latin scripts (Arabic, Cyrillic, Chinese, Japanese, Korean, Greek, Georgian, Persian, Nepali).
        /// All locales are officially supported by the Bogus library.
        /// See: https://github.com/bchavez/Bogus#locales
        /// </summary>
        private static Dictionary<string, string> GetCountryLocaleMapping()
        {
            return new Dictionary<string, string>
            {
                // Countries mapped to VALID Bogus locales
                { "Afghanistan", "en" },
                { "Aland Islands", "fi" },
                { "Albania", "en" },
                { "Algeria", "ar" },
                { "American Samoa", "en_US" },
                { "Andorra", "es" },
                { "Angola", "pt_PT" },
                { "Anguilla", "en" },
                { "Antarctica", "en" },
                { "Antigua and Barbuda", "en" },
                { "Argentina", "es" },
                { "Armenia", "en" },
                { "Aruba", "nl" },
                { "Australia", "en_AU" },
                { "Austria", "de_AT" },
                { "Azerbaijan", "az" },
                { "Bahamas", "en" },
                { "Bahrain", "ar" },
                { "Bangladesh", "en" },
                { "Barbados", "en" },
                { "Belarus", "ru" },
                { "Belgium", "nl_BE" },
                { "Belize", "en" },
                { "Benin", "fr" },
                { "Bermuda", "en" },
                { "Bhutan", "en" },
                { "Bolivia", "es" },
                { "Bonaire, Sint Eustatius and Saba", "nl" },
                { "Bosnia and Herzegovina", "hr" },
                { "Botswana", "en" },
                { "Bouvet Island", "en" },
                { "Brazil", "pt_BR" },
                { "British Indian Ocean Territory", "en" },
                { "British Virgin Islands", "en" },
                { "Brunei", "en" },
                { "Bulgaria", "en" },
                { "Burkina Faso", "fr" },
                { "Burundi", "fr" },
                { "Cambodia", "en" },
                { "Cameroon", "fr" },
                { "Canada", "en_CA" },
                { "Canary Islands", "es" },
                { "Cape Verde", "pt_PT" },
                { "Cayman Islands", "en" },
                { "Central African Republic", "fr" },
                { "Chad", "fr" },
                { "Chile", "es" },
                { "China", "zh_CN" },
                { "Christmas Island", "en" },
                { "Cocos (Keeling) Islands", "en" },
                { "Colombia", "es" },
                { "Comoros", "fr" },
                { "Congo", "fr" },
                { "Cook Islands", "en" },
                { "Costa Rica", "es" },
                { "Côte d'Ivoire", "fr" },
                { "Croatia", "hr" },
                { "Cuba", "es" },
                { "Curaçao", "nl" },
                { "Cyprus", "el" },
                { "Czech Republic", "cz" },
                { "Democratic Republic of the Congo", "fr" },
                { "Denmark", "en" },
                { "Djibouti", "fr" },
                { "Dominica", "en" },
                { "Dominican Republic", "es" },
                { "Ecuador", "es" },
                { "Egypt", "ar" },
                { "El Salvador", "es" },
                { "England", "en_GB" },
                { "Equatorial Guinea", "es" },
                { "Eritrea", "en" },
                { "Estonia", "en" },
                { "Eswatini", "en" },
                { "Ethiopia", "en" },
                { "Falkland Islands", "en" },
                { "Faroe Islands", "en" },
                { "Fiji", "en" },
                { "Finland", "fi" },
                { "France", "fr" },
                { "French Guiana", "fr" },
                { "French Polynesia", "fr" },
                { "French Southern Territories", "fr" },
                { "Gabon", "fr" },
                { "Georgia", "ge" },
                { "Germany", "de" },
                { "Ghana", "en" },
                { "Gibraltar", "en" },
                { "Greece", "el" },
                { "Greenland", "en" },
                { "Grenada", "en" },
                { "Guadeloupe", "fr" },
                { "Guam", "en" },
                { "Guatemala", "es" },
                { "Guernsey", "en" },
                { "Guinea", "fr" },
                { "Guinea-Bissau", "pt_PT" },
                { "Guyana", "en" },
                { "Haiti", "fr" },
                { "Heard Island and McDonald Islands", "en" },
                { "Holy See", "it" },
                { "Honduras", "es" },
                { "Hong Kong", "zh_CN" },
                { "Hungary", "en" },
                { "Iceland", "en" },
                { "India", "en_IND" },
                { "Indonesia", "id_ID" },
                { "Iran", "fa" },
                { "Iraq", "ar" },
                { "Isle of Man", "en" },
                { "Israel", "en" },
                { "Italy", "it" },
                { "Jamaica", "en" },
                { "Japan", "ja" },
                { "Jersey", "en" },
                { "Jordan", "ar" },
                { "Kazakhstan", "ru" },
                { "Kenya", "en" },
                { "Kiribati", "en" },
                { "Kosovo", "en" },
                { "Kuwait", "ar" },
                { "Kyrgyzstan", "ru" },
                { "Laos", "en" },
                { "Latvia", "lv" },
                { "Lebanon", "ar" },
                { "Lesotho", "en" },
                { "Liberia", "en" },
                { "Libya", "ar" },
                { "Liechtenstein", "de" },
                { "Lithuania", "en" },
                { "Luxembourg", "fr" },
                { "Macao", "zh_CN" },
                { "Madagascar", "fr" },
                { "Malawi", "en" },
                { "Malaysia", "en" },
                { "Maldives", "en" },
                { "Mali", "fr" },
                { "Malta", "en" },
                { "Marshall Islands", "en" },
                { "Martinique", "fr" },
                { "Mauritania", "ar" },
                { "Mauritius", "en" },
                { "Mayotte", "fr" },
                { "Mexico", "es_MX" },
                { "Micronesia", "en" },
                { "Moldova", "ro" },
                { "Monaco", "fr" },
                { "Mongolia", "en" },
                { "Montenegro", "hr" },
                { "Montserrat", "en" },
                { "Morocco", "ar" },
                { "Mozambique", "pt_PT" },
                { "Myanmar", "en" },
                { "Namibia", "af_ZA" },
                { "Nauru", "en" },
                { "Nepal", "ne" },
                { "Netherlands", "nl" },
                { "New Caledonia", "fr" },
                { "New Zealand", "en" },
                { "Nicaragua", "es" },
                { "Niger", "fr" },
                { "Nigeria", "en_NG" },
                { "Niue", "en" },
                { "Norfolk Island", "en" },
                { "North Korea", "ko" },
                { "North Macedonia", "en" },
                { "Northern Ireland", "en_GB" },
                { "Northern Mariana Islands", "en" },
                { "Norway", "nb_NO" },
                { "Oman", "ar" },
                { "Pakistan", "en" },
                { "Palau", "en" },
                { "Panama", "es" },
                { "Papua New Guinea", "en" },
                { "Paraguay", "es" },
                { "Peru", "es" },
                { "Philippines", "en" },
                { "Pitcairn", "en" },
                { "Poland", "pl" },
                { "Portugal", "pt_PT" },
                { "Puerto Rico", "es" },
                { "Qatar", "ar" },
                { "Republic of Ireland", "en_IE" },
                { "Réunion", "fr" },
                { "Romania", "ro" },
                { "Russian Federation", "ru" },
                { "Rwanda", "fr" },
                { "Samoa", "en" },
                { "San Marino", "it" },
                { "Sao Tome and Principe", "pt_PT" },
                { "Saudi Arabia", "ar" },
                { "Scotland", "en_GB" },
                { "Senegal", "fr" },
                { "Serbia", "hr" },
                { "Seychelles", "en" },
                { "Sierra Leone", "en" },
                { "Singapore", "en" },
                { "Slovakia", "sk" },
                { "Slovenia", "hr" },
                { "Solomon Islands", "en" },
                { "Somalia", "ar" },
                { "South Africa", "en_ZA" },
                { "South Georgia and the South Sandwich Islands", "en" },
                { "South Korea", "ko" },
                { "South Sudan", "ar" },
                { "Spain", "es" },
                { "Sri Lanka", "en" },
                { "St Barthélemy", "fr" },
                { "St Helena, Ascension and Tristan da Cunha", "en" },
                { "St Kitts and Nevis", "en" },
                { "St Lucia", "en" },
                { "St Maarten (Dutch)", "nl" },
                { "St Martin (French)", "fr" },
                { "St Pierre and Miquelon", "fr" },
                { "St Vincent and the Grenadines", "en" },
                { "Sudan", "ar" },
                { "Suriname", "nl" },
                { "Sweden", "sv" },
                { "Switzerland", "de_CH" },
                { "Syria", "ar" },
                { "Taiwan", "zh_TW" },
                { "Tajikistan", "ru" },
                { "Tanzania", "en" },
                { "Thailand", "en" },
                { "The Gambia", "en" },
                { "The Occupied Palestinian Territories", "ar" },
                { "Timor-Leste", "pt_PT" },
                { "Togo", "fr" },
                { "Tokelau", "en" },
                { "Tonga", "en" },
                { "Trinidad and Tobago", "en" },
                { "Tunisia", "ar" },
                { "Turkey", "tr" },
                { "Turkmenistan", "ru" },
                { "Turks and Caicos Islands", "en" },
                { "Tuvalu", "en" },
                { "Uganda", "en" },
                { "Ukraine", "uk" },
                { "United Arab Emirates", "ar" },
                { "United Kingdom", "en_GB" },
                { "United States Minor Outlying Islands", "en_US" },
                { "United States of America", "en_US" },
                { "Uruguay", "es" },
                { "Uzbekistan", "ru" },
                { "Vanuatu", "en" },
                { "Venezuela", "es" },
                { "Viet Nam", "vi" },
                { "Virgin Islands of the United States", "en_US" },
                { "Wales", "en_GB" },
                { "Wallis and Futuna", "fr" },
                { "Western Sahara", "ar" },
                { "Yemen", "ar" },
                { "Zambia", "en" },
                { "Zimbabwe", "en_ZA" }
            };
        }

        /// <summary>
        /// Generates a postcode appropriate for the country
        /// </summary>
        private static string GeneratePostcodeForCountry(Bogus.Faker faker, string countryName)
        {
            return countryName switch
            {
                "United Kingdom" or "England" or "Scotland" or "Wales" or "Northern Ireland"
                    => faker.Address.ZipCode("?? #??").ToUpper(),
                "United States of America" or "United States Minor Outlying Islands"
                    => faker.Address.ZipCode("#####"),
                "Canada" => faker.Address.ZipCode("?#? #?#").ToUpper(),
                _ => faker.Address.ZipCode()
            };
        }

        /// <summary>
        /// Generates a phone number appropriate for the country
        /// </summary>
        private static string GeneratePhoneNumberForCountry(Bogus.Faker faker, string countryName)
        {
            return countryName switch
            {
                "United Kingdom" or "England" or "Scotland" or "Wales" or "Northern Ireland"
                    => GenerateRandomUKPhonenumber(),
                _ => faker.Phone.PhoneNumber()
            };
        }

        /// <summary>
        /// Generates a safe email address that doesn't start with a period or other invalid characters.
        /// Includes fallback mechanism to ensure valid email format.
        /// </summary>
        private static string GenerateSafeEmail(Bogus.Faker faker)
        {
            var email = faker.Internet.Email();
            var maxAttempts = 10;
            var attempts = 0;

            // Keep regenerating until we get a valid email
            while (attempts < maxAttempts)
            {
                // Check if email starts with invalid characters
                if (!email.StartsWith(".") &&
                    !email.StartsWith("-") &&
                    !email.StartsWith("_") &&
                    email.Contains("@") &&
                    !email.StartsWith("@"))
                {
                    return email;
                }

                // Regenerate if invalid
                email = faker.Internet.Email();
                attempts++;
            }

            // Fallback: manually create a valid email if all attempts fail
            var random = new Random();
            var prefix = $"user{random.Next(10000, 99999)}";
            var providers = new[] { "gmail.com", "outlook.com", "yahoo.com", "hotmail.com", "test.com" };
            var provider = providers[random.Next(providers.Length)];

            return $"{prefix}@{provider}";
        }

        #endregion

        /// <summary>
        /// Retrieves a file downloaded inside a Selenium Grid node container and saves it
        /// to the local automation-downloads directory on the agent machine.
        ///
        /// When running against a remote Selenium Grid, Chrome downloads land on the node's
        /// filesystem — not the agent's. This method uses the Grid 4 file download REST API
        /// (POST /session/{id}/se/files) to transfer the file as a base64-encoded zip,
        /// then extracts it locally.
        ///
        /// Falls back silently when the driver is not a RemoteWebDriver (e.g. local runs).
        /// </summary>
        /// <param name="driver">The active WebDriver session.</param>
        /// <param name="fileName">The exact file name to retrieve (e.g. "file.xlsx").</param>
        /// <param name="gridUrl">The Selenium Grid hub URL. Defaults to http://localhost:4444.</param>
        public static void RetrieveFileFromGrid(IWebDriver driver, string fileName, string gridUrl = "http://localhost:4444")
        {
            if (driver is not OpenQA.Selenium.Remote.RemoteWebDriver remoteDriver)
            {
                return;
            }

            var downloadDir = Path.Combine(Path.GetTempPath(), "automation-downloads");
            Directory.CreateDirectory(downloadDir);

            var sessionId = remoteDriver.SessionId.ToString();
            var endpoint = $"{gridUrl}/session/{sessionId}/se/files";
            var timeout = TimeSpan.FromSeconds(30);
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                try
                {
                    using var client = new HttpClient();
                    var payload = System.Text.Json.JsonSerializer.Serialize(new { name = fileName });
                    var requestContent = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

                    var response = client.PostAsync(endpoint, requestContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var body = response.Content.ReadAsStringAsync().Result;
                        using var doc = System.Text.Json.JsonDocument.Parse(body);

                        if (!doc.RootElement.TryGetProperty("value", out var value)) break;
                        if (!value.TryGetProperty("contents", out var contentsElement)) break;

                        var base64Contents = contentsElement.GetString();
                        if (string.IsNullOrEmpty(base64Contents)) break;

                        // Grid wraps the downloaded file in a zip archive
                        using var zipStream = new MemoryStream(Convert.FromBase64String(base64Contents));
                        using var zip = new System.IO.Compression.ZipArchive(zipStream, System.IO.Compression.ZipArchiveMode.Read);

                        foreach (var entry in zip.Entries)
                        {
                            var localPath = Path.Combine(downloadDir, entry.Name);
                            using var entryStream = entry.Open();
                            using var fileStream = File.Create(localPath);
                            entryStream.CopyTo(fileStream);
                            Console.WriteLine($"[GRID DOWNLOAD] Retrieved '{entry.Name}' from Grid node → '{localPath}'");
                        }

                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[GRID DOWNLOAD] Waiting for '{fileName}' on Grid: {ex.Message}");
                }

                Thread.Sleep(1000);
            }

            Console.WriteLine($"[GRID DOWNLOAD] Timed out waiting for '{fileName}' from Grid node at {endpoint}");
        }
    }

    #region Operator Details Model

    /// <summary>
    /// Model to hold operator details
    /// </summary>
    public class OperatorDetails
    {
        public string OperatorName { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string CityOrTown { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets the full address as a comma-separated string
        /// </summary>
        public string Address => $"{AddressLine1}, {CityOrTown}, {Postcode}";
    }

    #endregion

    #region Multi-Species Data Models

    /// <summary>
    /// Holds identification details for a single animal (microchip, passport, tattoo).
    /// </summary>
    public class AnimalIdentification
    {
        public string Microchip { get; set; } = string.Empty;
        public string Passport { get; set; } = string.Empty;
        public string Tattoo { get; set; } = string.Empty;
    }

    /// <summary>
    /// Holds all data for a single animal within a species: identification + permanent address.
    /// </summary>
    public class AnimalData
    {
        public AnimalIdentification Identification { get; set; } = new();
        public OperatorDetails? PermanentAddress { get; set; }
    }

    /// <summary>
    /// Holds all data for a single species: counts + per-animal data.
    /// </summary>
    public class SpeciesData
    {
        public string Name { get; set; } = string.Empty;
        public string NumberOfAnimals { get; set; } = string.Empty;
        public string NumberOfPackages { get; set; } = string.Empty;

        /// <summary>
        /// Per-animal data keyed by 1-based animal index.
        /// </summary>
        public Dictionary<int, AnimalData> Animals { get; set; } = [];

        /// <summary>
        /// Gets or creates the AnimalData for the given 1-based index.
        /// </summary>
        public AnimalData GetOrCreateAnimal(int animalIndex)
        {
            if (!Animals.TryGetValue(animalIndex, out var animal))
            {
                animal = new AnimalData();
                Animals[animalIndex] = animal;
            }
            return animal;
        }
    }

    /// <summary>
    /// Central store for all multi-species data in a scenario.
    /// Stored once in ScenarioContext under key "MultiSpeciesData".
    /// Replaces the fragmented SpeciesAnimals, SpeciesPackages, Identification_*, PermanentAddress_* keys.
    /// </summary>
    public class MultiSpeciesData
    {
        /// <summary>
        /// All species keyed by species name (e.g. "Canis familiaris").
        /// Maintains insertion order for iteration.
        /// </summary>
        public Dictionary<string, SpeciesData> Species { get; set; } = [];

        /// <summary>
        /// Gets or creates the SpeciesData for the given species name.
        /// </summary>
        public SpeciesData GetOrCreateSpecies(string speciesName)
        {
            if (!Species.TryGetValue(speciesName, out var species))
            {
                species = new SpeciesData { Name = speciesName };
                Species[speciesName] = species;
            }
            return species;
        }

        /// <summary>
        /// Whether any species data has been recorded.
        /// </summary>
        public bool HasData => Species.Count > 0;

        /// <summary>
        /// Validates all species data against actual review page data and returns mismatches.
        /// This single method replaces ValidateSpeciesDetails, ValidateIdentificationDetails,
        /// and ValidatePermanentAddresses from ReviewYourNotificationSteps.
        /// </summary>
        public List<string> ValidateAgainstReviewPage(
            List<(string species, string numberOfAnimals, string numberOfPackages)> actualSpeciesDetails,
            Func<string, List<(string animal, string microchip, string passport, string tattoo)>> getIdentificationForSpecies,
            List<(string animalName, string addressText)> actualPermanentAddresses)
        {
            var mismatches = new List<string>();

            foreach (var (speciesName, expected) in Species)
            {
                // --- Validate species counts ---
                var match = actualSpeciesDetails.FirstOrDefault(d =>
                    d.species.Equals(speciesName, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(match.species))
                {
                    mismatches.Add($"Species '{speciesName}': Not found on review page. Found: [{string.Join(", ", actualSpeciesDetails.Select(d => d.species))}]");
                    continue;
                }

                if (match.numberOfAnimals != expected.NumberOfAnimals)
                    mismatches.Add($"Species '{speciesName}' NumberOfAnimals: Expected '{expected.NumberOfAnimals}', Found '{match.numberOfAnimals}'");
                else
                    Console.WriteLine($"[REVIEW VALIDATION] ✓ Species '{speciesName}' NumberOfAnimals: '{expected.NumberOfAnimals}' matches");

                if (!string.IsNullOrEmpty(expected.NumberOfPackages))
                {
                    if (match.numberOfPackages != expected.NumberOfPackages)
                        mismatches.Add($"Species '{speciesName}' NumberOfPackages: Expected '{expected.NumberOfPackages}', Found '{match.numberOfPackages}'");
                    else
                        Console.WriteLine($"[REVIEW VALIDATION] ✓ Species '{speciesName}' NumberOfPackages: '{expected.NumberOfPackages}' matches");
                }

                // --- Validate identification details ---
                if (expected.Animals.Count > 0 && expected.Animals.Values.Any(a =>
                        !string.IsNullOrEmpty(a.Identification.Microchip) ||
                        !string.IsNullOrEmpty(a.Identification.Passport) ||
                        !string.IsNullOrEmpty(a.Identification.Tattoo)))
                {
                    var actualRows = getIdentificationForSpecies(speciesName);

                    if (actualRows.Count == 0)
                    {
                        mismatches.Add($"Identification '{speciesName}': No identification rows found on review page");
                        continue;
                    }

                    foreach (var (animalIndex, animalData) in expected.Animals)
                    {
                        var rowIndex = animalIndex - 1;
                        if (rowIndex >= actualRows.Count)
                        {
                            mismatches.Add($"Identification '{speciesName}' Animal {animalIndex}: Row not found (only {actualRows.Count} rows)");
                            continue;
                        }

                        var actualRow = actualRows[rowIndex];
                        var id = animalData.Identification;

                        ValidateField($"Identification_{speciesName}_{animalIndex}_Microchip", id.Microchip, actualRow.microchip, mismatches);
                        ValidateField($"Identification_{speciesName}_{animalIndex}_Passport", id.Passport, actualRow.passport, mismatches);
                        ValidateField($"Identification_{speciesName}_{animalIndex}_Tattoo", id.Tattoo, actualRow.tattoo, mismatches);
                    }
                }

                // --- Validate permanent addresses ---
                foreach (var (animalIndex, animalData) in expected.Animals)
                {
                    if (animalData.PermanentAddress == null)
                        continue;

                    var expectedLabel = $"{speciesName} {animalIndex}";
                    var addrMatch = actualPermanentAddresses.FirstOrDefault(a =>
                        a.animalName.Equals(expectedLabel, StringComparison.OrdinalIgnoreCase));

                    if (string.IsNullOrEmpty(addrMatch.animalName))
                    {
                        mismatches.Add($"PermanentAddress '{expectedLabel}': Row not found on review page");
                        continue;
                    }

                    var addr = animalData.PermanentAddress;
                    var actualText = addrMatch.addressText;

                    ValidateContains($"PermanentAddress_{speciesName}_{animalIndex}_AddressName", addr.OperatorName, actualText, mismatches);
                    ValidateContains($"PermanentAddress_{speciesName}_{animalIndex}_AddressLine1", addr.AddressLine1, actualText, mismatches);
                    ValidateContains($"PermanentAddress_{speciesName}_{animalIndex}_CityOrTown", addr.CityOrTown, actualText, mismatches);
                    ValidateContains($"PermanentAddress_{speciesName}_{animalIndex}_Postcode", addr.Postcode, actualText, mismatches);
                    ValidateContains($"PermanentAddress_{speciesName}_{animalIndex}_Telephone", addr.TelephoneNumber, actualText, mismatches);
                    ValidateContains($"PermanentAddress_{speciesName}_{animalIndex}_Email", addr.Email, actualText, mismatches);
                }
            }

            return mismatches;
        }

        private static void ValidateField(string label, string expected, string actual, List<string> mismatches)
        {
            if (string.IsNullOrEmpty(expected)) return;

            if (expected.Equals(actual, StringComparison.OrdinalIgnoreCase))
                Console.WriteLine($"[REVIEW VALIDATION] ✓ {label}: '{expected}' matches");
            else
                mismatches.Add($"{label}: Expected '{expected}', Found '{actual}'");
        }

        private static void ValidateContains(string label, string expected, string actualText, List<string> mismatches)
        {
            if (string.IsNullOrEmpty(expected)) return;

            if (actualText.Contains(expected, StringComparison.OrdinalIgnoreCase))
                Console.WriteLine($"[REVIEW VALIDATION] ✓ {label}: '{expected}' matches");
            else
                mismatches.Add($"{label}: Expected '{expected}' in '{actualText}'");
        }
    }

    #endregion
}