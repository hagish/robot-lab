using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour 
{
	public float EnergyBoost = 0.5f;

    public bool IsCollectBySignal = false;
    public bool IsCollectByPlayer = true;

	public void Hit(Signal signal) 
	{
		Sender sender = signal.GetPlayerSender ();
        if (IsCollectBySignal) Process(sender);
	}

    private void OnCollisionEnter(Collision collision) {
        var sender = collision.gameObject.GetComponentInParent<Sender>();
        if (IsCollectByPlayer) Process(sender);
    }

    private void Process(Sender sender) {
        if (sender == null) return;
        sender.AddEnergyBoost(EnergyBoost);
        Destroy(transform.gameObject);
    }
}
