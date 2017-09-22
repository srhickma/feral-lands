using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Noise;

public class Chunk : MonoBehaviour {

	public NoiseGen noiseGen = new NoiseGen();
	private List<Vector3> newVertices = new List<Vector3>();
	private List<int> newTriangles = new List<int>();
	private List<Vector2> newUV = new List<Vector2>();
	private List<Vector2> lightUV = new List<Vector2>();
	private Vector2[,,] blocks;
	private float tUnit = 0.25f;
	private Vector2 tStone = new Vector2 (0, 3);
	private Vector2 tDirt = new Vector2 (1, 3);
	private Vector2 tGrassTop = new Vector2 (2, 3);
	private Vector2 tGrass = new Vector2 (3, 3);
	private Mesh mesh;
	private MeshCollider col;
	private int faceCount;
	public GameObject worldGO;
	private World world;
	public int chunkSize = 64;
	public Vector3 chunkPos;
	private int chunkX, chunkY, chunkZ;
	private bool genFinished = false;
	public bool update = false;
	private float lMUnit = 0.1f;
	public int lightL = 5;

	void Start(){
		
	}

	void Update(){
	
	}

	public void initChunk(){
		world = worldGO.GetComponent("World") as World;
		mesh = GetComponent<MeshFilter> ().mesh;
		col = GetComponent<MeshCollider> ();
		genChunk();
	}
	
	public void genChunk(){
		chunkX = (int) chunkPos.x * chunkSize;
		chunkY = (int) chunkPos.y * chunkSize;
		chunkZ = (int) chunkPos.z * chunkSize;
		blocks = new Vector2[chunkSize, chunkSize, chunkSize];
		for(int x = chunkX; x < chunkX + chunkSize; x ++){
			for(int z = chunkZ; z < chunkZ + chunkSize; z ++){
				x = Mathf.Abs(x);
				z = Mathf.Abs(z);
				int stone = PerlinNoise(x,0,z,10,5,1.2f);
				stone += PerlinNoise(x,300,z,35,5,0) + 10;
				int dirt = PerlinNoise(x,100,z,50,2,0) + 1;
				for(int y = chunkY; y < chunkY + chunkSize; y ++){
					if(y <= stone)blocks[Mathf.Abs(x % chunkSize),Mathf.Abs(y % chunkSize),Mathf.Abs(z % chunkSize)].x = 1;
					else if(y <= dirt + stone)blocks[Mathf.Abs(x % chunkSize),Mathf.Abs(y % chunkSize),Mathf.Abs(z % chunkSize)].x = 2;
				}
			}
		}
		generateMesh();
	}

	void LateUpdate(){
		if(update){
			generateMesh();
			update = false;
		}
	}

	void updateMesh(){
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.uv2 = lightUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();
		col.sharedMesh = null;
		col.sharedMesh = mesh;
		newVertices.Clear();
		newUV.Clear();
		lightUV.Clear();
		newTriangles.Clear();
		faceCount = 0;
	}

	public void generateMesh(){
		for(int x=0; x<chunkSize; x++){
			for(int y=0; y<chunkSize; y++){
				for(int z=0; z<chunkSize; z++){
					if(block(x,y,z).x != 0){
						if(block(x,y+1,z).x == 0){
							cubeTop(x,y,z);
						}
						
						if(block(x,y-1,z).x == 0){
							cubeBot(x,y,z);
						}
						
						if(block(x+1,y,z).x == 0){
							cubeEast(x,y,z);
						}
						
						if(block(x-1,y,z).x == 0){
							cubeWest(x,y,z);
						}
						
						if(block(x,y,z+1).x == 0){
							cubeNorth(x,y,z);
						}
						
						if(block(x,y,z-1).x == 0){
							cubeSouth(x,y,z);
						}
						
					}
					
				}
			}
		}
		updateMesh();
	}
	
