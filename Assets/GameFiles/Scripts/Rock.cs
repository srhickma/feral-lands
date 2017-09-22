using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	public int health = 100, spawnIndex = 0, type;
	private bool dead = false;
	public GameObject rock2, rock3, nextRock;
	private GameObject playerGO;
	private Player player;
	public RockSpawner rockSpawner;
	public AudioClip rockHitA, rockBreakA;
	private AudioSource audioSource;

	void Start () {

	}

	void Awake(){
		playerGO = GameObject.Find("Player");
		player = playerGO.GetComponent("Player") as Player;
		if(gameObject.name == "Rock1(Clone)"){
			nextRock = rock2;
			type = 1;
		}
		else if(gameObject.name == "Rock2(Clone)"){
			nextRock = rock3;
			type = 2;
		}
		else{
			nextRock = null;
			type = 3;
		}
		audioSource = GetComponent<AudioSource>();
	}

	void Update () {
		if(health < 1 && !dead){
			if(nextRock != null){
				GameObject newRock = Instantiate(nextRock, transform.position, transform.rotation) as GameObject;
				Rock newRockScript = newRock.GetComponent("Rock") as Rock;
				newRockScript.spawnIndex = spawnIndex;
				newRockScript.rockSpawner = rockSpawner;
			}
			else rockSpawner.spawnUsed[spawnIndex] = false;
			int r = (int) Random.Range(0F, 20F);
			player.getItem(2, 5);
			if(r > 15)player.getItem(5, r - 15);
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<MeshCollider>().enabled = false;
			dead = true;
		}
		else if(dead && !audioSource.isPlaying){
			Destroy(gameObject);
		}
	}

	public void lowerHealth(int ammount){
		health -= ammount;
		if(health > 0)audioSource.clip = rockHitA;
		else audioSource.clip = rockBreakA;
		audioSource.Play();
	}

}
