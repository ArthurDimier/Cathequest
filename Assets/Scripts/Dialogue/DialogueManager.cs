using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    public Text nameText;
    public Text dialogueText;
    public List<Dialogue> dialogues;
    private int idDialogue;
    private Dialogue dialogueActuel;
    private Queue<Tuple<string, string>> lines;
    private Tuple<string, string> line;
    private GameObject dialogueInterface;
    public bool dialogOpen;

    void Start() {
        lines = new Queue<Tuple<string, string>>();
        idDialogue = 0;
        this.dialogueInterface = GameObject.FindWithTag("dialogue_interface");
        this.dialogueInterface.SetActive(false);
        this.dialogOpen = false;
    }

    public void StartDialogue() {
        // on affiche l'interface des dialogues
        this.dialogueInterface.SetActive(true);
        this.dialogOpen = true;
        // on récupère le dialogue actuel avec son ID
        dialogueActuel = dialogues[idDialogue];
        foreach (Tuple<string, string> line in dialogueActuel.getDialogue()) {
            // On parcourt les input pour peupler la liste phrases et de personnages
            lines.Enqueue(line);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (lines.Count == 0) {
            // Si la liste de dialogues est vide, on termine le dialogue
            EndDialogue();
            return;
        }
        line = lines.Dequeue(); // On prend le prochain dialogue et on l'enlève de la liste
        nameText.gameObject.GetComponent<UILocalizeText>().Clear();
        nameText.text = line.Item1; // On affiche le nom du personnage qui parle
        nameText.gameObject.GetComponent<UILocalizeText>().UpdateTranslation();
        dialogueText.gameObject.GetComponent<UILocalizeText>().Clear();
        dialogueText.text = line.Item2; // On affiche ce qu'il dit
        dialogueText.gameObject.GetComponent<UILocalizeText>().UpdateTranslation(); // on traduit
    }

    void EndDialogue() {
        // on cache l'interface des dialogues
        this.dialogueInterface.SetActive(false);
        this.dialogOpen = false;
        // on vide la queue
        lines.Clear();
    }

    public int getIdDialogue() {
        return this.idDialogue;
    }

    public void setIdDialogue() {
        if(this.idDialogue < 7) {
            this.idDialogue += 1;
        }
    }

    public void resetIdDialogue() {
        this.idDialogue = 0;
    }
}
