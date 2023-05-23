using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterController : MonoBehaviour
{
    [Header("Manager:")]
    //[SerializeField] private GameManager _gameManager;
    //private Animator anim;
    public GameObject joystick;
    public GameObject[] cameraCharacter;
    private MoveController moveController;
    public TextMeshProUGUI countCoinsText;
    //public AudioSource sourceFishBone;
    //public AudioSource sourceSmash;

    private int countCoins;
    private bool isAlive = false;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        moveController = GetComponent<MoveController>();
        isAlive = true;
        //IsAlive();
    }

    public void Update()
    {
        if (isAlive == true)
        {
            moveController.InputHandler();
            moveController.Movebale();
        }
        else
            return;
    }

    public void FixedUpdate()
    {
        countCoinsText.text = countCoins.ToString();

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

    public void IsAlive()
    {
        StartCoroutine(WaitGameCoroutine());
    }

    /*public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.TryGetComponent(out Fish fish))
{
sourceFishBone.Play();
_gameManager.IncreaseCoins();
other.gameObject.SetActive(false);
}*/

        if (other.TryGetComponent(out Obstacle _))
        {
            //sourceSmash.Play();
            Died();
            //StartCoroutine(WaitLoseCoroutine());
        }

        if (other.GetComponent<Coin>())
        {
            countCoins++;
            other.gameObject.SetActive(false);
        }

        if (other.GetComponent<IdleZone>())
        {
            cameraCharacter[0].gameObject.SetActive(true);
            cameraCharacter[1].gameObject.SetActive(false);
            joystick.SetActive(true);

            GetComponent<MoveController>().enabled = false;
            GetComponent<SwipeController>().enabled = false;
            GetComponent<JoystickController>().enabled = true;
            GetComponent<CharacterController>().enabled = false;
        }
    }

    private void Died()
    {
        isAlive = false;
        //anim.Play("Death_1");
    }

    private IEnumerator WaitLoseCoroutine()
    {
        yield return new WaitForSeconds(3f);
        //_gameManager.LoseGame();
    }

    private IEnumerator WaitGameCoroutine()
    {
        yield return new WaitForSeconds(3f);
        isAlive = true;
        //.Play("runStart");
    }
}
