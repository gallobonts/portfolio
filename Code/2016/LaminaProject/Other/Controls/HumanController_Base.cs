using UnityEngine;
using System.Collections;


public class HumanController_Base : MonoBehaviour
{
//prevents repeated calls to 'getcomponent'
[HideInInspector]
public Rigidbody2D myRigidbody2D;
[HideInInspector]
public BoxCollider2D myBoxCollider2D;
[HideInInspector]
public Transform myTransform;
[HideInInspector]
public Animator myAnimator;


protected GameObject myGameObject;
protected Brain_Base myBrain;
public int playerNum;
public Controls myControls;
public bool lockPause;
protected GameObject lastThingToHitMe;
public LayerMask interactLayer;
float interactDistance = .5f;


 

//movement
  [HideInInspector]
public Vector2 addForce;
public float speedVariance = .50f;//used to tweak speed


virtual protected void Awake( )
{
  myGameObject = this.gameObject;
  myRigidbody2D = myGameObject.GetComponent<Rigidbody2D>();
  myBoxCollider2D = myGameObject.GetComponent<BoxCollider2D>();
  myTransform = this.transform;
    myAnimator = this.GetComponent<Animator>();
  
    InitializeControllerStates();
}
  virtual protected void InitializeControllerStates(){}

public void SetController( Controls newControls, int newPlayerNum )
{
  myControls = newControls;
  playerNum = newPlayerNum;
}

public Controls GetController( )
{
  return myControls;
}

protected void HandlePause( )
{
  if (myControls.start.WasPressed && GOD.myGOD.isPlayerPaused [playerNum - 1])
  {
    GOD.myGOD.UnPauseGame(playerNum);
  }

}
  
public void HandlePickup( )
{
  if (myControls.pickupNthrow.WasPressed)
  {
    //find all possible interactable objects in range
    Collider2D[] possiblePickups;
    possiblePickups = Physics2D.OverlapCircleAll(myTransform.position, myBrain.pickUpDistance, LayerMaskHandler.instance.pickUpLayer);
    if (possiblePickups.Length == 0)
    {
      myBrain.objectHeld = null;
      return;
    }
      
    //find closest interactable object in range
    float distance = 1000f;//start at an impossiblity
    int newObjectHeld = 0;
    for (int i=0; i<possiblePickups.Length; i++)
    {
      float newDistance = (possiblePickups [i].transform.position - this.transform.position).magnitude;
        Vector3 dir= (possiblePickups[i].transform.position -this.transform.position).normalized;
        //if path not clear
        RaycastHit2D hit;
        hit=Physics2D.Raycast(myTransform.position,dir,newDistance);
        if(hit.transform!=possiblePickups[i].transform)
        {
          Debug.Log("path to " +possiblePickups[i].transform.name + "blocked by "+hit.transform.name);
          Debug.DrawLine(myTransform.position,possiblePickups[i].transform.position,Color.red,3.0f);
          Debug.DrawRay(myTransform.position,dir,Color.green,3.0f);
  
          newDistance=1000;
        }

        if (newDistance < distance)
      {
        distance = newDistance;
        newObjectHeld = i;
      }

    }

    if(distance==1000)//if no paths clear
      {return;}

    myBrain.objectHeld = possiblePickups [newObjectHeld].gameObject;
      
    //parent the object to 'pick up' and 0 out it's local position so it will appear to be carried
    myBrain.objectHeld.transform.parent = myBrain.myPickUp;
     
    myBrain.objectHeld.transform.localPosition = new Vector3(0, 0, 0);
    myBrain.objectPickUpScript = myBrain.objectHeld.GetComponent<PickUp>();
    myBrain.objectPickUpScript.PickMeUp();
     
    myBrain.holdingObject = true;
  }
    
}

public void HandleInteractions( )

{
  if (myControls.interact.WasPressed)
  {
      StartCoroutine(Interact());
  }
}
  IEnumerator Interact() 
  {
     Vector3 position = transform.position + (Vector3)(myBrain.direction * 1.5f);
    
    Collider2D col = Physics2D.OverlapCircle(position, interactDistance, interactLayer);
    if (col)
    {
      yield return new WaitForEndOfFrame();
      col.GetComponent<InteractionBase>().Use(myBrain);
    }

  }

protected void DelayedUnlockPause( )
{
  UnityEngine.Debug.Log("unlock");
  lockPause = false;

}

public void HandleAttacks( )
{
  if (myControls.attack.IsPressed)
  {
     
    if (myBrain.objectHeld)
    {
      myBrain.objectPickUpScript.Use();
    }//if object held
    else
    {
      if (myBrain.equippedAttacks [0] != null && !myBrain.attackLock [0])
      {
        myBrain.equippedAttacks [0].UseAttack(myBrain.direction);
      }//if attack equipped
    }// else object not held
  }//if attack is pressed

  if (myControls.magic1.IsPressed)
  {   
    if (myBrain.equippedAttacks [1] != null && !myBrain.attackLock [1])
    {
      myBrain.equippedAttacks [1].UseAttack(myBrain.direction);
    }
  }

  if (myControls.magic2.IsPressed)
  {
    if (myBrain.equippedAttacks [2] != null && !myBrain.attackLock [2])
    {
      myBrain.equippedAttacks [2].UseAttack(myBrain.direction);
    }
  }


}

  public void ApplyMovement( )
{
    myRigidbody2D.AddForce(addForce);
    addForce = new Vector2(0, 0);
}

protected virtual void UpdateAnimation( )
{
}

protected virtual void FixedUpdate( )
{ 
  if (GOD.myGOD.isPlayerFreeze [playerNum - 1])
  {
    return;
  }
    ApplyMovement();
    UpdateAnimation();
  }
 

}
