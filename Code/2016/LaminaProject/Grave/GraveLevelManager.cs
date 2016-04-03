using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;



public class GraveLevelManager : LevelManager
{

public PlayerLaminaManager[] players = new PlayerLaminaManager[2];
public GameObject[]  graveCanvas = new GameObject[3];
public PauseMenuUI[] pauseMenuUI = new PauseMenuUI[3];
public GraveLaminaSelectionGUI[]laminaSelectionGUI= new GraveLaminaSelectionGUI[3];
public IslandGUI[] islandGUI= new IslandGUI[3];


 public Transform ownedLaminaParent;
//[HideInInspector]
//public List<string> ownledMonsters = new List<string>();


 //crystal state stuff
[HideInInspector]
public List<GameObject>ownedLamina = new List<GameObject>();
public List<Sprite> ownedLaminaSprites = new List<Sprite>();//all of the owned lamina
public List<Sprite> unusedLaminaSprites = new List<Sprite>();//all of the unusued ones
public List<Sprite> deadLaminaSprites= new List<Sprite>();  //all of the dead ones
bool[] dead = new bool[2];


 int[] equippedLamina = new int[2];//the lamina they have equipped in relation to the owned list
 int[] crystalLamina = new int[2];//the lamina they have equipped in relation to the unused list

 bool[] playerInCrystalState = new bool[2];

override public void InitializeLevel(LevelManagerInitialization initialize)
{
    InitializeOwnedMonsterList(initialize.ownedMonsters, initialize.player1Mon, initialize.player2Mon);

    InitializeCanvases();
    playerInCrystalState [0] = false;
    playerInCrystalState [1] = false;

    GOD_Memory.instance.Load();

}

 
 
void InitializeOwnedMonsterList(List<string> ownedMonsters, int player1Mon, int player2Mon)
{

  foreach (string i in ownedMonsters)
  {
      MeetLamina(i);  
   }
  equippedLamina[0] = player1Mon;
  equippedLamina[1] = player2Mon;

}

  public override bool ReviveLamina()
  {
      if (deadLaminaSprites.Count > 0)
    {
      unusedLaminaSprites.Add(deadLaminaSprites[0]);
      deadLaminaSprites.RemoveAt(0);
    
    return true;
  }
  else
  {
      return false;
  }
  }

  public override void MeetLamina(string newLamina)
  {
    GameObject temp = null;
    Sprite tempSprite = null;

    //load lamina game objects
    temp = Resources.Load<GameObject>(newLamina + "Lamina_Grave");
    
    if (temp == null)
    {
      Debug.Log(newLamina + "Lamina_Grave not found in Resources");
    }
    
    temp = GameObject.Instantiate(temp) as GameObject;
    temp.transform.parent = ownedLaminaParent;
    ownedLamina.Add(temp);
    temp.SetActive(false);
    
    //load lamina face sprites
    tempSprite = Resources.Load<Sprite>(newLamina + "Lamina_FaceSprite");
    if (tempSprite == null)
    {
      Debug.Log(newLamina + "Lamina_FaceSprite not found in Resources");
    }
    
    ownedLaminaSprites.Add(tempSprite);
    unusedLaminaSprites.Add(tempSprite);


    if (playerInCrystalState [0])//if player 1 is already in crystal state
  {
      EnterCrystalState(1);//re-enter player 1

  }
    if (playerInCrystalState [1])//if player 2 is already in crystal state
  {
      EnterCrystalState(2);//re-enter player 2 

  }
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
    pauseMenuUI [0].SetDevice(1,firstPlayerInputDevice, firstPlayerControls);
    pauseMenuUI [1].SetDevice(1,firstPlayerInputDevice, firstPlayerControls); 
    laminaSelectionGUI[0].SetDevice(firstPlayerInputDevice, firstPlayerControls);
    laminaSelectionGUI[1].SetDevice(firstPlayerInputDevice, firstPlayerControls);

  }

  graveCanvas [1].SetActive(false);
  graveCanvas [2].SetActive(false);
}

