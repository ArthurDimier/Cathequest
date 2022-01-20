using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnigmeStatues : MonoBehaviour {

    private List<string> listButtons;
    private GameObject texte;
    private GameObject statueOuest;
    public GameObject epee;
    public GameObject journal_5;


    // Start is called before the first frame update
    void Start() {

        listButtons = new List<string>();
        this.texte = GameObject.FindWithTag("text");


    }

    public void enigmeStatues(Button button) {
        //La réponse est 1429
        if (listButtons.Count == 0) {
            if (button.buttonName == "ouest1") {
                listButtons.Add("1");
            } else {
                listButtons.Clear();
            }
        } else if (listButtons.Count == 1) {
            if (button.buttonName == "ouest4") {
                listButtons.Add("4");
            } else {
                listButtons.Clear();


            }
        } else if (listButtons.Count == 2) {
            if (button.buttonName == "ouest2") {
                listButtons.Add("2");
            } else{
                listButtons.Clear();
            
            }
        } else if (listButtons.Count == 3) {
            if (button.buttonName == "ouest9") {
                listButtons.Add("9");
                AudioSource son = GameObject.FindWithTag("congratulations_sound").GetComponent<AudioSource>();
                son.Play();
                son.volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
                Instantiate(epee, new Vector3(0.019f, 2.301f, -43.938f), epee.transform.rotation);
                Instantiate(journal_5, new Vector3(-0.0173f, 0.1f, -42.479f), journal_5.transform.rotation);
                this.statueOuest = GameObject.FindWithTag("StatueOuest");
                StartCoroutine(rotationStatue(statueOuest));

                GameObject.FindObjectOfType<GameManager>().getDialogueManager().setIdDialogue();
                GameObject.FindObjectOfType<GameManager>().getDialogueManager().StartDialogue();

                Debug.Log("Fin du jeu");
            } else {
                listButtons.Clear();
            }
        }
    }

    public IEnumerator rotationStatue(GameObject statueOuest) {
        float time = 0;
        Vector3 myRotation = new Vector3(0f, 300f, 0f);
        while (time < 3.0f) {
            myRotation.y -= 2f;
            statueOuest.transform.rotation = Quaternion.Euler(myRotation);
            time += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
