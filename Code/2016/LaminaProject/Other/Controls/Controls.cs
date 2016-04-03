using UnityEngine;
using System.Collections;
using InControl;

[System.Serializable]
public class Controls : PlayerActionSet 
{

    public PlayerAction left;
    public PlayerAction right;
    public PlayerAction up;
    public PlayerAction down;
    public PlayerTwoAxisAction move;

  
    public PlayerAction attack;//r2
    public PlayerAction block;//l2
    public PlayerAction magic1;//l1
    public PlayerAction magic2;//r1

    public PlayerAction jump;//x button
    public PlayerAction interact;//circle
    public PlayerAction pickupNthrow;//square
    public PlayerAction technique;//triangle


    public PlayerAction start;
    public PlayerAction select;
    //Player switchCharacters with dpad


public Controls()
{
    left = CreatePlayerAction("left");
    right = CreatePlayerAction("right");
    up = CreatePlayerAction("up");    
    down = CreatePlayerAction("down");
    move = CreateTwoAxisPlayerAction( left, right, down, up );        

    attack = CreatePlayerAction("attack");        
    block = CreatePlayerAction("block"); 

    magic1 = CreatePlayerAction("magic1");        
    magic2 = CreatePlayerAction("magic2");

    pickupNthrow = CreatePlayerAction("pickupNthrow");
    jump = CreatePlayerAction("jump");
    interact = CreatePlayerAction("interact");
    technique= CreatePlayerAction("technique");

    start= CreatePlayerAction("start");
    select = CreatePlayerAction("select");



    }

	public static Controls CreateWithKeyboardBindings()
	{
		Controls newControls = new Controls();
    /*
		newControls.left.AddDefaultBinding(Key.LeftArrow);
		newControls.right.AddDefaultBinding(Key.RightArrow);
		newControls.up.AddDefaultBinding(Key.UpArrow);
		newControls.down.AddDefaultBinding(Key.DownArrow);

		newControls.left.AddDefaultBinding(Key.A);
		newControls.right.AddDefaultBinding(Key.D);
		newControls.up.AddDefaultBinding(Key.W);
		newControls.down.AddDefaultBinding(Key.S);
		/*
		newControls.attack1.AddDefaultBinding(Key.R);
		newControls.attack2.AddDefaultBinding(Key.T);
		newControls.attack3.AddDefaultBinding(Key.Y);
		newControls.attack4.AddDefaultBinding(Key.U);
		*/

    /*
		newControls.interact.AddDefaultBinding(Key.Space);
		newControls.use.AddDefaultBinding(Key.Z);
		newControls.pickupNthrow.AddDefaultBinding(Key.E);
		
		newControls.start.AddDefaultBinding(Key.PadEnter);
		*/
		return newControls;

	}

    public static Controls CreateWithJoystickBindings()
    {
        Controls newControls= new Controls();
    
        //analogs
        newControls.left.AddDefaultBinding(InputControlType.LeftStickLeft);
        newControls.right.AddDefaultBinding(InputControlType.LeftStickRight);
        newControls.up.AddDefaultBinding(InputControlType.LeftStickUp);
        newControls.down.AddDefaultBinding(InputControlType.LeftStickDown);

        //bumbers
        newControls.magic1.AddDefaultBinding(InputControlType.LeftBumper );
        newControls.magic2.AddDefaultBinding(InputControlType.RightBumper);

        //triggers
        newControls.block.AddDefaultBinding(InputControlType.LeftTrigger);
        newControls.attack.AddDefaultBinding(InputControlType.RightTrigger);
        
        //4 buttons on the right
        newControls.jump.AddDefaultBinding(InputControlType.Action1);
        newControls.interact.AddDefaultBinding(InputControlType.Action2);
        newControls.pickupNthrow.AddDefaultBinding(InputControlType.Action3);
        newControls.technique.AddDefaultBinding(InputControlType.Action4);

        //pause
        newControls.start.AddDefaultBinding(InputControlType.Start);
        newControls.start.AddDefaultBinding(InputControlType.Pause);
        newControls.start.AddDefaultBinding(InputControlType.Menu);
        newControls.start.AddDefaultBinding(InputControlType.Options);

        //select
    newControls.select.AddDefaultBinding(InputControlType.Back);
    newControls.select.AddDefaultBinding(InputControlType.Select);


        return newControls;
    }
}
