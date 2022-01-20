using UnityEngine;
using UnityEngine.AI;

public class npcMovement : MonoBehaviour {
    // Position que le PNJ doit suivre
    private Transform transformToFollow;
    // composant qui se charge de l'IA du déplacement
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 vect0;

    void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // on récupère le composant transform du joueur
        transformToFollow = GameObject.FindWithTag("Player").transform;
        vect0 = new Vector3(0,0,0);
    }

    void Update() {
        // On met la destination du PNJ sur la position choisie
        agent.destination = transformToFollow.position;
        // si le PNJ bouge, on lui applique l'animation de marche, sinon celle où il reste debout
        if(agent.velocity == vect0) {
            anim.SetInteger("walk",0);
        } else {
            anim.SetInteger("walk",1);
        }
    }
}