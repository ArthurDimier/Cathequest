using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onglet : MonoBehaviour {
    public GameObject inventoryKey;
    public GameObject inventoryObject;
    public GameObject inventoryJournaux;
    public GameObject buttonKey;
    public GameObject buttonObject;
    public GameObject buttonJournaux;


    void Start() {
        showKey();
    }

    //Fonction permettant de voir l'onglet des clés
    public void showKey() {
        this.inventoryKey.SetActive(true);
        this.inventoryObject.SetActive(false);
        this.inventoryJournaux.SetActive(false);
        buttonKey.GetComponent<Image>().color = Color.red;
        buttonObject.GetComponent<Image>().color = Color.white;
        buttonJournaux.GetComponent<Image>().color = Color.white;
    }

    //Fonction permettant de voir l'onglet des objets simples
    public void showObject() {
        this.inventoryObject.SetActive(true);
        this.inventoryKey.SetActive(false);
        this.inventoryJournaux.SetActive(false);
        buttonObject.GetComponent<Image>().color = Color.red;
        buttonKey.GetComponent<Image>().color = Color.white;
        buttonJournaux.GetComponent<Image>().color = Color.white;
    }

    //Fonction permettant de voir l'onglet des journaux
    public void showJournaux() {
        this.inventoryJournaux.SetActive(true);
        this.inventoryObject.SetActive(false);
        this.inventoryKey.SetActive(false);
        buttonJournaux.GetComponent<Image>().color = Color.red;
        buttonKey.GetComponent<Image>().color = Color.white;
        buttonObject.GetComponent<Image>().color = Color.white;
    }
}
