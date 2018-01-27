using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICooldown : MonoBehaviour {
    public int PlayerId;
    public float scaleValue;
    public float maxHeight;
    private RectTransform rt;
    private float nextTriggerTimer;
	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
    
        UKMessenger.AddListener<int, float>("cooldowntime_set", gameObject, (playerId, val) => {
            if (playerId == PlayerId) {
                nextTriggerTimer = val;
               // Debug.Log("received cooldown : "+val);
            }
        });

    
    }

    void Update()
    {
        float scale = (nextTriggerTimer - Time.time)* scaleValue;
        float scaleClammped = Mathf.Clamp(scale, 0.0f, maxHeight);
       
        rt.gameObject.SetActive(true);
        rt.sizeDelta = new Vector2( rt.sizeDelta.x, scaleClammped);
    }
}
