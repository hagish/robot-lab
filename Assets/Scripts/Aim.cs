using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {
    public GameObject Root;

    public void SetDirection (Vector3 direction) {
        Root.transform.rotation = Quaternion.LookRotation(direction);
    }
}
