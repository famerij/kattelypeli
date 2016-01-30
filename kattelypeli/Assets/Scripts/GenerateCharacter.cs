using UnityEngine;
using System.Collections;

public class GenerateCharacter : MonoBehaviour
{

    [SerializeField]
    private GameObject[] heads;
    [SerializeField]
    private GameObject[] bodies;

    private GameObject head, body;
    private int i;
    // Use this for initialization
    void Start()
    {

        i = Random.Range(0, heads.Length);
        body = Instantiate(bodies[i], transform.position, Quaternion.identity) as GameObject;
        body.transform.SetParent(transform);
        body.transform.localScale = Vector3.one;
        body.transform.localPosition = Vector3.zero;

        i = Random.Range(0, heads.Length);
        head = Instantiate(heads[i], transform.position, Quaternion.identity) as GameObject;
        head.transform.SetParent(transform);
        head.transform.localScale = Vector3.one;
        head.transform.localPosition = Vector3.zero;

    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Reroll();
        }
    }

   public void Reroll()
    {
        Destroy(head);
        Destroy(body);

        i = Random.Range(0, heads.Length);
        body = Instantiate(bodies[i], transform.position, Quaternion.identity) as GameObject;
        body.transform.SetParent(transform);
        body.transform.localScale = Vector3.one;
        body.transform.localPosition = Vector3.zero;

        i = Random.Range(0, heads.Length);
        head = Instantiate(heads[i], transform.position, Quaternion.identity) as GameObject;
        head.transform.SetParent(transform);
        head.transform.localScale = Vector3.one;
        head.transform.localPosition = Vector3.zero;

    }
}
