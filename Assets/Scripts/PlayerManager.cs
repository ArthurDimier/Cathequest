using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public Inventory inventory;

    void Awake() {
        if (inventory == null) {
            inventory = new Inventory();
        }
    }

    public Inventory getInventory() {
        return this.inventory;
    }
}
