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

    public ElectroPos electroPos;
    public ElectroSpawn electroSpawn;

    public int countCoins;
    public int countElectro;
    public int countCoinsShop = 10;
    public int countElectroShop;
    private bool isAlive;
    private bool isFvxOn = true;
    private bool isElecetroOn;

    private void Start()
    {
        moveController = GetComponent<MoveController>();

        isAlive = true;
    }

    public void Update()
    {
        countCoinsText[0].text = countCoins.ToString();
        countCoinsText[1].text = countCoinsShop.ToString();
        countCoinsText[2].text = countElectroShop.ToString();
        countCoinsText[3].text = countElectro.ToString();

        if (countCoinsShop == 0)
        {
            fabrikaObject[0].transform.DOMoveY(0, 3);
            if (isFvxOn == true)
            {
                fvxObjects[0].SetActive(true);
                StartCoroutine(WaitCloseFvx());
            }
        }

        if (electroPos.countFullPos < 16 && isElecetroOn == true)
        {
            StartCoroutine(WaitEelectroPos());
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
        if (other.GetComponent<Electro>())
        {
            other.gameObject.transform.DOMove(objectCoinsPosition[0], 1);
            countElectro++;

        }

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
        //canvas[1].SetActive(true);
        canvas[2].SetActive(true);
        canvas[3].SetActive(true);
        isElecetroOn = true;
    }

    private IEnumerator WaitEelectroPos()
    {
        for (int i = 0; i < electroSpawn.electroSpawnList.Count; i++)
        {
            electroSpawn.electroSpawnList[i].SetActive(true);
            electroSpawn.electroSpawnList[i].transform.DOMove(electroPos.electroPos[i].transform.position, 1);
            electroPos.countFullPos++;
            yield return new WaitForSeconds(1f);
        }
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
            if (countCoinsShop > 1)
            {
                objectCoins[i].transform.DOMove(coinsShop[0].transform.position, 0.4f);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
