using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour 
{

  protected Brain_Base myBrain;

  [HideInInspector]
  public GameObject myGameObject;
  [HideInInspector]
  public Rigidbody2D myRigidBody2D;
  [HideInInspector]
  
  public Transform myTransform;
  


  protected void Awake()
  {
    myGameObject = gameObject;
    myRigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
    myTransform = this.transform;
   }


  public virtual void Use(){}
  public virtual void Die(){Destroy(myGameObject);}

  void OnTriggerEnter2D(Collider2D col)
  {
    if (col.tag == "Player")
  {
      myBrain = col.gameObject.GetComponentInParent<Brain_Base>();
      Use();
      Die();
  }
  
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    if(col.gameObject.tag== "Player")
    {
      myBrain = col.gameObject.GetComponentInParent<Brain_Base>();
      Use();
      Die();
    }
  }
}
