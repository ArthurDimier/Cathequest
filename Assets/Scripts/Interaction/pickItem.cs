using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickItem : MonoBehaviour {
    private GameObject mainCamera;
    private PlayerManager playerManager;
    private Item item_selected;
    public float distance;
    private Color startcolor;
    private GameObject texte;
    private GameObject texte_nom_item;
    private GameObject texte_description_item;
    private GameObject image_item;
    private GameObject image_item_group;
    private GameManager gameManager;
    public int slotCount = 16;
    public GameObject canvasInventory;

    // Start is called before the first frame update
    void Start() {
        this.texte = GameObject.FindWithTag("text");
        this.texte_nom_item = GameObject.FindWithTag("nom_item");
        this.texte_description_item = GameObject.FindWithTag("description_item");
        this.image_item = GameObject.FindWithTag("image_item");
        this.image_item_group = GameObject.FindWithTag("item_group");
		this.mainCamera = GameObject.FindWithTag("MainCamera");
        this.playerManager = FindObjectOfType<PlayerManager>();
        this.gameManager = FindObjectOfType<GameManager>();

        //En premier lieu, il n'y a pas d'item
        display_item("", "", null, false);
        texte_nom_item.GetComponent<UILocalizeText>().Clear();
        texte_description_item.GetComponent<UILocalizeText>().Clear();

        //On ajoute les items qui doivent être directement dans l'inventaire
        Item journal1 = GameObject.FindWithTag("journal_1").GetComponent<Item>();
        Item cle_cathedrale = GameObject.FindWithTag("cle_cathedrale").GetComponent<Item>();
        Item cle_coffre = GameObject.FindWithTag("cle_coffre").GetComponent<Item>();
        pickTheItem(journal1);
        pickTheItem(cle_cathedrale);
        pickTheItem(cle_coffre);
    }

    // Update is called once per frame
    void Update() {
        //Si on est pas dans les options
        if(gameManager.getOption().getInOption() == false){
            pickup();
        }
    }

    //Ajoute l'item à l'inventaire
    //item : un item
    public void pickTheItem(Item item) {
        bool canAdd = false;
        if(item.getIsKey()==false) {
            if(item.getNom().Contains("Journal") == false && item.getNom().Contains("journal") == false) {
                if(this.playerManager.getInventory().getItems().Count < 16) {
                    canAdd = true;
                    this.playerManager.getInventory().addItem(item);
                }
            }
            else {
                if(this.playerManager.getInventory().getJournaux().Count < 16) {
                    canAdd = true;
                    this.playerManager.getInventory().addJournal(item);
                }
            }
        } else {
            if(this.playerManager.getInventory().getKeys().Count < 16) {
                canAdd = true;
                this.playerManager.getInventory().addKey(item);
            }
        }

        if(canAdd == true) {
            inventoryInterface interfaceInv = this.canvasInventory.GetComponent<inventoryInterface>();
            interfaceInv.refresh();
            //On fait disparaitre l'item de la scene
            Destroy(item.gameObject);
            //On reinitialise le texte
            this.texte.GetComponent<UILocalizeText>().Clear();
            display_item("", "", null, false);
            texte_nom_item.GetComponent<UILocalizeText>().Clear();
            texte_description_item.GetComponent<UILocalizeText>().Clear();
        } else {
            Debug.Log("Inventory is full");
        }

    }

    void pickup() {
  		int x = Screen.width / 2;
  		int y = Screen.height / 2;

  		Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
  		RaycastHit hit;

        //Si la camera à un objet au milieu de son champ de vision
  		if(Physics.Raycast(ray, out hit, distance)) {
            //On récupère le script item de cet objet
  			Item p = hit.collider.GetComponent<Item>();

            //Si l'objet est bien un item
  			if(p != null) {
                //Si l'item actuel est différent du précédent
                if(item_selected != p) {
                    //On redonne sa couleur d'origine à l'item précedemment selectionné.
                    if(item_selected != null && startcolor != null) {
                        item_selected.gameObject.GetComponent<Renderer>().material.color = startcolor;
                    }
                    //L'item selectionné devient le nouveau
                    item_selected = p;
                    //La couleur d'origine devient la couleur de l'item selectionné
                    startcolor = p.gameObject.GetComponent<Renderer>().material.color;
                    //Le nouvel item devient bleu
                    p.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                    //On affiche le texte
                    texte.GetComponent<Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Prendre";
                    texte.GetComponent<UILocalizeText>().UpdateTranslation();
                    display_item(p.getNom(), p.getDescription(), p.getSprite(), true);
                }

                //Si l'utilisateur utilise la touche d'interaction
                if(Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E")) {
                    //On récupère l'item
                    AudioSource son = GameObject.FindWithTag("pick_item_sound").GetComponent<AudioSource>();
                    son.Play();
                    son.volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
                    pickTheItem(p);
  				}
  			} else {
                //Si item_selected n'est pas null
                if(item_selected!=null) {
                    //On redonne sa couleur d'origine à l'item pour attester qu'il n'est plus selectionné
                    item_selected.gameObject.GetComponent<Renderer>().material.color = startcolor;
                    //On donne la valeur null à item_selected pour permettre à l'utilisateur de reselectionné le même objet sans avoir à en selectionné un autre
                    item_selected = null;
                    //On reinitialise le texte
                    texte.GetComponent<UILocalizeText>().Clear();
                    display_item("", "", null, false);
                    texte_nom_item.GetComponent<UILocalizeText>().Clear();
                    texte_description_item.GetComponent<UILocalizeText>().Clear();
                }
            }
        } else {
            //Si item_selected n'est pas null
            if(item_selected!=null) {
                //On redonne sa couleur d'origine à l'item pour attester qu'il n'est plus selectionné
                item_selected.gameObject.GetComponent<Renderer>().material.color = startcolor;
                //On donne la valeur null à item_selected pour permettre à l'utilisateur de reselectionné le même objet sans avoir à en selectionné un autre
                item_selected = null;
                //On reinitialise le texte
                texte.GetComponent<UILocalizeText>().Clear();
                display_item("", "", null, false);
                texte_nom_item.GetComponent<UILocalizeText>().Clear();
                texte_description_item.GetComponent<UILocalizeText>().Clear();
            }
        }
    }

    //Fonction permettant de modifier l'affichage du popup d'un objet
    //item_name : le nom de l'item
    //item_description : la description de l'item
    //item_sprite : le sprite de l'item
    //visibility : le popup doit-il s'afficher ?
    void display_item(string item_name, string item_description, Sprite item_sprite, bool visibility) {
        if (item_name != null) {
            texte_nom_item.GetComponent<Text>().text = item_name;
            texte_nom_item.GetComponent<UILocalizeText>().UpdateTranslation();
        }
        if (item_description != null) {
            texte_description_item.GetComponent<Text>().text = item_description;
            texte_description_item.GetComponent<UILocalizeText>().UpdateTranslation();
        }
        image_item.GetComponent<Image>().sprite = item_sprite;
        image_item.SetActive(visibility);
        image_item_group.SetActive(visibility);
    }
}
