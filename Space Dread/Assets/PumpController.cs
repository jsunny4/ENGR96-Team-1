using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpController : MonoBehaviour
{
[SerializeField] Animator anim;

    public PlayerController p;
    private bool hasTriggered = false;
    private float resetTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        if (!hasTriggered)
        {
            // Change your variable here
            p.temp -= 2;

            // Set the flag to true to prevent further changes
            hasTriggered = true;
            StartCoroutine(ResetFlagAfterTime());
        }
        anim.SetTrigger("PumpClicked");
    }

    IEnumerator ResetFlagAfterTime()
    {
        yield return new WaitForSeconds(resetTime);

        // Reset the flag to allow another change
        hasTriggered = false;
    }
}
