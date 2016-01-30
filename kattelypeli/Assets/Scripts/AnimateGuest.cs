using UnityEngine;
using System.Collections;

public class AnimateGuest : MonoBehaviour {

    [SerializeField]
    private AnimationCurve curveY;
    [SerializeField]
    private AnimationCurve curveZ;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float moveSpeed = 5f;

    private float timerOffsetY;
    private float timerOffsetZ;
    [SerializeField]
    private float timerY = 0;
    private float timerZ = 0f;
    [SerializeField]
    private float yPos;
    [SerializeField]
    private float yOffset;

    [SerializeField]
    private float distance;
    [SerializeField]
    private float scale;
    
    private Vector2 middle;

    public Vector3 target;


    public bool moving;

	// Use this for initialization
	void Start () {

        moving = false;
        timerOffsetY = Random.Range(0f, 1f);
        timerOffsetZ = Random.Range(0f, 1f);
        timerY += timerOffsetY;
        timerZ += timerOffsetZ;
        yPos = transform.position.y;

        middle = new Vector3(0, 0, 0);

        target = transform.position;
	}

    // Update is called once per frame
    void Update() {

        transform.position = Vector3.MoveTowards(transform.position,
        target, Time.deltaTime * moveSpeed);

        if (transform.position == target)
        {
            timerY = Mathf.Lerp(timerY, 0, 10);
            moving = false;
        }
        else moving = true;


        distance = Vector3.Distance(new Vector3(transform.position.x, 0, 0), middle);
        scale = 1 - distance / 33;

        if (transform.position.x < middle.x)
        {
            transform.localScale = new Vector3(scale, scale, transform.localScale.z);
            yOffset = distance / 5;
        }

        if (moving)
        {
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
            transform.position = new Vector3(transform.position.x, yPos + (curveY.Evaluate(timerY)*0.5f) /*+ yOffset*/, transform.position.z);

            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, curveZ.Evaluate(timerZ) * 5);
        }
    }
}
