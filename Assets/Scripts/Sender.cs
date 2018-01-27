﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour {

    public int PlayerId;

    public float Timeout = 1f;
    public float Lifetime = 1f;
	public float MinAngleInDegrees = 10.0f;
	public float MaxAngleInDegrees = 180.0f;
	public float MaxSpeed = 4.0f;
	public float MinSpeed = 0.5f;
    public float DeltaAngle = 1f;
    public float ParticleRadius = 0.5f;
	public float SpawnDistance = 0.5f;

    public float Energy;
    public float EnergyReg;
	public Vector3 Direction;

	private float nextTriggerTime;
	private Aim aim;

	void Awake()
	{
		aim = GetComponent<Aim> ();
	}

    [System.Serializable]
    public class Entry {
        public string Command;
        public float Cost;
        public float Cooldown;
    }

    public List<Entry> Entries;

    private void OnEnable()
    {
        if (GetComponent<AvatarController>() != null) {
			GetComponent<AvatarController>().triggerAction = (Vector3 direction, string command) => {
				
                Trigger(direction, command);
			}; 
        }

        // StartCoroutine(TestSendLoop());
    }

    IEnumerator TestSendLoop () {
		while (true) {
            Trigger(Direction, "Up");
            yield return new WaitForSeconds(Timeout);
        }
	}

    void Trigger(Vector3 direction, string command) {
        if (Time.time <= nextTriggerTime) return;
        if (direction.sqrMagnitude <= 0f) return;

        float cooldown = 0f;
        float cost = 0f;

		float ConeFraction = aim.GetConeFraction ();
		float AngleInDegree = aim.GetAngleInDegrees ();
		float Speed = Mathf.Lerp (MaxSpeed, MaxSpeed, ConeFraction);

        foreach (var it in Entries) {
            if (it.Command == command) {
                cooldown = it.Cooldown;
                cost = it.Cost;
            }
        }
		
        if (cost > Energy) return;

        nextTriggerTime = Time.time + cooldown;
        Energy -= cost;
        UKMessenger.Broadcast<int, float>("energy_set", PlayerId, Energy);
        UKMessenger.Broadcast<int, float>("cooldowntime_set", PlayerId, nextTriggerTime);
        SignalSystem.Instance.Spawn(new SignalSystem.Info() {
            AngleInDegree = AngleInDegree,
            Lifetime = Lifetime,
			MainDirection = direction.normalized,
            Source = transform.position,
            Speed = Speed,
            ParticleSize = ParticleRadius,
			CommandDirection = direction.normalized,
            Command = command,
        }, DeltaAngle);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Direction);
    }

    private void FixedUpdate() {
        Energy += EnergyReg * Time.deltaTime;
        Energy = Mathf.Clamp01(Energy);
        UKMessenger.Broadcast<int, float>("energy_set", PlayerId, Energy);
    }
}
