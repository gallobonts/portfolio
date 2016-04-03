using UnityEngine;
using System.Collections;


[System.Serializable]
public class SFX_Info : MonoBehaviour //custom info for each sfx
{

  public AudioClip clip;

  public float volLowRange= .75f;//limited 0->1
  public float volHighRange= 1.0f;
  
  public float lowPitchRange=.95f;//limited 0->2.5?
  public float highPitchRange=1.05f;


}
