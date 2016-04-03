using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;


public class GUIMenuBase : MonoBehaviour 
{
	public GameObject mainMenuPanel;

  protected int playerNum;

	protected string currentFlow="MainMenu";
	protected Stack<string> lastFlow=new Stack<string>();
	TwoAxisInputControl filteredDirection= new TwoAxisInputControl();


  //[HideInInspector]
	public Button focusedButton;
	InputDevice inputDevice;
 	Controls myControls;
  float sensitivityLevel=.015f;

  bool mylock=false;

void Awake()
{
	//filteredDirection = new TwoAxisInputControl();
	//filteredDirection.StateThreshold = 0.5f;
	lastFlow.Push("MainMenu");
}

void Update()
		{
    if(mylock){return;}
		if(inputDevice==null ||focusedButton==null)
		{return;}

    if (myControls.interact.WasPressed)
  {
      ChangeFlow("Back");
  }
		filteredDirection.Filter( inputDevice.Direction, Time.deltaTime );

		/*
		if (filteredDirection.Left.WasRepeated)
		{
			Debug.Log( "!!!" );
		}
		*/

		// Move focus with directional inputs.
		//if (myControls.move.Up.WasPressed)
 
   // if (myControls.move.Left.WasPressed)

    if (Mathf.Abs(filteredDirection.X) > Mathf.Abs(filteredDirection.Y))//if left & right more pressedthan up & down
  {
    //if (filteredDirection.Left.WasPressed)
      if(filteredDirection.Left.WasPressed && filteredDirection.X < -sensitivityLevel)
    {
      if (focusedButton.navigation.selectOnLeft == null)
      {
//        Debug.Log("left failed");
        return;
      }
      SetFocus(focusedButton.navigation.selectOnLeft.GetComponent<Button>());
        mylock=false;
        StartCoroutine(WaitToUnlock());
      return;
    }
		
    //if (myControls.move.Right.WasPressed)
    //if (filteredDirection.Right.WasPressed) 
      if(filteredDirection.Right.WasPressed && filteredDirection.X > sensitivityLevel)
    {
      if (focusedButton.navigation.selectOnRight == null)
      {return;}
      SetFocus(focusedButton.navigation.selectOnRight.GetComponent<Button>());
        mylock=false;
        StartCoroutine(WaitToUnlock());
        return;
    }
  }//if x is more than y
    else
    {
  //   if(filteredDirection.Up.WasPressed)  
      if(filteredDirection.Up.WasPressed && filteredDirection.Y > sensitivityLevel)
    {
        Debug.Log("filtered direction.y= "+filteredDirection.Y);

      if(focusedButton.navigation.selectOnUp==null)
      {return;}
      SetFocus(focusedButton.navigation.selectOnUp.GetComponent<Button>());
        mylock=false;
        StartCoroutine(WaitToUnlock());

        return;
    }
    
    //if (myControls.move.Down.WasPressed)
     //if(filteredDirection.Down.WasPressed)
      if(filteredDirection.Down.WasPressed && filteredDirection.Y < -sensitivityLevel)
    {
        Debug.Log("filtered direction.y= "+filteredDirection.Y);
      if(focusedButton.navigation.selectOnDown==null)
      {return;}
      SetFocus(focusedButton.navigation.selectOnDown.GetComponent<Button>());
        mylock=false;
        StartCoroutine(WaitToUnlock());

        return;
    }
    }//else y>x

		if(myControls.jump.WasPressed)
		{
			focusedButton.onClick.Invoke();
		}
		
	}


public void SetDevice(int newPlayerNum, InputDevice newDevice, Controls newControls)
	{
    playerNum = newPlayerNum;
		inputDevice=newDevice;
		myControls = newControls;
 	}

	protected void Back()
	{
    Debug.Log("Back! " + lastFlow.Count);
		if (lastFlow.Count > 1)
  {
    lastFlow.Pop();
    string pop = lastFlow.Pop();
    ChangeFlow(pop);
			
  }
  else
  {
      StartCoroutine(QuitPanels());

  }

	}
	

	protected void SetFocus(Button newFocus)
	{
		if (newFocus != null)
		{
			focusedButton = newFocus;
			newFocus.Select();
		}
	}

	

	virtual public void ResetPanels(){}
  virtual public IEnumerator QuitPanels(){return null;}

  IEnumerator WaitToUnlock()
  {
    //wait 3 frames
    yield return new WaitForSeconds(.1f);
   
    mylock = false;

  }

	virtual public void ChangeFlow(string newFlow)
	{
		ResetPanels();

		if(newFlow=="Back")
		{return;}
		
		currentFlow=newFlow;
		lastFlow.Push(currentFlow);
	}


}
