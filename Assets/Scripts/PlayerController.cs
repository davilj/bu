using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	public  float speed;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
	}

	void FixedUpdate() {

		float moveHorizontal = Input.GetAxis("Horizontal");
		//Debug.Log("MH: " + moveHorizontal);

		float moveVertical = Input.GetAxis("Vertical");
		//Debug.Log("MV: " + moveVertical);

		Vector3 movement = new Vector3(0.0f, 0.0f, moveVertical); 
		//rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		if (Math.Abs(moveHorizontal) > Math.Abs(moveVertical)) {
			movement = new Vector3(moveHorizontal, 0.0f, 0.0f); 
			//rb.constraints = RigidbodyConstraints.FreezeRotationX ;
		} 

		rb.AddForce(movement * speed);
		//Vector3 currentPos = rb.transform.position;

		//Vector3 forcePos = currentPos + new Vector3(0,0,1);
		//rb.AddForceAtPosition(movement * speed, forcePos);
		//transform.rot

	}




		




}
