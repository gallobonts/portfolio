using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum MirokType
{
  AUDIO,VIDEO
}

[System.Serializable]
public class MirokButtonInfo 
{
  public Button button;
  [HideInInspector]
  public Text text;
  public MirokType type;
  public AudioClip myAudio;
  public MovieTexture myMovie;
  public Button.ButtonClickedEvent thingToDo;
  public LevelManager myLevelManager;
}

public class MirokButton:MonoBehaviour
{
  public MirokButtonInfo myButtonInfo;

	
  public void Start()
  {
    myButtonInfo.button.onClick = myButtonInfo.thingToDo;

  }
  public void PlayMyMovie()
  {
    myButtonInfo.myLevelManager.PlayMirok(myButtonInfo.myMovie);
  }
}
