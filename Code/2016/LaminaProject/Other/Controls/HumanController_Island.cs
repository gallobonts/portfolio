using UnityEngine;
using System.Collections;
using InControl;


public enum FaceDirection
{
	LEFT,RIGHT,UP,DOWN
}
public class HumanController_Island : HumanController_Base 
{
  public bool debugControler=false;

	//reference to brain
	public LaminaBrain_Island myLaminaBrain;
	FaceDirection myFaceDirection;

  //throw info
  public float throwDistance;
  public float moveToThrowDilution;

	void Start()
	{
		myBrain=myLaminaBrain;
		myFaceDirection = FaceDirection.RIGHT;
		myBrain.direction = new Vector2 (1, 0);
	}

void Update () 
	{

		if(!myLaminaBrain.inControl)
		{return;}
		
		if(GOD.myGOD.isPlayerFreeze[playerNum-1])
    {HandlePause();return;}
	
			if(myControls.start.WasPressed && !GOD.myGOD.isPlayerPaused[playerNum-1])
			{GOD.myGOD.PauseGame(playerNum);}
      
      //i believe this is a unity based bug
      //unity usese both directinput & xinput and it sometimes throws off the logic for matching controllers
      //this cauess a phantom controller to appear
      //when controller a uses a trigger, it works but when controller b uses a trigger, it causes the trigger of both a & b to activate
      //theoretically the problem should go away upon computer restart...re-plugging & turning off unity works sometimes but not always
      //the glitch goes away...and should not theoretically effect it when it is released.

      if(debugControler)
        {
        if(myControls.start.WasPressed)
        {Debug.Log(myTransform.name+" pressed start");}
        if(myControls.block.WasPressed)
        {Debug.Log(myTransform.name+" pressed block");}
        if(myControls.attack.WasPressed)
        {Debug.Log(myTransform.name+" pressed attack");}
        if(myControls.magic1.WasPressed)
        {Debug.Log(myTransform.name+" pressed magic1");}
        if(myControls.magic2.WasPressed)
        {Debug.Log(myTransform.name+" pressed magic2");}
        if(myControls.interact.WasPressed)
        {Debug.Log(myTransform.name+" pressed interact");}
        if(myControls.jump.WasPressed)
        {Debug.Log(myTransform.name+" pressed jump");}
        if(myControls.pickupNthrow.WasPressed)
        {Debug.Log(myTransform.name+" pressed pickupNthrow");}
        if(myControls.up.WasPressed)
        {Debug.Log(myTransform.name+" pressed up");}
        if(myControls.down.WasPressed)
        {Debug.Log(myTransform.name+" pressed down");}
        if(myControls.left.WasPressed)
        {Debug.Log(myTransform.name+" pressed left");}
        if(myControls.right.WasPressed)
        {Debug.Log(myTransform.name+" pressed right");}
        }

      HandleAttacks();
    HandleInteractions();

			//if holding an object, check and see if the player wants to throw it, else check and see if the player wants to pick one up
        if (myBrain.holdingObject) 
			{
				HandleThrow();
			}
			else
				{HandlePickup ();}

 
    UpdateMovement();
    UpdateAnimation();
	}

  void HandleThrow()
  {
    if(!myBrain.objectPickUpScript.canUse){return;}

    if (myControls.pickupNthrow.WasPressed)
    {

      Vector2 throwForce = myBrain.direction * throwDistance;
      
      if (addForce.x != 0)//make run and throw matter
      {
        throwForce.x += addForce.x * moveToThrowDilution;
      }
      myBrain.objectHeld.GetComponent<Rigidbody2D>().velocity = throwForce;


      //make it not kinematic before we throw it
      myBrain.objectPickUpScript.PutMeDown();
  

   }

  }
	void HandleLevelUp()
	{
/*
		myInteractionManager.InteractionOptions[0].text= myLaminaBrain.myNodes[0].name;
		myInteractionManager.InteractionOptions[1].text= myLaminaBrain.myNodes[1].name;
		myInteractionManager.InteractionOptions[2].text=null;
		myInteractionManager.InteractionOptions[3].text=null;

		myInteractionManager.ShowInteractions();

		if(myControls.interact.WasPressed)
		{myLaminaBrain.ChooseLevelUp(0);ExitLevelUp(); }
		
		else if(myControls.pickupNthrow.WasPressed)
		{myLaminaBrain.ChooseLevelUp(1);ExitLevelUp();}
*/

	}
	void ExitLevelUp()
	{
//		HideInteractionMenu();
	//	myHumanState=humanState.DEFAULT;
	}




