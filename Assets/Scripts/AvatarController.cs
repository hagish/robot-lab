using UnityEngine;
using System.Collections;

public class AvatarController : MonoBehaviour 
{
    public PlayerData playerInfo;
    public float speed = 15.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    [Range(0,1)]
    public float inputThreshold = 0.5f;

    public System.Action<Vector3, int> triggerAction;

    private Vector3 lastMoveDirection;

    void Update() 
    {
        CharacterController controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;

       // if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis(playerInfo.controllerId + "Horizontal"), 0, Input.GetAxis(playerInfo.controllerId + "Vertical"));
            if(moveDirection.magnitude > inputThreshold)
            {
                lastMoveDirection = moveDirection;
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                controller.Move(moveDirection * Time.deltaTime);
            }
        }

        if (triggerAction != null) {
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
                triggerAction(lastMoveDirection, 0);
			}
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                triggerAction(lastMoveDirection, 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                triggerAction(lastMoveDirection, 2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                triggerAction(lastMoveDirection, 3);
            }
    	}
            
    }
}
