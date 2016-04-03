using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TalkCanvas : GUIMenuBase 
{
  GameObject myGameObject=null;
  //panels to open & close
  public Transform buyPanel;


	//focus buttons
  public Button talkButton;
  public Button buyButton;
  public Button sellButton;

  public void Awake()
  {
    myGameObject = this.gameObject;
  }
  public void SetUp(bool sell)
  {
    if (sell)
  {
      //set up talk->sell
  }
  else
  {
    //set up talk->buy
  }
    ChangeFlow("MainMenu");
    if (myGameObject == null)
  {
      Awake();
  }
    myGameObject.SetActive(true);
  }//end set up

  override sealed public void ResetPanels()
  {
    mainMenuPanel.SetActive(false);
    focusedButton = null;
  }

  override public IEnumerator QuitPanels()
  {
    yield return new WaitForEndOfFrame(); 
    ResetPanels();
    GOD.myGOD.UnFreezePlayer(playerNum);

  }

 
  override sealed public void ChangeFlow(string newFlow) 
  {
    base.ChangeFlow(newFlow);
    
    switch(newFlow)
    {
    case "MainMenu":
    {
      mainMenuPanel.SetActive(true);
      SetFocus(talkButton);
      lastFlow.Clear();
      lastFlow.Push("MainMenu");

      break;
    }
    case "Back":
    {
      Back();
      break;
    }
    default:
    {
      UnityEngine.Debug.Log(newFlow + " does not exist as a flow");
      break;
    }
    }//end switch
    
  }

}
