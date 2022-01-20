using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalizeText : MonoBehaviour {

    public bool isInteraction = false;
    // This instances key, as well as the french translation
    [HideInInspector]
    public string TranslationKey = "";

    [HideInInspector]
    public Text TextToTranslate;

    // References to text values
    private string OriginalText = "";


    void Start() {}

    // This gets run automatically if the original text
    // hasn't been set when you go to update it.
    // You shouldn't need to manually run this from anywhere.
    public void Init() {

        // Grab the TextToTranslate if we haven't
        if (TextToTranslate == null) {
            TextToTranslate = GetComponent<Text>();
        }

        // Grab the original value of the text before we update if
        if (TextToTranslate != null) {
            OriginalText = TextToTranslate.text;
        }

        // Set the translation key to the original french text
        if (TranslationKey == "") {
            TranslationKey = OriginalText;
        }
    }

    // This function is called from scripts whenever a text need to be clear.
    public void Clear() {
        TranslationKey = "";
        TextToTranslate = GetComponent<Text>();
        OriginalText = "";
        TextToTranslate.text = "";
    }

    // This gets called from LanguageManager and other script when needed
    public void UpdateTranslation() {
        // If original text is empty, then this object hasn't been initiated so
        // it should do that

        if (OriginalText == "") {
            Init();
        }

        // If the object has no Text object then we shouldn't try to set
        // the text so just stop
        if (TextToTranslate == null) {
            return;
        }

        if (PlayerPrefs.GetString("lang") != "français" && TranslationKey != "") {
            string newText = "";
            string key = "";
            if (!isInteraction || TranslationKey.IndexOf("]") == -1) {
                newText = FindObjectOfType<GameManager>().languageManager.GetText(TranslationKey);
            } else if(isInteraction && TranslationKey.IndexOf("]") > -1) {
                key = TranslationKey.Substring(TranslationKey.IndexOf("]")+2);
                newText = FindObjectOfType<GameManager>().languageManager.GetText(key);

                if (newText != "") {
                    newText = "[" + PlayerPrefs.GetString("Interaction", "E") + "] " + newText;
                }
            }
            
            if (newText != "") {
                TextToTranslate.text = newText;
            } else {
                if (!isInteraction || TranslationKey.IndexOf("]") == -1) {
                    Debug.Log("[UILocalizeText] Key " + OriginalText + " doesn't have an entry in this language.");
                } else {
                    Debug.Log("[UILocalizeText] Key " + key + " doesn't have an entry in this language.");
                }
            }

        } else if (PlayerPrefs.GetString("lang") == "français") {
            if (TextToTranslate != null && OriginalText != null) {
                TextToTranslate.text = OriginalText;
            }
        }
    }
}
