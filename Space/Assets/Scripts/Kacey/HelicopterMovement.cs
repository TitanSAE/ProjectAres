using UnityEngine;
using System.Collections;
using System;

public enum PlayerMoveEnum
{
    Undetermined = 0,
    LeftThruster,
    RightThruster,
    BottomThruster,
    LeftBottomThruster,
    RightBottomThruster,
	ForwardThruster,
	BackwardsThruster,
}

public class HelicopterMovement
{
    public ParticleSystem ThrusterBottom { get; set; }
    public ParticleSystem ThrusterLeft { get; set; }
    public ParticleSystem ThrusterRight { get; set; }
    private const float fuelBurnSpeed = 20;
    public static bool HaveFuel { get { return Globals.Game.FuelRemaining > State.FuelMinimum; } }

    public HelicopterMovement()
    {
    }

    public static bool OfShipInitialized
    {
        get
        {
            return Globals.PlayerShip != null
                && Globals.BottomThruster != null
                && Globals.LeftThruster != null
                && Globals.RightThruster != null;
        }
    }

    public Vector3 ThrustOn(PlayerMoveEnum direction)
    {
        if (HelicopterMovement.HaveFuel == false)
            return new Vector3(0, 0, 0);

       
        if (direction != PlayerMoveEnum.Undetermined)
        {
            float fuelRemaining = Globals.Game.FuelRemaining;
            fuelRemaining -= fuelBurnSpeed * Time.deltaTime;
            if (fuelRemaining <= State.FuelMinimum) //we ran out of fuel
            {
                fuelRemaining = State.FuelMinimum; //uses 0.01 because a value of 0 causes trouble in handling division by zero

                ThrusterBottom.Emit(0);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(0);
                Globals.Game.FuelRemaining = fuelRemaining;

                return new Vector3(0, 0, 0);
            }

            Globals.Game.FuelRemaining = fuelRemaining;
        }

        float sideThrust = 600 * Time.deltaTime;
        float bottomThrust = 2000 * Time.deltaTime;
		float forwardThrust = 600 * Time.deltaTime;
		float backwardThrust = 600 * Time.deltaTime;
        switch (direction)
        {
		case PlayerMoveEnum.LeftThruster:
				ThrusterBottom.Emit (0);
				ThrusterLeft.Emit (1);
				ThrusterRight.Emit (0);
				
                return new Vector3(sideThrust, 0, 0);
            case PlayerMoveEnum.RightThruster:
                ThrusterBottom.Emit(0);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(1);
				
                return new Vector3(-sideThrust, 0, 0);
            case PlayerMoveEnum.LeftBottomThruster:
                ThrusterBottom.Emit(1);
                ThrusterLeft.Emit(1);
                ThrusterRight.Emit(0);
				
                return new Vector3(sideThrust, bottomThrust, 0);
            case PlayerMoveEnum.BottomThruster:
                ThrusterBottom.Emit(1);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(0);
				
                return new Vector3(0, bottomThrust, 0);
            case PlayerMoveEnum.RightBottomThruster:
                ThrusterBottom.Emit(1);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(1);
				
                return new Vector3(-sideThrust, bottomThrust, 0);

			case PlayerMoveEnum.ForwardThruster:
			return new Vector3 (0, bottomThrust, forwardThrust);
			case PlayerMoveEnum.BackwardsThruster:
			return new Vector3(0, bottomThrust, -backwardThrust);

            case PlayerMoveEnum.Undetermined:
            default:
                ThrusterBottom.Emit(0);
                ThrusterLeft.Emit(0);
                ThrusterRight.Emit(0);
                return new Vector3(0, 0, 0);
        }
    }
}