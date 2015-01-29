/*
	the knockout powerup
*/
using UnityEngine;
using System.Collections;

public class KnockOut : PowerUpBase {

Quaternion rotateEnd;
Transform pivotPoint;

GameObject wep2;
Transform mesh2;
Quaternion rotateBegin2;
Quaternion rotateEnd2;
Vector3 startPosition2;
Transform pivotPoint2;

Collider col;
Collider col2;

int maxSwings=10;
int swingCount=0;
public float animationSpeed=200;
    



void Start()
{

	//find the socket and attach the model
	Transform tempTransform = this.transform.root.Find("Frame1/CatapultAttachJoint"); //to hold GO with CatapultScript
	wep = (GameObject)Instantiate((GameObject)Resources.Load("Hand"),tempTransform.position,tempTransform.rotation);
	wep.transform.root.parent = tempTransform;

	//mirror the first hand
	wep2 = (GameObject)Instantiate((GameObject)Resources.Load("Hand"), tempTransform.position, tempTransform.rotation);
	wep2.transform.root.parent = tempTransform;

	//animation coordinates 
	startPosition = new Vector3(1.6f, .5f, 0f);
	mesh = wep.transform;
	mesh.localPosition = startPosition;
	pivotPoint = mesh.transform.FindChild("PivotPoint");
	rotateBegin = Quaternion.Euler(new Vector3(0, 270, 0));
	mesh.localRotation = rotateBegin;
	rotateEnd = Quaternion.Euler(new Vector3(0, 350, 0));

	//send hand's animation coordinates
	startPosition2 = new Vector3(-3f, .5f, 0f);
	mesh2 = wep2.transform;
	mesh2.localPosition = startPosition2;
	mesh2.localScale = new Vector3(mesh2.localScale.x * -1, mesh2.localScale.y, mesh2.localScale.z);
	pivotPoint2 = mesh2.transform.FindChild("PivotPoint");
	rotateBegin2 = Quaternion.Euler(new Vector3(0, 90, 0));
	mesh2.localRotation = rotateBegin2;
	rotateEnd2 = Quaternion.Euler( new Vector3(0, 180, 0));


	//set up colliders
	col = mesh.GetComponent<BoxCollider>();
	col2 = mesh2.GetComponent<BoxCollider>();

	//make collider false while powerup is inactive (else the colliders end up messing up the car's movement)
	wep.SetActive(false);
	wep2.SetActive(false);
	col.enabled = false;
	col2.enabled = false;
	rootName = this.transform.root.name;

}

		// Update is called once per frame
void FixedUpdate () 
{
    if (!attached)
    { return; }

    if (rootName == playerRootName && (IsPressed || Input.GetKey(KeyCode.E)) && attached)
    { StartAnimation = true; col.enabled = true; col2.enabled = true; IsPressed = false;
   
    }


    else if (rootName != playerRootName && aiUse && wep.activeSelf)
    {
        StartAnimation = true;
        
    }

    aiUse = false;
    Animate();



}

void Animate()
{

	if (!StartAnimation) {return;}
	if(swingCount >= maxSwings)
	{
		Detach();
		return;
	}

	if (!ReturnToStarting)
	{
		//start the animation going forward
		mesh.RotateAround(pivotPoint.position,new Vector3(0,90,0),(animationSpeed*Time.deltaTime));
		mesh2.RotateAround(pivotPoint2.position,new Vector3(0, 90, 0),(-animationSpeed * Time.deltaTime));


		if(mesh.localRotation.eulerAngles.y >= rotateEnd.eulerAngles.y)
		{
			//change direction
			ReturnToStarting = true;
		}
	} 
	else 
	{
	//start the animation going backwards
		mesh.RotateAround(pivotPoint.position,new Vector3(0,90,0),(-animationSpeed*Time.deltaTime));
		mesh2.RotateAround(pivotPoint2.position,new Vector3(0,90,0),(animationSpeed*Time.deltaTime));

		//change direction
		if(mesh.localRotation.eulerAngles.y <= rotateBegin.eulerAngles.y)
		{
		swingCount++;
		ReturnToStarting = false;
			mesh.localPosition = startPosition;
			mesh.localRotation = rotateBegin;

			mesh2.localPosition = startPosition2;
			mesh2.localRotation = rotateBegin2;


		}
	}

}



override public void Attach()
{

	StandardAttach();

	wep2.SetActive(true);
	col.enabled = false;
	col2.enabled = false;
	swingCount = 0;

}

override  public void Detach()
{
	if(GameObject.Find ("KnockOut")!=null)
	{
		GameObject.Find ("KnockOut").SetActive (false);
	}
	    StandardDetach();
	    wep2.SetActive(false);
	}
	
}