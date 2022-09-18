using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using prntProject;
using System.Net;

  

Console.Write("How many attempts: ");
int n = 0;
int.TryParse(Console.ReadLine(), out n);
Console.Write("File name: ");
string name = Console.ReadLine();

bool overrideFiles = false;
if (args.Length > 0)
    overrideFiles = args[0] == "-o";

PrntManager manager = new PrntManager(overrideFiles);

manager.Start(n, name);




