using UnityEngine;
using System.Collections;

public class MeetLamina : MonoBehaviour 
{
  public string myLamina;

  void OnTriggerEnter2D( Collider2D col )
  {
    if(col.tag=="Player")
    {
      Debug.Log("just recieved the "+ myLamina+" lamina");
      GOD.myGOD.MeetLamina(myLamina);
      Destroy(this.gameObject);
    }
  }
}
