using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro : MonoBehaviour
{
    public bool isOnCharacter;
    public bool isAlready;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShopNeft>())
        {
            GetComponentInParent<CharacterController>().countElectro--;
            GetComponentInParent<CharacterController>().countNeftShop--;
            gameObject.SetActive(false);
        }

        if (other.GetComponent<ClotherShop>())
        {
            GetComponentInParent<CharacterController>().countElectro--;
            GetComponentInParent<CharacterController>().clotherShopElectro--;
            gameObject.SetActive(false);
        }

        if (isAlready) return;

        if (other.TryGetComponent(out CharacterController character))
        {
            character.AddNewItemElectro(this.transform);
            isAlready = true;
        }

        
    }
}
