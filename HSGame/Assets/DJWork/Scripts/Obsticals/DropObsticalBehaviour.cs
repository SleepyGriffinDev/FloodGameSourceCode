using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObsticalBehaviour : MonoBehaviour
{

    [SerializeField]
    float timer = 100.0f;
    [SerializeField]
    GameObject Object;
    [SerializeField]
    Transform spawner;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            GameObject newObject = Instantiate(Object, spawner.position, spawner.rotation);
            timer += 5;
        }
    }
}
