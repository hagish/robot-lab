using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class AvatarController : MonoBehaviour 
{
    public PlayerData playerInfo;
    public float speed = 15.0F;
    public float RotationSpeed = 15.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    private Vector3 faceDirection = Vector3.fwd;
    public XboxController XboxController;

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

    
        float dx = XCI.GetAxis(XboxAxis.LeftStickX, XboxController);
        float dy = XCI.GetAxis(XboxAxis.LeftStickY, XboxController);

        moveDirection = new Vector3(dx, 0, dy);
        if(moveDirection.magnitude > inputThreshold)
        {
            lastMoveDirection = moveDirection;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);
        }
        SetRotation();
        CallTrigger(faceDirection);    
    }

    public void CallTrigger(Vector3 direction)
    {
       if (triggerAction != null) 
       {
            if (XCI.GetButtonDown(XboxButton.LeftBumper, XboxController))
            {
                triggerAction(direction, "Up");
			}

            if (XCI.GetButtonDown(XboxButton.RightBumper, XboxController))
            {
                triggerAction(direction, "Down");
            } 
           if (XCI.GetButtonDown(XboxButton.LeftStick, XboxController))
            {
                triggerAction(direction, "Left");
            }
            if (XCI.GetButtonDown(XboxButton.RightStick, XboxController))
            {
                triggerAction(direction, "Right");
            }
            if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController) > 0.6f)
            {
                triggerAction(direction, "Move");
            }
            if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController) > 0.6f)
            {
                triggerAction(direction, "Block");
            }
    	}
    }
    public void SetRotation()
    {
        float dxR = XCI.GetAxis(XboxAxis.RightStickX, XboxController);
        float dyR = XCI.GetAxis(XboxAxis.RightStickY, XboxController);

        var direction = new Vector3(dxR, 0, dyR);

        if(direction.magnitude > 0.2f )
            faceDirection = direction;

        if (Aim != null) Aim.SetDirection(faceDirection);

    }
}
