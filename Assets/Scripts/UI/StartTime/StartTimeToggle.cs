using UnityEngine;

public class StartTimeToggle : MonoBehaviour
{
    public GameObject timeToggle;
    
    public void ToggleStartTime(bool value)
    {
        timeToggle.SetActive(!value);
    }
}