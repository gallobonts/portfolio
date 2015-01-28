using UnityEngine;
using System.Collections;

public class Deconstructor : MonoBehaviour 

{
  //  public Transform Platforms_dead;
    void OnTriggerExit2D(Collider2D other) 
    {
    
        Platform myPlatform= other.gameObject.GetComponent<Platform>();
        if (myPlatform == null) { return; }
        myPlatform.move = false;
  
    }
 


}
