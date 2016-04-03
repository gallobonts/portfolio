using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using InControl;

public enum Technique
{
  DOUBLEJUMP,//0
  DIG,//1
  SPRINT,//2
  SWIM,//3
  FLY,//4
  THICK_SKIN,//5
  HEAVY_WEIGHT,//6
  CLIMB,//7
  DIVE,//8
  GLIDE,//9
  CHEW,//10
  CATNAP,//11
  COUNT//12
}


public class Brain_Base : MonoBehaviour
 {
  public string myName;
  public Stats currentStats;
  public Stats deltaStats;
  public Stats baseStats;

  public bool[] hasTechnique = new bool[(int)Technique.COUNT];

  //priority stuff
  public Priorities myPriorities;
  public float expMultiplier=1.0f;
  public float healingMultiplier=1.0f;
  protected bool hungerPangLull=true;//used to determine if waiting for hunger pang or not. default true
  
  
  //status effects
  public StatusEffects myStatusEffects;
  public float stageOnePriorityPenalty=50f;
  public float stageTwoPriorityPenalty=25f;
  public float stageThreePriorityPenalty=0f;
  
  //starve effects
  public float stageOneStarveTime=30f;
  public float stageTwoStarveTime=15f;
  public float stageThreeStarveTime=5f;
  public float stageOneStarveEffect=.1f;
  public float stageTwoStarveEffect=.2f;
  public float stageThreeStarveEffect=.3f;

	//levelUp stuff
	protected int lastLevelChoice=0;
	protected int skillPair=0;
	public SkillTreeNodes[] myNodes= new SkillTreeNodes[2];
	public LevelInfo myLevelInfo;

  //pick up stuff
  public PickUpController pickUpSpriteController;
  public Transform myPickUp;//reference to the empty object 'pickup'
  public float pickUpDistance=5f;//distance you can pick an object up
  
  public bool holdingObject=false;//whether you are holding an object or not
  public GameObject objectHeld=null;//the game object you are holding
  [HideInInspector]
  public PickUp objectPickUpScript;//the script attached to said game object


	public Team myTeam;


 
	//controls
	public Controls myControls=null;
  public int playernum = -1;

	//attacks
	public AttackBase[] equippedAttacks= new AttackBase[3];
	//attack information
	public bool[] attackLock= new bool[3];
	protected int[] previousLocks= new int[3];

	public Vector2 direction= new Vector2(1,0);

	//interaction information
	public GameObject myInteractionObject;
	float interactionDistance=1.25f;

  //self refrences
  [HideInInspector]
  public GameObject myGameObject;
  [HideInInspector]
  public Transform myTransform;

  virtual protected void Awake()
  {
    myGameObject = this.gameObject;
    myTransform = this.transform;
  }

	virtual protected void Start()
	{
		baseStats.health=baseStats.maxHealth;
		currentStats.SetEqual(baseStats);
		lockAllAttacks(false);
    SetUpAttacks();
	}

	protected void SetUpAttacks()
  {
    if (equippedAttacks [0] != null)
  {
    return;
  }
    Debug.Log("set up attacks " + myTransform.name);
    GameObject temp=null;

    if(equippedAttacks[0]==null)
    {
      //load the pefab,instantiate the prefab, then set it's parent
      temp=Resources.Load(attackNode.Scratch.ToString()+"_Attack")as GameObject;
      temp=GameObject.Instantiate(temp)as GameObject;
      temp.transform.parent= transform.FindChild("Attacks");
      
      equippedAttacks[0]=temp.GetComponent("AttackBase")as AttackBase;
      equippedAttacks[0].SetUp(myGameObject,myTeam);
      

    }
    if(equippedAttacks[1]==null)
    {
      //load the pefab,instantiate the prefab, then set it's parent
      temp=Resources.Load(attackNode.FireBall.ToString()+"_Attack")as GameObject;
      temp=GameObject.Instantiate(temp)as GameObject;
      temp.transform.parent= transform.FindChild("Attacks");
      
      equippedAttacks[1]=temp.GetComponent("AttackBase")as AttackBase;
      equippedAttacks[1].SetUp(myGameObject,myTeam);
      

    }
    if(equippedAttacks[2]==null)
    {
      //load the pefab,instantiate the prefab, then set it's parent
      temp=Resources.Load(attackNode.FireBall.ToString()+"_Attack")as GameObject;
      temp=GameObject.Instantiate(temp)as GameObject;
      temp.transform.parent= transform.FindChild("Attacks");
      
      equippedAttacks[2]=temp.GetComponent("AttackBase")as AttackBase;
      equippedAttacks[2].SetUp(myGameObject,myTeam);
      
      
    }

  }

	public void lockAllAttacks(bool isLock)
	{
		for(int i=0; i<3;i++)
		{
			attackLock[i]=isLock;
		}
	
	}

	virtual public void SetActive(InputDevice inputDevice,Controls newControls,int newPlayerNum)
	{}

	
	public void GetInteractions()
	{
		//find all possible interactable objects in range
		Collider2D[] possibleInteractions;
		possibleInteractions=Physics2D.OverlapCircleAll(myTransform.position,interactionDistance, LayerMaskHandler.instance.interactableLayer);
		if (possibleInteractions.Length == 0) { myInteractionObject = null; return; }
		
		//find closest interactable object in range
		float distance=1000f;//start at an impossiblity
		int interactionObject=0;
		for(int i=0;i<possibleInteractions.Length;i++)
		{
			float newDistance= (possibleInteractions[i].transform.position-myTransform.position).magnitude;
			if(newDistance<distance)
			{distance=newDistance; interactionObject=i;}
		}
		myInteractionObject = possibleInteractions[interactionObject].gameObject;
		InteractionBase myInteraction = myInteractionObject.GetComponent("InteractionBase") as InteractionBase;
		
		myInteraction.Use ();
		
		
	}


}
