using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkForGround : MonoBehaviour {
    public bool isGrounded;

    public float hitDistance;

    public LayerMask layer;

    // Update is called once per frame
    void Update() {
        UpdateStats();
    }

    public void UpdateStats() {
        if (isGrounded) {
            hitDistance = 0.00f;
        } else {
            hitDistance = 0.00f;
        }

        if(Physics.Raycast(transform.position - new Vector3(0, 0.95f, 0), -transform.up, hitDistance, layer)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }
}
