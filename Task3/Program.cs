
public class SingleRandomizer
{
    private static SingleRandomizer instance;
    private static readonly object lockObject = new object();
    private Random random;

    private SingleRandomizer()
    {
        random = new Random();
    }

    public static SingleRandomizer Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SingleRandomizer();
                    }
                }
            }
            return instance;
        }
    }

    public int Next()
    {
        lock (lockObject)
        {
            return random.Next();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Используйте SingleRandomizer из разных потоков
        Thread thread1 = new Thread(() =>
        {
            var randomizer = SingleRandomizer.Instance;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Thread 1: {randomizer.Next()}");
            }
        });

        Thread thread2 = new Thread(() =>
        {
            var randomizer = SingleRandomizer.Instance;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Thread 2: {randomizer.Next()}");
            }
        });

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
    }
}