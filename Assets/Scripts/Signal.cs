using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    public SignalSystem.Info Info; 

    private float spawnTime;

    public LayerMask CollisionLayerMask;

    public ParticleSystem ParticleSystem;
	public Sender Sender;

    private static Collider[] colliderResult = new Collider[100];
    private static RaycastHit[] hitResult = new RaycastHit[100];

    public void Init(SignalSystem.Info info, Sender sender) {
        this.Info = info;
        transform.position = info.Source;
        spawnTime = Time.time;

        Sender = sender;

        var ems = ParticleSystem.emission;
        ems.rateOverTime = 0f;

        var main = ParticleSystem.main;
        main.startLifetime = info.Lifetime;
        main.duration = info.Lifetime;
        main.loop = false;

        ParticleSystem.Emit(info.Particles.Length);
        ParticleSystem.GetParticles(info.Particles);
    }

    private void ProcessCollider (Collider collider, SignalSystem.Info info, ParticleSystem.Particle p, int i) {
        var agent = collider.GetComponent<Agent>();
        if (agent != null) {
            agent.Hit(this);
        }
        var wall = collider.GetComponent<Wall>();
        if (wall != null) {
            // GameObject.Destroy(gameObject);
            p.velocity = Vector3.zero;
            p.position = Vector3.one * 10000f;
            info.Particles[i] = p;
        }
        var powerUp = collider.GetComponent<PowerUpScript>();
        if (powerUp != null) {
            powerUp.Hit(this);
        }
    }

    private void UpdateParticles() {
        if (Info == null) return;

        ParticleSystem.GetParticles(Info.Particles);
        if (Time.time - spawnTime > Info.Lifetime) {
            GameObject.Destroy(gameObject);
        } else {
            for (int i = 0; i < Info.Particles.Length; ++i) {
                var p = Info.Particles[i];

                if (Vector3.Distance(p.position, Info.Source) < Info.warmupDistance) {
                    // just skip
                    continue;
                }

                var stepDist = p.velocity.magnitude * Time.deltaTime;

                if (stepDist >= 0f) {
                    UnityEngine.Profiling.Profiler.BeginSample("SphereCast");
                    int count = Physics.SphereCastNonAlloc(p.position.SetY(0f), Info.ParticleSize, p.velocity.normalized, hitResult, stepDist, CollisionLayerMask);
                    Debug.DrawLine(p.position.SetY(0f), p.position.SetY(0f)+p.velocity.normalized * stepDist, Color.magenta, 1f, false);

                    for (int j = 0; j < count; ++j) {
                        ProcessCollider(hitResult[j].collider, Info, p, i);
                    }
                    UnityEngine.Profiling.Profiler.EndSample();
                } else {
                    UnityEngine.Profiling.Profiler.BeginSample("OverlapSphere");
                    int count = Physics.OverlapSphereNonAlloc(p.position.SetY(0f), Info.ParticleSize, colliderResult, CollisionLayerMask);

					for (int j = 0; j < count; ++j) {
                        ProcessCollider(colliderResult[j], Info, p, j);
					}
                    UnityEngine.Profiling.Profiler.EndSample();
                }
            }
        }

        ParticleSystem.SetParticles(Info.Particles, Info.Particles.Length);
    }

    private void FixedUpdate() {
        UpdateParticles();
    }

    public string GetCommand() {return Info.Command;}
	public Sender GetPlayerSender() {return Info.playerSender;}
	public int GetPlayerOriginId() {return Info.PlayerOriginId;}
    public int GetSignalGroupId() {return Info.SignalGroupId;}
    public Vector3 GetCommandDirection() {return Info.CommandDirection;}

    public float Strength() {
        return Mathf.Clamp01(Time.time - spawnTime / Info.Lifetime);
    }

}
