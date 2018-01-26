using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigPart : MonoBehaviour {

    private SigPartSystem.SignalParticleInfo info;
    private float spawnTime;

    public void Init(SigPartSystem.SignalParticleInfo info) {
        this.info = info;
        transform.position = info.Signal.Source;
        spawnTime = Time.time;
    }

    void FixedUpdate() {
        if (info == null) return;

        if (Time.time - spawnTime > info.Signal.Lifetime) {
            GameObject.Destroy(gameObject);
        } else {
            transform.Translate(info.Direction.normalized * info.Signal.Speed * Time.deltaTime);
        }
    }
}
