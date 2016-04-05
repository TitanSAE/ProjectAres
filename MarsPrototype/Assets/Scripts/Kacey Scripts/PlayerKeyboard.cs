using UnityEngine;
using System.Collections;

/// <summary>
/// Component of Level1 scenes root "Ship" gameobject
/// Moves the ship in response to the player pressing the keyboards arrow keys
/// </summary>
public class PlayerKeyboard : MonoBehaviour
{
	public Camera mainCamera;
	public Camera roverCamera;
	bool toggle = false;
	public GameObject crossHair;
    private void Start()
    {
    }

    public static bool IsKeyboardThrustersOn
    {
        get
        {
            return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }
    }

    private void Update()
    {
		if (GameObject.Find ("GameManager").GetComponent<GameManager> ().copterControl == true) {
			bool isPressed = PlayerKeyboard.IsKeyboardThrustersOn;

			if (Input.GetKeyUp (KeyCode.C)) {

				toggle = !toggle;
				if (toggle == false) {
					crossHair.gameObject.SetActive (true);
				} else {
					crossHair.gameObject.SetActive (false);
				}
				mainCamera.gameObject.SetActive (toggle);
				Debug.Log ("Pressed");
			}
			if (Thrusters.OfShipInitialized) {
				//determine how to move the ship
				PlayerMoveEnum choice = PlayerMoveEnum.Undetermined;
				if (Input.GetAxis ("Horizontal") > 0) { //right
					choice = PlayerMoveEnum.LeftThruster;
				} else if (Input.GetAxis ("Horizontal") < 0) { //left
					choice = PlayerMoveEnum.RightThruster;
				}

				if (Input.GetAxis ("Vertical") > 0) {
					choice = PlayerMoveEnum.ForwardThruster;
					Debug.Log (choice);
				} else if (Input.GetAxis ("Vertical") < 0) {
					choice = PlayerMoveEnum.BackwardsThruster;
				}	

				if (Input.GetKey (KeyCode.Space)) { //up
					Debug.Log ("Pressed");
					//retain horizontal movement in choice
					choice = (PlayerMoveEnum)((int)PlayerMoveEnum.BottomThruster + (int)choice);
				}

				//determine how much to move in choice direction
				Thrusters thrusters = new Thrusters () {
					ThrusterBottom = Globals.BottomThruster,
					ThrusterLeft = Globals.LeftThruster,
					ThrusterRight = Globals.RightThruster,
				};
				Vector3 shipForce = thrusters.ThrustOn (choice);

				//move the ship
				Globals.PlayerShip.AddForce (shipForce);
			}
		}
    }
}