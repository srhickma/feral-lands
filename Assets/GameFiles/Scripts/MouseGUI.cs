using UnityEngine;
using System.Collections;

public class MouseGUI : MonoBehaviour {

	private Player player;

	void Start () {
		player = gameObject.GetComponent("Player") as Player;
	}

	void Update () {
	
	}

	void OnGUI(){
		GUI.skin = player.IGUI;
		GUI.depth = -1;
		if(player.itemOnMouse.x != 0){
			GUI.Label(new Rect(Input.mousePosition.x - 25, Screen.height - Input.mousePosition.y  - 25, 50, 50), player.itemTextures[(int) player.itemOnMouse.x]);
			if(player.itemOnMouse.y != 1)GUI.Label(new Rect(Input.mousePosition.x + 7, Screen.height - Input.mousePosition.y + 10, 100, 100), player.itemOnMouse.y.ToString());
		}
	}

}
