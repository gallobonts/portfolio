using UnityEngine;
using System.Collections;
using RacingGameKit;

static public class StatusChange
{
    public const int RANDOM = 0;
    public const int COLLISION=1;//occured....once
    public const int GOTPASSED=2;//_no
    public const int PASSEDOTHER = 3;//_no  occured once
    public const int DOINGPOORLY = 4;
    public const int DOINGWELL = 5;//_no
    public const int TOOFAST = 6;//works but needs to be changed
    public const int TOOSLOW = 7;
    public const int POWERUP = 8;//ignoring for now
    public const int LEFT = 9;//ignoring for now
    public const int RIGHT = 10;//ignoring for now
    public const int WRONGWAY = 11;//ignoring for now
    public const int LENGTH = 12;
}
public class ComedicCommentary : MonoBehaviour {

    int[] queueCount = new int[StatusChange.LENGTH];
    bool[] isQueue = new bool[StatusChange.LENGTH];
    bool[] audioExists = new bool[StatusChange.LENGTH];
    int frequency;
	public TextAsset commentaryConfigFile;
	
	public AudioClip[] audioClipsStart;
	public AudioClip[] audioClipsFinish;
	public AudioClip[] audioClipsCollision;
	public AudioClip[] audioClipsGettingPassed;
	public AudioClip[] audioClipsPassingOthers;
	public AudioClip[] audioClipsDrivingTooSlow;
	public AudioClip[] audioClipsDrivingTooFast;
	public AudioClip[] audioClipsDoingPoorly;
    public AudioClip[] audioClipsDoingWell;
    public AudioClip[] audioClipsRandom;
    public AudioClip[] audioClipsPowerup;
    public AudioClip[] audioClipsLeft;
    public AudioClip[] audioClipsRight;
    public AudioClip[] audioClipsWrongWay;

	
	private AudioSource audioSource;
	private AudioSource audioSourceStartFinish;
	private Race_Manager raceManager;
	private Racer_Register racerRegister;
	private float lastPlayed;
	private int numRacers;
	private int place;
	private bool startPlayed;
	private int thresholdWell;
	private int thresholdPoor;
	private string dir;
	
   
	// Use this for initialization
	void Start () {

        for (int i=0; i < queueCount.Length; i++)
        {
            queueCount[i] = 0;
            isQueue[i] = false;
            audioExists[i] = false;
        }
        isQueue[StatusChange.RANDOM] = true;

        if (GameManager.frequency==-1) { GameManager.frequency = 10; }
        frequency = 60 - (GameManager.frequency * 5);
		audioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		audioSourceStartFinish = (AudioSource)gameObject.AddComponent("AudioSource");
		
		GameObject GameManagerContainerGameObject = GameObject.Find("_RaceManager");
		raceManager = (Race_Manager)GameManagerContainerGameObject.GetComponent(typeof(Race_Manager));
		racerRegister = (Racer_Register)transform.GetComponent(typeof(Racer_Register));
		
		lastPlayed = 0.0f; // First commentary will happen after timeBetweenClips duration after race start
		numRacers = raceManager.RacePlayers;
		place = 0;
		startPlayed = false;
		thresholdPoor = 100;
		thresholdWell = 0;
		
		switch(numRacers)
		{
			case 3:
			case 4:
				thresholdPoor = 3;
				thresholdWell = 1;
				break;
			case 5:
			case 6:
				thresholdPoor = 4;
				thresholdWell = 2;
				break;
			case 7:
			case 8:
				thresholdPoor = 5;
				thresholdWell = 3;
				break;
		}
		
		if (commentaryConfigFile != null)
		{
			LoadCommentaryConfiguration(commentaryConfigFile);
		}
		
	}

    void StartTrigger(int triggerStatus)
    {
        isQueue[triggerStatus] = true;
    }

    //doesn't work
	void OnCollisionEnter() {
        isQueue[StatusChange.COLLISION] = true;
	}
	
	// Update is called once per frame
	void Update () {
		lastPlayed += Time.deltaTime;
		if (place == 0) // RacerStanding is not initialized at the time Start() is run.
		{
			place = racerRegister.RacerStanding;
		}
		else if (place != racerRegister.RacerStanding)
		{
			if (place > racerRegister.RacerStanding)
			{
				isQueue[StatusChange.GOTPASSED] = true;
			}
			if (place < racerRegister.RacerStanding)
			{
				isQueue[StatusChange.PASSEDOTHER] = true;
			}
			place = racerRegister.RacerStanding;
		}
		
		// START works
		if (startPlayed == false && racerRegister.IsRacerStarted)
		{
			audioSourceStartFinish.clip = audioClipsStart[Random.Range(0, audioClipsStart.Length)];
			audioSourceStartFinish.Play();
			lastPlayed = -audioSourceStartFinish.clip.length;
			startPlayed = true;
		}
		
		// FINISH
		if (racerRegister.IsRacerFinished)
		{
			audioSourceStartFinish.clip = audioClipsFinish[Random.Range(0, audioClipsFinish.Length)];
			audioSourceStartFinish.Play();
			lastPlayed = -600.0f;
		}

        // DOING POORLY doesn't work
        if (place >= thresholdPoor) { isQueue[StatusChange.DOINGPOORLY] = true; }
    
        // DOING WELL works
        else if (place <= thresholdWell) { isQueue[StatusChange.DOINGWELL] = false; }

        // DRIVING TOO SLOW works
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6 < 50) { isQueue[StatusChange.TOOSLOW] = true; }

