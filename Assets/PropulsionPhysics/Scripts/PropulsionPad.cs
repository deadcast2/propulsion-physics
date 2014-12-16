using UnityEngine;

namespace Polycrime
{
    public class PropulsionPad : MonoBehaviour
    {
        public Transform target;
        public float reachTime = 1.5f;
        public Color trajectoryColor = Color.magenta;
        public bool showTrajectory = true;

        protected virtual void Start()
        {
            // Added an empty start so the script can be enabled/disabled
        }

        ////////////////////////////////////////////////////////////////////////////////
        // When an object hits me, check to see if it implements the tsg_IPropelBehavior
        // interface and if it does call its implemented method.
        protected virtual bool PropelObject(GameObject propelObject, Vector3 velocity)
        {
            var objectInterface = propelObject.GetComponent(typeof(IPropelBehavior)) as IPropelBehavior;

            if (objectInterface != null)
            {
                objectInterface.React(velocity);
                return true;
            }

            return false;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            HandleTrigger(other.gameObject, other.bounds);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            HandleTrigger(other.gameObject, other.bounds);
        }

        private void HandleTrigger(GameObject gameObject, Bounds bounds)
        {
            if (PropulsionPadActive())
            {
                Vector3 veloctiy = TrajectoryMath.CalculateVelocity(bounds.center, target.position, reachTime);
                PropelObject(gameObject, veloctiy);
            }
        }

        private void OnDrawGizmos()
        {
            if (showTrajectory && PropulsionPadActive())
            {
                DrawTrajectory();
            }
        }

        private bool PropulsionPadActive()
        {
            return (enabled && target != null && reachTime > 0);
        }

        private void DrawTrajectory()
        {
            Vector3 initialVelocity = TrajectoryMath.CalculateVelocity(transform.position, target.position, reachTime);
            float deltaTime = reachTime / initialVelocity.magnitude;
            int drawSteps = (int)(initialVelocity.magnitude - 0.5f);
            Vector3 currentPosition = transform.position;
            Vector3 previousPosition = currentPosition;
            Gizmos.color = trajectoryColor;

            if (IsParabolicVelocity(initialVelocity))
            {
                for (int i = 0; i < drawSteps; i++)
                {
                    currentPosition += (initialVelocity * deltaTime) + (0.5f * Physics.gravity * deltaTime * deltaTime);
                    initialVelocity += Physics.gravity * deltaTime;
                    Gizmos.DrawLine(previousPosition, currentPosition);

                    //////////////////////////////////////////////////////////////////////////////////
                    // If the next loop is the last iteration, then don't update the previous position
                    // vector so it can be used to draw the gizmos arrow.
                    if ((i + 1) < drawSteps)
                    {
                        previousPosition = currentPosition;
                    }
                }
                DrawArrow(previousPosition, (currentPosition - previousPosition));
            }
            else
            {
                Vector3 newUpDirection = new Vector3(currentPosition.x, target.position.y, currentPosition.z);
                Gizmos.DrawLine(currentPosition, newUpDirection);
                DrawArrow(newUpDirection, new Vector3(0f, 0.01f, 0f));
            }
        }

        private void DrawArrow(Vector3 position, Vector3 direction)
        {
            int[] arrowAngles = new int[] { 225, 135 };

            foreach (int angle in arrowAngles)
            {
                Vector3 endPoint = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * Vector3.forward;
                Gizmos.DrawRay(position + direction, endPoint * 0.5f);
            }
        }

        private bool IsParabolicVelocity(Vector3 velocity)
        {
            return !(velocity.x == 0 && velocity.z == 0);
        }
    }
}