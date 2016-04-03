using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class IslandLevelManager : LevelManager 
{
	//island stuff
	public float tempCameraZed=-1405;

	public GameObject[]  islandCanvas = new GameObject[3];
  public PauseMenuUI[] pauseMenuUI= new PauseMenuUI[3];
  public IslandGUI[] islandGUI= new IslandGUI[3];

  public List<GameObject>ownedLamina = new List<GameObject>();
  public List<LaminaBrain_Island> players = new List<LaminaBrain_Island>();
  int[] equippedLamina = new int[2];//the lamina they have equipped in relation to the owned list

  public Transform ownedLaminaParent;

 
	override public void  InitializeLevel (LevelManagerInitialization initialize)
	{
    if (players.Count > 0)
  {
      Debug.Log("sir we have a problem");
      foreach( LaminaBrain_Island lb in players)
      {
        Destroy(lb.gameObject);
      }
      players.Clear();
      ownedLamina.Clear();
  }

    if (GOD.myGOD.isNewGame)
  {
    SetUpNewGame();
  }


 
    InitializeOwnedMonsterList(initialize.ownedMonsters, initialize.player1Mon, initialize.player2Mon);
    InitializeCanvases();

    if(GOD_Memory.instance==null)
    {
     GOD.myGOD.GetComponent<GOD_Memory>().Awake();
    }
      GOD_Memory.instance.Load();


    GOD.myGOD.isNewGame = false;



	}
	
  void SetUpNewGame()
   {
      //randomize the start of the island
 //   Debug.Log("set up new game");

   }
  void InitializeCanvases()
  {
    if (!firstPlayerSet)
    {
      registerFirstPlayer.SetActive(true);
    }
    else
    {
      registerFirstPlayer.SetActive(false);

      SetUpDevices();

      islandGUI[0].SetUp(players [equippedLamina[0]].GetGUIInfo());
      islandGUI[1].SetUp(players [equippedLamina[0]].GetGUIInfo());

    }
    
    islandCanvas [1].SetActive(false);
    islandCanvas [2].SetActive(false);
  }

  public void SetUpDevices( )
  {
    SetDevices(0,firstPlayerInputDevice, firstPlayerControls);
    SetDevices(1,firstPlayerInputDevice, firstPlayerControls);
    if(isCoOp)
    {SetDevices(2,secondPlayerInputDevice, secondPlayerControls);}
  }

  public delegate void SetDeviceType(int playerNum,InputDevice newDevice, Controls newControls);
  public static event SetDeviceType SetDevices;

  void InitializeOwnedMonsterList(List<string> ownedMonsters, int player1Mon, int player2Mon)
  {
   
    GameObject temp = null;
    float tempX = 0;
    foreach (string i in ownedMonsters)
    {
  //    Debug.Log("initializing owned monster # " +i);
      //load lamina game objects

      temp = Resources.Load<GameObject>(i + "Lamina_Island");
      
      if (temp == null)
      {
        Debug.Log(i + "Lamina_Island not found in Resources");
      }
      
      temp = GameObject.Instantiate(temp) as GameObject;

      temp.transform.parent = ownedLaminaParent;

      //temp code until huts are programmed in
      temp.transform.localPosition= new Vector3(0,0,0);
      temp.transform.position+= new Vector3(tempX,0,0);
      tempX+=5;

      ownedLamina.Add(temp);
      LaminaBrain_Island tempBrain= (LaminaBrain_Island)temp.GetComponent<LaminaBrain_Island>();
      tempBrain.SetInactive();
      players.Add(tempBrain);

      
  
    }
    equippedLamina[0] = player1Mon;
    equippedLamina[1] = player2Mon;
    
  }

 
override public void PauseGame(int playerNum)
{
		if(!isCoOp)
		{
			pauseMenuUI[0].ChangeFlow("MainMenu");//open up 1player pause menu
		}
		else//else it's co-op
		{
			if(playerNum==1)
			{
				pauseMenuUI[1].ChangeFlow("MainMenu");//open up 2player p1 pause menu
			}
			else//else it's player 2
			{
				pauseMenuUI[2].ChangeFlow("MainMenu");//open up 2player p2 pause menu
			}
		}
}

override public void UnPauseGame(int playerNum)
{

		if(!isCoOp)
		{
 			pauseMenuUI[0].ResetPanels();//close up 1player pause menu
		}
		else//else it's co-op
		{
			if(playerNum==1)
			{
				pauseMenuUI[1].ResetPanels();//close up 2player p1 pause menu
			}
			else//else it's player 2
			{
				pauseMenuUI[2].ResetPanels();//open up 2player p2 pause menu
			}
		}

}

  override public void RegisterController(InputDevice inputDevice, Controls newControls)
  {
    //island register controller
    if (!firstPlayerSet)//register player 1 in game
    {
      //prevent re-registering p1
      firstPlayerSet = true;
      
      //setup input & controls
      firstPlayerControls = newControls; 
      firstPlayerInputDevice = inputDevice;
      
      //remove the 'press start' hud item
      registerFirstPlayer.SetActive(false);
      
      //activate player 1

      if(equippedLamina[0] == -1)//if player 1 isn't set
      {
        
        equippedLamina[0] = 0;
        equippedLamina[1] = -1;//make sure player 2 isn't set either
      }
     
     
      players [equippedLamina[0]].SetActive(firstPlayerInputDevice, firstPlayerControls, 1);


      
      //set up canvas
      islandCanvas [0].SetActive(true);
      islandCanvas [1].SetActive(false);
      islandCanvas [2].SetActive(false);

      //set up pause menu
      SetUpDevices();
      
      //reset pause menus
      pauseMenuUI[0].ResetPanels();
      pauseMenuUI[1].ResetPanels();
      pauseMenuUI[2].ResetPanels();
      
      //set up ui
      islandGUI[0].SetUp(players [equippedLamina[0]].GetGUIInfo());
      islandGUI[1].SetUp(players [equippedLamina[0]].GetGUIInfo());



    }//done registering player 1


    else if (!isCoOp)//register player 2 in game
    {
      if (GOD.myGOD.isGamePaused == true)
      {
        return;
      }
      
      Controls check = players [equippedLamina[0]].GetController();
      if(check.Device== inputDevice)
      {
            return;
      }
      //setup input & controls
      secondPlayerInputDevice = inputDevice;
      secondPlayerControls = newControls;
      
      //remove the 'press start' hud button
      //registerSecondPlayer.SetActive(false);
      
      //set up camaeras
      isCoOp = true;
      
      //activate player 2
      //if a lamina is previously equipped, keep that one equipped, else equip the first one in the list
      //give player1 controlers

      if (equippedLamina[1] == -1)//if we don't have a specific lamina equipped
      {
        if(equippedLamina[0]!=0)//if player 1 doesn't already have the 1st lamina equipped, then equip it
        {
          equippedLamina[1]=0;
        }
        else//else equipe the 2nd lamina
        {
          equippedLamina[1]=1;
        }

      }//

      Debug.Log("registering player 2 with mon number " + equippedLamina[1]);
     players [equippedLamina[1]].SetActive(secondPlayerInputDevice, secondPlayerControls, 2);
      
     //setup pause menu
       SetUpDevices();  
      
      
      //reset pause menus
      pauseMenuUI[1].ChangeFlow("MainMenu");//fixes a bug that occured when p1 unregisters, then re-registers making p2->p1 and then when p1 pressed interact, menu was not resetting
      
      pauseMenuUI[0].ResetPanels();
      pauseMenuUI[1].ResetPanels();
      pauseMenuUI[2].ResetPanels();

      //set up pause menu
      islandGUI[2].SetUp(players [equippedLamina[1]].GetGUIInfo());


      islandCanvas [0].SetActive(false);
      islandCanvas [1].SetActive(true);
      islandCanvas [2].SetActive(true);
      
      
    }//end else if
    
    SetUpCameras();
    
  
  }//end register player



  override public void UnRegisterController(int playerNum)
  {
    
    if(playerNum==1)//remove player 1 & set player 2 up as player 1
    {
      if(!isCoOp)//unregister first player & no player 2 set up
      {
        //reset to no1 being registered
        registerFirstPlayer.SetActive(true);
        firstPlayerSet=false;
      }
      else//remove player 1
      {
        
        //mark it as not co-op
        isCoOp=false;
        
        //swap control & input
        firstPlayerControls=secondPlayerControls; 
        firstPlayerInputDevice=secondPlayerInputDevice;
        secondPlayerControls=null;
        secondPlayerInputDevice=null;
        
        //make sure game isn't paused
        GOD.myGOD.isGamePaused=false;
        GOD.myGOD.isPlayerPaused[0]=false;
        GOD.myGOD.isPlayerPaused[1]=false;
                

        //unequip lamina
        players [equippedLamina[0]].SetInactive();
        players [equippedLamina[1]].SetInactive();

      //update equipped lamina
        equippedLamina[0]=equippedLamina[1];
        equippedLamina[1]=-1;
        
        
        
        //re-equip player 1
        players[equippedLamina[0]].SetActive(firstPlayerInputDevice, firstPlayerControls, 1);

        
        //set up new pause menu
        SetUpDevices();

        islandGUI[0].SetUp(players [equippedLamina[0]].GetGUIInfo());
        islandGUI[1].SetUp(players [equippedLamina[0]].GetGUIInfo());

        
        
        SetUpCameras();
       
        
        pauseMenuUI[0].ResetPanels();
        pauseMenuUI[1].ResetPanels();
        pauseMenuUI[2].ResetPanels();
        
        islandCanvas[0].SetActive(true);
        islandCanvas[1].SetActive(false);
        islandCanvas[2].SetActive(false);
        
        
        
        
      }//else myGOD.isCoOp
    }
    else//just remove player 2
    {
      //mark it as not co-op
      isCoOp=false;
      
      //make sure game isn't paused
      GOD.myGOD.isGamePaused=false;
      GOD.myGOD.isPlayerPaused[0]=false;
      GOD.myGOD.isPlayerPaused[1]=false;

      players[equippedLamina[1]].SetInactive(); 
      equippedLamina[1]=-1;
    
      
      SetUpCameras();
      
      
      pauseMenuUI[0].ResetPanels();
      pauseMenuUI[1].ResetPanels();
      pauseMenuUI[2].ResetPanels();
      
      islandCanvas[0].SetActive(true);
      islandCanvas[1].SetActive(false);
      islandCanvas[2].SetActive(false);
      
      
      
      
    }//else co-op
    
    
  }


  override protected void SetUpCameras( )// setup camera->get camera target-> set camera target
  {
    
    if(singlePlayerCamera==null || splitScreenCameras[0]==null || splitScreenCameras[1]==null)
    {
      UnityEngine.Debug.Log("Camera's Not Set");
      return;
    }
    
    if(!isCoOp)
    {
      singlePlayerCamera.SetActive(true);
      splitScreenCameras[0].SetActive(false);
      splitScreenCameras[1].SetActive(false);
      
    }
    else
    {
      
      
      singlePlayerCamera.SetActive(false);
      splitScreenCameras[0].SetActive(true);
      splitScreenCameras[1].SetActive(true);
      
    }
    SetCameraTargets();
    
  }

  void SetCameraTargets()
  {
    if(!isCoOp)
    {
      FollowPlayer camera1=singlePlayerCamera.GetComponent<FollowPlayer>() as FollowPlayer;
      camera1.SetTarget(players[equippedLamina[0]].myGameObject);
    }
    else
    {
      FollowPlayer camera1= splitScreenCameras[equippedLamina[0]].GetComponent<FollowPlayer>() as FollowPlayer;
      camera1.SetTarget(players[equippedLamina[0]].myGameObject);
      FollowPlayer camera2= splitScreenCameras[equippedLamina[1]].GetComponent<FollowPlayer>() as FollowPlayer;
      camera2.SetTarget(players[equippedLamina[1]].myGameObject);
    }
    
    
  }


	
}
