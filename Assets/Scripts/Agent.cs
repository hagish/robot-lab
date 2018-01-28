using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    public GameObject Body;

    public Animator BodyAnimator;
    public ParticleSystem CuddleParticleSystem;
    public AudioSource CuddleSound;

    public float FullCommandDuration = 1.0f;

    public float BodyRotateSpeed = 90;

    public float commandDuration = 1f;
    public float movementSpeed = 0.05f;

    public Renderer CommandRenderer;

    public bool IsToZeroOnDeath;

	private int numClips;
    private int currentSignalGroupId;
    private float timeLastSignal;
    private Vector3 dir;
    private Rigidbody rb;
	private AudioSource audioSource;

    public Renderer Renderer;
    public Material MaterialActive;

    public Material MaterialInactive;

    public Material MaterialRight;
    public Material MaterialLeft;
    public Material MaterialUp;
    public Material MaterialDown;
    public Material MaterialBlock;
	public AudioClip[] audioClips;

    public struct CommandEntry {
        public string Command;
        public Vector3 Direction;
        public float Strength;
    }
		
    internal void ExitReached(AgentExit agentExit) {
        if (IsToZeroOnDeath && LevelManager.Instance.CanSpawnMore()) {
            GetComponent<Respawn>().DoRespawn();
            Reset();
        } else {
            GameObject.Destroy(gameObject);
        }
    }

    public void Reset() {
        dir = new Vector3(0.0f, 0.0f, 0.0f);
        timeLastSignal = -100.0f;
        currentSignalGroupId = -1;
    }

    void Awake() {
        Reset();
        rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> ();
		numClips = audioClips.Length;

        if (Renderer == null) Renderer = GetComponentInChildren<Renderer>();
    }

    public bool IsCommandActive() {
        return Time.time < timeLastSignal + commandDuration;
    }

    private void Update() {
        if (Renderer != null) Renderer.sharedMaterial = IsCommandActive() ? MaterialInactive : MaterialActive;
        if (!IsCommandActive() && CommandRenderer != null) CommandRenderer.gameObject.SetActive(false);

        if (dir.sqrMagnitude > 0.1f) {
			Body.transform.rotation = Quaternion.RotateTowards(Body.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * BodyRotateSpeed);            
        }
    }

    void FixedUpdate() {
        float currentTime = Time.time;

        rb.velocity = Vector3.zero;

        var isMoving = false;

        if (IsCommandActive()) {
            //transform.position += movementSpeed * dir;
            rb.MovePosition(transform.position + movementSpeed * dir);
            //Debug.Log ("Moving agent; dir : " + transform.position);
            isMoving = true;
        }

        BodyAnimator.SetBool("Moving", isMoving);
    }


    void ChangeDirection(Vector3 newDir) {
        dir = newDir;
        timeLastSignal = Time.time;
		if (numClips > 0) {
			int clipId = UnityEngine.Random.Range (0, numClips - 1);
			audioSource.clip = audioClips [clipId];
			audioSource.Play ();
		}
    }

    void ShowCommand(string command, Color color) {
        if (CommandRenderer == null) return;

        CommandRenderer.gameObject.SetActive(true);

        switch (command) {
            case "Left":
                CommandRenderer.material = MaterialLeft;
                break;
            case "Right":
                CommandRenderer.material = MaterialRight;
                break;
            case "Up":
                CommandRenderer.material = MaterialUp;
                break;
            case "Down":
                CommandRenderer.material = MaterialDown;
                break;
            case "Block":
                CommandRenderer.material = MaterialBlock;
                break;
            default:
                CommandRenderer.gameObject.SetActive(false);
                break;
        }

		CommandRenderer.material.SetColor("_Color", color);
    }

    void ProcessCommand(string command, Vector3 commandDirection, float strength, Sender sender) {
        if (IsCommandActive()) return;

        commandDuration = FullCommandDuration * strength;

        // Debug.LogFormat("XXX {0} {1} {2}", command, commandDirection, strength, commandDuration);

        ShowCommand(command, sender.Color);

        switch (command) {
            case "Move":
                ChangeDirection(commandDirection);
                break;
            case "Left":
                //Debug.Log ("Agent moving left");
                ChangeDirection(new Vector3(-1.0f, 0.0f, 0.0f));
                break;

            case "Right":
                //Debug.Log ("Agent moving right");
                ChangeDirection(new Vector3(1.0f, 0.0f, 0.0f));
                break;

            case "Down":
                //Debug.Log ("Agent moving down");
                ChangeDirection(new Vector3(0.0f, 0.0f, -1.0f));
                break;

            case "Up":
                //Debug.Log ("Agent moving up");
                ChangeDirection(new Vector3(0.0f, 0.0f, 1.0f));
                break;

            case "Block":
                ChangeDirection(Vector3.zero);
                break;

            default:
                //Debug.Log("Unrecognised command : " + command);
                break;
        }
    }

    public void Hit(Signal signal) {
        // If signal originates from the wrong player(s), then ignore
        //if (signal.GetPlayerOriginId() != playerGroupId) return;

        // If signal came from previously received signal, then also ignore
        if (signal.GetSignalGroupId() == currentSignalGroupId) return;

        currentSignalGroupId = signal.GetSignalGroupId();
        if (IsCommandActive()) {
            // ignore
        } else {
            ProcessCommand(signal.GetCommand(), signal.GetCommandDirection(), signal.Strength(), signal.Sender);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var agent = collision.gameObject.GetComponentInParent<Agent>();

        if (agent != null && CuddleParticleSystem != null) {
            CuddleParticleSystem.Play();
        }

        if (agent != null && CuddleSound != null) {
            CuddleSound.Play();
        }
    }
}
