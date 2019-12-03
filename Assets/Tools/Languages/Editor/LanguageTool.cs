using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Text;

public class LanguageTool : MonoBehaviour
{

    #region Menu
    [MenuItem("Assets/Language Tool/CSV to XML")]
    static void CreateLanguageFile()
    {
        var selected = Selection.activeObject as TextAsset;

        var savePath = EditorUtility.SaveFilePanelInProject("Language File", "", "xml", "Choose path to save Language file");

        if (string.IsNullOrEmpty(savePath))
            return;

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.NewLineOnAttributes = false;
        XmlWriter writer = XmlWriter.Create(savePath, settings);

        //Start writing
        writer.WriteStartElement("", "Language", "");

        string[] lines = selected.text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        int lineCount = lines.Length; 
        for (int i = 0; i < lineCount; i++)
        {
            string line = lines[i];
            int index = line.IndexOf(",");
            if (index < 0)
                continue;
            string temp = line.Substring(0, index) + "|" + line.Substring(index + 1, line.Length - index - 1);
            var result = temp.Split(new string[] { "|" }, System.StringSplitOptions.None);
            var resultCount = result.Length;
            if (resultCount < 2)
                continue;

            var key = result[0];
            var value = result[1].Replace("\"", string.Empty).Split(); 

            writer.WriteStartElement("item");
            writer.WriteAttributeString("", "key", null, key);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }



        writer.WriteEndElement();
        writer.Close();

        Debug.Log("SUCCESSFUL CREATE LANGAUAGE FILE");
    }

    [MenuItem("Assets/Language Tool/XML to CSV")]
    static void CreateSCVFile()
    {
        var filePath = EditorUtility.SaveFilePanel("Create SCV File", "", "", "csv");

        if (string.IsNullOrEmpty(filePath))
            return;

        System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, false);

        var selected = Selection.activeObject as TextAsset;
        var result = Language.ReadXML(selected.text);

        foreach (var key in result.Keys)
        {
            var line = string.Format("{0},{1}", key, result[key]);
            file.WriteLine(line);
        }

        file.Close();
    }
    #endregion
}
