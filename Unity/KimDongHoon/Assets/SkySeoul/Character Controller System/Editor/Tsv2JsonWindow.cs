using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class Tsv2JsonWindow : EditorWindow
{
    [MenuItem("Tool/Tsv To Json (Old)")]
    public static void ShowWindow()
    {
        GetWindow<Tsv2JsonWindow>("Tsv to Json");
    }

    bool _parseNumber = false;

    string _savePath = "";

    string _tsvValue;
    Vector2 _tsvValueScrollPos;

    private void OnGUI()
    {
        _parseNumber = EditorGUILayout.Toggle("Parse Number", _parseNumber);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Tsv Text", EditorStyles.boldLabel);
        _tsvValueScrollPos = EditorGUILayout.BeginScrollView(_tsvValueScrollPos);
        _tsvValue = EditorGUILayout.TextArea(_tsvValue, GUILayout.MinHeight(300));
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Parse", GUILayout.Height(50)))
        {
            if(_tsvValue.EndsWith(Environment.NewLine))
            {
                _tsvValue = _tsvValue.Substring(0, _tsvValue.Length - 1);
            }

            Parse(_tsvValue);
            _tsvValue = "";
            Repaint();
        }
        if (GUILayout.Button("Parse from clipboard", GUILayout.Height(50)))
        {
            Parse(EditorGUIUtility.systemCopyBuffer);
            Repaint();
        }
    }
    void Parse(string tsv)
    {
        string json = Tsv2JsonUtility.ParseTsv(tsv, _parseNumber);
        if (!string.IsNullOrEmpty(json))
        {
            SetSavePath();

            System.IO.File.WriteAllText(_savePath, json);
            AssetDatabase.Refresh();
        }
        EditorUtility.DisplayDialog("Complete", "변환이 완료되었습니다.\n" + _savePath, "Ok");
    }

    void SetSavePath()
    {
        _savePath = EditorUtility.SaveFilePanel("Save Json", Application.dataPath, "export", "json");
    }
}
