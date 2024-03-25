using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class VitaminLogger
{
    private static ManualLogSource _logger;

    static VitaminLogger()
    {
        _logger = BepInEx.Logging.Logger.CreateLogSource("VitaminLogger");
    }

    public static void LogInfo(string message)
    {
        _logger.LogInfo(message);
    }

    public static void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    public static void LogError(string message)
    {
        _logger.LogError(message);
    }
}