	void UpdateMovement()
	{

		if(!myLaminaBrain.inControl)
		{return;}

			addForce+= new Vector2(1,0)*myControls.move.X* myLaminaBrain.currentStats.speed*speedVariance;
			addForce+= new Vector2(0,1)*myControls.move.Y* myLaminaBrain.currentStats.speed*speedVariance;
		
    
     if(addForce!=new Vector2(0,0))
  {myRigidbody2D.velocity=addForce;}
		//this.gameObject.rigidbody2D.AddForce(addForce*Time.deltaTime*20);
	}


	override protected void UpdateAnimation()
	{
		//myAnimator.SetFloat("h_speed", Mathf.Abs(addForce.x));
		//myAnimator.SetBool("grounded",grounded);
		//myAnimator.SetFloat("v_speed",Mathf.Abs(addForce.y));

		if(Mathf.Abs(addForce.x)>Mathf.Abs(addForce.y))//we are moving more left/right than up & down
		{
			//handle horizontal flipping
			if(addForce.x>3.0f)
			{
				if(myFaceDirection!=FaceDirection.RIGHT){myFaceDirection=FaceDirection.RIGHT;Flip();}
			}
			else if(addForce.x < -3.0f)
			{
				if(myFaceDirection!=FaceDirection.LEFT){myFaceDirection=FaceDirection.LEFT;Flip();}
			}
		
		}

		else
		{
			//handle vertical flipping
			if(addForce.y>3.0f)
			{
				if(myFaceDirection!=FaceDirection.UP){myFaceDirection=FaceDirection.UP;Flip();}
			}
			else if(addForce.y < -3.0f)
			{
				if(myFaceDirection!=FaceDirection.DOWN){myFaceDirection=FaceDirection.DOWN;Flip();}
			}
		}
	}

	void Flip()
	{
		//mySpriteController.Flip ();
    myBrain.pickUpSpriteController.Flip (myFaceDirection);
    if(myBrain.objectHeld)
    {myBrain.objectPickUpScript.Flip (myFaceDirection);}
	}


	
}


/*
cut interaction code

  /*
  void UpdateInteractions()
  {
    myLaminaBrain.GetInteractions(); 

    //quit if no interactions found
    if(myLaminaBrain.myInteractionObject==null)
    {return;}


    myHumanState = humanState.INTERFACE;


    //clear old entries
    for (int i = 0; i < 4; i++)
    {
//      myInteractionManager.InteractionOptions[i].text=null;

    }

    if(myLaminaBrain.myInteractionObject.tag=="Shop")
    {
//Control     myLaminaBrain.myInteractionObject.GetComponent<Shop>().ActivateShop(playerNum,myControls);
      myHumanState= humanState.SHOP;
    }
    else
    {
      //determine number of entires then fill them
      int length=myLaminaBrain.myInteractionOptions.Length;
      if(length<5)
      {
        for (int i = 0; i < length; i++)
        {
          myInteractionManager.InteractionOptions[i].text=myLaminaBrain.myInteractionOptions[i];
          //Debug.Log("option " + (i + 1) + "is " + myLaminaBrain.myInteraction.options[i]);
        }
      }


      myInteractionManager.ShowInteractions();
    }//end else
  }//end update interactions


void HideInteractionMenu()
  {
    /*
    //clear old entries
    for (int i = 0; i < 4; i++)
    {
      myInteractionManager.InteractionOptions[i].text=null;
      
    }
    myInteractionManager.HideInteractions(); 
    myHumanState=humanState.DEFAULT;

}

/*
  void HandleInteractions()
  { 
//    int length=myLaminaBrain.myInteractionOptions.Length;
    //InteractionMessage myMessage;
    //myMessage.updatePriorities=myLaminaBrain.myPriorities;

    if(myControls.interact.WasPressed)//hit bottom button ps=x xbox=a
    {
    //  myMessage.option=myLaminaBrain.myInteractionOptions[0];
    //  myLaminaBrain.myInteractionObject.SendMessage("UseOption",myMessage);
    }
    else if(myControls.pickupNthrow.WasPressed)//hit right button ps=circle xbox=b
    { 
      
      myMessage.option=myLaminaBrain.myInteractionOptions[1];
      myLaminaBrain.myInteractionObject.SendMessage("UseOption",myMessage);   
    }

    else if(myControls.use.WasPressed)//hit left button ps=square xbox=x
    { 
      
      myMessage.option=myLaminaBrain.myInteractionOptions[2];
      myLaminaBrain.myInteractionObject.SendMessage("UseOption",myMessage);
    }

  /* triangle not programmed

  Control   else if(Input.GetButtonDown(myControls.optionsButton)&&length>3)//hit top button & only 4 options ps=triangle xbox=y
    else if(myControls.)
    { myMessage.option=myLaminaBrain.myInteractionOptions[3];
      myLaminaBrain.myInteractionObject.SendMessage("UseOption",myMessage);

    }


  }
*/

    



 