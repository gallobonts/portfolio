using UnityEngine;
using System.Collections;

public class DigInstance : MonoBehaviour 
{
	public float lifeSpan=30f;
	float currentLifeSpan;
	GameObject myGameObject;
	Transform myTransform;
	Transform myParent;

	void Awake()
	{
		myTransform = this.transform;
		myGameObject = this.gameObject;
		myParent = myTransform.parent;
		myGameObject.SetActive (false);
	}

	public void Use(Vector3 spawnLocation)
	{
		//make sure the re-appear calls ontriggerenter
		if(myGameObject.activeSelf)
		{Die();}


		myTransform.position=spawnLocation;
		myTransform.parent = null;

		myGameObject.SetActive (true);
		currentLifeSpan = lifeSpan;
	}

	public void Update()
	{
//    Debug.Log("current lifespan= " + currentLifeSpan + " max lifespan= " + lifeSpan);
		currentLifeSpan -= Time.deltaTime;
		if(currentLifeSpan<=0)
		{
			Die ();
		}
	}

	public void Die()
	{
		myGameObject.SetActive (false);
		myTransform.parent = myParent;
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if(other.tag=="DigSpot")
		{
      Debug.Log("colliding with digspot");
      DigSpot check= other.GetComponent<DigSpot>();
      if(check)
      {check.Use();}
      else
      {
        DigSpot_Grave check2=other.GetComponent<DigSpot_Grave>();
        check2.Use();
      }

		}
		else if(other.tag=="GraveEntrance")
		{
			other.GetComponent<GraveEntrance>().Use();
			Die ();
		}
	}
}
