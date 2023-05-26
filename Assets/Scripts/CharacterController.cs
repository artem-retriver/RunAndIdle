using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    [Header("Manager:")]
    public GameObject[] canvas;
    public GameObject[] fabrikaObject;
    public GameObject[] fvxObjects;
    public GameObject[] coinsShop;
    public GameObject joystick;
    public GameObject startPos;
    public GameObject[] cameraCharacter;
    public GameObject[] objectCoins;
    public Vector3[] objectCoinsPosition;
    private MoveController moveController;
    public TextMeshProUGUI[] countCoinsText;

    public int countCoins;
    public int countCoinsElectro = 10;
    public int countCoinsNeft = 10;
    private bool isAlive;
    private bool isFvxOn = true;

    private void Start()
    {
        moveController = GetComponent<MoveController>();

        isAlive = true;
    }

    public void Update()
    {
        countCoinsText[0].text = countCoins.ToString();
        countCoinsText[1].text = countCoinsElectro.ToString();

        if (countCoinsElectro == 0)
        {
            fabrikaObject[0].transform.DOMoveY(0, 3);
            if (isFvxOn == true)
            {
                fvxObjects[0].SetActive(true);
                StartCoroutine(WaitCloseFvx());
            }

        }

        for (int i = 0; i < objectCoinsPosition.Length; i++)
        {
            objectCoinsPosition[i] = objectCoins[i].transform.position;
        }

        if (isAlive == true && GetComponent<JoystickController>().enabled == false)
        {
            moveController.InputHandler();
            moveController.Movebale();
        }
        else
            return;
    }

    public void FixedUpdate()
    {
        if (isAlive == true)
        {
            moveController.Move();
        }
        else
        {
            moveController.UnMove();
        }
        return;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent(out Obstacle _))
        {
            Died();
        }

        if (other.GetComponent<Coin>())
        {
            countCoins++;
            other.gameObject.SetActive(false);
        }

        if (other.GetComponent<IdleZone>())
        {
            StartCoroutine(WaitIdleZone());
            /*for (int i = 0; i < countCoins; i++)
            {
                objectCoins[i].SetActive(true);
                objectCoins[i].transform.position = cameraCharacter[0].transform.position;
                objectCoins[i].transform.DOMove(objectCoinsPosition[i], 1);
            }*/

            cameraCharacter[0].SetActive(true);
            cameraCharacter[1].SetActive(false);
            joystick.SetActive(true);

            GetComponent<JoystickController>().enabled = true;
            other.gameObject.SetActive(false);
        }

        if (other.GetComponent<CoinsShop>())
        {
            StartCoroutine(WaitFalseCoins());
        }
    }

    private void Died()
    {
        isAlive = false;
    }

    private IEnumerator WaitCloseFvx()
    {
        yield return new WaitForSeconds(4f);
        fvxObjects[0].SetActive(false);
        isFvxOn = false;
        fvxObjects[1].SetActive(true);
        fvxObjects[2].SetActive(true);
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
    }

    private IEnumerator WaitIdleZone()
    {
        for (int i = 0; i < countCoins; i++)
        {
            objectCoins[i].SetActive(true);
            objectCoins[i].transform.position = startPos.transform.position;
            objectCoins[i].transform.DOMove(objectCoinsPosition[i], 1);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator WaitFalseCoins()
    {
        for (int i = 0; i < objectCoins.Length; i++)
        {
            if (countCoinsElectro > 1)
            {
                objectCoins[i].transform.DOMove(coinsShop[0].transform.position, 0.4f);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
