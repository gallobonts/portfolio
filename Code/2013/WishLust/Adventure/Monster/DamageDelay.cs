using UnityEngine;
using System.Collections;

public class DamageDelay : MonoBehaviour {
	
	public Timer damageDelay= new Timer(.5f,true);
	
	
	// Update is called once per frame
	void Update () 
	{
		if(damageDelay.CheckTime(Time.deltaTime))
		{
			
		}
	}
}
