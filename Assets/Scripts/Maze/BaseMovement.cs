using UnityEngine;
using System.Collections;

public class BaseMovement : MonoBehaviour {
    public GameObject horzPedal = null;
    public GameObject vertPedal = null;
    public float horzRot, vertRot;
    public float pedalFactor = -10;
    public float rotFactor = 30f;

    private Vector3 hPedal;
    private Vector3 vPedal;


    void Start () {
        //Get References to the game pedals for applying a rotation in Update()
        //initialize all values for use
        hPedal = horzPedal.transform.eulerAngles;
        vPedal = vertPedal.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        //Get Input from Input.GetAxis
        horzRot = Input.GetAxis("Horizontal") * -rotFactor;
        vertRot = Input.GetAxis("Vertical") * -rotFactor;
        //Stretch it from 0-1 to 0 - maxRotFactor

        //Apply a part of the rotation to this (and children) to rotate the plafield
        //Use Quaternion.Slerp und Quaternion.Euler for doing it
        //REM: Maybe you have to invert one axis to get things right visualy

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(vertRot, 0, -horzRot), Time.deltaTime * 2.0f);

        //Apply an exaggerated ammount of rotation to the pedals to visualize the players input
        horzPedal.transform.rotation = Quaternion.Slerp(horzPedal.transform.rotation, Quaternion.Euler(horzRot * pedalFactor, hPedal.y, hPedal.z), Time.deltaTime * 2.0f);
        vertPedal.transform.rotation = Quaternion.Slerp(vertPedal.transform.rotation, Quaternion.Euler(vertRot * pedalFactor, vPedal.y, vPedal.z), Time.deltaTime * 2.0f);
        //Make the rotation look right

    }
}
