using UnityEngine;
using System.Collections;

public class ChunkData {

	private BlockData[,,] blocks = new BlockData[Chunk.SIZE_Z, Chunk.SIZE_Y, Chunk.SIZE_X];
	private Map map;
	private Vector3i position;
	private Chunk chunk;
	
	public ChunkData(Map map, Vector3i position) {
		this.map = map;
		this.position = position;
	}
	
	public Chunk GetChunkInstance() {
		if(chunk == null) chunk = Chunk.CreateChunk(position, map, this);
		return chunk;
	}
	public Chunk GetChunk() {
		return chunk;
	}
	
	
	public void SetBlock(BlockData block, Vector3i pos) {
		SetBlock(block, pos.x, pos.y, pos.z);
	}
	public void SetBlock(BlockData block, int x, int y, int z) {
		blocks[z, y, x] = block;
		map.GetLightmap().SetLight(LightComputer.MIN_LIGHT, position, new Vector3i(x, y, z));
	}
	
	public BlockData GetBlock(Vector3i pos) {
		return GetBlock(pos.x, pos.y, pos.z);
	}
	public BlockData GetBlock(int x, int y, int z) {
		return blocks[z, y, x];
	}
	
	
	public Map GetMap() {
		return map;
	}
	public Vector3i GetPosition() {
		return position;
	}
	
}
