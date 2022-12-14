using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGeneration : MonoBehaviour {
	public terrainGenerator generator;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		generateSurrounding(Chunk.getRoundChunkPos(transform.position));
	}

	public void generateSurrounding(Vector3Int chunkPos){
		for(int i=-1;i<2;i++){
			for(int j=-1; j<2;j++){
				generator.generateChunk(chunkPos.x + i, chunkPos.z +j);
			}
		}
	}
}
