using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class AvatarController : MonoBehaviour 
{
    public float speed = 15.0F;

    private Vector3 faceDirection = Vector3.forward;
    public XboxController XboxController;

    public Aim Aim;

    [Range(0,1)]
    public float inputThreshold = 0.5f;

    public System.Action<Vector3, string> triggerAction;

    public Vector3 lastMoveDirection;
    public float maxMoveSpeed = 15.0f;
    public Vector3 newPosition;

    private Vector3 lastNonZeroDirection;

    private void Awake()
    {
        Aim = GetComponent<Aim>();
    }

    void FixedUpdate() 
    {
        Movement();
        SetRotation();
        CallTrigger(faceDirection);    
    }

    public void Movement()
    {
        // Left stick movement
        Rigidbody controller = GetComponent<Rigidbody>();
        newPosition = transform.position;
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, XboxController);
        float axisY = XCI.GetAxis(XboxAxis.LeftStickY, XboxController);
        float newPosX = newPosition.x + (axisX * maxMoveSpeed * Time.deltaTime);
        float newPosZ = newPosition.z + (axisY * maxMoveSpeed * Time.deltaTime);
        newPosition = new Vector3(newPosX, transform.position.y, newPosZ);
        controller.MovePosition(newPosition);
       // transform.position = newPosition;
        

    }

    public void CallTrigger(Vector3 direction)
    {
       if (triggerAction != null) 
       {
            if (XCI.GetButton(XboxButton.Y, XboxController))
            {
                triggerAction(direction, "Up");
			}
            if (XCI.GetButton(XboxButton.A, XboxController))
            {
                triggerAction(direction, "Down");
            } 
           if (XCI.GetButton(XboxButton.X, XboxController))
            {
                triggerAction(direction, "Left");
            }
            if (XCI.GetButton(XboxButton.B, XboxController))
            {
                triggerAction(direction, "Right");
            }

            if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController) > inputThreshold)
            {
                triggerAction(direction, "Block");
            }
            if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController) > inputThreshold)
            {
                triggerAction(direction, "Move");
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
