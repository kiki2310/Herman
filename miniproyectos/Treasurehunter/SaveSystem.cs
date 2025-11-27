using System.IO;
using UnityEngine;

public static class SaveSystem
{
    static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(string json)
    {
        File.WriteAllText(SavePath, json);
#if UNITY_EDITOR
        Debug.Log($"[Save] {SavePath}");
#endif
    }

    public static bool TryLoad(out string json)
    {
        if (File.Exists(SavePath))
        {
            json = File.ReadAllText(SavePath);
            return true;
        }
        json = null;
        return false;
    }

    public static void Delete()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);
    }
}
