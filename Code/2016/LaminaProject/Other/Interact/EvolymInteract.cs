using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine.UI;



public class EvolymInteract : InteractionBase  
{
  public TalkCanvas[] TalkCanvas= new TalkCanvas[3];
 // public GameObject missionCanvas;
 // public Text missionText;

  public List<Dialogue> lockedRandomConvo= new List<Dialogue>();
  public List<Dialogue> unlockedRandomConvo= new List<Dialogue>();

  public List<Dialogue> lockedReadyMissions= new List<Dialogue>();
  public Dialogue readyMission=null;

  public PlayDialogue myPlayDialogue;

  public override void Use(Brain_Base myBrain)
  {
  //pause game
  GOD.myGOD.FreezePlayer(myBrain.playernum);
    
  //open up interaction menu
  TalkCanvas [0].SetUp(true);

}


  override protected void Awake()
  {
    base.Awake();
    IslandLevelManager.SetDevices += SetupDevice;
  }
  
  public void SetupDevice(int playerNum,InputDevice newDevice, Controls newControls)
  {
    TalkCanvas [0].SetDevice(1,newDevice, newControls);
    
  }

  public void Talk()
  {
   Dialogue talkDialogue = null;

   if (readyMission.text != null)
  {      
    talkDialogue=readyMission;
    readyMission = null;
  }
  else
  {
      talkDialogue=unlockedRandomConvo[0];
  }

    myPlayDialogue.Play(talkDialogue);
    StartCoroutine(TalkCanvas [0].QuitPanels());

  }
}
