using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public Vector2 direction=new Vector2(0,0);
	public float speed=40;
	protected Vector3 velocity;
	public float damage;
	public Timer deathTimer= new Timer(1f,true);

	protected Rigidbody2D myRigidBody;
	protected GameObject myGameObject;

	void Start()
	{
		myRigidBody= this.rigidbody2D;
		myGameObject=this.gameObject;
	}
	
	protected void FixedUpdate () 
	{
		velocity=speed*direction;
		velocity.z=0;
		myRigidBody.velocity=velocity;
		if(deathTimer.CheckTime(Time.deltaTime))
		{
			myGameObject.SetActive(false);
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag=="monster")
		{
			monsterAI script= (monsterAI) other.transform.GetComponent(typeof(monsterAI));
			script.TakeDamage(damage);
			myGameObject.SetActive(false);
		
		}
		else if(other.transform.tag=="obstacle")
		{
			myGameObject.SetActive(false);
		}
	}
}
