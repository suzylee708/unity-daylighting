using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondCamera;

    void Start()
    {
        // Ensure the Main Camera is active and the Second Camera is inactive at the start
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
        if (secondCamera != null) secondCamera.gameObject.SetActive(false);
    }

    public void SwitchToSecondCamera()
    {
        if (mainCamera != null && secondCamera != null)
        {
            mainCamera.gameObject.SetActive(false); // Disable main camera
            secondCamera.gameObject.SetActive(true); // Enable second camera
        }
        else
        {
            Debug.LogError("One or both cameras are not assigned!");
        }
    }
}
