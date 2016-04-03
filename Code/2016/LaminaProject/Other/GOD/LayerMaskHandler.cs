using UnityEngine;
using System.Collections;

public class LayerMaskHandler : MonoBehaviour 
{
	static public LayerMaskHandler instance;

	public LayerMask interactableLayer;
	public LayerMask pickUpLayer;
	public LayerMask playerLayer;
	public LayerMask groundLayer;

	public LayerMask knoberAILayer;
	public LayerMask playerAILayer;
	public LayerMask MovingLayer;
	
  public LayerMask WaterLayer;

	/*
	public LayerMask territoryLayer;//layer of objects that boost territory
	public LayerMask AILayer;
	public LayerMask knoberLayer;

	public LayerMask ObstacleAvoidanceLayer;
	*/


	void Awake()
	{
		if(instance!=null && instance!=this)
		{
			Destroy(this);
		}
		instance=this;

	}

}



