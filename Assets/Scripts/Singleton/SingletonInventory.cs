using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingletonInventory : MonoBehaviour {
    private static SingletonInventory _instance;

    public static SingletonInventory Instance { get { return _instance; } }

    private void Awake(){
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this);
        }
    }
}
