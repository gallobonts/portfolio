using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SunThrowProjectile : MonoBehaviour {

    public float scaleIncrease = .5f;
    public float speed=.8f;
    public float maxScale=100f;
    public List <int> ids=new List<int>();

   
    void Start()
    {
   
    }
    void FixedUpdate()
    {
        transform.position += transform.forward * speed;
     
        if (transform.localScale.x < maxScale)
        {
            transform.localScale += new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);
            float upIncrease = scaleIncrease * .25f;
            transform.position += new Vector3(0f, upIncrease);
         }

        if (!Physics.Raycast(transform.position, -Vector3.up))
        {
         
            Destroy(this.gameObject);
        }
        
      

           }

    void OnTriggerEnter(Collider other) 
    {

        ID_Manager tempID_Manager = (ID_Manager)other.transform.root.GetComponent("ID_Manager");
        if (tempID_Manager == null)
        {return;}
        for (int i = 0; i < ids.Count; i++)
        { if (tempID_Manager.ID == ids[i]) { return; } }
        
        PowerUpHit tempPowerUpHit = other.transform.root.gameObject.GetComponent<PowerUpHit>();
        if (!tempPowerUpHit) { return; }

        ids.Add(tempID_Manager.ID);
        tempPowerUpHit.GeneraliHit();
    }

}
