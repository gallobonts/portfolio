using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PauseMenuUI : GUIMenuBase 
{
  public LevelManager myLevelManager;
	//panels to open & close
	public GameObject storyTypePanel;
  public Transform contentPanel;  


	//focus buttons
	public Button saveButton;

  //story_panel focus buttons
	public Button graveEntriesButton;
  public Button ShortStoriesButton;
  public Button storyPanelBackButton;

  public GameObject sampleButton;
  public List<MirokButton>graveMiroks;
  public List<MirokButton>otherMiroks;
 
  

	override sealed public void ResetPanels()
	{
 		mainMenuPanel.SetActive(false);
		storyTypePanel.SetActive(false);
    focusedButton = null;
	}

  override public IEnumerator QuitPanels()
  {
    yield return new WaitForEndOfFrame();
    ResetPanels();
    GOD.myGOD.UnPauseGame(playerNum);
  }

	override sealed public void ChangeFlow(string newFlow) 
	{
		base.ChangeFlow(newFlow);
 
		switch(newFlow)
		{
			case "MainMenu":
			{
				mainMenuPanel.SetActive(true);
    		SetFocus(saveButton);
        lastFlow.Clear();
        lastFlow.Push("MainMenu");

				break;
			}
		case "StoryType":
		{
			storyTypePanel.SetActive(true);
			SetFocus(graveEntriesButton);
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

  public void  PopulateGraveMiroks()
  {
    /*
     * create mirok buton list here....
     * graveMiroks= god.getGravemiroks(); ect?
     */

    List<MirokButtonInfo> graveMirokButtons = GOD.myGOD.GetGraveMiroks();

    foreach (MirokButton mirok in graveMiroks)
  {
      Destroy(mirok.gameObject);
  }
    graveMiroks.Clear();
    Navigation newNavigation= new Navigation();
   
    for(int i=0; i<graveMirokButtons.Count; i++)
    {
      GameObject newButton = Instantiate(sampleButton) as GameObject;
      MirokButton newMirokButton = newButton.GetComponent<MirokButton>();

       //load info
      newMirokButton.myButtonInfo.myMovie=graveMirokButtons[i].myMovie;
      newMirokButton.myButtonInfo.myAudio=graveMirokButtons[i].myAudio;
      newMirokButton.myButtonInfo.type=graveMirokButtons[i].type;
      newMirokButton.myButtonInfo.myLevelManager=myLevelManager;


      //handle navigation
      newNavigation=newMirokButton.myButtonInfo.button.navigation;
      newNavigation.selectOnLeft= graveEntriesButton;
      newMirokButton.myButtonInfo.button.navigation=newNavigation;

      newButton.transform.SetParent(contentPanel);

      graveMiroks.Add(newMirokButton);//keep a reference to all the mirok buttons
     if(i==0)//if it's the first button
      {
     
        //set left side's navigation to the first button
        newNavigation=graveEntriesButton.navigation;
        newNavigation.selectOnRight= newMirokButton.myButtonInfo.button;
        graveEntriesButton.navigation=newNavigation;

        newNavigation=ShortStoriesButton.navigation;
        newNavigation.selectOnRight= newMirokButton.myButtonInfo.button;
        ShortStoriesButton.navigation=newNavigation;

        newNavigation=storyPanelBackButton.navigation;
        newNavigation.selectOnRight= newMirokButton.myButtonInfo.button;
        storyPanelBackButton.navigation=newNavigation;



      }
      else if(i== (graveMirokButtons.Count-1))//if it's the last button
      {
    
        //set first one's up to the last button
        newNavigation= graveMiroks[0].myButtonInfo.button.navigation;
        newNavigation.selectOnUp= graveMiroks[i].myButtonInfo.button;
        graveMiroks[0].myButtonInfo.button.navigation=newNavigation;
      
        //set last one's down to top button
         newNavigation= graveMiroks[i].myButtonInfo.button.navigation;
        newNavigation.selectOnDown= graveMiroks[0].myButtonInfo.button;
        graveMiroks[i].myButtonInfo.button.navigation=newNavigation;

        //set the last button's up to the 2nd to last button
       newNavigation= graveMiroks[i].myButtonInfo.button.navigation;
        newNavigation.selectOnUp= graveMiroks[i-1].myButtonInfo.button;
        graveMiroks[i].myButtonInfo.button.navigation=newNavigation;


        //set the 2nd to last button's down to this button
        newNavigation= graveMiroks[i-1].myButtonInfo.button.navigation;
        newNavigation.selectOnDown= graveMiroks[i].myButtonInfo.button;
        graveMiroks[i-1].myButtonInfo.button.navigation=newNavigation;


      }
      else//all the in between buttons
      {
        //set the last button's down to this button
       newNavigation= graveMiroks[i-1].myButtonInfo.button.navigation;
       newNavigation.selectOnDown= graveMiroks[i].myButtonInfo.button;
        graveMiroks[i-1].myButtonInfo.button.navigation=newNavigation;

        //set this button's up to the last button
        newNavigation= graveMiroks[i].myButtonInfo.button.navigation;
        newNavigation.selectOnUp= graveMiroks[i-1].myButtonInfo.button;
        graveMiroks[i].myButtonInfo.button.navigation=newNavigation;

      }
    }//end for loop

    SetFocus(graveMiroks[0].myButtonInfo.button);

  }//end populate grave miroks


}//end class
