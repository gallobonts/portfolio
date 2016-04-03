using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using InControl;


public class IslandCanvas : MonoBehaviour 
{
  public PauseMenuUI[] pauseMenuUI= new PauseMenuUI[3];
 
  public void Awake()
  {
    IslandLevelManager.SetDevices += SetupDevice;
  }

  public void SetupDevice(int playerNum,InputDevice newDevice, Controls newControls)
  {
    pauseMenuUI [playerNum].SetDevice(1,newDevice, newControls);

  }

}
