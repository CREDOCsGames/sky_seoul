using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.Collections.Generic;

public static class Tsv2JsonUtility
{
    public static string ParseTsv(string input, bool parseNumber)
    {
        if (string.IsNullOrEmpty(input))
            throw new Exception("Tsv is null or empty");

        bool isArray = false;
        StringBuilder builder = new StringBuilder();

        List<string> attributeNames = new List<string>();

        string[] lines = input.Split('\n');
        if (lines.Length > 2)
        {
            isArray = true;
            builder.AppendLine("[");
        }

        var names = lines[0].Split('\t');
        for (int i = 0; i < names.Length; i++)
        {
            if (attributeNames.Contains(names[i]) == false)
            {
                ParseJsonAttribute(ref names[i], false);
                attributeNames.Add(names[i]);
            }
        }

        for (int y = 1; y < lines.Length; y++)
        {
            builder.AppendLine("\t{");
            var tabValues = lines[y].Split('\t');
            for (int x = 0; x < attributeNames.Count; x++)
            {
                if (tabValues.Length <= x)
                {
                    builder.AppendFormat("\t\t{0}:{1}", attributeNames[x], "");
                }
                else
                {
                    ParseJsonAttribute(ref tabValues[x], parseNumber);
                    builder.AppendFormat("\t\t{0}:{1}", attributeNames[x], tabValues[x]);
                }
                if (x != tabValues.Length - 1)
                    builder.AppendLine(",");
            }

            builder.Append(Environment.NewLine);
            builder.Append("\t}");
            if (isArray && y != lines.Length - 1)
                builder.AppendLine(",");
        }
        if (isArray)
        {
            builder.Append(Environment.NewLine);
            builder.Append("]");
        }

        return builder.ToString();
    }

    public static void ParseJsonAttribute(ref string str, bool parseNumber)
    {

        if (string.IsNullOrEmpty(str))
        {
            str = "\"\"";
            return;
        }
        else if (str[str.Length - 1] == '\r')
        {
            str = str.Remove(str.Length - 1);
            if (string.IsNullOrEmpty(str))
            {
                str = "\"\"";
                return;
            }
        }

        if (parseNumber)
        {
            long lValue;
            if (long.TryParse(str, out lValue))
                return;
            float fValue;
            if (float.TryParse(str, out fValue))
                return;
        }

        for (int i = 0; i < str.Length; i++)
        {
            var ch = str[i];
            if (ch == '\n' || ch == '\"'
                || ch == '\f' || ch == '\r'
                || ch == '\t' || ch == '\\')
            {
                if (ch == '\"' && (i == 0 || i == str.Length - 1))
                    continue;

                str = str.Insert(i, "\\");
                i++;
            }
        }

        if (str[0] != '\"')
            str = str.Insert(0, "\"");

        if (str[str.Length - 1] != '\"')
            str = str.Insert(str.Length, "\"");
    }
}
