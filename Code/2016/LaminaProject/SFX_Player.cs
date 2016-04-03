using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SFX_Player : MonoBehaviour 
{
  public SFX_Info[] sfx;//clips
  STAudioSource sfxSource;

  public int priority=100;//should be a range from 0->?
  public float maxDistance=20f;

  //add any elements as needed, panning/doppler effect ect

 
  public void Awake()
  {
   sfxSource =this.gameObject.AddComponent<STAudioSource>();

    sfxSource.playOnAwake = false;

    sfxSource.priority = priority;
    sfxSource.maxDistance = maxDistance;
  }

  public void Play()
  {
    SFX_Info clipInfo;

    //pick a random sfx
    int randomIndex = Random.Range(0, sfx.Length);
    clipInfo = sfx [randomIndex];


    //randomize the info so it doesn't sound repetitve
    float randomPitch = Random.Range(clipInfo.lowPitchRange, clipInfo.highPitchRange);
    float randomVol = Random.Range(clipInfo.volLowRange, clipInfo.volHighRange);

    //set the info
    sfxSource.clip = clipInfo.clip;
    sfxSource.pitch = randomPitch;
    sfxSource.volume = randomVol;

    //play the info
    sfxSource.Play();
}
	
  /* interesting ideas
   * 
   * set volume= relative velocity.magnityde * volmultiplier
   * 
   * if( velocity < threshhold)
   * play soft crash
   * else play hard crash
   * 
   * 
   * 
   * 
   */
}
