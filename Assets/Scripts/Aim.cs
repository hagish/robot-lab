using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {
    public GameObject Root;

    public void SetDirection (Vector3 direction) {
        if (direction.sqrMagnitude > 0f) {
			Root.transform.rotation = Quaternion.LookRotation(direction);            
        }
    }
}
