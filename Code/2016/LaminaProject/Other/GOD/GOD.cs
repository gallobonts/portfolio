using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using InControl;

public class GOD : MonoBehaviour {

	//singleton stuff
	public static GOD myGOD;
	bool nullify=false;//helps with singleton design

  public bool loadGame=false;

	//general load screen
	public GameObject loadScreenPrefab;
	LoadingScreen loadScreen;
	LoadControls loadControls= new LoadControls();

	//game state, is each controller paused & is entier game paused
  public bool[] isPlayerFreeze= new bool[2];
	public bool[] isPlayerPaused= new bool[2];
	public bool isGamePaused=false;
	
  [HideInInspector]
  public bool isNewGame=false;
	//register controller stuff
	public LevelManager currentLevelManager;

	public List<string> ownedLamina;
  public int currentSkulls=0;

  public int ownedGraveMiroks=0;
	public int player1EquippedLamina=-1;
	public int player2EquippedLamina=-1;

  public GOD_MirokManager myMirokManager;

  public delegate void voidDelegate();//used for the timer function
  
 

	void Awake()
	{

    if(nullify)
		{return;}

		if(myGOD!=null && myGOD!=this)
		{
			Destroy(this.gameObject);
      nullify=true;
		}

		DontDestroyOnLoad(this.gameObject);
		myGOD= this;

 
    Load();

    //we reset it on level change

		OnLevelWasLoaded(Application.loadedLevel);

	}


  void Save()
  {
    string savePath = GOD_Memory.rootFolder + "god";
    
    GOD_Memory.instance.CheckPath(savePath);
    
    savePath+="?tag=" + "god";

      //save base stats

      ES2.Save<int>(myGOD.currentSkulls, savePath    + "skullCurrency");
      ES2.Save(myGOD.ownedLamina, savePath + "ownedLamina");

  }

	void Load()
  {
    if(!myGOD.loadGame){return;}


    string loadPath = GOD_Memory.rootFolder + "god";
 //   UnityEngine.Debug.Log("load path= " + loadPath);
    if (!ES2.Exists(loadPath)){return;}
    loadPath+="?tag=" + "god";

    myGOD.currentSkulls = ES2.Load<int>(loadPath + "skullCurrency");

    myGOD.ownedLamina = ES2.LoadList<string>(loadPath + "ownedLamina");


  }

	void OnLevelWasLoaded(int level)
	{
 		if(nullify)
		{return;}

		if(myGOD!=null && myGOD!=this)
		{
			DestroyImmediate(this.gameObject);
      myGOD.nullify=true;
		}

		if(!myGOD.loadScreen)//ensure loadscreen is set up
		{
			myGOD.loadScreen= GameObject.Instantiate(loadScreenPrefab).GetComponent<LoadingScreen>();
			myGOD.loadScreen.transform.SetParent(this.transform);
		}

    GOD_Memory.SaveEarly += Save;
   
		//initialize shit
		myGOD.currentLevelManager= (LevelManager) GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>() as LevelManager;
    LevelManagerInitialization initialize= new LevelManagerInitialization();

    initialize.ownedMonsters = ownedLamina;
    initialize.player1Mon = player1EquippedLamina;
    initialize.player2Mon = player2EquippedLamina;
    initialize.ownedGraveMiroks = ownedGraveMiroks;
  	myGOD.currentLevelManager.InitializeLevel (initialize);


    myGOD.currentLevelManager.SetLoadControls(myGOD.loadControls);

		FreezeGame(false);
		myGOD.isPlayerPaused[0]=false;
		myGOD.isPlayerPaused[1]=false;
    myGOD.isPlayerFreeze [0] = false;
    myGOD.isPlayerFreeze [1] = false;

    myGOD.nullify = true;

	}//end onlevel was loaded

  	
public void UnRegisterController(int playernNum)
{
	myGOD.currentLevelManager.UnRegisterController(playernNum);
}

public void RegisterController(InputDevice inputDevice,Controls newControls)
{
  myGOD.currentLevelManager.RegisterController(inputDevice,newControls);
  
}//end register player

 public List<MirokButtonInfo> GetGraveMiroks ()
  {  

    return myMirokManager.graveMirokButtons;
 

  }
void FreezeGame(bool freeze)
	{
		if(freeze)
		{			
			myGOD.isGamePaused=true;
			Time.timeScale=0.0f; //freeze gameplay
		}
		else
		{
			myGOD.isGamePaused=false;
			Time.timeScale=1.0f; //unfreeze gameplay
		}
	}

  public void FreezePlayer(int FreezePlayerNum)
  {
    if(!myGOD.currentLevelManager.isCoOp)
    {
      myGOD.isPlayerFreeze[0]=true;//mark player 1 as paused
      FreezeGame(true);
      
    }
    else//else it's co-op
    {
      if(FreezePlayerNum==1)
      {
        
        myGOD.isPlayerFreeze[0]=true;//markplayer 1 as paused
        
        if(myGOD.isPlayerFreeze[1])//if player 2 is already paused, freeze gameplay
        {FreezeGame(true);}
        
        
      }
      else//else it's player 2
      {
        
        myGOD.isPlayerFreeze[1]=true;//markplayer 2 as paused
        
        if(myGOD.isPlayerFreeze[0])//if player 1 is already paused, freeze gameplay
        {FreezeGame(true);}
        
      }
    }
  }

