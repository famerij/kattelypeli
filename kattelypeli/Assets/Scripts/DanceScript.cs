using UnityEngine;
using System.Collections;

public class DanceScript : MonoBehaviour {


    [SerializeField]
    private AnimationCurve curveY;
    [SerializeField]
    private AnimationCurve curveZ;
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float danceFactor = 1;

    private float timerOffsetY;
    private float timerOffsetZ;
    [SerializeField]
    private float timerY = 0;
    private float timerZ = 0f;
    [SerializeField]
    private float yPos;
    public float yOffset;

    private Vector2 middle;

    public Vector3 target;

    public bool moving;


    // Use this for initialization
    void Start()
    {

        moving = false;
        timerOffsetY = Random.Range(0f, 1f);
        timerOffsetZ = Random.Range(0f, 1f);
        timerY += timerOffsetY;
        timerZ += timerOffsetZ;
        yPos = transform.position.y;

        middle = new Vector3(0, 0, 0);

    }

        // Update is called once per frame
        void Update() {

        timerY += Time.deltaTime * speed;
        if (timerY > 1)
        {
            timerY = 0;
        }
        timerZ += Time.deltaTime * speed;
        if (timerZ > 1)
        {
            timerZ = 0;
        }
        transform.position = new Vector3(transform.position.x, yPos + (curveY.Evaluate(timerY)*danceFactor), transform.position.z);

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, curveZ.Evaluate(timerZ) * 5);

    }
}
