using UnityEngine;
using System.Collections;

public class Bow : Weapon 
{
	public GameObject arrowPrefab;
	private GameObject[] myArrows= new GameObject[5];
	Transform mytransform;

	new void Start()
	{
		mytransform= this.transform;
		canAttack=true;

		for(int i=0; i<myArrows.Length;i++)
		{
			myArrows[i]= (GameObject)Instantiate(arrowPrefab,mytransform.position,mytransform.rotation);
			myArrows[i].transform.parent= this.transform;
			Arrow script = (Arrow) myArrows[i].GetComponent("Arrow");
			script.damage= damage;

		}
	}

	new void Update()
	{
		if(!canAttack)
		{
			if(attackTimer.CheckTime(Time.deltaTime))
			{
				canAttack=true;
				SendMessageUpwards("DoneAttacking");
			}
		}

	}
	

	new public void Attack(Vector2 dirFacing)
	{

		canAttack=false;
		int firedArrow=-1;
		
		//find free arrow
		for(int i=0; i<myArrows.Length;i++)
		{
	
			if(!myArrows[i].activeSelf)
			{
				firedArrow=i;
				break;
			}
		}
		if(firedArrow!=-1)
		{
			myArrows[firedArrow].transform.position= mytransform.position 
										+ new Vector3(dirFacing.x,dirFacing.y,0)
										*2;
			RotateDirection(dirFacing);
			myArrows[firedArrow].transform.rotation=mytransform.rotation;
			myArrows[firedArrow].SetActive(true);
			Arrow script = (Arrow) myArrows[firedArrow].GetComponent("Arrow");
			script.direction= dirFacing;
			script.deathTimer.ResetTimer();
		}

	}

	new public void OnTriggerEnter2D(Collider2D other)//over-ride the old
	{}


}
