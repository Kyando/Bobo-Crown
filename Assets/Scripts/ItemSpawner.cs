using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject item;
    public float heightOffset = 1;

    public float minForce = 4.0f;
    public float maxForce = 12.0f;
    public float xOffset = 1.0f;

    public void SpawnObject()
    {
        float spawnX = Random.Range(1.2f, 5.55f) * -1;
        float spawnY = transform.position.y + heightOffset;
        float xForce = Random.Range(140f, 200f);

        if (Random.Range(0, 2) % 2 == 0)
        {
            spawnX *= -1;
            xForce *= -1;
        }

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
        GameObject spawnedObject = Instantiate(item, spawnPosition, Quaternion.identity);

        spawnedObject.GetComponent<Rigidbody2D>()
            .AddForce(Vector2.up * Random.Range(minForce, maxForce) + (Vector2.right * xForce));

        Rigidbody2D objectRb = spawnedObject.GetComponent<Rigidbody2D>();
        objectRb.AddForce(Vector2.down * 0.5f, ForceMode2D.Impulse);
    }
}