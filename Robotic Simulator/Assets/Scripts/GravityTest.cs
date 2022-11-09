using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTest : MonoBehaviour
{
    float startTime = 0;
    public Rigidbody rb;
    bool collided=false;
    bool started = false;
    void FixedUpdate()
    {
        StartCoroutine(Wait2Drop());
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collided){
            var colTime= System.DateTime.Now.Millisecond;
            collided=true;
            Debug.Log("Start time: " + startTime + " Collision time: " + colTime);
            Debug.Log("Collision time: " + (colTime - startTime));
        }
    }
    IEnumerator Wait2Drop()
    {
        yield return new WaitForSeconds(2);
        if(!started){
            startTime = System.DateTime.Now.Millisecond;
            started=true;
            rb.useGravity = true;
            Debug.Log("Drop!");
            
        }
    }
}
