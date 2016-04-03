using UnityEngine;
using System.Collections;

public class YtoZScale : MonoBehaviour 
{
  public bool debug=false;
  Transform myTransform;
  static float minZ=0;
  static float maxZ=20;
  static float maxY=200;
  float minY;
  float midZ;

	void Awake () 
  {
    myTransform = this.transform;
    minY = -1 * maxY;
    midZ = ((maxZ - minZ)/2) + minZ;

    if (midZ == 0)
  {
    midZ = 0.01f;
  }

	}
	
	void Update () 
  {
    float newZ = midZ;//start at the middle
    float checkY = myTransform.position.y;//check where y is

    if (checkY < 0)//if we are in the negative area, value returned should be between Zmin & zMid
  {
      newZ-= (checkY/minY)*midZ;
  }
    else if (checkY > 0)
  {
      newZ+= (checkY/maxY)*midZ;
  }
      Vector3 newPosition = myTransform.position;
      newPosition.z = newZ;
      myTransform.position = newPosition;

    if (debug)
  {
      Debug.Log(gameObject.name+ " y = "+checkY+ " minY= " + minY + " midz= " +midZ+ " newz = " + newZ);
  }
	}
}
