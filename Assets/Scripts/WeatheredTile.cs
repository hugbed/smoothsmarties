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

	public override void RefreshTile(Vector3Int location, ITilemap tilemap)
	{
		tilemap.RefreshTile(location);
	}

	public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
	{
		if (Application.isPlaying == false)
		{
			tileData.sprite = m_editor;
			return;
		}

		switch (GetWeather(location, tilemap))
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

		// TODO: Switch to effect instead of changing sprite
		if (IsStruckByLightning(location, tilemap))
		{
			tileData.sprite = m_lightning;
		}
	}

	private int MapRange(int value, int fromLow, int fromHigh, int toLow, int toHigh)
	{
		return toLow + ((toHigh - toLow) / (fromHigh - fromLow)) * (value - fromLow);
	}

	private Nullable<Vector3Int> CellToNoise(Vector3Int location, ITilemap tilemap)
	{
		if (name == "WOutsideTile" || name == "WCasernTile")
			return null; // Y fait tout le temps beau sul bord pis a caserne

		var weather = tilemap.GetComponent<Tilemap>()
				.transform.parent.gameObject
				.GetComponentInChildren<WeatherControl>();

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
			var weather = tilemap.GetComponent<Tilemap>()
				.transform.parent.gameObject
				.GetComponentInChildren<WeatherControl>();
			return weather.IsStruckByLightning(noiseLocation.Value);
		}
		return false;
	}

	private Weather GetWeather(Vector3Int location, ITilemap tilemap)
	{
		var noiseLocation = CellToNoise(location, tilemap);
		if (noiseLocation.HasValue)
		{
			var weather = tilemap.GetComponent<Tilemap>()
				.transform.parent.gameObject
				.GetComponentInChildren<WeatherControl>();
			return weather.GetWeather(noiseLocation.Value);
		}
		return Weather.Clear;
	}
}
