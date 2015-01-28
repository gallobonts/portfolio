using UnityEngine;
using System.Collections;

public abstract class Tool : MonoBehaviour 
{


	Timer attackTimer= new Timer(.75f,true);
	
	public void Start()
	{

	}
	public void Use(Vector2 dirFacing)
	{
		transform.localPosition= dirFacing*.25f;
		
	}
	public void Update()
	{
		if(Animate ())
		{
			SendMessageUpwards("DoneAttacking");
			gameObject.SetActive(false);
		}
	}

	
	public bool Animate()
	{
		return attackTimer.CheckTime(Time.deltaTime);		
	}

}
