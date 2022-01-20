using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public string nom;
    public Sprite sprite;
    public int id;
    public string description;
    public AudioClip son = null;
    public bool isKey;
    public string link_prefab = null;
    public string texte = null;

    public bool getIsKey() {
        return this.isKey;
    }

    public string getNom() {
        return this.nom;
    }

    public Sprite getSprite() {
        return this.sprite;
    }

    public string getTexte() {
      return this.texte;
    }
    
    public int getId() {
        return this.id;
    }

    public string getDescription() {
        return this.description;
    }

    public AudioClip getSon() {
        return this.son;
    }

    public string getPrefab() {
        return this.link_prefab;
    }
}
