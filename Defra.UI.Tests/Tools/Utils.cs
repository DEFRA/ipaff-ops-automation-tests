using Bogus;
using OpenQA.Selenium;
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

        #region Operator Details Generation

        /// <summary>
        /// Generates random operator details with realistic data using Bogus Faker library.
        /// Randomly selects a country from the available countries list.
        /// </summary>
        public static OperatorDetails GenerateOperatorDetails()
        {
            // List of all countries from IPAFFS dropdown with their best-match Bogus locales
            var countryLocaleMap = GetCountryLocaleMapping();

            // Randomly select a country
            var random = new Random();
            var selectedCountry = countryLocaleMap.ElementAt(random.Next(countryLocaleMap.Count));
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
                OperatorName = faker.Company.CompanyName(),
                AddressLine1 = faker.Address.StreetAddress(),
                CityOrTown = faker.Address.City(),
                Postcode = postcode,
                Country = countryName,
                TelephoneNumber = phoneNumber,
                Email = faker.Internet.Email().ToLower()
            };

            return operatorDetails;
        }

        /// <summary>
        /// Maps country names from IPAFFS to their most appropriate Bogus locale codes
        /// </summary>
        private static Dictionary<string, string> GetCountryLocaleMapping()
        {
            return new Dictionary<string, string>
            {
                // All countries from IPAFFS HTML dropdown mapped to Bogus locales
                { "Afghanistan", "en" },
                { "Aland Islands", "en" },
                { "Albania", "en" },
                { "Algeria", "ar" },
                { "American Samoa", "en_US" },
                { "Andorra", "es" },
                { "Angola", "pt" },
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
                { "Bosnia and Herzegovina", "en" },
                { "Botswana", "en" },
                { "Bouvet Island", "en" },
                { "Brazil", "pt_BR" },
                { "British Indian Ocean Territory", "en" },
                { "British Virgin Islands", "en" },
                { "Brunei", "en" },
                { "Bulgaria", "bg" },
                { "Burkina Faso", "fr" },
                { "Burundi", "fr" },
                { "Cambodia", "en" },
                { "Cameroon", "fr" },
                { "Canada", "en_CA" },
                { "Canary Islands", "es" },
                { "Cape Verde", "pt" },
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
                { "Denmark", "da" },
                { "Djibouti", "fr" },
                { "Dominica", "en" },
                { "Dominican Republic", "es" },
                { "Ecuador", "es" },
                { "Egypt", "ar" },
                { "El Salvador", "es" },
                { "England", "en_GB" },
                { "Equatorial Guinea", "es" },
                { "Eritrea", "en" },
                { "Estonia", "et" },
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
                { "Georgia", "ka" },
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
                { "Guinea-Bissau", "pt" },
                { "Guyana", "en" },
                { "Haiti", "fr" },
                { "Heard Island and McDonald Islands", "en" },
                { "Holy See", "it" },
                { "Honduras", "es" },
                { "Hong Kong", "zh_CN" },
                { "Hungary", "hu" },
                { "Iceland", "en" },
                { "India", "en_IND" },
                { "Indonesia", "id" },
                { "Iran", "fa" },
                { "Iraq", "ar" },
                { "Isle of Man", "en" },
                { "Israel", "he" },
                { "Italy", "it" },
                { "Jamaica", "en" },
                { "Japan", "ja" },
                { "Jersey", "en" },
                { "Jordan", "ar" },
                { "Kazakhstan", "en" },
                { "Kenya", "en" },
                { "Kiribati", "en" },
                { "Kosovo", "en" },
                { "Kuwait", "ar" },
                { "Kyrgyzstan", "en" },
                { "Laos", "en" },
                { "Latvia", "lv" },
                { "Lebanon", "ar" },
                { "Lesotho", "en" },
                { "Liberia", "en" },
                { "Libya", "ar" },
                { "Liechtenstein", "de" },
                { "Lithuania", "lt" },
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
                { "Montenegro", "en" },
                { "Montserrat", "en" },
                { "Morocco", "ar" },
                { "Mozambique", "pt" },
                { "Myanmar", "en" },
                { "Namibia", "en" },
                { "Nauru", "en" },
                { "Nepal", "en" },
                { "Netherlands", "nl" },
                { "New Caledonia", "fr" },
                { "New Zealand", "en" },
                { "Nicaragua", "es" },
                { "Niger", "fr" },
                { "Nigeria", "en" },
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
                { "Sao Tome and Principe", "pt" },
                { "Saudi Arabia", "ar" },
                { "Scotland", "en_GB" },
                { "Senegal", "fr" },
                { "Serbia", "sr_Latn" },
                { "Seychelles", "en" },
                { "Sierra Leone", "en" },
                { "Singapore", "en" },
                { "Slovakia", "sk" },
                { "Slovenia", "sl" },
                { "Solomon Islands", "en" },
                { "Somalia", "ar" },
                { "South Africa", "en_ZA" },
                { "South Georgia and the South Sandwich Islands", "en" },
                { "South Korea", "ko" },
                { "South Sudan", "en" },
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
                { "Tajikistan", "en" },
                { "Tanzania", "en" },
                { "Thailand", "th" },
                { "The Gambia", "en" },
                { "The Occupied Palestinian Territories", "ar" },
                { "Timor-Leste", "pt" },
                { "Togo", "fr" },
                { "Tokelau", "en" },
                { "Tonga", "en" },
                { "Trinidad and Tobago", "en" },
                { "Tunisia", "ar" },
                { "Turkey", "tr" },
                { "Turkmenistan", "en" },
                { "Turks and Caicos Islands", "en" },
                { "Tuvalu", "en" },
                { "Uganda", "en" },
                { "Ukraine", "uk" },
                { "United Arab Emirates", "ar" },
                { "United Kingdom", "en_GB" },
                { "United States Minor Outlying Islands", "en_US" },
                { "United States of America", "en_US" },
                { "Uruguay", "es" },
                { "Uzbekistan", "en" },
                { "Vanuatu", "en" },
                { "Venezuela", "es" },
                { "Viet Nam", "vi" },
                { "Virgin Islands of the United States", "en_US" },
                { "Wales", "en_GB" },
                { "Wallis and Futuna", "fr" },
                { "Western Sahara", "ar" },
                { "Yemen", "ar" },
                { "Zambia", "en" },
                { "Zimbabwe", "en" }
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