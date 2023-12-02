using Newtonsoft.Json;

public interface ILoggerRepository
{
    void Log(string message);
}

public class TxtLoggerRepository : ILoggerRepository
{
    private readonly string filePath;

    public TxtLoggerRepository(string path)
    {
        filePath = path;
    }

    public void Log(string message)
    {
        File.AppendAllText(filePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
    }
}

public class JsonLoggerRepository : ILoggerRepository
{
    private readonly string filePath;

    public JsonLoggerRepository(string path)
    {
        filePath = path;
    }

    public void Log(string message)
    {
        List<string> logs = new List<string>();

        if (File.Exists(filePath))
        {
            string existingJson = File.ReadAllText(filePath);
            logs = JsonConvert.DeserializeObject<List<string>>(existingJson) ?? new List<string>();
        }

        logs.Add($"{DateTime.Now}: {message}");

        string newJson = JsonConvert.SerializeObject(logs, Formatting.Indented);
        File.WriteAllText(filePath, newJson);
    }
}

public class MyLogger
{
    private readonly ILoggerRepository repository;

    public MyLogger(ILoggerRepository repository)
    {
        this.repository = repository;
    }
    
    public void Log(string message)
    {
        repository.Log(message);
    }
}

class Program
{
    static void Main()
    {
        string path = "/home/thematrix/CSharpProjects/Lab15/Task2/";
        MyLogger txtLogger = new MyLogger(new TxtLoggerRepository(path + "log.txt"));

        txtLogger.Log("This is a log entry for TXT.");

       MyLogger jsonLogger = new MyLogger(new JsonLoggerRepository(path + "log.json"));

        jsonLogger.Log("This is a log entry for JSON.");

        Console.WriteLine("Logs are written. Press Enter to exit.");
        Console.ReadLine();
    }
}
