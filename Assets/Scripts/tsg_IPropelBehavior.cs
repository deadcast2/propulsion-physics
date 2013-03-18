using UnityEngine;
using System.Collections.Generic;

/////////////////////////////////////////////////
// Implement this interface to add new propulsion
// physics behaviors.
public interface tsg_IPropelBehavior {
  void React(Vector3 velocity);
}