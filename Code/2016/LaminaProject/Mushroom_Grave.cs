using UnityEngine;
using System.Collections;

public class Mushroom_Grave : MonoBehaviour 
{
  BoxCollider2D myBoxColider2D;
  public float bounciness=5;
  public SFX_Player bounceNoise;

  void Awake()
  {
  myBoxColider2D = this.GetComponent<BoxCollider2D>();

  myBoxColider2D.sharedMaterial.bounciness = bounciness;
  //ficker the collider
  myBoxColider2D.enabled = false;
  myBoxColider2D.enabled = true;
}
	
  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.collider.CompareTag("Player"))
  {
      bounceNoise.Play();
  }

  }
}
