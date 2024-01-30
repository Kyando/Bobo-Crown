using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public PlayerController playerController;
    public Heath healthController;
    public CameraShake cameraShake;
    public Spawner itemSpawner;

    [Header("Kingu")] public KingEnemy kingEnemy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DealDamage()
    {
        healthController.health = healthController.health - 1;
        if (healthController.health <= 0)
        {
            healthController.health = 0;

            if (SceneManager.GetActiveScene().name == "King Battle")
            {   
                Debug.Log("Caiu no g1");
                SceneManager.LoadScene("Game Over 1");
            }
            else
            {
                Debug.Log("Caiu no g0");
                SceneManager.LoadScene("Game Over");
            }

            Debug.Log("GAME OVER!!!");
        }

        playerController.TakeDamage();
        cameraShake.Shake(0.07f);

        // playerController.GetComponent<SpriteRenderer>().color =
        //     new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1);
    }

    public void ScreenShake()
    {
        cameraShake.Shake(0.04f);
    }

    public void SpawnItem()
    {
        SoundController.Instance.PlaySound(SoundController.Instance.plateiaSfx);
        
        itemSpawner.SpawnObject();
        itemSpawner.SpawnObject();
        itemSpawner.SpawnObject();
        if (kingEnemy != null)
        {
            kingEnemy.MakeMeLaugh();
        }
    }

    public void HealHealth()
    {
        healthController.health = healthController.health + 1;
        if (healthController.health > 3)
        {
            healthController.health = 3;
        }

        // playerController.GetComponent<SpriteRenderer>().color =
        //     new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1);
    }
}