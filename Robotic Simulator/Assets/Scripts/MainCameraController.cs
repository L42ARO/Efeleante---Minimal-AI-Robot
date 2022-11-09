using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public GameObject activeBot;
    GameObject CameraPosition;
    void FixedUpdate()
    {
        if (activeBot != null)
        {
            CameraPosition = activeBot.GetComponent<RobotController>().CameraPosition;
            transform.position = CameraPosition.transform.position;
            transform.rotation = CameraPosition.transform.rotation;
        }
    }
}
