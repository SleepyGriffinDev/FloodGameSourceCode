using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JBDropObstacleBehaviour : MonoBehaviour
{
    [SerializeField]
    float timer_length = 3.0f;
    float timer = 3.0f;
    [SerializeField]
    GameObject Object;
    
    private Transform spawner;

    void Start()
    {
        spawner = this.gameObject.transform;

        timer = timer_length;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            GameObject newObject = Instantiate(Object, spawner.position, spawner.rotation);
            timer = timer_length;
        }
    }
}
