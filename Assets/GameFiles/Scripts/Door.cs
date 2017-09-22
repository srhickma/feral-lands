using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	
	public string creator;
	public float origRot;
	bool open = false;

	void Start () {
	
	}

	void Update () {
	
	}

	public bool attemptOpen(string name){
		if(name == creator || gameObject.name == "WoodDoor(Clone)"){
			Vector3 move;
			if(origRot == 90f)move = new Vector3(-0.8125f, 0f, 0.8125f);
			else move = new Vector3(-0.8125f, 0f, -0.8125f);
			if(!open){
				transform.eulerAngles += new Vector3(0, 90, 0);
				transform.position += move;
				open = true;
			}
			else{
				transform.eulerAngles -= new Vector3(0, 90, 0);
				transform.position -= move;
				open = false;
			}
			return true;
		}
		return false;
	}

}
