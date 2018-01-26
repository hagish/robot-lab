using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	const float commandDuration = 1.0f;
	const float movementSpeed = 0.05f;

	private int currentSignalGroupId;
	private float timeLastSignal;
	private Vector3 dir;

	public int playerGroupId;


	void Awake()
	{
		dir = new Vector3 (0.0f, 0.0f, 0.0f);
		timeLastSignal = -100.0f;
		currentSignalGroupId = -1;
	}


	void Start ()
	{
	}


	void Update ()
	{
		float currentTime = Time.time;

		if (currentTime - timeLastSignal < commandDuration) {
			transform.parent.transform.position += movementSpeed * dir;
			Debug.Log ("Moving agent; dir : " + dir);
		}
	}


	void ProcessCommand(string command)
	{
		switch (command)
		{
		case "Left":
			Debug.Log ("Agent moving left");
			timeLastSignal = Time.time;
			dir = new Vector3 (-1.0f, 0.0f, 0.0f);
			break;

		case "Right":
			Debug.Log ("Agent moving right");
			timeLastSignal = Time.time;
			dir = new Vector3 (1.0f, 0.0f, 0.0f);
			break;

		case "Down":
			Debug.Log ("Agent moving down");
			timeLastSignal = Time.time;
			dir = new Vector3 (0.0f, 0.0f, -1.0f);
			break;

		case "Up":
			Debug.Log ("Agent moving up");
			timeLastSignal = Time.time;
			dir = new Vector3 (0.0f, 0.0f, 1.0f);
			break;

		default:
			Debug.Log("Unrecognised command : " + command);
			break;
		}
	}

    internal void Hit(SigPartSystem.SignalParticleInfo info) {
        Debug.LogError(info);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError(collision);
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "signal") {

			Debug.Log("Detected signal particle");
			Signal signal = other.gameObject.GetComponent<Signal> ();

			// If signal originates from the wrong player(s), then ignore
			if (signal.GetPlayerOriginId () != playerGroupId) return;

			// If signal came from previously received signal, then also ignore
			if (signal.GetSignalGroupId() == currentSignalGroupId) return;

			ProcessCommand (signal.GetCommand ());
		}
	}

}
