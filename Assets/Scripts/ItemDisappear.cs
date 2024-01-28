using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisapear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() == null)
            return;
        GameController.Instance.HealHealth();
        Destroy(this.gameObject);
    }
}