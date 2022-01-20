using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeStatuette : MonoBehaviour {
    private bool isSuccess = false;
    private GameObject barriere1;
    private GameObject barriere2;
    private GameObject barriere3;
    private GameObject barriere4;

    void Start() {
      barriere1 = GameObject.FindWithTag("barriere_statue_1");
      barriere2 = GameObject.FindWithTag("barriere_statue_2");
      barriere3 = GameObject.FindWithTag("barriere_statue_3");
      barriere4 = GameObject.FindWithTag("barriere_statue_4");
    }


    void OnCollisionEnter (Collision collision) {
      Item item = collision.gameObject.GetComponent<Item>();

      if(item != null) {
        if(item.getId() == 4) {
          collision.gameObject.transform.Rotate(0,90,0);
          if(this.isSuccess == false) {
            AudioSource son = GameObject.FindWithTag("congratulations_sound").GetComponent<AudioSource>();
            son.Play();
            son.volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
            this.isSuccess = true;
            GameObject.FindObjectOfType<GameManager>().getDialogueManager().setIdDialogue();
            GameObject.FindObjectOfType<GameManager>().getDialogueManager().StartDialogue();
            StartCoroutine(moveBarriere(barriere1));
            StartCoroutine(moveBarriere(barriere2));
            StartCoroutine(moveBarriere(barriere3));
            StartCoroutine(moveBarriere(barriere4));
          }
        }
      }
    }

    // On translate les barrières vers le bas pour qu'elles disparraissent
    // barriere : le gameObject de la barrière
    public IEnumerator moveBarriere(GameObject barriere) {
        float time = 0;
        while(time < 5.0f){
            barriere.transform.Translate(-barriere.transform.up * 0.04f);
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
