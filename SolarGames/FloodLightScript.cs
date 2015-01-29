/*
    attaches to a trigger volume
    Determines if the player is in sunlight and should start charging it's battery
*/
using UnityEngine;
using System.Collections;

public class FloodLightScript : MonoBehaviour {

    BatteryHUD myhud;
    public float floodLightIntensity = 10;

    void Start()
    {
        myhud = GameObject.Find("EnergyUI").GetComponent<BatteryHUD>();
    }

    //you are in light, charge battery
    void OnTriggerEnter(Collider other) 
    {
        if (other.transform.root.name != "Buggy_Tier_2(Clone)")
         { return; }

        myhud.floodLightIntensity = floodLightIntensity;
        Debug.Log("flood light enter");

    }

    //you have left light, stop charging battery
    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.name != "Buggy_Tier_2(Clone)")
        { return; }

        myhud.floodLightIntensity = 0.0f;
        Debug.Log("flood light exit");

    }
}
