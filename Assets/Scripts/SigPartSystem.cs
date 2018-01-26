using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigPartSystem : MonoBehaviour {
    public static SigPartSystem Instance;

    [System.Serializable]
    public class Entry {
        public string Command;
        public Material Material;
    }

    public List<Entry> Entries;

    public GameObject Prefab;

    private int nextSignalId = 1;

    public void Awake() {
        Instance = this;
    }

    [System.Serializable]
    public class SignalInfo {
        public Vector3 Source;
        public Vector3 MainDirection;
        public float AngleInDegree;
        public float Speed;
        public float Lifetime;

        public Material Material;
        public String Command;
        public int PlayerOriginId;
        public int SignalGroupId;

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
        public SignalInfo Signal;
        public Vector3 Direction;
        public float Radius;
    }

    public void Spawn(SignalInfo info, float deltaAngle, float particleRadius) {
        var id = nextSignalId;
        nextSignalId += 1;

        info.SignalGroupId = id;
        foreach (var it in Entries) {
            if (it.Command == info.Command) {
                info.Material = it.Material;
            }
        }

        var min = info.MinDirection;
        var max = info.MaxDirection;

        int maxI = Mathf.CeilToInt(info.AngleInDegree / deltaAngle);

		for (int i = 0; i <= maxI; ++i) {
            var f = (float)i / (float)maxI;
            var dir = info.MainDirection;

            if (f <= 0.5f) {
                dir = Vector3.Slerp(min, info.MainDirection, (f - 0.0f) * 2f);
            } else {
                dir = Vector3.Slerp(info.MainDirection, max, (f - 0.5f) * 2f);
            }

			SpawnParticle(info, dir, particleRadius);                
        }
    }

    private void SpawnParticle(SignalInfo info, Vector3 dir, float radius) {
        var go = GameObject.Instantiate(Prefab);
        var sp = go.GetComponent<SigPart>();

        sp.Init(new SignalParticleInfo() { 
            Direction = dir,
            Signal = info,
            Radius = radius,
        });

        var signal = go.GetComponent<Signal>();
        signal.SetCommand(info);
    }
}
