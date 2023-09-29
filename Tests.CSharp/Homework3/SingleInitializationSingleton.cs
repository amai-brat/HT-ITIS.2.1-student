namespace Tests.CSharp.Homework3;

public class SingleInitializationSingleton
{
    public const int DefaultDelay = 3_000;
    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;

    private static Lazy<SingleInitializationSingleton> _instance = 
         new(() => new SingleInitializationSingleton(), true);

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    public int Delay { get; }

    public static SingleInitializationSingleton Instance => _instance.Value;

    internal static void Reset()
    {
        lock (Locker)
        {
            _instance = new Lazy<SingleInitializationSingleton>(
                () => new SingleInitializationSingleton(), true);
            _isInitialized = false;   
        }
    }

    public static void Initialize(int delay)
    {
        lock (Locker)
        {
            if (_isInitialized) throw new InvalidOperationException();
            
            _instance = new Lazy<SingleInitializationSingleton>(
                () => new SingleInitializationSingleton(delay), true);
            _isInitialized = true;
        }
    }
}