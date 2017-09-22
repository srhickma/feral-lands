using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public GameObject bloodSplatter, hitO;
	bool hit = false;
	float y, z, hitT = 0f;
	int damage = 0;
	Vector3 startForward;

	void Start () {

	}

	void FixedUpdate(){
		if(!hit)transform.eulerAngles = new Vector3(Quaternion.LookRotation(Vector3.Slerp(startForward, GetComponent<Rigidbody>().velocity.normalized, Time.deltaTime)).eulerAngles.x, y, z);
		else{
			hitT += Time.deltaTime;
			if(hitT > 20f || hitO == null)Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision){
		if(collision.collider.gameObject.tag != "Player"){
			hitO = collision.collider.gameObject;
			hit = true;
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponent<Collider>().enabled = false;
			if(collision.collider.gameObject.tag == "animal"){
				collision.collider.gameObject.SendMessage("lowerHealth", damage);
				Instantiate(bloodSplatter, transform.position, new Quaternion(0,0,0,0));
				Destroy(gameObject);
			}
		}
	}

	void setDamage(int d){
		damage = d;
		startForward = transform.forward;
		y = transform.eulerAngles.y;
		z = transform.eulerAngles.z;
	}

}
