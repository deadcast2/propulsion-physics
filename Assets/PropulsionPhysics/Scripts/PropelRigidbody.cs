using UnityEngine;

namespace Polycrime
{
    public class PropelRigidbody : MonoBehaviour, IPropelBehavior
    {
        private Rigidbody cachedRigidbody3D;
        private Rigidbody2D cachedRigidbody2D;

        public void React(Vector3 velocity)
        {
            if (cachedRigidbody3D) cachedRigidbody3D.velocity = velocity;

            if (cachedRigidbody2D) cachedRigidbody2D.velocity = velocity;
        }

        private void Awake()
        {
            cachedRigidbody3D = GetComponent<Rigidbody>();
            cachedRigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
