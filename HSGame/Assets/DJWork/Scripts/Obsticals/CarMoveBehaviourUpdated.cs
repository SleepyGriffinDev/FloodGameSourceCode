using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoveBehaviourUpdated : MonoBehaviour
{
    //[SerializeField]
    //float speed = 0.5f;
    [SerializeField]
    float back = -0.5f;
    [SerializeField]
    float timer = 0;
    [SerializeField]
    public Vector3 targetPosition = new Vector3(-254, 23, 0);

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(back * Time.deltaTime, 0, 0);

        // Timer
        timer += Time.deltaTime;

        if (timer >= 5)
        {
            transform.position = targetPosition;
            timer = 0;
            Debug.Log("ANYTHING!");
        }
    }
}
