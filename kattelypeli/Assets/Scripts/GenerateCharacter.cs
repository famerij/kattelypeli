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

    private string cName, surname, firstTitle, secondTitle;

    public string WholeName
    {
        get { return cName + " " + surname + ", " + firstTitle + " " + secondTitle; }
    }


    private GenerateNames names;

    // Use this for initialization
    void Start()
    {
        names = FindObjectOfType<GenerateNames>();

        cName = names.nameList[Random.Range(0, names.nameList.Count)];
        surname = names.surnameList[Random.Range(0, names.surnameList.Count)];
        firstTitle = names.firstTitleList[Random.Range(0, names.firstTitleList.Count)];
        secondTitle = names.secondTitleList[Random.Range(0, names.secondTitleList.Count)];

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

        print(cName + " " + surname + " " + firstTitle + " " + secondTitle);

    }

    //void Update()
    //{
    //    //if(Input.GetButtonDown("Fire1"))
    //    //{
    //    //    Reroll();
    //    //}
    //}

    public void Reroll()
    {
        Destroy(head);
        Destroy(body);

        cName = names.nameList[Random.Range(0, names.nameList.Count)];
        surname = names.surnameList[Random.Range(0, names.surnameList.Count)];
        firstTitle = names.firstTitleList[Random.Range(0, names.firstTitleList.Count)];
        secondTitle = names.secondTitleList[Random.Range(0, names.secondTitleList.Count)];

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

        print(cName + " " + surname + " " + firstTitle + " " + secondTitle);

    }
}