override public void Die(int playerNum)
{
   if (playerNum == 1)
  {
      deadLaminaSprites.Add(ownedLaminaSprites[equippedLamina[0]]);
      equippedLamina[0]=-1;
      dead[0]=true;
  }
  else
  {
      deadLaminaSprites.Add(ownedLaminaSprites[equippedLamina[0]]);
      equippedLamina[1]=-1;
      dead[1]=true;
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
    players [0].gameObject.SetActive(true);
  
      
    //give player1 controlers
      GameObject playerLamina;
    
    //if a lamina is previously equipped, keep that one equipped, else equip the first one in the list
    if (equippedLamina[0] != -1)
    {
 
      playerLamina = ownedLamina [equippedLamina[0]];
    }
    else
    {
 
      playerLamina = ownedLamina [0];
      equippedLamina[0] = 0;
      equippedLamina[1] = -1;//make sure player 2 isn't set either
    }
 
      unusedLaminaSprites.Remove(ownedLaminaSprites[equippedLamina[0]]);//remove the player 1 lamina from the unused lamina list
 
    players [0].SetActive(true, firstPlayerInputDevice, firstPlayerControls, 1, playerLamina);
     
      
    //set up pause menu
    pauseMenuUI [0].SetDevice(1,firstPlayerInputDevice, firstPlayerControls);
    pauseMenuUI [1].SetDevice(1,firstPlayerInputDevice, firstPlayerControls);
    laminaSelectionGUI[0].SetDevice(firstPlayerInputDevice, firstPlayerControls);
    laminaSelectionGUI[1].SetDevice(firstPlayerInputDevice, firstPlayerControls);
    
    
    islandGUI[0].SetUp(players [0].GetGUIInfo());
    islandGUI[1].SetUp(players [0].GetGUIInfo());
   
      //reset pause menus
    pauseMenuUI[0].ResetPanels();
    pauseMenuUI[1].ResetPanels();
    pauseMenuUI[2].ResetPanels();

    //set up canvas
    graveCanvas [0].SetActive(true);
    graveCanvas [1].SetActive(false);
    graveCanvas [2].SetActive(false);
      
 
  }//done registering player 1
  else if (!isCoOp)//register player 2 in game
  {
    if (GOD.myGOD.isGamePaused == true)
    {
      return;
    }
      
    Controls check = players [0].GetController();
    if (check.Device == inputDevice)
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
      GameObject playerLamina;
    
   
    if (equippedLamina[1] != -1)//if we already have a specific lamina equipped, equip it
    {
        Debug.Log("enter if");
      playerLamina = ownedLamina [equippedLamina[1]];
    }
    else
    {
        if(equippedLamina[0]!=0)//if player 1 doesn't already have the 1st lamina equipped, then equip it
        {
          Debug.Log("enter 2nd if");
        playerLamina = ownedLamina [0];
        equippedLamina[1]=0;
        }
        else//else equipe the 2nd lamina
        {
          playerLamina = ownedLamina [1];
          equippedLamina[1]=1;
     
        }
     }
      unusedLaminaSprites.Remove(ownedLaminaSprites[equippedLamina[1]]);//remove the player 2 lamina from the unused lamina list

    players [1].SetActive(true, secondPlayerInputDevice, secondPlayerControls, 2, playerLamina);
   
    //setup pause menu
    pauseMenuUI [2].SetDevice(2,secondPlayerInputDevice, secondPlayerControls);
      laminaSelectionGUI[2].SetDevice(secondPlayerInputDevice, secondPlayerControls);
      islandGUI[2].SetUp(players [1].GetGUIInfo());
      Debug.Log("test2");

     
      
      
    //reset pause menus
      pauseMenuUI[1].ChangeFlow("MainMenu");//fixes a bug that occured when p1 unregisters, then re-registers making p2->p1 and then when p1 pressed interact, menu was not resetting

    pauseMenuUI[0].ResetPanels();
    pauseMenuUI[1].ResetPanels();
    pauseMenuUI[2].ResetPanels();

    graveCanvas [0].SetActive(false);
    graveCanvas [1].SetActive(true);
    graveCanvas [2].SetActive(true);
   

  }//end else if

  SetUpCameras();

}//end register player

 
 public void ReturnToIsland()
 {
    GOD.myGOD.LoadLevel("new_island");


 }

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


        //re capture lamina
        players[0].RemoveLamina(ownedLaminaParent);
        unusedLaminaSprites.Add(ownedLaminaSprites[equippedLamina[0]]);//add the player 1 lamina back to the unused lamina list

      
        //swap brains
        PlayerLaminaManager temp=null;
        temp=players[0];
        players[0]=players[1];
        players[1]=temp;


        //update equipped lamina
        equippedLamina[0]=equippedLamina[1];
        equippedLamina[1]=-1;
      
       

        //re-equip player 1
        GameObject playerLamina;
        playerLamina = ownedLamina [equippedLamina[0]];
        players [0].SetActive(true, firstPlayerInputDevice, firstPlayerControls, 1, playerLamina);
   

        //set up new pause menu
        pauseMenuUI[0].SetDevice(1,firstPlayerInputDevice,firstPlayerControls);
        pauseMenuUI[1].SetDevice(1,firstPlayerInputDevice,firstPlayerControls);
        laminaSelectionGUI[0].SetDevice(firstPlayerInputDevice, firstPlayerControls);
        laminaSelectionGUI[1].SetDevice(firstPlayerInputDevice, firstPlayerControls);
        islandGUI[0].SetUp(players [0].GetGUIInfo());
        islandGUI[1].SetUp(players [0].GetGUIInfo());

         
        
        SetUpCameras();
        
        
        pauseMenuUI[0].ResetPanels();
        pauseMenuUI[1].ResetPanels();
        pauseMenuUI[2].ResetPanels();
        
        graveCanvas[0].SetActive(true);
        graveCanvas[1].SetActive(false);
        graveCanvas[2].SetActive(false);
        
        
        

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



      //re capture p2 lamina
      players[1].RemoveLamina(ownedLaminaParent);
            
      //update equipped lamina
      unusedLaminaSprites.Add(ownedLaminaSprites[equippedLamina[1]]);//add the player 2 lamina back to the unused lamina list

      equippedLamina[1]=-1;

      
      SetUpCameras();
      
      
      pauseMenuUI[0].ResetPanels();
      pauseMenuUI[1].ResetPanels();
      pauseMenuUI[2].ResetPanels();
      
      graveCanvas[0].SetActive(true);
      graveCanvas[1].SetActive(false);
      graveCanvas[2].SetActive(false);
      
      


    }//else co-op

   
}



  override public void PauseGame(int playerNum)
{
  if (!isCoOp)
  {
    pauseMenuUI [0].ChangeFlow("MainMenu");//open up 1player pause menu
  }
  else//else it's co-op
  {
    if (playerNum == 1)
    {
      pauseMenuUI [1].ChangeFlow("MainMenu");//open up 2player p1 pause menu
    }
    else//else it's player 2
    {
      pauseMenuUI [2].ChangeFlow("MainMenu");//open up 2player p2 pause menu
    }
  }
}//end pause game
  
