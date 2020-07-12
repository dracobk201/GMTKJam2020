using System;
using UnityEngine;

public class ControllerHandler : MonoBehaviour
{
    #region Directional buttons
    [Header("Directional Buttons Variables")]
    [SerializeField] private BoolReference horizontalSinglePress = null;
    [SerializeField] private FloatReference horizontalAxis = null;
    [SerializeField] private GameEvent nonHorizontalAxisEvent = null;
    [SerializeField] private GameEvent leftButtonEvent = null;
    [SerializeField] private GameEvent rightButtonEvent = null;
    private bool _isHorizontalAxisInUse;

    [SerializeField] private BoolReference verticalSinglePress = null;
    [SerializeField] private FloatReference verticalAxis = null;
    [SerializeField] private GameEvent upButtonEvent = null;
    [SerializeField] private GameEvent downButtonEvent = null;
    [SerializeField] private GameEvent nonVerticalAxisEvent = null;
    private bool _isVerticalAxisInUse;

    [SerializeField] private GameEvent anyDirectionalAxisEvent = null;
    //[SerializeField] private GameEvent noDirectionalAxisEvent = null;

    #endregion

    #region Camera Buttons
    [SerializeField] private FloatReference mouseVerticalAxis = null;
    [SerializeField] private FloatReference mouseHorizontalAxis = null;
    [SerializeField] private GameEvent cameraLeftEvent = null;
    [SerializeField] private GameEvent cameraRightEvent = null;
    [SerializeField] private GameEvent cameraUpEvent = null;
    [SerializeField] private GameEvent cameraDownEvent = null;
    [SerializeField] private GameEvent anyCameraAxisEvent = null;
    [SerializeField] private GameEvent noMouseVerticalAxis = null;
    [SerializeField] private GameEvent noMouseHorizontalAxis = null;
    #endregion

    #region Action Buttons
    [Header("Action Buttons Variables")]
    [SerializeField] private GameEvent startButtonEvent = null;
    [SerializeField] private GameEvent quitButtonEvent = null;
    [SerializeField] private GameEvent fireButtonEvent = null;
    [SerializeField] private GameEvent secondFireButtonEvent = null;
    [SerializeField] private GameEvent confirmButtonEvent = null;

    private bool _isStartAxisInUse = false;
    private bool _isQuitAxisInUse = false;
    private bool _isFireAxisInUse = false;
    private bool _isSecondFireAxisInUse = false;
    private bool _isConfirmAxisInUse = false;
    #endregion

    [Header("UI Active Variables")]
    [SerializeField] private BoolReference uiPanelActive = null;
    [SerializeField] private GameEvent uiChangeEvent = null;

    private void Update()
    {
        CheckingVerticalAxis();
        CheckingHorizontalAxis();
        CheckingMouseVerticalAxis();
        CheckingMouseHorizontalAxis();
        CheckingStartButton();
        CheckingQuitButton();
        CheckingFireButton();
        CheckingSecondFireButton();
        CheckingConfirmButton();
    }

    #region Horizontal Functions

    private void CheckingHorizontalAxis()
    {
        var axisValue = Input.GetAxisRaw(Global.HorizontalAxis);
        if (axisValue < 0 && !_isHorizontalAxisInUse)
            LeftDirectionActions(axisValue);
        else if (axisValue > 0 && !_isHorizontalAxisInUse)
            RightDirectionActions(axisValue);
        else if (Math.Abs(axisValue) < Global.Tolerance)
            NoHorizontalActions();
    }

    private void NoHorizontalActions()
    {
        horizontalAxis.Value = 0;
        if (horizontalSinglePress.Value)
            _isHorizontalAxisInUse = false;
        nonHorizontalAxisEvent.Raise();
    }

    private void RightDirectionActions(float value)
    {
        horizontalAxis.Value = value;
        if (horizontalSinglePress.Value)
            _isHorizontalAxisInUse = true;
        rightButtonEvent.Raise();
        anyDirectionalAxisEvent.Raise();
    }

    private void LeftDirectionActions(float value)
    {
        horizontalAxis.Value = value;
        if (horizontalSinglePress.Value)
            _isHorizontalAxisInUse = true;
        leftButtonEvent.Raise();
        anyDirectionalAxisEvent.Raise();
    }

    #endregion

    #region Vertical Functions
    private void CheckingVerticalAxis()
    {
        var axisValue = Input.GetAxisRaw(Global.VerticalAxis);
        if (axisValue < 0 && !_isVerticalAxisInUse)
            DownDirectionActions(axisValue);
        else if (axisValue > 0 && !_isVerticalAxisInUse)
            UpDirectionActions(axisValue);
        else if (Math.Abs(axisValue) < Global.Tolerance)
            NoVerticalActions();
    }

    private void NoVerticalActions()
    {
        verticalAxis.Value = 0;
        if (verticalSinglePress.Value)
            _isVerticalAxisInUse = false;
        nonVerticalAxisEvent.Raise();
    }

