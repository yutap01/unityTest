using UnityEngine;
using System.Collections;

public class Grid<T> {
	
	private T[,,] grid;
	private int minX, minY, minZ;
	private int maxX, maxY, maxZ;
	
	public Grid() {
		grid = new T[0, 0, 0];
	}
	
	public Grid(Vector3i min, Vector3i max) {
		this.minX = min.x;
		this.minY = min.y;
		this.minZ = min.z;
		
		this.maxX = max.x;
		this.maxY = max.y;
		this.maxZ = max.z;
		
		Vector3i size = GetSize();
		grid = new T[size.z, size.y, size.x];
	}
	
	
	
	public void Set(T obj, Vector3i pos) {
		grid[pos.z-minZ, pos.y-minY, pos.x-minX] = obj;
	}
	public void Set(T obj, int x, int y, int z) {
		grid[z-minZ, y-minY, x-minX] = obj;
	}
	
	public T Get(Vector3i pos) {
		return grid[pos.z-minZ, pos.y-minY, pos.x-minX];
	}
	public T Get(int x, int y, int z) {
		return grid[z-minZ, y-minY, x-minX];
	}
	
	public T SafeGet(Vector3i pos) {
		if(!IsCorrectIndex(pos.x, pos.y, pos.z)) return default(T);
		return grid[pos.z-minZ, pos.y-minY, pos.x-minX];
	}
	public T SafeGet(int x, int y, int z) {
		if(!IsCorrectIndex(x, y, z)) return default(T);
		return grid[z-minZ, y-minY, x-minX];
	}
	
	public void AddOrReplace(T obj, Vector3i pos) {
		AddOrReplace(obj, pos.x, pos.y, pos.z);
	}
	public void AddOrReplace(T obj, int x, int y, int z) {
		int dMinX = 0, dMinY = 0, dMinZ = 0;
		int dMaxX = 0, dMaxY = 0, dMaxZ = 0;
		
		if(x < minX) dMinX = x-minX;
		if(y < minY) dMinY = y-minY;
		if(z < minZ) dMinZ = z-minZ;
		
		if(x >= maxX) dMaxX = x-maxX+1;
		if(y >= maxY) dMaxY = y-maxY+1;
		if(z >= maxZ) dMaxZ = z-maxZ+1;
		
		if(dMinX != 0 || dMinY != 0 || dMinZ != 0 || 
		   dMaxX != 0 || dMaxY != 0 || dMaxZ != 0) {
			Increase(dMinX, dMinY, dMinZ, 
			     	 dMaxX, dMaxY, dMaxZ);
		}
		
		grid[z-minZ, y-minY, x-minX] = obj;
	}
	
	private void Increase(int dMinX, int dMinY, int dMinZ, 
		                  int dMaxX, int dMaxY, int dMaxZ) {
		int oldMinX = minX;
		int oldMinY = minY;
		int oldMinZ = minZ;
		
		int oldMaxX = maxX;
		int oldMaxY = maxY;
		int oldMaxZ = maxZ;
		
		T[,,] oldGrid = grid;
		
		minX += dMinX;
		minY += dMinY;
		minZ += dMinZ;
		
		maxX += dMaxX;
		maxY += dMaxY;
		maxZ += dMaxZ;
		
		int sizeX = maxX-minX;
		int sizeY = maxY-minY;
		int sizeZ = maxZ-minZ;
		grid = new T[sizeZ, sizeY, sizeX];
		
		for(int z=oldMinZ; z<oldMaxZ; z++) {
			for(int y=oldMinY; y<oldMaxY; y++) {
				for(int x=oldMinX; x<oldMaxX; x++) {
					grid[z-minZ, y-minY, x-minX] = oldGrid[z-oldMinZ, y-oldMinY, x-oldMinX];
				}
			}
		}
	}
	
	public bool TestIndex(int x, int y, int z) {
		if( !(x>=0 && x<grid.GetLength(2)) ) Debug.Log("Error X "+x);
		if( !(y>=0 && y<grid.GetLength(1)) ) Debug.Log("Error Y "+y);
		if( !(z>=0 && z<grid.GetLength(0)) ) Debug.Log("Error Z "+z);
		return x>=0 && x<grid.GetLength(2) &&
			   y>=0 && y<grid.GetLength(1) &&
			   z>=0 && z<grid.GetLength(0);
	}
	
	public bool IsCorrectIndex(Vector3i pos) {
		return IsCorrectIndex(pos.x, pos.y, pos.z);
	}
	public bool IsCorrectIndex(int x, int y, int z) {
		if(x<minX  || y<minY  || z<minZ) return false;
		if(x>=maxX || y>=maxY || z>=maxZ) return false;
		return true;
	}
	
	public Vector3i GetMin() {
		return new Vector3i(minX, minY, minZ);
	}
	
	public Vector3i GetMax() {
		return new Vector3i(maxX, maxY, maxZ);
	}
	
	public Vector3i GetSize() {
		return new Vector3i(maxX-minX, maxY-minY, maxZ-minZ);
	}
	
	public int GetMinX() {
		return minX;
	}
	public int GetMinY() {
		return minY;
	}
	public int GetMinZ() {
		return minZ;
	}
	
	public int GetMaxX() {
		return maxX;
	}
	public int GetMaxY() {
		return maxY;
	}
	public int GetMaxZ() {
		return maxZ;
	}
	
}