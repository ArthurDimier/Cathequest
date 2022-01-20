using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    public int doorId;
    public int itemId; // l'id de la clef liée à la porte
    public string scene; // nom de la scène de destination
    public bool isOpen = false; // porte ouverte ou fermée

    void Start() {
    }

    public int getDoorId() {
      return this.doorId;
    }

    public int getItemId() {
      return this.itemId;
    }

    public string getScene() {
      return this.scene;
    }
}
