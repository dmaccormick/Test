using UnityEngine;
using UnityEditor;

public class Controller_Window : EditorWindow
{
    //--- Private Variables ---//
    //Data
    private int id = 0;
    private Vector2 scrollPos;
    private GameObject controllerManagerPrefab;

    //Drawing offset
    float MAX_OFFSET_THUMBSTICK = 15.0f;
    float MAX_TRIGGER_SCALE = 0.8f;

    //Textures
    private Texture tex_Controller;
    private Texture tex_LS_Normal;
    private Texture tex_LS_Pressed;
    private Texture tex_RS_Normal;
    private Texture tex_RS_Pressed;
    private Texture tex_LT;
    private Texture tex_RT;
    private Texture tex_A;
    private Texture tex_B;
    private Texture tex_X;
    private Texture tex_Y;
    private Texture tex_LB;
    private Texture tex_RB;
    private Texture tex_Select;
    private Texture tex_Start;
    private Texture tex_Dpad_Up;
    private Texture tex_Dpad_Right;
    private Texture tex_Dpad_Down;
    private Texture tex_Dpad_Left;



    //--- Unity Functions ---//
    [MenuItem("Window/Custom/Controller Values")]
    public static void ShowWindow()
    {
        //Show the window in the Unity editor interface
        EditorWindow window = EditorWindow.GetWindow<Controller_Window>();
        window.titleContent = new GUIContent("Controller Values", "Visualize the inputs from connected Xbox 360 controllers");
        window.Show();
    }

    private void OnEnable()
    {
        //Load in the controller manager prefab
        loadControllerManager();

        //Load all of the necessary textures for the display. They are in the Editor Default Resources folder in Assets
        loadTextures();
    }

    private void Update()
    {
        //If there is not a controller manager in the scene, spawn one
        //Allows this plugin to be completely drag and drop
        if (GameObject.FindObjectOfType<ControllerManager>() == null)
        {
            //Spawn the object
            GameObject obj = Instantiate(controllerManagerPrefab);

            //Rename to avoid the (clone) suffix
            obj.name = "ControllerManager";
        }

        //Force the window to redraw 
        this.Repaint();
    }

    private void OnGUI()
    {
        //Put everything into a scrollable view
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, null);

        //Draw the controller graphic
        drawController();

        //Create text field to select which controller should be visualized
        displayIDField();

        //Show if the controller is connected or not
        bool isConnected = ControllerManager.getIsConnected(id);
        displayBoolValue("Is Connected:", isConnected);
        if (!isConnected)
            GUILayout.Label("Note: Disconnected Controllers will show all 0 and false values!", EditorStyles.helpBox);

        //Output all of the fields from the requested controller as labels
        //Thumbsticks
        GUILayout.Space(20);
        GUILayout.Label("Thumbsticks:", EditorStyles.boldLabel);
        displayFloatPair("LS_X:", ControllerManager.getLeftStick_X(id), "LS_Y:", ControllerManager.getLeftStick_Y(id));
        displayFloatPair("RS_X:", ControllerManager.getRightStick_X(id), "RS_Y:", ControllerManager.getRightStick_Y(id));

        //Triggers
        GUILayout.Space(20);
        GUILayout.Label("Triggers:", EditorStyles.boldLabel);
        displayFloatValue("LT:", ControllerManager.getLeftTrigger(id));
        displayFloatValue("RT:", ControllerManager.getRightTrigger(id));

        //Face Buttons
        GUILayout.Space(20);
        GUILayout.Label("Face Buttons:", EditorStyles.boldLabel);
        displayBoolValue("A:", ControllerManager.getButton(id, (int)Button.A));
        displayBoolValue("B:", ControllerManager.getButton(id, (int)Button.B));
        displayBoolValue("X:", ControllerManager.getButton(id, (int)Button.X));
        displayBoolValue("Y:", ControllerManager.getButton(id, (int)Button.Y));

        //Bumpers
        GUILayout.Space(20);
        GUILayout.Label("Bumpers:", EditorStyles.boldLabel);
        displayBoolValue("LB:", ControllerManager.getButton(id, (int)Button.LB));
        displayBoolValue("RB:", ControllerManager.getButton(id, (int)Button.RB));

        //Misc Buttons
        GUILayout.Space(20);
        GUILayout.Label("Misc. Buttons:", EditorStyles.boldLabel);
        displayBoolValue("SELECT:", ControllerManager.getButton(id, (int)Button.Select));
        displayBoolValue("START:", ControllerManager.getButton(id, (int)Button.Start));
        displayBoolValue("LS:", ControllerManager.getButton(id, (int)Button.LS));
        displayBoolValue("RS:", ControllerManager.getButton(id, (int)Button.RS));

