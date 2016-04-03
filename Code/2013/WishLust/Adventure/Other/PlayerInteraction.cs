using UnityEngine;
using System.Collections;

public abstract class PlayerInteraction : MonoBehaviour
{
	public bool canInteract=false;
	private Vector2 myPosition;
	private BoxCollider2D myCollider;
	int playerLayerMask=1;
	int playerLayer=10;
	private float extraSpace=1f;
	protected Collider2D playerTarget;
	
	public void  Start () 
	{
		playerLayerMask=1<<playerLayer;
		myPosition=this.transform.position;
		myCollider= (BoxCollider2D)gameObject.collider2D;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//set size of overlapcircle to the size of the collider+25
		float size= myCollider.size.x;
		if(myCollider.size.y>size)
		{
			size=myCollider.size.y;
		}
		size+=extraSpace;
		
		playerTarget= Physics2D.OverlapCircle(myPosition,size,playerLayerMask);
		
		
		if(playerTarget!=null)
		{
			canInteract=true;
		}
		else
		{
			canInteract=false;
		}
		
		if(canInteract)
		{
			
			if(Input.GetButtonDown("X")||Input.GetKeyDown("space"))
			{
				Interact();
			}
		}
	}
	
	abstract public void Interact();


}
