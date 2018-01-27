using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {
    public GameObject Root;
    public float lerpSpeed = 0.005f;
    Vector3 lastDirection = Vector3.zero;
    public void SetDirection (Vector3 direction) {
        if(direction == Vector3.zero)
        {
           direction = lastDirection;
        }
        Quaternion lastAngle = Quaternion.LookRotation(lastDirection);
        Quaternion newAngle = Quaternion.LookRotation(direction);
        Root.transform.rotation = Quaternion.RotateTowards(lastAngle, newAngle, lerpSpeed);
       
        lastDirection = direction;
    }
}
