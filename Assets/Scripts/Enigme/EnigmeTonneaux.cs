using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnigmeTonneaux : MonoBehaviour {
    private Color couleurClaire;
    private Color couleurFoncee;
    private List<int> listIdTonneaux;
    private bool passe = false;
    private GameObject texte;

    void Start() {
        this.listIdTonneaux = new List<int>();
        this.texte = GameObject.FindWithTag("text");
    }

    void Update() {
        if(GameObject.FindWithTag("tonneau_1") != null && this.passe == false) {
            this.couleurClaire = GameObject.FindWithTag("tonneau_1").GetComponent<Renderer>().material.color;
            this.couleurFoncee = GameObject.FindWithTag("tonneau_4").GetComponent<Renderer>().material.color;
            this.passe = true;
        }
    }

    public void enigmeTonneaux(Button button) {
        // La solution est 7/10

        if (listIdTonneaux.Count < 3) {
            if (button.buttonId == 6) {
                this.listIdTonneaux.Add(1);
                button.GetComponent<Renderer>().material.color = Color.red;
            } else if(button.buttonId == 7) {
                this.listIdTonneaux.Add(2);
                button.GetComponent<Renderer>().material.color = Color.red;
            } else if(button.buttonId == 8) {
                this.listIdTonneaux.Add(3);
                button.GetComponent<Renderer>().material.color = Color.red;
            } else if(button.buttonId == 9) {
                this.listIdTonneaux.Add(4);
                button.GetComponent<Renderer>().material.color = Color.red;
            } else if(button.buttonId == 10) {
                this.listIdTonneaux.Add(5);
                button.GetComponent<Renderer>().material.color = Color.red;
            } else if(button.buttonId == 11) {
                this.listIdTonneaux.Add(6);
                button.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        if(listIdTonneaux.Count == 2) {
            GameObject tonneau1 = null;
            GameObject tonneau2 = null;
            //On redonne leur couleur d'origine aux tonneaux
            foreach(int id in this.listIdTonneaux) {
                if(id<4) {
                    if(tonneau1 == null) {
                        tonneau1 = GameObject.FindWithTag("tonneau_" + id);
                        tonneau1.GetComponent<Renderer>().material.color = this.couleurClaire;
                    } else {
                        tonneau2 = GameObject.FindWithTag("tonneau_" + id);
                        tonneau2.GetComponent<Renderer>().material.color = this.couleurClaire;
                    }
                } else {
                    if(tonneau1 == null) {
                        tonneau1 = GameObject.FindWithTag("tonneau_" + id);
                        tonneau1.GetComponent<Renderer>().material.color = this.couleurFoncee;
                    } else {
                        tonneau2 = GameObject.FindWithTag("tonneau_" + id);
                        tonneau2.GetComponent<Renderer>().material.color = this.couleurFoncee;
                    }
                }
            }
            StartCoroutine(moveTonneaux(tonneau1,tonneau2));
        }
        StartCoroutine(TextCoroutine());
    }

    public IEnumerator TextCoroutine() {
        yield return new WaitForSeconds(2);
        texte.GetComponent<UILocalizeText>().Clear();
    }

    //On translate les tonneaux pour que les 2 deux tonneaux choisis soient intervertis
    //tonneau1 : le gameObject du premier tonneau
    //tonneau2 : le gameObject du second tonneau
    public IEnumerator moveTonneaux(GameObject tonneau1, GameObject tonneau2) {
        float time = 0;
        float distanceT1toT2;
        float T1originalPositionZ = tonneau1.transform.position.z;
        float T2originalPositionZ = tonneau2.transform.position.z;
        bool T1SmallerThanT2 = false;
        if(tonneau1.transform.position.z >= tonneau2.transform.position.z) {
            distanceT1toT2 = tonneau2.transform.position.z - tonneau1.transform.position.z;
            T1SmallerThanT2 = false;
        } else {
            distanceT1toT2 = tonneau1.transform.position.z - tonneau2.transform.position.z;
            T1SmallerThanT2 = true;
        }

        float duree = Math.Abs(distanceT1toT2)/2;
        while(time < duree) {
            //Si le tonneau1 est positionné avant le tonneau2 alors il va vers la droite sinon, il va vers la gauche.
            if(T1SmallerThanT2 == false) {
                tonneau1.transform.Translate(-tonneau1.transform.forward * 0.1f);
                tonneau2.transform.Translate(tonneau2.transform.forward * 0.1f);
            } else {
                tonneau1.transform.Translate(tonneau1.transform.forward * 0.1f);
                tonneau2.transform.Translate(-tonneau2.transform.forward * 0.1f);
            }
            time += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        // Si c'est la bonne combinaison
        if(this.listIdTonneaux.Contains(2) && this.listIdTonneaux.Contains(5)) {
            AudioSource son = GameObject.FindWithTag("congratulations_sound").GetComponent<AudioSource>();
            son.Play();
            son.volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
            GameObject player = GameObject.FindWithTag("Player");
            Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            GameObject prefab = Resources.Load("_Prefabs/item_simple/Statue_ange") as GameObject;
            Quaternion prefabRotation = prefab.transform.rotation;
            GameObject item = Instantiate(prefab, playerPosition + (player.transform.forward * 1), prefabRotation) as GameObject;
            pressButton press = FindObjectOfType<pressButton>();
            press.setEnigmeTonneaux(null);
            GameObject.FindObjectOfType<GameManager>().getDialogueManager().setIdDialogue();
            GameObject.FindObjectOfType<GameManager>().getDialogueManager().StartDialogue();
            Destroy(this);
        } else {
            this.listIdTonneaux.Clear();
            yield return new WaitForSeconds(1f);
            time = 0;
            //Les tonneaux retournent à leur position d'origine
            while(time < duree) {
                //Si le tonneau1 est positionné avant le tonneau2 alors il va vers la gauche sinon, il va vers la droite.
                if(T1SmallerThanT2 == false) {
                    tonneau1.transform.Translate(tonneau1.transform.forward * 0.1f);
                    tonneau2.transform.Translate(- tonneau2.transform.forward * 0.1f);
                } else {
                    tonneau1.transform.Translate(- tonneau1.transform.forward * 0.1f);
                    tonneau2.transform.Translate(tonneau2.transform.forward * 0.1f);
                }
                time += 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}