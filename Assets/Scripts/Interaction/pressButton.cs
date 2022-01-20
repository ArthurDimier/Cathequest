using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressButton : MonoBehaviour {
    public float distance;
    private GameObject mainCamera;
    private GameManager gameManager;
    private GameObject texte;
    private EnigmeEsoterique enigmeEsoterique;
    private EnigmeTonneaux enigmeTonneaux;
    private EnigmeStatues enigmeStatues;
    private bool coffreOuvert;
    private Button buttonStatue;
    private Color colorButtonStatue;
    private bool isSelected;

    void Start() {
        this.enigmeEsoterique = FindObjectOfType<EnigmeEsoterique>();
        this.enigmeTonneaux = FindObjectOfType<EnigmeTonneaux>();
        this.enigmeStatues = FindObjectOfType<EnigmeStatues>();
        this.mainCamera = GameObject.FindWithTag("MainCamera");
        this.gameManager = FindObjectOfType<GameManager>();
        this.texte = GameObject.FindWithTag("text");
        this.coffreOuvert = false;
        this.isSelected = false;
        this.buttonStatue = null;
        this.colorButtonStatue = Color.white;
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
                //Enigme esoterique
                if (button.buttonId > 0 && button.buttonId < 6 && this.enigmeEsoterique != null) {
                    this.texte.GetComponent<UnityEngine.UI.Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Interagir";
                    this.texte.GetComponent<UILocalizeText>().UpdateTranslation();
                    if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                        Color buttonColor = button.GetComponent<Renderer>().material.color;
                        StartCoroutine(enigmeEsoterique.ColorButtonCoroutine(buttonColor, button));
                        enigmeEsoterique.enigmeEsoterique(button);
                    }
                }
                //Enigme des tonneaux
                if (button.buttonId > 5 && button.buttonId < 12 && this.enigmeTonneaux != null) {
                    this.texte.GetComponent<UnityEngine.UI.Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Interagir";
                    this.texte.GetComponent<UILocalizeText>().UpdateTranslation();
                    if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                        enigmeTonneaux.enigmeTonneaux(button);
                    }
                }
                // Enigme de la statue
                if (button.buttonId > 11 && button.buttonId < 16) {
                    buttonStatue = button;
                    colorButtonStatue = button.GetComponent<Renderer>().material.color;

                    button.GetComponent<Renderer>().material.color = Color.red;
                    this.texte.GetComponent<UnityEngine.UI.Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Interagir";
                    this.texte.GetComponent<UILocalizeText>().UpdateTranslation();
                    if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                        enigmeStatues.enigmeStatues(button);
                    }
                }
                //Enigme du coffre
                if (button.buttonId == 16 && this.coffreOuvert == false) {
                    Inventory inventory = FindObjectOfType<Inventory>();
                    bool key_find = false;
                    //On regarde si le joueur possède bien la clé
                    foreach (Item clef in inventory.getKeys()) {
                        if (clef.getId() == 1) {
                            key_find = true;
                        }
                    }
                    if (key_find == true) {
                        this.texte.GetComponent<UnityEngine.UI.Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Ouvrir";
                        this.texte.GetComponent<UILocalizeText>().UpdateTranslation();
                        if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                            this.coffreOuvert = true;
                            GameObject couvercle = GameObject.FindWithTag("couvercle");
                            StartCoroutine(ouvrirCoffre(couvercle));
                        }
                    }
                }
            } else {
                if (this.isSelected == true) {
                    texte.GetComponent<UILocalizeText>().Clear();
                    if(buttonStatue != null){
                        buttonStatue.GetComponent<Renderer>().material.color = Color.white;
                    }
                    this.isSelected = false;
                }
            }
        } else {
            if(this.isSelected == true) {
                texte.GetComponent<UILocalizeText>().Clear();
                if(buttonStatue != null) {
                    buttonStatue.GetComponent<Renderer>().material.color = Color.white;
                }
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

    public void setEnigmeStatues(EnigmeStatues enigme) {
        this.enigmeStatues = enigme;
    }

    //Fonction permettant d'ouvrir le coffre et de finir l'épreuve des tonneaux
    //couvercleCoffre : gameObject du couvercle du coffre
    public IEnumerator ouvrirCoffre(GameObject couvercleCoffre) {
        float time = 0;
        Vector3 myRotation = new Vector3(-90f, 0f, 0f);
        while(time < 1.0f) {
            myRotation.x -= 1.0f ;
            couvercleCoffre.transform.rotation = Quaternion.Euler(myRotation);
            time += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        AudioSource son = GameObject.FindWithTag("congratulations_sound").GetComponent<AudioSource>();
        son.Play();
        son.volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
        GameObject prefab = Resources.Load("_Prefabs/Journal/Journal_du_coffre") as GameObject;
        Quaternion prefabRotation = prefab.transform.rotation;
        Vector3 position = new Vector3(0.115f, 1.066f, -49.906f);
        GameObject item = Instantiate(prefab, position, prefabRotation) as GameObject;

        GameObject.FindObjectOfType<GameManager>().getDialogueManager().setIdDialogue();
        GameObject.FindObjectOfType<GameManager>().getDialogueManager().StartDialogue();


        Destroy(GameObject.Find("PlaqueDestroy"));
    }
}