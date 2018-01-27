using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    SignalSystem.Info info; 

    private float spawnTime;

    public LayerMask CollisionLayerMask;

    public ParticleSystem ParticleSystem;
    private Collider[] colliderResult = new Collider[100];

    public void Init(SignalSystem.Info info) {
        this.info = info;
        transform.position = info.Source;
        spawnTime = Time.time;

        var ems = ParticleSystem.emission;
        ems.rateOverTime = 0f;

        var main = ParticleSystem.main;
        main.startLifetime = info.Lifetime;
        main.duration = info.Lifetime;
        main.loop = false;

        ParticleSystem.Emit(info.Particles.Length);
        ParticleSystem.GetParticles(info.Particles);
    }

    private void UpdateParticles() {
        if (info == null) return;

        ParticleSystem.GetParticles(info.Particles);

        if (Time.time - spawnTime > info.Lifetime) {
            GameObject.Destroy(gameObject);
        } else {
            for (int i = 0; i < info.Particles.Length; ++i) {
                var p = info.Particles[i];
                int count = Physics.OverlapSphereNonAlloc(p.position.SetY(0f), info.ParticleSize, colliderResult, CollisionLayerMask);
                for (int j = 0; j < count; ++j) {
                    var agent = colliderResult[j].GetComponent<Agent>();
                    if (agent != null) {
                        agent.Hit(this);
                    }
                    var wall = colliderResult[j].GetComponent<Wall>();
                    if (wall != null) {
                        // GameObject.Destroy(gameObject);
                        p.velocity = Vector3.zero;
                        p.position = Vector3.one * 10000f;
                        info.Particles[i] = p;
                    }
                }
				
                //p.velocity = vel;
                //p.position += vel * dt;
                //info.Particles[i] = p;
            }
        }

        ParticleSystem.SetParticles(info.Particles, info.Particles.Length);
    }

    private void FixedUpdate() {
        UpdateParticles();
    }

    public string GetCommand() {return info.Command;}
	public int GetPlayerOriginId() {return info.PlayerOriginId;}
    public int GetSignalGroupId() {return info.SignalGroupId;}
    public Vector3 GetCommandDirection() {return info.CommandDirection;}

    public float Strength() {
        return Mathf.Clamp01(Time.time - spawnTime / info.Lifetime);
    }

}
