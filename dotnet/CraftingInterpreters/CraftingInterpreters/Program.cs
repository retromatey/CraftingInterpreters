namespace CraftingInterpreters;

public class Program
{
    public static bool HadError { get; set; } = false;

    // Exit codes found at:
    // https://www.freebsd.org/cgi/man.cgi?query=sysexits&apropos=0&sektion=0&manpath=FreeBSD+4.3-RELEASE&format=html\
    
    // The command was used incorrectly, e.g., with the wrong number of
    // arguments, a bad flag, a bad syntax in a parameter, or whatever.
    private const int EX_USAGE = 64;
    
    // The input data was incorrect in some way.  This should only be
    // used for user's data and not system files.
    private const int EX_DATAERR = 65;
    
    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: clox [script]");
            Environment.Exit(EX_USAGE);
        } 
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    private static void RunFile(string path)
    {
        var source = File.ReadAllText(path);
        Run(source);

        if (HadError)
        {
            Environment.Exit(EX_DATAERR);
        }
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();

            if (line is null)
            {
                break;
            }
            
            Run(line);
            HadError = false;
        }
    }

    private static void Run(string source)
    {
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        
        // For now, just print the tokens.
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        HadError = true;
    }
}