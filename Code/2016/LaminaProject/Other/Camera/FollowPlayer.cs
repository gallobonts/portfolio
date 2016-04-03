using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour 
{
	public GameObject target;
	public float speed= .5f;//normal speedof camera
	public float deadZone= 1f;//determines when to stop moving
	public float followZone;
	public bool startFollow=false;
	public bool fastFollow=false;
	Vector3 lastTargetPositon= new Vector3(0,0);
  public float zDistance=80;

 

	void FixedUpdate()
	{
		if(!target)
		{return;}

		if(!startFollow)
		{
			Vector3 targetPosition= target.transform.position;	
      Vector3 positionDifference= targetPosition-this.transform.position;
      positionDifference.z=0;
//			Vector3 direction = positionDifference.normalized;
			float  distance= positionDifference.magnitude;
      positionDifference.z-=zDistance;   

   
      if(distance > deadZone)
			{
				startFollow=true;
			}

		}
		else
		{
			Follow();
		}
	
	}

	public void SetTarget(GameObject newTarget)
	{
		target=newTarget;
	}
	void Follow()
	{
		Vector3 targetPosition= target.transform.position;
	

		if(!fastFollow)
		{

			Vector3 positionDifference= targetPosition-this.transform.position;
			positionDifference.z=0;
			Vector3 direction = positionDifference.normalized;
			float  distance= positionDifference.magnitude;
		
			if(distance < followZone)
			{fastFollow=true;}

			if(distance<1)
			{distance=1;}

			this.transform.position+= direction* (distance*speed);

		}
		else
		{
			float currentZ= this.transform.position.z;
			targetPosition.z=currentZ;
			this.transform.position=targetPosition;

			if(targetPosition==lastTargetPositon)
			{
				fastFollow=false;
				startFollow=false;
			}
			else
			{
				lastTargetPositon=targetPosition;
			}
		}


	}

}
