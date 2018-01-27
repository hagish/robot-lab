using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnergy : MonoBehaviour {
    public int PlayerId;

    private RectTransform rt;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
    
        UKMessenger.AddListener<int, float>("energy_set", gameObject, (playerId, val) => {
            if (playerId == PlayerId) {
                var sd = rt.sizeDelta;
                rt.sizeDelta = new Vector2(100f * val, rt.sizeDelta.y);
            }
        });
    }
}
