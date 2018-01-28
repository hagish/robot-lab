using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
    public Vector3 StartPosition;

	void OnEnable () {
        StartPosition = transform.position;	
	}

    public void DoRespawn() {
        transform.position = StartPosition;
    }
}
