using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsForceTest : MonoBehaviour
{
    public Transform goal;

    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay = 0.002f;
    public float shake_intensity = 2.5f;
    private float temp_shake_intensity = 0;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce((goal.position - transform.position) * 0.3f);

    }





    void OnGUI() {
        if (GUI.Button(new Rect(20, 40, 80, 20), "Shake")) {
            Shake();
        }
    }

    void Update() {

        Vector3 dir = rb.velocity;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);


        if (temp_shake_intensity > 0) {
            //transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
            transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * 2.2f,
                originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * 2.2f,
                originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * 2.2f,
                originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * 2.2f);
            temp_shake_intensity -= shake_decay;
        }
    }



    void Shake() {
        originPosition = transform.position;
        originRotation = transform.rotation;
        temp_shake_intensity = shake_intensity;

    }
}
