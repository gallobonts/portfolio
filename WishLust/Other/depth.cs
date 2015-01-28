using UnityEngine;
using System.Collections;

public class depth : MonoBehaviour {

	Transform mytransform;
	void start()
	{
		mytransform= transform;

	}
	// Update is called once per frame
	void Update () 
	{
		//set your z position to your y position
		transform.position= new Vector3(transform.position.x,transform.position.y,transform.position.y);
	}
}
