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
    private Slider timerSlider;
    [SerializeField]
    private float handShakeTimeLimit = 3f;
    [SerializeField]
    private float handShakeTimer = 0f;
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
                handShakeTimer = 0f;
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

        if (timerSlider != null) timerSlider.maxValue = handShakeTimeLimit;

        flashCanvas.gameObject.SetActive(false);

        playerHand.gameObject.SetActive(true);
        otherHand.gameObject.SetActive(true);

        ChangeState(HandShakeState.Running);
    }

    void Update()
    {
        // Debug
        //if (Input.GetKeyDown(KeyCode.Return))
        //    StartHandShakeSequence();

        if (currentState == HandShakeState.Done)
        {
            return;
        }
        
        if (currentState == HandShakeState.Running)
        {
            HandleInput();

            handShakeTimer += Time.deltaTime;

            if (handShakeTimer > handShakeTimeLimit)
            {
                ChangeState(HandShakeState.Fail);
            }

            if (timerSlider != null)
            {
                timerSlider.value =  handShakeTimer;
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(ChangePlayerHandSprite(0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(ChangePlayerHandSprite(1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(ChangePlayerHandSprite(2));
        }
        //else if (Input.GetAxis("Button4") > 0)
        //{
        //    playerHand.SetSprite(3, playerHandSprites[3]);
        //}
        //else if (Input.GetAxis("Button5") > 0)
        //{
        //    playerHand.SetSprite(4, playerHandSprites[4]);
        //}

        if (currentState == HandShakeState.Running && Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(OfferHandShakeAnimation());
        }
    }

    IEnumerator ChangePlayerHandSprite(int index)
    {
        playerHand.handSpriteRenderer.enabled = true;
        playerHand.handSpriteRenderer.sprite = playerChangeSprite;
        yield return new WaitForSeconds(.1f);
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

    IEnumerator OfferHandShakeAnimation()
    {
        float timer = 0f;

        Vector3 pos = playerHand.transform.position;

        while (timer < 1f)
        {
            playerHand.transform.position = Vector3.Lerp(playerHand.transform.position, playerHand.transform.position + Vector3.up, Time.deltaTime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        playerHand.transform.position = pos;

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
