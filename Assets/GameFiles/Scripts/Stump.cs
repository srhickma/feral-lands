using UnityEngine;
using System.Collections;

public class Stump : MonoBehaviour {
	
	public int timeToWait = 100, waitedTime = 0, lastClockTime = 0;
	public GameObject tree;
	private GameObject clockGO;
	private Clock clock;

	void Start () {
		clockGO = GameObject.Find("Clock");
		clock = clockGO.GetComponent("Clock") as Clock;
		timeToWait = (int) Random.Range(30F,42F);
	}

	void awake(){

	}

	void Update () {
		if(lastClockTime != clock.hour)waitedTime ++;
		if(waitedTime > timeToWait){
			transform.eulerAngles = new Vector3(-90, 0, 0);
			Instantiate(tree, transform.position, transform.rotation);
			Destroy(gameObject);
		}
		lastClockTime = clock.hour;
	}

}
