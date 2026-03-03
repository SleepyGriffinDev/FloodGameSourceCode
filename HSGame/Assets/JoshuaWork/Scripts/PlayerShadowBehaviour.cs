using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadowBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform shadowTransform;

    [SerializeField]
    float shadowRayLength = 20.0f;

    Vector3 shadowDefaultPostion;

    [SerializeField]
    LayerMask shadowRayMask;
    RaycastHit shadowOut;

    private void Start()
    {
        shadowDefaultPostion = shadowTransform.localPosition;
    }

    private void Update()
    {
        //Debug.DrawLine(transform.position, transform.position + new Vector3(0, -1f * shadowRayLength, 0), Color.red); //Forward

        if (Physics.Raycast(transform.position, Vector3.down, out shadowOut, shadowRayLength, shadowRayMask)) //Forward Raycast)
        {
            if (!shadowTransform.gameObject.activeInHierarchy) shadowTransform.gameObject.SetActive(true);
            //Debug.Log(shadowOut.distance);
            shadowTransform.localPosition = new Vector3(shadowDefaultPostion.x, -shadowOut.distance + 0.01f, shadowDefaultPostion.z);
        }
        else shadowTransform.gameObject.SetActive(false);

    }
}
