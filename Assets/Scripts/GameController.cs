using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public Camera topCamera;
	public Camera gameCamera;
	private bool inTopMode;


	// Use this for initialization
	void Start () {
		inTopMode = false;
		switchCamera(inTopMode);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("t")) {
			inTopMode=true;
		};
			
		if (Input.GetKey("g")) {
			inTopMode=false;
		};
				
		switchCamera(inTopMode);
	}

	void switchCamera(bool inTopMode) {
		if (inTopMode) {
			topCamera.GetComponent<Camera>().enabled=true;
			gameCamera.GetComponent<Camera>().enabled=false;
		} else {
			topCamera.GetComponent<Camera>().enabled=false;
			gameCamera.GetComponent<Camera>().enabled=true;
		}
	}
}
