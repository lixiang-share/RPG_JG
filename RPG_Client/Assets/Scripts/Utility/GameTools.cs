using UnityEngine;
using System.Collections;

public static class GameTools {

    public static void Log(object o)
    {
        UITools.log(o);
    }

    public static void LogError(object o)
    {
        UITools.logError(o);
    }
    public static void LogWarring(object o)
    {
        UITools.logWarning(o);
    }
}
