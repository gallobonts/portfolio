using UnityEngine;
using System.Collections;

public enum RESOURCE_NAMES
{
	wood,
	scoropionTail,
	iron,
	length
};


public class Resource : MonoBehaviour {
	public RESOURCE_NAMES myResource;
	public Sprite[] resourceSprites = new Sprite[(int)RESOURCE_NAMES.length];

	public void Start()
	{
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = resourceSprites[(int)myResource];

	}

	public void SetUp(RESOURCE_NAMES newResource)
	{
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
				renderer.sprite = resourceSprites[(int)newResource];
		myResource=newResource;

	}
	
void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag=="Player")
		{
			ResourceCount dropResource= new ResourceCount();
			dropResource.resourceCount=1;
			dropResource.resourceName=myResource;

			Controls script = (Controls) other.transform.gameObject.GetComponent(typeof(Controls));
			script.AddResource(dropResource);
			Destroy(gameObject);
		}
	}



}
