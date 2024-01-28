using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{

    public GameObject nextWave;
    public bool finalWave = false; 
    private void Update()
    {

        if (transform.childCount <= 0)
        {
            if (finalWave)
            {
                SceneManager.LoadScene("CG2");
                return;
            }
            nextWave.SetActive(true);
            Destroy(this.gameObject);
        }

    }
}
