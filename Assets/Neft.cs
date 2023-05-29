using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neft : MonoBehaviour
{
    public bool isAlready;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ClotherShop>())
        {
            GetComponentInParent<CharacterController>().countNeft--;
            GetComponentInParent<CharacterController>().clotherShopNeft--;
            gameObject.SetActive(false);
        }

        if (isAlready) return;

        if (other.TryGetComponent(out CharacterController character))
        {
            character.AddNewItemNeft(this.transform);
            isAlready = true;
        }
    }
}
