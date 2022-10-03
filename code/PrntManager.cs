using OpenQA.Selenium;
using System.Drawing;

namespace prntProject;
public class PrntManager
{
    WebDriver webDriver;
    InternetManager internetManager;
    string path, BaseUrl, name;

    public readonly bool overrideFiles;
    public readonly bool checkExcludedFiles;
    public readonly string excludedDirectoryPath;
    public int succesfull { get; private set; }
    public Dictionary<string, string> urlsTested { get; private set; }
    List<Bitmap> WrongImages;
    int n;

    public PrntManager(int n, string name, bool overrideFiles = false, string wrongDirectoryPath = null)
    {
        path = AppDomain.CurrentDomain.BaseDirectory;
        BaseUrl = "https://prnt.sc/";
        succesfull = 0;
        urlsTested = new Dictionary<string, string>();
        internetManager = new();
        this.overrideFiles = overrideFiles;
        this.n = n;
        this.name = name;


        if (wrongDirectoryPath != null)
        {
            checkExcludedFiles = true;
            this.excludedDirectoryPath = path + wrongDirectoryPath;
            // get all files
            string[] files = Directory.GetFiles(this.excludedDirectoryPath, "*.*", SearchOption.AllDirectories);
            // add files to list
            WrongImages = new List<Bitmap>();
            foreach (string file in files)
            {
                WrongImages.Add(new Bitmap(file));
            }
        }
        // create new web driver
        webDriver = internetManager.GetChromeDriver(path);
        webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);

    }
    public void Start(string? acceptCookeXPath = "//*[@id=\"qc-cmp2-ui\"]/div[2]/div/button[3]", string imageXPath = "//*[@id=\"screenshot-image\"]")
    {
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
            DownloadNexPage(name + i.ToString(), imageXPath, i);

        webDriver.Quit();

    }
    public void DownloadNexPage(string name, string imageXPath, int i)
    {
        string url = internetManager.GetRandomUrl(BaseUrl);

        Console.WriteLine(url);
        webDriver.Navigate().GoToUrl(url);
        string imageUrl = internetManager.GetImageUrl(imageXPath, webDriver);

        FileManager.CreateDirectory(path);

        string filePath = $"results\\{name}";
        string fileExtension = Path.GetExtension(new Uri(imageUrl).GetLeftPart(UriPartial.Path));

        if (internetManager.DownloadImage(filePath, fileExtension, new Uri(imageUrl), overrideFiles) == false) return;
        urlsTested.Add(filePath, url);

        if (CheckOptionalArguments(path + filePath + fileExtension))
        {
            Console.WriteLine($"Downloaded: {i + 1}/{n}");
            succesfull++;
        }
    }
    bool CheckOptionalArguments(string filePath)
    {
        if (checkExcludedFiles == false)
            return true;


        // if checkWrongFiles is true check if downloaded image is excluded image
        // if is delete

        Bitmap downloadedImage = FileManager.GetBitmapFromFile(filePath);

        foreach (var item in WrongImages)
        {
            if (FileManager.CompareBitmapsFast(item, downloadedImage) == false) continue;
            if (File.Exists(filePath))
                File.Delete(filePath);
            Console.WriteLine("Info: excluded image");
            return false;
        }

        return true;
    }
    public void SaveLinks()
    {
        string links = "";
        string path = FileManager.CheckFile("results\\links", ".txt");
        foreach (KeyValuePair<string, string> kvp in urlsTested)
        {
            links += $"name:{kvp.Key} link:{kvp.Value} {Environment.NewLine}";
        }
        File.AppendAllText(path + ".txt", links);
    }

}

