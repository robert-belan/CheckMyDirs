using System.Diagnostics;

var stopwatch = new Stopwatch();

while (true)
{
    Console.WriteLine("(Write \"quit\" or something like that to exit)");
    Console.Write("Enter path: ");
    var filepath = Console.ReadLine().ToLower();

    switch (filepath)
    {
        case "q":
        case "quit":
        case "exit":
        case "konec":
        case "finito":
        case "uz me to neba": 
        case "something like that":
            return;
    }
   
    try
    {
        stopwatch.Start();
            var files = new DirectoryInfo(filepath).EnumerateFiles("*.*", SearchOption.AllDirectories);
        stopwatch.Stop();

        foreach (var file in files)
        {
            Console.WriteLine(file);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

        Console.Write("\nPress any key (not Any) to enter new path...\n");
        Console.ReadLine();
        Console.Clear();
    }
    
    Console.WriteLine(stopwatch.ElapsedMilliseconds);
    stopwatch.Reset();

}



