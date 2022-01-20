using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class openDoor : MonoBehaviour {
    private GameObject mainCamera;
    private PlayerManager playerManager;
    private Door door_selected;
    public float distance;
    private Color startcolor;
    private GameObject texte;
    private GameManager gameManager;
    private bool isOpeningDoor = false;

    void Start() {
        texte = GameObject.FindWithTag("text");
		mainCamera = GameObject.FindWithTag("MainCamera");
        playerManager = FindObjectOfType<PlayerManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        // Si on est pas dans les options
        if(gameManager.getOption().getInOption() == false){
            openTheDoor();
        }
    }

    void openTheDoor() {
        int x = Screen.width /2;
        int y = Screen.height / 2;
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        Inventory inventory = playerManager.getInventory();
        bool key_find = false;

        if (Physics.Raycast(ray, out hit, distance)) {
            Door door = hit.collider.GetComponent<Door>();
            if (door != null) {
                string door_text = "Vous ne possédez pas la clef adéquate pour ouvrir la porte !";
                foreach(Item clef in inventory.getKeys()) {
                    if(clef.getId() == door.getItemId()) {
                        door_text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Ouvrir la porte";
                        key_find = true;
                    }
                }
                int key_id = door.getItemId();
                if(door_selected != door) {
                    // On redonne sa couleur d'origine à la porte précedemment selectionnée
                    if(door_selected != null && startcolor != null){
                        door_selected.gameObject.GetComponent<Renderer>().material.color = startcolor;
                    }
                    // L'item selectionné devient le nouveau
                    door_selected = door;
                    // La couleur d'origine devient la couleur de l'item selectionné
                    startcolor = door.gameObject.GetComponent<Renderer>().material.color;
                    // Le nouvel item devient bleu
                    if(key_find == true) {
                        door.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                    } else {
                        door.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                }

                texte.GetComponent<Text>().text = door_text;
                texte.GetComponent<UILocalizeText>().UpdateTranslation();
                if (key_find == true) {
                    if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                        door.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
                        door.gameObject.GetComponent<AudioSource>().Play();
                        StartCoroutine(changeScene(door.getScene()));
                    }
                }
            } else {
                //Si item_selected n'est pas null
                if(door_selected!=null) {
                    //On redonne sa couleur d'origine à l'item pour attester qu'il n'est plus selectionné
                    door_selected.gameObject.GetComponent<Renderer>().material.color = startcolor;
                    //On donne la valeur null à door_selected pour permettre à l'utilisateur de reselectionné le même objet sans avoir à en selectionné un autre
                    door_selected = null;
                    //On reinitialise le texte
                    texte.GetComponent<UILocalizeText>().Clear();
                }
            }
        } else {
            //Si item_selected n'est pas null
            if(door_selected!=null) {
                //On redonne sa couleur d'origine à l'item pour attester qu'il n'est plus selectionné
                door_selected.gameObject.GetComponent<Renderer>().material.color = startcolor;
                //On donne la valeur null à door_selected pour permettre à l'utilisateur de reselectionné le même objet sans avoir à en selectionné un autre
                door_selected = null;
                //On reinitialise le texte
                texte.GetComponent<UILocalizeText>().Clear();
            }
        }
    }

    //Fonction permettant de changer de scene avec un fondu au noir
    //scene : le nom de la scene où on veut aller
    IEnumerator changeScene(string scene) {
        this.isOpeningDoor = true;
        Image image = GameObject.FindWithTag("fondu_noir").GetComponent<Image>();
        Color cl = image.color;
        while(cl.a<1) {
            cl.a += 0.005f;
            image.color = cl;
            yield return null;
        }
        SceneManager.LoadScene(scene);
        cl.a = 0;
        image.color = cl;
        texte.GetComponent<UILocalizeText>().Clear();
        this.isOpeningDoor = false;
        if(scene == "game_Cathedrale") {
            GameObject.FindWithTag("Player").transform.position = new Vector3(-0.1f,1.08f,50f);
            GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0,180f,0);

            gameManager.getDialogueManager().setIdDialogue();
            gameManager.getDialogueManager().StartDialogue();
        }
    }

    public bool getIsOpeningDoor() {
        return this.isOpeningDoor;
    }
}
