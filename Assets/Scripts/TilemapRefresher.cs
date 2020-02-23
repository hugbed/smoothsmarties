using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRefresher : MonoBehaviour
{
	private Tilemap tilemap;
	void Start()
	{
		tilemap = GetComponent<Tilemap>();
		tilemap.RefreshAllTiles();
	}

	public void Step()
	{
		tilemap.RefreshAllTiles();
	}
}
