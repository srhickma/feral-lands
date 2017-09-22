using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {

	public GameObject branches;
	public GameObject stump;
	private GameObject playerGO;
	private Player player;
	public int health = 100, distance = 250;

	void Start () {
		playerGO = GameObject.Find("Player");
		player = playerGO.GetComponent("Player") as Player;
		if(QualitySettings.GetQualityLevel() == 5)distance = 500;
	}

	void Update () {
		if(Vector3.Distance(transform.position, playerGO.transform.position) > distance)branches.SetActive(false);
		else branches.SetActive(true);
		if(health < 1){
			Instantiate(stump, transform.position, new Quaternion(0,0,0,0));
			player.getItem(1, 5);
			player.getItem(3, 5);
			player.getItem(4, 10);
			Destroy(gameObject);
		}
	}

	public void lowerHealth(int ammount){health -= ammount;}

	public void loadStump(){
		Instantiate(stump, transform.position, new Quaternion(0,0,0,0));
		Destroy(gameObject);
	}

}
