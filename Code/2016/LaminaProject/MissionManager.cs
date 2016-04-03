using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour 
{

  public List<Mission> lockedMissions= new List<Mission>();
  public List<Mission> activeMissions= new List<Mission>();
  public List<Mission> completedMissions= new List<Mission>();


  public ActiveMissionUI[] activeMissionHUD = new ActiveMissionUI[3];

 
  public delegate void MissionReward(string missionName);//using delegates so that everything can just subscribe to the master save/load
  public static event MissionReward Reward;


 

  void InitializeLockedMissions()
  {
    //there should be a way to do this via pulling info from an excel sheet

  }

  public void UnlockMission(string missionName)
  {
    bool missionFound = false;
    Mission remove = null;
    foreach (Mission m in lockedMissions)
  {
      if(m.missionName==missionName)
      {
        activeMissions.Add(m);//add mission to active missions
        activeMissionHUD[0].SetMission(m);
        activeMissionHUD[1].SetMission(m);
        activeMissionHUD[2].SetMission(m);

        remove=m;
        missionFound=true;
        break;
     }
  }
    if (missionFound)
  {
    lockedMissions.Remove(remove);
  }
    else
  {
    Debug.Log("mission " + missionName + " not found in locked Missions List");
  }


  }

  public void CompleteObjective(string objective)
  {

    foreach (Mission m in activeMissions)
  {
      if(m.CompleteObjective(objective))//check to see if an objective is complete
      {
        activeMissionHUD[0].UpdateMission(m);
        activeMissionHUD[1].UpdateMission(m);
        activeMissionHUD[2].UpdateMission(m);


        if(m.CompleteMission())//check if ALL objectives are complete
        {
          completedMissions.Add(m);//add mission to active missions
          activeMissions.Remove(m);
          if(Reward!=null)
          {Reward(m.missionName);}//is a delegate function

          activeMissionHUD[0].CompleteMission(m);
          activeMissionHUD[1].CompleteMission(m);
          activeMissionHUD[2].CompleteMission(m);

        }
      }

    }//foreach mission

  }

}
