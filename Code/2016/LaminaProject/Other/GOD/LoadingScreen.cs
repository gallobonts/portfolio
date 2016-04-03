using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

public GameObject loadingPanel;
public GameObject text;
public GameObject progressBar;

int loadProgress= 0;

void Awake()
{
	loadingPanel.SetActive(false);
	
}

public void LoadLevel(string levelToLoad)
{
	this.gameObject.GetComponent<LoadingScreen>().StartCoroutine(DisplayLoadingScreen(levelToLoad));
}


IEnumerator DisplayLoadingScreen(string level)
{

	loadingPanel.SetActive(true);

	text.GetComponent<Text>().text= "Loading Progress " +loadProgress+"%";
	progressBar.transform.localScale= new Vector3(0.0f,progressBar.transform.localScale.y,progressBar.transform.localScale.z);


	AsyncOperation async = Application.LoadLevelAsync(level);//creates a background thread, loading the level
	while(!async.isDone)
	{
		loadProgress= (int)(async.progress*100);
	
		text.GetComponent<Text>().text= "Loading Progress " +loadProgress+"%";
		progressBar.transform.localScale= new Vector3(async.progress,progressBar.transform.localScale.y, progressBar.transform.localScale.z);

		yield return null;//gives control away at the end of the while loop, allowing the application to continue loading the level

	}

	loadingPanel.SetActive(false);
	

}



}
