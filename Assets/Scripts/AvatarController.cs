using UnityEngine;
using System.Collections.Generic;
using XboxCtrlrInput;

public class AvatarController : MonoBehaviour
{
    private Vector3 faceDirection = Vector3.forward;
    public XboxController XboxController;

    public Aim aim;

    [Range(0, 1)]
    public float inputThreshold = 0.5f;

    public System.Action<Vector3, string> triggerAction;
    private List<string> ActionCommands;

    public Vector3 lastMoveDirection;
    public float maxMoveSpeed = 15.0f;
    public Vector3 newPosition;

    private Vector3 lastNonZeroDirection;
    private int commandCounter = 19010;
    private bool callExecuteAction = false;
    public string _selectedCommand = "Up";
    public string SelectedCommand
    {
        get
        {
            return _selectedCommand;
        }

    }

    public List<string> CommandList = new List<string>();

    // This will make sure to callBlockAction immidiately after pushing the trigger but wont spam till you release it
    private bool callBlockAction = true;

    private void Awake()
    {
        aim = GetComponent<Aim>();
    }

    private void Start()
    {
        CommandList.Add("Left");
        CommandList.Add("Right");
        CommandList.Add("Up");
        CommandList.Add("Down");
        //CommandList.Add("Block");

        ActionCommands = new List<string>();

    }

    void Update()
    {
        Movement();
        SetRotation();
        CallTrigger(faceDirection);

        if (Input.GetKeyDown(KeyCode.Space))
        {
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
            string command = GetCommandFromBummer();
            if (!string.IsNullOrEmpty(command))
            {
                _selectedCommand = command;
                AddCommand(command);
            }

            command = GetCommandFromButtons();
            if (!string.IsNullOrEmpty(command))
            {
                _selectedCommand = command;
                AddCommand(command);
            }

            command = GetCommandFromDPad();
            if (!string.IsNullOrEmpty(command))
            {
                _selectedCommand = command;
                AddCommand(command);
            }



            if (InputManager.Get(XboxController).GetAxis(XboxAxis.LeftTrigger) > inputThreshold)
            {
                if (callBlockAction)
                {
                    triggerAction(direction, "Block");
                    callBlockAction = false;
                }

            }
            else
            {
                callBlockAction = true;
            }

            if (InputManager.Get(XboxController).GetAxis(XboxAxis.RightTrigger) > inputThreshold)
            {

                aim.DecreaseConeAngle();
                callExecuteAction = true;
            }
            else
            {
                aim.IncreaseConeAngle();
                if (callExecuteAction)
                {
                    ExecuteCommandActions(direction);
                    callExecuteAction = false;
                }

            }

        }
    }

    public void SetRotation()
    {

        float dxR = InputManager.Get(XboxController).GetAxis(XboxAxis.RightStickX);
        float dyR = InputManager.Get(XboxController).GetAxis(XboxAxis.RightStickY);

        var direction = new Vector3(dxR, 0, dyR);

        if (direction.magnitude > 0.2f)
            faceDirection = direction;

        if (aim != null) aim.SetDirection(faceDirection);
    }

    public string GetCommandFromBummer()
    {
        string commandText = "";
        if (InputManager.Get(XboxController).GetButtonDown(XboxButton.LeftBumper))
        {
            --commandCounter;
        }
        if (InputManager.Get(XboxController).GetButtonDown(XboxButton.RightBumper))
        {
            ++commandCounter;
        }

        int commandIndex = commandCounter % CommandList.Count;
        commandText = CommandList[commandIndex];
        return commandText;
    }

    public string GetCommandFromButtons()
    {
        if (InputManager.Get(XboxController).GetButtonDown(XboxButton.Y))
            return "Up";

        else if (InputManager.Get(XboxController).GetButtonDown(XboxButton.A))
            return "Down";

        else if (InputManager.Get(XboxController).GetButtonDown(XboxButton.X))
            return "Left";

        else if (InputManager.Get(XboxController).GetButtonDown(XboxButton.B))
            return "Right";
        else
            return "";
    }

    public string GetCommandFromDPad()
    {

        if (InputManager.Get(XboxController).GetButtonDown(XboxButton.DPadUp))
            return "Up";

        else if (InputManager.Get(XboxController).GetButtonDown(XboxButton.DPadDown))
            return "Down";

        else if (InputManager.Get(XboxController).GetButtonDown(XboxButton.DPadLeft))
            return "Left";

        else if (InputManager.Get(XboxController).GetButtonDown(XboxButton.DPadRight))
            return "Right";
        else
            return "";
    }

    public void AddCommand(string command)
    {
        ActionCommands.Clear();

        if (!string.IsNullOrEmpty(command))
        {
            ActionCommands.Add(command);

            Debug.Log("New command : " + command);
        }
    }

    public void ExecuteCommandActions(Vector3 direction)
    {
        // Just send one command for now.
        // Fire and Forget thing.
        string command = "";
        if (ActionCommands != null && ActionCommands.Count > 0)
        {
            command = ActionCommands[0];
            triggerAction(direction, command);
            // ActionCommands.Clear();
        }

    }

}
