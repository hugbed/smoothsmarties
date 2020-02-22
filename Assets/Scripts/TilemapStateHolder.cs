using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileState
{
	// public Vector3Int localPlace { get; set; }
	// public Vector3 worldLocation { get; set; }
	// public TileBase tileBase { get; set; }
	// public Tilemap tilemapMember { get; set; }

	// Below is needed for Breadth First Searching
	// public bool IsExplored { get; set; }
	// public WorldTile ExploredFrom { get; set; }
	// public int Cost { get; set; }
	public float weather { get; set; }
}

public class TilemapStateHolder : MonoBehaviour
{
	public Dictionary<Vector3Int, TileState> tiles;

	void Start()
	{
		var tilemap = gameObject.GetComponent<Tilemap>();
		if (tilemap != null)
		{
			BoundsInt bounds = tilemap.cellBounds;
			TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

			for (int x = 0; x < bounds.size.x; x++) {
				for (int y = 0; y < bounds.size.y; y++) {
					TileBase tile = allTiles[x + y * bounds.size.x];
					if (tile != null) {
						Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
					} else {
						Debug.Log("x:" + x + " y:" + y + " tile: (null)");
					}
				}
			} 

			// foreach (var pos in tilemap.cellBounds.allPositionsWithin)
			// {
			// 	var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
			// 	if (!tilemap.HasTile(localPlace)) continue;
			// 	var state = new TileState
			// 	{
			// 		localPlace = localPlace,
			// 		worldLocation = tilemap.CellToWorld(localPlace),
			// 		tileBase = tilemap.GetTile(localPlace),
			// 		tilemapMember = tilemap,
			// 		name = localPlace.x + "," + localPlace.y,
			// 	};
			// 	tiles.Add(state.worldLocation, state);
			// }
		}
	}

	void ForecastWeather()
	{
		// foreach (var tile in tiles)
		// {
		// 	float xCoord = xOrg + x / noiseTex.width * scale;
		// 	float yCoord = yOrg + y / noiseTex.height * scale;
		// 	float sample = Mathf.PerlinNoise(xCoord, yCoord);
			// tile.Key
			// tile.Value
		// }
		// TODO:
		//	- Roll forecast for each tile
		//	- Assign state for each tile, identifying striked zones
	}

	void AdvanceTurn()
	{
		ForecastWeather();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			AdvanceTurn();
		}
	}
}
