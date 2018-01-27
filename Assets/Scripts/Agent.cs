using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	public float FullCommandDuration = 1.0f;

    public float commandDuration = 1f;
	public float movementSpeed = 0.05f;

	private int currentSignalGroupId;
	private float timeLastSignal;
	private Vector3 dir;
	private Rigidbody rb;

    public Renderer Renderer;
    public Material MaterialActive;
    public Material MaterialInactive;

	//public int playerGroupId;


	void Awake()
	{
		dir = new Vector3 (0.0f, 0.0f, 0.0f);
		timeLastSignal = -100.0f;
		currentSignalGroupId = -1;
		rb = GetComponent<Rigidbody> ();

        if (Renderer == null) Renderer = GetComponentInChildren<Renderer>();
	}

    public bool IsCommandActive() {
        return Time.time < timeLastSignal + commandDuration;
    }

    private void Update() {
        if (Renderer != null) Renderer.sharedMaterial = IsCommandActive() ? MaterialActive : MaterialInactive;
    }

    void FixedUpdate ()
	{
		float currentTime = Time.time;
		
        rb.velocity = Vector3.zero;

        if (IsCommandActive()) {
            //transform.position += movementSpeed * dir;
			rb.MovePosition (transform.position + movementSpeed * dir);
			//Debug.Log ("Moving agent; dir : " + transform.position);
		}
	}


	void ChangeDirection(Vector3 newDir)
	{        
		dir = newDir;
		timeLastSignal = Time.time;
	}


    void ProcessCommand(string command, Vector3 commandDirection, float strength)
	{        
        if (IsCommandActive()) return;

        commandDuration = FullCommandDuration * strength;

 		switch (command)
		{
            case "Move":
                ChangeDirection(commandDirection);
                break;
		case "Left":
			//Debug.Log ("Agent moving left");
			ChangeDirection (new Vector3 (-1.0f, 0.0f, 0.0f));
			break;

		case "Right":
			//Debug.Log ("Agent moving right");
			ChangeDirection (new Vector3 (1.0f, 0.0f, 0.0f));
			break;

		case "Down":
			//Debug.Log ("Agent moving down");
			ChangeDirection (new Vector3 (0.0f, 0.0f, -1.0f));
			break;

		case "Up":
			//Debug.Log ("Agent moving up");
			ChangeDirection (new Vector3 (0.0f, 0.0f, 1.0f));
			break;

        case "Block":
                ChangeDirection(Vector3.zero);
            break;
            
            default:
			//Debug.Log("Unrecognised command : " + command);
			break;
		}
	}

    public void Hit(SigPart sigPart) {
        Signal signal = sigPart.GetComponent<Signal>();

        // If signal originates from the wrong player(s), then ignore
        //if (signal.GetPlayerOriginId() != playerGroupId) return;

        // If signal came from previously received signal, then also ignore
        if (signal.GetSignalGroupId() == currentSignalGroupId) return;

		currentSignalGroupId = signal.GetSignalGroupId ();
        ProcessCommand(signal.GetCommand(), signal.GetCommandDirection(), sigPart.Strength());
    }
}
