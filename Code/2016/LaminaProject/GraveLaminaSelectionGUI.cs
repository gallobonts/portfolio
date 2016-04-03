using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;



public class GraveLaminaSelectionGUI : MonoBehaviour
{
public GraveLevelManager myLevelManager;
public GameObject myPanel;
InputDevice inputDevice;
Controls myControls;
int selected;
List<Sprite> laminaList = new List<Sprite>();
public Button[] view = new Button[3];
Button focusedButton;
int playerNum;


public void SetPanel(bool active)
{
  myPanel.SetActive(active);
}

public void SetUp(List<Sprite> newlaminaList, int newSelected, int newPlayerNum)
{
  selected = newSelected;
  laminaList = newlaminaList;
  playerNum = newPlayerNum;

  SetView();

  SetFocus(view [1]);//focus centerbutton
    myPanel.SetActive(true);
}

void Update()
{
  if (!myPanel.activeSelf)
  {
    return;
  }
  if (inputDevice == null)
  {
    Debug.Log("is input device null? ");
  }
  else if (focusedButton == null)
  {
    Debug.Log("is focused button null ");
  }
  if (inputDevice == null || focusedButton == null || !myPanel.activeSelf)
  {
    return;
  }

  // Move focus with directional inputs.
  if (myControls.move.Up.WasPressed)
  {
    selected = roundSelected(selected - 1);
    SetView();
  }

  // Move focus with directional inputs.
  if (myControls.move.Down.WasPressed)
  {
    selected = roundSelected(selected + 1);
    SetView();
  }

  if (myControls.jump.WasPressed)
  {
 //     Debug.Log("inteact pressed, selected= "+ selected);
      myLevelManager.ExitCrystalState(laminaList [selected],playerNum);//exit crystal state
//      Debug.Log("crystal exited");

  }


  
}

public void SetDevice(InputDevice newDevice, Controls newControls)
{
  inputDevice = newDevice;
  myControls = newControls;
}
 
int roundSelected(int i)
{
  if (i == -1)
  {
    i = laminaList.Count - 1;//catch if selected=0
  }
  else if (i == laminaList.Count)
  {
    i = 0;//catch if selected=max
  }
  return i;
}

void SetView()
{
 
  
  int i;
  
  if (laminaList.Count == 1)//catch scenario where only 1 lamina is available
  {
      view [0].image.overrideSprite = laminaList [0];
      view [1].image.overrideSprite = laminaList [0];     
     view [2].image.overrideSprite = laminaList [0];
  }
  else
  {
      i = roundSelected(selected - 1);
      view [0].image.overrideSprite = laminaList [i];
      view [1].image.overrideSprite = laminaList [selected]; 
      i = roundSelected(selected + 1);
      view [2].image.overrideSprite = laminaList [i];

   
  }
}

void SetFocus(Button newFocus)
{
  if (newFocus != null)
  {
    focusedButton = newFocus;
    newFocus.Select();
  }
}
}