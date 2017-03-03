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
		Debug.Log("Start->stepsize: " + stepSize);
		//int id = Random.Range(0,1000);

		aName = " -[ " + DateTime.Now.Millisecond +" ]";
		Debug.Log("Name:  " + aName);
		Debug.Log("Name:  " + name);
		Debug.Log("active:  " + Active);
	}

	// Update is called once per frame
	void Update () {
		

		if (Active) {
			
			Renderer rend = GetComponent<Renderer>();
			rend.material.shader = Shader.Find("Specular");
			rend.material.SetColor("_SpecColor", Color.red);

			if (calcEnd) {
				Vector3 target = transform.position;
				Debug.Log("currentPos: " + target);
				calcEnd=false;
			}

			if (!stepping) {
				if (Input.GetKey("i")) {
					target = new Vector3(transform.position.x, transform.position.y, transform.position.z + stepSize); 
					stepping = true;
					Debug.Log("Target,i: " + target);
				}

				if (Input.GetKey("k")) {
					target = new Vector3(transform.position.x, transform.position.y, transform.position.z - stepSize);
					stepping = true;
					Debug.Log("Target,k: " + target);
				}
			}

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


