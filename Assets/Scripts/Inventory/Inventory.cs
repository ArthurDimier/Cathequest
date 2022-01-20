using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	private List<Item> objets = new List<Item>(); // Liste des objets présents dans l'inventaire
	private List<Item> clefs = new List<Item>(); // Liste des clefs possédées par le joueur
	private List<Item> journaux = new List<Item>(); // Liste des clefs possédées par le joueur
	private bool isOpen;
	private GameObject inventoryInterface;
	private GameManager gameManager;
	private GameObject texteDialogue;
	private GameObject nomDialogue;
	private GameObject panelTexteDialogue;
	private GameObject panelNomDialogue;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        //Si on presse la touche d'inventaire + on est dans une scene du jeu + on est pas dans les options
        if (Input.inputString.ToUpper() == PlayerPrefs.GetString("Inventory", "I") && gameManager.getCurrentScene().StartsWith("game") == true && gameManager.getOption().getInOption() == false) {
            if(this.isOpen == false) {
                //On ouvre l'inventaire
                onIsOpen();
            } else {
                //On ferme l'inventaire
                onIsClose();
            }
        }
    }

    //Fonction appelée lorsque l'on ouvre l'inventaire
    void onIsOpen() {
        //On rend visible l'inventaire
        showInventory(true);
        this.isOpen = true;
        //On rend le curseur visible
        Cursor.visible = true;
        //On debloque le curseur
        Cursor.lockState = CursorLockMode.None;
        GameObject varGameObject = GameObject.Find("MainCamera");
        //On empêche la camera de suivre le mouvement de la souris
        varGameObject.GetComponent<mouseLook>().enabled = false;
	}

    //Fonction appelée lorsque l'on ferme l'inventaire
	public void onIsClose() {
		this.texteDialogue = GameObject.FindWithTag("texte_dialogue");
		this.nomDialogue = GameObject.FindWithTag("nom_dialogue");
		this.panelNomDialogue = GameObject.FindWithTag("nom_dialogue_panel");
		this.panelTexteDialogue = GameObject.FindWithTag("texte_dialogue_panel");
        //On cache l'inventaire
        showInventory(false);
        this.isOpen = false;
        //On cache le curseur
        Cursor.visible = false;
        //On bloque le curseur
        Cursor.lockState = CursorLockMode.Locked;
        GameObject varGameObject = GameObject.Find("MainCamera");
        //On permet à la camera de suivre le mouvement de la souris
        varGameObject.GetComponent<mouseLook>().enabled = true;
        // On remet à null les valeurs des dialogues et on remet les panels en transparent
        Image imageNomPanel = this.panelNomDialogue.GetComponent<Image>();
        Image imageTextePanel = this.panelTexteDialogue.GetComponent<Image>();
        Color colorImageNomPanel = imageNomPanel.color;
        Color colorImageTextePanel = imageTextePanel.color;
        colorImageNomPanel.a = 0;
        colorImageTextePanel.a = 0;
        imageNomPanel.color = colorImageNomPanel;
        imageTextePanel.color = colorImageTextePanel;
        this.nomDialogue.GetComponent<UILocalizeText>().Clear();
        this.texteDialogue.GetComponent<UILocalizeText>().Clear();
    }

    //Initialiste l'interface de l'inventaire
    public void loadInventory() {
        //On cherche le gameObject de l'inventaire et on le cache
        this.inventoryInterface = GameObject.FindWithTag("inventory_interface");
        showInventory(false);
    }

    // Ajoute un item à l'inventaire
    // item : un item
    public void addItem(Item item) {
        objets.Add(item);
        // Debug.Log("[Inventory] Object " + item.getNom() + " added.");
    }

    // Supprime un item de l'inventaire
    // item : un item
    public void removeItem(Item item) {
        int idx = objets.IndexOf(item);
        if (idx != -1) {
            objets.Remove(item);
        }
        // Debug.Log("[Inventory] Object " + item.getNom() + " removed.");
    }

    //Supprime un item de l'inventaire grâce à son id
    // id : l'id de l'item
    public void removeItemById(int id) {
        foreach(Item item in this.objets) {
            if(item.getId() == id) {
                objets.Remove(item);
                // Debug.Log("[Inventory] Object " + item.getNom() + " removed.");
                break;
            }
        }
    }

    // Ajoute une clef à l'inventaire
    // clef : un item clef
    public void addKey(Item clef) {
        clefs.Add(clef);
        // Debug.Log("[Inventory] Key " + clef.getNom() + " added.");
    }

    // Ajoute un journal à l'inventaire
    // journal : un item journal
    public void addJournal(Item journal) {
        journaux.Add(journal);
        // Debug.Log("[Inventory] journaux " + journal.getNom() + " added.");
    }

    public List<Item> getKeys() {
        return this.clefs;
    }

    public List<Item> getJournaux() {
        return this.journaux;
    }

    public List<Item> getItems() {
        return this.objets;
    }

    public bool getIsOpen() {
        return this.isOpen;
    }

    public void setIsOpen(bool state) {
        this.isOpen = state;
    }

    public void clearInventaireClefs() {
        this.clefs.Clear();
    }

    public void clearInventaireJournaux() {
        this.journaux.Clear();
    }

    public void clearInventaireObjets() {
        this.objets.Clear();
    }

    // Rend visible ou invisible l'inventaire
    // state : l'inventaire doit-il être visible ?
    public void showInventory(bool state) {
        this.inventoryInterface.SetActive(state);
    }

    public GameObject getInventoryInterface() {
        return this.inventoryInterface;
    }

}
