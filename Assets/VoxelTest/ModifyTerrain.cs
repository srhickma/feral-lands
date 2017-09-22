using UnityEngine;
using System.Collections;

public class ModifyTerrain : MonoBehaviour {

//	public World world;
//	public GameObject cameraGO;
//
//	void Start () {
//		world = gameObject.GetComponent("World") as World;
//	}
//
//	void Update () {
//		if(Input.GetMouseButtonDown(0)){
//			replaceBlockCenter(10, 0);
//		}
//		if(Input.GetMouseButtonDown(1)){
//			addBlockCenter(10, 1);
//		}
//	}
//
//	public void replaceBlockCenter(float range, byte block){
//		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
//		RaycastHit hit;
//		if (Physics.Raycast (ray, out hit)) {
//			if(hit.distance<range){
//				replaceBlockAt(hit, block);
//			}
//		}
//	}
//	
//	public void addBlockCenter(float range, byte block){
//		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
//		RaycastHit hit;
//		if (Physics.Raycast (ray, out hit)) {
//			if(hit.distance<range){
//				addBlockAt(hit,block);
//			}
//			Debug.DrawLine(ray.origin,ray.origin+( ray.direction*hit.distance),Color.green,2);
//		}
//	}
//	
//	public void ReplaceBlockCursor(byte block){
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//		if(Physics.Raycast(ray, out hit)){
//			replaceBlockAt(hit, block);
//			Debug.DrawLine(ray.origin,ray.origin+( ray.direction*hit.distance),Color.green,2);
//		}
//	}
//	
//	public void AddBlockCursor(byte block){
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//		if(Physics.Raycast(ray, out hit)){
//			addBlockAt(hit, block);
//			Debug.DrawLine(ray.origin,ray.origin+( ray.direction*hit.distance),Color.green,2);
//		}
//	}
//	
//	public void replaceBlockAt(RaycastHit hit, byte block){
//		Vector3 position = hit.point;
//		position += (hit.normal*-0.5f);
//		setBlockAt(position, block);
//	}
//
//	public void addBlockAt(RaycastHit hit, byte block){
//		Vector3 position = hit.point;
//		position+=(hit.normal*0.5f);
//		setBlockAt(position,block);
//	}
//	
//	public void setBlockAt(Vector3 position, byte block){
//		int x = Mathf.RoundToInt(position.x);
//		int y = Mathf.RoundToInt(position.y);
//		int z = Mathf.RoundToInt(position.z);
//		setBlockAt(x, y, z, block);
//	}
//	
//	public void setBlockAt(int x, int y, int z, byte block){
//		world.data[x, y, z].x = block;
//		updateChunkAt(x, y, z);
//	}
//	
//	public void updateChunkAt(int x, int y, int z){
//		int updateX = Mathf.FloorToInt(x/world.chunkSize);
//		int updateY = Mathf.FloorToInt(y/world.chunkSize);
//		int updateZ = Mathf.FloorToInt(z/world.chunkSize);
//		world.chunks[updateX,updateY, updateZ].update=true;
//		if(x-(world.chunkSize*updateX)==0 && updateX!=0){
//			world.chunks[updateX-1,updateY, updateZ].update=true;
//		}
//		if(x-(world.chunkSize*updateX)==15 && updateX!=world.chunks.GetLength(0)-1){
//			world.chunks[updateX+1,updateY, updateZ].update=true;
//		}
//		if(y-(world.chunkSize*updateY)==0 && updateY!=0){
//			world.chunks[updateX,updateY-1, updateZ].update=true;
//		}
//		if(y-(world.chunkSize*updateY)==15 && updateY!=world.chunks.GetLength(1)-1){
//			world.chunks[updateX,updateY+1, updateZ].update=true;
//		}
//		if(z-(world.chunkSize*updateZ)==0 && updateZ!=0){
//			world.chunks[updateX,updateY, updateZ-1].update=true;
//		}
//		if(z-(world.chunkSize*updateZ)==15 && updateZ!=world.chunks.GetLength(2)-1){
//			world.chunks[updateX,updateY, updateZ+1].update=true;
//		}
//	}

}
