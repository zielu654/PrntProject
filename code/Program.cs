using prntProject;

Console.Write("How many attempts: ");
int n = 0;
int.TryParse(Console.ReadLine(), out n);
Console.Write("File name: ");
string name = Console.ReadLine();

bool overrideFiles = false;
bool checkExcludedImages = false;
string directoryExcludedPath = null;
// check optional parameters
for (int i = 0; i < args.Length; i++)
{
    // if "-o" override files (not change name)
    overrideFiles = args[i] == "-o";
    // if "-e" check if downloaded file is "empty" (optional add path to excluded images)
    if (args[i] == "-e")
    {
        checkExcludedImages = true;
        try
        {
            if(args[i + 1] != "-o")
                directoryExcludedPath = args[i + 1];
        }
        catch
        {
            directoryExcludedPath = "\\wrong";
        }
    }
}
if (checkExcludedImages == false)
    directoryExcludedPath = null;

PrntManager manager = new PrntManager(n, name, overrideFiles, directoryExcludedPath);

manager.Start();

Console.WriteLine($"Downloading ended: {manager.succesfull}/{n}");

Console.WriteLine("Save links? (y/n)");
string answer = Console.ReadLine();


if (answer == "y")
{
    manager.SaveLinks();
}

Console.WriteLine("Enter to exit");
Console.ReadKey();