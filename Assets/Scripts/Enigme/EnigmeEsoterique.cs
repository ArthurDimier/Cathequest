using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnigmeEsoterique : MonoBehaviour {

    private Button bouton1;
    private Button bouton2;
    private Button bouton3;
    private Button bouton4;
    private List<string> listButtons;
    private GameObject texte;




    void Start() {
        listButtons = new List<string>();
    }

    public void enigmeEsoterique(Button button) {
        // La solution est 32415

        this.texte = GameObject.FindWithTag("texte_enigme");
        if (listButtons.Count == 0) {
            if (button.buttonName == "3") {
                listButtons.Add("3");
            } else {
                listButtons.Clear();
                StartCoroutine(TextCoroutine("Tu t'es trompé ! Réinitialisation du système d'ouverture des portes."));
            }
        } else if (listButtons.Count == 1) {
            if (button.buttonName == "2") {
                listButtons.Add("2");
            } else {
                listButtons.Clear();
                StartCoroutine(TextCoroutine("Tu t'es trompé ! Réinitialisation du système d'ouverture des portes."));
            }
        } else if (listButtons.Count == 2) {
            if (button.buttonName == "4") {
                listButtons.Add("4");
            } else {
                listButtons.Clear();
                StartCoroutine(TextCoroutine("Tu t'es trompé ! Réinitialisation du système d'ouverture des portes."));
            }
        } else if (listButtons.Count == 3) {
            if (button.buttonName == "1") {
                listButtons.Add("1");
            } else {
                listButtons.Clear();
                StartCoroutine(TextCoroutine("Tu t'es trompé ! Réinitialisation du système d'ouverture des portes."));
            }
        } else if (listButtons.Count == 4) {
            if (button.buttonName == "5") {
                listButtons.Add("5");
                AudioSource son = GameObject.FindWithTag("congratulations_sound").GetComponent<AudioSource>();
                son.Play();
                son.volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
                GameObject passageSecret = GameObject.FindWithTag("passage_secret");
                GameObject.FindObjectOfType<GameManager>().getDialogueManager().setIdDialogue();
                GameObject.FindObjectOfType<GameManager>().getDialogueManager().StartDialogue();
                StartCoroutine(moveMur(passageSecret));
            } else {
                listButtons.Clear();
                StartCoroutine(TextCoroutine("Tu t'es trompé ! Réinitialisation du système d'ouverture des portes."));
            }
        }

    }

    public IEnumerator TextCoroutine(string message) {
        texte.GetComponent<Text>().text = message;
        texte.GetComponent<UILocalizeText>().UpdateTranslation();
        yield return new WaitForSeconds(1);
        texte.GetComponent<UILocalizeText>().Clear();
    }

    // Fonction permettant de faire clignoter le bouton
    // color : la couleur d'origine du bouton
    // button : le bouton
    public IEnumerator ColorButtonCoroutine(Color color, Button button) {

        button.GetComponent<Renderer>().material.color = Color.red;


        yield return new WaitForSeconds(0.5f);

        button.GetComponent<Renderer>().material.color = color;
    }

    // On translate le mur
    // passageSecret : le gameObject du passage secret
    public IEnumerator moveMur(GameObject passageSecret) {
        float time = 0;
        while(time < 2.0f){
            passageSecret.transform.Translate(-passageSecret.transform.forward * 0.05f * 1.5f);
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        pressButton press = FindObjectOfType<pressButton>();
        press.setEnigmeEsoterique(null);
        Destroy(this);
    }
}