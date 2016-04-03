using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour {

	Transform mySprite;
	Vector3 startLocalPosition;
	Vector3 startLocalScale;

	//saved local positions
	Vector3 leftPosition;
	Vector3 rightPosition;
	Vector3 upPosition;
	Vector3 downPosition;

	//saved local scales
	Vector3 leftScale;
	Vector3 rightScale;
	Vector3 upScale;
	Vector3 downScale;


	// Use this for initialization
	void Start () {
		mySprite = transform;

		startLocalPosition = mySprite.localPosition;
		startLocalScale = mySprite.localScale;

		rightPosition = startLocalPosition;
		rightScale = startLocalScale;


		leftPosition = rightPosition;
		leftPosition.x *= -1;
		leftScale = rightScale;
		leftScale.x *= -1;



		upPosition.x = rightPosition.y;
		upPosition.y = rightPosition.x;
		upScale.x = rightScale.y;
		upScale.y = rightScale.x;



		downPosition = upPosition;
		downPosition.y *= -1;
		downScale = upScale;
		downScale.y *= -1;



		//		myScale = mySprite.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Flip(FaceDirection newDirection)
	{/*
		//flip scale
		Vector3 theScale = mySprite.localScale;
		theScale.x *= -1;
		//mySprite.localScale= theScale;

		//flip location
		Vector3 newLocalPosition = mySprite.localPosition;
		newLocalPosition.x *= -1;
		mySprite.localPosition = newLocalPosition;
		*/

		if(newDirection== FaceDirection.RIGHT)
		{
			mySprite.localPosition=rightPosition;
			mySprite.localScale=rightScale;
		}
		else if (newDirection== FaceDirection.LEFT)
		{
			mySprite.localPosition=leftPosition;
			mySprite.localScale=leftScale;
		}
		else if (newDirection== FaceDirection.UP)
		{
			mySprite.localPosition=upPosition;
			mySprite.localScale=upScale;
		}
		else
		{
			mySprite.localPosition=downPosition;
			mySprite.localScale=downScale;
		}
	}

}
