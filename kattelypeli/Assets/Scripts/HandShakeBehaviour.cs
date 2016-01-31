using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HandShakeBehaviour : MonoBehaviour
{
    public enum HandShakeState { Running, Success, Fail, Done, GameOver }
    
    [SerializeField]
    private HandShakeState currentState;

    [SerializeField]
    private int[] gestureSequence;
    [SerializeField]
    private int currentGestureSequenceIndex = 0;

    private Vector3 playerHandStartPos;

    private System.Action<bool> onSequenceFinished;

    private int guestCounter = 0;
    private int disappointedGuests = 0;
    private int failedShakes = 0;

    #region Editor fields
    [Header("Scene objects")]
    [SerializeField]
    private Canvas flashCanvas;
    [SerializeField]
    private Canvas UICanvas;
    [SerializeField]
    private HandGesture playerHand;
    [SerializeField]
    private HandGesture otherHand;
    [SerializeField]
    private Image carpetBG;
    [SerializeField]
    private Image palaceBG;
    [SerializeField]
    private GameObject gameOverCanvas;
    [SerializeField]
    private Text gameOverComment;
    [SerializeField]
    private Slider gameOverSlider;

    [Header("Assets")]
    [SerializeField]
    private List<GameObject> handShakeAnimations;
    [SerializeField]
    private List<Sprite> playerHandSprites;
    //[SerializeField]
    //private Sprite playerChangeSprite;
    [SerializeField]
    private List<Sprite> otherHandSprites;

    [Header("Other")]
    [SerializeField]
    private Slider patienceSlider;
    [SerializeField]
    private float patienceTimeLimit = 3f;

    private float patienceTimer = 0f;
    private bool decreasePatience = true;
    #endregion

    void ChangeState(HandShakeState _newState)
    {
        currentState = _newState;
        
        switch (_newState)
        {
            case HandShakeState.Running:
                Debug.Log("New hand shake started");

                decreasePatience = true;

                if (currentGestureSequenceIndex >= gestureSequence.Length)
                {
                    ChangeState(HandShakeState.Done);
                    return;
                }

                playerHand.Disable();
                otherHand.SetSprite(gestureSequence[currentGestureSequenceIndex], otherHandSprites[gestureSequence[currentGestureSequenceIndex]]);
                break;
            case HandShakeState.Success:
                Debug.Log("Success!");
                currentGestureSequenceIndex++;
                StartCoroutine(RunSuccessAnimation());
                break;
            case HandShakeState.Fail:
                Debug.Log("Fail!");
                currentGestureSequenceIndex++;
                failedShakes++;
                ChangeState(HandShakeState.Running);
                break;
            case HandShakeState.Done:
                Debug.Log("Done");
                playerHand.gameObject.SetActive(false);
                otherHand.gameObject.SetActive(false);
                guestCounter++;
                float failedShakeRatio = (float)failedShakes / gestureSequence.Length;
                bool failed = false;
                Debug.Log("FailedShakeRatio = " + failedShakeRatio);
                if (failedShakeRatio == 1f || patienceTimer <= 0f)
                {
                    disappointedGuests++;
                    ChangeState(HandShakeState.GameOver);
                }
                if (failedShakeRatio > .5f || patienceTimer <= 0f)
                {
                    disappointedGuests++;
                    Debug.Log("Guest left disappointed");
                    failed = true;
                }
                if (onSequenceFinished != null)
                    onSequenceFinished(failed);
                break;
            case HandShakeState.GameOver:
                gameOverSlider.value = (float)disappointedGuests / guestCounter;
                gameOverComment.text = disappointedGuests + "/" + guestCounter + " vieraista olivat pettyneitä.";
                gameOverCanvas.SetActive(true);
                FindObjectOfType<GenerateLine>().enabled = false;
                this.enabled = false;
                break;
            default:
                break;
        }
    }

    void Awake()
    {
        flashCanvas.worldCamera = Camera.main;
        UICanvas.worldCamera = Camera.main;
        palaceBG.gameObject.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void StartHandShakeSequence(System.Action<bool> onFinished)
    {
        onSequenceFinished = onFinished;

        int sequenceCount = guestCounter / 3;
        if (sequenceCount <= 0)
            sequenceCount = 1;

        gestureSequence = new int[sequenceCount];
        for (int i = 0; i < gestureSequence.Length; i++)
        {
             gestureSequence[i] = Random.Range(0, playerHandSprites.Count);
        }

        currentGestureSequenceIndex = 0;

        if (patienceSlider != null) patienceSlider.maxValue = patienceTimeLimit;

        flashCanvas.gameObject.SetActive(false);

        playerHand.gameObject.SetActive(true);
        playerHandStartPos = playerHand.transform.position;

        otherHand.gameObject.SetActive(true);

        patienceTimer = patienceTimeLimit;
        failedShakes = 0;

        ChangeState(HandShakeState.Running);
    }

    void Update()
    {
        // Debug
        //if (Input.GetKeyDown(KeyCode.Return))
        //    StartHandShakeSequence(null);

        if (currentState == HandShakeState.Done || currentState == HandShakeState.GameOver)
        {
            return;
        }
        
        if (currentState == HandShakeState.Running && decreasePatience)
        {
            HandleInput();

            patienceTimer -= Time.deltaTime;
        }

        if (patienceTimer <= 0f)
        {
            ChangeState(HandShakeState.Done);
        }

        if (patienceSlider != null)
        {
            patienceSlider.value = patienceTimer;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (playerHand.CurrentSpriteIndex != 0)
                ChangePlayerHandSprite(0);
            StartCoroutine(CheckGestureDelayed());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (playerHand.CurrentSpriteIndex != 1)
                ChangePlayerHandSprite(1);
            StartCoroutine(CheckGestureDelayed());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (playerHand.CurrentSpriteIndex != 2)
                ChangePlayerHandSprite(2);
            StartCoroutine(CheckGestureDelayed());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (playerHand.CurrentSpriteIndex != 3)
                ChangePlayerHandSprite(3);
            StartCoroutine(CheckGestureDelayed());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (playerHand.CurrentSpriteIndex != 4)
                ChangePlayerHandSprite(4);
            StartCoroutine(CheckGestureDelayed());
        }
        //else
        //{
        //    ChangePlayerHandSprite(-1);
        //    playerHand.SetSprite(-1, playerChangeSprite);
        //}

        //if (currentState == HandShakeState.Running && Input.GetKeyDown(KeyCode.Space))
        //{
        //    CheckGesture();
        //}
    }

    void ChangePlayerHandSprite(int index)
    {
        decreasePatience = false;
        if (index != -1)
            playerHand.SetSprite(index, playerHandSprites[index]);
        playerHand.handSpriteRenderer.enabled = true;
    }

    IEnumerator RunSuccessAnimation()
    {
        palaceBG.gameObject.SetActive(true);
        flashCanvas.gameObject.SetActive(true);
        playerHand.gameObject.SetActive(false);
        otherHand.gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        flashCanvas.gameObject.SetActive(false);

        GameObject handShakeObj = Instantiate(handShakeAnimations[playerHand.CurrentSpriteIndex]);
        Animation handShakeAnim = handShakeObj.GetComponent<Animation>();
        
        while (handShakeAnim.isPlaying)
        {
            yield return null;
        }

        DestroyImmediate(handShakeObj);
        playerHand.gameObject.SetActive(true);
        otherHand.gameObject.SetActive(true);
        palaceBG.gameObject.SetActive(false);
        ChangeState(HandShakeState.Running);
    }

    IEnumerator CheckGestureDelayed()
    {
        decreasePatience = false;
        yield return new WaitForSeconds(.2f);

        CheckGesture();
    }

    void CheckGesture()
    {
        if (playerHand.CurrentSpriteIndex == otherHand.CurrentSpriteIndex)
        {
            ChangeState(HandShakeState.Success);
        }
        else
        {
            ChangeState(HandShakeState.Fail);
        }
    }
}
