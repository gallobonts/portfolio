using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD_equipment : MonoBehaviour 
{
	public Rect[] WeaponArea= new Rect[4];
	public string [] myWeaponNames= new string[4];
	public Texture2D[] weaponTex=new Texture2D[4];

	public Rect toolArea;
	public Texture2D toolTex;

	public Rect genieArea;
	public Texture2D genieTex;

	Controls myControls;
	void OnEnable()
	{
		myControls= (Controls) transform.GetComponent(typeof(Controls));
		GameObject[] myWeapons =myControls.myWeapons;
	

		for(int i=0; i<4;i++)
		{
			myWeaponNames[i]= myWeapons[i].name;//get name of weapon
			myWeaponNames[i]=myWeaponNames[i].Remove(myWeaponNames[i].Length-7);//remove clone from name
			myWeaponNames[i]+="Icon";//add icon
			weaponTex[i]=(Texture2D)Resources.Load("WeaponIcons/"+myWeaponNames[i]);
		}

		toolTex=(Texture2D)Resources.Load("ToolIcons/ToolIcon");
		genieTex= (Texture2D)Resources.Load("GenieIcons/GenieIcon");

	}

	public  void UpdateWeapons()
	{
		myControls= (Controls) transform.GetComponent(typeof(Controls));
		GameObject[] myWeapons =myControls.myWeapons;
		
		
		for(int i=0; i<4;i++)
		{
			myWeaponNames[i]= myWeapons[i].name;//get name of weapon
			myWeaponNames[i]=myWeaponNames[i].Remove(myWeaponNames[i].Length-7);//remove clone from name
			myWeaponNames[i]+="Icon";//add icon
			weaponTex[i]=(Texture2D)Resources.Load("WeaponIcons/"+myWeaponNames[i]);
		}
	}

	public void UpdateTools()
	{
		if(myControls.myTool!=null)
		{
		string myToolName= myControls.myTool.name;
		myToolName=myToolName.Remove(myToolName.Length-7);
		myToolName+="Icon";
		toolTex=(Texture2D)Resources.Load("ToolIcons/"+myToolName);
		}

	}

	public void OnGUI()
	{

		for(int i=0; i<4;i++)
		{
			GUI.DrawTexture(WeaponArea[i],weaponTex[i]);
		}
		GUI.DrawTexture(toolArea,toolTex);
		GUI.DrawTexture(genieArea,genieTex);
		
	}
	
}
