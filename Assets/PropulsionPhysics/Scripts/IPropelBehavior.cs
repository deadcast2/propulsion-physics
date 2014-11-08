using UnityEngine;

namespace Polycrime
{
    /////////////////////////////////////////////////
    // Implement this interface to add new propulsion
    // physics behaviors.
    public interface IPropelBehavior
    {
        void React(Vector3 velocity);
    }
}