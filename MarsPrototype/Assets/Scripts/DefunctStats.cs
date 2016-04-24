using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DefunctStats : MonoBehaviour {

    [Header("Stats")]
    public float fTimeDuration = 5;
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
        tHealthValue.text = "" + ((int)fRoverHealth).ToString() + "%";
        imHealthFill.fillAmount = fRoverHealth / 100f;
        tEnergyValue.text = "" + ((int)fRoverEnergy).ToString() + "%";
        imEnergyFill.fillAmount = fRoverEnergy / 100f;
    }
}
