using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pressStatue : MonoBehaviour {
    public float distance;
    private GameObject mainCamera;
    private GameManager gameManager;
    private GameObject texte;
    private EnigmeEsoterique enigmeEsoterique;
    private EnigmeTonneaux enigmeTonneaux;
    private EnigmeStatues enigmeStatues;
    private bool isSelected;

    void Start() {
        this.enigmeEsoterique = FindObjectOfType<EnigmeEsoterique>();
        this.enigmeTonneaux = FindObjectOfType<EnigmeTonneaux>();
        this.enigmeStatues = FindObjectOfType<EnigmeStatues>();
        this.mainCamera = GameObject.FindWithTag("MainCamera");
        this.gameManager = FindObjectOfType<GameManager>();
        this.texte = GameObject.FindWithTag("text");
        this.isSelected = false;
    }

    void Update() {
        //Si on est pas dans les options
        if (gameManager.getOption().getInOption() == false) {
            action();
        }
    }

    void action() {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance)) {
            Button button = hit.collider.GetComponent<Button>();
            if (button != null) {
                isSelected = true;
                if (button.buttonId > 0 && button.buttonId < 6) {
                    this.texte.GetComponent<Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Interagir";
                    this.texte.GetComponent<UILocalizeText>().UpdateTranslation();
                    if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                        Color buttonColor = button.GetComponent<Renderer>().material.color;
                        StartCoroutine(enigmeEsoterique.ColorButtonCoroutine(buttonColor, button));
                        enigmeEsoterique.enigmeEsoterique(button);
                    }
                }
                if (button.buttonId > 5 && button.buttonId < 12 && this.enigmeTonneaux != null) {
                    this.texte.GetComponent<Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Interagir";
                    this.texte.GetComponent<UILocalizeText>().UpdateTranslation();
                    if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                        enigmeTonneaux.enigmeTonneaux(button);
                    }
                }

            }
            else {
                if (this.isSelected == true) {
                    texte.GetComponent<UILocalizeText>().Clear();
                    this.isSelected = false;
                }
            }
        } else {
            if (this.isSelected == true) {
                texte.GetComponent<UILocalizeText>().Clear();
                this.isSelected = false;
            }
        }
    }

    public void setEnigmeEsoterique(EnigmeEsoterique enigme) {
        this.enigmeEsoterique = enigme;
    }

    public void setEnigmeTonneaux(EnigmeTonneaux enigme) {
        this.enigmeTonneaux = enigme;
    }
}