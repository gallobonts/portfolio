using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraveSoundManager : MonoBehaviour {

  public List<AudioClip> bgAudioClips= new List<AudioClip>();
  public STAudioSource audioSource;

  public void Start()
  {
    int i = Random.Range(0, bgAudioClips.Count);
    audioSource.clip = bgAudioClips [i];
   
    audioSource.Play();
  }
}
