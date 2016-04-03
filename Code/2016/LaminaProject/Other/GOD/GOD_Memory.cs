using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GOD_Memory : MonoBehaviour 
{
  public bool saveDebug = false;
  public bool loadDebug = false;
	public static GOD_Memory instance;
  bool nullify=false;//helps with singleton design
  List<string> pathList= new List<string>();

	public static string rootFolder;

	public void Awake()
	{
    if(nullify){return;}

		//give it a singelton style
		if(instance==null)
		{
			instance= this;//
      nullify=true;
			Initialize();
		}


	}
  void OnLevelWasLoaded(int level)
  {
  if (nullify){return;}
    
    //give it a singelton style
    if(instance==null)
    {
      instance= this;//
      nullify=true;
      Initialize();
    }
}
	void Initialize()
	{
		rootFolder= Application.persistentDataPath + "/LaminaSaveData/save1/";
	}


  //save sysem based on 'bik' 's save system

  //3 types of save, to allow things to be saved in a particular order if needed
  public delegate void SaveType();//using delegates so that everything can just subscribe to the master save/load
  public static event SaveType SaveEarly;
  public static event SaveType SaveNormal;
  public static event SaveType SaveLate;

  public delegate void LoadType();//using delegates so that everything can just subscribe to the master save/load
  public static event LoadType LoadEarly;
  public static event LoadType LoadNormal;
  public static event LoadType LoadLate;


  public void CheckPath(string path)
  {
    if (pathList.Contains(path))//we only want to handle each path once
  {
      return;
  }
    pathList.Add(path);

    if (ES2.Exists(path))
    {
      ES2.Delete(path);
    }
  }

  public void EraseGame()
  {
    if (ES2.Exists(rootFolder))
    {
      ES2.Delete(rootFolder);
    }
  }

  public void Save()
  {
   
    if (SaveEarly != null){SaveEarly(); }
    if(SaveNormal!=null){SaveNormal();}
    if(SaveLate!=null){SaveLate();}

    if(saveDebug)
    {Debug.Log("save complete");}

  }

                  
 public void ClearDelegates()
  {
    SaveEarly = null;
    SaveNormal = null;
    SaveLate = null;

    LoadEarly = null;
    LoadNormal = null;
    LoadLate = null;

    pathList.Clear();
  }

  public void Load()
  {
    //find file path
    string laminaFilePath = rootFolder;
    if (LoadEarly != null){LoadEarly(); }
    if(LoadNormal!=null){LoadNormal();}
    if(LoadLate!=null){LoadLate();}
    
    if(loadDebug){Debug.Log("Load complete");}

 }
}

     



