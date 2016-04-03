using UnityEngine;
using System.Collections;

public class HUD_health : MonoBehaviour 
{
	public Rect area= new Rect(0,0,20,20);
	public Texture2D[] healthTex= new Texture2D[5];
	Controls myControls;

	void Start()
	{
		myControls= (Controls) transform.GetComponent(typeof(Controls));
	}

public void OnGUI()
	{
		int health= (int) myControls.health;
		if(health<=0)
			return;

		int numberOfObjects= health/ healthTex.Length;
		health%=5;
		Rect draw_area=area;
	
		for(int i=0; i<numberOfObjects; i++)
			{
				GUI.DrawTexture(draw_area,healthTex[4]);
				draw_area.x+=area.width;
			}
		if(health!=0)
		{
			GUI.DrawTexture(draw_area,healthTex[health-1]);
		}
	

	}

}
