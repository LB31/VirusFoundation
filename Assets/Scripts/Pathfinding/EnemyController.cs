using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    // Path finding
    public Transform goal;
    private NavMeshAgent agent;

    //private Animator animator;

    private Vector3 originPos;


    private bool CrashedPlayer;
    private float t;

    private bool SpinnedWorld;

    private void Start() {
        originPos = transform.position;
        agent = GetComponent<NavMeshAgent>();

        //animator = GetComponent<Animator>();
        //animator.Play("Walk");
    }

    private void Update() {

        t += Time.deltaTime;
        
        if (t > 5) {
            t = 0;
            CrashedPlayer = false;
            agent.enabled = true;
            agent.destination = GameManager.Instance.ExitLevel1.position;
            //animator.Play("Walk");
        }

        // if enemy has reached his origin position
        //if (!agent.pathPending && SpinnedWorld) {
        //    if (agent.remainingDistance <= agent.stoppingDistance) {
        //        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
        //            //animator.Play("Idle");
        //        }
        //    }
        //}

    }




    // Tracks, if angel have attacked player to stop him for some seconds
    //private void OnTriggerEnter(Collider other) {
    //    if (other.gameObject.CompareTag("Player")) {
    //        CrashedPlayer = true;
    //        agent.enabled = false;
    //        //animator.Play("Idle");
    //        //HealthManager.Instance.DamagePlayer(true, 10);
    //    }
    //}

    public void ReturnToSpawn() {
        CrashedPlayer = false;
        agent.enabled = true;
        agent.destination = originPos;
        //animator.Play("Walk");

    }


}