        // DRIVING TOO FAST doesn't work
        else if (gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f > 120) { isQueue[StatusChange.TOOFAST] = true; }

    	if (lastPlayed > frequency)
		{

			audioSource.clip = null;

            int playCount = 5000;
            int playNum = -1;
        
            for (int i = 0; i < StatusChange.LENGTH; i++)
            {
                if (audioExists[i] == false || isQueue[i] == false)
                {
                   // Debug.Log("i = " + i + " "+ audioExists[i]);
                    continue;
                }

              
                if (queueCount[i] < playCount)
                { playNum = i; playCount = queueCount[i]; }
            }//end for loop

            switch (playNum)
            {
                case StatusChange.RANDOM:
                    {
                        audioSource.clip = audioClipsRandom[Random.Range(0, audioClipsRandom.Length)];
                        break;
                    }

                case StatusChange.DOINGPOORLY:
                    {
                        audioSource.clip = audioClipsDoingPoorly[Random.Range(0, audioClipsDoingPoorly.Length)];
                        break;
                    }
                case StatusChange.DOINGWELL:
                    {
                        audioSource.clip = audioClipsDoingWell[Random.Range(0, audioClipsDoingWell.Length)];
                        break;
                    }
                case StatusChange.TOOFAST:
                    {
                        audioSource.clip = audioClipsDrivingTooFast[Random.Range(0, audioClipsDrivingTooFast.Length)];
                        break;
                    }
                case StatusChange.TOOSLOW:
                    {
                        audioSource.clip = audioClipsDrivingTooSlow[Random.Range(0, audioClipsDrivingTooSlow.Length)];
                        break;
                    }

                case StatusChange.POWERUP:
                    {
                        audioSource.clip = audioClipsPowerup[Random.Range(0, audioClipsPowerup.Length)];
                        break;
                    }
                case StatusChange.GOTPASSED:
                    {
                        audioSource.clip = audioClipsGettingPassed[Random.Range(0, audioClipsGettingPassed.Length)];
                        break;
                    }
                case StatusChange.PASSEDOTHER:
                    {
                        audioSource.clip = audioClipsPassingOthers[Random.Range(0, audioClipsPassingOthers.Length)];
                        break;
                    }
                case StatusChange.COLLISION:
                    {
                        audioSource.clip = audioClipsCollision[Random.Range(0, audioClipsCollision.Length)];
                        break;
                    }
                case StatusChange.LEFT:
                    {
                        audioSource.clip = audioClipsLeft[Random.Range(0, audioClipsLeft.Length)];
                        break;
                    }
                case StatusChange.RIGHT:
                    {
                        audioSource.clip = audioClipsRight[Random.Range(0, audioClipsRight.Length)];
                        break;
                    }
                case StatusChange.WRONGWAY:
                    {
                        audioSource.clip = audioClipsWrongWay[Random.Range(0, audioClipsWrongWay.Length)];
                        break;
                    }
            }//end switch

            // Play audio clip
            if (audioSource.clip != null)
            {
                audioSource.Play();
                queueCount[playNum]++;
                lastPlayed = -audioSource.clip.length;
            }
		}//end if time 

      

