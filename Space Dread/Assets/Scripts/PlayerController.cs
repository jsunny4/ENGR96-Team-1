using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public double temp;
    public TextMeshProUGUI countText;
    public PumpController pp;
    public CamSwitch cs;
    // Start is called before the first frame update
    void Start()
    {
        temp = 50;
        SetTempText();
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.isPaused) return;

        temp += 0.005;
        SetTempText();
        if (Input.GetMouseButtonDown(0))
        {
            Camera currentCamera = Camera.current;
            // Create a ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about the hit
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // The ray hit an object
                Debug.Log("Hit: " + hit.collider.gameObject.name);

                // Perform actions based on the hit object
                // For example, you might check the tag of the hit object
                if (hit.collider.gameObject.name == "Pump Handle")
                {
                    Debug.Log("Object Clicked!");
                    pp.OnClick();
                }
                if (hit.collider.gameObject.name == "left monitor overlay")
                {
                    Debug.Log("Object Clicked!");
                    cs.OnClick(5);
                }
                if (hit.collider.gameObject.name == "middle monitor overlay")
                {
                    Debug.Log("Object Clicked!");
                    cs.OnClick(6);
                }
                if (hit.collider.gameObject.name == "right monitor overlay")
                {
                    Debug.Log("Object Clicked!");
                    cs.OnClick(7);
                }
                if (hit.collider.gameObject.name == "middle pc monitor overlay")
                {
                    Debug.Log("Object Clicked!");
                    cs.OnClick(4);
                }
                if (hit.collider.gameObject.name == "right pc monitor overlay")
                {
                    Debug.Log("Object Clicked!");
                    cs.OnClick(3);
                }
            }
        }
    }
    //Displays current temp
    void SetTempText()
    {
        countText.text = "Temp: " + ((int)temp).ToString();

        if (temp >= 120)
        {
            // Display the lose text.
            SceneManager.LoadScene("Game Over");
        }
    }

    public void OnClick()
    {
        Debug.Log("Object Clicked!");
        // Add your custom code here
        temp += 5;
    }
}
