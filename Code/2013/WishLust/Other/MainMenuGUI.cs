using UnityEngine;
using System.Collections;

public enum MainMenuState
{
	MAIN,
	PLAYGAME,
    PLAYGAME_PLAYERCOUNT
};

public class MainMenuGUI : MonoBehaviour {
	MainMenuState mainMenuState;
	void Start()
	{
		mainMenuState=MainMenuState.MAIN;

	}

	void OnGUI()
	{
		switch(mainMenuState)
		{
		case MainMenuState.MAIN:
		{
			if(GUI.Button(new Rect(10,10,100,50),"Play Game"))
			{mainMenuState=MainMenuState.PLAYGAME;}
			break;
		}
		case MainMenuState.PLAYGAME:
		{
			if(GUI.Button (new Rect(10,210,100,50),"Load Game"))
				{God.god.LoadGame();}
            if (GUI.Button(new Rect(10, 110, 100, 50), "New Game"))
            { mainMenuState = MainMenuState.PLAYGAME_PLAYERCOUNT; }
				break;
		}//end case play game

        case MainMenuState.PLAYGAME_PLAYERCOUNT:
        {
            for (int i = 1; i < 5; i++)
            {
                if(GUI.Button(new Rect(10,110+50*i,100,50),i+" Players"));
                {
                    God.god.NewGame(i);
                }
            }
            break;
        }
		}//end switch
	}
}
