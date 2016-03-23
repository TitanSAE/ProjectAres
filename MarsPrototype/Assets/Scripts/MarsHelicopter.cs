using UnityEngine;
using System.Collections;
using System;

public enum HelicopterMovement {
    Undetermined = 0,
    LeftThruster,
    RightThruster,
    BottomThruster,
    LeftBottomThruster,
    RightBottomThruster,
	ForwardThruster,
	BackwardsThruster,
}

public class MarsHelicopter : MonoBehaviour {
	public ParticleSystem ThrusterBottom;
	public ParticleSystem ThrusterLeft;
	public ParticleSystem ThrusterRight;

    private const float fuelBurnSpeed = 20;
    //public static bool HaveFuel { get { return Globals.Game.FuelRemaining > State.FuelMinimum; } }

	void Start() {
		//
	}

	void Update() {
		HelicopterMovement choice = HelicopterMovement.Undetermined;
		if (Input.GetAxis ("Horizontal") > 0) { //right
			choice = HelicopterMovement.LeftThruster;
		} else if (Input.GetAxis ("Horizontal") < 0) { //left
			choice = HelicopterMovement.RightThruster;
		}

		if (Input.GetAxis ("Vertical") > 0) {
			choice = HelicopterMovement.ForwardThruster;
			Debug.Log (choice);
		} else if (Input.GetAxis ("Vertical") < 0) {
			choice = HelicopterMovement.BackwardsThruster;
		}	

		if (Input.GetKey (KeyCode.Space)) { //up
			Debug.Log ("Pressed");
			//retain horizontal movement in choice
			choice = (HelicopterMovement)((int)HelicopterMovement.BottomThruster + (int)choice);
		}

		ThrustOn (choice);

		//move the ship
		//Globals.PlayerShip.AddForce (shipForce);
	}

    public Vector3 ThrustOn(HelicopterMovement direction) {
        //if (HelicopterMovement.HaveFuel == false)
        //    return new Vector3(0, 0, 0);

       
        if (direction != HelicopterMovement.Undetermined) {
            //float fuelRemaining = Globals.Game.FuelRemaining;
            //fuelRemaining -= fuelBurnSpeed * Time.deltaTime;
            //if (fuelRemaining <= State.FuelMinimum) //we ran out of fuel
            {
                //fuelRemaining = State.FuelMinimum; //uses 0.01 because a value of 0 causes trouble in handling division by zero

                ThrusterBottom.Emit(0);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(0);
                //Globals.Game.FuelRemaining = fuelRemaining;

                return new Vector3(0, 0, 0);
            }

            //Globals.Game.FuelRemaining = fuelRemaining;
        }

        float sideThrust = 600 * Time.deltaTime;
        float bottomThrust = 2000 * Time.deltaTime;
		float forwardThrust = 600 * Time.deltaTime;
		float backwardThrust = 600 * Time.deltaTime;
        switch (direction) {
		case HelicopterMovement.LeftThruster:
				ThrusterBottom.Emit (0);
				ThrusterLeft.Emit (1);
				ThrusterRight.Emit (0);
				
                return new Vector3(sideThrust, 0, 0);
            case HelicopterMovement.RightThruster:
                ThrusterBottom.Emit(0);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(1);
				
                return new Vector3(-sideThrust, 0, 0);
            case HelicopterMovement.LeftBottomThruster:
                ThrusterBottom.Emit(1);
                ThrusterLeft.Emit(1);
                ThrusterRight.Emit(0);
				
                return new Vector3(sideThrust, bottomThrust, 0);
            case HelicopterMovement.BottomThruster:
                ThrusterBottom.Emit(1);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(0);
				
                return new Vector3(0, bottomThrust, 0);
            case HelicopterMovement.RightBottomThruster:
                ThrusterBottom.Emit(1);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(1);
				
                return new Vector3(-sideThrust, bottomThrust, 0);

			case HelicopterMovement.ForwardThruster:
			return new Vector3 (0, bottomThrust, forwardThrust);
			case HelicopterMovement.BackwardsThruster:
			return new Vector3(0, bottomThrust, -backwardThrust);

            case HelicopterMovement.Undetermined:
            default:
                ThrusterBottom.Emit(0);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(0);
                return new Vector3(0, 0, 0);
        }
    }
}