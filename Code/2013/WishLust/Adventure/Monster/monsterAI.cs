using UnityEngine;
using System.Collections;

abstract public class monsterAI : MonoBehaviour {
	protected int playerLayer=10;
	protected int playerLayerMask=1;
	protected float attackDist=50;
	public GameObject AttackArea;

	protected float proximity;

	public RESOURCE_NAMES[] dropResources;
	public float dropChance;
	public Transform resource;

	protected Timer attackTimer= new Timer(.5f,true);
	public bool canAttack= true;

	protected Vector2 dirFacing;


	public enum AIState
	{
		Attack,
		Idle,
		Wander
	}


	//handles
	protected  Transform player;
	public AIState myAIstate=AIState.Idle;
	public TextMesh displayDamage;

	//monster stats
	public float speed;
	public float maxHealth;
	public float health;
	public float damage;


	//ai
		//wander
	protected bool findNewTarget=true;
	protected Timer wanderTimer= new Timer(5,true);

		//seek
	protected Vector3 seekPosition;
	protected float minDistance=3;
		//idle
	public 	float idleChance=15;
	protected Timer idleTimer= new Timer();


	protected Timer dirTimer= new Timer(.5f,true);
	protected Vector2 oldPosition;


	// Use this for initialization
	protected void Start () 
	{
		playerLayerMask=1<<playerLayer;

		health=maxHealth;
		idleTimer.SetTimer(Random.Range(0,3));//initialize the idle timer
		canAttack=true;
		displayDamage = (TextMesh)this.GetComponentInChildren(typeof(TextMesh));
	}

	
	// Update is called once per frame
	protected void FixedUpdate() 
	{
		switch(myAIstate)
		{
		case AIState.Wander: 
				Wander();
				break;
		case AIState.Attack:
			Attack();
			break;

		case AIState.Idle:
			Idle ();
			break;

			}

			Vector2 newPosition= transform.position;
			Vector2 newDirection= newPosition-oldPosition;	
			if(newDirection!= new Vector2(0,0))
			{dirFacing=newDirection;
			dirFacing.Normalize();
			}
			
			oldPosition=newPosition;

		Vector2 endposition= (Vector2)transform.position+ dirFacing*10;
		Debug.DrawLine(transform.position,endposition);


	
	}


	protected void Wander()
	{
		if(findNewTarget)
		{
			float checkDist=15;//distance away from current position to center of search circle
			float checkRadius=10;//radius of search circle

			seekPosition= this.transform.position + transform.forward * checkDist;
			Vector2 pointOnCircumference = Random.insideUnitCircle * checkRadius;
			seekPosition+= new Vector3(pointOnCircumference.x,pointOnCircumference.y,0);
			seekPosition.z=0;
		}

		findNewTarget =Seek(seekPosition);


		if(findNewTarget)
		{
			wanderTimer.ResetTimer();
			float rand = Random.Range(0,100);
			if(rand<=idleChance)
			{myAIstate=AIState.Idle;}
		}
		else if(wanderTimer.CheckTime(Time.deltaTime))//change targets every so often
		{findNewTarget=true;}

	}

	protected void Idle()
	{
		if(idleTimer.CheckTime(Time.deltaTime))
		{
			idleTimer.SetTimer(Random.Range(0,3));
			myAIstate=AIState.Wander;
		}

	}

	protected void Attack()
	{
		Collider2D playerTarget= Physics2D.OverlapCircle(this.transform.position,attackDist, playerLayerMask);
		if(playerTarget==null)
		{
			myAIstate=AIState.Wander;
			return;
		}
		else
		{player= playerTarget.transform;}

		//get close enough to attack
		if(Seek(player.position))
		{
			if(canAttack)
			{
					AttackArea.SetActive(true);
					AttackArea.SendMessage("Attack",dirFacing);
					canAttack=false;
			}

		}
	
	}
	//called by athe monsterattack script
	public void DoneAttacking()
	{
		canAttack=true;
	}

	protected bool Seek(Vector3 targetPosition)
	{

		//find the direction of the target
		Vector3 direction = targetPosition - transform.position;

		//find the distance to that point
		float distance=direction.magnitude;
		//
		if(distance<minDistance)
		{return true;}

		direction.Normalize();
		transform.Translate (direction*speed*Time.deltaTime);

		return false;

	}

	
	protected bool Seek(Vector3 targetPosition,float proximity)
	{
		
		//find the direction of the target
		Vector3 direction = targetPosition - transform.position;
		
		//find the distance to that point
		float distance=direction.magnitude;
		//
		if(distance<proximity)
		{return true;}
		
		direction/=distance;
		transform.Translate (direction*speed*Time.deltaTime);
		return false;
		
	}

	
	protected bool Flee(Vector3 targetPosition,float proximity)
	{
		
		//find the direction of the target
		Vector3 direction = transform.position - targetPosition ;
		
		//find the distance to that point
		float distance=direction.magnitude;
		//
		if(distance<proximity)
		{return true;}
		
		direction/=distance;
		transform.Translate (direction*speed*3*Time.deltaTime);
		return false;
		
	}


	public void TakeDamage(float damage)
	{
	
//		displayDamage.text=damage.ToString();

		myAIstate = AIState.Attack;
		health-=damage;
		if(health<=0)
		{Die();}

	}
	

	public void Die()
	{
		float dropNum = Random.Range(0,100);
		if(dropNum<dropChance)
		{
			int itemNum= (int)Random.Range (0,dropResources.Length);
			Transform  drop = Instantiate(resource,transform.position,transform.rotation) as Transform;
			Resource script = (Resource) drop.gameObject.GetComponent(typeof(Resource));
			script.SetUp(dropResources[itemNum]);
		}

		Destroy(gameObject);
	}
}

