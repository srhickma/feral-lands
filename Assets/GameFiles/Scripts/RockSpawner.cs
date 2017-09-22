using UnityEngine;
using System.Collections;

public class RockSpawner : MonoBehaviour {

	public GameObject rock1, rock2, rock3;
	private GameObject clockGO;
	private Clock clock;
	public bool loadComplete = false;
	public bool[] spawnUsed = new bool[58];
	private int nextSpawn = 1, lastClockTime = 0;
	private float curTime = 0;

	void Start () {
		clockGO = GameObject.Find("Clock");
		clock = clockGO.GetComponent("Clock") as Clock;
	}

	void Update () {
		if(lastClockTime != clock.hour && loadComplete){
			for(int i = 0; i < 4; i ++){
				int r = (int) Random.Range(0F, 58F);
				if(!spawnUsed[r]){
					spawnRock(r);
					spawnUsed[r] = true;
				}
			}
			curTime = 0;
		}
		lastClockTime = clock.hour;
	}

	public void spawn(){
		for(int i = 0; i < 58; i ++){
			if(!spawnUsed[i]){
				spawnRock(i);
				spawnUsed[i] = true;
			}
		}
	}

	void spawnRock(int i){
		int r = (int) Random.Range(1F, 5F);
		GameObject randomSpawn = GameObject.Find("Rock" + (i + 1).ToString()), newRock;
		if(r == 1)newRock = Instantiate(rock1, randomSpawn.transform.position, randomSpawn.transform.rotation) as GameObject;
		else if(r == 2)newRock = Instantiate(rock2, randomSpawn.transform.position, randomSpawn.transform.rotation) as GameObject;
		else newRock = Instantiate(rock3, randomSpawn.transform.position, randomSpawn.transform.rotation) as GameObject;
		Rock newRockScript = newRock.GetComponent("Rock") as Rock;
		newRockScript.spawnIndex = i;
		newRockScript.rockSpawner = this;
	}

	public void loadRock(int i, int r){
		spawnUsed[i] = true;
		GameObject randomSpawn = GameObject.Find("Rock" + (i + 1).ToString()), newRock;
		if(r == 1)newRock = Instantiate(rock1, randomSpawn.transform.position, randomSpawn.transform.rotation) as GameObject;
		else if(r == 2)newRock = Instantiate(rock2, randomSpawn.transform.position, randomSpawn.transform.rotation) as GameObject;
		else newRock = Instantiate(rock3, randomSpawn.transform.position, randomSpawn.transform.rotation) as GameObject;
		Rock newRockScript = newRock.GetComponent("Rock") as Rock;
		newRockScript.spawnIndex = i;
		newRockScript.rockSpawner = this;
	}

}
