using UnityEngine;
using System.Collections;



public abstract class InteractionBase : MonoBehaviour 
{
  protected Transform myParent;

  protected int myLayer;
  [HideInInspector]
  public GameObject myGameObject;
  [HideInInspector]
  public Rigidbody2D myRigidBody2D;
  [HideInInspector]
  
  public Transform myTransform;
  
  protected Brain_Base myBrain;
  
  [HideInInspector]
  public bool canUse=true;

  
  protected  virtual void Awake()
  {
    myGameObject = gameObject;
    myLayer = myGameObject.layer;
    myRigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
    myTransform = this.transform;
    myParent = myTransform.parent;
  }

	virtual public void Start()
	{}
	public virtual void Use(){}
  public virtual void Use(Brain_Base brain){}
}
