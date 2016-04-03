using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class PlayMirok : MonoBehaviour
{
  public MovieTexture myMovie;
  private STAudioSource myAudio;
  RawImage myRawImage;
	
  public GameObject graveCanvases;
  public GameObject mirokcanvas;

  bool isPlaying=false;

	void Awake()
  {
    myRawImage = GetComponent<RawImage>();
    myAudio = GetComponent<STAudioSource>();
  }


  public void Play(MovieTexture newMovie)
  {
    graveCanvases.SetActive(false);
    mirokcanvas.SetActive(true);

    myMovie = newMovie;
    myRawImage.texture = myMovie as MovieTexture;
    myAudio.clip = myMovie.audioClip;

    StartMovie();

  }
  void Update()
  {
    if(!isPlaying){return;}
    //if either the movie is over or the player has quit
    if (!myMovie.isPlaying)
  {
      Priorities.education=125;
      StopMovie();

  }
      if(Input.GetKeyDown(KeyCode.Space))
  {
      Priorities.education=125;

      StopMovie();

  }
 	
  }

  void StartMovie()
  {
    myMovie.Stop();//make sure it's rewound
    myMovie.Play();
    myAudio.Play();
    isPlaying = true;

    Priorities.education = 100;
  }
  void StopMovie()
  {
    graveCanvases.SetActive(true);
    mirokcanvas.SetActive(false);
    myMovie.Pause();
    myAudio.Pause();
    isPlaying = false;

  }
}
