using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_moveElements : MonoBehaviour {

	// Use this for initialization
	public GameObject[] points;
	public float speed;
	private int status = 0;
	public bool antivirus = false;
	void Start () {
		if(antivirus)
		this.transform.position=points[3].transform.position;
		else
		this.transform.position=points[0].transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	//this.transform.position -= this.transform.right * 1 * Time.deltaTime;
	MoveToTarget();
	}
	void MoveToTarget()
	{
		float step =  speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position,points[status].transform.position,step);
		if(transform.position == points[status].transform.position)
		{
			if(status == 3)
				status=0;
			else
				status++;
		}
	}
}
