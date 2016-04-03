/*
	attaches to player, determines what happens when the player is hit by knockout
*/
using UnityEngine;
using System.Collections;

public class KnockOutHit : MonoBehaviour 
{
int checkID;

void Start()
{
	ID_Manager ID_Script = (ID_Manager) this.transform.root.GetComponent("ID_Manager");
	checkID = ID_Script.ID;
}


void OnTriggerEnter(Collider other) 
{
	//make it's the player being hit, and not their weapon or a projectile
	PowerUpHit tempPowerUpHit= other.transform.root.gameObject.GetComponent<PowerUpHit>();
	if(!tempPowerUpHit){return; }

	//prevent you from hitting yourself with the ability 
	ID_Manager tempID_Manager = (ID_Manager) other.transform.root.GetComponent("ID_Manager");
	if(tempID_Manager.ID == checkID){return;}

	//general hit causes player to flip and halt movement for a moment
	tempPowerUpHit.GeneraliHit();
}

}