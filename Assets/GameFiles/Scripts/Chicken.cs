using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour {
	
	private GameObject playerGO;
	public GameObject crateGO, footsteps;
	Player player;
	NavMeshAgent agent;
	public int health = 60;
	private bool alive = true, atDest = true, agro = false;
	private float deathT = 0f, waitT = 0f, wait = 0f, agroT = 0f, audioT = 0f;
	public AudioClip[] chickenA = new AudioClip[4], grassA = new AudioClip[4];
	private AudioSource audioSource, fAudioSource;
	
	void Start () {
		playerGO = GameObject.Find("Player");
		agent = gameObject.GetComponent<NavMeshAgent>();
		player = playerGO.GetComponent("Player") as Player;
	}
	
	void Awake () {
		playerGO = GameObject.Find("Player");
		agent = gameObject.GetComponent<NavMeshAgent>();
		player = playerGO.GetComponent("Player") as Player;
		Vector3 destination = new Vector3(transform.position.x + Random.Range(-20.0f, 20.0f), transform.position.y, transform.position.z + Random.Range(-20.0f, 20.0f));
		agent.SetDestination(destination);
		audioSource = GetComponent<AudioSource>();
		fAudioSource = footsteps.GetComponent<AudioSource>();
	}
	
	void Update () {
		if(Vector3.Distance(transform.position, playerGO.transform.position) > 250){
			foreach (Transform child in transform)
				child.gameObject.SetActive(false);
		}
		else{
			foreach (Transform child in transform)
				child.gameObject.SetActive(true);
		}
		if(alive){
			if(!audioSource.isPlaying && audioT < 0f){
				audioSource.clip = chickenA[UnityEngine.Random.Range(0, 4)];
				audioT = UnityEngine.Random.Range(1f, 15f);
				audioSource.Play();
			}
			else if(audioT >= 0f)audioT -= Time.deltaTime;
			if(agro){
				agent.speed = 6f;
				agroT += Time.deltaTime;
				if(agroT > 10f){
					agro = false;
					agroT = 0f;
				}
			}
			else agent.speed = 3f;
			if(agent.velocity.sqrMagnitude > 0.1f && !atDest)wait = Random.Range(1.0f, 6.0f);
			atDest = !(agent.velocity.sqrMagnitude > 0.1f);
			if(atDest){
				waitT += Time.deltaTime;
				if(waitT > wait || agro){
					Vector3 destination;
					if(!agro)destination = new Vector3(transform.position.x + Random.Range(-5.0f, 5.0f), transform.position.y, transform.position.z + Random.Range(-5.0f, 5.0f));
					else destination = new Vector3(transform.position.x + 2 * Random.Range(-10.0f, 10.0f), transform.position.y, transform.position.z + 2 * Random.Range(-10.0f, 10.0f));
					agent.SetDestination(destination);
					waitT = 0f;
				}
			}
			else{
				GetComponent<Animation>().Play("ChickenRun");
				if(!fAudioSource.isPlaying){
					fAudioSource.clip = grassA[Random.Range(0, 4)];
					fAudioSource.Play();
				}
			}
		}
		else{
			deathT += Time.deltaTime;
			if(deathT > 10f)Destroy(gameObject);
		}
	}
	
	public void lowerHealth(int damage){
		health -= damage;
		agro = true;
		agroT = 0f;
		if(health < 1 && alive){
			alive = false;
			dropItem(new Vector2(10, 1));
			dropItem(new Vector2(7, 2));
			GetComponent<Animation>().Stop("CheckenRun");
			GetComponent<Animation>().Play("ChickenDie");
			agent.SetDestination(transform.position);
			(gameObject.GetComponent("BoxCollider") as BoxCollider).enabled = false;
			AnimalSpawner.animals --;
		}
	}
	
	void dropItem(Vector2 v){
		Ray r = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), -transform.up);
		RaycastHit h;
		if(Physics.Raycast(r, out h)){
			GameObject c = null;
			if(h.collider.gameObject.name == "Crate(Clone)")c = Instantiate(crateGO, new Vector3(transform.position.x, h.collider.gameObject.transform.position.y, transform.position.z), new Quaternion(0,0,0,0)) as GameObject;
			else c = Instantiate(crateGO, new Vector3(transform.position.x, h.point.y + 0.25f, transform.position.z), new Quaternion(0,0,0,0)) as GameObject;
			(c.GetComponent("Crate") as Crate).spawn(v);
		}
	}
	
	public void loadHealth(int health){
		this.health = health;
	}

}
