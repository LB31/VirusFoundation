using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] conn;
    public Material change_conn;

    public void ColorChange(int i)
    {
        print("bla");
        conn[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }
    void Start()
    {
        conn = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform item in transform)
        {
            conn[i] = item.gameObject;
            i++;
        }
    }

    //{ for (int i= 0; i< conn.Length; i++)
    //    conn[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    //}+-

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                    hit.collider.GetComponent<Renderer>().material.SetColor("_Color", change_conn.color);
            }
        }
    }
}