        //DPAD
        GUILayout.Space(20);
        GUILayout.Label("DPAD:", EditorStyles.boldLabel);
        displayBoolValue("UP:", ControllerManager.getButton(id, (int)Button.DPAD_Up));
        displayBoolValue("RIGHT:", ControllerManager.getButton(id, (int)Button.DPAD_Right));
        displayBoolValue("DOWN:", ControllerManager.getButton(id, (int)Button.DPAD_Down));
        displayBoolValue("LEFT:", ControllerManager.getButton(id, (int)Button.DPAD_Left));

        //End the scroll view
        EditorGUILayout.EndScrollView();
    }

    private void displayIDField()
    {
        //Display the ID field which is a string and int pair, laid out horizontally
        GUILayout.Label("Controller Information:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Controller ID:", EditorStyles.label);
        id = EditorGUILayout.IntField(id, EditorStyles.textField);
        EditorGUILayout.EndHorizontal();
    }

    private void displayFloatPair(string _titleX, float _valueX, string _titleY, float _valueY)
    {
        //Display two sets of floats with two titles, useful for left and right sticks
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(_titleX, _valueX.ToString());
        EditorGUILayout.LabelField(_titleY, _valueY.ToString());
        EditorGUILayout.EndHorizontal();
    }

    private void displayFloatValue(string _title, float _value)
    {
        //Display float with title beside it, useful for triggers
        EditorGUILayout.LabelField(_title, _value.ToString());
    }

    private void displayBoolValue(string _title, bool _value)
    {
        //Display bool with title beside it, useful for buttons
        EditorGUILayout.LabelField(_title, _value.ToString());
    }

    private void drawController()
    {
        //Always the controller background
        GUI.DrawTexture(new Rect(0.0f, 0.0f, 512.0f, 512.0f), tex_Controller, ScaleMode.ScaleToFit);

        //Always draw the thumbsticks ---
        //Left Stick
        Vector2 LS = new Vector2(ControllerManager.getLeftStick_X(id), ControllerManager.getLeftStick_Y(id));
        Vector2 offsetLS = new Vector2(MAX_OFFSET_THUMBSTICK * LS.x, -MAX_OFFSET_THUMBSTICK * LS.y); //Negative Y value because it starts in top left

        //Draw the pressed version or the regular version
        if (ControllerManager.getButton(id, (int)Button.LS))
            GUI.DrawTexture(new Rect(86.0f + offsetLS.x, 142.0f + offsetLS.y, 100.0f, 100.0f), tex_LS_Pressed); //Clicked in left stick
        else
            GUI.DrawTexture(new Rect(86.0f + offsetLS.x, 142.0f + offsetLS.y, 100.0f, 100.0f), tex_LS_Normal); //Normal left stick

        //Right Stick
        Vector2 RS = new Vector2(ControllerManager.getRightStick_X(id), ControllerManager.getRightStick_Y(id));
        Vector2 offsetRS = new Vector2(MAX_OFFSET_THUMBSTICK * RS.x, -MAX_OFFSET_THUMBSTICK * RS.y); //Floats come from the controller stick values

        if (ControllerManager.getButton(id, (int)Button.RS))
            GUI.DrawTexture(new Rect(268.0f + offsetRS.x, 214.0f + offsetRS.y, 100.0f, 100.0f), tex_RS_Pressed); //Clicked in right stick
        else
            GUI.DrawTexture(new Rect(268.0f + offsetRS.x, 214.0f + offsetRS.y, 100.0f, 100.0f), tex_RS_Normal); //Normal right stick

        //Always draw the triggers ---
        //Left Trigger -> Draw scaled and moved to make the "pressing down" effect
        float LT = ControllerManager.getLeftTrigger(id);
        float LT_ScaleOffset = 1.0f - (LT * MAX_TRIGGER_SCALE); 
        float LT_YPosOffset = 75.0f * (1.0f - LT_ScaleOffset);
        GUI.DrawTexture(new Rect(85.0f, 10.0f + LT_YPosOffset, 75.0f, 75.0f * LT_ScaleOffset), tex_LT); //Left Trigger

        //Right Trigger -> Draw scaled and moved to make the "pressing down" effect
        float RT = ControllerManager.getRightTrigger(id);
        float RT_ScaleOffset = 1.0f - (RT * MAX_TRIGGER_SCALE); 
        float RT_YPosOffset = 75.0f * (1.0f - RT_ScaleOffset);
        GUI.DrawTexture(new Rect(350.0f, 10.0f + RT_YPosOffset, 75.0f, 75.0f * RT_ScaleOffset), tex_RT); //Right Trigger

        //Conditionally draw the facebuttons
        if (ControllerManager.getButton(id, (int)Button.A))
            GUI.DrawTexture(new Rect(355.0f, 200.0f, 50.0f, 50.0f), tex_A); //A Button
        if (ControllerManager.getButton(id, (int)Button.B))
            GUI.DrawTexture(new Rect(390.0f, 170.0f, 50.0f, 50.0f), tex_B); //B Button
        if (ControllerManager.getButton(id, (int)Button.X))
            GUI.DrawTexture(new Rect(320.0f, 170.0f, 50.0f, 50.0f), tex_X); //X Button
        if (ControllerManager.getButton(id, (int)Button.Y))
            GUI.DrawTexture(new Rect(353.0f, 138.0f, 50.0f, 50.0f), tex_Y); //Y Button

        //Conditionally draw the bumpers
        if (ControllerManager.getButton(id, (int)Button.LB))
            GUI.DrawTexture(new Rect(75.0f, 65.0f, 128.0f, 100.0f), tex_LB); //Left Bumper
        if (ControllerManager.getButton(id, (int)Button.RB))
            GUI.DrawTexture(new Rect(310.0f, 65.0f, 128.0f, 100.0f), tex_RB); //Right Bumper

        //Conditionally draw the misc buttons (except for the thumbsticks, as they are drawn earlier)
        if (ControllerManager.getButton(id, (int)Button.Select))
            GUI.DrawTexture(new Rect(205.0f, 175.0f, 35.0f, 35.0f), tex_Select); //Select
        if (ControllerManager.getButton(id, (int)Button.Start))
            GUI.DrawTexture(new Rect(273.0f, 175.0f, 35.0f, 35.0f), tex_Start); //Start

        //Conditionally draw the dpad buttons
        if (ControllerManager.getButton(id, (int)Button.DPAD_Up))
            GUI.DrawTexture(new Rect(130.0f, 200.0f, 128.0f, 128.0f), tex_Dpad_Up); //Dpad up
        if (ControllerManager.getButton(id, (int)Button.DPAD_Right))
            GUI.DrawTexture(new Rect(130.0f, 200.0f, 128.0f, 128.0f), tex_Dpad_Right); //Dpad right
        if (ControllerManager.getButton(id, (int)Button.DPAD_Down))
            GUI.DrawTexture(new Rect(130.0f, 200.0f, 128.0f, 128.0f), tex_Dpad_Down); //Dpad down
        if (ControllerManager.getButton(id, (int)Button.DPAD_Left))
            GUI.DrawTexture(new Rect(130.0f, 200.0f, 128.0f, 128.0f), tex_Dpad_Left); //Dpad right

        //Space out the other fields as needed so they are below the controller image. (Image is 512x512)
        GUILayout.Space(512.0f);
    }

    private void loadControllerManager()
    {
        //Load the controller manager from the editor folder
        controllerManagerPrefab = (GameObject)EditorGUIUtility.Load("ControllerManager.prefab");
    }

    private void loadTextures()
    {
        //Controller
        //Controller Texture From: https://thewolfbunny.deviantart.com/art/Xbox-One-Controller-Template-558306289
        tex_Controller = (Texture)EditorGUIUtility.Load("tex_Controller.png");

        //Thumbsticks
        //All Button Textures From: https://opengameart.org/content/free-keyboard-and-controllers-prompts-pack
        //Some were edited but most are the originals
        tex_LS_Normal = (Texture)EditorGUIUtility.Load("tex_LS_Normal.png");
        tex_LS_Pressed = (Texture)EditorGUIUtility.Load("tex_LS_Pressed.png");
        tex_RS_Normal = (Texture)EditorGUIUtility.Load("tex_RS_Normal.png");
        tex_RS_Pressed = (Texture)EditorGUIUtility.Load("tex_RS_Pressed.png");

        //Triggers
        tex_LT = (Texture)EditorGUIUtility.Load("tex_LT.png");
        tex_RT = (Texture)EditorGUIUtility.Load("tex_RT.png");

        //Face buttons
        tex_A = (Texture)EditorGUIUtility.Load("tex_A.png");
        tex_B = (Texture)EditorGUIUtility.Load("tex_B.png");
        tex_X = (Texture)EditorGUIUtility.Load("tex_X.png");
        tex_Y = (Texture)EditorGUIUtility.Load("tex_Y.png");

        //Bumpers
        tex_LB = (Texture)EditorGUIUtility.Load("tex_LB.png");
        tex_RB = (Texture)EditorGUIUtility.Load("tex_RB.png");

        //Misc Buttons
        tex_Select = (Texture)EditorGUIUtility.Load("tex_Select.png");
        tex_Start = (Texture)EditorGUIUtility.Load("tex_Start.png");

        //DPAD
        tex_Dpad_Up = (Texture)EditorGUIUtility.Load("tex_Dpad_Up.png");
        tex_Dpad_Right = (Texture)EditorGUIUtility.Load("tex_Dpad_Right.png");
        tex_Dpad_Down = (Texture)EditorGUIUtility.Load("tex_Dpad_Down.png");
        tex_Dpad_Left = (Texture)EditorGUIUtility.Load("tex_Dpad_Left.png");
    }
}
