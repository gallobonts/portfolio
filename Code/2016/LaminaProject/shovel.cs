using UnityEngine;
using System.Collections;

public class shovel : PickUp 
{
	//prefab
	public GameObject digInstance;
	//array of instances
	public DigInstance[] myDigInstances;
	//size of the array
	public int spawnDigCount=10;
	public int useInstanceNum=0;

	
	override public void Start()
	{
		base.Start ();
		myDigInstances= new DigInstance[spawnDigCount];
		for(int i=0;i<spawnDigCount;i++)
		{
			GameObject newInstance= (GameObject)Instantiate(digInstance);
			newInstance.SetActive(false);
			
			myDigInstances[i]=newInstance.GetComponent("DigInstance") as DigInstance;
			myDigInstances[i].transform.parent= this.transform;
			
			
		}
		
	}
	override public void Use()
	{
    if(!canUse){return;}
    base.Use();

    //find spawn location
		Vector2 spawnLocation= myTransform.position;

		//use the instance
		myDigInstances [useInstanceNum].Use (spawnLocation);
		//figure out next object to be used
		useInstanceNum++;
		useInstanceNum%=spawnDigCount;
		



		
	}
}
   