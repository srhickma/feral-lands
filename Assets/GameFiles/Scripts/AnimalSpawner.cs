using UnityEngine;
using System.Collections;

public class AnimalSpawner : MonoBehaviour {

	public GameObject boar, bear, deer, rabbit, chicken;
	public NavMesh mesh;
	public static int animals;
	int maxAnimals = 20;
	bool ready = false;
	float pasT = 0f, waitT = 0f;
	public bool loadComplete = false;
	
	void Start () {

	}

	void Update () {
		if(animals < maxAnimals && loadComplete){
			pasT += Time.deltaTime;
			if(pasT > waitT){
				pasT = 0f;
				spawnAnimal();
			}
			waitT = Random.Range(0f, 20f);
		}
	}

	public void spawn(){
		for(int i = 0; i < 20; i ++)
			spawnAnimal();
	}

	void spawnAnimal(){
		float x, z;
		bool foundSpawn = false;
		GameObject animal;
		NavMeshHit h;
		do{
			x = Random.Range(0, 1000) - 500;
			z = Random.Range(0, 1000) - 500;
			if(NavMesh.SamplePosition(new Vector3(1000f + x, 100f, 1000f + z), out h, 100, -1))foundSpawn = true;
		}while(!foundSpawn);
		int type = Random.Range(0, 100);
		if(type < 30)animal = boar;
		else if(type < 45)animal = deer;
		else if(type < 55)animal = rabbit;
		else if(type < 70)animal = chicken;
		else animal = bear;
		Instantiate(animal, h.position, new Quaternion(0,0,0,0));
		animals ++;
	}

}
