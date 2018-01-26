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

	IEnumerator Start () {
		while (true) {
            SigPartSystem.Instance.Spawn(new SigPartSystem.SignalInfo(){
                AngleInDegree = AngleInDegree,
                Lifetime = Lifetime,
                MainDirection = Direction,
                Source = transform.position,
                Speed = Speed,
            }, DeltaAngle, ParticleRadius);
            yield return new WaitForSeconds(Timeout);
        }	
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Direction);
    }
}
