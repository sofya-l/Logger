using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum LogLevel
{
    TRACE,
    DEBUG,
    INFO,
    WARNING,
    ERROR
}

public enum OutputTarget
{
    CONSOLE,
    FILE,
    BOTH
}

public class Logger
{
    private static readonly Logger instance = new Logger();
    private static readonly object locker = new object();

    private LogLevel logLevel;
    private OutputTarget outputTarget;
    private string logFileName = "Logger.log";

    private Logger() { }

    public static Logger GetInstance()
    {
        return instance;
    }

    public void SetLogLevel(LogLevel level)
    {
        logLevel = level;
    }

    public void SetOutputTarget(OutputTarget target)
    {
        outputTarget = target;
    }

    public void SetLogFileName(string fileName)
    {
        logFileName = fileName;
    }

    public void Log(LogLevel level, string message)
    {
       if (level < logLevel)
            return;

        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {level} -> {message}";

        switch (outputTarget)
        {
            case OutputTarget.CONSOLE:
                Console.WriteLine(logEntry);
                break;
            case OutputTarget.FILE:
                WriteToFile(logEntry);
                break;
            case OutputTarget.BOTH:
                Console.WriteLine(logEntry);
                WriteToFile(logEntry);
                break;
            default:
                break;
        }
    }

    private void WriteToFile(string logEntry)
    {
        lock (locker)
        {
            using (StreamWriter writer = File.AppendText(logFileName))
            {
                writer.WriteLine(logEntry);
            }
        }
    }

    public void LOGE(string message)
    {
        Log(LogLevel.ERROR, message);
    }

    public void LOGW(string message)
    {
        Log(LogLevel.WARNING, message);
    }

    public void LOGI(string message)
    {
        Log(LogLevel.INFO, message);
    }

    public void LOGD(string message)
    {
        Log(LogLevel.DEBUG, message);
    }

    public void LOGT(string message)
    {
        Log(LogLevel.TRACE, message);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Logger logger = Logger.GetInstance();
        logger.SetLogLevel(LogLevel.INFO);
        logger.SetOutputTarget(OutputTarget.BOTH);
        logger.SetLogFileName("my_log.log");

        logger.LOGI("User logged in");
        logger.LOGW("Connection lost");
        logger.LOGE("Critical error occurred");
        logger.LOGD("Debug Information-Product Starting");
        logger.LOGT("Trace Information-Product Ending");
        Console.ReadLine();
    }
}
