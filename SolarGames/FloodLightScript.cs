using UnityEngine;
using System.Collections;

public class FloodLightScript : MonoBehaviour {

    BatteryHUD myhud;
    public float floodLightIntensity = 10;

    void Start()
    {
        myhud = GameObject.Find("EnergyUI").GetComponent<BatteryHUD>();
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.transform.root.name != "Buggy_Tier_2(Clone)")
         { return; }

        myhud.floodLightIntensity = floodLightIntensity;
        Debug.Log("flood light enter");

    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.name != "Buggy_Tier_2(Clone)")
        { return; }

        myhud.floodLightIntensity = 0.0f;
        Debug.Log("flood light exit");

    }
}
