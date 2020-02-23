using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Weather { Clear, Cloudy, Stormy };

[CreateAssetMenu]
public class WeatheredTile : TileBase
{
	public Sprite m_editor;
	public Sprite m_clear;
	public Sprite m_cloudy;
	public Sprite m_stormy;
	public Sprite m_lightning;
	public GameObject m_electrifyPrefab;
	public GameObject m_LightningStrike;
	static private Dictionary<Vector3, GameObject> m_weatherEffect = new Dictionary<Vector3, GameObject>();

	static public void ClearWeatherEffect()
	{
		foreach (var weatherEffect in m_weatherEffect.Values)
		{
			DestroyImmediate(weatherEffect);
		}
		m_weatherEffect.Clear();
	}

	public override void RefreshTile(Vector3Int location, ITilemap tilemap)
	{
		tilemap.RefreshTile(location);
	}

	public override void GetTileData(Vector3Int location, ITilemap itilemap, ref TileData tileData)
	{
		if (Application.isPlaying == false)
		{
			tileData.sprite = m_editor;
			return;
		}

		switch (GetForecast(location, itilemap))
		{
			case Weather.Cloudy:
				tileData.sprite = m_cloudy;
				break;
			case Weather.Stormy:
				tileData.sprite = m_stormy;
				break;
			default: // Default to clear
			case Weather.Clear:
				tileData.sprite = m_clear;
				break;
		}

		if (IsStruckByLightning(location, itilemap))
		{
			if ( !m_weatherEffect.ContainsKey(location) )
			{
				var tilemap = itilemap.GetComponent<Tilemap>();
				var world = tilemap.CellToWorld(location);

				// Spawn the electrified effect on top of the tile
				world.z = -0.1f;
				m_weatherEffect.Add(location, Instantiate(m_electrifyPrefab, world, Quaternion.identity));

				//// Spawn the lightningStrike
				//world.z = -0.2f;
				//Instantiate(m_LightningStrike, world, Quaternion.identity);
			}
		}
	}

	private int MapRange(int value, int fromLow, int fromHigh, int toLow, int toHigh)
	{
		return toLow + ((toHigh - toLow) / (fromHigh - fromLow)) * (value - fromLow);
	}

	private WeatherControl GetWeatherFromTilemap(ITilemap tilemap)
	{
		return tilemap.GetComponent<Tilemap>()
			.transform.parent.gameObject
			.GetComponentInChildren<WeatherControl>();
	}

	private Nullable<Vector3Int> CellToNoise(Vector3Int location, ITilemap tilemap)
	{
		if (name == "WOutsideTile" || name == "WCasernTile")
			return null; // These tiles are reserved (not really part of the board)

		var weather = GetWeatherFromTilemap(tilemap);
		if (weather == null)
			return null; // No weather control instance while in editor

		if (!weather.IsForecasting())
			return null; // No weather display during setup phase

		// Convert from tilemap to noise map coordinates
		var bounds = tilemap.cellBounds;
		var converted = new Vector3Int();
		converted.x = MapRange(location.x, bounds.min.x, bounds.max.x, 0, weather.noiseWidth);
		converted.y = MapRange(location.y, bounds.min.y, bounds.max.y, 0, weather.noiseHeight);
		return converted;
	}

	private bool IsStruckByLightning(Vector3Int location, ITilemap tilemap)
	{
		var noiseLocation = CellToNoise(location, tilemap);
		if (noiseLocation.HasValue)
		{
			var weather = GetWeatherFromTilemap(tilemap);
			return weather.IsStruckByLightning(noiseLocation.Value);
		}
		return false;
	}

	private Weather GetForecast(Vector3Int location, ITilemap tilemap)
	{
		var noiseLocation = CellToNoise(location, tilemap);
		if (noiseLocation.HasValue)
		{
			var weather = GetWeatherFromTilemap(tilemap);
			return weather.GetForecast(noiseLocation.Value);
		}
		return Weather.Clear;
	}
}
