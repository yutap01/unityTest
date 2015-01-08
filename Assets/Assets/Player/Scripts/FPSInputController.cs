using UnityEngine;
using System.Collections;

public class FPSInputController : MonoBehaviour {
	
	private CharacterMotor motor;
	private float jumpPressedTime = -100;

	// Use this for initialization
	void Start () {
		motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		// Get the input vector from kayboard or analog stick
		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		if (direction != Vector3.zero) {
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = direction.magnitude;
			direction = direction / directionLength;
		
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
		
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
		
			// Multiply the normalized direction vector by the modified length
			direction = direction * directionLength;
		}
	
		// Apply the direction to the CharacterMotor
		motor.inputMoveDirection = transform.TransformDirection(direction);
		
		if(Input.GetButtonDown("Jump")) {
			jumpPressedTime = Time.time;
		}
		if( !Input.GetButton("Jump") ) {
			jumpPressedTime = -100;
		}
		motor.inputJump = Time.time - jumpPressedTime <= 0.2f;
		if(motor.IsJumping() && !motor.IsGrounded()) {
			motor.inputJump = Input.GetButton("Jump");
		}
	}
	
	void OnJump() {
		jumpPressedTime = -100;
	}
	
	
}
