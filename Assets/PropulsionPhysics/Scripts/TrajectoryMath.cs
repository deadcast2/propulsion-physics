using UnityEngine;
using System.Collections;

namespace Polycrime
{
    public static class TrajectoryMath
    {
        private static float verticalOnlyMin = 0.5f;

        ////////////////////////////////////////////////////////////////////////////////////
        // If the target is far enough away, the normal trajectory is calculated based on 
        // the editor's set gravity. A non-parabolic trajectory is calculated if the target
        // is almost straight overhead. The verticalOnlyMin can be adjusted to when the
        // velocity calculation should switch to vertical populsion only.
        public static Vector3 CalculateVelocity(Vector3 startPoint, Vector3 endPoint, float time)
        {
            Vector3 direction = (endPoint - startPoint);
            float gravity = Physics.gravity.magnitude;
            float yVelocity = (direction.y / time) + (0.5f * gravity * time);

            if (TargetTooClose(startPoint, endPoint))
            {
                return new Vector3(0, yVelocity, 0);
            }
            else
            {
                return new Vector3(direction.x / time, yVelocity, direction.z / time);
            }
        }

        private static bool TargetTooClose(Vector3 startPoint, Vector3 endPoint)
        {
            Vector3 targetPosition = endPoint;
            Vector3 leveledTarget = new Vector3(targetPosition.x, startPoint.y, targetPosition.z);

            return Vector3.Distance(leveledTarget, startPoint) <= verticalOnlyMin;
        }
    }
}
