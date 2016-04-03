using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public float smooth= 7f;
	public float checkDist=10f;
	bool follow=true;
	public GameObject[] followArray;

	// Use this for initialization
	void Start () 
	{
//		Debug.Log ("screen height=" +Screen.height);
//		Debug.Log ("screen width=" + Screen.width);
		followArray = GameObject.FindGameObjectsWithTag("Player");


	}
	
	// Update is called once per frame
	void Update () 
	{   
//		checkDist=Mathf.Min (Screen.width,Screen.height);
//		checkDist/=2;
		Vector3 newPosition = followArray[0].transform.position;
		newPosition.z=-10;
		float distance=Vector3.Distance(transform.position,newPosition);
		if(distance>checkDist)
		{follow=true;}
		else if(distance<.5f)
		{follow=false;}
		if(follow)
		{transform.position=Vector3.Lerp(transform.position,newPosition,smooth*Time.deltaTime);}

	}
}

