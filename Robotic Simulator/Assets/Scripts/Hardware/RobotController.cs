using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    #region Variables
    public GameObject CameraPosition;
    public Tracks Tracks;
    public Rigidbody rb;
    public Sensors sensors;
    float initForce = 40f; //NOTE: Force is in Newtons
    Controller controller;
    float trackMaxVelocity = 2f;
    #endregion
    #region Unity Methods
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 10;
        controller = new Controller();
    }
    void Update()
    {
        controller.KeyBoardListener();
        UpdateSensors();
        InterpretetSensors();
    }
    void FixedUpdate()
    {
        StartCoroutine(TrueUpdate());
    }
    #endregion
    //NOTE: Any of the code that will go in fixed updte write it here:
    IEnumerator TrueUpdate()
    {
        yield return new WaitForSeconds(1.5f);
        MoveRobot();
    }
    void MoveRobot()
    {
        if (!controller.IsActive()) return;
        Vector3 pushPos = new Vector3();
        Vector3 direction = new Vector3();
        var activeTracks = new Dictionary<string, bool>();
        activeTracks.Add("Left", false);
        activeTracks.Add("Right", false);
        if (controller.leftFwd)
        {
            pushPos = Tracks.left.transform.position;
            direction = transform.forward;
            rb.AddForceAtPosition(direction * initForce, pushPos);
            activeTracks["Left"] = true;
        }
        if (controller.rightFwd)
        {
            pushPos = Tracks.right.transform.position;
            direction = transform.forward;
            rb.AddForceAtPosition(direction * initForce, pushPos);
            activeTracks["Right"] = true;
        }
        if (controller.leftBack)
        {
            pushPos = Tracks.left.transform.position;
            direction = -transform.forward;
            rb.AddForceAtPosition(direction * initForce, pushPos);
            activeTracks["Left"] = true;
        }
        if (controller.rightBack)
        {
            pushPos = Tracks.right.transform.position;
            direction = -transform.forward;
            rb.AddForceAtPosition(direction * initForce, pushPos);
            activeTracks["Right"] = true;
        }
        var maxVelocity = (activeTracks["Left"] && activeTracks["Right"]) ? trackMaxVelocity * 2 : trackMaxVelocity;
        var maxAngularVelocity = trackMaxVelocity;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxAngularVelocity);
    }
    void UpdateSensors()
    {
        sensors.GetUltrasonicDistance();
        sensors.GetLightIntensities();
    }
    void InterpretetSensors()
    {
        // ClearLog();
        Debug.Log("Ultrasonic: " + sensors.ultrasonicDistance);
        Debug.Log("FL Light: " + sensors.Light.intensity.frontLeft + " FR Light: " + sensors.Light.intensity.frontRight);
        Debug.Log("BL Light: " + sensors.Light.intensity.backLeft + " BR Light: " + sensors.Light.intensity.backRight);
    }
    public void ClearLog() //you can copy/paste this code to the bottom of your script
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
#region Custom Classes
class Controller
{
    public bool rightFwd, rightBack, leftFwd, leftBack;
    public Vector3 movement;
    public Controller()
    {
        rightFwd = false;
        rightBack = false;
        leftFwd = false;
        leftBack = false;
    }
    public void KeyBoardListener()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            leftFwd = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            leftFwd = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rightFwd = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            rightFwd = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            leftBack = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            leftBack = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rightBack = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            rightBack = false;
        }
    }
    public bool IsActive()
    {
        return rightBack || rightFwd || leftBack || leftFwd;
    }
}
[System.Serializable]
public class Sensors
{
    public GameObject Ultrasonic;
    [System.NonSerialized] public float ultrasonicDistance;
    // public LightGroup Lights;
    public LightGroup Light;
    public void GetUltrasonicDistance()
    {
        var script = Ultrasonic.GetComponent<UltrasonicController>();
        ultrasonicDistance = script.distance;
    }
    public void GetLightIntensities()
    {
        Light.intensity.backLeft = Light.backLeft.GetComponent<LightSensorController>().GetLightIntensity();
        Light.intensity.backRight = Light.backRight.GetComponent<LightSensorController>().GetLightIntensity();
        Light.intensity.frontLeft = Light.frontLeft.GetComponent<LightSensorController>().GetLightIntensity();
        Light.intensity.frontRight = Light.frontRight.GetComponent<LightSensorController>().GetLightIntensity();
    }
}
#endregion
#region Custom Structures
[System.Serializable]
public struct Tracks
{
    public GameObject left, right;
}
[System.Serializable]
public struct LightGroup
{
    public GameObject backLeft, frontLeft, frontRight, backRight;
    [System.NonSerialized] public IntensityValues intensity;
}
public struct IntensityValues
{
    public float backLeft, backRight, frontLeft, frontRight;
}
#endregion


