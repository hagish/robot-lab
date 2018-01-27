using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public abstract class InputSource {

    public abstract float GetAxis(XboxAxis axis);
    public abstract bool GetButton(XboxButton button);
}

public class InputSourceXbox : InputSource {
    public XboxController XboxController;

    public override float GetAxis(XboxAxis axis) {
        return XCI.GetAxis(axis);
    }

    public override bool GetButton(XboxButton button) {
        return XCI.GetButton(button);
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
}

public class InputSourceChain : InputSource {
    public List<InputSource> sources = new List<InputSource>();

    public override float GetAxis(XboxAxis axis) {
        foreach (var source in sources) {
            var d = source.GetAxis(axis);
            if (d != 0f) return d;
        }
        return 0f;
    }

    public override bool GetButton(XboxButton button) { 
        foreach (var source in sources) {
            var d = source.GetButton(button);
            if (d) return d;
        }
        return false;
    }
}