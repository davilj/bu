using UnityEngine;
using System.Collections;

public class HoldObject : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter (Collider collider) {
		collider.transform.parent=gameObject.transform.parent;
		//collider.transform.localScale.x = 1/gameObject.transform.localScale.x;
		//gameObject.transform.localScale.y = 1/collider.transform.localScale.y;
		//gameObject.transform.localScale.z = 1/collider.transform.localScale.z;
		//collider.transform.localScale = Vector3.one;
	}
	
	// Update is called once per frame
	void OnTriggerExit(Collider collider) {
		collider.transform.parent=null;
	}
}
