using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using TMPro;

public class PlayerController : MonoBehaviour
{
    public double temp;
    public TextMeshProUGUI countText;
    public PumpController pp;

    // Start is called before the first frame update
    void Start()
    {
        temp = 50;
        SetTempText();
    }

    // Update is called once per frame
    void Update()
    {
        temp += 0.001;
        SetTempText();
        if (Input.GetMouseButtonDown(0))
        {
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
            }
        }
    }
    //Displays current temp
 void SetTempText() 
    {
        countText.text = "Temp: " + ((int)temp).ToString();

 if (temp >= 150)
        {
 // Display the lose text.
            //loseTextObject.SetActive(true);
        }
    }

    public void OnClick()
    {
        Debug.Log("Object Clicked!");
        // Add your custom code here
        temp += 5;
    }
}
