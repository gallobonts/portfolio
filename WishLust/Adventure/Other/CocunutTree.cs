using UnityEngine;
using System.Collections;

public class CocunutTree : MonoBehaviour {

	public GameObject Coconut;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other==null)
		{return;}
		if(other.name=="Axe(Clone)")
		{
			Instantiate(Coconut,this.transform.position,this.transform.rotation);
			Destroy(this.gameObject);
		}
	}

}
