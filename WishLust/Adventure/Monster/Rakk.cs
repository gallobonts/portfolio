using UnityEngine;
using System.Collections;

public class Rakk : monsterAI
{
	GameObject rock;
	Transform myTransform;
	Rakk_Rock script;
	public float attackDelay=1f;
	new void Start()
	{
			base.Start();
			attackTimer= new Timer(attackDelay,false);
			minDistance=20;
			myTransform=this.transform;
			rock= (GameObject)Instantiate(AttackArea,myTransform.position,myTransform.rotation);
			rock.transform.parent= myTransform;
			script = (Rakk_Rock) rock.GetComponent("Rakk_Rock");
			script.damage= damage;
			


	}
	//there should be no reason for this....
	new void FixedUpdate()
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


	new void Attack()
	{
		attackTimer.Update(Time.deltaTime);
		if(attackTimer.CheckDone())
		{
			canAttack=true;
		}
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
					rock.SetActive(true);
					rock.transform.position=myTransform.position;
				Vector3 attackDirection= player.position- myTransform.position ;
					attackDirection.Normalize();
					script.direction=attackDirection;
					
					canAttack=false;
				}
				
			}
	}

	new void DoneAttacking()
	{
		attackTimer.ResetTimer();
	}
}
