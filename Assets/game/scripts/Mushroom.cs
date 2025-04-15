using System;
using UnityEngine;

public class Mushroom : Singelton<Mushroom>
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealthController.Instance.HealPlayer();
            Destroy(gameObject);
        }
    }
}
