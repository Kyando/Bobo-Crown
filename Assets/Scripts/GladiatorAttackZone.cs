using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladiatorAttackZone : MonoBehaviour
{
    public bool canDealDamage = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.GetComponent<PlayerController>() != null && canDealDamage)
        {
            canDealDamage = false;
            GameController.Instance.DealDamage();
            gameObject.SetActive(false);
            
            SoundController.Instance.PlaySound(SoundController.Instance.gladiatorAttackSfx);
        }
        
    }
}
