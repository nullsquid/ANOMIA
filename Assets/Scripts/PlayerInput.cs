using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {


    private float _h;
    private float _v;
    private bool _isInputEnabled = false;


    public float H
    {
        get
        {
            return _h;
        }
    } 
    public float V
    {
        get
        {
            return _v;
        }
    }
    public bool InputEnabled
    {
        get
        {
            return _isInputEnabled;
        }

        set
        {
            _isInputEnabled = value;
        }
    }

    private void Update() {

    }
    public void GetKeyInput() {

        if (_isInputEnabled == true) {
            _h = Input.GetAxisRaw("Horizontal");
            _v = Input.GetAxisRaw("Vertical");
        }
        else {
            _h = 0f;
            _v = 0f;
        }
    }
}
