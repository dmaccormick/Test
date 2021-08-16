using UnityEngine;
using System.Runtime.InteropServices;

//List of all of the buttons on the controller
//Matched to the index in the DLL
public enum Button
{
    //Face Buttons
    A,
    B,
    X,
    Y,

    //Bumpers
    LB,
    RB,

    //Misc Buttons
    Select,
    Start,
    LS,
    RS,

    //Dpad
    DPAD_Up,
    DPAD_Right,
    DPAD_Down,
    DPAD_Left,

    NUM_BUTTONS
}


//DLL Bridge Class
//Also automatically updates all of the controllers as needed
//Need to execute in edit mode so the editor window works there too
[ExecuteInEditMode]
public class ControllerManager : MonoBehaviour
{
    //--- DLL Entry Points ---//
    [DllImport("ControllerDLL")]
    public static extern void initializeControllers();

    [DllImport("ControllerDLL")]
    public static extern void updateControllers(float _dt);

    [DllImport("ControllerDLL")]
    public static extern void vibrateController(int _id, float _duration, float _leftPower, float _rightPower);

    [DllImport("ControllerDLL")]
    public static extern bool getButton(int _id, int _button);

    [DllImport("ControllerDLL")]
    public static extern float getLeftStick_X(int _id);

    [DllImport("ControllerDLL")]
    public static extern float getLeftStick_Y(int _id);

    [DllImport("ControllerDLL")]
    public static extern float getRightStick_X(int _id);

    [DllImport("ControllerDLL")]
    public static extern float getRightStick_Y(int _id);

    [DllImport("ControllerDLL")]
    public static extern float getLeftTrigger(int _id);

    [DllImport("ControllerDLL")]
    public static extern float getRightTrigger(int _id);

    [DllImport("ControllerDLL")]
    public static extern bool getIsConnected(int _id);



    //--- Unity Functions ---//
    void Start()
    {
        //Initialize all of the controllers in the DLL. Only needs to be called once
        initializeControllers();
    }

    void Update()
    {
        //Update all of the controllers so they have the new values from XInput
        updateControllers(Time.deltaTime);
    }
}
