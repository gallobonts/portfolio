using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ObjectiveData
{
  public string objectiveName;
  public bool isCompleted;
}

[System.Serializable]
public class Mission  
{
  public string missionName;
  public List<ObjectiveData> myData = new List<ObjectiveData>();
 

  public bool CompleteObjective(string objective)
  {

    foreach (ObjectiveData od in myData)
  {
      if (od.objectiveName==objective && !od.isCompleted)
    {
        od.isCompleted=true;
        return true;
    }
  }
    //check if you have an objective & complete it if you do
 
    return false;
  }

  public bool CompleteMission()
  {
    foreach (ObjectiveData od in myData)
    {
      if(!od.isCompleted)
      {
        return false;
      }
    }
    
    return true;
  }
}
