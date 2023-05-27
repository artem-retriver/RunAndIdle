using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    [Header("Manager:")]
    public Transform PositionElectro;
    public List<GameObject> objectElectro = new();
    public List<GameObject> objectNeft = new();
    public GameObject[] canvas;
    public GameObject[] fabrikaObject;
    public GameObject[] fvxObjects;
    public GameObject[] coinsShop;
    public GameObject joystick;
    public GameObject startPos;
    public GameObject[] cameraCharacter;
    public GameObject[] objectCoins;
    public GameObject[] objectCoinsPosition;
    private MoveController moveController;
    public TextMeshProUGUI[] countCoinsText;

    public ElectroPos electroPos;
    public ElectroSpawn electroSpawn;

    public NeftPos neftPos;
    public NeftSpawn neftSpawn;

    public int countCoins;
    public int countElectro;
    public int countNeft;
    public int countCoinsShop = 10;
    public int countElectroShop;
    public int countNeftShop = 10;
    public int numOfHoldElectro;
    public int numOfHoldNeft;

    private bool isAlive;
    private bool isFvxOn = true;
    private bool isFvxOnNeft = true;
    public bool isElecetroOn;
    public bool isNeftOn;

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
        countCoinsText[4].text = countNeftShop.ToString();
        countCoinsText[5].text = countNeft.ToString();

        if (countCoinsShop == 0)
        {
            fabrikaObject[0].transform.DOMoveY(0, 3);
            if (isFvxOn == true)
            {
                fvxObjects[0].SetActive(true);
                StartCoroutine(WaitCloseFvx());
            }
        }

        if (countNeftShop == 0)
        {
            fabrikaObject[1].transform.DOMoveY(-1.82f, 3);
            if (isFvxOnNeft == true)
            {
                fvxObjects[3].SetActive(true);
                StartCoroutine(WaitCloseFvxNeft());
            }
        }

        if (isElecetroOn == true)
        {
            StartCoroutine(WaitEelectroPos());
        }

        if (isNeftOn == true)
        {
            StartCoroutine(WaitNeftPos());
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

            cameraCharacter[0].SetActive(true);
            cameraCharacter[1].SetActive(false);
            

            GetComponent<JoystickController>().enabled = true;
            other.gameObject.SetActive(false);
        }

        if (other.GetComponent<CoinsShop>())
        {
            StartCoroutine(WaitFalseCoins());
        }

        if (other.GetComponent<ShopNeft>())
        {
            StartCoroutine(WaitFalseElectro());
        }
    }

    public void AddNewItemElectro(Transform itemAdd)
    {
        itemAdd.DOMove(PositionElectro.position, 0.3f).OnComplete(() =>
        {
            itemAdd.SetParent(PositionElectro, true);
            objectElectro.Add(itemAdd.gameObject);

            if (numOfHoldElectro > 10)
            {
                itemAdd.localPosition = new Vector3(0.2f, 0.2f * (numOfHoldElectro - 10), 0);
            }
            else
            {
                itemAdd.localPosition = new Vector3(-0.2f, 0.2f * numOfHoldElectro, 0);
            }
            
            itemAdd.localRotation = Quaternion.identity;
            numOfHoldElectro++;
            countElectro++;
        });
    }

    public void AddNewItemNeft(Transform itemAdd)
    {
        itemAdd.DOMove(PositionElectro.position, 0.3f).OnComplete(() =>
        {
            itemAdd.SetParent(PositionElectro, true);
            objectNeft.Add(itemAdd.gameObject);

            if (numOfHoldNeft > 5)
            {
                itemAdd.localPosition = new Vector3(0.2f, 0.2f * (numOfHoldNeft - 5), 0);
            }
            else
            {
                itemAdd.localPosition = new Vector3(-0.2f, 0.2f * numOfHoldNeft, 0);
            }

            itemAdd.localRotation = Quaternion.identity;
            numOfHoldNeft++;
            countNeft++;
        });
    }

    private void Died()
    {
        isAlive = false;
    }

    private IEnumerator WaitCloseFvxNeft()
    {
        yield return new WaitForSeconds(4f);
        fvxObjects[3].SetActive(false);
        isFvxOnNeft = false;
        fvxObjects[4].SetActive(true);
        fvxObjects[5].SetActive(true);
        canvas[4].SetActive(false);
        
        canvas[5].SetActive(true);
        canvas[6].SetActive(true);
        isNeftOn = true;
        //isElecetroOn = true;
    }

    private IEnumerator WaitCloseFvx()
    {
        yield return new WaitForSeconds(4f);
        fvxObjects[0].SetActive(false);
        isFvxOn = false;
        fvxObjects[1].SetActive(true);
        fvxObjects[2].SetActive(true);
        canvas[0].SetActive(false);
        canvas[2].SetActive(true);
        canvas[3].SetActive(true);
        isElecetroOn = true;
    }

    private IEnumerator WaitNeftPos()
    {
        for (int i = 0; i < neftSpawn.neftSpawnList.Count; i++)
        {
            neftSpawn.neftSpawnList[i].gameObject.SetActive(true);

            neftSpawn.neftSpawnList[i].gameObject.transform.DOMove(neftPos.neftPos[i].transform.position, 0.3f);
            yield return new WaitForSeconds(0.3f);
        }

        isNeftOn = false;
    }

    private IEnumerator WaitEelectroPos()
    {
        for (int i = 0; i < electroSpawn.electroSpawnList.Count; i++)
        {
            electroSpawn.electroSpawnList[i].gameObject.SetActive(true);

            electroSpawn.electroSpawnList[i].gameObject.transform.DOMove(electroPos.electroPos[i].transform.position, 0.3f);
            yield return new WaitForSeconds(0.3f);
        }

        isElecetroOn = false;
    }

    private IEnumerator WaitIdleZone()
    {
        for (int i = 0; i < countCoins; i++)
        {
            objectCoins[i].SetActive(true);
            objectCoins[i].transform.position = startPos.transform.position;
            objectCoins[i].transform.DOMove(objectCoinsPosition[i].transform.position, 0.5f);

            yield return new WaitForSeconds(0.2f);
        }

        joystick.SetActive(true);
    }

    private IEnumerator WaitFalseCoins()
    {
        for (int i = 0; i < objectCoins.Length; i++)
        {
            if (countCoinsShop > 0)
            {
                objectCoins[i].transform.DOMove(coinsShop[0].transform.position, 0.2f);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private IEnumerator WaitFalseElectro()
    {
        for (int i = 0; i < objectElectro.Count; i++)
        {
            if (countNeftShop > 0)
            {
                objectElectro[i].transform.DOMove(coinsShop[1].transform.position, 0.2f);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
