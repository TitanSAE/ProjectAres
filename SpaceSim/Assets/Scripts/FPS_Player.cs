using UnityEngine;
using System.Collections;

//Modified from Unity Standard Assets

[RequireComponent (typeof (CharacterController))]
public class FPS_Player: MonoBehaviour {

	public float fWalkSpeed = 6.0f;
	public float fRunSpeed = 11.0f;
	public bool bLimitDiagonalSpeed = true;
	public bool bRunToggleMode = false;
	public float fJumpSpeed = 8.0f;
	public float fGravity = 20.0f;
	public float fFallDamageThreshold = 10.0f;
	public bool bSlideWhenOverSlopeLimit = false;
	//Uses "Slide" tag
	public bool bSlideOnTaggedObjects = false;
	public float fSlideSpeed = 12.0f;
	public bool bAirControl = false;
	public float fAntiSlopeStutter = 0.75f;
	public int iAntiBunnyHop = 1;
	public bool bFrozen = false;

	private Vector3 vMoveDirection = Vector3.zero;
	private bool bGrounded = false;
	private CharacterController controller;
	private Transform localtrans;
	private float fSpeed;
	private RaycastHit hit;
	private float fFallStartLevel;
	private bool bFalling;
	private float fSlideLimit;
	private float fRayLength;
	private Vector3 vContactPoint;
	private bool bPlayerControl = false;
	private int iJumpTimer;

	void Start() {
		controller = GetComponent<CharacterController>();
		localtrans = transform;
		fSpeed = fWalkSpeed;
		fRayLength = controller.height * 0.5f + controller.radius;
		fSlideLimit = controller.slopeLimit - 0.1f;
		iJumpTimer = iAntiBunnyHop;
	}

	void FixedUpdate() {
		float inputX = 0.0f;
		float inputY = 0.0f;
		float inputModifyFactor = 1.0f;

		if (!bFrozen) {
			inputX = Input.GetAxis("Horizontal");
			inputY = Input.GetAxis("Vertical");
			//Diagonal movement limit
			inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && bLimitDiagonalSpeed) ? 0.7071f : 1.0f;
		}

		if (bGrounded) {
			bool bSliding = false;
			//Should we slide?
			if (Physics.Raycast(localtrans.position, -Vector3.up, out hit, fRayLength)) {
				if (Vector3.Angle(hit.normal, Vector3.up) > fSlideLimit) {
					bSliding = true;
				}
			}
			//Double check straight down for really steep slopes
			else {
				Physics.Raycast(vContactPoint + Vector3.up, -Vector3.up, out hit);
				if (Vector3.Angle(hit.normal, Vector3.up) > fSlideLimit) {
					bSliding = true;
				}
			}

			//Fall damage
			if (bFalling) {
				bFalling = false;
				if (localtrans.position.y < fFallStartLevel - fFallDamageThreshold) {
					TakeFallingDamage(fFallStartLevel - localtrans.position.y);
				}
			}

			//Simple run speed if toggle mode is disabled
			if (!bRunToggleMode) {
				fSpeed = Input.GetButton("Run") ? fRunSpeed : fWalkSpeed;
			}

			//Sliding (if enabled)
			if ((bSliding && bSlideWhenOverSlopeLimit) || (bSlideOnTaggedObjects && hit.collider.tag == "Slide")) {
				Vector3 hitNormal = hit.normal;
				vMoveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize (ref hitNormal, ref vMoveDirection);
				vMoveDirection *= fSlideSpeed;
				bPlayerControl = false;
			}
			//Anti-bump
			else {
				vMoveDirection = new Vector3(inputX * inputModifyFactor, -fAntiSlopeStutter, inputY * inputModifyFactor);
				vMoveDirection = localtrans.TransformDirection(vMoveDirection) * fSpeed;
				bPlayerControl = true;
			}

			//Jump
			if (!Input.GetButton("Jump")) {
				iJumpTimer++;
			}
			else if (iJumpTimer >= iAntiBunnyHop) {
				vMoveDirection.y = fJumpSpeed;
				iJumpTimer = 0;
			}
		}
		//In the air
		else {
			//Start counting fall height
			if (!bFalling) {
				bFalling = true;
				fFallStartLevel = localtrans.position.y;
			}

			//Air control
			if (bAirControl && bPlayerControl) {
				vMoveDirection.x = inputX * fSpeed * inputModifyFactor;
				vMoveDirection.z = inputY * fSpeed * inputModifyFactor;
				vMoveDirection = localtrans.TransformDirection(vMoveDirection);
			}
		}

		//Apply gravity
		vMoveDirection.y -= fGravity * Time.deltaTime;

		//Grounded?
		bGrounded = (controller.Move(vMoveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;

		//Rocket Booster quickie
		if (Input.GetKey(KeyCode.Space)) {
			vMoveDirection.y = 5.0f;
		}
	}

	void Update() {
		//If the run button is set to toggle switch between walk/run speed
		if (bRunToggleMode && bGrounded && Input.GetButtonDown("Run"))
			fSpeed = (fSpeed == fWalkSpeed? fRunSpeed : fWalkSpeed);
	}
		
	void OnControllerColliderHit(ControllerColliderHit hit) {
		vContactPoint = hit.point;
	}
		
	void TakeFallingDamage(float fallDistance) {
		print ("Fell " + fallDistance);   
	}
}