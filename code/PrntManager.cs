using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V102.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace prntProject
{
    public class PrntManager
    {
        WebDriver webDriver;
        string DriverPath, BaseUrl;
        Random rnd;
        Tables t;
        public readonly bool overrideFiles;
        public int succesfull { get; private set; }
        public Dictionary<string, string> urlsTested { get; private set; }

        public PrntManager(bool overrideFiles = false)
        {
            DriverPath = AppDomain.CurrentDomain.BaseDirectory;
            BaseUrl = "https://prnt.sc/";
            rnd = new Random();
            t = new Tables();
            this.overrideFiles = overrideFiles;
            succesfull = 0;
            urlsTested = new Dictionary<string, string>();
        }
        public void Start(int n, string name, string? acceptCookeXPath = "//*[@id=\"qc-cmp2-ui\"]/div[2]/div/button[3]", string imageXPath = "//*[@id=\"screenshot-image\"]")
        {
            // create new web driver
            webDriver = GetChromeDriver();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);

            Console.WriteLine(BaseUrl);
            webDriver.Navigate().GoToUrl(BaseUrl);

            // accept cookies
            if (acceptCookeXPath != null)
            {
                var input = webDriver.FindElement(By.XPath(acceptCookeXPath));
                input.Click();
            }

            // download image from next page
            for (int i = 0; i < n; i++)
                DownloadNexPage(name + i.ToString(), imageXPath, i, n);

            webDriver.Quit();
        }
        public void DownloadNexPage(string name, string imageXPath, int i , int n)
        {
            string url = GetRandomUrl();

            Console.WriteLine(url);
            webDriver.Navigate().GoToUrl(url);

            // get image url
            var image = webDriver.FindElement(By.XPath(imageXPath));
            string imageUrl = image.GetAttribute("src");

            // check if result directory exist (if not create)
            if (Directory.Exists(DriverPath + "results\\") == false)
                Directory.CreateDirectory(DriverPath + "results\\");

            // download image from url
            using (WebClient webClient = new WebClient())
            {
                string filePath = $"results\\{name}";
                string fileExtension = Path.GetExtension(new Uri(imageUrl).GetLeftPart(UriPartial.Path));
                try
                {
                    // check if file already exist
                    if (overrideFiles == false)
                        filePath = CheckFile(filePath, fileExtension);

                    urlsTested.Add(filePath, url);

                    // download file
                    webClient.DownloadFile(imageUrl, filePath + fileExtension);
                    succesfull++;
                    Console.WriteLine($"Downloaded: {i}/{n}");
                }
                catch (Exception e)
                {
                    Console.Write($"Error: {e.Message}");
                }
            }

        }
        public string CheckFile(string filePath, string fileExtension)
        {
            bool correct = false;
            for (int i = 0; correct == false; i++)
            {
                if (File.Exists(filePath + (i > 0 ? $"({i})" : "") + fileExtension) == false)
                {
                    correct = true;
                    filePath += i > 0 ? $"({i})" : "";
                }
            }
            return filePath;
        }
        private WebDriver GetChromeDriver(bool headless = true)
        {
            var options = new ChromeOptions();

            if (headless) options.AddArguments("--headless");

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
        private string GetRandomUrl()
        {
            return BaseUrl
           + t[rnd.Next(10, 36)]
           + t[rnd.Next(10, 36)]
           + t[rnd.Next(0, 9)]
           + t[rnd.Next(0, 9)]
           + t[rnd.Next(0, 9)]
           + t[rnd.Next(0, 9)];
        }
    }
}
