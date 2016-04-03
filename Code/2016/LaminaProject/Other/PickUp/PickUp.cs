using UnityEngine;
using System.Collections;

//attached to any object that can be picked up
public class PickUp : InteractionBase 
{


  public float delayInSeconds=.5f;
	//saved local scales
public	Vector3 startLocalScale;
  public  	Vector3 leftScale;
  public  	Vector3 rightScale;
  public  	Vector3 upScale;
  public  	Vector3 downScale;




    override public void Start()
	{
		startLocalScale = myTransform.localScale;

		rightScale = startLocalScale;
  
		leftScale = rightScale;
		leftScale.x *= -1;

		upScale.x = rightScale.y;
		upScale.y = rightScale.x;


		downScale = upScale;
		downScale.y *= -1;

		leftScale = rightScale;
		leftScale.x *= -1;
		
		upScale.x = rightScale.y;
		upScale.y = rightScale.x;
		
		downScale = upScale;
		downScale.y *= -1;

	}
	override public void Use()
  {
    canUse = false;
    Invoke("CanUseCountDown",delayInSeconds);

  }

  public void CanUseCountDown()
  {
    canUse = true;
  }

	public void PickMeUp()
	{
		//change the layer to the 'held' layer
		myGameObject.layer = LayerMask.NameToLayer ("Held");
		myRigidBody2D.isKinematic = true;
		myTransform.localPosition = new Vector3 (0, 0, 0);
    myBrain = GetComponentInParent<Brain_Base>();
    canUse = true;
  
	}

	public void PutMeDown()
	{
		myGameObject.layer = myLayer;
		myRigidBody2D.isKinematic = false;

    //reset what you are holding
    myBrain.holdingObject = false;
    myBrain.objectHeld = null;
    myBrain.objectPickUpScript = null;

    myBrain = null;

    myTransform.parent = myParent;
	}



	public void Flip(FaceDirection newDirection)
	{

		if(newDirection== FaceDirection.RIGHT)
		{
			myTransform.localScale=rightScale;
		}
		else if (newDirection== FaceDirection.LEFT)
		{
			myTransform.localScale=leftScale;
		}
		else if (newDirection== FaceDirection.UP)
		{
			myTransform.localScale=upScale;
		}
		else
		{
			myTransform.localScale=downScale;
		}
	}

}
