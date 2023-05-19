using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    public Action<Define.KeyBoardEvent> KeyBoardAction = null;

    bool _leftPressed = false;
    bool _rightPressed = false;
    bool _keyPressed = false;
    float _mousePressedTime = 0;
    float _keyPressedTime = 0;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0)) //좌클
            {
                if (!_leftPressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.LeftPointerDown);
                    _mousePressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.LeftPress);
                _leftPressed = true;
            }
            else
            {
                if (_leftPressed)
                {
                    if (Time.time < _mousePressedTime + 0.2f) ;
                    MouseAction.Invoke(Define.MouseEvent.LeftClick);
                    MouseAction.Invoke(Define.MouseEvent.LeftPointerUp);
                }
                _leftPressed = false;
                _mousePressedTime = 0;
            }

            if (Input.GetMouseButton(1)) //우클
            {
                if (!_rightPressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.RightPointerDown);
                    _mousePressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.RightPress);
                _rightPressed = true;
            }
            else
            {
                if (_rightPressed)
                {
                    if (Time.time < _mousePressedTime + 0.2f) ;
                    MouseAction.Invoke(Define.MouseEvent.RightClick);
                    MouseAction.Invoke(Define.MouseEvent.RightPointerUp);
                }
                _rightPressed = false;
                _mousePressedTime = 0;
            }
        }

        if (KeyBoardAction != null)
        {
            if (Input.GetKey(KeyCode.Q)) //q
            {
                if (!_keyPressed)
                {
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.Q_KeyDown);
                    _keyPressedTime = Time.time;
                }
                KeyBoardAction.Invoke(Define.KeyBoardEvent.Q_KeyHold);
                _keyPressed = true;
            }
            else
            {
                if (_keyPressed)
                {
                    if (Time.time < _keyPressedTime + 0.2f) ;
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.Q_Key);
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.Q_KeyUp);
                }
                _keyPressed = false;
                _keyPressedTime = 0;
            }

            if (Input.GetKey(KeyCode.Space)) //space
            {
                if (!_keyPressed)
                {
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.SpaceDown);
                    _keyPressedTime = Time.time;
                }
                KeyBoardAction.Invoke(Define.KeyBoardEvent.SpaceHold);
                _keyPressed = true;
            }
            else
            {
                if (_keyPressed)
                {
                    if (Time.time < _keyPressedTime + 0.2f) ;
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.Space);
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.SpaceUp);
                }
            }
            if (Input.GetKey(KeyCode.W)) //w
            {
                if (!_keyPressed)
                {
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.W_KeyDown);
                    _keyPressedTime = Time.time;
                }
                KeyBoardAction.Invoke(Define.KeyBoardEvent.W_keyHold);
                Debug.Log("W HOLDING!");
                _keyPressed = true;
            }
            else
            {
                if (_keyPressed)
                {
                    if (Time.time < _keyPressedTime + 0.2f) ;
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.W_Key);
                    KeyBoardAction.Invoke(Define.KeyBoardEvent.W_KeyUp);
                }

            }

            _keyPressed = false;
            _keyPressedTime = 0;
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
        KeyBoardAction = null;
    }
}
