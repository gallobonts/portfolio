using UnityEngine;
using System.Collections;

public class Dialouge : MonoBehaviour 
{
	public bool playDialogue=false;

    public Rect textArea = new Rect(0, 200, Screen.width, Screen.height - 200);
	public string[] text;
    [HideInInspector]
	public int frameCount=0;//prevents playing the dialogue the same frame it quits
	public int dialogePadding=20;
	public Texture background;
    public int fontsize = 20;

    private int stepCount = 0;

	GUIStyle dialogueStyle= new GUIStyle();

	void Start()
	{
        dialogueStyle.fontSize = fontsize;
		dialogueStyle.padding= new RectOffset(dialogePadding,dialogePadding,dialogePadding,dialogePadding);
	}

    void Update()
    {
        if (frameCount < Time.frameCount || !playDialogue)
        { return; }

        if (Input.GetButtonDown("X") || Input.GetKeyDown("space"))
        {
            stepCount++;
            if (text.Length <= stepCount)
            {
                playDialogue = false;
            }
        }
    }
	void OnGUI()
	{
		if(!playDialogue)
		{return;}
		Time.timeScale=0;
		GUI.DrawTexture(textArea,background,ScaleMode.StretchToFill);

	

		GUI.Label(textArea,text[stepCount],dialogueStyle);


		if(Input.GetButtonDown("X")||Input.GetKeyDown("space"))
		{
			
			frameCount=Time.frameCount+1;
			Time.timeScale=1;
		}
	}


}
