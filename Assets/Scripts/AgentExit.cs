﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentExit : MonoBehaviour {

	public int playerExitId;

	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.GetComponentInParent<Agent>() != null)
		{
			Debug.Log ("Agent collision detected");
			Agent agent = other.gameObject.GetComponent<Agent> ();
			if (agent.playerGroupId == playerExitId) {
				Debug.Log ("Destroying agent");
				Destroy (other.gameObject);
			}
		}
	}
}