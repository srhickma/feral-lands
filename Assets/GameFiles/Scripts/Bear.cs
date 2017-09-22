using UnityEngine;
using System.Collections;

public class Bear : MonoBehaviour {
	
	private GameObject playerGO;
	public GameObject crateGO, head, footsteps;
	Player player;
	NavMeshAgent agent;
	public int health = 280;
	public bool alive = true, atDest = true, agro = false, atPlayer = false;
	private float deathT = 0f, waitT = 0f, wait = 0f, agroT = 0f, atT = 0f, audioT = 0f;
	public AudioClip[] bearA = new AudioClip[4], bearAgroA = new AudioClip[3], grassA = new AudioClip[4];
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
		randomMove();
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
			Vector3 relativePos = playerGO.transform.position - transform.position;
			Quaternion targetRot = Quaternion.LookRotation(relativePos);
			Quaternion relative = Quaternion.Inverse(transform.rotation) * targetRot;
			if((Vector3.Distance(transform.position, playerGO.transform.position) < 30 && (relative.eulerAngles.y < 80 || relative.eulerAngles.y > 280)) || Vector3.Distance(transform.position, playerGO.transform.position) < 10){
				agro = true;
				agroT = 0f;
			}
		}
		if(alive){
			if(!audioSource.isPlaying && audioT < 0f){
				float maxT = 15f;
				if(!agro)audioSource.clip = bearA[UnityEngine.Random.Range(0, 4)];
				else{
					audioSource.clip = bearAgroA[UnityEngine.Random.Range(0, 3)];
					if(atPlayer)maxT = 0.1f;
					else maxT = 5f;
				}
				audioT = UnityEngine.Random.Range(0f, maxT);
				audioSource.Play();
			}
			else if(audioT >= 0f)audioT -= Time.deltaTime;
			if(agro && !atPlayer){
				agent.SetDestination(player.transform.position);
				agent.speed = 11f;
				agroT += Time.deltaTime;
				if(agroT > 10f){
					randomMove();
					agro = false;
					agroT = 0f;
				}
				if(Vector3.Distance(transform.position, playerGO.transform.position) > 80){
					agro = false;
					randomMove();
				}
			}
			else agent.speed = 5f;
			if(agent.velocity.sqrMagnitude > 0.1f && !atDest)wait = Random.Range(5.0f, 10.0f);
			atPlayer = Vector3.Distance(transform.position, playerGO.transform.position) < 3;
			atDest = !(agent.velocity.sqrMagnitude > 0.1f);
			if(atPlayer){
				Quaternion Rot = transform.rotation;
				transform.LookAt(new Vector3(playerGO.transform.position.x, transform.position.y, playerGO.transform.position.z));
				transform.rotation = Quaternion.RotateTowards(Rot, transform.rotation, 200 * Time.deltaTime);
				agent.enabled = false;
				GetComponent<Animation>().Play("BearAttack");
				atT += Time.deltaTime;
				if(atT > 1f){
					playerGO.SendMessage("recieveDamage", Random.Range(20, 40));
					atT = 0f;
				}
			}
			else{
				agent.enabled = true;
				head.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
				if(atT < 40f)atT += Time.deltaTime;
			}
			if(atDest || atPlayer){
				waitT += Time.deltaTime;
				if((waitT > wait || agro) && !atPlayer){
					randomMove();
					waitT = 0f;
				}
			}
			else{
				GetComponent<Animation>().Play("BearRun");
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

	void randomMove(){
		Vector3 destination = new Vector3(transform.position.x + Random.Range(-20.0f, 20.0f), transform.position.y, transform.position.z + Random.Range(-20.0f, 20.0f));
		agent.SetDestination(destination);
	}
	
	public void lowerHealth(int damage){
		health -= damage;
		agro = true;
		agroT = 0f;
		if(health < 1 && alive){
			alive = false;
			dropItem(new Vector2(10, 5));
			dropItem(new Vector2(8, 5));
			GetComponent<Animation>().Stop("BearRun");
			GetComponent<Animation>().Play("BearDie");
			agent.enabled = false;
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
