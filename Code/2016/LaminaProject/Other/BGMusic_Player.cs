using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class BGMusic_Player : MonoBehaviour 
{
  public List<AudioClip> bgAudioClips= new List<AudioClip>();
 


  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
  {

       int i= Random.Range(0,bgAudioClips.Count);

      SoundManager.instance.TransitionIn(bgAudioClips[i]);
  }

  }

  void OnTriggerExit2D(Collider2D other)
  {
    SoundManager.instance.TransitionOut();
    
  }

}
