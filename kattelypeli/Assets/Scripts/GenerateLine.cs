using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GenerateLine : MonoBehaviour
{
    public enum LineState { MovingIn, ShakingHands, MovingOut }

    private LineState currentState = LineState.MovingIn;

    private HandShakeBehaviour handShakeBehaviour;

    [SerializeField]
    private Text guestName;

    [SerializeField]
    private GameObject[] guests;
    [SerializeField]
    private GameObject guestInstatiate;
    private GameObject guest;

    [SerializeField]
    private float speed = 5f;
    private float xPos = -7f;
    private float zPos = 0;
    private float scale = 1;
    private int x;
    private int y;

    private float middletarget = 1f;

    private bool moving;

    bool isGuestMoving = false;
    float delayTimer = 0f;

    public Vector3 target;

    void ChangeState(LineState _newState)
    {
        currentState = _newState;

        switch (currentState)
        {
            case LineState.MovingIn:
                MoveTheLine();
                break;
            case LineState.ShakingHands:
                delayTimer = 0f;
                break;
            case LineState.MovingOut:
                foreach (var guest in guests)
                {
                    guest.GetComponent<AnimateGuest>().moving = true;
                }
                MoveOut();
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        x = 0;
        moving = false;
        for (int i = 0; i < guests.Length; i++)
        {
            guest = (Instantiate(guestInstatiate, new Vector3(xPos, -6.5f, zPos), Quaternion.identity) as GameObject);
            guest.transform.position = new Vector3(guest.transform.position.x, guest.transform.position.y + guest.GetComponent<AnimateGuest>().yOffset, transform.position.z - .5f);
            guests[i] = guest;
            xPos -= 2f;
            zPos = 5;
            guest.GetComponent<AnimateGuest>().target = new Vector3(middletarget, guest.transform.position.y, guest.transform.position.z);
            middletarget -= 2f;
        }

        handShakeBehaviour = FindObjectOfType<HandShakeBehaviour>();
        handShakeBehaviour.gameObject.SetActive(false);

        ChangeState(LineState.MovingIn);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        guestName.text = guests[x].GetComponentInChildren<GenerateCharacter>().WholeName;

        //if (moving && Input.GetButtonDown("Fire2"))
        //{
        //    MoveOut();
        //}
        //if (Input.GetButtonDown("Fire2") && !moving)
        //{
        //    moving = true;
        //    print("I'm gonna see the president!!");
        //    MoveTheLine();
        //}

        switch (currentState)
        {
            case LineState.MovingIn:
                isGuestMoving = false;

                foreach(var guest in guests)
                {
                    guest.transform.position = Vector3.MoveTowards(guest.transform.position, guest.GetComponent<AnimateGuest>().target, Time.deltaTime * 5);
                    if (guest.GetComponent<AnimateGuest>().moving)
                        isGuestMoving = true;
                }

                if (!isGuestMoving)
                {
                    // Start shaking with delay
                    if (delayTimer > 1f)
                    {
                        handShakeBehaviour.gameObject.SetActive(true);
                        handShakeBehaviour.StartHandShakeSequence(ShakingHandsFinished);
                        ChangeState(LineState.ShakingHands);
                    }
                    else
                        delayTimer += Time.deltaTime;
                }
                break;
            case LineState.ShakingHands:
                
                break;
            case LineState.MovingOut:
                isGuestMoving = false;
                MoveOut();
                foreach (var guest in guests)
                {
                    guest.transform.position = Vector3.MoveTowards(guest.transform.position, guest.GetComponent<AnimateGuest>().target, Time.deltaTime * 5);
                    if (guest.GetComponent<AnimateGuest>().moving)
                        isGuestMoving = true;
                }
                if (!isGuestMoving)
                {
                    ChangeState(LineState.MovingIn);
                }
                break;
            default:
                break;
        }
    }

    void ShakingHandsFinished(bool failed)
    {
        handShakeBehaviour.gameObject.SetActive(false);
        ChangeState(LineState.MovingOut);
    }

    void MoveTheLine()
    {
        guests[x].GetComponent<AnimateGuest>().target = new Vector3(5, guests[x].transform.position.y, guests[x].transform.position.z);
        guestName.text = guests[x].GetComponentInChildren<GenerateCharacter>().WholeName;

        xPos = 1;
        for (y = x + 1; y < guests.Length; y++)
        {
            guests[y].GetComponent<AnimateGuest>().target = new Vector3(xPos, guests[y].transform.position.y, guests[y].transform.position.z);
            xPos -= 2f;
        }
        if(x > 1 && x < guests.Length)
        {
            for (y = 0; y < x; y++)
            {
                guests[y].GetComponent<AnimateGuest>().target = new Vector3(xPos, guests[y].transform.position.y, guests[y].transform.position.z);
                xPos -= 2f;
            }
        }
    }

    void MoveOut()
    {
        guests[x].GetComponent<AnimateGuest>().target = new Vector3(15, guests[x].transform.position.y, guests[x].transform.position.z);

        //print(guests[x].GetComponent<AnimateGuest>().target);

        if (Mathf.Approximately(guests[x].transform.position.x, guests[x].GetComponent<AnimateGuest>().target.x))
        {
            //print("WHOT");
            guests[x].transform.position = new Vector3(xPos, guests[x].transform.position.y, guests[x].transform.position.z);
            guests[x].GetComponent<AnimateGuest>().target = new Vector3(xPos, guests[x].transform.position.y, guests[x].transform.position.z);
            guests[x].GetComponentInChildren<GenerateCharacter>().Reroll();
            x++;
            if(x >= guests.Length)
            {
                x = 0;
            }
        }
    }
}

