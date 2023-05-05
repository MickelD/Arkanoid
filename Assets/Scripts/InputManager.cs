using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public float xInput;

    [SerializeField] private string _xAxisName;

    private void Update()
    {
        xInput = Input.GetAxisRaw(_xAxisName);
    }
}
