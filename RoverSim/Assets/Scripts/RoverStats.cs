using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoverStats : MonoBehaviour {

    [Header ("Stats")]
    [Range(0, 100)]
    public float fRoverHealth;
    [Range(0, 100)]
    public float fRoverEnergy;

    [Header("UI")]
    public Image imHealthFill;
    public Text tHealthValue;
    public Image imEnergyFill;
    public Text tEnergyValue;

	void Start ()
    {

	}
	
	void Update ()
    {
        UpdateStats();
	}

    void UpdateStats()
    {
        tHealthValue.text = "" + fRoverHealth + "%";
        imHealthFill.fillAmount = fRoverHealth / 100f;
        tEnergyValue.text = "" + fRoverEnergy + "%";
        imEnergyFill.fillAmount = fRoverEnergy / 100f;
    }
}
