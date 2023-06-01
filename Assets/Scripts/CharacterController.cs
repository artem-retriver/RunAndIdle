using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    [Header("Manager:")]
    public GameObject[] clotherCharacter;
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

    public int clotherShopPopcorn;
    public int clotherShopElectro;
    public int clotherShopNeft;

    private bool isAlive;
    private bool isFvxOn = true;
    private bool isFvxOnNeft = true;
    private bool isFvxOnClotherShop = true;
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

        countCoinsText[6].text = clotherShopPopcorn.ToString();
        countCoinsText[7].text = clotherShopElectro.ToString();
        countCoinsText[8].text = clotherShopNeft.ToString();

        if (clotherShopPopcorn == 0 && clotherShopElectro == 0 && clotherShopNeft == 0)
        {
            fabrikaObject[2].transform.DOMoveY(0, 3);
            if (isFvxOnClotherShop == true)
            {
                fvxObjects[6].SetActive(true);
                StartCoroutine(WaitCloseFvxClotherShop());
            }
        }

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
        if (cameraCharacter[1].GetComponent<CameraController>().isIdleCamera == true)
        {
            
        }

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

            cameraCharacter[1].GetComponent<CameraController>().isIdleCamera = true;
            cameraCharacter[1].transform.DOMove(new Vector3(2.3f, 17.4f, 190.6f), 2f);
            cameraCharacter[1].transform.DOLocalRotate(new Vector3(55, -11.382f, 0), 2f);
            //cameraCharacter[1].SetActive(false);


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

        if (other.GetComponent<ClotherShop>())
        {
            StartCoroutine(WaitLoseAllPopcorn());
            StartCoroutine(WaitLoseAllElectro());
            StartCoroutine(WaitLoseAllNeft());
        }
    }

    public void AddNewItemElectro(Transform itemAdd)
    {
        itemAdd.DOJump(PositionElectro.position + new Vector3(0, 0.2f, 0), 1.5f, 1, 0.1f).OnComplete(() =>
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
        itemAdd.DOJump(PositionElectro.position + new Vector3(0, 0.2f, 0), 1.5f, 1, 0.1f).OnComplete(() =>
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

    private IEnumerator WaitCloseFvxClotherShop()
    {
        yield return new WaitForSeconds(3f);
        fvxObjects[6].SetActive(false);
        isFvxOnClotherShop = false;
        canvas[7].SetActive(false);

        clotherCharacter[0].SetActive(false);
        clotherCharacter[1].SetActive(true);
    }

    private IEnumerator WaitCloseFvxNeft()
    {
        yield return new WaitForSeconds(3f);
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
        yield return new WaitForSeconds(3f);
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

            neftSpawn.neftSpawnList[i].gameObject.transform.DOMove(neftPos.neftPos[i].transform.position, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        isNeftOn = false;
    }

    private IEnumerator WaitEelectroPos()
    {
        for (int i = 0; i < electroSpawn.electroSpawnList.Count; i++)
        {
            electroSpawn.electroSpawnList[i].gameObject.SetActive(true);

            electroSpawn.electroSpawnList[i].gameObject.transform.DOMove(electroPos.electroPos[i].transform.position, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        isElecetroOn = false;
    }

    private IEnumerator WaitIdleZone()
    {
        int currentCoins = countCoins;
        for (int i = 0; i < currentCoins; i++)
        {
            objectCoins[i].SetActive(true);
            objectCoins[i].transform.position = startPos.transform.position;
            objectCoins[i].transform.DOMove(objectCoinsPosition[i].transform.position, 0.1f);
            countCoins--;
            yield return new WaitForSeconds(0.1f);
        }

        joystick.SetActive(true);
    }

    private IEnumerator WaitLoseAllPopcorn()
    {
        for (int i = objectCoins.Length - 1; i > -1; i--)
        {
            if (clotherShopPopcorn > 0)
            {
                objectCoins[i].transform.DOMove(coinsShop[2].transform.position, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator WaitLoseAllNeft()
    {
        for (int i = objectNeft.Count - 1; i > -1; i--)
        {
            if (clotherShopNeft > 0)
            {
                objectNeft[i].transform.DOMove(coinsShop[2].transform.position, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator WaitLoseAllElectro()
    {
        for (int i = objectElectro.Count - 1; i > -1; i--)
        {
            if (clotherShopElectro > 0)
            {
                objectElectro[i].transform.DOMove(coinsShop[2].transform.position, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator WaitFalseCoins()
    {
        for (int i = objectCoins.Length - 1; i > 0; i--)
        {
            if (countCoinsShop > 0)
            {
                objectCoins[i].transform.DOMove(coinsShop[0].transform.position, 0.1f);
                
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WaitFalseElectro()
    {
        for (int i = objectElectro.Count - 1; i > 0; i--)
        {
            if (countNeftShop > 0)
            {
                objectElectro[i].transform.DOMove(coinsShop[1].transform.position, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
