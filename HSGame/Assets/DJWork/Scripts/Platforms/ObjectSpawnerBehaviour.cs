using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject[] gameObjects; // list of objects it would pick from
    [SerializeField]
    float timer; // timer
    [SerializeField]
    public int randomPrefabPicker; // picks a random object from list
    [SerializeField]
    Transform spawnPosition; // postion of spawner
    [SerializeField]
    public Vector3 targetPosition = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    [SerializeField]
    void Start()
    {
        timer = 10; // picks a random number from 1 to 10
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnObject();
            timer = 10;
        }
    }

    void SpawnObject()
    {
        // detects if there are any prefabs
        if (gameObjects.Length == 0)
        {
            Debug.LogWarning("No prefabs assigned to prefabsToSpawn array!");
            return;
        }
        else
        {
            randomPrefabPicker = Random.Range(0, 2);

            Instantiate(gameObjects[randomPrefabPicker], targetPosition, Quaternion.identity);
        }
    }
}
