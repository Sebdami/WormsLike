using UnityEngine;
using System.Collections;
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
			ReplaceBlockCenter(5,0);
		}
		
		if(Input.GetMouseButtonDown(1)){
			AddBlockCenter(5,255);
			
		}
		
		LoadChunks(GameObject.FindGameObjectWithTag("Player").transform.position,32,48);
		
	}
	
	public void LoadChunks(Vector3 playerPos, float distToLoad, float distToUnload){
		
		
		for(int x=0;x<world.chunks.GetLength(0);x++){
			for(int z=0;z<world.chunks.GetLength(2);z++){

				if(world.chunks[x,0,z]==null){
					world.GenColumn(x,z);
				}
			}
		}
		
	}
	
	public void ReplaceBlockCenter(float range, byte block){
		//Replaces the block directly in front of the player
		
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
        //Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f);
        //Debug.Log("oi");
        //if (colliders != null)
        //{
        //    for (int i = 0; i < colliders.Length; i++)
        //    {
        //        Debug.Log(colliders[i].transform.position);
        //        SetBlockAt(colliders[i].transform.position, block);
        //    }
        //}

        if (Physics.Raycast(ray, out hit))
        {
            range *= 2.0f;
            if (hit.distance < range)
            {
                Vector3 position = hit.point;
                position += (hit.normal * 0.5f);
                position.x = Mathf.Round(position.x);
                position.y = Mathf.Round(position.y);
                position.z = Mathf.Round(position.z);

                for (int x = (int)(position.x - range); x < (int)(position.x + range); x++)
                {
                    for (int y = (int)(position.y - range); y < (int)(position.y + range); y++)
                    {
                        for (int z = (int)(position.z - range); z < (int)(position.z + range); z++)
                        {

                            Vector3 newPos = new Vector3(x, y, z);
                            if (Vector3.Distance(position, newPos) < range/ 2.0f)
                            SetBlockAt(newPos, block);
                        }
                    }
                }
            }
        }

        




        //if (Physics.Raycast (ray, out hit)) {

        //	if(hit.distance<range){
        //		ReplaceBlockAt(hit, block);
        //	}
        //}

    }

	public void AddBlockCenter(float range, byte block){
		//Adds the block specified directly in front of the player
		
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
	
		if (Physics.Raycast (ray, out hit)) {
			
			if(hit.distance<range){
				AddBlockAt(hit,block);
			}
			Debug.DrawLine(ray.origin,ray.origin+( ray.direction*hit.distance),Color.green,2);
		}
		
	}
	
	public void ReplaceBlockCursor(byte block){
		//Replaces the block specified where the mouse cursor is pointing
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
	
		if (Physics.Raycast (ray, out hit)) {
			
			ReplaceBlockAt(hit, block);
			
		}
		
	}
	
	public void AddBlockCursor( byte block){
		//Adds the block specified where the mouse cursor is pointing
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
	
		if (Physics.Raycast (ray, out hit)) {
			
			AddBlockAt(hit, block);
			Debug.DrawLine(ray.origin,ray.origin+( ray.direction*hit.distance),Color.green,2);
		}
		
	}
	
	public void ReplaceBlockAt(RaycastHit hit, byte block) {
		//removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
			Vector3 position = hit.point;
			position+=(hit.normal*-0.5f);
			
			SetBlockAt(position, block);
	}
	
	public void AddBlockAt(RaycastHit hit, byte block) {
		//adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
			Vector3 position = hit.point;
			position+=(hit.normal*0.5f);
			
			SetBlockAt(position,block);
			
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

        if (world.isInBounds(x + 1, y, z) && world.data[x + 1, y, z] == 254)
        {
            world.data[x + 1, y, z] = 255;
        }

        if (world.isInBounds(x-1, y, z) && world.data[x-1,y,z]==254){
			world.data[x-1,y,z]=255;
		}
		if(world.isInBounds(x, y, z + 1) && world.data[x,y,z+1]==254){
			world.data[x,y,z+1]=255;
		}
		if(world.isInBounds(x,y,z - 1) && world.data[x,y,z-1]==254){
			world.data[x,y,z-1]=255;
		}
		if(world.isInBounds(x, y + 1, z) && world.data[x,y+1,z]==254){
			world.data[x,y+1,z]=255;
		}
		world.data[x,y,z]=block;
		
		UpdateChunkAt(x,y,z,block);
	
	}
	
	public void UpdateChunkAt(int x, int y, int z, byte block){		//To do: add a way to just flag the chunk for update and then it updates in lateupdate
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
