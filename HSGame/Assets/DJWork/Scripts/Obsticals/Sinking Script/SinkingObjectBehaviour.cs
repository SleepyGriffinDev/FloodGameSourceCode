using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SinkingObjectBehaviour : MonoBehaviour
{
    [SerializeField] float sink = 0.5f;
    bool setter = false;
    [SerializeField] float timer = 0;
    [SerializeField] public Vector3 startPosition = new Vector3(0, 0, 0);

    // Update is called once per frame
    void Update()
    {

        if (setter == true)
        {
            transform.Translate(Vector3.back * sink * Time.deltaTime);

            timer += Time.deltaTime;

            if (timer >= 9)
            {
                transform.Translate(startPosition);
                setter = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            setter = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            setter = false;
            transform.Translate(startPosition);
        }
    }
}
