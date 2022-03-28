using System;
using ExitGames.Client.Photon;

public static class GameExtensions
{
    public static T GetEnumInProperties<T>(this Hashtable table, string key) => (T)Enum.Parse(typeof(T), (string)table[key]);

    public static bool GetBoolInProperties(this Hashtable table, string key) => bool.Parse((string)table[key]);
}
