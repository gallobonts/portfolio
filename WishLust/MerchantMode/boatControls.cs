using UnityEngine;
using System.Collections;

public class boatControls : MonoBehaviour {

	public float speed=10;


	private Vector2 dirFacing;


	void FixedUpdate () 
	{
		Vector2 newVelocity;
		newVelocity.y= Input.GetAxis ("L_Vertical")*speed;
		if(Input.GetKey("up"))
		{
			newVelocity.y=speed;
		}
		else if(Input.GetKey("down"))
		{
			newVelocity.y=-speed;
		}
		newVelocity.x=Input.GetAxis ("L_Horizontal")* speed;

		rigidbody2D.velocity=newVelocity;
		if(Input.GetKey("left"))
		{
			newVelocity.x=-speed;
		}
		else if(Input.GetKey("right"))
		{
			newVelocity.x=speed;
		}
		
		rigidbody2D.velocity=newVelocity;
		if(newVelocity!= new Vector2(0,0))
		{dirFacing=newVelocity.normalized;}
	}

}
