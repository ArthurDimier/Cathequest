using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class LanguageManager : MonoBehaviour {
    // This is where the current loaded language will go
    private static Hashtable textTable;

    [HideInInspector]
    // Just a reference for the current language, default to français
    public string CurrentLanguage = "français";


    // Run this when you are ready to start the language process,
    // you usually want to do this after everything has loaded
    public void Init() {
        // You should use an enum for storing settings that are a unique list,
        // This gets a string representation of an enum
        // CurrentLanguage = Enum.GetName(typeof(Settings.Languages), (int)FindObjectOfType<GameManager>().Settings.Language);

        // Pass that language into LoadLanguage, remember we are in init
        // so this should only run once.
        CurrentLanguage = PlayerPrefs.GetString("lang", "français");
        LoadLanguage(CurrentLanguage);
    }

    // You call this when you want to update all text boxes with the new translation
    // Run this after Init
    // Run this whenever you run LoadLanguage
    // Run this whenever you load a new scene and want to translate the new UI
    public void UpdateAllTextBoxes() {
        // Find all active and inactive text boxes and loop through them
        UILocalizeText[] temp = Resources.FindObjectsOfTypeAll<UILocalizeText>()as UILocalizeText[];
        foreach (UILocalizeText text in temp) {
            // Run the update translation function on each text
            text.UpdateTranslation();
        }
    }

    // Run this whenever a language changes, like in when a setting is changed - then run UpdateAllTextBoxes
    //This is based off of http://wiki.unity3d.com/index.php?title=TextManager, though heavily modified and expanded
    public void LoadLanguage(string lang) {
        CurrentLanguage = lang;
        PlayerPrefs.SetString("lang", lang);
        if (lang == "français") {
            UpdateAllTextBoxes();
        } else if (lang != "français") {
            string fullpath = "Languages/" + lang + ".po"; // the file is actually ".txt" in the end

            TextAsset textAsset = (TextAsset)Resources.Load(fullpath);
            if (textAsset == null) {
                Debug.Log("[TextManager] " + fullpath + " file not found.");
                return;
            } else {
                Debug.Log("[TextManager] loading: " + fullpath);

                if (textTable == null) {
                    textTable = new Hashtable();
                }

                textTable.Clear();

                StringReader reader = new StringReader(textAsset.text);
                string key = null;
                string val = null;
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("msgid \"")) {
                        key = line.Substring(7, line.Length -8);
                    } else if (line.StartsWith("msgstr \"")) {
                        val = line.Substring(8, line.Length -9);
                    } else {
                        if (key != null && val != null) {
                            // TODO: add error handling here in case of duplicate keys
                            textTable.Add(key, val);
                            key = val = null;
                        }
                    }
                }
                UpdateAllTextBoxes();
                reader.Close();
            }   
        }
    }

    // This handles selecting the value from the translation array
    // and returning it, the UILocalizeText calls this
    public string GetText(string key) {
        string result = "";
        if (key != null && textTable != null) {
            if (textTable.ContainsKey(key)) {
                result = (string)textTable[key];
            }
        }
        return (string)result;
    }
}
