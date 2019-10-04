using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeShootController : MonoBehaviour
{
    public LeanFingerSwipe lfs;
    public float ShootScale = 100;

    public GameObject AntiVirus;

    private GameObject obstacle;


    void Start() {
        lfs = FindObjectOfType<LeanFingerSwipe>();
        lfs.OnSwipeDelta.AddListener(ShootObject);
    }

    private void ShootObject(Vector2 arg0) {
        StartCoroutine(GameManager.Instance.PlaySound("Swipe"));
        Rigidbody rb = obstacle.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddRelativeForce(new Vector3(arg0.x / ShootScale, arg0.y / ShootScale, 50));
        StartCoroutine(GameManager.Instance.PlaySound("Scream"));
        StartCoroutine(obstacle.GetComponent<AntiVirusController2>().Suizide());
    }

    public void SpawnAntiVirus() {
        obstacle = Instantiate(AntiVirus);
        obstacle.name = "blubedi";
        obstacle.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        obstacle.transform.position = Camera.main.transform.position + Camera.main.transform.forward / 2;
        obstacle.transform.parent = Camera.main.transform;
        obstacle.transform.rotation = Camera.main.transform.rotation;
        Rigidbody rb = obstacle.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 1f;
    }
}
