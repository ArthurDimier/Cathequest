using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour {
    public float mouseSensitivity;

    public Transform playerBody;

    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        //On récupère la sensibilité de la souris
        this.mouseSensitivity = PlayerPrefs.GetFloat("Sensibility", 200);

        float mouseX = Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * this.mouseSensitivity * Time.deltaTime;

        this.xRotation -= mouseY;

        //On bloque la rotation verticale de la caméra pour que le personnage ne se torde pas la nuque !
        this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(this.xRotation, 0f, 0f);
        this.playerBody.Rotate(Vector3.up * mouseX);
    }
}
