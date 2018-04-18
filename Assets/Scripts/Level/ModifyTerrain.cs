using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ModifyTerrain : MonoBehaviour {
	
	World world;
	GameObject cameraGO;

	// Use this for initialization
	void Start () {
	
		world=gameObject.GetComponent("World") as World;
		cameraGO=GameObject.FindGameObjectWithTag("MainCamera");
			
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonDown(0)){
			//ReplaceBlockCenter(5,0);
		}
		
		if(Input.GetMouseButtonDown(1)){
			//AddBlockCenter(5,255);
			
		}
		
		LoadChunks();
		
	}
	
	public void LoadChunks(){
		
		
		for(int x=0;x<world.chunks.GetLength(0);x++){
			for(int z=0;z<world.chunks.GetLength(2);z++){

				if(world.chunks[x,0,z]==null){
					world.GenColumn(x,z);
				}
			}
		}
		
	}
	
	public void SphereAtPosition(Vector3 _pos, float radius, byte block = 0){
        _pos = world.TransformWorldPosToLocal(_pos);
        _pos.x = Mathf.Round(_pos.x);
        _pos.y = Mathf.Round(_pos.y);
        _pos.z = Mathf.Round(_pos.z);
        radius /= world.transform.localScale.x;
        radius *= 2.0f;
        for (int x = (int)(_pos.x - radius); x < (int)(_pos.x + radius); x++)
        {
            for (int y = (int)(_pos.y - radius); y < (int)(_pos.y + radius); y++)
            {
                for (int z = (int)(_pos.z - radius); z < (int)(_pos.z + radius); z++)
                {

                    Vector3 newPos = new Vector3(x, y, z);
                    if (Vector3.Distance(_pos, newPos) < radius / 2.0f)
                        SetBlockAt(newPos, block);
                }
            }
        }
    }
	
	public void SetBlockAt(Vector3 position, byte block) {
		//sets the specified block at these coordinates
		
		int x= Mathf.RoundToInt( position.x );
		int y= Mathf.RoundToInt( position.y );
		int z= Mathf.RoundToInt( position.z );
		
		SetBlockAt(x,y,z,block);
	}
	
	public void SetBlockAt(int x, int y, int z, byte block) {

        //Check if given block is in bounds

        if (!world.isInBounds(x, y, z))
            return;

        
        //adds the specified block at these coordinates

  //      if (world.isInBounds(x + 1, y, z) && world.data[x + 1, y, z] == 254)
  //      {
  //          world.data[x + 1, y, z] = 255;
  //      }

  //      if (world.isInBounds(x-1, y, z) && world.data[x-1,y,z]==254){
		//	world.data[x-1,y,z]=255;
		//}
		//if(world.isInBounds(x, y, z + 1) && world.data[x,y,z+1]==254){
		//	world.data[x,y,z+1]=255;
		//}
		//if(world.isInBounds(x,y,z - 1) && world.data[x,y,z-1]==254){
		//	world.data[x,y,z-1]=255;
		//}
		//if(world.isInBounds(x, y + 1, z) && world.data[x,y+1,z]==254){
		//	world.data[x,y+1,z]=255;
		//}
		world.data[x,y,z]=block;

        UpdateChunkAt(x, y, z);

    }

    public void UpdateChunkAt(int x, int y, int z)
    { 
		//Updates the chunk containing this block
		
		int updateX= Mathf.FloorToInt( x/world.chunkSizexy);
		int updateY= Mathf.FloorToInt( y/world.chunkSizexy);
		int updateZ= Mathf.FloorToInt( z/world.chunkSizez);
		
		world.chunks[updateX,updateY, updateZ].update=true;
		
		if(x-(world.chunkSizexy*updateX)==0 && updateX!=0){
			world.chunks[updateX-1,updateY, updateZ].update=true;
		}
		
		if(x-(world.chunkSizexy*updateX)==15 && updateX!=world.chunks.GetLength(0)-1){
			world.chunks[updateX+1,updateY, updateZ].update=true;
		}
		
		if(y-(world.chunkSizexy*updateY)==0 && updateY!=0){
			world.chunks[updateX,updateY-1, updateZ].update=true;
		}
		
		if(y-(world.chunkSizexy*updateY)==15 && updateY!=world.chunks.GetLength(1)-1){
			world.chunks[updateX,updateY+1, updateZ].update=true;
		}
		
		if(z-(world.chunkSizez*updateZ)==0 && updateZ!=0){
			world.chunks[updateX,updateY, updateZ-1].update=true;
		}
		
		if(z-(world.chunkSizez*updateZ)==15 && updateZ!=world.chunks.GetLength(2)-1){
			world.chunks[updateX,updateY, updateZ+1].update=true;
		}
		
	}
	
}
