using UnityEngine;
using System.Collections;

public class GraveRoots : MonoBehaviour 
{
  void OnCollisionEnter2D(Collision2D col)
  {

    if (col.gameObject.tag == "Attack")
  {
      AttackInstance script = col.gameObject.GetComponent<AttackInstance>();
      if(script.myElementalType== ElementalType.fire)
      {
        Die();
      }
  }
}
  void Die()
  {
    //we will do a more elegant solution later involving the level manager doing the array-destroy method
    Destroy (this.gameObject);
  }

}
