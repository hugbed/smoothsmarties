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

	public WeatherControl m_weatherControl;

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
			default:
			case Weather.Clear:
				tileData.sprite = m_clear;
				break;
		}
	}

	private Weather GetWeather(Vector3Int location, ITilemap tilemap)
	{
		// TODO: Implement me.
		return Weather.Clear;
	}
}
