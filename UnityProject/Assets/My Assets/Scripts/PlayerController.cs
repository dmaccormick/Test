using UnityEngine;
using UnityEngine.UI;

//Script to move the capsules in the scene
//The number behind the capsule represents the controller number that controls it
//Every button on the controller has some functionality that affects the capsule
public class PlayerController : MonoBehaviour
{
    //--- Public References ---//
    [Header("Public References")]
    public Text controllerNumberDisplay;
    public Mesh[] meshes;



    //--- Public Variables ---//
    [Header("Public Variables")]
    public float movementSpeed;
    public float rotationSpeed;



    //--- Private References ---//
    private MeshFilter filter;
    private Renderer renderer;
    private Material material;



    //--- Private Variables ---//
    private static int numPlayers = 0;
    private int thisController;



    //--- Unity Functions ---//
    void Start()
    {
        //Init the private references
        filter = GetComponent<MeshFilter>();
        renderer = GetComponent<Renderer>();
        material = renderer.material;

        //Init the private variables
        thisController = numPlayers;
        numPlayers++;

        //Show the controller ID in the scene
        controllerNumberDisplay.text = thisController.ToString();
    }

    void Update()
    {
        //Move using the left stick ---
        Vector3 movementDir = new Vector3(ControllerManager.getLeftStick_X(thisController), 0.0f, ControllerManager.getLeftStick_Y(thisController)) * Time.deltaTime;
        transform.Translate(movementDir * movementSpeed);


        //Rotate on the Y axis using the right stick ---
        float rotY = ControllerManager.getRightStick_X(thisController);
        transform.Rotate(Vector3.up * rotY * rotationSpeed * Time.deltaTime);


        //Rotate Z axis using the bumpers ---
        if (ControllerManager.getButton(thisController, (int)Button.RB))
            transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
        else if (ControllerManager.getButton(thisController, (int)Button.LB))
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);


        //Scale up using the right trigger ---
        float scaleIncrease = ControllerManager.getRightTrigger(thisController) * 0.01f;
        transform.localScale += new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);


        //Scale down using the left trigger ---
        float scaleDecrease = ControllerManager.getLeftTrigger(thisController) * 0.01f;
        transform.localScale -= new Vector3(scaleDecrease, scaleDecrease, scaleDecrease);


        //Change colour using the face buttons. Also vibrate the controller ---
        if (ControllerManager.getButton(thisController, (int)Button.A))
        {
            material.color = Color.green;
            ControllerManager.vibrateController(thisController, 0.15f, 0.5f, 0.5f);
        }
        else if (ControllerManager.getButton(thisController, (int)Button.B))
        {
            material.color = Color.red;
            ControllerManager.vibrateController(thisController, 0.15f, 0.5f, 0.5f);
        }
        else if (ControllerManager.getButton(thisController, (int)Button.X))
        {
            material.color = Color.blue;
            ControllerManager.vibrateController(thisController, 0.15f, 0.5f, 0.5f);
        }
        else if (ControllerManager.getButton(thisController, (int)Button.Y))
        {
            material.color = Color.yellow;
            ControllerManager.vibrateController(thisController, 0.15f, 0.5f, 0.5f);
        }


        //Change mesh using the dpad ---
        if (ControllerManager.getButton(thisController, (int)Button.DPAD_Up))
            filter.mesh = meshes[0];
        else if (ControllerManager.getButton(thisController, (int)Button.DPAD_Right))
            filter.mesh = meshes[1];
        else if (ControllerManager.getButton(thisController, (int)Button.DPAD_Down))
            filter.mesh = meshes[2];
        else if (ControllerManager.getButton(thisController, (int)Button.DPAD_Left))
            filter.mesh = meshes[3];


        //Move up and down using select and start ---
        if (ControllerManager.getButton(thisController, (int)Button.Select))
            transform.Translate(-Vector3.up * movementSpeed * Time.deltaTime);
        else if (ControllerManager.getButton(thisController, (int)Button.Start))
            transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);


        //Reset rotation by clicking the left stick in ---
        if (ControllerManager.getButton(thisController, (int)Button.LS))
            transform.rotation = Quaternion.identity;


        //Reset the height by clicking the right stick in ---
        if (ControllerManager.getButton(thisController, (int)Button.RS))
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }
}
