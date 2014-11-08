using UnityEngine;

namespace Polycrime
{
    public class PropelRigidBody : MonoBehaviour, IPropelBehavior
    {
        public void React(Vector3 velocity)
        {
            if (rigidbody)
            {
                rigidbody.velocity = velocity;
            }
        }
    }
}