using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JBMovingObstacleBehaviour : MonoBehaviour
{
    /*
     
        Set in Inspector to choose the needed movement system

        HORIZONTAL - move back and forth on the x-axis: Uses speed & timer
        VERTICAL - move up and down on the y-axis: Uses speed & timer
        FLOATINGX - sways like an object floating in water (Vertical motion, rotation on the X-Axis): 
        FLOATINGZ - sways like an object floating in water (Vertical motion, rotation on the Z-Axis): 
     
        Objects this go one should have the parent (what this is linked to), be an empty game object, 
        with the object geometry as a child that is referenced here
        
     */

    enum movementOptions { 
    
        HORIZONTAL,
        VERTICAL,
        FLOATINGX,
        FLOATINGZ,

    };

    [SerializeField]
    GameObject objectGeometry;


    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    float timer = 0;
    [SerializeField]
    float timerMax = 5;
    [SerializeField]
    movementOptions motion = movementOptions.HORIZONTAL;

    // Update is called once per frame
    void FixedUpdate()
    {

        //Horizontal Movement
        if (motion == movementOptions.HORIZONTAL)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

            // Timer
            timer += Time.deltaTime;

            if (timer >= timerMax)
            {
                speed *= -1;
                timer = 0;
            }
        }
        //Vertical Movement
        else if (motion == movementOptions.VERTICAL) {

            transform.position += new Vector3(0, speed * Time.deltaTime, 0);

            // Timer
            timer += Time.deltaTime;

            if (timer >= timerMax)
            {
                speed *= -1;
                timer = 0;
            }
        }
        //Floating, X Rotation Movement
        //Default Movement
        else
        {
            Debug.Log("Its not done yet :(");
        }
    }
}
