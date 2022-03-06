using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatabaseManager
{
    public static string username;
    public static int userid;
    public static int currentruntime;

    public static bool LoggedIn { get { return username != null; } }

}
