using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBuildingCollapseBehaviour : MonoBehaviour
{


    [SerializeField]
    BoxCollider collapseTrigger;
    [SerializeField]
    GameObject building;

    [SerializeField]
    bool beginCollapse = false;
    bool JerkOver = false;
    bool resetStartTime = true;


    readonly float finalRotation = 30f;
    readonly float finalAltitude = 40f; //actual is zero, but 100 units are covered
    //float secondsOfInitialJerk = 0.5f;
    //float collapseDuration = 8f;
    //float collapseProgress;
    readonly float iJerkSpeed = 1f;
    readonly float finalJerkRotation = 15f;
    float startTime;
    readonly float rotSpeed = 0.5f;
    readonly float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (resetStartTime)
        {
            startTime = Time.time;
            resetStartTime = false;
        }


        if (JerkOver)
        {
            float rotCovered = (Time.time - startTime) * rotSpeed;
            float fractionOfRot = rotCovered / finalRotation;

            float distCovered = (Time.time - startTime) * speed;
            float fractionOfDist = distCovered / finalAltitude;

            beginCollapse = false;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 30), fractionOfRot);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -40f, transform.position.z), fractionOfDist);
        }

        if (beginCollapse)
        {
            collapseTrigger.gameObject.layer = 0;
            building.layer = 0;

            float rotJerkCovered = (Time.time - startTime) * iJerkSpeed;
            float fractionOfJerkRot = rotJerkCovered / finalJerkRotation;

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -3f, transform.position.z), fractionOfJerkRot);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(3f, transform.eulerAngles.y, transform.eulerAngles.z), fractionOfJerkRot);
        }
        if (transform.eulerAngles.x >= 2.9f && !JerkOver)
        {
            resetStartTime = true;
            JerkOver = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!beginCollapse)
            {
                Debug.Log("collapse");
                resetStartTime = true;

                beginCollapse = true;
            }
        }
    }
}
