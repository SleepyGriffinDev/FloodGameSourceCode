using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMoverBehaviourUpdated : MonoBehaviour
{
    [SerializeField]
    public Vector3 checkPointPosition = new Vector3(0, 0, 0);
    [SerializeField]
    GameObject checkPoint;


    private void Start()
    {
        checkPointPosition = transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            checkPoint.transform.position = checkPointPosition;
            Destroy(gameObject);
        }
    }
}