override public void UnPauseGame(int playerNum)
{
  if (!isCoOp)
  {
    pauseMenuUI [0].ResetPanels();//close up 1player pause menu
  }
  else//else it's co-op
  {
    if (playerNum == 1)
    {
      pauseMenuUI [1].ResetPanels();//close up 2player p1 pause menu
    }
    else//else it's player 2
    {
      pauseMenuUI [2].ResetPanels();//open up 2player p2 pause menu
    }
  }
    
}//end unpause game


 public void EnterCrystalState(int crystalPlayerNum)
  {
    GOD.myGOD.FreezePlayer(crystalPlayerNum);
 
    int selectedLamina;

    //single player enter crystal state
    if (!isCoOp)
    {  
      //first get a copt of player 1 equipped lamina, then unequip it
      if(equippedLamina[0]!=-1)//used for looping the 're-enter' crystal state
      {
        //add the old lamina back to the list of unused
        unusedLaminaSprites.Add(ownedLaminaSprites[equippedLamina[0]]);

        crystalLamina[0]= equippedLamina[0];
        equippedLamina[0]=-1;
      }
    
      if(dead[0])//if we entered here due to death
      {
        selectedLamina=0;
      }
      else//entered normally
      {
     selectedLamina= unusedLaminaSprites.IndexOf(ownedLaminaSprites[crystalLamina[0]]);
      }

      if(unusedLaminaSprites.Count==0)
      {Debug.Log("game over");return;}

      //set up lamina selection
      laminaSelectionGUI[0].SetUp(unusedLaminaSprites, selectedLamina, crystalPlayerNum);
      playerInCrystalState [0] = true;

    }

    //co op enter crystal state
    else
    {
        //1st player in co op enter crystal state
      if (crystalPlayerNum == 1)
      {
        //first get a copt of player 1 equipped lamina, then unequip it
        if(equippedLamina[0]!=-1)//used for looping the 're-enter' crystal state
        {
          //add the old lamina back to the used list
          unusedLaminaSprites.Add(ownedLaminaSprites[equippedLamina[0]]);
          
          crystalLamina[0]= equippedLamina[0];
          equippedLamina[0]=-1;

          if(playerInCrystalState[1])//if player 2 is already in crystal state
          {
            EnterCrystalState(2);//re-enter player 2 
          }
          playerInCrystalState [0] = true;//after enter crystal state to prevent loop

        }
        if(dead[0])//if we entered here due to death
        {
          selectedLamina=0;
        }
        else//entered normally
        {
          selectedLamina= unusedLaminaSprites.IndexOf(ownedLaminaSprites[crystalLamina[0]]);
        }
        
        if(unusedLaminaSprites.Count==0)
        {Debug.Log("game over");return;}

        //set up lamina selection
        laminaSelectionGUI[1].SetUp(unusedLaminaSprites, selectedLamina, crystalPlayerNum);

    
      }
      else//else it's player 2
      {
        //first get a copy of player 2 equipped lamina, then unequip it
        if(equippedLamina[1]!=-1)//used for looping the 're-enter' crystal state
        {
          //add the old lamina back to the list
          unusedLaminaSprites.Add(ownedLaminaSprites[equippedLamina[1]]);
          
          crystalLamina[1]= equippedLamina[1];
          equippedLamina[1]=-1;

          if(playerInCrystalState[0])//if player 1 is already in crystal state
          {
            EnterCrystalState(1);//re-enter player 1
          }
          playerInCrystalState [1] = true;//after enter crystal state to prevent loop

        }

        if(dead[1])//if we entered here due to death
        {
          selectedLamina=0;
        }
        else//entered normally
        {
          selectedLamina= unusedLaminaSprites.IndexOf(ownedLaminaSprites[crystalLamina[1]]);
        }
        
        if(unusedLaminaSprites.Count==0)
        {Debug.Log("game over");return;}
        
        //set up lamina selection
        laminaSelectionGUI[2].SetUp(unusedLaminaSprites, selectedLamina, crystalPlayerNum);

      }
    }
   
//    Debug.Log("entered crystal state");

  }//enter crystal state
  public void ExitCrystalState(Sprite selectedLamina, int crystalPlayerNum)
 {
    GOD.myGOD.UnFreezePlayer(crystalPlayerNum);
    dead [crystalPlayerNum-1] = false;

    int selected = ownedLaminaSprites.IndexOf(selectedLamina);

    GameObject playerLamina;
    if (!isCoOp)
    {
     
      equippedLamina[0] = selected;
      playerLamina = ownedLamina [selected];
      players [0].SwitchLamina(playerLamina);
      islandGUI[0].SetUp(players [0].GetGUIInfo());
      islandGUI[1].SetUp(players [0].GetGUIInfo());

      unusedLaminaSprites.Remove(ownedLaminaSprites[equippedLamina[0]]);


      laminaSelectionGUI[0].SetPanel(false);
      playerInCrystalState [0] = false;
     

    }
    else//else it's co-op
    {
      if (crystalPlayerNum == 1)
      {
        equippedLamina[0] = selected;
        playerLamina = ownedLamina [selected];
        players [0].SwitchLamina(playerLamina);
        islandGUI[0].SetUp(players [0].GetGUIInfo());
        islandGUI[1].SetUp(players [0].GetGUIInfo());

        laminaSelectionGUI[1].SetPanel(false);//open up 2player p1 pause menu
        playerInCrystalState [0] = false;//before enter to prevent loop

        //remove the lamina from the unused list
        unusedLaminaSprites.Remove(ownedLaminaSprites[equippedLamina[0]]);


        if(playerInCrystalState[1])//if player 2 is already in crystal state
        {
          if(equippedLamina[0]==crystalLamina[1])//if player 1 equipped the lamina player 2 previously equipped, then we need to set the 'previously equipped' to something else
          {
            if(crystalLamina[1]==0)//if the 1st lamina was equipped, equip the 2nd
            {crystalLamina[1]=1;}
            else
            {crystalLamina[1]=0;}//else equip the 1st
          }
          EnterCrystalState(2);//re-enter player 2 
        }


      }
      else//else it's player 2
      {
        equippedLamina[1] = selected;
        playerLamina = ownedLamina [selected];
        players [1].SwitchLamina(playerLamina);
       islandGUI[2].SetUp(players [1].GetGUIInfo());


        laminaSelectionGUI[2].SetPanel(false);//open up 2player p2 pause menu
        playerInCrystalState [1] = false;//goes before re-enter to prevent loop

         unusedLaminaSprites.Remove(ownedLaminaSprites[equippedLamina[1]]);

        
        if(playerInCrystalState[0])//if player 1 is already in crystal state
        {
          if(equippedLamina[1]==crystalLamina[0])//if player 2 equipped the lamina player 1 previously equipped, then we need to set the 'previously equipped' to something else
          {
            if(crystalLamina[0]==0)//if the 1st lamina was equipped, equip the 2nd
            {crystalLamina[0]=1;}
            else
            {crystalLamina[0]=0;}//else equip the 1st
          }
          EnterCrystalState(1);//re-enter player 1
        }//end if player 1 in crystal state

      }//end else it's player 2

    }//end else co-op

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
      camera1.SetTarget(players[0].myGameObject);
    }
    else
    {
      FollowPlayer camera1= splitScreenCameras[0].GetComponent<FollowPlayer>() as FollowPlayer;
      camera1.SetTarget(players[0].myGameObject);
      FollowPlayer camera2= splitScreenCameras[1].GetComponent<FollowPlayer>() as FollowPlayer;
      camera2.SetTarget(players[1].myGameObject);
    }
    
    
  }



}//end class
