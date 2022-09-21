using prntProject;

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

Console.WriteLine($"Downloading ended: {manager.succesfull}/{n}");

Console.WriteLine("Save links? (y/n)");
string answer = Console.ReadLine();

string links = "";
if (answer == "y")
{
    string path = manager.CheckFile("results\\links", ".txt");
    foreach (KeyValuePair<string, string> kvp in manager.urlsTested)
    {
        links += $"name:{kvp.Key} link:{kvp.Value} {Environment.NewLine}";
    }
    File.AppendAllText(path + ".txt", links);
}
Console.WriteLine("Enter to exit");
Console.ReadKey();