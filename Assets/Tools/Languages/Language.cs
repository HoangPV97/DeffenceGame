// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class Language
{
    private static XmlDocument xmlDocument;
    private static Dictionary<string, string> dict;

    private static List<LabelLanguages> ListLableLanguage = new List<LabelLanguages>();

    public static Dictionary<Languages, string> languageResource = new Dictionary<Languages, string>();

    static Language()
    {
        languageResource[Languages.vn] = (Resources.Load("Languages/" + Languages.vn.ToString()) as TextAsset).text;
        languageResource[Languages.en] = (Resources.Load("Languages/" + Languages.en.ToString()) as TextAsset).text;
    }

    //hàm load file languages
    public static void LoadLanguageFile(Languages languageType)
    {
        if (!languageResource.ContainsKey(languageType))
            languageResource[languageType] = (Resources.Load("Languages/" + languageType.ToString()) as TextAsset).text;
        dict = ReadXML(languageResource[languageType]);
    }

    #region API

    public static Dictionary<string, string> ReadXML(string xml)
    {
        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        var result = new Dictionary<string, string>();
        if (xmlDocument.ChildNodes.Count > 0)
        {
            var list = xmlDocument.ChildNodes.Item(1);
            if (list.ChildNodes.Count > 0)
            {
                for (int i = 0; i < list.ChildNodes.Count; i++)
                {
                    var t = list.ChildNodes.Item(i);
                    if (t.Name == "item")
                    {
                        string key = t.Attributes["key"].Value;
                        string val = t.InnerText;
                        if (val.EndsWith("\n"))
                            val = val.Substring(0, val.Length - 2);
                        if (!result.ContainsKey(key))
                            result.Add(key, val);
                        else
                            result[key] = val;
                    }
                }
            }
        }

        return result;
    }

    //Hàm được gọi lúc chuyển đổi ngôn ngữ
    public static void ChangeLanguage(Languages lang)
    {
        LoadLanguageFile(lang);
        for (int i = 0; i < ListLableLanguage.Count; i++)
        {
            ListLableLanguage[i].SetText();
        }
    }

    public static string GetKey(string key)
    {
        if (dict == null)
            return key;
        if (dict.ContainsKey(key))
        {
            string str = dict[key];
            str.Replace("\\n", "\n");
            str.Replace("&lt;", "<");
            str.Replace("&gt;", ">");
            return str.Trim();
        }

        return key;
    }

    //Sau khi gán text thì add vào list để lúc chuyển scene remove hết đi
    public static void AddLableLanguage(LabelLanguages lang)
    {
        if (ListLableLanguage.Contains(lang))
            return;
        ListLableLanguage.Add(lang);
    }


    public static void RemoveLaleLanguage(LabelLanguages lang)
    {
        if (!ListLableLanguage.Contains(lang))
            return;
        ListLableLanguage.Remove(lang);
    }

    public static Dictionary<string, string> GetLanguageDictionary()
    {
        return dict;
    }

    #endregion
}