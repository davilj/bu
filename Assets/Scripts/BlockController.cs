using UnityEngine;
using System.Collections;
using System;


public class BlockController : MonoBehaviour {
	public bool Active;
	public string aName;
	bool stepping;
	bool calcEnd;
	int k=6;
	float stepSize;
	Vector3 target;


	// Use this for initialization
	void Start () {
		//active=false;
		stepSize = GetComponent<Renderer>().bounds.size.z;
		//Debug.Log("Start->stepsize: " + stepSize);
		//int id = Random.Range(0,1000);

		aName = " -[ " + DateTime.Now.Millisecond +" ]";
		//Debug.Log("Name:  " + aName);
		//Debug.Log("Name:  " + name);
		//Debug.Log("active:  " + Active);
	}

	public void MoveBlock(float newX, float newY, float newZ) {
		Debug.Log("Moving block in block controller (" + newX + ", " + newY + ")");
		target = new Vector3(newX, newY, newZ); 
		stepping=true;
	}

	public void removeNorth() {
		Debug.Log("remvoe a bridge: North");
		deactivateBridge("BridgeNorth");
	}

	public void addNorth() {
		Debug.Log("add a bridge: North");
		activateBridge("BridgeNorth");
	}

	public void removeSouth() {
		Debug.Log("remvoe a bridge: South");
		deactivateBridge("BridgeSouth");
	}

	public void addSouth() {
		Debug.Log("add a bridge: south");
		activateBridge("BridgeSouth");
	}

	public void removeWest() {
		Debug.Log("remvoe a bridge: west");
		deactivateBridge("BridgeWest");
	}

	public void addWest() {
		Debug.Log("adding a bridge: West");
		activateBridge("BridgeWest");
	}

	public void removeEast() {
		Debug.Log("remvoe a bridge: east");
		deactivateBridge("BridgeEast");
	}

	public void addEast() {
		Debug.Log("adding a bridge: East");
		activateBridge("BridgeEast");
	}

	public void removeAll() {
		removeNorth();
		removeSouth();
		removeWest();
		removeEast();
	}

	private void activateBridge(string bridgeName) {
		transform.FindChild(bridgeName).gameObject.SetActive(true);
	}

	private void deactivateBridge(string bridgeName) {
		transform.FindChild(bridgeName).gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (Active) {
			
			Renderer rend = GetComponent<Renderer>();
			rend.material.shader = Shader.Find("Specular");
			rend.material.SetColor("_SpecColor", Color.red);

			if (stepping) {
				float step = k * Time.deltaTime;

				transform.position=Vector3.MoveTowards(transform.position, target, step);
				//Debug.Log("transform.position--->: " + transform.position);
				//Debug.Log("transform.target--->: " + target);
				if (target==transform.position) {
					stepping=false;
					calcEnd=true;
					Debug.Log("reach the end");
					Debug.Log("Target: " + target);
					Debug.Log("New Pos: " + transform.position);
				}
			}
		} else {
			Renderer rend = GetComponent<Renderer>();
			rend.material.shader = Shader.Find("Specular");
			rend.material.SetColor("_SpecColor", Color.green);
		}

	}
}


