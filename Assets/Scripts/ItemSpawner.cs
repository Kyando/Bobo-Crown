using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject item;
    public float heightOffset = 1;
    public float spawnRate = 20;
    private float _timer = 0;

    void Update()
    {
        if (_timer < spawnRate)
        {
            _timer = _timer + Time.deltaTime;
        }    
        else
        {
            spawnObject();
            _timer = 0;
        }

    }

    void spawnObject()
    {
        float spawnX = Random.Range(-5.49f, 5.55f);
        float spawnY = transform.position.y + heightOffset;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
        GameObject spawnedObject = Instantiate(item, spawnPosition, Quaternion.identity);
        
        Rigidbody2D objectRb = spawnedObject.GetComponent<Rigidbody2D>();
        objectRb.AddForce(Vector2.down * 0.5f, ForceMode2D.Impulse);
        
        
    }
}