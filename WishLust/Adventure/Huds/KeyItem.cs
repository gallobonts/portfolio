using UnityEngine;
using System.Collections;

[System.Serializable]
public enum KeyItems
{
	poop
};

public class KeyItem : MonoBehaviour {


	public KeyItems myKeyItem;

	void OnCollisionEnter2D(Collision2D other)
	{

		if(other.transform.tag=="Player")
		{
			Controls script= (Controls) other.gameObject.GetComponent("Controls");
			script.AddKeyItem(myKeyItem);
			Destroy(this.gameObject);
		}
	}
}
