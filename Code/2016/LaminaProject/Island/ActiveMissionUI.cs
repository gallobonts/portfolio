using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActiveMissionUI : MonoBehaviour
{
public  IslandLevelManager myLevelManager;

public Sprite completeImage;
public Sprite incompleteImage;
Mission activeMission;
public Text missionName;
public GameObject[] objectiveGameObject = new GameObject[3];
public Image[] objectiveImage = new Image[3];
public Text[] objectiveText = new Text[3];
public GameObject missionComplete;

GameObject myGameObject;

public void SetMission( Mission active )
{
  if (myGameObject == null)
  {
    myGameObject = this.gameObject;
  }
  myGameObject.SetActive(true);

  activeMission = active;
  missionName.text = activeMission.missionName;


  for (int i =0; i<3; i++)
  {
    if (i < activeMission.myData.Count)
    {
      objectiveGameObject [i].SetActive(true);
      if (activeMission.myData [i].isCompleted)
      {
        objectiveImage [i].overrideSprite = completeImage;
      }
      else
      {
        objectiveImage [i].overrideSprite = incompleteImage;
      }

      objectiveText [i].text = activeMission.myData [i].objectiveName;

    }
    else
    {
      objectiveGameObject [i].SetActive(false);

    }
  }//for loop
    
    missionComplete.SetActive(false);
}//set mission


public void UpdateMission( Mission m )
{
  if (m.missionName == activeMission.missionName)
  {
    activeMission = m;
    for (int i =0; i<3; i++)
    {
      if (i < activeMission.myData.Count)
      {
        if (activeMission.myData [i].isCompleted)
        {
          objectiveImage [i].overrideSprite = completeImage;
        }
      }
    }

  }
  
}

public void CompleteMission( Mission m )
{
  for (int i =0; i<3; i++)
  {
    objectiveGameObject [i].SetActive(false);    
  }//end for loop
    missionComplete.SetActive(true);

    if(myGameObject.activeInHierarchy==true)
    {
    StartCoroutine(DelayReset());
    }
}

  IEnumerator DelayReset()
  {
    yield return new WaitForSeconds (3.0f);
    if (myLevelManager.myMissionManager.activeMissions.Count > 0)
  {
    SetMission(myLevelManager.myMissionManager.activeMissions [0]);
  }
  else
  {
      myGameObject.SetActive(false);
  }

  }

}
