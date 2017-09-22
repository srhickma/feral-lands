using UnityEngine;
using System.Collections;

public class Furnace : MonoBehaviour {

	public Vector2[] inv = new Vector2[3];
	private Vector3[] recipies = {new Vector3(1,1,16), new Vector3(5,16,18)};
	private Vector3 recipe = new Vector3(0, 0, 0), lastRecipe = new Vector3(0, 0, 0);
	private int smeltTime = 0;
	private float elapsedTime = 0;
	private Player player;
    public GameObject fire;

	void Start () {
		
	}

	void Awake(){
		player = (GameObject.Find("Player") as GameObject).GetComponent("Player") as Player;
		if(gameObject.name == "Stone Furnace(Clone)")smeltTime = 20;
		else smeltTime = 10;
	}

	void Update () {
		if(recipe != new Vector3(0, 0, 0)){
            fire.SetActive(true);
			elapsedTime += Time.deltaTime;
			if(elapsedTime > smeltTime){
				inv[0] = new Vector2(inv[0].x, inv[0].y - 1);
				inv[1] = new Vector2(inv[1].x, inv[1].y - 1);
				if(recipe.z == 16)inv[2] = new Vector2(recipe.z, inv[2].y + 3);
				else inv[2] = new Vector2(recipe.z, inv[2].y + 1);
				elapsedTime = 0;
				checkRecipies();
			}
		}
        else fire.SetActive(false);
	}

	public void setInv(Vector2[] i){
		inv[0] = i[0];
		inv[1] = i[1];
		checkRecipies();
	}

	void checkRecipies(){
		bool found = false;
		lastRecipe = recipe;
		foreach(Vector3 v in recipies){
			if(inv[0].x == v.x && inv[1].x == v.y && inv[0].y > 0 && inv[1].y > 0){
				found = true;
				recipe = v;
			}
			if(inv[0].y < 1)inv[0] = new Vector2(0, 0);
			if(inv[1].y < 1)inv[1] = new Vector2(0, 0);
		}
		if(!found)recipe = new Vector3(0, 0, 0);
		if(recipe != lastRecipe)resetTimer();
		if(player.currentMachineGO == gameObject){
			player.crafting[12] = inv[0];
			player.crafting[17] = inv[1];
			player.recipeProduct = inv[2];
		}
	}

	public void clearProduct(){inv[2] = new Vector2(0, 0);}

	void resetTimer(){elapsedTime = 0;}

	public void loadInv(Vector2[] inv){
		this.inv = inv;
	}

}
