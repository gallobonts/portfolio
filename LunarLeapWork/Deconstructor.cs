/*
	stops the platforms as they cross it
*/
using UnityEngine;
using System.Collections;

public class Deconstructor : MonoBehaviour 

{
  //  public Transform Platforms_dead;
    void OnTriggerExit2D(Collider2D other) 
    {
    
    	//check to make sure it was a platform that passed it
        Platform myPlatform= other.gameObject.GetComponent<Platform>();
        if (myPlatform == null) { return; }

        myPlatform.move = false;

  
    }
 


}
