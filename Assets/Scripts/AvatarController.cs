﻿using UnityEngine;
using System.Collections;

public class AvatarController : MonoBehaviour 
{
    public PlayerData playerInfo;
    public float speed = 15.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public Aim Aim;

    [Range(0,1)]
    public float inputThreshold = 0.5f;

    public System.Action<Vector3, string> triggerAction;

    private Vector3 lastMoveDirection;

    private void Awake()
    {
        Aim = GetComponent<Aim>();
    }

    void FixedUpdate() 
    {
        CharacterController controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;

       // if (controller.isGrounded)
        {
            var dx = Input.GetAxisRaw(playerInfo.controllerId + "Horizontal");
            var dy = Input.GetAxisRaw(playerInfo.controllerId + "Vertical");
            moveDirection = new Vector3(dx, 0, dy);
            if(moveDirection.magnitude > inputThreshold)
            {
                lastMoveDirection = moveDirection;
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                controller.Move(moveDirection * Time.deltaTime);
            }
        }

        var direction = lastMoveDirection;
        if (Aim != null) Aim.SetDirection(direction);

        if (triggerAction != null) {
			if (Input.GetKey(KeyCode.Alpha1)) {
                triggerAction(direction, "Up");
			}
            if (Input.GetKey(KeyCode.Alpha2)) {
                triggerAction(direction, "Down");
            }
            if (Input.GetKey(KeyCode.Alpha3)) {
                triggerAction(direction, "Left");
            }
            if (Input.GetKey(KeyCode.Alpha4)) {
                triggerAction(direction, "Right");
            }
    	}
            
    }
}
