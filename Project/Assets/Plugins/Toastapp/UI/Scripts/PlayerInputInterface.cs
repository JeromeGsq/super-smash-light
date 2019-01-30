using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInputInterface : MonoBehaviour {

    public abstract float GetAxis(string _axisName);

    public abstract bool GetButtonDown(string _buttonName);

}
