using UnityEngine;
using System.Collections;

public class SaleScript : MonoBehaviour
{
    Animator animator;
    float timer = 0f;
    float randomTime = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        randomTime = Random.Range(10f, 15f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > randomTime)
        {
            Debug.Log("Playing sale animation");
            animator.SetTrigger("Play");

            timer = 0f;
            randomTime = Random.Range(10f, 15f);
        }
    }
}
