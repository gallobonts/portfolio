using UnityEngine;
using System.Collections;


public class MonsterAttack : MonoBehaviour 
{
	int damage=1;
	
	bool canAttack;
	Timer attackTimer= new Timer(2f,true);
	public float newdir=1.0f;
	public void Start()
	{
		
		canAttack=true;
	}
	public void Attack(Vector2 dirFacing)
	{
		canAttack =true;
		transform.localPosition= dirFacing;
	}
	public void Update()
	{
		if(Animate ())
		{
			SendMessageUpwards("DoneAttacking");
			gameObject.SetActive(false);
		}
	}
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		if(canAttack&& other.transform.tag=="Player")
		{
			Controls script= (Controls) other.transform.GetComponent(typeof(Controls));
			script.ChangeHealth(-damage);
			canAttack=false;
		}
	}
	public bool Animate()
	{
		return attackTimer.CheckTime(Time.deltaTime);		
	}
	
	
}

