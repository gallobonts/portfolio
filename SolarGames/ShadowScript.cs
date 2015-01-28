using UnityEngine;
using System.Collections;

public class ShadowScript : MonoBehaviour {

    BatteryHUD myhud;
 
    void Start()
    {
        myhud = GameObject.Find("EnergyUI").GetComponent<BatteryHUD>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name != "Buggy_Tier_2(Clone)")
        { return; }

        myhud.inShade = true;
        Debug.Log("shadow enter");

    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.name != "Buggy_Tier_2(Clone)")
        { return; }

        myhud.inShade = false;
        Debug.Log("shadow exit");

    }
}
