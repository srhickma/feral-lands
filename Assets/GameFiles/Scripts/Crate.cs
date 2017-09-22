using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	public GameObject playerGO, text, label;
	public Vector2 item;
	Player player;
	float curTime = 0;

	void Start () {

	}

	public void spawn(Vector2 i){
		item = i;
		playerGO = GameObject.Find("Player");
		player = playerGO.GetComponent("Player") as Player;
		text.GetComponent<TextMesh>().text = (item.y.ToString() + "x " + player.itemNames[(int) item.x]);
	}

	void Update () {
		curTime += Time.deltaTime;
		if(curTime > 300)Destroy(gameObject);
		if(Vector3.Distance(transform.position, player.transform.position) < 3 && player.lookingAt == gameObject){
			label.active = true;
			label.transform.LookAt(player.transform);
		}
		else label.active = false;
	}

}
