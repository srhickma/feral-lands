using UnityEngine;
using System.Collections;

public class Bow : MonoBehaviour {

	float time = 0, lz = 0;
	int power = 0;
	public GameObject makeshiftArrow, stoneArrow, ironArrow, usedArrow;
	public AudioClip bowDraw, bowRelease;
	private AudioSource bowAS;
	
	void Start () {
	
	}

	void Awake(){
		bowAS = GetComponent<AudioSource>();
	}

	void Update () {
	
	}

	public IEnumerator shoot(int type){
		int damage = 0, damageMult = 1;
		if(gameObject.transform.parent.gameObject.name == "Bow")power = 60;
		else{
			power = 90;
			damageMult = 2;
		}
		if(type == 35){
			usedArrow = makeshiftArrow;
			damage = damageMult * 20;
		}
		else if(type == 36){
			usedArrow = stoneArrow;
			damage = damageMult * 25;
		}
		else{
			usedArrow = ironArrow;
			damage = damageMult * 30;
		}
		GameObject arrowb = Instantiate(usedArrow, transform.position, transform.rotation) as GameObject;
		arrowb.GetComponent<Rigidbody>().isKinematic = true;
		arrowb.GetComponent<Rigidbody>().useGravity = false;
		arrowb.GetComponent<Collider>().enabled = false;
		arrowb.transform.Rotate(new Vector3(180,0,0));
		arrowb.transform.parent = gameObject.transform;
		bowAS.PlayOneShot(bowDraw);
		while(lz > -0.5f){
			yield return new WaitForSeconds(0.01f);
			lz -= 0.02f;
			arrowb.transform.localPosition = new Vector3(arrowb.transform.localPosition.x, arrowb.transform.localPosition.y, arrowb.transform.localPosition.z - 0.02f);
			gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, lz));
			arrowb.transform.eulerAngles = transform.eulerAngles + new Vector3(180f, 0f, 0f);
		}
		while(lz < -0.05f){
			yield return new WaitForSeconds(0.005f);
			lz += 0.05f;
			arrowb.transform.localPosition = new Vector3(arrowb.transform.localPosition.x, arrowb.transform.localPosition.y, arrowb.transform.localPosition.z + 0.05f);
			gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, lz));
			arrowb.transform.eulerAngles = transform.eulerAngles + new Vector3(180f, 0f, 0f);
		}
		do{
			Destroy(arrowb);
			GameObject arrow = Instantiate(usedArrow, transform.position, transform.rotation) as GameObject;
			arrow.transform.Rotate(new Vector3(180,0,0));
			arrow.GetComponent<Rigidbody>().velocity = transform.forward * power;
			arrow.SendMessage("setDamage", damage);
			bowAS.PlayOneShot(bowRelease);
		}while(false);
	}

}
