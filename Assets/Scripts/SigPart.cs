using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigPart : MonoBehaviour {

    private SigPartSystem.SignalParticleInfo info;
    private float spawnTime;

    public Renderer Renderer;

    public void Init(SigPartSystem.SignalParticleInfo info) {
        this.info = info;
        transform.position = info.Signal.Source;
        spawnTime = Time.time;
        Renderer.sharedMaterial = info.Signal.Material;   
    }

    void FixedUpdate() {
        if (info == null) return;

        if (Time.time - spawnTime > info.Signal.Lifetime) {
            GameObject.Destroy(gameObject);
        } else {
            var dirStep = info.Direction.normalized * info.Signal.Speed * Time.deltaTime;
            RaycastHit hitInfo;
            var overlaps = Physics.OverlapSphere(transform.position, info.Radius);
            foreach (var collider in overlaps) {
                var agent = collider.GetComponent<Agent>();
                if (agent != null) {
                    agent.Hit(this);
                }
                var wall = collider.GetComponent<Wall>();
                if (wall != null) {
                    GameObject.Destroy(gameObject);
                }
            }
			transform.Translate(dirStep);
        }

    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, info.Radius);
    }

    public float Strength() {
        return Mathf.Clamp01(Time.time - spawnTime / info.Signal.Lifetime);
    }
}