    private void UpDirectionActions(float value)
    {
        verticalAxis.Value = value;
        if (verticalSinglePress.Value)
            _isVerticalAxisInUse = true;
        upButtonEvent.Raise();
        anyDirectionalAxisEvent.Raise();
    }

    private void DownDirectionActions(float value)
    {
        verticalAxis.Value = value;
        if (verticalSinglePress.Value)
            _isVerticalAxisInUse = true;
        downButtonEvent.Raise();
        anyDirectionalAxisEvent.Raise();
    }

    #endregion

    #region Camera Vertical Functions

    private void CheckingMouseVerticalAxis()
    {
        var mouseVerticalValue = Input.GetAxisRaw(Global.MouseVerticalAxis);
        if (mouseVerticalValue < 0)
            DownRotationActions(mouseVerticalValue);
        else if (mouseVerticalValue > 0)
            UpRotationActions(mouseVerticalValue);
        else if (Math.Abs(mouseVerticalValue) < Global.Tolerance)
            NoMouseVerticalActions();
    }

    private void NoMouseVerticalActions()
    {
        mouseVerticalAxis.Value = 0;
        noMouseVerticalAxis.Raise();
    }

    private void UpRotationActions(float mouseVerticalValue)
    {
        mouseVerticalAxis.Value = mouseVerticalValue;
        cameraUpEvent.Raise();
        anyCameraAxisEvent.Raise();
    }

    private void DownRotationActions(float mouseVerticalValue)
    {
        mouseVerticalAxis.Value = mouseVerticalValue;
        cameraDownEvent.Raise();
        anyCameraAxisEvent.Raise();
    }

    #endregion

    #region Camera Horizontal Functions

    private void CheckingMouseHorizontalAxis()
    {
        var mouseHorizontalValue = Input.GetAxisRaw(Global.MouseHorizontalAxis);
        if (mouseHorizontalValue < 0)
            LeftRotationActions(mouseHorizontalValue);
        else if (mouseHorizontalValue > 0)
            RightRotationActions(mouseHorizontalValue);
        else if (Math.Abs(mouseHorizontalValue) < Global.Tolerance)
            NoMouseHorizontalActions();
    }

    private void NoMouseHorizontalActions()
    {
        mouseHorizontalAxis.Value = 0;
        noMouseHorizontalAxis.Raise();
    }

    private void RightRotationActions(float mouseHorizontalValue)
    {
        mouseHorizontalAxis.Value = mouseHorizontalValue;
        cameraRightEvent.Raise();
        anyCameraAxisEvent.Raise();
    }

    private void LeftRotationActions(float mouseHorizontalValue)
    {
        mouseHorizontalAxis.Value = mouseHorizontalValue;
        cameraLeftEvent.Raise();
        anyCameraAxisEvent.Raise();
    }

    #endregion

    #region Action Functions

    private void CheckingStartButton()
    {
        if (Input.GetAxisRaw(Global.StartAxis) != 0 && !_isStartAxisInUse)
        {
            startButtonEvent.Raise();
            _isStartAxisInUse = true;
        }
        else if (Math.Abs(Input.GetAxisRaw(Global.StartAxis)) < Global.Tolerance)
            _isStartAxisInUse = false;
    }

    private void CheckingQuitButton()
    {
        if (Input.GetAxisRaw(Global.QuitAxis) != 0 && !_isQuitAxisInUse)
        {
            quitButtonEvent.Raise();
            _isQuitAxisInUse = true;
        }
        else if (Math.Abs(Input.GetAxisRaw(Global.QuitAxis)) < Global.Tolerance)
            _isQuitAxisInUse = false;
    }

    private void CheckingFireButton()
    {
        if (Input.GetAxisRaw(Global.FireAxis) != 0 && !_isFireAxisInUse)
        {
            fireButtonEvent.Raise();
            _isFireAxisInUse = true;
        }
        else if (Math.Abs(Input.GetAxisRaw(Global.FireAxis)) < Global.Tolerance)
            _isFireAxisInUse = false;
    }

    private void CheckingSecondFireButton()
    {
        if (Input.GetAxisRaw(Global.SecondFireAxis) != 0 && !_isSecondFireAxisInUse)
        {
            secondFireButtonEvent.Raise();
            _isSecondFireAxisInUse = true;
        }
        else if (Math.Abs(Input.GetAxisRaw(Global.SecondFireAxis)) < Global.Tolerance)
            _isSecondFireAxisInUse = false;
    }

    private void CheckingConfirmButton()
    {
        if (Input.GetAxisRaw(Global.JumpAxis) != 0 && !_isConfirmAxisInUse)
        {
            confirmButtonEvent.Raise();
            _isConfirmAxisInUse = true;
        }
        else if (Math.Abs(Input.GetAxisRaw(Global.JumpAxis)) < Global.Tolerance)
            _isConfirmAxisInUse = false;
    }

    #endregion

    #region UI Functions

    private void CheckChangeButtonUi()
    {
        if (uiPanelActive.Value)
            uiChangeEvent.Raise();
    }

    #endregion
}