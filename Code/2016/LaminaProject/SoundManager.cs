using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour 
{

  public AudioMixerSnapshot wanderSnapShot;
  public AudioMixerSnapshot poiSnapshot;


  public List<AudioClip> transitionSFX= new List<AudioClip>();
  public List<AudioClip> wanderClips= new List<AudioClip>();
  AudioClip poiClip;

  public float bpm=128;//beats per minute of tempo, good for fading


  public STAudioSource wanderSource;
  public STAudioSource poiSurce;
  public STAudioSource transitionSource;

  private float transitionInTime;
  private float transitionOutTime;

  private float quarterNote;//?


  public static SoundManager instance= null;

  public bool insidePOI=false;

  GameObject myGameObject;

  void Awake()
  {
    if(instance==null)
    {instance=this;}
    else if(instance!=this)
    {Destroy(gameObject);}

    DontDestroyOnLoad(gameObject);
    myGameObject = this.gameObject;

  }


  void Start()
  {
    quarterNote = 60 / bpm; //gives you the length of the quarternote in seconds
    transitionInTime = quarterNote*8;
    transitionOutTime = quarterNote * 16; //?

    int i = Random.Range(0, wanderClips.Count);
    wanderSource.clip = wanderClips [i];

    poiSurce.clip = wanderClips [i];

    poiSurce.Play();
    wanderSource.Play();

  }

  public void TransitionIn(AudioClip clip)
  {
    insidePOI = true;
    poiClip = clip;
    
    poiSnapshot.TransitionTo(transitionInTime);//transitions to in poi snap shot in 'transition in time'
    
    poiSurce.clip = poiClip;

    PlayTransition();
    poiSurce.Play();
    wanderSource.Play();
    
  }

  public void PlayTransition()
  {
    int i = Random.Range(0, transitionSFX.Count);
 
    transitionSource.clip = transitionSFX [i];
    transitionSource.Play();

  }
  public void TransitionOut()
  {
    
    insidePOI = false;

    int i = Random.Range(0, wanderClips.Count);
    wanderSource.clip = wanderClips [i];

    
    PlayTransition();
    poiSurce.Play();
    wanderSource.Play();

    wanderSnapShot.TransitionTo(transitionOutTime);//transitions to in poi snap shot in 'transition in time'

  }

 



}
