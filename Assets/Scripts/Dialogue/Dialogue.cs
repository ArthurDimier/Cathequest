using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialogue {
    public int idDialogue; // ID du dialogue
    public string[] names; // Input des noms
    [TextArea(2, 10)]
    public string[] sentences; // Input des phrases

    private List<Tuple<string, string>> dialogues; // Structure finale

    public void populate () {
        this.dialogues = new List<Tuple<string, string>>(); // Initialisation de 'dialogues'
        for (int i = 0; i < this.names.Length; i++) { // On parcourt la liste de noms et la liste de phrases
            this.dialogues.Add(new Tuple<string, string>(this.names[i], this.sentences[i])); // On peuple 'dialogues'
        }
    }
    public List<Tuple<string, string>> getDialogue() {
        this.populate(); // On peuple this.dialogues avant de le return
        return this.dialogues;
    }
}
