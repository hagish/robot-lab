using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class AvatarController : MonoBehaviour 
{
    private Vector3 faceDirection = Vector3.forward;
    public XboxController XboxController;

    public Aim aim;

    [Range(0,1)]
    public float inputThreshold = 0.5f;

    public System.Action<Vector3, string> triggerAction;

    public Vector3 lastMoveDirection;
    public float maxMoveSpeed = 15.0f;
    public Vector3 newPosition;

    private Vector3 lastNonZeroDirection;

    private void Awake()
    {
        aim = GetComponent<Aim>();
    }

    private void Start() {
    }

    void Update()
    {
        Movement();
        SetRotation();
        CallTrigger(faceDirection);    

        if (Input.GetKeyDown(KeyCode.Space)) {
            XboxController = XboxController == XboxController.First ? XboxController.Second : XboxController.First;
		}
    }

    public void Movement()
    {
        // Left stick movement
        Rigidbody controller = GetComponent<Rigidbody>();
        newPosition = transform.position;
        float axisX = InputManager.Get(XboxController).GetAxis(XboxAxis.LeftStickX);
        float axisY = InputManager.Get(XboxController).GetAxis(XboxAxis.LeftStickY);
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
            if (InputManager.Get(XboxController).GetButton(XboxButton.Y))
            {
                triggerAction(direction, "Up");
			}
            if (InputManager.Get(XboxController).GetButton(XboxButton.A))
            {
                triggerAction(direction, "Down");
            } 
            if (InputManager.Get(XboxController).GetButton(XboxButton.X))
            {
                triggerAction(direction, "Left");
            }
            if (InputManager.Get(XboxController).GetButton(XboxButton.B))
            {
                triggerAction(direction, "Right");
            }

            if (InputManager.Get(XboxController).GetAxis(XboxAxis.LeftTrigger) > inputThreshold)
            {
                triggerAction(direction, "Block");
            }
            if (InputManager.Get(XboxController).GetAxis(XboxAxis.RightTrigger) > inputThreshold)
            {
                triggerAction(direction, "Block");
            }

			if (InputManager.Get(XboxController).GetButton(XboxButton.LeftBumper))
			{
				aim.DecreaseConeAngle();
			}
			if (InputManager.Get(XboxController).GetButton(XboxButton.RightBumper))
			{
				aim.IncreaseConeAngle();
			}
    	}
    }

    public void SetRotation()
    {
      
        float dxR = InputManager.Get(XboxController).GetAxis(XboxAxis.RightStickX);
        float dyR = InputManager.Get(XboxController).GetAxis(XboxAxis.RightStickY);

        var direction = new Vector3(dxR, 0, dyR);
     
        if(direction.magnitude > 0.2f )
            faceDirection = direction;

      if (aim != null) aim.SetDirection(faceDirection);
    }
}
