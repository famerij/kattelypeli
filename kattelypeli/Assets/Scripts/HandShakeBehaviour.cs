using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HandShakeBehaviour : MonoBehaviour
{
    public enum HandShakeState { Running, Success, Fail, Done }
    
    [SerializeField]
    private HandShakeState currentState;

    [SerializeField]
    private int[] gestureSequence;
    [SerializeField]
    private int currentGestureSequenceIndex = 0;

    private Vector3 playerHandStartPos;

    private System.Action onSequenceFinished;

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

    [Header("Assets")]
    [SerializeField]
    private List<GameObject> handShakeAnimations;
    [SerializeField]
    private List<Sprite> playerHandSprites;
    [SerializeField]
    private Sprite playerChangeSprite;
    [SerializeField]
    private List<Sprite> otherHandSprites;

    [Header("Other")]
    [SerializeField]
    private Slider patienceSlider;
    [SerializeField]
    private float patienceTimeLimit = 3f;

    private float patienceTimer = 0f;
    #endregion

    void ChangeState(HandShakeState _newState)
    {
        currentState = _newState;

        switch (_newState)
        {
            case HandShakeState.Running:
                Debug.Log("New hand shake started");

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
                ChangeState(HandShakeState.Running);
                break;
            case HandShakeState.Done:
                Debug.Log("Done");
                playerHand.gameObject.SetActive(false);
                otherHand.gameObject.SetActive(false);
                if (onSequenceFinished != null)
                    onSequenceFinished();
                break;
            default:
                break;
        }
    }

    void Awake()
    {
        flashCanvas.worldCamera = Camera.main;
        UICanvas.worldCamera = Camera.main;
    }

    public void StartHandShakeSequence(System.Action onFinished)
    {
        onSequenceFinished = onFinished;

        gestureSequence = new int[Random.Range(2, 4)];
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

        ChangeState(HandShakeState.Running);
    }

    void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.Return))
            StartHandShakeSequence(null);

        if (currentState == HandShakeState.Done)
        {
            return;
        }
        
        if (currentState == HandShakeState.Running)
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (playerHand.CurrentSpriteIndex != 0)
                StartCoroutine(ChangePlayerHandSprite(0));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (playerHand.CurrentSpriteIndex != 1)
                StartCoroutine(ChangePlayerHandSprite(1));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (playerHand.CurrentSpriteIndex != 2)
                StartCoroutine(ChangePlayerHandSprite(2));
        }
        else
        {
            ChangePlayerHandSprite(-1);
            playerHand.SetSprite(-1, playerChangeSprite);
        }

        if (currentState == HandShakeState.Running && Input.GetKeyDown(KeyCode.Space))
        {
            CheckGesture();
        }
    }

    IEnumerator ChangePlayerHandSprite(int index)
    {
        playerHand.handSpriteRenderer.enabled = true;
        playerHand.handSpriteRenderer.sprite = playerChangeSprite;
        yield return new WaitForSeconds(.1f);
        if (index != -1)
            playerHand.SetSprite(index, playerHandSprites[index]);
    }

    IEnumerator RunSuccessAnimation()
    {
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
        ChangeState(HandShakeState.Running);
    }

    //void OfferHandShakeAnimation()
    //{
    //    //float timer = 0f;

    //    //while (timer < .5f)
    //    //{
    //    //    playerHand.transform.position = Vector3.Lerp(playerHand.transform.position, playerHand.transform.position + Vector3.up, Time.deltaTime * 2);
    //    //    timer += Time.deltaTime;
    //    //    yield return new WaitForEndOfFrame();
    //    //}

    //    //playerHand.transform.position = playerHandStartPos;

    //    CheckGesture();
    //}

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
