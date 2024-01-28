using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KinguAttackZone : MonoBehaviour
{
    public bool canDealDamage = false;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && canDealDamage)
        {
            canDealDamage = false;
            GameController.Instance.DealDamage();
            
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && canDealDamage)
        {
            canDealDamage = false;
            GameController.Instance.DealDamage();
            
            gameObject.SetActive(false);
        }
    }
}
