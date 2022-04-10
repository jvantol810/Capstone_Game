using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatabaseManager
{
    public static string username;
    public static int userid;
    public static int currentruntime;
    public static int currency;
    public static string ownedhats;

    public static bool LoggedIn { get { return username != null; } }

}
