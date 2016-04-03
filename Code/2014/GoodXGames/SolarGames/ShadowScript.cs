/*
    attached to a trigger volume
    triggers 'in shade' and prevents the battery from charging
*/
using UnityEngine;
using System.Collections;

public class ShadowScript : MonoBehaviour {

BatteryHUD myhud;

void Start()
{
    myhud = GameObject.Find("EnergyUI").GetComponent<BatteryHUD>();
}

//is in shade, thus no charge
void OnTriggerEnter(Collider other)
{
    if (other.transform.root.name != "Buggy_Tier_2(Clone)")
    { return; }

    myhud.inShade = true;
    Debug.Log("shadow enter");

}

//is not in shade, thus charge
void OnTriggerExit(Collider other)
{
    if (other.transform.root.name != "Buggy_Tier_2(Clone)")
    { return; }

    myhud.inShade = false;
    Debug.Log("shadow exit");

}

}
