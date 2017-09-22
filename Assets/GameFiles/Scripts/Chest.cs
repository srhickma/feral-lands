using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	public Vector2[] inv = new Vector2[25];

	void Start () {
	
	}
	
	void Update () {
		
	}

	public void loadInv(Vector2[] inv){
		this.inv = inv;
	}

}
