using UnityEngine;
using System.Collections;

public class DigTechnique_Grave : Grave_Technique 
{
  public DigInstance myDigInstance;
  Transform myTransform;

  void Awake()
  {
    myTransform = this.transform;
  }

  public void Use()
  {
    myDigInstance.Use(myTransform.position);
  }
	
}
