using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

    public CharacterController controller;
    private float speed = 10f;
    private float gravity = 9.81f;
    private Animator anim;
    private bool isWalking = true;
    private KeyCode keyCode_Forward;
    private KeyCode keyCode_Backward;
    private KeyCode keyCode_Right;
    private KeyCode keyCode_Left;
    private GameManager gameManager;
    private openDoor openDoor;

    Vector3 velocity;

    void Start() {
        anim = GameObject.FindWithTag("PlayerAvatar").GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        openDoor = FindObjectOfType<openDoor>();
    }

    void Update() {
        //Si on est pas dans les options et qu'on est pas en train d'ouvrir une porte, on peut se déplacer
        if(gameManager.getOption().getInOption() == false && openDoor.getIsOpeningDoor() == false) {
            bool forward = Input.GetKey(keyCode_Forward);
            bool backward = Input.GetKey(keyCode_Backward);
            bool left = Input.GetKey(keyCode_Left);
            bool right = Input.GetKey(keyCode_Right);

            // On convertis les playerPref en KeyCode, on le fait ici car les touches peuvent changer
            keyCode_Forward = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "Z"));
            keyCode_Backward = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));
            keyCode_Left = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "Q"));
            keyCode_Right = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));

            float z = 0;
            float x = 0;
            if(forward == true || backward == true || left == true || right == true) {
                if(isWalking == false) {
                    GetComponent<AudioSource>().Play();
                    GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("BruitageVolume", 1);
                    isWalking = true;
                }
                // Si on se dléplace vers l'avant
                if(forward == true || backward == true) {
                    anim.SetInteger("walk",1);
                    anim.SetInteger("lateral_left",0);
                    anim.SetInteger("lateral_right",0);
                    if(forward == true) {
                        z = 1;
                    } else {
                        z = -1;
                    }

                    // On peut aussi se déplacer latéralement
                    if(right == true) {
                        x = 1;
                    } else if(left == true) {
                        x = -1;
                    }
                } else {
                    //Si on se déplace seulement latéralement
                    if(right == true) {
                        anim.SetInteger("lateral_right",1); 
                        anim.SetInteger("lateral_left",0);
                        anim.SetInteger("walk",0);
                        x = 1;
                    } else {
                        anim.SetInteger("lateral_left",1);
                        anim.SetInteger("lateral_right",0);
                        anim.SetInteger("walk",0);
                        x = -1;
                    }
                }
                Vector3 move = transform.right * x + transform.forward * z;

                controller.Move(move * speed * Time.deltaTime);

                velocity.y -= gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
            } else {
                anim.SetInteger("walk", 0);
                anim.SetInteger("lateral_left", 0);
                anim.SetInteger("lateral_right", 0);
                if(isWalking == true) {
                    GetComponent<AudioSource>().Stop();
                    isWalking = false;
                }
            }
        }
    }
}