        for (int i = 1; i < StatusChange.LENGTH; i++)
        {
            isQueue[i] = false;
        }

	}
	
	private void LoadCommentaryConfiguration(TextAsset ConfigurationFile)
	{
		try
		{
			string jSonData = ConfigurationFile.text;
			
			if (jSonData != "")
			{
				JSONObject j = new JSONObject(jSonData);
				
				if (j.HasField("commentary_directory"))
				{
					dir = j.GetField("commentary_directory").str;
					
					audioClipsStart = new AudioClip[j.GetField("audio_clips_start").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_start").list.Count; i++)
					{
                        audioClipsStart[i] = Resources.Load(dir + j.GetField("audio_clips_start").list[i].GetField("fileName").str) as AudioClip;
					}
					
					audioClipsFinish = new AudioClip[j.GetField("audio_clips_finish").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_finish").list.Count; i++)
					{
						audioClipsFinish[i] = Resources.Load(dir + j.GetField("audio_clips_finish").list[i].GetField("fileName").str) as AudioClip;
					}
					
					audioClipsCollision = new AudioClip[j.GetField("audio_clips_collision").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_collision").list.Count; i++)
					{
						audioClipsCollision[i] = Resources.Load(dir + j.GetField("audio_clips_collision").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsCollision[0] != null) { audioExists[StatusChange.COLLISION] = true; }
					}
					
					audioClipsGettingPassed = new AudioClip[j.GetField("audio_clips_getting_passed").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_getting_passed").list.Count; i++)
					{
						audioClipsGettingPassed[i] = Resources.Load(dir + j.GetField("audio_clips_getting_passed").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsGettingPassed[0] != null) { audioExists[StatusChange.GOTPASSED] = true; }
					}
					
					audioClipsPassingOthers = new AudioClip[j.GetField("audio_clips_passing_others").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_passing_others").list.Count; i++)
					{
						audioClipsPassingOthers[i] = Resources.Load(dir + j.GetField("audio_clips_passing_others").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsPassingOthers[0] != null) { audioExists[StatusChange.PASSEDOTHER] = true; }
					}
					
					audioClipsDrivingTooSlow = new AudioClip[j.GetField("audio_clips_driving_too_slow").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_driving_too_slow").list.Count; i++)
					{
						audioClipsDrivingTooSlow[i] = Resources.Load(dir + j.GetField("audio_clips_driving_too_slow").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsDrivingTooSlow[0] != null) { audioExists[StatusChange.TOOSLOW] = true; }
					}
					
					audioClipsDrivingTooFast = new AudioClip[j.GetField("audio_clips_driving_too_fast").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_driving_too_fast").list.Count; i++)
					{
						audioClipsDrivingTooFast[i] = Resources.Load(dir + j.GetField("audio_clips_driving_too_fast").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsDrivingTooFast[0] != null) { audioExists[StatusChange.TOOFAST] = true; }
					}
					
					audioClipsDoingPoorly = new AudioClip[j.GetField("audio_clips_doing_poorly").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_doing_poorly").list.Count; i++)
					{
						audioClipsDoingPoorly[i] = Resources.Load(dir + j.GetField("audio_clips_doing_poorly").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsDoingPoorly[0] != null) { audioExists[StatusChange.DOINGPOORLY] = true; }
					}
					
					audioClipsDoingWell = new AudioClip[j.GetField("audio_clips_doing_well").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_doing_well").list.Count; i++)
					{
						audioClipsDoingWell[i] = Resources.Load(dir + j.GetField("audio_clips_doing_well").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsDoingWell[0] != null) { audioExists[StatusChange.DOINGWELL] = true; }
					}


                    audioClipsRandom = new AudioClip[j.GetField("audio_clips_random").list.Count];
					for (int i = 0; i < j.GetField("audio_clips_random").list.Count; i++)
					{
                        audioClipsRandom[i] = Resources.Load(dir + j.GetField("audio_clips_random").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsRandom[0] != null) { audioExists[StatusChange.RANDOM] = true; }
					}

                    audioClipsPowerup = new AudioClip[j.GetField("audio_clips_powerup").list.Count];
                    for (int i = 0; i < j.GetField("audio_clips_powerup").list.Count; i++)
                    {
                        audioClipsPowerup[i] = Resources.Load(dir + j.GetField("audio_clips_powerup").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsPowerup[0] != null) { audioExists[StatusChange.POWERUP] = true; }
                    }

                    audioClipsLeft = new AudioClip[j.GetField("audio_clips_left").list.Count];
                    for (int i = 0; i < j.GetField("audio_clips_left").list.Count; i++)
                    {
                        audioClipsLeft[i] = Resources.Load(dir + j.GetField("audio_clips_left").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsLeft[0] != null) { audioExists[StatusChange.LEFT] = true; }
                    }

                    audioClipsRight = new AudioClip[j.GetField("audio_clips_right").list.Count];
                    for (int i = 0; i < j.GetField("audio_clips_right").list.Count; i++)
                    {
                        audioClipsRight[i] = Resources.Load(dir + j.GetField("audio_clips_right").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsRight[0] != null) { audioExists[StatusChange.RIGHT] = true; }
                    }

                    audioClipsWrongWay = new AudioClip[j.GetField("audio_clips_wrong_way").list.Count];
                    for (int i = 0; i < j.GetField("audio_clips_wrong_way").list.Count; i++)
                    {
                        audioClipsWrongWay[i] = Resources.Load(dir + j.GetField("audio_clips_wrong_way").list[i].GetField("fileName").str) as AudioClip;
                        if (audioClipsWrongWay[0] != null) { audioExists[StatusChange.WRONGWAY] = true; }
                    }
				}
				else
				{
					Debug.LogError("INVALID COMMENTARY CONFIG FILE!\nFile is not properly set up as a commentary config file.");
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError("INVALID COMMENTARY CONFIG FILE!\n" + e.ToString());
			Debug.Break();
		}
	}
}
