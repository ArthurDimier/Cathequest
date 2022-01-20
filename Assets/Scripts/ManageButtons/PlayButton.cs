using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
    //Fonction permettant d'aller dans la première scene de jeu
    public void play() {
        SceneManager.LoadScene("game_start");
    }

    //Fonction permettant d'aller dans la scene des options
    public void options() {
        SceneManager.LoadScene("options");
    }

    //Fonction permettant d'aller dans la scene du menu
    public void menu() {
        //Si on arrive au menu depuis le jeu, il faut indiquer que le menu des options est fermé.
        if(FindObjectOfType<GameManager>().getCurrentScene().StartsWith("game") == true){
            FindObjectOfType<GameManager>().getOption().setInOption(false);
        }
        SceneManager.LoadScene("menu");
    }

    //Fonction permettant d'aller dans la scene du preload
    public void preload() {
        SceneManager.LoadScene("_preload");;
    }
}
