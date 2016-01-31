using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
