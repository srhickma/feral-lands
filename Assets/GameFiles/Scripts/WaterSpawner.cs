using UnityEngine;
using System.Collections;

public class WaterSpawner : MonoBehaviour {

	public GameObject water;
	GameObject w;

	void Start () {
		for(int x = 0; x < 20; x ++){
			for(int z = 0; z < 20; z ++){
				if((x > 17 || x < 2) || (z > 17 || z < 2))w = Instantiate(water, new Vector3(x * 100 + 50, 1, z * 100 + 50), new Quaternion(0,0,0,0)) as GameObject;
			}
		}
	}

	void Update () {
	
	}

}
