using UnityEngine;
using System.Collections;

public class BloodSplatter : MonoBehaviour {

	private float t = 0f;
	
	void Start () {
	
	}

	void Update () {
		t += Time.deltaTime;
		if(t > 1.5f)Destroy(gameObject);
	}
	
}
