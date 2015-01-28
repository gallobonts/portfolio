using UnityEngine;
using System.Collections;

public class Axe : Tool 
{
	new void Use(Vector2 dirFacing)
	{
		base.Use (dirFacing);
	}
}
