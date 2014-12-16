using UnityEngine;

namespace Polycrime
{
    public class PropulsionTarget : MonoBehaviour
    {
        [Range(0.1f, 1.0f)]
        public float size = 0.2f;
        public Color color = Color.red;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, size);
        }
    }
}