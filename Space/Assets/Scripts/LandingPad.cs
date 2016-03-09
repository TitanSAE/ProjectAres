using UnityEngine;
using System.Collections;

/// <summary>
/// Component of the prefabs LandingPad gameobject
/// </summary>
public class LandingPad : MonoBehaviour
{
    public Light stationLight;
    public Material onMaterial;
	public Material offMaterial;
    public Color onColor;
    public PlayerShip shipScriptInstance;

    // Use this for initialization
    void Start()
    {
        if (Globals.PlayerShip != null)
        {
            shipScriptInstance = Globals.PlayerShip.GetComponent(typeof(PlayerShip)) as PlayerShip;
        }
        else
        {
            //populate the shipScriptInstance variable with the PlayerShip script attached to the Ship using its "Player" Tag
            shipScriptInstance = GameObject.FindWithTag("Player").GetComponent(typeof(PlayerShip)) as PlayerShip;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Activate()
    {
//        
          
            GetComponent<Renderer>().material = onMaterial;
           

        
    }

	public void DeActivate()
	{


		GetComponent<Renderer>().material = offMaterial;



	}
}