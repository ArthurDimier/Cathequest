using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryInterface : MonoBehaviour {
    public Transform inventorySlotsKey;
    public Transform inventorySlotsObject;
    public Transform inventorySlotsJournaux;
    private Inventory inventory;
    public Transform itemPrefab;

    //Raffrachissement de l'interface pour qu'elle soit synchronisée avec l'inventaire
    public void refresh() {

        this.inventory = FindObjectOfType<Inventory>();

        //On supprime les items de l'interface avant de refresh
        //On detruit les clés
        foreach (Transform child in inventorySlotsKey) {
            Destroy(child.gameObject);
        }
        //On detruit les items simples
        foreach (Transform child in inventorySlotsObject) {
            Destroy(child.gameObject);
        }
        //On detruit les journaux
        foreach (Transform child in inventorySlotsJournaux) {
            Destroy(child.gameObject);
        }

        //On ajoute les clés
        foreach(Item key in inventory.getKeys()) {
            Transform newItem;
            newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity) as Transform;
            newItem.SetParent(inventorySlotsKey, false);
            ItemSlots itemInventory = newItem.GetComponent<ItemSlots>();
            itemInventory.itemID = key.getId();
            itemInventory.itemDescription = key.getDescription();
            itemInventory.itemSprite = key.getSprite();
            itemInventory.itemNom = key.getNom();
            itemInventory.son = key.getSon();
            itemInventory.isKey = key.getIsKey();
        }

        //On ajoute les journaux
        foreach(Item journal in inventory.getJournaux()) {
            Transform newItem;
            newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity) as Transform;
            newItem.SetParent(inventorySlotsJournaux, false);
            ItemSlots itemInventory = newItem.GetComponent<ItemSlots>();
            itemInventory.itemID = journal.getId();
            itemInventory.itemDescription = journal.getDescription();
            itemInventory.itemSprite = journal.getSprite();
            itemInventory.itemNom = journal.getNom();
            itemInventory.son = journal.getSon();
            itemInventory.itemTexte = journal.getTexte();
            itemInventory.isKey = journal.getIsKey();
        }

        //On ajoute les items simples
        foreach(Item item in inventory.getItems()) {
            Transform newItem;
            newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity) as Transform;
            newItem.SetParent(inventorySlotsObject, false);
            ItemSlots itemInventory = newItem.GetComponent<ItemSlots>();
            itemInventory.itemID = item.getId();
            itemInventory.itemDescription = item.getDescription();
            itemInventory.itemSprite = item.getSprite();
            itemInventory.itemNom = item.getNom();
            itemInventory.son = item.getSon();
            itemInventory.isKey = item.getIsKey();
            itemInventory.link_prefab = item.getPrefab();
        }
    }
}
