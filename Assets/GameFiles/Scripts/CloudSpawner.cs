using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class CloudSpawner : MonoBehaviour {

    float spawnT = 0, startT = 0;
    bool started = false;
    public GameObject cloud;
    
	void Start () {
	    
	}
	
	void Update () {
        spawnT += Time.deltaTime;
        if(startT > 1.5f)started = true;
        else startT += Time.deltaTime;
        if(spawnT > 10f || (!started && spawnT > 0.01f)){
            spawnCloud();
            spawnT = 0f;
        }
	    foreach(GameObject g in GameObject.FindGameObjectsWithTag("cloud")){
            float speed = (750f - g.transform.position.y) / 50f;
            if(!started)speed *= 1000;
            g.transform.position += new Vector3(Time.deltaTime / 5f * speed, 0f, Time.deltaTime * speed);
            if(g.transform.position.z > 4000f)Destroy(g);
        }
	}

    public void spawnCloud(){
        bool goodSpawn = true;
        int x = 0, y = 0, z = 0;
        x = Random.Range(-1000, 3000);
        y = Random.Range(400, 600);
        z = Random.Range(-2100, -2000);
        foreach(Collider c in  Physics.OverlapSphere(new Vector3(x, y, z), Random.Range(1, 400))){
            if(c.gameObject.tag == "cloud"){
                goodSpawn = false;
                break;
            }
        }
        if(goodSpawn){
            GameObject c = Instantiate(cloud, new Vector3(x, y, z), new Quaternion(0,0,0,0))as GameObject;
            c.transform.localScale = new Vector3(Random.Range(100, 500), Random.Range(20, 60), Random.Range(100, 500));
        }
    }

}
