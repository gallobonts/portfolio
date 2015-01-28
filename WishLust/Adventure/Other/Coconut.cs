using UnityEngine;
using System.Collections;

public class Coconut : MonoBehaviour {

	public int healAmount=3;
	public void OnTriggerEnter2D(Collider2D other)
	{

		if(other.tag=="Player")
		{
			Controls script= (Controls) other.transform.GetComponent(typeof(Controls));
			script.ChangeHealth(healAmount);
			Destroy (this.gameObject);
		}
	}
}
