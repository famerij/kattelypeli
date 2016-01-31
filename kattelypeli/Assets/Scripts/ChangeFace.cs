using UnityEngine;
using System.Collections;

public class ChangeFace : MonoBehaviour {

    [SerializeField]
    private Sprite happyFace;
    [SerializeField]
    private Sprite sadFace;

    private SpriteRenderer sR;

    // Use this for initialization
    void Start () {
        sR = GetComponent<SpriteRenderer>();
        sR.sprite = happyFace;
	}
	
    public void isHappy(bool happy)
    {
        if (happy)
        {
            sR.sprite = happyFace;
        }
        else sR.sprite = sadFace;
    }
}
