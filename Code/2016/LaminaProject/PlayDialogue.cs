using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayDialogue : MonoBehaviour 
{
  //refrences
  public  IslandLevelManager myLevelManager;

  public GameObject MissionCanvas;
  public Text missionText;

  private Dialogue myDialogue;
  private STAudioSource myAudio;

  bool isPlaying=false;
  bool hasVoice=false;

  float maxPlayTime=5f;
  float currentPlayTime;

  public void Play(Dialogue newDialogue)
  {
    MissionCanvas.SetActive(true);
    if(isPlaying)//if something is already playing, play a new one
    {StopDialogue();}

    myDialogue = newDialogue;

    missionText.text = myDialogue.text;
    myAudio = myDialogue.audio;

    if (myAudio == null)
  {
    hasVoice = false;
    currentPlayTime=maxPlayTime;
  }
  else
  {
      hasVoice=true;
  }

    StartDialogue();
  }
  void Update()
  {
    if(!isPlaying){return;}

    if (hasVoice)//code for files with audio
  {
    if (!myAudio.isPlaying)
    {
      StopDialogue();
      MissionCanvas.SetActive(false);
    }
  }//has voice

    else
  {
      currentPlayTime-=Time.deltaTime;
        if(currentPlayTime<=0)
      {
        StopDialogue();
        MissionCanvas.SetActive(false);
      }
   }//else no voice

  }


  void StartDialogue()
  {
    isPlaying = true;
    if (hasVoice)
  {
    myAudio.Play();
  }

  }

  void StopDialogue()
  {
    isPlaying = false;

    if (myDialogue.missionUnlock != null)
    {
      myLevelManager.myMissionManager.UnlockMission(myDialogue.missionUnlock);
    }

  }

}
