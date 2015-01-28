using UnityEngine;
using System.Collections;


public class Constructor : MonoBehaviour {

    public int level=0;
    public LevelInfo[] myLevels;
   

 /* *******************************************
 *   Platforms
 *  ******************************************** */
    //object references
    private GameObject platformsInPlay;
    private GameObject platformsDead;
    private GameObject basePlatformPlacement;
    private GameObject basePlatform;
 
    //general platform references
    private Vector3 platformPosition;
    Platform basePlatformClass;

    //spwaning
    private float platformMaxWait;
    private float platformWait;
    private float lastPlatformWait;
    private int maxPlatformCount = 5;//number of platforms initialized
    private bool canSpawnPlatforms = true;
    private bool willCluster = false;
    bool reverseSpawn = false;
  
    //the bounds of the level. As the level changes this would need to be updated
    private float bottomYBound = -55f;
    private float topYBound = 55f;
    private float yBoundScale;
    private float realHeight;

    //height levels used to determine the general area to spawn
    private int heightLevelDeviation;
    private int lastHeightLevel;
    private int currentHeightLevel;
      

    void Start()
    {
        //basic math to allow the editor to only have to choose a value from 1-10 to decide the height
        yBoundScale = (topYBound - bottomYBound) / 10;


        //get all the emtpy game object place holders
        platformsInPlay = GameObject.Find("Platforms_inPlay");
        platformsDead= GameObject.Find("Platforms_dead");
        basePlatformPlacement = GameObject.Find("basePlatformPlacement");
       
        GameObject platformsPlacement = GameObject.Find("PlatformPlacements");
        platformPosition = platformsPlacement.transform.position;
      //get the different heights to place the platforms
        int diffPlatformCount=myLevels[level].platforms.Length;
        
        //add the base platform in
        ResetBase();     
        //acquire level info
        currentHeightLevel = myLevels[level].maxHeightLevel;
        realHeight = currentHeightLevel * yBoundScale;
        heightLevelDeviation = myLevels[level].heightLevelDeviation;
        lastHeightLevel = 0;
        platformMaxWait = myLevels[level].platformWaitTime;
        platformWait = platformMaxWait;

        //add all the other platforms
        for(int i=0;i<diffPlatformCount;i++)//iterate through all possible platforms
        {
            for(int j=0; j<maxPlatformCount; j++)//make allthe platforms at start
            {
                //create max platformcount of platforms per platform type
               GameObject newPlatform = (GameObject) Instantiate(myLevels[level].platforms[i]);
               newPlatform.transform.parent = platformsDead.transform;
            }
        }
    }

    void Update()
    {
        Debug.Log("base platform =" + basePlatformClass.move);
        if (canSpawnPlatforms && !willCluster) { SpawnPlatforms(); }
        else { CheckSpawn();}
    }

    void CheckSpawn()
    {
        platformWait -= Time.deltaTime;
        if (platformWait <= 0f)
        { 
           canSpawnPlatforms = true;
            
           platformWait = Random.Range(0f, platformMaxWait);
           
           //make sure platform wait doesn't linger in the low numbers like .01 for too long
           platformWait += lastPlatformWait;
           if (platformWait > platformMaxWait) { platformWait = Random.Range(0f, .5f * platformMaxWait); }
           
           lastPlatformWait = platformWait;//keep track of what's already been done
        }

    }
    public void LevelReset()
    {

        Destroy(basePlatform);
        ResetBase();

        currentHeightLevel = 0;
         Transform[] resetPlatforms;


        resetPlatforms = platformsInPlay.GetComponentsInChildren<Transform>() as Transform[]; 

        for (int i = 0; i < resetPlatforms.Length; i++)
        {
            resetPlatforms[i].transform.position = platformPosition;
           if (i > 0)
            {
                Platform resetPlatformclass = resetPlatforms[i].GetComponent<Platform>();
                resetPlatformclass.move = false; 
            }
        }
        
        resetPlatforms = platformsDead.GetComponentsInChildren<Transform>() as Transform[]; 
         for (int i = 0; i < resetPlatforms.Length; i++)
        {
            resetPlatforms[i].transform.position = platformPosition;
           if (i > 0)
            {
                Platform resetPlatformclass = resetPlatforms[i].GetComponent<Platform>();
                resetPlatformclass.move = false; 
            }
        }
    
    }
    void ResetBase()
    {
        basePlatform = (GameObject)Instantiate(myLevels[level].basePlatform);
        basePlatform.transform.parent = basePlatformPlacement.transform;
        basePlatform.transform.position = basePlatformPlacement.transform.position;
        basePlatformClass = basePlatform.GetComponent<Platform>();
        if (basePlatformClass == null) { Debug.Log("base Platform in level " + level + " does not contain the platform class"); }
        basePlatformClass.move = true;
        basePlatformClass.speed = myLevels[level].levelSpeed;
       
    }
    void SpawnPlatforms()
    {

        canSpawnPlatforms = false;
        Transform[] newPlatforms;

        if (!reverseSpawn)
        { newPlatforms = platformsDead.GetComponentsInChildren<Transform>() as Transform[]; }
        else
        { newPlatforms = platformsInPlay.GetComponentsInChildren<Transform>() as Transform[]; }
    
        //already went through everything once, so let's repeat
        if (newPlatforms.Length == 1)
        { reverseSpawn = !reverseSpawn;

            if (!reverseSpawn)
                { newPlatforms = platformsDead.GetComponentsInChildren<Transform>() as Transform[];}
             else
                { newPlatforms = platformsInPlay.GetComponentsInChildren<Transform>() as Transform[]; }
        }
        
        //randomize which one we are grabbing. start at 1 because newPlatform[0] will be the empty game object Platforms_dead
        int randPlatform = Random.Range(1, newPlatforms.Length);

        Platform myRandPlatform = newPlatforms[randPlatform].GetComponent<Platform>();

        //make sure we don't pull a platform that is on screen
        if(myRandPlatform.move)
        { canSpawnPlatforms = false; return; }
        //determine the height level
     
        int randHeightLevelDeviation = Random.Range(-heightLevelDeviation, heightLevelDeviation);
        lastHeightLevel = Mathf.Clamp(lastHeightLevel + randHeightLevelDeviation, 0, currentHeightLevel);
    

        float minRandHeight = lastHeightLevel * yBoundScale;
        float maxRandHeight = minRandHeight + yBoundScale - 1f;
        float randHeight =  Random.Range(minRandHeight, maxRandHeight);

        //copy position of one of the possible heights
        newPlatforms[randPlatform].position = platformPosition + new Vector3(0,randHeight,0);
        
      
        
        Platform myPlatform = newPlatforms[randPlatform].GetComponent<Platform>();
        if (myPlatform == null) { Debug.Log("Platform # " + randPlatform + " in level " + level + " does not contain the platform class"); }
        myPlatform.move = true;
        myPlatform.speed = myLevels[level].levelSpeed;
        if (!reverseSpawn)
        { newPlatforms[randPlatform].parent = platformsInPlay.transform; }
        else
        { newPlatforms[randPlatform].parent = platformsDead.transform; }
       

    }

    void OnTrigger2D(Collider2D other)
    {
        willCluster = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        willCluster = false;
    }

}
