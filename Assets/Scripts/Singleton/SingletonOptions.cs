using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingletonOptions : MonoBehaviour {
    private static SingletonOptions _instance;

    public static SingletonOptions Instance { get { return _instance; } }

    private void Awake(){
        if (_instance == null){
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this);
        }
    } 
}
