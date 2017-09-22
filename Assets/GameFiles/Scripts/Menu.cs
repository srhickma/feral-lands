using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private GameObject playerGO;
	public GameObject mainC, controlsC, spC, KJSC;
    public GameObject SPIn, SPLoadScroll, button, deleteButton, SPScrollView, SPScrollBar;
	public bool sP = false, mP = false;
    public List<GameObject> worlds = new List<GameObject>();
    private int scrollFixCount = 0;
    private bool KJSDone = false;

	void Start () {
        
	}

	void Update () {
        if(Time.time > 2f && KJSDone == false){
            KJSC.SetActive(false);
            mainC.SetActive(true);
            KJSDone = true;
        }
        if(scrollFixCount > 0){
            SPScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            scrollFixCount --;
        }
	}

	public void randomSpawn(){
		playerGO = GameObject.Find("Player");
		float x, z;
		bool foundSpawn = false;
		RaycastHit h;
		do{
			x = Random.Range(0, 1000) - 500;
			z = Random.Range(0, 1000) - 500;
			Ray r = new Ray(new Vector3(1000f + x, 1000f, 1000f + z), -transform.up);
			if(Physics.Raycast(r, out h) && h.collider.gameObject.tag == "placeAble")foundSpawn = true;
		}while(!foundSpawn);
		playerGO.transform.position = new Vector3(1000f + x, h.point.y + 5f, 1000f + z);
	}

	//Main

    public void singleplayer(){
		foreach(Transform child in SPLoadScroll.transform)
			Destroy(child.gameObject);
        worlds = new List<GameObject>();
        SPLoadScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(300,0);
        SPLoadScroll.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        mainC.SetActive(false);
        spC.SetActive(true);
        foreach(string s in FileLink.getSaves()){
            GameObject g = Instantiate(button, new Vector3(0 , 0, 0), new Quaternion(0,0,0,0)) as GameObject;
            GameObject x = Instantiate(deleteButton, new Vector3(0 , 0, 0), new Quaternion(0,0,0,0)) as GameObject;
            g.transform.SetParent(SPLoadScroll.transform, false);
            x.transform.SetParent(SPLoadScroll.transform, false);
            if(worlds.Count != 0)g.GetComponent<RectTransform>().localPosition = worlds[worlds.Count - 1].GetComponent<RectTransform>().localPosition + new Vector3(0f, -30f, 0f);
            else g.GetComponent<RectTransform>().localPosition = new Vector3(0 , -15, 0);
            x.GetComponent<RectTransform>().localPosition = g.GetComponent<RectTransform>().localPosition + new Vector3(110, 0, 0);
            g.transform.localScale = new Vector3(1f, 1f, 1f);
            x.transform.localScale = new Vector3(1f, 1f, 1f);
            g.name = s;
            x.name = s;
            g.GetComponentInChildren<Text>().text = s;
            g.GetComponent<Button>().onClick.AddListener(delegate{loadSP(g.name);});
            x.GetComponent<Button>().onClick.AddListener(delegate{deleteWorld(x.name);});
            SPLoadScroll.GetComponent<RectTransform>().sizeDelta += new Vector2(0f,30f);
            SPLoadScroll.GetComponent<RectTransform>().localPosition -= new Vector3(0f,15f,0f);
            worlds.Add(g);
        }
        scrollFixCount = 10;
    }

    public void createSP(){
        if(SPIn.GetComponent<InputField>().text != "")loadSP(SPIn.GetComponent<InputField>().text);
    }

    public void loadSP(string name){
        sP = true;
		WorldData.sType = "s";
		DontDestroyOnLoad(gameObject);
        WorldData.world = name;
		Application.LoadLevel("World");
    }

    public void deleteWorld(string name){
        FileLink.deleteSave(name);
        singleplayer();
    }

	public void multiplayer(){
		mP = true;
		WorldData.sType = "m";
		DontDestroyOnLoad(gameObject);
		Application.LoadLevel("World");
	}

	public void controls(){
		mainC.SetActive(false);
		controlsC.SetActive(true);
	}

	public void quit(){Application.Quit();}

	//Options

	public void back(){
		mainC.SetActive(true);
		controlsC.SetActive(false);
        spC.SetActive(false);
	}

}
