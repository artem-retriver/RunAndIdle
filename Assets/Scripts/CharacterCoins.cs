using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCoins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CoinsShop>())
        {
            GetComponentInParent<CharacterController>().countCoins--;
            GetComponentInParent<CharacterController>().countCoinsShop--;
            gameObject.SetActive(false);
        }
    }
}
