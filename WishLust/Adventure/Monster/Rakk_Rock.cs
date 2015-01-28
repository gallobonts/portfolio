using UnityEngine;
using System.Collections;

public class Rakk_Rock : Arrow {


	new void FixedUpdate () 
	{
		velocity=speed*direction;
		velocity.z=0;
		myRigidBody.velocity=velocity;
		if(deathTimer.CheckTime(Time.deltaTime))
		{
			myGameObject.SetActive(false);
			SendMessageUpwards("DoneAttacking");
		}
	}
	new public void OnTriggerEnter2D(Collider2D other)
	{

		if(other.transform.tag=="Player")
		{
			Debug.Log("fuck");
			Controls script= (Controls) other.transform.GetComponent(typeof(Controls));
			script.ChangeHealth(-damage);
			myGameObject.SetActive(false);
			SendMessageUpwards("DoneAttacking");
			
		}
		else if(other.transform.tag=="obstacle")
		{
			myGameObject.SetActive(false);
			SendMessageUpwards("DoneAttacking");
		}
	}
}
