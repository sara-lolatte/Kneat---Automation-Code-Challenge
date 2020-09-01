namespace Challenge.Framework.Selenium
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Opera;
    using WebDriverManager;
    using WebDriverManager.DriverConfigs.Impl;

    /// <summary>
    /// Base class to initialize the driver etc.
    /// </summary>
    public static class Driver
    {
        /// <summary>
        /// Gets or sets the instance
        /// </summary>
        public static IWebDriver Instance { get; set; }

        /// <summary>
        /// Initializes the driver and goes to Web Page desired
        /// </summary>
        public static void WebPage_Initialize(string url)
        {
            StartDriver(string.Empty);
            Driver.TurnOnImplicitWait();
        }

        /// <summary>
        /// Starts up the driver and applies options
        /// </summary>
        /// <param name="browserType">Which browser to start</param>
        public static void StartDriver(string browserType)
        {
            switch (browserType)
            {
                case "edge":
                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AcceptInsecureCertificates = true;
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    Instance = new EdgeDriver(edgeOptions);
                    break;
                case "IE":
                    InternetExplorerOptions internetExplorerOptions = new InternetExplorerOptions();
                    new DriverManager().SetUpDriver(new InternetExplorerConfig());
                    Instance = new InternetExplorerDriver(internetExplorerOptions);
                    break;
                case "firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AcceptInsecureCertificates = true;
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    Instance = new FirefoxDriver(firefoxOptions);
                    break;
                case "opera":
                    OperaOptions operaOptions = new OperaOptions();
                    operaOptions.AcceptInsecureCertificates = true;
                    new DriverManager().SetUpDriver(new OperaConfig());
                    Instance = new OperaDriver(operaOptions);
                    break;
                default:
                    ChromeOptions chromeOptions = new ChromeOptions();

                    // Enable these arguments for a headless run
                    // chromeOptions.AddArguments("--headless", "--ignore-certificate-errors", "--disable-gpu", "--window-size=1920,1080", "--start-maximized", "--no-sandbox", "--proxy-server='direct://'", "--proxy-bypass-list=*");

                    // Enable these arguments for a run with UI
                    chromeOptions.AddArguments("--no-sandbox", "--ignore-certificate-errors", "--start-maximized");
                    var service = ChromeDriverService.CreateDefaultService();
                    service.LogPath = "C:\\Downloads\\chromedriver.log";
                    service.EnableVerboseLogging = true;
                    chromeOptions.AcceptInsecureCertificates = true;
                    chromeOptions.AddUserProfilePreference("download.default_directory", "C:\\Downloads");
                    chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                    chromeOptions.AddUserProfilePreference("download.directory_upgrade", true);
                    chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    Instance = new ChromeDriver(service, chromeOptions, TimeSpan.FromMinutes(2));

                    // * Uncomment code below and comment out line above to run with throttled throughput *
                    //// ChromeDriver chromeDriver = new ChromeDriver(service, chromeOptions, TimeSpan.FromMinutes(2));
                    //// chromeDriver.NetworkConditions = new ChromeNetworkConditions()
                    //// { DownloadThroughput = 6000, UploadThroughput = 1000, Latency = TimeSpan.FromMilliseconds(1) };
                    //// Instance = chromeDriver;

                    // "--disable-gpu", "--enable-logging", "--disable-extensions", "--screenshot", "--disable-popup-blocking", "--incognito"
                    // --allow-insecure-localhost", "--kiosk", "--silent-launch", "--no-startup-window"
                    break;
            }
        }

        /// <summary>
        /// Closes the instance of the driver
        /// </summary>
        public static void Close()
        {
            Instance.Close();
            Instance.Quit();
            Instance.Dispose();
        }

        /// <summary>
        /// Turns off the wait, performa an action and turns on wait
        /// </summary>
        /// <param name="action">The action to be performed</param>
        public static void NoWait(Action action)
        {
            TurnOffImplicitWait();
            action();
            TurnOnImplicitWait();
        }

        /// <summary>
        /// An implicit wait is to tell WebDriver to poll the DOM for a certain amount of time when
        /// trying to find an element or elements if they are not immediately available.
        /// The default setting is 0. Once set, the implicit wait is set for the life of the WebDriver
        /// object instance.
        /// </summary>
        public static void TurnOnImplicitWait()
        {
            Instance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Turns off wait
        /// </summary>
        public static void TurnOffImplicitWait()
        {
            Instance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }
    }
}
