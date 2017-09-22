using UnityEngine;
using System.Collections;

public class Deer : MonoBehaviour {

	private GameObject playerGO;
	public GameObject crateGO, body, footsteps;
	Player player;
	NavMeshAgent agent;
	public int health = 160;
	private int startT = 0;
	private bool alive = true, atDest = true, agro = false;
	private float deathT = 0f, waitT = 0f, wait = 0f, agroT = 0f, audioT = 0f;
	public AudioClip[] deerA = new AudioClip[4], grassA = new AudioClip[4];
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
		Vector3 destination = new Vector3(transform.position.x + Random.Range(-40.0f, 40.0f), transform.position.y, transform.position.z + Random.Range(-40.0f, 40.0f));
		agent.SetDestination(destination);
		audioSource = GetComponent<AudioSource>();
		fAudioSource = footsteps.GetComponent<AudioSource>();
	}
	
	void Update () {
		startT ++;
		if(Vector3.Distance(transform.position, playerGO.transform.position) > 250 && startT > 20){
			foreach (Transform child in transform)
				child.gameObject.SetActive(false);
		}
		else{
			foreach (Transform child in transform)
				child.gameObject.SetActive(true);
		}
		if(alive){
			Vector3 relativePos = playerGO.transform.position - transform.position;
			Quaternion targetRot = Quaternion.LookRotation(relativePos);
			Quaternion relative = Quaternion.Inverse(transform.rotation) * targetRot;
			if((Vector3.Distance(transform.position, playerGO.transform.position) < 15 && (relative.eulerAngles.y < 80 || relative.eulerAngles.y > 280)) || Vector3.Distance(transform.position, playerGO.transform.position) < 5){
				agro = true;
				body.GetComponent<Animation>().Stop("DeerWalk");
			}
			if(agro){
				if(audioT <= 0 && !audioSource.isPlaying){
					audioSource.clip = deerA[Random.Range(0, 4)];
					audioSource.Play();
					audioT = Random.Range(0f, 6f);
				}
				else audioT -= Time.deltaTime;
				body.GetComponent<Animation>().Play("DeerRun");
				agent.speed = 20f;
				agroT += Time.deltaTime;
				if(agroT > 10f){
					agro = false;
					agroT = 0f;
				}
			}
			else{
				agent.speed = 5f;
				audioT = 0f;
			}
			if(agent.velocity.sqrMagnitude > 0.1f && !atDest)wait = Random.Range(1.0f, 10.0f);
			atDest = !(agent.velocity.sqrMagnitude > 0.1f);
			if(atDest){
				waitT += Time.deltaTime;
				if(waitT > wait || agro){
					Vector3 destination;
					if(!agro)destination = new Vector3(transform.position.x + Random.Range(-20.0f, 20.0f), transform.position.y, transform.position.z + Random.Range(-20.0f, 20.0f));
					else destination = new Vector3(transform.position.x + 2f * Random.Range(-40.0f, 40.0f), transform.position.y, transform.position.z + 2f * Random.Range(-40.0f, 40.0f));
					agent.SetDestination(destination);
					waitT = 0f;
				}
			}
			else{
				if(!agro)body.GetComponent<Animation>().Play("DeerWalk");
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
		body.GetComponent<Animation>().Stop("DeerWalk");
		health -= damage;
		if(!agro){
			audioSource.clip = deerA[Random.Range(0, 4)];
			audioSource.Play();
			audioT = Random.Range(0f, 6f);
		}
		agro = true;
		agroT = 0f;
		if(health < 1 && alive){
			alive = false;
			dropItem(new Vector2(10, 3));
			dropItem(new Vector2(8, 3));
			body.GetComponent<Animation>().Stop("DeerRun");
			body.GetComponent<Animation>().Play("DeerDie");
			agent.SetDestination(transform.position);
			(gameObject.GetComponent("BoxCollider") as BoxCollider).enabled = false;
			AnimalSpawner.animals --;
		}
	}
	
	void dropItem(Vector2 v){
		Ray r = new Ray(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), -transform.up);
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
