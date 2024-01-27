using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private float targetY;
    public float minTargetY;
    public float maxTargetY;
    void Start()
    {
        targetY = Random.Range(minTargetY, maxTargetY);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < targetY)
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}