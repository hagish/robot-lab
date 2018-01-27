using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public abstract class InputSource {

    public abstract float GetAxis(XboxAxis axis);
    public abstract bool GetButton(XboxButton button);
    public abstract bool GetButtonDown(XboxButton button);
}

public class InputSourceXbox : InputSource {
    public XboxController XboxController;

    public InputSourceXbox(XboxController xboxController) {
        this.XboxController = xboxController;
    }

    public override float GetAxis(XboxAxis axis) {
        return XCI.GetAxis(axis, XboxController);
    }

    public override bool GetButton(XboxButton button)
    {
        return XCI.GetButton(button, XboxController);
    }

     public override bool GetButtonDown(XboxButton button)
    {
        return XCI.GetButtonDown(button, XboxController);
    }
}

public class InputSourceKeyboard : InputSource {
    public Dictionary<XboxAxis, UKTuple<KeyCode, KeyCode>> axisMapping = new Dictionary<XboxAxis, UKTuple<KeyCode, KeyCode>>();
    public Dictionary<XboxButton, KeyCode> buttonMapping = new Dictionary<XboxButton, KeyCode>();

    public override float GetAxis(XboxAxis axis) {
        UKTuple<KeyCode, KeyCode> posNegCode;
        if (axisMapping.TryGetValue(axis, out posNegCode)) {
            if (Input.GetKey(posNegCode.a)) return -1f;
            else if (Input.GetKey(posNegCode.b)) return 1f;
            else return 0f;
        } else {
            return 0f;
        }
    }

    public override bool GetButton(XboxButton button) {
        KeyCode code;
        if (buttonMapping.TryGetValue(button, out code)) {
            if (Input.GetKey(code)) return true;
            else return false;
        } else {
            return false;
        }
    }

    public override bool GetButtonDown(XboxButton button)
    {
        KeyCode code;
        if (buttonMapping.TryGetValue(button, out code))
        {
            if (Input.GetKeyDown(code)) return true;
            else return false;
        }
        else
        {
            return false;
        }
    }

    public InputSourceKeyboard Assign(XboxButton button, KeyCode code) {
        buttonMapping[button] = code;
        return this;
    }
    public InputSourceKeyboard Assign(XboxAxis axis, KeyCode negCode, KeyCode posCode) {
        axisMapping[axis] = new UKTuple<KeyCode, KeyCode>(negCode, posCode);
        return this;
    }
}

public class InputSourceChain : InputSource {
    public List<InputSource> sources = new List<InputSource>();

    public InputSourceChain(params InputSource[] sources) {
        this.sources = new List<InputSource>(sources);
    }

    public override float GetAxis(XboxAxis axis) {
        foreach (var source in sources) {
            var d = source.GetAxis(axis);
            if (d != 0f) return d;
        }
        return 0f;
    }

    public override bool GetButton(XboxButton button)
    {
        foreach (var source in sources)
        {
            var d = source.GetButton(button);
            if (d) return d;
        }
        return false;
    }

     public override bool GetButtonDown(XboxButton button)
    {
        foreach (var source in sources)
        {
            var d = source.GetButtonDown(button);
            if (d) return d;
        }
        return false;

    }
}

public static class InputManager {
    public static InputSource _Xbox1 = new InputSourceXbox(XboxController.First);
    public static InputSource _Xbox2 = new InputSourceXbox(XboxController.Second);
    public static InputSource _Key1 = new InputSourceKeyboard()
                .Assign(XboxAxis.LeftStickX, KeyCode.A, KeyCode.D)
                .Assign(XboxAxis.LeftStickY, KeyCode.S, KeyCode.W)
                .Assign(XboxAxis.RightStickX, KeyCode.J, KeyCode.L)
                .Assign(XboxAxis.RightStickY, KeyCode.K, KeyCode.I)
                .Assign(XboxAxis.LeftTrigger, KeyCode.Q, KeyCode.E)
                .Assign(XboxAxis.RightTrigger, KeyCode.U, KeyCode.O)
                .Assign(XboxButton.A, KeyCode.G)
                .Assign(XboxButton.B, KeyCode.H)
                .Assign(XboxButton.X, KeyCode.F)
                .Assign(XboxButton.Y, KeyCode.T);

    public static InputSource _Nop = new InputSourceKeyboard();

    public static InputSource Player1 = new InputSourceChain(_Xbox1, _Key1);
    public static InputSource Player2 = new InputSourceChain(_Xbox2);


    public static InputSource Get(int playerId) {
        if (playerId == 1) return Player1;
        else if (playerId == 2) return Player2;
        else return _Nop;
    }

    public static InputSource Get(XboxController xboxController) {
        if (xboxController == XboxController.First) return Get(1);
        else if (xboxController == XboxController.Second) return Get(2);
        else return Get(0);
    }
}