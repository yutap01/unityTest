using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour {
	
	private Transform cameraTrans;
	private Map map;
	private Block selectedBlock;
	
	void Awake() {
		cameraTrans = transform.GetComponentInChildren<Camera>().transform;
		map = (Map) GameObject.FindObjectOfType( typeof(Map) );
	}
	
	public void SetSelectedBlock(Block block) {
		selectedBlock = block;
	}

	public Block GetSelectedBlock() {
		return selectedBlock;
	}
	
	// Update is called once per frame
	void Update () {
		if(Screen.showCursor) return;
		
		if( Input.GetKeyDown(KeyCode.LeftControl) ) {
			Vector3i? point = GetCursor(false);
			if(point.HasValue) {
				byte light = map.GetLightmap().GetLight(point.Value.x, point.Value.y, point.Value.z);
				Debug.Log("Light "+light);
			}
		}
		
		if( Input.GetKeyDown(KeyCode.RightControl) ) {
			Vector3i? point = GetCursor(true);
			if(point.HasValue) {
				byte light = map.GetLightmap().GetLight(point.Value.x, point.Value.y, point.Value.z);
				Debug.Log("Light "+light);
			}
		}
		
		if( Input.GetMouseButtonDown(0) ) {
			Vector3i? point = GetCursor(true);
			if(point.HasValue) 
				map.SetBlockAndRecompute(new BlockData(), point.Value);
		}
		
		if( Input.GetMouseButtonDown(1) ) {
			Vector3i? point = GetCursor(false);
			if(point.HasValue) {
				BlockData block = new BlockData( selectedBlock );
				block.SetDirection( GetDirection(-transform.forward) );
				map.SetBlockAndRecompute(block, point.Value);
			}
		}
		
	}
	
	void OnDrawGizmos() {
		if(!Application.isPlaying) return;
		Vector3i? cursor = GetCursor(true);
		if(cursor.HasValue) {
			Gizmos.DrawWireCube( (Vector3)cursor.Value, Vector3.one*1.05f );
		}
	}
	
	private Vector3i? GetCursor(bool inside) {
		Ray ray = new Ray(cameraTrans.position, cameraTrans.forward);
		Vector3? point =  RayBoxCollision.Intersection(map, ray, 10);
		if( point.HasValue ) {
			Vector3 pos = point.Value;
			if(inside) pos += ray.direction*0.01f;
			if(!inside) pos -= ray.direction*0.01f;
			int posX = Mathf.RoundToInt(pos.x);
			int posY = Mathf.RoundToInt(pos.y);
			int posZ = Mathf.RoundToInt(pos.z);
			return new Vector3i(posX, posY, posZ);
		}
		return null;
	}
	
	private static BlockDirection GetDirection(Vector3 dir) {
		if( Mathf.Abs(dir.z) >= Mathf.Abs(dir.x) ) {
			// 0 или 180
			if(dir.z >= 0) return BlockDirection.Z_PLUS;
			return BlockDirection.Z_MINUS;
		} else {
			// 90 или 270
			if(dir.x >= 0) return BlockDirection.X_PLUS;
			return BlockDirection.X_MINUS;
		}
	}
	
}
