using UnityEngine;

namespace Polycrime
{
    public class PropelRigidbody : MonoBehaviour, IPropelBehavior
    {
        public void React(Vector3 velocity)
        {
            if (rigidbody)
            {
                rigidbody.velocity = velocity;
            }

            if (rigidbody2D)
            {
                rigidbody2D.velocity = velocity;
            }
        }
    }
}