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
		var tilemap = itilemap.GetComponent<Tilemap>();
		var weatherControl = tilemap.transform.parent.gameObject
			.GetComponentInChildren<WeatherControl>();

		var bounds = tilemap.cellBounds;
		var convert = new Vector3Int();
		convert.x = Convert(location.x, bounds.min.x, bounds.max.x, 0, weatherControl.pixWidth);
		convert.y = Convert(location.y, bounds.min.y, bounds.max.y, 0, weatherControl.pixHeight);

		var color = weatherControl.SampleTex(convert);
		float value = color.r;

		// Determine the weather forecast
		Weather forecast = Weather.Clear;
		if (value > 0.9)
		{
			forecast = Weather.Stormy;
		}
		else if (value > 0.5)
		{
			forecast = Weather.Cloudy;
		}

		// Check if there is a special weather event
		if (forecast == Weather.Stormy)
		{
			if (Random.value <= 0.33)
			{
				return Weather.Lightning;
			}
		}

		return forecast;
	}
}
