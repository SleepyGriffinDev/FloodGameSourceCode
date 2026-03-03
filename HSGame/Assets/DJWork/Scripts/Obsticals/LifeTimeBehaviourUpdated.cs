using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeBehaviourUpdated : MonoBehaviour
{
    [SerializeField]
    float lifeTime = 5;
    

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Destroy(gameObject);
        }
    }
}