	public void PauseGame(int pausePlayerNum)
	{
    UnityEngine.Debug.Log("Pause game " + pausePlayerNum);

    if(isPlayerFreeze[pausePlayerNum-1]){return;}//quit if we are already paused

    myGOD.FreezePlayer(pausePlayerNum);
    isPlayerPaused [pausePlayerNum-1] = true;
		myGOD.currentLevelManager.PauseGame(pausePlayerNum);
	}

 
  public void UnFreezePlayer(int unFreezePlayerNum)
  {
    
    isPlayerPaused [unFreezePlayerNum] = false;

    if(!myGOD.currentLevelManager.isCoOp)
    {
      myGOD.isPlayerFreeze[0]=false;//mark player 1 as !paused
      FreezeGame(false);
      
    }
    else//else it's co-op
    {
      if(unFreezePlayerNum==1)
      {
        
        myGOD.isPlayerFreeze[0]=false;//markplayer 1 as !paused
        FreezeGame(false);
        
      }
      else//else it's player 2
      {
        
        myGOD.isPlayerFreeze[1]=false;//markplayer 2 as !paused
        FreezeGame(false);
      }
    }

  }

  public void Die(int playerNum)
  {
    if (playerNum == 1)
    {
    }
    else
    {
    }
    currentLevelManager.Die(playerNum);
  }


 
	public void UnPauseGame(int pausePlayerNum)
	{
    UnityEngine.Debug.Log("unpause game " + pausePlayerNum);
    
    if(!isPlayerFreeze[pausePlayerNum-1]){return;}//quit if we are already paused

    isPlayerPaused [pausePlayerNum-1] = false;
    UnFreezePlayer(pausePlayerNum);
		myGOD.currentLevelManager.UnPauseGame(pausePlayerNum);
	}


	public void LoadLevel(string levelToLoad)
	{
    GOD_Memory.instance.Save();
    myGOD.loadControls=myGOD.currentLevelManager.GetLoadControls();

 

    //this prevents objects who have subscribed to saving & loading from being maintained throughout the scenes
    GOD_Memory.instance.ClearDelegates();

    myGOD.nullify = false;
    myGOD.loadGame = true;

		myGOD.loadScreen.LoadLevel(levelToLoad);
	}
	
	public void QuitGame(int pausePlayerNum)
	{
		if(!myGOD.currentLevelManager.isCoOp)
		{
			LoadLevel("StartScene");
		}
		else//else it's co-op
		{
			UnRegisterController(pausePlayerNum);
		}

	}

  public void MeetLamina(string newLamina)
  {
    ownedLamina.Add(newLamina);
    currentLevelManager.MeetLamina(newLamina);
  }

  public bool ReviveLamina()
  {
    return currentLevelManager.ReviveLamina();
  }

  //these should probably end up somewhere else

  public bool isLayerinLayerMask(int layer, LayerMask mask)
  {
    return (mask == (mask | (1 << layer))); // removes the layer from the mask, then checks if it's still the same mask
  
  }

  //allows 'invoke' type methods to occur without needing a mono behavior
  public void StartTimer(voidDelegate methodToCall,float time)
  {
    StartCoroutine (TimerCoroutine (time, methodToCall));
  }
  
  private IEnumerator TimerCoroutine(float time, voidDelegate methodToCall)
  {
    yield return new WaitForSeconds (time);
    methodToCall.Invoke ();
  }


  //should probbaly be in a different class
	public void TraceStack()
	{
		// Get call stack
		StackTrace trace = new StackTrace();
		string traceTxt;
		if( trace.FrameCount>=3)
		{traceTxt="trace: function called by " +trace.GetFrame(2).GetMethod().Name;}
		else
		{traceTxt= "trace: origin unkown";}
		UnityEngine.Debug.Log(traceTxt);
	}

  public void DebugStar(Vector2 pos)
    //draws a star at said point...can be used to figure out what targets you are setting for debugging purposes
  {
    Vector3 start;
    Vector3 end;
    start = pos + new Vector2( 0.5f, 0f);
    end = pos +new Vector2( -0.5f, 0f);
    UnityEngine.Debug.DrawLine ( start, end, Color.red, 0, false );
    start = pos + new Vector2( 0f, 0.5f);
    end = pos + new Vector2( 0f, -0.5f);
    UnityEngine.Debug.DrawLine ( start, end, Color.red, 0, false);

    //z axis, if needed in future
 //   start = pos + Vector3( 0, 0, 0.5 );
  //  end = pos + Vector3( 0, 0, -0.5 );
   // UnityEngine.Debug.DrawLine ( start, end, Color.red, 0, false);
  }

}


