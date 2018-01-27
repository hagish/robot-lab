using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSystem : MonoBehaviour {
    public static SignalSystem Instance;

    public Vector3 Offset;

    [System.Serializable]
    public class Entry {
        public string Command;
        public Material Material;
        public Color Color;
    }

    public List<Entry> Entries;

    public GameObject Prefab;

    private int nextSignalId = 1;

    public void Awake() {
        Instance = this;
    }

    [System.Serializable]
    public class Info {
        public Vector3 Source;
        public Vector3 MainDirection;
        public float AngleInDegree;
        public float Speed;
        public float Lifetime;
        public Color Color;
        public Material Material;
        public Vector3 CommandDirection;
        public String Command;
        public int PlayerOriginId;
        public int SignalGroupId;

        public ParticleSystem.Particle[] Particles;

        public Vector3 MinDirection {
            get {
                var d = Mathf.Min(AngleInDegree / 2f, 180f - 1f);
                return Quaternion.Euler(0f, -d, 0f) * MainDirection;
            }
        }

        public Vector3 MaxDirection {
            get {
                var d = Mathf.Min(AngleInDegree / 2f, 180f - 1f);
                return Quaternion.Euler(0f, d, 0f) * MainDirection;
            }
        }
    }

    [System.Serializable]
    public class SignalParticleInfo {
        public Info Signal;
        public Vector3 Direction;
        public float Radius;
    }

    public void Spawn(Info info, float deltaAngle, float particleRadius) {
        var id = nextSignalId;
        nextSignalId += 1;

        info.SignalGroupId = id;
        foreach (var it in Entries) {
            if (it.Command == info.Command) {
                info.Material = it.Material;
                info.Color = it.Color;
            }
        }
        info.Source += Offset;

        var min = info.MinDirection;
        var max = info.MaxDirection;

        int maxI = Mathf.CeilToInt(info.AngleInDegree / deltaAngle);

        var go = GameObject.Instantiate(Prefab, info.Source, Quaternion.identity);
        var signal = go.GetComponent<Signal>();

        info.Particles = new ParticleSystem.Particle[maxI + 1];
		
        signal.Init(info);

        for (int i = 0; i <= maxI; ++i) {
            var f = (float)i / (float)maxI;
            var dir = info.MainDirection;

            if (f <= 0.5f) {
                dir = Vector3.Slerp(min, info.MainDirection, (f - 0.0f) * 2f);
            } else {
                dir = Vector3.Slerp(info.MainDirection, max, (f - 0.5f) * 2f);
            }

            var p = info.Particles[i];

            p.position = info.Source;
            p.velocity = dir * info.Speed;
            p.startColor = info.Color;
            p.remainingLifetime = info.Lifetime;
            p.startSize = particleRadius;

            info.Particles[i] = p;
        }

        signal.ParticleSystem.SetParticles(info.Particles, info.Particles.Length);
        signal.ParticleSystem.Play();
    }
}
