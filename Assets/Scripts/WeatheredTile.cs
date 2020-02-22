using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Weather { Clear, Cloudy, Stormy, Lightning };

[CreateAssetMenu]
public class WeatheredTile : TileBase
{
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
		switch (GetWeather(location, tilemap))
		{
			case Weather.Cloudy:
				tileData.sprite = m_cloudy;
				break;
			case Weather.Stormy:
				tileData.sprite = m_stormy;
				break;
			case Weather.Lightning:
				tileData.sprite = m_lightning;
				break;
			default: // Default to clear
			case Weather.Clear:
				tileData.sprite = m_clear;
				break;
		}
	}

	private int Convert(int input, int in_min, int in_max, int out_min, int out_max)
	{
		return out_min + ((out_max - out_min) / (in_max - in_min)) * (input - in_min);
	}

	private Weather GetWeather(Vector3Int location, ITilemap itilemap)
	{
		if (name == "WOutsideTile" || name == "WCasernTile")
			return Weather.Clear; // Y fait tout le temps beau sul bord pis a caserne

		var tilemap = itilemap.GetComponent<Tilemap>();
		var weather = tilemap.transform.parent.gameObject
			.GetComponentInChildren<WeatherControl>();

		if (weather == null)
			return Weather.Clear; // No weather control instance while in editor

		if (!weather.IsForecasting())
			return Weather.Clear; // No weather display during setup phase

		// Convert from tilemap to noise map coordinates
		var bounds = tilemap.cellBounds;
		var converted = new Vector3Int();
		converted.x = Convert(location.x, bounds.min.x, bounds.max.x, 0, weather.noiseWidth);
		converted.y = Convert(location.y, bounds.min.y, bounds.max.y, 0, weather.noiseHeight);

		return weather.GetWeather(converted);
	}
}
