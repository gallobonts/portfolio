using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class GUI_info
{
	public string name;
	
	public LevelInfo myLevelInfo= new LevelInfo();
	
	public Stats myStats= new Stats();
	
	public StatusEffects myStatusEffects= new StatusEffects();
	public Priorities myPriorities= new Priorities();
	public string[] attacks= new string[4];
	
}

public class IslandGUI : MonoBehaviour 
{
  public Text myName;
  public Image laminaIcon;
  public Text currency;
  public Text lvl;
  public Text exp;
  public Text hp;
  public Text sleep;
  public Text wanderlust;
  public Text hunger;
  public Text education;
  public Text statusEffects;


  GUI_info myInfo = null;



  public void SetUp(GUI_info newInfo)
  {
    myInfo = newInfo;
    myName.text = "Name:     " + newInfo.name;
    laminaIcon.overrideSprite=Resources.Load<Sprite>(newInfo.name + "Lamina_FaceSprite");


  }

  public void Update()
  {
    currency.text = "Currency: " + GOD.myGOD.currentSkulls;
    if(myInfo==null){return;}
    lvl.text = "Level: " + myInfo.myLevelInfo.level.ToString();
    exp.text="Exp: "+myInfo.myLevelInfo.experience.ToString()+" / " +myInfo.myLevelInfo.experienceTilNxtLvl.ToString();
    hp.text = "HP: " + myInfo.myStats.health + " / " + myInfo.myStats.maxHealth;
    sleep.text="Sleep: " + (int)myInfo.myPriorities.sleep;
    wanderlust.text="Wanderlust: " + (int)myInfo.myPriorities.wanderLust;
    hunger.text="Hunger: " + (int)myInfo.myPriorities.hunger;
    education.text="Education: " + (int)Priorities.education;

 
    statusEffects.text="Status Effects: ";
    if(myInfo.myStats.health<=0)
    {statusEffects.text+="death...";}
    else
    {

      if(myInfo.myStatusEffects.genius>0)
      {
        statusEffects.text+= "genius ";
      }
      if(myInfo.myStatusEffects.dimWit>0)
      {
        statusEffects.text+= "dimwit stage "+myInfo.myStatusEffects.dimWit.ToString()+" ";
      }
      if(myInfo.myStatusEffects.stirCrazy>0)
        {
        statusEffects.text+= "stir crazy ";
        }
      if(myInfo.myStatusEffects.homeSick>0)
        {
        statusEffects.text+= "homesick stage "+myInfo.myStatusEffects.homeSick.ToString()+" ";
        }
     if(myInfo.myStatusEffects.sleepDerpivation>0)
        {
        statusEffects.text+= "sleep depreivation stage "+myInfo.myStatusEffects.sleepDerpivation.ToString()+" ";
        }
      if(myInfo.myStatusEffects.starvation>0)
        {
        statusEffects.text+= "starvation stage "+myInfo.myStatusEffects.starvation.ToString()+" ";
        }

    }

  }










}
