
using System.IO;
using System.Net;
using UnityEngine;
using System;
public static class ToolHelper
{
    public static string[] TextFormat = { " ", " K", " M", " B", " T", " a"," b"," c"," d"," e"," f"," g"," h"," i"," j"," k"," l", " m",
    " n"," o"," p"," q"," r"," s", " t"," u"," v"," w","x"," y"," z"," aa"," ab"," ac"," ad"," ae"," af"," ag"," ah"," ai"," aj"," ak"," al"," am"," an"," ao"," ap"," aq"," ar"," as"," at"," au"," av"," aw"," ax"," ay"," az"};
    // takes a double and returns a simplified string representation
    public static string FormatDouble(double number)
    {
        string txt = "";
        if (number < 1000)
        {
            number = (int)number;
            if (number < 1000)
                txt = number.ToString();
        }
        else
        {
            if (number <= 0)
                return "0";
            int a = (int)System.Math.Log10(number);
            int b = a / 3;
            if (b == 0)
                return number.ToString("F2");
            if (b == 1)
                txt = (number / 1000).ToString("F2") + " K";
            else if (b < TextFormat.Length)
                txt = (number / System.Math.Pow(10, b * 3)).ToString("F2") + TextFormat[b];
            else
             if (number < 1000)
                txt = number.ToString();
            string[] tmp = txt.Split(' ');
            string[] tmp2 = tmp[0].Split('.');
            if (tmp2.Length > 1)
            {
                if (tmp2[1] == "00")
                    return tmp2[0] + " " + tmp[1];
            }
        }
        return txt;

    }


    public static string FormatDouble2(double number)
    {
        if (number == 0)
            return "0";
        int a = (int)System.Math.Log10(number);
        int b = a / 3;
        if (b == 0)
            return ((int)number).ToString("F0");
        return FormatDouble(number);
    }

    public static string FormatFloat(double number)
    {
        return string.Format("{0:0.##}", number);
    }

    // takes a double and returns a simplified string representation
    public static string FormatLong(long number)
    {
        if (number < 1000)
        {
            return number.ToString("F0");
        }
        else if (number < 1000000)
        {
            return ((double)number / 1000).ToString("F2") + " K";
        }
        else if (number < 1000000000)
        {
            return ((double)number / 1000000).ToString("F2") + " M";
        }
        else if (number < 1000000000000)
        {
            return ((double)number / 1000000000).ToString("F2") + " B";
        }
        else if (number < 1000000000000000)
        {
            return ((double)number / 1000000000000).ToString("F2") + " T";
        }
        else
        {
            return number.ToString("0.00e0");
        }
    }


    // returns a string representation of the total multiplier
    public static string FormatMultiplier(double number)
    {
        if (number < 1000)
        {
            return number.ToString("F2");
        }
        else if (number < 1000000)
        {
            return (number / 1000).ToString("F2") + " K";
        }
        else if (number < 1000000000)
        {
            return (number / 1000000).ToString("F2") + " M";
        }
        else if (number < 1000000000000)
        {
            return (number / 1000000000).ToString("F2") + " B";
        }
        else if (number < 1000000000000000)
        {
            return (number / 1000000000000).ToString("F2") + " T";
        }
        else
        {
            return number.ToString("0.00e0");
        }
    }
    public static string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    public static void SetDefaultTransform(this Transform transform)
    {
        transform.gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public static string GetTextTime(double sec, bool isOfflineReward = false)
    {
        //var TimeSpan1 = TimeSpan.FromSeconds(sec);
        //if (TimeSpan1.Days > 0)
        //{
        //    return string.Format("{0}d{1}h{2}m{3}s", TimeSpan1.Days, TimeSpan1.Hours, TimeSpan1.Minutes, TimeSpan1.Seconds);
        //}
        //if(TimeSpan1.Hours >0)
        //{
        //    return string.Format("{0}h{1}m{2}s",TimeSpan1.Hours, TimeSpan1.Minutes, TimeSpan1.Seconds);
        //}
        //if(TimeSpan1.Minutes >0)
        //{
        //    return string.Format("{0}m{1}s",  TimeSpan1.Minutes, TimeSpan1.Seconds);
        //}
        //if(TimeSpan1.Seconds >= 0)
        //{
        //    return string.Format("{0}s",  TimeSpan1.Seconds);
        //}
        //return "";
        var TimeSpan1 = TimeSpan.FromSeconds(sec);
        string timeOffline = "";
        if (TimeSpan1.Hours > 0)
        {
            /*if (TimeSpan1.Hours == 6 && isOfflineReward)
            {
                timeOffline = "06:00:00";
                return timeOffline;
            }*/
            if (TimeSpan1.Hours < 10)
            {
                timeOffline = "0" + TimeSpan1.Hours.ToString();
            }
            else
            {
                timeOffline = TimeSpan1.Hours.ToString();
            }
        }
        else
        {
            timeOffline = "00";
        }
        if (TimeSpan1.Minutes > 0)
        {
            if (TimeSpan1.Minutes < 10)
            {
                timeOffline += ":0" + TimeSpan1.Minutes.ToString();
            }
            else
            {
                timeOffline += ":" + TimeSpan1.Minutes.ToString();
            }
        }
        else
        {
            timeOffline += ":00";
        }
        if (TimeSpan1.Seconds > 0)
        {
            if (TimeSpan1.Seconds < 10)
            {
                timeOffline += ":0" + TimeSpan1.Seconds.ToString();
            }
            else
            {
                timeOffline += ":" + TimeSpan1.Seconds.ToString();
            }
        }
        else
        {
            timeOffline += ":00";
        }
        return timeOffline;

    }

    public static int WorkerString2Int(string wk)
    {
        switch (wk)
        {
            case "worker1":
                return 1;
            case "worker2":
                return 2;
            case "worker3":
                return 3;
            case "worker4":
                return 4;
        }
        return 1;
    }

    public static int GuardString2Int(string wk)
    {
        switch (wk.ToLower())
        {
            case "guard1":
                return 1;
            case "guard2":
                return 2;
        }
        return 1;
    }
}
