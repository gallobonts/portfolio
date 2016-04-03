using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*base of all 3 types of ai, grave, island & aicontroller
 * 
 * hiearcy looks like this
 * 					            baseAI
 * 			      / 				               \
 *     base KnobberAI		              LaminaAI
 * 			/           \
 *   grave__EnemyAI  island_enemyAI
*/
public abstract class AI_Base : MonoBehaviour 
{
	//self refrences
  [HideInInspector]
	public Rigidbody2D myRigidBody2D; 
  [HideInInspector]
  public  BoxCollider2D myBoxCollider;
  [HideInInspector]
  public  Transform myTransform;
  [HideInInspector]
  public  GameObject myGameObject;

	//current ai state
	public AIState_Base myAIState;


	//movement stuff
	public Vector2 myDirection;
	public float speedVariance=.6f;
	[HideInInspector]
	public Vector2 addForce;








	public Team myTeam;
	public GameObject targetObject;
  public Transform targetTransform;
	public Vector2 target;


	public bool debug;


	protected GameObject lastThingToHitMe;



	//functions!
	public abstract void ApplyMovement();
 
	protected void Start()
	{
		myBoxCollider=this.GetComponent("BoxCollider2D") as BoxCollider2D;
		myRigidBody2D = this.GetComponent<Rigidbody2D> ();
    myTransform = this.transform;
    myGameObject = this.gameObject;
	}

	protected void FixedUpdate()
	{
		addForce= new Vector2(0,0);
		
		myAIState.Update (Time.deltaTime);
		ApplyMovement();
		
	}
	
}




