using UnityEngine;
using System.Collections;

public class rock : MonoBehaviour {

	
	public RESOURCE_NAMES[] dropResources;
	public Transform resource;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other==null)
		{return;}
		if(other.name=="PickAxe(Clone)")
		{
			int itemNum= (int)Random.Range (0,dropResources.Length);
			Transform  drop = Instantiate(resource,transform.position,transform.rotation) as Transform;
			Resource script = (Resource) drop.gameObject.GetComponent(typeof(Resource));
			script.SetUp(dropResources[itemNum]);
			Destroy(this.gameObject);
		}
	}
}
