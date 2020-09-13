using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI fuelText;

    public void UpdateFuelText(float fuel) { fuelText.text = "Fuel " + Mathf.RoundToInt(fuel); }
}
