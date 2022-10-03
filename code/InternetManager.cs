using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace prntProject;
public  class InternetManager
{
    Tables t;
    Random rnd;
    public InternetManager()
    {
        t = new();
        rnd = new();
    }
    public  WebDriver GetChromeDriver(string path,bool headless = true)
    {
        var options = new ChromeOptions();
        if (headless) options.AddArguments("--headless");
        options.AddArguments("--log-level=3");

        return new ChromeDriver(path, options, TimeSpan.FromSeconds(300));
    }
    public string GetRandomUrl(string BaseUrl)
    {
        return BaseUrl
       + t[rnd.Next(10, 36)]
       + t[rnd.Next(10, 36)]
       + t[rnd.Next(0, 9)]
       + t[rnd.Next(0, 9)]
       + t[rnd.Next(0, 9)]
       + t[rnd.Next(0, 9)];
    }
    public string GetImageUrl(string imageXPath, WebDriver webDriver)
    {
        // get image url
        var image = webDriver.FindElement(By.XPath(imageXPath));
        string imageUrl = image.GetAttribute("src");
        return imageUrl;
    }
    public bool DownloadImage(string filePath, string fileExtension, Uri imageUrl, bool overrideFiles)
    {
        // download image from url
        using (WebClient webClient = new WebClient())
        {
            try
            {
                // check if file already exist
                if (overrideFiles == false)
                    filePath = FileManager.CheckFile(filePath, fileExtension);

                // download file
                webClient.DownloadFile(imageUrl, filePath + fileExtension);
            }
            catch (Exception e)
            {
                Console.Write($"Error: {e.Message}");
                return false;
            }
            finally
            {
                webClient.Dispose();
            }
        }
        return true;
    }
}

