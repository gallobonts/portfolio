/*
    Sun throw powerup
*/
using UnityEngine;
using System.Collections;


public class SunThrow : PowerUpBase
{ 
//animation coordinates
Quaternion rotateMiddle;
Quaternion rotateEnd;
Transform pivotPoint;

int animationStage = 0;
public float animationSpeed = 100;

//the location the sun starts
Transform SunPosition;

//holds the sun to be thrown
public GameObject mySun;

//prevents from hitting self
int checkID;

void Start()
{
    //find self
    ID_Manager ID_Script = (ID_Manager)this.transform.root.GetComponent("ID_Manager");
    checkID = ID_Script.ID;

    //find socket, and create weapon
    Transform tempTransform = this.transform.root.Find("Frame1/CatapultAttachJoint"); //to hold GO with CatapultScript
    wep = (GameObject)Instantiate((GameObject)Resources.Load("SunThrow"), tempTransform.position, tempTransform.rotation);
    wep.transform.root.parent = tempTransform;

    //find pivot points, and starting positions
    startPosition = new Vector3(-.35f, .19f, 1.32f);
    mesh = wep.transform;
    mesh.localPosition = startPosition;
    pivotPoint = mesh.transform.FindChild("PivotPoint");
    SunPosition = mesh.transform.FindChild("SunPosition");
   
    rotateBegin = Quaternion.Euler(new Vector3(57, 340, 50));

    //set initial rotations
    mesh.localRotation = rotateBegin;
    rotateMiddle = Quaternion.Euler(new Vector3(333, 355, 80));
    rotateEnd = Quaternion.Euler(new Vector3(81, 150, 73));


    wep.SetActive(false);
    rootName = this.transform.root.name;
}

void FixedUpdate()
{
    if (!attached)
    { return; }
    //starts animation when the player attacks
    if (rootName == playerRootName && (IsPressed || Input.GetKey(KeyCode.E)) && attached)
    {StartAnimation = true ; IsPressed = false;}

    else if (rootName != playerRootName && aiUse && wep.activeSelf)
    { StartAnimation = !StartAnimation; mesh.localRotation = rotateBegin; }

    aiUse = false;
    Animate();
}

void Animate()
{
   
    if (!StartAnimation) { return; }
    if (animationStage == 0)
    {
        //move the catapult backward, in a rotation such as one would prepare to throw a baseball
        mesh.RotateAround(pivotPoint.position, transform.right, -(animationSpeed * Time.deltaTime));

        if (mesh.localRotation.eulerAngles.z >= rotateMiddle.eulerAngles.z)
        {
            animationStage = 1;
        }
    }
    else if (animationStage == 1)
    {
        //reverse the animation, actually throwing the sun 
        mesh.RotateAround(pivotPoint.position, transform.right, (animationSpeed * Time.deltaTime));
        if (mesh.localRotation.eulerAngles.z <= rotateEnd.eulerAngles.z)
        {
            animationStage = 2;
        }
    }
    else
    {
        Fire();
        Detach();

    }

}

void Fire()
{
    //actually throw the sun   
    GameObject mySunProjectile = (GameObject) Instantiate(mySun, SunPosition.position,this.transform.rotation);
    SunThrowProjectile tempscript = mySunProjectile.GetComponent<SunThrowProjectile>();
    tempscript.ids.Add(checkID);

} 

public override void Attach()
{
    StandardAttach();
    animationStage = 0;
}

public override void Detach()
{
	if(GameObject.Find ("SunThrow")!=null)
	{
		GameObject.Find ("SunThrow").SetActive (false);
	}

	StandardDetach();
}
}