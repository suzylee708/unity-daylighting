using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle summerToggle;     // Reference to the Summer Toggle
    public Toggle winterToggle;     // Reference to the Winter Toggle
    public Toggle furnitureToggle;  // Reference to the Furniture Toggle
    public Toggle dogToggle;        // Reference to the Dog Toggle
    public GameObject summerFBX;    // Reference to the Summer FBX
    public GameObject winterFBX;    // Reference to the Winter FBX
    public GameObject furnitureFBX; // Reference to the Furniture FBX
    public GameObject dogFBX;       // Reference to the Dog FBX

    void Start()
    {
        // Ensure all toggles are off at the start
        summerToggle.isOn = false;
        winterToggle.isOn = false;
        furnitureToggle.isOn = false;
        dogToggle.isOn = false;

        // Ensure all FBX objects are disabled at the start
        summerFBX.SetActive(false);
        winterFBX.SetActive(false);
        furnitureFBX.SetActive(false);
        dogFBX.SetActive(false);

        // Add listeners to the toggles
        summerToggle.onValueChanged.AddListener(OnSummerToggleChanged);
        winterToggle.onValueChanged.AddListener(OnWinterToggleChanged);
        furnitureToggle.onValueChanged.AddListener(OnFurnitureToggleChanged);
        dogToggle.onValueChanged.AddListener(OnDogToggleChanged);
    }

    void OnSummerToggleChanged(bool isOn)
    {
        // Show or hide the Summer FBX based on the toggle state
        summerFBX.SetActive(isOn);
    }

    void OnWinterToggleChanged(bool isOn)
    {
        // Show or hide the Winter FBX based on the toggle state
        winterFBX.SetActive(isOn);
    }

    void OnFurnitureToggleChanged(bool isOn)
    {
        // Show or hide the Furniture FBX based on the toggle state
        furnitureFBX.SetActive(isOn);
    }

    void OnDogToggleChanged(bool isOn)
    {
        // Show or hide the Dog FBX based on the toggle state
        dogFBX.SetActive(isOn);
    }
}
