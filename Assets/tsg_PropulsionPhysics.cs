using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class tsg_PropulsionPhysics : MonoBehaviour {
  public Transform target;
  public float reachTime = 1.5f;
  public AudioClip propelSound;
  public Color trajectoryColor = Color.magenta;
  public bool showTrajectory = true;
  public float verticalOnlyMin = 0.5f;
  
  void Start() {
    // Added an empty start so the script could be enabled/disabled
  }

  void OnTriggerEnter(Collider other) {
    if(PropulsionPadActive()) {
      //////////////////////////////////////////////////////////////////
      // To prevent the collider from missing the target, get the
      // closest point the collider hit on the trigger and calculate the
      // velocity based on that starting point.
      Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
      PropelObject(other.gameObject, CalculateVelocity(hitPoint));
    }
  }

  void OnDrawGizmos() {
    if(showTrajectory && PropulsionPadActive()) {
      DrawTrajectory();
    }
  }
  
  public void SetTarget(Transform newTarget, float newReachTime) {
    if(newTarget != null && newReachTime > 0) {
      target = newTarget;
      reachTime = newReachTime;
    }
  }

  private

  bool PropulsionPadActive() {
    return enabled && target != null && reachTime > 0;
  }
 
  ////////////////////////////////////////////////////////////////////////////////
  // When an object hits me, check to see if it implements the tsg_IPropelBehavior
  // interface and if it does call its implemented method.
  void PropelObject(GameObject propelObject, Vector3 velocity) {
    tsg_IPropelBehavior objectInterface = propelObject.GetComponent(typeof(tsg_IPropelBehavior)) as tsg_IPropelBehavior;
    
    if(objectInterface != null) {
      objectInterface.React(velocity);
      PlayPropulsionSound();
    }
  }

  void PlayPropulsionSound() {
    if(propelSound) {
      AudioSource.PlayClipAtPoint(propelSound, transform.position);
    }
  }
 
  ////////////////////////////////////////////////////////////////////////////////
  // This is the heart of the Propulsion Physics script. If the target is far enough away
  // the normal trajectory is calculated based on the editor's set gravity. A non-
  // parabolic trajectory is calculated if the target is almost straight overhead.
  // The verticalOnlyMin can be adjusted to when the velocity calculation should
  // switch to vertical populsion only.
  Vector3 CalculateVelocity(Vector3 startPoint) {
    Vector3 direction = (target.position - startPoint);
    float gravity = Physics.gravity.magnitude;
    float yVelocity = (direction.y / reachTime) + (0.5f * gravity * reachTime);
    
    if(TargetTooClose()) {
      return new Vector3(0, yVelocity, 0);
    } else {
      return new Vector3(direction.x / reachTime, yVelocity, direction.z / reachTime);
    }
  }

  bool TargetTooClose(){
    Vector3 targetPosition = target.position;
    Vector3 leveledTarget = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
    return Vector3.Distance(leveledTarget, transform.position) <= verticalOnlyMin;
  }

  void DrawTrajectory() {
    Vector3 initialVelocity = CalculateVelocity(transform.position);
    float deltaTime = reachTime / initialVelocity.magnitude;
    int drawSteps = (int)(initialVelocity.magnitude - 0.5f);
    Vector3 currentPosition = transform.position;
    Vector3 previousPosition = currentPosition;
    Gizmos.color = trajectoryColor;
    
    if(IsParabolicVelocity(initialVelocity)) {
      for(int i = 0; i < drawSteps; i++) {
        currentPosition += (initialVelocity * deltaTime) + (0.5f * Physics.gravity * deltaTime * deltaTime);
        initialVelocity += Physics.gravity * deltaTime;
        Gizmos.DrawLine(previousPosition, currentPosition);
        //////////////////////////////////////////////////////////////////////////////////
        // If the next loop is the last iteration, then don't update the previous position
        // vector so it can be used to draw the gizmos arrow.
        if((i+1) < drawSteps) {
          previousPosition = currentPosition;
        }
      }
      DrawArrow(previousPosition, (currentPosition - previousPosition));
    } else {
      Vector3 newUpDirection = new Vector3(currentPosition.x, target.position.y, currentPosition.z);
      Gizmos.DrawLine(currentPosition, newUpDirection);
      DrawArrow(newUpDirection, new Vector3(0f, 0.01f, 0f));
    }
  }

  void DrawArrow(Vector3 position, Vector3 direction) {
    int[] arrowAngles = new int[] { 225, 135 };
    foreach(int angle in arrowAngles) {
      Vector3 endPoint = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * Vector3.forward;
      Gizmos.DrawRay(position + direction, endPoint * 0.5f);
    }
  }

  bool IsParabolicVelocity(Vector3 velocity) {
    return !(velocity.x == 0 && velocity.z == 0);
  }
}