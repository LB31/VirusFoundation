using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerLevel2 : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 0.05f;

    public Transform goal;

    private Animator anim;

    private bool arrived;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Move", 0, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        if (Vector3.Distance(transform.position, goal.position) >= 0.01) {
            transform.position = Vector3.MoveTowards(transform.position, goal.position, step);
             
        } else if(!arrived) {
            anim.Play("RobotAttack", 0, 0.25f);
            arrived = true;
        }
    }
}
