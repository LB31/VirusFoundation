using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public LeanFingerSwipe lfs;

    private GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        lfs = FindObjectOfType<LeanFingerSwipe>();

        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.useGravity = false;


    }

    // Update is called once per frame
    void Update()
    {
        lfs.OnSwipeDelta.AddListener(Bla);
    }

    private void Bla(Vector2 arg0) {
        print(arg0);
        float scale = 100;
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(new Vector3(arg0.x / scale, arg0.y / scale, 10));
    }

    public void SpawnAntiVirus() {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void Swipe() {
        print("swipe");
    }

    public void Swipe2() {
        print("swipe2");
    }
}
