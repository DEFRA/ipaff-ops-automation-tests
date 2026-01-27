using Bogus;
using OpenQA.Selenium;
using Reqnroll;
using System.Globalization;

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

        public static string GenerateMicrochipNumber()
        {
            return DateTime.Now.ToString("ddMMyyHHmmssfff");
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

        public static void ScrollAndClick(this IWebElement element, IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView()", element);
            element.Click();
        }

        public static void ScrollToElement(this IWebElement element, IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView()", element);
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

        #endregion

        #region Operator Details Generation

        /// <summary>
        /// Generates random operator details with realistic data using Bogus Faker library.
        /// Randomly selects a country from the available countries list.
        /// Uses regionally authentic locales including non-Latin scripts (Arabic, Cyrillic, Chinese, Japanese, Korean, Greek, Georgian, Persian, Nepali).
        /// </summary>
        public static OperatorDetails GenerateOperatorDetails()
        {
            return GenerateOperatorDetails(null);
        }

        /// <summary>
        /// Generates random operator details with realistic data using Bogus Faker library.
        /// For Importer operator type, restricts country selection to Great Britain (England, Scotland, Wales, Northern Ireland).
        /// For other types, randomly selects from all available countries.
        /// Uses regionally authentic locales including non-Latin scripts (Arabic, Cyrillic, Chinese, Japanese, Korean, Greek, Georgian, Persian, Nepali).
        /// </summary>
        /// <param name="operatorType">The type of operator (e.g., "Importer", "Exporter", "Transporter"). Pass null for random selection from all countries.</param>
        public static OperatorDetails GenerateOperatorDetails(string? operatorType)
        {
            // List of all countries from IPAFFS dropdown with their best-match Bogus locales
            var countryLocaleMap = GetCountryLocaleMapping();

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
                // For other types, select from all available countries
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
}