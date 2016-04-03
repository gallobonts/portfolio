using UnityEngine;
using System.Collections;

public class HumanController_Grave : HumanController_Base
{
public LaminaBrain_Grave myGraveLaminaBrain;
public SpriteController mySpriteController;
public GameObject mySprite;

//bool inChewArea=false;

public DigTechnique_Grave myDigTechnique;
bool facingRight = true;
  public  bool inDigArea = false;


//jumping mechancis
  [HideInInspector]
 public Vector2 oldPosition= new Vector2(0,0);
//public Transform mySprite;

public float jumpHeight;
public float waterJumpHeight = 1500;

 
public Transform groundCheck;//used to visually tell where the ground is
public Transform waterCheck;
public float groundRadius = 0.4f;//radius between lamina and ground before it's considered grounded
LayerMask groundLayerMask;
public  float floatSpeed = 2f;
[HideInInspector]
public LayerMask waterLayerMask;

//throw info
public float throwDistance;
public float moveToThrowDilution;

  //controller stuff
  [HideInInspector]
  public  ControllerState_Grave myControllerState;
  
  //grave controller states
  [HideInInspector]
  public  ControllerState_Grave_Ground myControllerState_GraveGround;
  [HideInInspector]
  public  ControllerState_Grave_Air myControllerState_GraveAir;
  [HideInInspector]
  public  ControllerState_Grave_Water myControllerState_GraveWater;


void Start( )
{
  groundLayerMask = LayerMaskHandler.instance.groundLayer;
  waterLayerMask = LayerMaskHandler.instance.WaterLayer;
   
  myBrain = myGraveLaminaBrain;
  
 
}

  override protected void InitializeControllerStates()
  {
    myControllerState_GraveGround = new ControllerState_Grave_Ground(this);
    myControllerState_GraveAir = new ControllerState_Grave_Air(this);
    myControllerState_GraveWater = new ControllerState_Grave_Water(this);

    myControllerState = myControllerState_GraveAir;

  }

public  void Reset( )
{
    myControllerState.SwitchState(myControllerState_GraveAir);
    inDigArea = false;
}

public void ChangeDigState( bool newDigState )
{
 inDigArea = newDigState;
}
  
void  Update( )
{

  //stuff that relates to all states, such as pausing

  if (GOD.myGOD.isPlayerPaused [playerNum - 1])
  {
    HandlePause();
    return;
  }//unpause game

  HandlePausing();//pause game
  HandleCrystalSwitch();

  myControllerState.Update(Time.deltaTime);

  UpdateAnimation();
    
}


void OnCollisionEnter2D( Collision2D other )
{
  if (other.transform.tag == "Attack")
  {
    AttackInstance attckinst = other.gameObject.GetComponent<AttackInstance>();
    if (attckinst.owner == myGameObject)
    {
      return;
    }
    lastThingToHitMe = attckinst.owner;
    TakeDam(attckinst.damage);
  }
  else if (other.transform.tag == "KillZone")
  {
    TakeDam(9999);//kill meh!
  }
}
  void OnTriggerEnter2D( Collider2D other )
  {
    if (GOD.myGOD.isLayerinLayerMask(other.gameObject.layer, waterLayerMask))
    {
      myControllerState.SwitchState(myControllerState_GraveWater);
    }
  }
 
  void OnTriggerExit2D(Collider2D other)
  {
    
    if (GOD.myGOD.isLayerinLayerMask(other.gameObject.layer, waterLayerMask))
    {
      myControllerState.SwitchState(myControllerState_GraveAir);
    }
  }

  public void TakeDam( float dam )
{
  myBrain.currentStats.health -= dam;
  if (myBrain.currentStats.health <= 0)
  {
    Die();
  }
}

  public void Die( )
{
  Debug.Log("I DIED!");
  GOD.myGOD.Die(playerNum);
  myGraveLaminaBrain.myPlayerLaminaManager.RewindPosition(oldPosition);
  myGraveLaminaBrain.myPlayerLaminaManager.EnterCyrstalState();

}







  
  public void PauseVerticalVelocity()
  {
    myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0.0f);

  }

  public void HandlePausing( )
{
  if (myControls.start.WasPressed && !GOD.myGOD.isPlayerPaused [playerNum - 1])
  {
    GOD.myGOD.PauseGame(playerNum);
  }

}



  public void HandleCrystalSwitch( )
{
  if (myControls.select.WasPressed)
  {
    myGraveLaminaBrain.myPlayerLaminaManager.EnterCyrstalState();
  }
}


override protected void UpdateAnimation( )
{
  myAnimator.SetFloat("h_speed", Mathf.Abs(addForce.x));
 // myAnimator.SetBool("grounded", true);//fix this
 // myAnimator.SetFloat("v_speed", Mathf.Abs(addForce.y));
 
  //handle flipping
  if (addForce.x > 3.0f)
  {
    myGraveLaminaBrain.direction = new Vector2(1, 0);
    if (!facingRight)
    {
      Flip();
    }
  }
  else if (addForce.x < -3.0f)
  {
    myGraveLaminaBrain.direction = new Vector2(-1, 0);
    if (facingRight)
    {
      Flip();
    }
  }
}

void Flip( )
{
  facingRight = !facingRight;
  mySpriteController.Flip();
  if (facingRight)
  {
    myBrain.pickUpSpriteController.Flip(FaceDirection.RIGHT);
    if (myBrain.objectHeld)
    {
      myBrain.objectPickUpScript.Flip(FaceDirection.RIGHT);
    }
  }
  else
  {
    myBrain.pickUpSpriteController.Flip(FaceDirection.LEFT);
    if (myBrain.objectHeld)
    {
      myBrain.objectPickUpScript.Flip(FaceDirection.LEFT);
    }
  }

  
}

public bool IsGrounded( )
{
    //bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayerMask);
    // return grounded;

//    Collider2D test =Physics2D.OverlapCircleNonAlloc(groundCheck.position, 0, groundLayerMask);
    Vector3 pos = groundCheck.position;


    Collider2D[] checks= new Collider2D[5];
    int count = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadius, checks,groundLayerMask);


    Debug.DrawLine(pos - new Vector3(.5f * groundRadius, 0, 0), pos + new Vector3(.5f * groundRadius, 0, 0));
    Debug.DrawLine(pos - new Vector3(0,.5f * groundRadius, 0), pos +new Vector3(0,.5f * groundRadius, 0));
    Debug.DrawLine(pos - new Vector3(0,0,.5f * groundRadius), pos +new  Vector3(0,0,.5f * groundRadius));

    while(count>0)
    {
      
      count--;

//      Debug.Log("ground collision with "+checks[count].name);
      if(!checks[count].isTrigger)  
      {
        return true;
      }
    }
    return false;
   
}


public void HandleThrow( )
{
  if (!myBrain.objectPickUpScript.canUse)
  {
    return;
  }

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
}
