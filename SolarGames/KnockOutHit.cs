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
	PowerUpHit tempPowerUpHit= other.transform.root.gameObject.GetComponent<PowerUpHit>();
	if(!tempPowerUpHit){return; }

	ID_Manager tempID_Manager = (ID_Manager) other.transform.root.GetComponent("ID_Manager");
	if(tempID_Manager.ID == checkID){return;}

tempPowerUpHit.GeneraliHit();
}

}