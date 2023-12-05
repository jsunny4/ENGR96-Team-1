using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public CamSwitch cs;
    public PlayerController p;
    public Light spotlightOx;
    public Light spotlightStorage;
    public Light spotlightDocking;
    public Light spotlightEngine;
    public Light spotlightHallway;
    private Light currCam;
    private float normalIntensity = 1.0f;
    private float highIntensity = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setLight(int num) {
        if (num == 3) {
            currCam = spotlightOx;
            highIntensity = 8.0f;
        }
        if (num == 4) {
            currCam = spotlightEngine;
            highIntensity = 20.0f;
        }
        if (num == 5) {
            currCam = spotlightDocking;
            highIntensity = 100.0f;
        }
        if (num == 6) {
            currCam = spotlightStorage;
            highIntensity = 23.0f;
        }
        if (num == 7) {
            currCam = spotlightHallway;
            highIntensity = 20.0f;
        }
    }

    void ChangeIntensity(float newIntensity)
    {
        // Ensure the intensity is not negative
        newIntensity = Mathf.Max(newIntensity, 0.0f);

        // Set the new intensity
        currCam.intensity = newIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (cs.onCam) {
            setLight(cs.curr);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                p.temp += 5;
                setLight(cs.curr);
                Debug.Log("Space Pressed!");
                // Spacebar pressed, set intensity to high
                ChangeIntensity(highIntensity);
            }

            // Check for spacebar release
            if (Input.GetKeyUp(KeyCode.Space))
            {
                setLight(cs.curr);
                Debug.Log("Space Released!");
                // Spacebar released, set intensity to normal
                ChangeIntensity(normalIntensity);
            }
        }
    }
}
