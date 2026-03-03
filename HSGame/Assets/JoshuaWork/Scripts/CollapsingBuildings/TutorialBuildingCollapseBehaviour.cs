using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuildingCollapseBehaviour : MonoBehaviour
{


    [SerializeField]
    GameObject collapseTrigger;

    [SerializeField]
    Rigidbody fallingPlankRB;
    [SerializeField]
    BoxCollider fallingPlank;

    [SerializeField]
    bool beginCollapse = false;
    bool JerkOver = false;
    bool resetStartTime = true;


    readonly float finalRotation = 90f;
    readonly float finalAltitude = 100f; //actual is -100, but 100 units are covered
    //float secondsOfInitialJerk = 0.5f;
    //float collapseDuration = 8f;
    //float collapseProgress;
    readonly float iJerkSpeed = 1f;
    readonly float finalJerkRotation = 15f;
    float startTime;
    readonly float rotSpeed = .01f;
    readonly float speed = .01f;
    
    
    //float disablePlankTimerMax = 1.0f;
    //float disablePlankTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (resetStartTime)
        {
            startTime = Time.time;
            resetStartTime = false;
        }


        if (transform.eulerAngles.x >= 15 && transform.eulerAngles.z < 90)
        {
            float rotCovered = (Time.time - startTime) * rotSpeed;
            float fractionOfRot = rotCovered / finalRotation;

            float distCovered = (Time.time - startTime) * speed;
            float fractionOfDist = distCovered / finalAltitude;

            beginCollapse = false;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90), fractionOfRot);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -100f, transform.position.z), fractionOfDist);
        }

        if (beginCollapse)
        {

            fallingPlank.gameObject.layer = 0;
            fallingPlank.isTrigger = true;

            //if (disablePlankTimer < disablePlankTimerMax)
            //{
            //    disablePlankTimer += Time.deltaTime;
            //}
            //else
            //{
            //    fallingPlank.gameObject.layer = 0;
            //    fallingPlank.isTrigger = true;
            //}
                float rotJerkCovered = (Time.time - startTime) * iJerkSpeed;
            float fractionOfJerkRot = rotJerkCovered / finalJerkRotation;

            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(15, transform.eulerAngles.y, transform.eulerAngles.z), fractionOfJerkRot);
            fallingPlankRB.isKinematic = false;
        }
        if (transform.eulerAngles.x >= 15 && !JerkOver)
        {
            resetStartTime = true;
            JerkOver = true;
        }

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collapse");
            if (!beginCollapse)
            {
                resetStartTime = true;

                beginCollapse = true;
                collapseTrigger.SetActive(false);
            }
        }
     }

}
