using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltrasonicController : MonoBehaviour
{
    public float maxDistance=40;//Length in decimeters
    public float distance=20;
    public UltrasonicEyes eyes;
    RaycastHit hit;
    void Update()
    {
        float distL = UseSonar(eyes.left);
        float distR = UseSonar(eyes.right);
        distance = Mathf.Min(distL, distR);
        Debug.DrawRay(transform.position, transform.forward * distance, Color.cyan);

    }
    float UseSonar(GameObject eye){
        Vector3 pos = eye.transform.position;
        Ray ray = new Ray(pos, transform.forward);
        float dist = maxDistance;
        if(Physics.Raycast(ray, out hit, maxDistance)){
            if (hit.collider.tag=="Env"){
                dist= hit.distance;//Distance is in decimeters, divide by 10 to get meters
            }
        }
        Debug.DrawRay(pos, transform.forward * dist, Color.blue);
        return dist;
    }
}

[System.Serializable]
public class UltrasonicEyes
{
    public GameObject right, left;
}