	void cube (Vector2 texturePos, int x, int y, int z){
		newTriangles.Add(faceCount * 4 );
		newTriangles.Add(faceCount * 4 + 1);
		newTriangles.Add(faceCount * 4 + 2);
		newTriangles.Add(faceCount * 4 );
		newTriangles.Add(faceCount * 4 + 2);
		newTriangles.Add(faceCount * 4 + 3);
		newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
		newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		lightUV.Add(new Vector2 (lMUnit * block(x, y, z).y + 0.09f, 0.01f));
		lightUV.Add(new Vector2 (lMUnit * block(x, y, z).y + 0.09f, 0.09f));
		lightUV.Add(new Vector2 (lMUnit * block(x, y, z).y + 0.01f, 0.09f));
		lightUV.Add(new Vector2 (lMUnit * block(x, y, z).y + 0.01f, 0.01f));
		faceCount++;
	}

	void cubeTop(int x, int y, int z){
		newVertices.Add(new Vector3 (x,  y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z ));
		newVertices.Add(new Vector3 (x,  y,  z ));
		Vector2 texturePos=new Vector2(0,0);
		if(blocks[x,y,z].x == 1)texturePos=tStone;
		else if(blocks[x,y,z].x == 2)texturePos=tGrassTop;
		cube(texturePos, x, y, z);
	}

	void cubeBot(int x, int y, int z){
		newVertices.Add(new Vector3 (x,  y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z + 1));
		newVertices.Add(new Vector3 (x,  y-1,  z + 1));
		Vector2 texturePos=new Vector2(0,0);
		if(blocks[x,y,z].x == 1)texturePos=tStone;
		else if(blocks[x,y,z].x == 2)texturePos=tDirt;
		cube(texturePos, x, y, z);
	}

	void cubeNorth(int x, int y, int z){
		newVertices.Add(new Vector3 (x + 1, y-1, z + 1));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y-1, z + 1));
		Vector2 texturePos=new Vector2(0,0);
		if(blocks[x,y,z].x == 1)texturePos=tStone;
		else if(blocks[x,y,z].x == 2)texturePos=tGrass;
		cube(texturePos, x, y, z);
	}

	void cubeEast(int x, int y, int z){
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x + 1, y - 1, z + 1));
		Vector2 texturePos=new Vector2(0,0);
		if(blocks[x,y,z].x == 1)texturePos=tStone;
		else if(blocks[x,y,z].x == 2)texturePos=tGrass;
		cube(texturePos, x, y, z);
	}

	void cubeSouth(int x, int y, int z){
		newVertices.Add(new Vector3 (x, y - 1, z));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		Vector2 texturePos=new Vector2(0,0);
		if(blocks[x,y,z].x == 1)texturePos=tStone;
		else if(blocks[x,y,z].x == 2)texturePos=tGrass;
		cube(texturePos, x, y, z);
	}

	void cubeWest(int x, int y, int z){
		newVertices.Add(new Vector3 (x, y- 1, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x, y - 1, z));
		Vector2 texturePos=new Vector2(0,0);
		if(blocks[x,y,z].x == 1)texturePos=tStone;
		else if(blocks[x,y,z].x == 2)texturePos=tGrass;
		cube(texturePos, x, y, z);
	}

	Vector2 block(int x, int y, int z){
		if(x < chunkSize && y < chunkSize && z < chunkSize && x > -1 && y > -1 && z > -1)return blocks[x, y, z];
		else return new Vector2(0, 0);
//		Vector3 nCVector = chunkPos;
//		if(x == 16){
//			nCVector.x ++;
//			x = 0;
//		}
//		else if(x == -1){
//			nCVector.x --;
//			x = 15;
//		}
//		if(y == 16){
//			nCVector.y ++;
//			y = 0;
//		}
//		else if(y == -1){
//			nCVector.y --;
//			y = 15;
//		}
//		if(z == 16){
//			nCVector.z ++;
//			z = 0;
//		}
//		else if(z == -1){
//			nCVector.z --;
//			z = 15;
//		}
		//Chunk foundChunk = world.searchChunks(nCVector);
		//if(foundChunk == null)return new Vector2(0, 0);
		//return foundChunk.blocks[x, y, z];
	}

	int PerlinNoise(int x,int y, int z, float scale, float height, float power){
		float rValue;
		rValue= noiseGen.GetNoise(((double)x) / scale * 10, ((double)y)/ scale, ((double)z) / scale / 5);
		rValue*=height * 5;
		
		if(power!=0){
			rValue=Mathf.Pow( rValue, power);
		}
		
		return (int) rValue;
	}

}
