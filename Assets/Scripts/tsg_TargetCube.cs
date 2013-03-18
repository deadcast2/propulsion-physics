using UnityEngine;

public class tsg_TargetCube : MonoBehaviour {
  void Start() {
    if(Application.isPlaying) {
      gameObject.SetActive(false);
    }
  }
}