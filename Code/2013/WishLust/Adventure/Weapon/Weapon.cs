using UnityEngine;
using System.Collections;



public class Weapon:MonoBehaviour
{
	public int damage=1;

	protected bool canAttack;
	protected Timer attackTimer= new Timer(.75f,true);

	public void Start()
	{
		canAttack=true;
	}

	protected void Attack(Vector2 dirFacing)//called by controls, the player script
	{
		canAttack =true;
		RotateDirection(dirFacing);

	}
	public void Update()
	{
		if(Animate ())
		{
			SendMessageUpwards("DoneAttacking");
			gameObject.SetActive(false);
		}
	}

	public void RotateDirection(Vector2 dirFacing)
	{
		float angle=Mathf.Atan2(-dirFacing.x,dirFacing.y);
		angle/=Mathf.Deg2Rad;
		transform.eulerAngles= new Vector3(0,0,angle);
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(canAttack&& other.transform.tag=="monster")
		{
			monsterAI script= (monsterAI) other.transform.GetComponent(typeof(monsterAI));
			script.TakeDamage(damage);
			canAttack=false;
		}
	}

	public bool Animate()
	{
		return attackTimer.CheckTime(Time.deltaTime);		
	}
	
}

