using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class FileLink : MonoBehaviour {

	static string path = Application.dataPath + "/" + "WDAT";
	
	void Start () {
		
	}

	void Update () {
		
	}

	public static void saveData(string name){
		if(!Directory.Exists(path))Directory.CreateDirectory(path);
		string players = "", machines = "", stumps = "", rocks = "", animals = "", crates = "", structures = "";
		using(StreamWriter sW = File.CreateText(path + "/" + name + ".txt")){
			sW.WriteLine(encrypt(WorldData.sType));
			GameObject player = GameObject.Find("Player");
			Player playerO = player.GetComponent<Player>();
			string inv = "", toolbar = "", armor = "";
			foreach(Vector2 v in playerO.inv){
				if(inv == "")inv += v.x.ToString() + "$" + v.y.ToString();
				else inv += "#" + v.x.ToString() + "$" + v.y.ToString();
			}
			foreach(Vector2 v in playerO.toolbar){
				if(toolbar == "")toolbar += v.x.ToString() + "$" + v.y.ToString();
				else toolbar += "#" + v.x.ToString() + "$" + v.y.ToString();
			}
			foreach(Vector2 v in playerO.armor){
				if(armor == "")armor += v.x.ToString() + "$" + v.y.ToString();
				else armor += "#" + v.x.ToString() + "$" + v.y.ToString();
			}
			players += "player@" + player.transform.position.x + "#" + player.transform.position.y + "#" + player.transform.position.z + "@" + playerO.health + "@" + playerO.nutrition + "@" + inv + "@" + toolbar + "@" + armor + "@" + playerO.spawnPoint.x + "#" + playerO.spawnPoint.y + "#" + playerO.spawnPoint.z;
			sW.WriteLine(encrypt(players));
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("machine")){
				if(g.name.Split('(').Length == 1){
					if(machines != "")machines += "!";
					machines += g.name + "@" + g.transform.position.x + "#" + g.transform.position.y + "#" + g.transform.position.z + "@" + g.transform.eulerAngles.y;
					Vector2[] mInv = null;	
					string mInvS = "@";
					if(g.name == "Iron Furnace" || g.name == "Stone Furnace")mInv = g.GetComponent<Furnace>().inv;
					else if(g.name == "Oven" || g.name == "Campfire")mInv = g.GetComponent<Oven>().inv;
					else if(g.name == "Chest")mInv = g.GetComponent<Chest>().inv;
					else mInvS = "";
					if(mInv != null){
						foreach(Vector2 v in mInv){
							if(mInvS != "@")mInvS += "#";
							mInvS += v.x.ToString() + "$" + v.y.ToString();
						}
					}
					machines += mInvS;
				}
			}
			sW.WriteLine(encrypt(machines));
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("stump")){
				if(stumps != "")stumps += "!";
				stumps += g.transform.position.x + "@" + g.transform.position.y + "@" + g.transform.position.z;
			}
			sW.WriteLine(encrypt(stumps));
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("rock")){
				if(rocks != "")rocks += "!";
				rocks += g.GetComponent<Rock>().spawnIndex + "@" + g.GetComponent<Rock>().type;
			}
			sW.WriteLine(encrypt(rocks));
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("animal")){
				int health = 0;
				if(g.name == "Rabbit(Clone)")health = g.GetComponent<Rabbit>().health;
				else if(g.name == "Deer(Clone)")health = g.GetComponent<Deer>().health;
				else if(g.name == "Boar(Clone)")health = g.GetComponent<Boar>().health;
				else if(g.name == "Bear(Clone)")health = g.GetComponent<Bear>().health;
				else if(g.name == "Chicken(Clone)")health = g.GetComponent<Chicken>().health;
				if(health > 0){
					if(animals != "")animals += "!";
					animals += g.name.Substring(0, 2) + "@" + g.transform.position.x + "#" + g.transform.position.y + "#" + g.transform.position.z + "@" + g.transform.eulerAngles.y + "@" + health;
				}	
			}
			sW.WriteLine(encrypt(animals));
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("crate")){
				if(crates != "")crates += "!";
				crates += g.transform.position.x + "#" + g.transform.position.y + "#" + g.transform.position.z + "@" + g.GetComponent<Crate>().item.x + "#" + g.GetComponent<Crate>().item.y;
			}
			sW.WriteLine(encrypt(crates));
			List<GameObject> s = new List<GameObject>();
			s.AddRange(GameObject.FindGameObjectsWithTag("platform"));
			s.AddRange(GameObject.FindGameObjectsWithTag("pillar"));
			s.AddRange(GameObject.FindGameObjectsWithTag("wall"));
			s.AddRange(GameObject.FindGameObjectsWithTag("stairs"));
			s.AddRange(GameObject.FindGameObjectsWithTag("foundation"));
			s.AddRange(GameObject.FindGameObjectsWithTag("door"));
			foreach(GameObject g in s){
				Structure script = g.GetComponent<Structure>();
				if(structures != "")structures += "!";
				int shifted = 0;
				if((script.type - 70) % 8 == 5){
					foreach(Transform t in g.GetComponentsInChildren<Transform>()){
						if(t.localPosition.z == 0f)shifted = 1;
					}
				}
				structures += (script.type - 70) + "@" + g.transform.position.x + "#" + g.transform.position.y + "#" + g.transform.position.z + "@" + g.transform.eulerAngles.y + "@" + script.creator + "@" + script.durability + "@" + shifted;
			}
			sW.WriteLine(encrypt(structures));
            Clock clock = GameObject.Find("Clock").GetComponent<Clock>();
            sW.WriteLine(encrypt(clock.hour + "!" + clock.minute));
		}
	}

	public static bool loadData(string name){
		try{
			string[] players = null, machines = null, stumps = null, rocks = null, animals = null, crates = null, structures = null;
            string time = "";
			using(StreamReader sR = new StreamReader(path + "/" + name + ".txt")){
				WorldData.sType = decrypt(sR.ReadLine());
				players = decrypt(sR.ReadLine()).Split('!');
				machines = decrypt(sR.ReadLine()).Split('!');
				stumps = decrypt(sR.ReadLine()).Split('!');
				rocks = decrypt(sR.ReadLine()).Split('!');
				animals = decrypt(sR.ReadLine()).Split('!');
				crates = decrypt(sR.ReadLine()).Split('!');
				structures = decrypt(sR.ReadLine()).Split('!');
                time = decrypt(sR.ReadLine());
			}
			string[] data;
			GameObject player = GameObject.Find("Player");
			Player playerS = player.GetComponent<Player>();
			foreach(string p in players){
				if(p.Split('@')[0] == "player"){
					data = p.Split('@')[1].Split('#');
					player.transform.position = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
					data = p.Split('@');
					playerS.health = Convert.ToInt16(data[2]);
					playerS.nutrition = Convert.ToInt16(data[3]);
					data = p.Split('@')[4].Split('#');
					for(int i = 0; i < data.Length; i ++)
						playerS.inv[i] = new Vector2((float)Convert.ToDouble(data[i].Split('$')[0]), (float)Convert.ToDouble(data[i].Split('$')[1]));
					data = p.Split('@')[5].Split('#');
					for(int i = 0; i < data.Length; i ++)
						playerS.toolbar[i] = new Vector2((float)Convert.ToDouble(data[i].Split('$')[0]), (float)Convert.ToDouble(data[i].Split('$')[1]));
					data = p.Split('@')[6].Split('#');
					for(int i = 0; i < data.Length; i ++)
						playerS.armor[i] = new Vector2((float)Convert.ToDouble(data[i].Split('$')[0]), (float)Convert.ToDouble(data[i].Split('$')[1]));
					data = p.Split('@')[7].Split('#');
					playerS.spawnPoint = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
				}
			}
			foreach(string m in machines){
				foreach(GameObject g in playerS.machines){
					if(g.name == m.Split('@')[0]){
						Vector3 pos, rot;
						data = m.Split('@')[1].Split('#');
						pos = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
						rot = new Vector3(0f, (float)Convert.ToDouble(m.Split('@')[2]), 0f);
						GameObject newM = Instantiate(g, pos, player.transform.rotation) as GameObject;
						newM.name = newM.name.Split('(')[0];
						newM.transform.eulerAngles = rot;
						if(m.Split('@').Length == 4){
							MonoBehaviour mScript = null;
							if(g.name == "Iron Furnace" || g.name == "Stone Furnace")mScript = newM.GetComponent<Furnace>();
							else if(g.name == "Oven" || g.name == "Campfire")mScript = newM.GetComponent<Oven>();
							else if(g.name == "Chest")mScript = newM.GetComponent<Chest>();
							data = m.Split('@')[3].Split('#');
							Vector2[] mInv = new Vector2[data.Length];
							for(int i = 0; i < data.Length; i ++)
								mInv[i] = new Vector2((float)Convert.ToDouble(data[i].Split('$')[0]), (float)Convert.ToDouble(data[i].Split('$')[1]));
							mScript.SendMessage("loadInv", mInv);
						}
					}
				}
			}
			foreach(string s in stumps){
				if(s != ""){
					foreach(GameObject t in GameObject.FindGameObjectsWithTag("tree")){
						if(Vector3.Distance(t.transform.position, new Vector3((float)Convert.ToDouble(s.Split('@')[0]), (float)Convert.ToDouble(s.Split('@')[1]), (float)Convert.ToDouble(s.Split('@')[2]))) < 1f)t.GetComponent<Tree>().loadStump();
					}
				}
			}
			RockSpawner rockSpawner = GameObject.Find("RockSpawner").GetComponent<RockSpawner>();
			foreach(string r in rocks)
				if(r != "")rockSpawner.loadRock(Convert.ToInt16(r.Split('@')[0]), Convert.ToInt16(r.Split('@')[1]));
			rockSpawner.loadComplete = true;
			AnimalSpawner animalSpawner = GameObject.Find("AnimalSpawner").GetComponent<AnimalSpawner>();
			foreach(string a in animals){
				if(a != ""){
					GameObject animal = null;
					if(a.Split('@')[0] == "Ra")animal = animalSpawner.rabbit;
					else if(a.Split('@')[0] == "De")animal = animalSpawner.deer;
					else if(a.Split('@')[0] == "Bo")animal = animalSpawner.boar;
					else if(a.Split('@')[0] == "Be")animal = animalSpawner.bear;
					else if(a.Split('@')[0] == "Ch")animal = animalSpawner.chicken;
					Vector3 pos, rot;
					data = a.Split('@')[1].Split('#');
					pos = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
					rot = new Vector3(0f, (float)Convert.ToDouble(a.Split('@')[2]), 0f);
					GameObject newA = Instantiate(animal, pos, player.transform.rotation) as GameObject;
					newA.transform.eulerAngles = rot;
					newA.SendMessage("loadHealth", Convert.ToInt16(a.Split('@')[3]));
					AnimalSpawner.animals ++;
				}
			}
			animalSpawner.loadComplete = true;
			GameObject crate = GameObject.Find("Player").GetComponent<Player>().crateGO;
			foreach(string c in crates){
				if(c != ""){
					data = c.Split('@')[0].Split('#');
					Vector3 pos = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
					data = c.Split('@')[1].Split('#');
					GameObject newC = Instantiate(crate, pos, new Quaternion(0,0,0,0)) as GameObject;
					newC.GetComponent<Crate>().spawn(new Vector2((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1])));
				}
			}
			foreach(string s in structures){
				if(s != ""){
					data = s.Split('@')[1].Split('#');
					Vector3 pos = new Vector3((float)Convert.ToDouble(data[0]), (float)Convert.ToDouble(data[1]), (float)Convert.ToDouble(data[2]));
					GameObject newS = Instantiate(playerS.structures[Convert.ToInt16(s.Split('@')[0])], pos, new Quaternion(0,0,0,0)) as GameObject;
					newS.transform.eulerAngles = new Vector3(0f, (float)Convert.ToDouble(s.Split('@')[2]), 0f);
					data = s.Split('@');
					newS.GetComponent<Structure>().load(Convert.ToInt16(data[0]), data[3], Convert.ToInt16(data[4]));
					if(Convert.ToInt16(data[0]) % 8 == 7)newS.GetComponent<Door>().creator = data[3];
					else if(Convert.ToInt16(data[0]) % 8 == 5 && data[5] == "1"){
						foreach(Transform t in newS.GetComponentsInChildren<Transform>())t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0f);
						newS.transform.position = pos;
					}
				}
			}
            Clock clock = GameObject.Find("Clock").GetComponent<Clock>();
            clock.hour = Convert.ToInt16(time.Split('!')[0]);
            clock.second = (float)Convert.ToInt16(time.Split('!')[1]);
			return true;
		}catch{return false;}
	}

    public static string[] getSaves(){
        List<String> saves = new List<String>();
        if(Directory.Exists(path)){
            string[] files = Directory.GetFiles(path);
            foreach(string fileName in files){
                string name = fileName.Split('\\')[fileName.Split('\\').Length - 1].Split('.')[0];
                if(name != "")saves.Add(name);
            }
        }
        return saves.ToArray();
    }

    public static void deleteSave(string name){
        try{
            if(Directory.Exists(path)){
                string[] files = Directory.GetFiles(path);
                foreach(string fileName in files){
                    string fName = fileName.Split('\\')[fileName.Split('\\').Length - 1].Split('.')[0];
                    if(fName == name)File.Delete(fileName);
                }   
            }
        }catch{};
    }

	private static string encrypt(string inp){
		String outp = "";
		foreach(char c in inp.ToCharArray()){
			outp += Convert.ToString(Convert.ToInt32(c), 2) + " ";
		}
		return outp;
	}

	private static string decrypt(string inp){
		String outp = "";
		foreach(string s in inp.Split(' ')){
			if(s != "")outp += Convert.ToChar(Convert.ToInt32(s, 2));
		}
		return outp;
	}

}
