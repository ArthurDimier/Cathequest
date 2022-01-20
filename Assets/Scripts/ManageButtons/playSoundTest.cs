using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playSoundTest : MonoBehaviour {
    private bool isPlay = false;

    //Fonction permettant de lancer le test du bruitage
    public void playBruitage() {
        if(this.isPlay == false) {
            GetComponent<AudioSource>().Play();
            this.isPlay = true;
            GameObject.FindWithTag("text_test_bruitage").GetComponent<Text>().text = "||";
        } else {
            GetComponent<AudioSource>().Stop();
            this.isPlay = false;
            GameObject.FindWithTag("text_test_bruitage").GetComponent<Text>().text = "►";
        }
    }

    //Fonction permettant de lancer le test de musique
    public void playMusique() {
        if(this.isPlay == false) {
            GetComponent<AudioSource>().Play();
            this.isPlay = true;
            GameObject.FindWithTag("text_test_musique").GetComponent<Text>().text = "||";
        } else {
            GetComponent<AudioSource>().Stop();
            this.isPlay = false;
            GameObject.FindWithTag("text_test_musique").GetComponent<Text>().text = "►";
        }
    }
}
