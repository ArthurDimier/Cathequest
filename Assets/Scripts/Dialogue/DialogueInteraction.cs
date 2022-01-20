using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour {
    private GameObject mainCamera, texte;
    public float distance;
    private int x, y;
    private Ray ray;
    private int idDialogueActuel;
    private DialogueManager dialogueManager;

    void Start() {
        this.texte = GameObject.FindWithTag("text");
        this.mainCamera = GameObject.FindWithTag("MainCamera");
        dialogueManager = GameObject.FindObjectOfType<GameManager>().getDialogueManager(); // on récupère le DialogueManager via le GameManager
    }

    void Update() {
        x = Screen.width / 2;
  		y = Screen.height / 2;
        // On créé un rayon qui part du centre de la caméra et qui va à [distance] de son point de départ
  		ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
        RaycastHit hit;
        // Si la camera à un objet au milieu de son champ de vision
  		if(Physics.Raycast(ray, out hit, distance)) {
            // Si on vise l'archonte
  			if(hit.collider.tag == "NPCAvatar") {
                // On affiche le texte
                texte.GetComponent<UnityEngine.UI.Text>().text = "[" + PlayerPrefs.GetString("Interaction", "E") + "] Parler";
                texte.GetComponent<UILocalizeText>().UpdateTranslation();

                // Si l'utilisateur utilise la touche d'interaction et que aucun dialogue n'est en cours, on en lance un
                if(Input.inputString.ToUpper() == PlayerPrefs.GetString("Interaction", "E") && !dialogueManager.dialogOpen) {
                    dialogueManager.StartDialogue();
  				}
  			}
        }
        // si il n'y a rien devant le joueur
        else if(!Physics.Raycast(ray, out hit, distance)) {
            texte.GetComponent<UILocalizeText>().Clear();
        }

        // si un dialogue est ouvert et qu'on appuie sur la barre espace, on passe à la ligne suivante
        if(dialogueManager.dialogOpen) {
            if(Input.GetKeyUp(KeyCode.Space)) {
                dialogueManager.DisplayNextSentence();
            }
        }
    }
}
