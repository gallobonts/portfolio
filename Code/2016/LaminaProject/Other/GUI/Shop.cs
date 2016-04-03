using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour 
{
	bool player1InShop=false;
	bool player2InShop=false;
//	Controls player1Controls= new Controls();
//	Controls player2Controls= new Controls();

	void Update()
	{
		if(player1InShop)
		{
	//		if(Input.GetButtonDown(player1Controls.pickUp_n_throwButton))
			{
				DeactivateShop(1);
			}
		}

		if(player2InShop)
		{	
//			if(Input.GetButtonDown(player2Controls.pickUp_n_throwButton))
			{
				DeactivateShop(2);
			}
		}

	}
	public void ActivateShop(int playerNum,Controls playerControls)
	{
		/*
		Debug.Log("Shop Activated");
		if(playerNum==1)
		{player1InShop=true;player1Controls=playerControls;}
		else
		{player2InShop=true;player2Controls=playerControls;}
		*/
	}

	void DeactivateShop(int playerNum)
	{
		Debug.Log("Shop Deactivated");
		if(playerNum==1)
		{player1InShop= false;}
		else
		{player2InShop =false;}
		
	}

}
