using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour {

	public GameObject cameraGO, headGO, machine, currentMachineGO, lookingAt, crateGO, bloodSplatter, inGameC, controlsC, menu, preStruct, rockHit, woodHit, swing, hurt;
	List<GameObject> crates = new List<GameObject>();
	private float mouseWheel = 0, nuT = 0, hyT = 0, fallT = 0f, fallStart = 0f, huT = 0f, footstepT = 0f;
	private int interactRange = 4, placeRange = 8, buildRange = 16, sWidth = Screen.width, sHeight = Screen.height, recipeIndex = 0, toolbarSlot = 0, currentMachineInt = 54, toolIndex = 0, arrow = 0, armorRating = 0, recipieViewed = 0, damageAlpha = 0, FPS = 0, FPSTick = 0, prevS = 0;
	public int health = 100, nutrition = 100;
	public GameObject[] machines = new GameObject[12], tools = new GameObject[17], structures = new GameObject[16], preStructures = new GameObject[16], preStructuresGood = new GameObject[16];
	private Quaternion machineRotation = new Quaternion();
	public string[] itemNames = {"", "Wood Log", "Stone", "Stick", "Leaf", "Iron Chunk", "Wood Plank", "Feather", "Animal Hide", "Leather", "Raw Meat", "Cooked Meat", "Leather Strips", "Tool Rod", "Stone Arrowhead", "Iron Arrowhead", "Charcoal", "Iron Plate", "Iron Ingot", "Bucket", "Bucket Of Saltwater", "Bucket Of Water", "Knife Blade", "Axe Head", "Pickaxe Head", "Hammer Head", "Cross-Guard", "Sword Blade", "Longsword Blade", "Stone Knife", "Stone Axe", "Stone Pickaxe", "Stone Hammer", "Bow", "Longbow", "Makeshift Arrow", "Stone Arrow", "Iron Arrow", "Iron Knife", "Iron Axe", "Iron Pickaxe", "Iron Hammer", "Iron Sword", "Iron Longsword", "Leather Gloves", "Leather Tunic", "Leather Pants", "Leather Boots", "Leather Hat", "Iron Gloves", "Iron Chestplate", "Iron Leggings", "Iron Boots", "Iron Helmet", "Backpack", "Workbench", "Tanning Rack", "Rain Barrel", "Water Purifier", "Campfire", "Chest", "Stone Furnace", "Bed", "Stone Anvil", "Iron Anvil", "Oven", "Iron Furnace", "Sharp Stone", "Wood Block", "Stone Block", "Wood Platform", "Wood Pillar", "Wood Wall", "Wood Doorway", "Wood Window", "Wood Stairs", "Wood Foundation", "Wood Door", "Stone Platform", "Stone Pillar", "Stone Wall", "Stone Doorway", "Stone Window", "Stone Stairs", "Stone Foundation", "Metal Door"};
	private string currentMachine = "", chat = "", chatText = "";
	public string username = "player";
	public Texture[] itemTextures = new Texture[86];
	public Texture damageIndicator;
	RecipeList recipeList = new RecipeList();
	private Recipe[] recipies;
	private int[] itemStackSize = {0, 15, 30, 30, 45, 30, 15, 45, 15, 15, 15, 15, 30, 5, 15, 15, 30, 15, 15, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 15, 15, 15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, armorRatings = {2,10,7,4,5,4,20,14,8,10};
	public Vector2[] inv = new Vector2[36], crafting = new Vector2[25], toolbar = new Vector2[6], armor = new Vector2[5];
	public Vector2 itemOnMouse = new Vector2(0, 0), recipeProduct = new Vector2(0, 0), chatScroll, crateScroll, recipeScroll;
	private Vector3[] toolDamage = {new Vector3(10,15,25), new Vector3(20,20,20), new Vector3(10,30,20), new Vector3(5,20,20), new Vector3(0,0,1), new Vector3(0,0,2), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(15,20,35), new Vector3(35,30,30), new Vector3(15,50,30), new Vector3(5,25,30), new Vector3(20,25,45), new Vector3(20,25,65), new Vector3(5,10,10), new Vector3(10,15,15)}, toolPos = new Vector3[17], toolRot = new Vector3[17];
	public Vector3 spawnPoint = new Vector3(0,0,0);
	private Vector3 testSPos = new Vector3(0,0,0), testSRot = new Vector3(0,0,0);
	private List<Vector4> getItemLabels = new List<Vector4>();
	private Vector4 gILTR =  new Vector4(0, 0, 0, 0);
	private Vector4[] craftingSpace = {new Vector4(1, 1, 1, 1), new Vector4(2, 2, 2, 2), new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(2, 2, 2, 2), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 0), new Vector4(2, 2, 2, 2), new Vector4(2, 2, 2, 2), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 1, 0)};
	private bool[] invSlots = new bool[36];
	public bool inInv = false, holdingTool = false, hitting = false, checking = false, hasArrow = false, alive = true, inRecipies = false, inMenu = false;
	private bool falling = false, tRemNeeded = false, hitMarker = false, goodPlace = false, stairShift = false, inCrates = false;
	private Rect invRect, toolbarRect, craftingRect, armorRect, healthRect, chatRect, recipiesRect, cratesRect;
	public Texture healthBar, nutritionBar;
	public GUISkin MGUI, IGUI, TGUI, HMGUI;
	public AudioClip[] concreteFStepsA = new AudioClip[4], woodFStepsA = new AudioClip[4];
	public AudioClip swooshA;
	private AudioSource footstepsAS, swingAS;
	private DepthOfField DOF;

	void Start () {
		menu = GameObject.Find("Menu");
		footstepsAS = GetComponent<AudioSource>();
		swingAS = swing.GetComponent<AudioSource>();
		DOF = cameraGO.GetComponent<DepthOfField>();
		addChat("Press F1 to chat");
		addChat("Press R to open your recipie book");
		addChat("Press E to open your inventory");
		recipies = recipeList.recipies;
		chatRect = new Rect(0, -20, 350, 171);
		for(int i = 0; i < 12; i ++)invSlots[i] = true;
		for(int i = 0; i < 17; i ++){
			if(tools[i] != null){
				toolPos[i] = tools[i].transform.localPosition;
				toolRot[i] = tools[i].transform.localEulerAngles;
			}
		}
		menu.GetComponent<AudioListener>().enabled = false;
		menu.GetComponent<Camera>().enabled = false;
		if(!FileLink.loadData(WorldData.world)){
			menu.SendMessage("randomSpawn");
			GameObject.Find("RockSpawner").SendMessage("spawn");
			GameObject.Find("AnimalSpawner").SendMessage("spawn");
			getItem(2, 1);
		}
		checkTools();
	}

	void Update () {
		sWidth = Screen.width;
		sHeight = Screen.height;
		if(alive){
			getInput();
			Ray rayb = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
			RaycastHit hitb;
			if(Physics.Raycast(rayb, out hitb)){
				lookingAt = hitb.collider.gameObject;
				if(hitb.distance > 1000f)DOF.focalLength = 1000f;
				else DOF.focalLength = hitb.distance;
			}
			else{
				lookingAt = null;
				DOF.focalLength = 1000f;
			}
			int s = (int) toolbar[toolbarSlot].x;
			if(s > 54 && s < 67 && prevS == s){
				Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
				RaycastHit hit;
				if((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject != null && hit.distance < placeRange) && (hit.collider.gameObject.tag == "placeAble" || hit.collider.gameObject.tag == "platform")){
					if(machine == null){
						machine = Instantiate(machines[s - 55], hit.point, machineRotation) as GameObject;
						machine.GetComponent<Collider>().enabled = false;
					}
					else machine.transform.position = hit.point;
					machine.transform.Rotate(Vector3.up * mouseWheel);
					machineRotation = machine.transform.rotation;
				}
				else Destroy(machine);
			}
			else Destroy(machine);
			prevS = s;
			if(s > 69){
				Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit) && hit.collider.gameObject != null && hit.distance < buildRange){
					if(preStruct == null){
						if(!goodPlace)preStruct = Instantiate(preStructures[s - 70], hit.point, new Quaternion(0,0,0,0)) as GameObject;
						else preStruct = Instantiate(preStructuresGood[s - 70], hit.point, new Quaternion(0,0,0,0)) as GameObject;
					}
					else{
						if(testStructure(hit, s, ray)){
							if(!goodPlace)Destroy(preStruct);
							else{
								if(stairShift){
									foreach(Transform t in preStruct.GetComponentsInChildren<Transform>())t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0f);
									stairShift = false;
								}
								preStruct.transform.position = testSPos;
								preStruct.transform.eulerAngles = testSRot;
							}
							goodPlace = true;
						}
						else{
							if(goodPlace)Destroy(preStruct);
							else{
								preStruct.transform.position = hit.point;
								preStruct.transform.eulerAngles = new Vector3(0,0,0);
							}
							goodPlace = false;
						}
					}
				}
				else Destroy(preStruct);
			}
			else Destroy(preStruct);
			if(falling){
				float distance = fallStart - transform.position.y;
				if(gameObject.GetComponent<CharacterController>().isGrounded && distance > 10f)lowerHealth((int)((distance - 9f) * (distance - 9f)));
			}
			else{
				fallT = 0f;
				fallStart = transform.position.y;
			}
			falling = !gameObject.GetComponent<CharacterController>().isGrounded;
			handleVitals();
		}
		else{
			respawn();
		}
		FPSTick ++;
		if(FPSTick > 100){
			FPSTick = 0;
			FPS = (int)Mathf.Round(1f / Time.deltaTime);
		}
	}

	void OnGUI(){
		invRect = new Rect(sWidth / 2 - 280, sHeight / 2 - 168, 340, 350);
		craftingRect = new Rect(sWidth / 2 + 68, sHeight / 2 - 168, 286, 350);
		armorRect = new Rect(sWidth / 2 - 350, sHeight / 2 - 168, 66, 276);
		toolbarRect = new Rect(sWidth / 2 - 158, sHeight - 76, 340, 76);
		healthRect = new Rect(sWidth - 150, sHeight - 65, 150, 65);
		cratesRect = new Rect(sWidth / 2 - 100, sHeight / 2 - 200, 200, 400);
		recipiesRect = new Rect(sWidth / 2 - 243, sHeight / 2 - 200, 486, 400);
		GUI.depth = 0;
		GUI.skin = MGUI;
		GUI.Label(new Rect(Screen.width - 100, 0, 100, 100), FPS.ToString() + " FPS");
        if(!inMenu){
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 100), "+");
		    if(hitMarker){
			    GUI.skin = HMGUI;
			    GUI.Label(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 100, 100), "X");
		    }
        }
		GUI.skin = MGUI;
		if(!inMenu)healthWindow();
		GUI.skin = IGUI;
		if(inInv){
			invRect = GUILayout.Window(0, invRect, invWindow, "Inventory");
			if(currentMachine != "")craftingRect = GUILayout.Window(1, craftingRect, craftingWindow, currentMachine);
			else craftingRect = GUILayout.Window(1, craftingRect, craftingWindow, "Crafting");
			armorRect = GUILayout.Window(2, armorRect, armorWindow, "Armor");
		}
		else if(inRecipies)recipiesRect = GUILayout.Window(5, recipiesRect, recipiesWindow, "   Recipies:");
		if(inInv || inRecipies || !alive || inMenu || inCrates){
			gameObject.GetComponent<CharacterMotor>().canControl = false;
			gameObject.GetComponent<MouseLook>().enabled = false;
			headGO.GetComponent<MouseLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
		}
		else{
			gameObject.GetComponent<CharacterMotor>().canControl = true;
			gameObject.GetComponent<MouseLook>().enabled = true;
			headGO.GetComponent<MouseLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
		}
		if(!inMenu)toolbarRect = GUILayout.Window(3, toolbarRect, toolbarWindow, "Toolbar");
		GUI.skin = MGUI;
		if(!inInv && !inRecipies && !inMenu)chatRect = GUILayout.Window(4, chatRect, chatWindow, "");
		getItemLabel();
		if(inCrates)cratesRect = GUILayout.Window(6, cratesRect, cratesWindow, "Crates");
		if(damageAlpha > 0){
			GUI.color = new Color32(255, 255, 255, (byte)damageAlpha);
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), damageIndicator);
			GUI.color = new Color32(255, 255, 255, 100);
			damageAlpha --;
		}
	}

	void respawn(){
		foreach(Vector2 v in inv)
			dropItem(v);
//		foreach(Vector2 v in crafting)
//			dropItem(v);
//		foreach(Vector2 v in toolbar)
//			dropItem(v);
//		foreach(Vector2 v in armor)
//			dropItem(v);
		inv = new Vector2[36];
		crafting = new Vector2[25];
		toolbar = new Vector2[6];
		armor = new Vector2[5];
		getItem(2, 1);
		health = 100;
		nutrition = 100;
		toolIndex = 0;
		checkTools();
		if(spawnPoint == new Vector3(0,0,0))menu.SendMessage("randomSpawn");
		else transform.position = spawnPoint;
		alive = true;
	}
	
	void handleVitals(){
		nuT += Time.deltaTime * 2;
		huT += Time.deltaTime;
		if(nuT > 43.2f && nutrition > 0){
			nutrition --;
			nuT = 0;
		}
		else if(nutrition == 0 && nuT > 12.96f){
			lowerHealth(1);
			nuT = 0;
		}
		if(huT > 5f && health < 100 && nutrition > 75){
			health ++;
			huT = 0f;
		}
	}

	void cratesWindow(int WindowID){
		crateScroll = GUILayout.BeginScrollView(crateScroll, GUILayout.Width(350));
		foreach(GameObject c in crates){
			GUILayout.BeginHorizontal();
			Crate crate = c.GetComponent("Crate") as Crate;
			if(GUILayout.Button(crate.item.y.ToString() + "x " + itemNames[(int)crate.item.x])){
				getItem((int) crate.item.x, (int) crate.item.y);
				Destroy(c);
				inCrates = false;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		if(GUILayout.Button("Take All")){
			foreach(GameObject c in crates){
				Crate crate = c.GetComponent("Crate") as Crate;
				getItem((int) crate.item.x, (int) crate.item.y);
				Destroy(c);
			}
			inCrates = false;
		}
	}

	void recipiesWindow(int windowID){
		if(recipies[recipieViewed].g[27] == 99){
			if(recipies[recipieViewed].g[25] == 16)GUILayout.Label("Requires: Campfire/Oven/Stone Furnace/Iron Furnace");
			else if(recipies[recipieViewed].g[25] == 11)GUILayout.Label("Requires: Campfire/Oven");
			else GUILayout.Label("Requires: Stone Furnace/Iron Furnace");
		}
		else if(recipies[recipieViewed].g[27] != 0)GUILayout.Label("Requires: " + itemNames[recipies[recipieViewed].g[27]]);
		else GUILayout.Label(" ");
		for(int i = 0; i < 25; i ++){
			if(i % 5 == 0)GUILayout.BeginHorizontal();
			if(GUILayout.Button(new GUIContent(itemTextures[(int) (recipies[recipieViewed].g[i])], itemNames[(int) (recipies[recipieViewed].g[i])]))){
				int indexa = 0;
				foreach(Recipe r in recipies){
					if(recipies[recipieViewed].g[i] == r.g[25])recipieViewed = indexa;
					indexa ++;
				}
			}
			if(i % 5 == 4)GUILayout.EndHorizontal();
		}
		GUI.Box(new Rect(118, 338, 100, 100), itemTextures[(int) recipies[recipieViewed].g[25]]);
		GUI.Label(new Rect(10, 0, 100, 100), itemNames[recipies[recipieViewed].g[25]]);
		if(recipies[recipieViewed].g[26] != 1)GUI.Label(new Rect(148, 370, 100, 100), recipies[recipieViewed].g[26].ToString());
        GUI.Label(new Rect(((int)Event.current.mousePosition.x - 5) / 54 * 54 + 5, ((int)Event.current.mousePosition.y + 10) / 54 * 54 - 10, 100, 100), GUI.tooltip);
		GUILayout.BeginArea(new Rect(286, 10, 200, 380));
		GUI.skin = MGUI;
		recipeScroll = GUILayout.BeginScrollView(recipeScroll, GUILayout.Width(200));
		int indexb = 0;
		foreach(Recipe r in recipies){
			if(GUILayout.Button(itemNames[r.g[25]]))recipieViewed = indexb;
			indexb ++;
		}
		GUILayout.EndScrollView();
		GUI.skin = IGUI;
		GUILayout.EndArea();
	}

	void chatWindow(int windowID){
		GUILayout.BeginHorizontal();
		GUILayout.Label(chat);
		GUILayout.EndHorizontal();
		GUI.SetNextControlName("chatText");
		chatText = GUI.TextField(new Rect(10, 145, 330, 20), chatText);
		if(Event.current.keyCode == KeyCode.Return){
			if(chatText != "")addChat("[You]" + chatText);
			chatText = "";
			GUI.FocusControl("");
		}
		else if(Event.current.keyCode == KeyCode.F1 && GUI.GetNameOfFocusedControl() != "chatText")GUI.FocusControl("chatText");
	}

	public void addChat(string text){
		string oldChat = text + "\n" + chat;
		int i = 0;
		chat = "";
		foreach(string s in oldChat.Split(("\n").ToCharArray())){
			i ++;
			if(i < 9)chat += s + "\n";
		}
	}

	void invWindow(int windowID){
		for(int i = 0; i < 36; i ++){
			if(i % 6 == 0)GUILayout.BeginHorizontal();
			if(invSlots[i]){
				if(GUILayout.Button(new GUIContent(itemTextures[(int)inv[i].x], itemNames[(int)inv[i].x]))){
					if(Event.current.button == 0)tradeSlots(itemOnMouse, inv[i], out itemOnMouse, out inv[i], false, false);
					else if(Event.current.button == 1)tradeSlots(itemOnMouse, inv[i], out itemOnMouse, out inv[i], false, true);
				}
				if(inv[i].x != 0 && inv[i].y != 1)GUI.Label(new Rect(40 + 54 * (i % 6), 50 + 54 * (i / 6), 100, 100), inv[i].y.ToString());
			}
			if(i % 6 == 5)GUILayout.EndHorizontal();
		}
        if(itemOnMouse.x == 0)GUI.Label(new Rect(((int)Event.current.mousePosition.x - 5) / 54 * 54 + 5, ((int)Event.current.mousePosition.y + 30) / 54 * 54 - 35, 100, 100), GUI.tooltip);
	}

	void craftingWindow(int windowID){
		int y = 0;
		if(currentMachine == "Chest")crafting = (currentMachineGO.GetComponent("Chest") as Chest).inv;
		if(currentMachine == "Iron Furnace" || currentMachine == "Stone Furnace"){
			crafting[12] = (currentMachineGO.GetComponent("Furnace") as Furnace).inv[0];
			crafting[17] = (currentMachineGO.GetComponent("Furnace") as Furnace).inv[1];
			recipeProduct = (currentMachineGO.GetComponent("Furnace") as Furnace).inv[2];
		}
		if(currentMachine == "Campfire" || currentMachine == "Oven"){
			crafting[12] = (currentMachineGO.GetComponent("Oven") as Oven).inv[0];
			crafting[17] = (currentMachineGO.GetComponent("Oven") as Oven).inv[1];
			recipeProduct = (currentMachineGO.GetComponent("Oven") as Oven).inv[2];
		}
		for(int i = 0; i < 25; i ++){
			if(i % 5 == 0)GUILayout.BeginHorizontal();
			Vector4 size = craftingSpace[currentMachineInt - 54];
			if(!(i % 5 <= 2 + size.x && i % 5 >= 2 - size.y && y <= 2 + size.z && y >= 2 - size.w)){
				GUILayout.Box("");
			}
			else if(GUILayout.Button(new GUIContent(itemTextures[(int) crafting[i].x], itemNames[(int) crafting[i].x]))){
				if(Event.current.button == 0)tradeSlots(itemOnMouse, crafting[i], out itemOnMouse, out crafting[i], false, false);
				else if(Event.current.button == 1)tradeSlots(itemOnMouse, crafting[i], out itemOnMouse, out crafting[i], false, true);
				Vector2[] fInv = {crafting[12], crafting[17], new Vector2(0, 0)};
				if(currentMachine == "Stone Furnace" || currentMachine == "Iron Furnace")(currentMachineGO.GetComponent("Furnace") as Furnace).setInv(fInv);
				if(currentMachine == "Campfire" || currentMachine == "Oven")(currentMachineGO.GetComponent("Oven") as Oven).setInv(fInv);
				if(currentMachine == "Chest")(currentMachineGO.GetComponent("Chest") as Chest).inv = crafting;
				else checkRecipies();
			}
			if(crafting[i].x != 0 && crafting[i].y != 1)GUI.Label(new Rect(40 + 54 * (i % 5), 50 + 54 * (i / 5), 100, 100), crafting[i].y.ToString());
			if(i % 5 == 4){
				y ++;
				GUILayout.EndHorizontal();
			}
		}
		if(currentMachine != "Chest"){
			if(GUI.Button(new Rect(118, 288, 100, 100), new GUIContent(itemTextures[(int) recipeProduct.x], itemNames[(int) recipeProduct.x])) && (itemOnMouse.x == 0 || itemOnMouse.x == recipeProduct.x)){
				if(recipeProduct.y == recipies[recipeIndex].g[26] && (itemOnMouse.y < itemStackSize[(int) itemOnMouse.x] || itemOnMouse.x == 0) && !(currentMachine == "Stone Furnace" || currentMachine == "Iron Furnace" || currentMachine == "Campfire" || currentMachine == "Oven")){
					for(int i = 0; i < 25; i ++){
						if(crafting[i].x != 0){
							if(crafting[i].y > 1)crafting[i] = new Vector2(crafting[i].x, crafting[i].y - 1);
							else crafting[i] = new Vector2(0, 0);
						}
					}
				}
				tradeSlots(itemOnMouse, recipeProduct, out itemOnMouse, out recipeProduct, true, false);
				if(currentMachine == "Stone Furnace" || currentMachine == "Iron Furnace")(currentMachineGO.GetComponent("Furnace") as Furnace).clearProduct();
				if(currentMachine == "Campfire" || currentMachine == "Oven")(currentMachineGO.GetComponent("Oven") as Oven).clearProduct();
				if(recipeProduct.y == 0){
					checkRecipies();
				}
			}
		}
		if(recipeProduct.x != 0 && recipeProduct.y != 1)GUI.Label(new Rect(148, 320, 100, 100), recipeProduct.y.ToString());
		if(itemOnMouse.x == 0)GUI.Label(new Rect(((int)Event.current.mousePosition.x - 5) / 54 * 54 + 5, ((int)Event.current.mousePosition.y + 30) / 54 * 54 - 35, 100, 100), GUI.tooltip);
	}

	void armorWindow(int windowID){
		int id = (int) itemOnMouse.x;
		if(GUILayout.Button(new GUIContent(itemTextures[(int) armor[0].x], itemNames[(int) armor[0].x])) && (id == 48 || id == 53 || id == 0)){
			tradeSlots(itemOnMouse, armor[0], out itemOnMouse, out armor[0], false, false);
			checkArmor();
		}
		if(GUILayout.Button(new GUIContent(itemTextures[(int) armor[1].x], itemNames[(int) armor[1].x])) && (id == 45 || id == 50 || id == 0 || id == 54)){
			tradeSlots(itemOnMouse, armor[1], out itemOnMouse, out armor[1], false, false);
			checkArmor();
		}
		if(GUILayout.Button(new GUIContent(itemTextures[(int) armor[2].x], itemNames[(int) armor[2].x])) && (id == 44 || id == 49 || id == 0)){
			tradeSlots(itemOnMouse, armor[2], out itemOnMouse, out armor[2], false, false);
			checkArmor();
		}
		if(GUILayout.Button(new GUIContent(itemTextures[(int) armor[3].x], itemNames[(int) armor[3].x])) && (id == 46 || id == 51 || id == 0)){
			tradeSlots(itemOnMouse, armor[3], out itemOnMouse, out armor[3], false, false);
			checkArmor();
		}
		if(GUILayout.Button(new GUIContent(itemTextures[(int) armor[4].x], itemNames[(int) armor[4].x])) && (id == 47 || id == 52 || id == 0)){
			tradeSlots(itemOnMouse, armor[4], out itemOnMouse, out armor[4], false, false);
			checkArmor();
		}
		if(itemOnMouse.x == 0)GUI.Label(new Rect(((int)Event.current.mousePosition.x - 5) / 54 * 54 + 5, ((int)Event.current.mousePosition.y + 30) / 54 * 54 - 35, 100, 100), GUI.tooltip);
	}

	void toolbarWindow(int windowID){
		GUILayout.BeginHorizontal();
		for(int i = 0; i < 6; i ++){
			if(i == toolbarSlot)GUI.skin = TGUI;
			if(GUILayout.Button(new GUIContent(itemTextures[(int) toolbar[i].x], itemNames[(int) toolbar[i].x]))){
				if(Event.current.button == 0)tradeSlots(itemOnMouse, toolbar[i], out itemOnMouse, out toolbar[i], false, false);
				else if(Event.current.button == 1)tradeSlots(itemOnMouse, toolbar[i], out itemOnMouse, out toolbar[i], false, true);
				checkTools();
			}
			if(toolbar[i].x != 0 && toolbar[i].y != 1)GUI.Label(new Rect(40 + 54 * i, 50, 100, 100), toolbar[i].y.ToString());
			GUI.skin = IGUI;
		}
		GUILayout.EndHorizontal();
		if(itemOnMouse.x == 0)GUI.Label(new Rect(((int)Event.current.mousePosition.x - 5) / 54 * 54 + 5, ((int)Event.current.mousePosition.y + 30) / 54 * 54 - 35, 100, 100), GUI.tooltip);
	}

	void healthWindow(){
		GUI.Box(healthRect, "");
		GUI.Label(new Rect(sWidth - 145, sHeight - 65, 100, 25), "Health");
		GUI.Label(new Rect(sWidth - 145, sHeight - 35, 100, 25), "Nutrition");
		GUI.Label(new Rect(sWidth - 75, sHeight - 65, 60, 25), health.ToString() + "%");
		GUI.Label(new Rect(sWidth - 75, sHeight - 35, 60, 25), nutrition.ToString() + "%");
		GUI.DrawTexture(new Rect(sWidth - 120, sHeight - 45, 100 * health / 100, 10), healthBar, ScaleMode.StretchToFill);
		GUI.DrawTexture(new Rect(sWidth - 120, sHeight - 15, 100 * nutrition / 100, 10), nutritionBar, ScaleMode.StretchToFill);
	}

	public void lowerHealth(int ammount){
		damageAlpha = 100;
		hurt.GetComponent<AudioSource>().Play();
		health -= ammount;
		if(health < 1){
			health = 0;
			alive = false;
		}
	}

	void checkRecipies(){
		int machineInt = 0;
		for(int i = 0; i < itemNames.Length; i ++)
			if(currentMachine == itemNames[i])machineInt = i;
		recipeProduct = new Vector2(0, 0);
		recipeIndex = 0;
		int[] craftingSlots = new int[28];
		for(int i = 0; i < 25; i ++)
			craftingSlots[i] = (int) crafting[i].x;
		Recipe entered = new Recipe(craftingSlots);
		for(int i = 0; i < recipies.Length; i ++){
			if(entered.equals(recipies[i]) && (machineInt == recipies[i].g[27] || (machineInt == 55 && recipies[i].g[27] == 0) || (machineInt == 64 && recipies[i].g[27] == 63))){
				recipeIndex = i;
				recipeProduct = (new Vector2(recipies[i].g[25], recipies[i].g[26]));
			}
		}
	}

	void checkArmor(){
		armorRating = 0;
		int j = 0;
		foreach(Vector2 v in armor){
			if(v.x == 54)for(int i = 0; i < 36; i ++)invSlots[i] = true;
			else{
				if(v.x != 0)armorRating += armorRatings[(int) v.x - 44];
				for(int i = 0; i < 36; i ++){
					if(i > 11 && j == 1){
						invSlots[i] = false;
						if(inv[i].x != 0)dropItem(inv[i]);
						inv[i] = new Vector3(0,0,0);
					}
				}
			}
			j ++;
		}
	}

	void tradeSlots(Vector2 M, Vector2 S, out Vector2 Mo, out Vector2 So, bool recOnly, bool single){
		if(M.x != S.x && !single){
			So = M;
			Mo = S;
		}
		else{
			if(recOnly){
				if(itemStackSize[(int) M.x] >= M.y + S.y){
					Mo = new Vector2(S.x, S.y + M.y);
					So = new Vector2(0, 0);
				}
				else{
					Mo = new Vector2(M.x, itemStackSize[(int) M.x]);
					So = new Vector2(M.x, M.y + S.y - itemStackSize[(int) S.x]);
				}
			}
			else if(single){
				if(itemStackSize[(int) M.x] >= 1 + S.y && (S.x == M.x || S.x == 0) && M.y > 0){
					So = new Vector2(M.x, S.y + 1);
					Mo = new Vector2(M.x, M.y - 1);
					if(M.y - 1 == 0)Mo = new Vector2(0, 0);
				}
				else{
					Mo = M;
					So = S;
				}
			}
			else{
				if(itemStackSize[(int) M.x] >= M.y + S.y){
					So = new Vector2(S.x, S.y + M.y);
					Mo = new Vector2(0, 0);
				}
				else{
					So = new Vector2(S.x, itemStackSize[(int) M.x]);
					Mo = new Vector2(M.x, M.y + S.y - itemStackSize[(int) M.x]);
				}
			}
		}
	}

	public void getItem(int ID, int ammount){
		int origAmmount = ammount;
		bool thruToolbar = false;
		for(int i = 0; i < 84; i ++){
			if(i > 41)thruToolbar = true;
			int i2 = i % 42;
			Vector2 curSlot;
			bool slotAvailable;
			if(i2 > 35){
				slotAvailable = true;
				curSlot = toolbar[i2 - 36];
			}
			else{
				slotAvailable = invSlots[i2];
				curSlot = inv[i2];
			}
			if(((curSlot.x == 0 && thruToolbar) || curSlot.x == ID) && slotAvailable && curSlot.y < itemStackSize[ID]){
				if(ammount + curSlot.y > itemStackSize[ID]){
					float curSlotAmmount = curSlot.y;
					curSlot = new Vector2(ID, itemStackSize[ID]);
					ammount = ammount - itemStackSize[ID] + (int) curSlotAmmount;
				}
				else{
					curSlot = new Vector2(ID, ammount + curSlot.y);
					ammount = 0;
				}
				if(i2 > 35)toolbar[i2 - 36] = curSlot;
				else inv[i2] = curSlot;
				if(ammount == 0)break;
			}
		}
		if(ammount < origAmmount){
			if(getItemLabels.Count > 0)getItemLabels.Add(new Vector4(ID, origAmmount - ammount, getItemLabels[getItemLabels.Count - 1].z - 30, 4F));
			else getItemLabels.Add(new Vector4(ID, origAmmount - ammount, sHeight / 2, 2F));
		}
		else if(ammount != 0)dropItem(new Vector2(ID, ammount));
	}

	void getItemLabel(){
		for(int i = 0; i < getItemLabels.Count; i ++){
			getItemLabels[i] = new Vector4(getItemLabels[i].x, getItemLabels[i].y, getItemLabels[i].z, getItemLabels[i].w - 0.01F);
			if(getItemLabels[i].w < 0){
				getItemLabels.Remove(getItemLabels[i]);
				for(int j = 0; j < getItemLabels.Count; j ++)
					getItemLabels[j] = new Vector4(getItemLabels[j].x, getItemLabels[j].y, getItemLabels[j].z + 25, getItemLabels[j].w);
			}
		}
		foreach(Vector4 v in getItemLabels){
			GUI.color = new Color(){r = 1F, g = 1F, b = 1F, a = v.w};
			if(v.y > 0)GUI.Box(new Rect(0, v.z, 200, 25), ("+" + v.y.ToString() + "x " + itemNames[(int) v.x]));
			else GUI.Box(new Rect(0, v.z, 200, 25), (v.y.ToString() + "x " + itemNames[(int) v.x]));
		}
		GUI.color = new Color(){r = 1F, g = 1F, b = 1F, a = 1F};
	}

	public void recieveDamage(int damage){lowerHealth((int)(damage - damage * (armorRating / 100f)));}

	IEnumerator hit(){
		if(holdingTool && !hitting && (((toolIndex == 4 || toolIndex == 5) && hasArrow) || (toolIndex != 4 && toolIndex != 5))){
			hitting = true;
			tools[toolIndex].GetComponent<Animation>().Play();
			if(toolIndex == 4 || toolIndex == 5){
				toolbar[toolbarSlot + 1] = new Vector2(toolbar[toolbarSlot + 1].x, toolbar[toolbarSlot + 1].y - 1);
				if(toolbar[toolbarSlot + 1].y == 0){
					toolbar[toolbarSlot + 1] = new Vector2(0, 0);
					checkTools();
				}
				tools[toolIndex].BroadcastMessage("shoot", arrow);
			}
			yield return new WaitForSeconds(0.4F);
			if(toolIndex != 4 && toolIndex != 5)swingAS.PlayOneShot(swooshA);
			Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit) && toolIndex != 4 && toolIndex != 5){
				if(hit.collider.gameObject != null && hit.distance < interactRange){
					hitMarker = true;
					string tag = hit.collider.gameObject.tag, name = hit.collider.gameObject.name;
					if(tag == "tree"){
						hit.collider.gameObject.SendMessage("lowerHealth", (int) toolDamage[toolIndex].x);
						woodHit.GetComponent<AudioSource>().Play();
					}
					else if(tag == "rock")hit.collider.gameObject.SendMessage("lowerHealth", (int) toolDamage[toolIndex].y);
					else if(tag == "animal" && !(toolIndex == 4 || toolIndex == 5)){
						hit.collider.gameObject.SendMessage("lowerHealth", (int) toolDamage[toolIndex].z);
						Instantiate(bloodSplatter, hit.point, new Quaternion(0,0,0,0));
					}
					else if(tag == "otherPlayer"){
						Instantiate(bloodSplatter, hit.point, new Quaternion(0,0,0,0));
					}
					else if(hit.collider.gameObject.tag == "machine" && (toolIndex == 3 || toolIndex == 12)){
						if(hit.collider.gameObject.layer == 13)woodHit.GetComponent<AudioSource>().Play();
						else rockHit.GetComponent<AudioSource>().Play();
						int i = 0;
						foreach(string s in itemNames){
							if(s == hit.collider.gameObject.name){
								getItem(i, 1);
								break;
							}
							i ++;
						}
						Destroy(hit.collider.gameObject);
					}
					else if(hit.collider.gameObject.GetComponent<Structure>() != null || (hit.collider.transform.parent != null && hit.collider.transform.parent.gameObject.GetComponent<Structure>() != null)){
						Structure s;
						if(tag == "platform" || tag == "pillar" || tag == "door")s = hit.collider.gameObject.GetComponent<Structure>();
						else s = hit.collider.transform.parent.gameObject.GetComponent<Structure>();
						if(s.creator == username && (toolIndex == 3 || toolIndex == 12))s.lowerDurability(s.durability, true);
						else{
							float damage;
							if(s.isWood)damage = toolDamage[toolIndex].x;
							else damage = toolDamage[toolIndex].y;
							if(toolIndex == 3 || toolIndex == 12)damage *= 2;
							s.lowerDurability((int)damage, s.creator == username);
						}
						if(s.isWood)woodHit.GetComponent<AudioSource>().Play();
						else rockHit.GetComponent<AudioSource>().Play();
					}
					else hitMarker = false;
				}
			}
			yield return new WaitForSeconds(0.6F);
			hitting = false;
			hitMarker = false;
		}
	}

	void interact(){
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			if(lookingAt.name == "Crate(Clone)" && hit.distance< interactRange){
				crates = new List<GameObject>();
				foreach(GameObject c in GameObject.FindGameObjectsWithTag("crate")){
					if(Vector3.Distance(c.transform.position, lookingAt.transform.position) < 1f)crates.Add(c);
				}
				if(crates.Count > 1){
					inCrates = true;
					inInv = false;
					inRecipies = false;
				}
				else{
					Crate crate = lookingAt.GetComponent("Crate") as Crate;
					getItem((int) crate.item.x, (int) crate.item.y);
					Destroy(lookingAt);
				}
			}
			else if(lookingAt.tag == "door" && hit.distance < interactRange){
				if(!lookingAt.GetComponent<Door>().attemptOpen(username))addChat("This door is locked");
			}
			else if(hit.distance < buildRange && toolbar[toolbarSlot].x > 69)placeStructure(hit, (int)toolbar[toolbarSlot].x, ray);
			else if(hit.collider.gameObject != null && hit.distance < placeRange){
				int s = (int) toolbar[toolbarSlot].x;
				if(hit.collider.gameObject.tag == "machine" && hit.distance < interactRange){
					currentMachine = hit.collider.gameObject.name;
					if(hit.collider.gameObject.name != "Bed"){
						for(int i = 0; i < itemNames.Length; i ++)
							if(currentMachine == itemNames[i])currentMachineInt = i;
						currentMachineGO = hit.collider.gameObject;
						inInv = true;
					}
					else{
						spawnPoint = transform.position;
						addChat("Spawn point set");
					}
				}
				else if((s > 54 && s < 67) && (hit.collider.gameObject.tag == "placeAble" || hit.collider.gameObject.tag == "platform")){
					GameObject g = Instantiate(machines[s - 55], hit.point, machineRotation) as GameObject;
					g.name = machines[s - 55].name;
					toolbar[toolbarSlot] = new Vector2(0, 0);
				}
			}
		}
		if(toolbar[toolbarSlot].x == 11 || toolbar[toolbarSlot].x == 10 && nutrition < 100){
			if(toolbar[toolbarSlot].x == 11)nutrition += 30;
			else if(toolbar[toolbarSlot].x == 10){
				nutrition += 10;
				health -= 10;
			}
			if(nutrition > 100)nutrition = 100;
			if(toolbar[toolbarSlot].y == 1)toolbar[toolbarSlot] = new Vector2(0, 0);
			else toolbar[toolbarSlot] = new Vector2(toolbar[toolbarSlot].x, toolbar[toolbarSlot].y - 1f);
		}
	}

	void checkTools(){
		int s = (int) toolbar[toolbarSlot].x;
		if(s == 2 || (s < 35 && s > 28) || (s < 44 && s > 37) || s == 67){
			holdingTool = true;
			if(s == 2)toolIndex = 15;
			else if(s == 67)toolIndex = 16;
			else toolIndex = s - 29;
			for(int j = 0; j < 17; j ++)
				if(tools[j] != null && j != toolIndex)tools[j].SetActive(false);
			tools[toolIndex].SetActive(true);
			tools[toolIndex].transform.localPosition = toolPos[toolIndex];
			tools[toolIndex].transform.localEulerAngles = toolRot[toolIndex];
			if(toolbarSlot < 5){
				int sb = (int) toolbar[toolbarSlot + 1].x;
				if((s == 33 || s == 34) && sb > 34 && sb < 38){
					hasArrow = true;
					arrow = sb;
				}
				else hasArrow = false;
			}
			else hasArrow = false;
		}
		else {
			holdingTool = false;
			for(int j = 0; j < 17; j ++)
				if(tools[j] != null)tools[j].SetActive(false);
		}
	}

	void dropItem(Vector2 v){
        if(v.y != 0){
		    Ray r = new Ray(transform.position, -transform.up);
		    RaycastHit h;
		    if((Physics.Raycast(r, out h)) && (h.collider.gameObject.tag == "placeAble" || h.collider.gameObject.tag == "machine" || h.collider.gameObject.tag == "tree" || h.collider.gameObject.name == "Crate(Clone)" || h.collider.gameObject.tag == "platform")){
			    GameObject c = null;
			    if(h.collider.gameObject.name == "Crate(Clone)")c = Instantiate(crateGO, new Vector3(transform.position.x, h.collider.gameObject.transform.position.y, transform.position.z), new Quaternion(0,0,0,0)) as GameObject;
			    else c = Instantiate(crateGO, new Vector3(transform.position.x, h.point.y + 0.25f, transform.position.z), new Quaternion(0,0,0,0)) as GameObject;
			    (c.GetComponent("Crate") as Crate).spawn(v);
			    if(getItemLabels.Count > 0)getItemLabels.Add(new Vector4(v.x, -v.y, getItemLabels[getItemLabels.Count - 1].z - 30, 4F));
			    else getItemLabels.Add(new Vector4(v.x, -v.y, sHeight / 2, 2F));
		    }
        }
	}

	void getInput(){
		mouseWheel = Input.GetAxis ("Mouse ScrollWheel") * 10;
		if(Input.GetMouseButtonDown(0) && !inInv && !inRecipies && !inMenu)StartCoroutine(hit());
		if(Input.GetMouseButtonDown(1) && !inInv && !inRecipies && !inMenu)interact();
		if(Input.GetKeyDown("e") && !inMenu){
			inInv = !inInv;
			inRecipies = false;
			if(!inInv){
				if(currentMachine != "Chest" && currentMachine != "Iron Furnace" && currentMachine != "Stone Furnace" && currentMachine != "Campfire" && currentMachine != "Oven"){
					foreach(Vector2 v in crafting)
						getItem((int) v.x, (int) v.y);
				}
				currentMachineInt = 54;
				currentMachine = "";
				currentMachineGO = null;
				recipeProduct = new Vector2(0, 0);
				crafting = new Vector2[25];
				if(itemOnMouse.x != 0){
					dropItem(itemOnMouse);
					itemOnMouse = new Vector2(0,0);
				}
			}
		}
		if(Input.GetKeyDown("r") && !inMenu){
			inRecipies = !inRecipies;
			inInv = false;
		}
		if(Input.GetKey("d") && inInv && itemOnMouse.x != 0){
			dropItem(itemOnMouse);
			itemOnMouse = new Vector2(0, 0);
		}
		if((Input.GetKey ("w") || Input.GetKey ("a") || Input.GetKey ("s") || Input.GetKey ("d")) && !inInv && !inRecipies && !inMenu){
			cameraGO.GetComponent<Animation>().Play();
			if(!footstepsAS.isPlaying && !falling && footstepT < 0f){
				footstepsAS.clip = concreteFStepsA[UnityEngine.Random.Range(0, 4)];
				foreach(Collider c in Physics.OverlapSphere(transform.position - new Vector3(0, 1f, 0), 0.25f)){
					if(c.gameObject.layer == 10){
						footstepsAS.clip = woodFStepsA[UnityEngine.Random.Range(0, 4)];
						footstepT = 0.5f;
					}
				}
				footstepsAS.Play();
			}
			else if(footstepT >= 0f)footstepT -= Time.deltaTime;
		}
		for(int i = 0; i < 6; i ++){
			if(Input.GetKeyDown((i + 1).ToString())){
				toolbarSlot = i;
				checkTools();
				if(preStruct != null)Destroy(preStruct);
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			inRecipies = false;
			inInv = false;
			inCrates = false;
			inMenu = !inMenu;
			inGameC.SetActive(inMenu);
            controlsC.SetActive(false);
			recipeProduct = new Vector2(0, 0);
			crafting = new Vector2[25];
			GameObject.Find("Main Camera").GetComponent<BlurOptimized>().enabled = inMenu;
		}
	}

	void placeStructure(RaycastHit r, int sType, Ray ray){
		if(testStructure(r, sType, ray)){
			GameObject structure = Instantiate(structures[sType - 70], testSPos, new Quaternion(0,0,0,0)) as GameObject;
			structure.transform.eulerAngles = testSRot;
			if(stairShift){
				foreach(Transform t in structure.GetComponentsInChildren<Transform>())t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0f);
				structure.transform.position = testSPos;
				stairShift = false;
			}
			structure.GetComponent<Structure>().initialize(sType - 70, username);
			if(sType == 85){
				structure.GetComponent<Door>().creator = username;
				structure.GetComponent<Door>().origRot = Mathf.Round(testSRot.y);
			}
			toolbar[toolbarSlot] = new Vector2(0, 0);
		}
	}

	bool testStructure(RaycastHit r, int sType, Ray ray){
		int type = (sType - 70) % 8;
		testSPos = new Vector3(0,0,0);
		testSRot = new Vector3(0,0,0);
		string tag = r.collider.gameObject.tag, name = r.collider.gameObject.name;
		if((type == 6) && (tag == "placeAble" || name == "Foundation")){
			if(tag == "placeAble"){
				testSPos = new Vector3((int)r.point.x / 9 * 9 + 5, ((int)r.point.y + (int)(6 - 7 % (r.point.y % 6))) / 6 * 6, (int)r.point.z / 9 * 9 + 5);
				RaycastHit a, b, c, d;
				Ray ra = new Ray(new Vector3(testSPos.x + 4.5f, testSPos.y + 1.5f, testSPos.z + 4.5f), -transform.up), rb = new Ray(new Vector3(testSPos.x + 4.5f, testSPos.y + 1.5f, testSPos.z - 4.5f), -transform.up), rc = new Ray(new Vector3(testSPos.x - 4.5f, testSPos.y + 1.5f, testSPos.z + 4.5f), -transform.up), rd = new Ray(new Vector3(testSPos.x - 4.5f, testSPos.y + 1.5f, testSPos.z - 4.5f), -transform.up);
				int underCount = 0;
				if(!(Physics.Raycast(ra, out a) && a.collider.gameObject.tag == "placeAble"))underCount ++;
				if(!(Physics.Raycast(rb, out b) && b.collider.gameObject.tag == "placeAble"))underCount ++;
				if(!(Physics.Raycast(rc, out c) && c.collider.gameObject.tag == "placeAble"))underCount ++;
				if(!(Physics.Raycast(rd, out d) && d.collider.gameObject.tag == "placeAble"))underCount ++;
				if(underCount > 2)testSPos.y += 6f;
			}
			else if(name == "Foundation"){
				Vector3 point = r.point + r.normal;
				testSPos = new Vector3((int)point.x / 9 * 9 + 5, r.collider.transform.parent.position.y, (int)point.z / 9 * 9 + 5);
			}
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("foundation")){
				if(testSPos.x == g.transform.position.x && testSPos.z == g.transform.position.z)return false;
			}
		}
		else if(type == 0 && (name == "RockFPillar" || name == "Foundation" || tag == "pillar")){
			if(tag == "pillar"){
				Vector3 center = ray.GetPoint(r.distance - 1f);
				center = new Vector3((int)center.x / 9 * 9 + 5, center.y, (int)center.z / 9 * 9 + 5);
				int pillars = 0;
				foreach(Collider c in Physics.OverlapSphere(center, 6f))if(c.gameObject.tag == "pillar")pillars ++;
				if(pillars > 3)testSPos = new Vector3(center.x, r.collider.transform.position.y + 3f, center.z);
			}
			else testSPos = r.transform.parent.position;
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("platform")){
				if(testSPos == g.transform.position)return false;
			}
		}
		else if(type == 1 && (name == "RockFPillar" || tag == "pillar" || tag == "platform")){
			if(name == "RockFPillar")testSPos = new Vector3(r.transform.position.x, r.transform.position.y + 8.06f, r.transform.position.z);
			else if(tag == "pillar")testSPos = new Vector3(r.transform.position.x, r.transform.position.y + 6f, r.transform.position.z);
			else if(tag == "platform")testSPos = new Vector3(r.transform.position.x + -1 * (r.transform.position.x - r.point.x) / Mathf.Abs(r.transform.position.x - r.point.x) * 4.49f, r.transform.position.y + 3f, r.transform.position.z + -1 * (r.transform.position.z - r.point.z) / Mathf.Abs(r.transform.position.z - r.point.z) * 4.49f);
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("pillar"))
				if(Vector3.Distance(g.transform.position, testSPos) < 1f)return false;
		}
		else if(type > 1 && type < 5 && tag == "pillar"){
			testSPos = new Vector3((r.transform.position + r.normal * 4.49f).x, r.transform.position.y - 3f, (r.transform.position + r.normal * 4.49f).z);
			int pillars = 0, platforms = 0;
			foreach(Collider c in Physics.OverlapSphere(testSPos, 5f)){
				if(c.gameObject.tag == "pillar")pillars ++;
				if(c.gameObject.tag == "platform" && c.transform.position.y == testSPos.y)platforms ++;
			}
			if(pillars > 1 && platforms > 0){
				if(r.normal.z == 1f || r.normal.z == -1f)testSRot = new Vector3(0, 90, 0);
				foreach(GameObject g in GameObject.FindGameObjectsWithTag("wall")){
					if(testSPos == g.transform.position && testSRot == g.transform.eulerAngles)return false;
				}
			}
			else return false;
		}
		else if(type == 5 && tag == "platform"){
			if(tag == "platform"){
				float xDiff = r.transform.position.x - r.point.x, zDiff = r.transform.position.z - r.point.z;
				if(r.normal.y != 0f){
					testSPos = r.transform.position + new Vector3(0f, 0.4f, 0f);
					if(xDiff / Mathf.Abs(xDiff) == -1 && zDiff / Mathf.Abs(zDiff) == 1 )testSRot = new Vector3(0, 90, 0);
					else if(xDiff / Mathf.Abs(xDiff) == 1 && zDiff / Mathf.Abs(zDiff) == 1 )testSRot = new Vector3(0, 180, 0);
					else if(xDiff / Mathf.Abs(xDiff) == 1 && zDiff / Mathf.Abs(zDiff) == -1 )testSRot = new Vector3(0, 270, 0);
					else testSRot = new Vector3(0, 0, 0);
				}
				else{
					testSPos = r.transform.position + r.normal * 9f;
					testSPos = new Vector3(testSPos.x, testSPos.y - 5.6f, testSPos.z);
					foreach(Collider c in Physics.OverlapSphere(r.transform.position, 2f)){
						if(c.gameObject.name == "Foundation")stairShift = true;
					}
					if(r.normal.z == 1)testSRot = new Vector3(0, 90, 0);
					else if(r.normal.x == 1)testSRot = new Vector3(0, 180, 0);
					else if(r.normal.z == -1)testSRot = new Vector3(0, 270, 0);
					else testSRot = new Vector3(0, 0, 0);
				}
			}
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("stairs")){
				if(testSPos == g.transform.position)return false;
			}
		}
		else if(type == 7 && name == "Doorway"){
			testSRot = r.transform.parent.eulerAngles;
			testSPos = r.transform.parent.position + new Vector3(0f, 0.5f, 0f);
		}
		if(testSPos != new Vector3(0,0,0) && ((sType - 70) / 8 + 10 == r.collider.gameObject.layer || r.collider.gameObject.tag == "placeAble"))return true;
		return false;
	}

	//InGameMenu

	public void saveQuit(){;
		FileLink.saveData(WorldData.world);
		Application.LoadLevel("Menu");
	}

    public void resume(){
        inMenu = false;
        inGameC.SetActive(false);
		GameObject.Find("Main Camera").GetComponent<BlurOptimized>().enabled = false;
    }

    public void options(){
        inGameC.SetActive(false);
        controlsC.SetActive(true);
    }

    public void back(){
        inGameC.SetActive(true);
        controlsC.SetActive(false);
    }

}
