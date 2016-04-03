using UnityEngine;
using System.Collections;


public class AttackInstance : MonoBehaviour 
{
	[HideInInspector]
	public Vector2 direction;
  [HideInInspector]
  public GameObject owner;


  public ElementalType myElementalType;
	public float deathCountDown;
	public Team myTeam;
	public float damage;
	
  public float speed;

  //self
  [HideInInspector]
  public GameObject myGameObject;
  [HideInInspector]
  public Transform myTransform;
  [HideInInspector]
  public Rigidbody2D myRigidBody;

  Transform parent;

  public void Awake()
  {
    myGameObject = this.gameObject;
    myTransform = this.transform;
    myRigidBody = this.GetComponent<Rigidbody2D>();
    myGameObject.SetActive(false);
  }

	public void Use(Vector2 newDirection,Vector2 spawnLocation)
	{
	
    myGameObject.SetActive(true);
    parent = myTransform.parent;
    myTransform.SetParent(null);

		direction=newDirection;

		//find angle
		float angle=Mathf.Atan2(direction.x,-direction.y);
		angle/=Mathf.Deg2Rad;//change angle from rad -> degree
		
		Quaternion directionInQuaternion= Quaternion.identity;
		directionInQuaternion.eulerAngles= new Vector3(0,0,angle);
		
		myTransform.rotation=directionInQuaternion;
		myTransform.position=spawnLocation;

		Invoke("Suicide",deathCountDown);
	}

  public void InvokeSuicide()
  {
    myGameObject.SetActive(true);
    Invoke("Suicide",deathCountDown);
  }
	void  Suicide()
	{
		myGameObject.SetActive(false);
    myTransform.SetParent(parent);
	}
	
  void FixedUpdate()
  {
    if(GOD.myGOD.isGamePaused)
    {return;}
    myRigidBody.velocity=direction*speed;
  }
  
  
  void OnCollisionEnter2D(Collision2D other)
  {
    if(other.gameObject!=owner)
    {
      DelayedSuicide();
    }
  }

	protected void DelayedSuicide()
	{
		Invoke("Suicide",.1f);
	}

}
