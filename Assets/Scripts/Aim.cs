using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {
    public GameObject Root;
    Vector3 lastDirection = Vector3.zero;
    public void SetDirection (Vector3 direction) {
        if(direction == Vector3.zero)
        {
           direction = lastDirection;
        }
        Root.transform.rotation = Quaternion.LookRotation(direction);
        lastDirection = direction;
    }
}
