using System.Collections.Generic;
using UnityEngine.Events;


public class LocalizationSystem : Manager<LocalizationSystem>
{
   public enum Language { English, French}

    public static Language language = Language.English;

    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedFN;

    public static bool isInit;

    public static CVSLoader cVSLoader;

    public OnLanguageTypeChange OnLanguage;

    public static void Init()
    {
        cVSLoader = new CVSLoader();
        cVSLoader.LoadCSV();

        UppdateDictionarys();

        isInit = true;
    }

    public void ChangeLanguages()
    {
        if(language == Language.English)
        {
            language = Language.French;
        }
        else
        {
            language = Language.English;
        }
        OnLanguage.Invoke();
    }
    private static void UppdateDictionarys()
    {
        localisedEN = cVSLoader.GetDictionaryValues("en");
        localisedFN = cVSLoader.GetDictionaryValues("fr");
    }

    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!isInit) { Init(); }

        return localisedEN;  
    }
    public static string GetLocalisedValue(string key)
    {
        if (!isInit) { Init(); }

        string value = key;

        switch (language)
        {
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case Language.French:
                localisedFN.TryGetValue(key, out value);
                break;
        }

        return value;
    }
    #if UNITY_EDITOR
    public static void Add(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if(cVSLoader == null)
        {
            cVSLoader = new CVSLoader();
        }

        cVSLoader.LoadCSV();
        cVSLoader.Add(key, value);
        cVSLoader.LoadCSV();

        UppdateDictionarys();
    }

    public static void Replace(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if (cVSLoader == null)
        {
            cVSLoader = new CVSLoader();
        }

        cVSLoader.LoadCSV();
        cVSLoader.Add(key, value);
        cVSLoader.LoadCSV();

        UppdateDictionarys();
    }

    public static void Remove(string key)
    {
        if (cVSLoader == null)
        {
            cVSLoader = new CVSLoader();
        }

        cVSLoader.LoadCSV();
        cVSLoader.Remove(key);
        cVSLoader.LoadCSV();

        UppdateDictionarys();
    }
#endif
}
[System.Serializable]
public class OnLanguageTypeChange : UnityEvent { }
