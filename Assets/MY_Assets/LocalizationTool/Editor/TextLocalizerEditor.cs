#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextLocalizerEditorWindow : EditorWindow
{
    public static void Open(string key)
    {
        TextLocalizerEditorWindow window = new TextLocalizerEditorWindow();
        window.titleContent = new GUIContent("Localizer Window");
        window.ShowUtility();
        window.key = key;
    }
    public string key;
    public string value;

    public void OnGUI()
    {
        key = EditorGUILayout.TextField("key:", key);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("value:", GUILayout.MaxWidth(50));

        EditorStyles.textArea.wordWrap = true;
        value = EditorGUILayout.TagField(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add"))
        {
            if(LocalizationSystem.GetLocalisedValue(key) != string.Empty)
            {
                LocalizationSystem.Replace(key, value);
            }
            else
            {
                LocalizationSystem.Add(key, value);
            }
        }

        minSize = new Vector2(460, 250);
        maxSize = minSize;
    }
}

public class TextLocaliserSearchWindow : EditorWindow 
{
    public static void Open()
    {
        TextLocaliserSearchWindow window = new TextLocaliserSearchWindow();
        window.titleContent = new GUIContent("Localisation Search");

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
        window.ShowAsDropDown(r, new Vector2(500, 300));
    }

    public string value;
    public Vector2 scroll;
    public Dictionary<string, string> dictionery;

    private void OnEnable()
    {
        dictionery = LocalizationSystem.GetDictionaryForEditor();     

    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("search:", EditorStyles.boldLabel);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();

        GetSearchResults();
    }

    private void GetSearchResults()
    {
        if(value == null) { return; }

        EditorGUILayout.BeginHorizontal();
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach(KeyValuePair<string, string> element in dictionery)
        {
            if(element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
            {
                EditorGUILayout.BeginVertical("Box");
                Texture closeIcon = (Texture)Resources.Load("close");

                GUIContent content = new GUIContent(closeIcon);

                if(GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key" + element.Key + "?", "This will remove the element from localization, are you sure?", "ok"))
                    {
                        LocalizationSystem.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalizationSystem.Init();
                        dictionery = LocalizationSystem.GetDictionaryForEditor();
                    }
                }

                EditorGUILayout.TextField(element.Key);
                EditorGUILayout.LabelField(element.Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}
#endif
