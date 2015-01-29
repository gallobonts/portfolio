/*
This is the main menu
*/
var button_width:float=125;
var button_height:float=50;
var rect_area:Rect= Rect(Screen.width+button_width/2+button_width,Screen.height/2-button_height,button_width,button_height);

function OnGUI()
{
GUILayout.BeginArea(rect_area);
GUILayout.BeginVertical("box");

if( GUILayout.Button("Start Game"))
{
	Application.LoadLevel("Island");
}
GUILayout.EndVertical();
GUILayout.EndArea();


}