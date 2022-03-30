using System;
using ExitGames.Client.Photon;

public static class GameExtensions
{
    public static T GetEnumInProperties<T>(this Hashtable table, string key) => (T)Enum.Parse(typeof(T), (string)table[key]);

    public static bool GetBoolInProperties(this Hashtable table, string key) => bool.Parse((string)table[key]);

    public static int GetIntInProperties(this Hashtable table, string key) => int.Parse((string)table[key]);

    public static void ResetPropertyValue(this Hashtable table, string key, object value)
    {
        if (!table.ContainsKey(key))
            table.Add(key, value.ToString());
        else
            table[key] = value.ToString();
    }
}
