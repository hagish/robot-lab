using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour {

    public float Timeout = 1f;
    public float Lifetime = 1f;
    public Vector3 Direction;
    public float AngleInDegree = 180f;
    public float Speed = 1f;

    public float DeltaAngle = 1f;
    public float ParticleRadius = 0.5f;

    public float Cooldown;
    private float nextTriggerTime;

    private void OnEnable()
    {
        GetComponent<AvatarController>().triggerAction = (Vector3 direction, string command) => {
            Trigger(direction, command);
        }; 
    }

    IEnumerator TestSendLoop () {
		while (true) {
            Trigger(Vector3.zero, "Up");
            yield return new WaitForSeconds(Timeout);
        }
	}

    void Trigger(Vector3 direction, string command) {
        if (Time.time <= nextTriggerTime) return;
        if (direction.sqrMagnitude <= 0f) return;
        nextTriggerTime = Time.time + Cooldown;

        SigPartSystem.Instance.Spawn(new SigPartSystem.SignalInfo() {
            AngleInDegree = AngleInDegree,
            Lifetime = Lifetime,
            MainDirection = direction,
            Source = transform.position,
            Speed = Speed,

            CommandDirection = direction,
            Command = command,
        }, DeltaAngle, ParticleRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Direction);
    }
}
