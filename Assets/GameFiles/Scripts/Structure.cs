using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Structure : MonoBehaviour {

	int[] durabilities = new int[]{10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000};
	public string creator;
	public int durability, type;
	private float decayTime = 0f;
	public bool isWood = false, noDurability = false;
	private Player player;
	
	void Start () {
	
	}

	void Update () {
		if(isWood){
			decayTime += Time.deltaTime;
			if(decayTime > 1f && !noDurability){
				lowerDurability(1, false);
				decayTime = 0f;
			}
			else if(decayTime > Random.Range(10f, 200f) && !noDurability){

			}
		}
	}

	public void initialize(int type, string creator){
		this.creator = creator;
		durability = durabilities[type];
		isWood = type < 8;
		this.type = type + 70;
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	public void load(int type, string creator, int durability){
		this.creator = creator;
		this.durability = durability;
		isWood = type < 8;
		this.type = type + 70;
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	public void lowerDurability(int ammount, bool isPlayer){
		durability -= ammount;
		if(durability < 1)destroy(isPlayer);
	}

	public void destroy(bool isPlayer){
		noDurability = true;
		bool destroyable = true;
		int sType = (type - 69) % 8;
		if(sType == 1){
			foreach(Collider c in Physics.OverlapSphere(transform.position + new Vector3(0, 1f, 0), 5f)){
				if(c.transform.position.y >= transform.position.y){
					if(c.gameObject.name == "Wall" || c.gameObject.name == "Window" || c.gameObject.name == "Doorway" || c.gameObject.name == "Stairs" || c.gameObject.tag == "door"){
						destroyable = false;
						break;
					}
				}
			}
		}
		else if(sType == 2){
			foreach(Collider c in Physics.OverlapSphere(transform.position + new Vector3(0, 1.5f, 0), 2f)){
				if((c.gameObject.tag == "pillar" && c.gameObject != gameObject) || c.gameObject.tag == "platform" || c.gameObject.name == "Wall" || c.gameObject.name == "Window" || c.gameObject.name == "Doorway"){
					destroyable = false;
					break;
				}
			}
		}
		else if(sType == 4){
			foreach(Collider c in Physics.OverlapSphere(transform.position, 2f)){
				if(c.gameObject.tag == "door")c.gameObject.GetComponent<Structure>().destroy(isPlayer);
			}
		}
		else if(sType == 7){
			foreach(Collider c in Physics.OverlapSphere(transform.position + new Vector3(0f, 4f, 0f), 8f)){
				if(c.gameObject.tag == "pillar"){
					int foundations = 0;
					foreach(Collider cb in Physics.OverlapSphere(c.transform.position + new Vector3(0f, -1.5f, 0f), 3f)){
						print(cb.gameObject.name);
						if(cb.gameObject.name == "RockFPillar")foundations ++;
					}
					if(foundations == 1)destroyable = false;
				}
				else if(c.gameObject.tag == "platform")destroyable = false;
			}
		}
		if(destroyable){
			player.getItem(type, 1);
			Destroy(gameObject);
		}
		else if(isPlayer)player.addChat("Structure cannot be destroyed");
	}
	
}
