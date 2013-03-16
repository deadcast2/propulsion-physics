using UnityEngine;
using System.Collections.Generic;

public class tsg_PropelRigidBody : MonoBehaviour, tsg_IPropelBehavior {
  public void React(Vector3 velocity) {
    if(rigidbody) {
      rigidbody.velocity = velocity;
    }
  }
}