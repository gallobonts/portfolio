using UnityEngine;
using System.Collections;

public class AIState_GravePatrol:AIState_Base
{
	public float collisionDetectionDistance = 1.5f;


	public AIState_GravePatrol(KnobberAI_Grave myAIController):base(myAIController){}


	float Idletimer;
	public float minIdleTimer=5;
	public float maxIdleTimer=10;

	protected override void EnterState ()
	{
		if(aiController.debug)
		{Debug.Log("entering patrol state");}

		aiController.myDirection = new Vector2 (1, 0);
		float RandomDirection = Random.Range (0, 100);
		if (RandomDirection > 50) 
		{
			aiController.myDirection.x *= -1;
		}

		Idletimer = Random.Range(minIdleTimer,maxIdleTimer);
	}
	
	public override void Update (float dt)
	{
		if(aiController.myDirection.x>0)//if facing right
		{
			
			RaycastHit2D rightSideCheck;
			rightSideCheck = Physics2D.Raycast (myTransform.position, Vector2.right, collisionDetectionDistance);
			Debug.DrawRay (myTransform.position, Vector2.right * collisionDetectionDistance, Color.red);
			if(rightSideCheck.collider!=null)
			{
				if(rightSideCheck.collider!=myBoxCollider)
				{aiController.Flip();}
			}
		}
		else if(aiController.myDirection.x<0)
		{
			//Creating Raycasts so we know what is in front of the enemy or at the back
			RaycastHit2D leftSideCheck;
			leftSideCheck = Physics2D.Raycast (myTransform.position, -Vector2.right, collisionDetectionDistance);
			Debug.DrawRay (myTransform.position, -Vector2.right * collisionDetectionDistance, Color.red);
			if(leftSideCheck.collider!=null)
			{
				if(leftSideCheck.collider!=myBoxCollider)
				{aiController.Flip();}
			}
		}
		
		aiController.addForce += aiController.myDirection * aiController.myStats.speed * aiController.speedVariance;

		Idletimer -= dt;
		if(Idletimer<0)
		{
			SwitchState(aiController.myAIState_GraveIdle);
		}
	}

 

}
