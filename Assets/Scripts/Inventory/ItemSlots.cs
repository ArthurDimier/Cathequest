using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour {
    public Text textItem;
    public GameObject textDisplay;
    public string itemNom;
    public int itemID;
    public Sprite itemSprite;
    public string itemDescription;
    public string itemTexte;
    public bool isKey;
    public AudioClip son;
    public string link_prefab = null;
    private GameObject texteDialogue;
    private GameObject nomDialogue;
    private GameObject panelTexteDialogue;
    private GameObject panelNomDialogue;

    void Start() {
        GetComponent<Image>().sprite = itemSprite;
        this.texteDialogue = GameObject.FindWithTag("texte_dialogue");
        this.nomDialogue = GameObject.FindWithTag("nom_dialogue");
        textItem.text = itemDescription;
        textItem.gameObject.GetComponent<UILocalizeText>().UpdateTranslation();
    }

    public void OnEnable() {
        DisableText();
    }

    public void ActiveText() {
        textDisplay.SetActive(true);
    }

    public void DisableText() {
        textDisplay.SetActive(false);
    }

    public void setItemSlot(int id) {
        //Initailise l'item slot
        this.itemID = id;
    }

    //Fonction permettant d'utiliser un item de l'inventaire
    public void action() {
        //Si l'item est un journal
        if(this.itemNom.Contains("Journal") || this.itemNom.Contains("journal")) {
            //On joue le son du journal
            playSound();
            this.nomDialogue.GetComponent<UILocalizeText>().Clear();
            this.nomDialogue.GetComponent<Text>().text = itemNom;
            this.nomDialogue.GetComponent<UILocalizeText>().UpdateTranslation();
            this.texteDialogue.GetComponent<UILocalizeText>().Clear();
            this.texteDialogue.GetComponent<Text>().text = itemTexte;
            this.texteDialogue.GetComponent<UILocalizeText>().UpdateTranslation();
            this.panelNomDialogue = GameObject.FindWithTag("nom_dialogue_panel");
            this.panelTexteDialogue = GameObject.FindWithTag("texte_dialogue_panel");
            Image imageNomPanel = this.panelNomDialogue.GetComponent<Image>();
            Image imageTextePanel = this.panelTexteDialogue.GetComponent<Image>();
            Color colorImageNomPanel = imageNomPanel.color;
            Color colorImageTextePanel = imageTextePanel.color;
            colorImageNomPanel.a = 255;
            colorImageTextePanel.a = 255;
            imageNomPanel.color = colorImageNomPanel;
            imageTextePanel.color = colorImageTextePanel;
        } else if(this.isKey == false) {
            putItem();
        }
    }

    //Fonction permettant d'ecouter le son d'un item
    public void playSound() {
        //On arrete tous les sons
        AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach(AudioSource audio in audios) {
            //Si l'audio n'est pas attaché à un gameObject qui gère la musique
            if(audio.gameObject.name != "Musique"){
                audio.Stop();
            }
        }
        //Si il y a un son sur l'item
        if(this.son != null) {
            gameObject.GetComponent<AudioSource>().clip = this.son;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    //Fonction permettant de poser un item
    public void putItem() {
        //On instancie l'objet dans la scene
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        GameObject prefab = Resources.Load(this.link_prefab) as GameObject;
        Quaternion prefabRotation = prefab.transform.rotation;
        GameObject item = Instantiate(prefab, playerPosition + (player.transform.forward * 1), prefabRotation) as GameObject;

        //On supprime l'objet de l'inventaire
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.removeItemById(item.GetComponent<Item>().getId());

        //On actualise l'interface
        inventoryInterface invInterface = FindObjectOfType<inventoryInterface>();
        invInterface.refresh();
    }
}
