using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {
	Transform mySprite;
	//Vector3 myScale;

	// Use this for initialization
	void Start () {
		mySprite = transform;
//		myScale = mySprite.localScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Flip()
	{
		Vector3 theScale = mySprite.localScale;
		theScale.x *= -1;
		mySprite.localScale= theScale;
	}
}
