using UnityEngine;
using System.Collections;

public class GenerateLine : MonoBehaviour
{

    [SerializeField]
    private GameObject[] guests;
    [SerializeField]
    private GameObject guestInstatiate;
    private GameObject guest;

    [SerializeField]
    private float speed = 5f;
    private float xPos = 1f;
    private float zPos = 0;
    private float scale = 1;
    private int x;
    private int y;

    private bool moving;

    // Use this for initialization
    void Start()
    {
        x = 0;
        moving = false;
        for (int i = 0; i < guests.Length; i++)
        {
            guest = (Instantiate(guestInstatiate, new Vector3(xPos, -6.5f, zPos), Quaternion.identity) as GameObject);
            guest.transform.position = new Vector3(guest.transform.position.x, guest.transform.position.y + guest.GetComponent<AnimateGuest>().yOffset, guest.transform.position.z);
            guests[i] = guest;
            xPos -= 2f;
            zPos = 5;
            guests[i].GetComponent<AnimateGuest>().moving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving && Input.GetButtonDown("Fire2"))
        {
            MoveOut();
        }
        if (Input.GetButtonDown("Fire2") && !moving)
        {
            moving = true;
            print("I'm gonna see the president!!");
            MoveTheLine();
        }

    }

    void MoveTheLine()
    {
        guests[x].GetComponent<AnimateGuest>().target = new Vector3(5, guests[x].transform.position.y, guests[x].transform.position.z);

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

        print(guests[x].GetComponent<AnimateGuest>().target);

        if (guests[x].transform.position.x == guests[x].GetComponent<AnimateGuest>().target.x)
        {
            guests[x].transform.position = new Vector3(xPos, guests[x].transform.position.y, guests[x].transform.position.z);
            guests[x].GetComponent<AnimateGuest>().target = new Vector3(xPos, guests[x].transform.position.y, guests[x].transform.position.z);
            guests[x].GetComponentInChildren<GenerateCharacter>().Reroll();
            x++;
            moving = false;
            if(x >= guests.Length)
            {
                x = 0;
            }
        }
    }
}

