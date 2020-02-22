using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeatherControl : MonoBehaviour
{
	// Noise generation
	public int noiseWidth = 100;
	public int noiseHeight = 100;
	public Vector2 noiseOrigin;
	public float noiseScale = 1.0F;

	private Texture2D noiseTex;
	private Color[] noisePix;
	private Color[] previousNoisePix;
	private Renderer noiseRenderer;

	public Vector2 noiseStep = new Vector2(1.0F, 1.0F);

	// Random values for weather
	public float cloudyThresh;
	public float stormyThresh;
	public float lightningTresh;

	private bool forecasting = false;

	void Awake()
	{
		noiseRenderer = GetComponent<Renderer>();
		noiseTex = new Texture2D(noiseWidth, noiseHeight);
		noisePix = new Color[noiseTex.width * noiseTex.height];
		previousNoisePix = new Color[noiseTex.width * noiseTex.height];
		noiseRenderer.material.mainTexture = noiseTex;
		ComputeNoise(); // Compute initial noise texture
	}

	private void ComputeNoise()
	{
		// For each pixel in the texture...
		float y = 0.0F;
		while (y < noiseTex.height)
		{
			float x = 0.0F;
			while (x < noiseTex.width)
			{
				// Keep a snapshot of the pixel data
				var previousColor = noisePix[(int)y * noiseTex.width + (int)x];
				previousNoisePix[(int)y * noiseTex.width + (int)x] = new Color(previousColor.r, previousColor.g, previousColor.b);

				float xCoord = noiseOrigin.x + x / noiseTex.width * noiseScale;
				float yCoord = noiseOrigin.y + y / noiseTex.height * noiseScale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);
				noisePix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}

		// Copy the pixel data to the texture and load it into the GPU.
		noiseTex.SetPixels(noisePix);
		noiseTex.Apply();
	}

	Color SamplePixData(Vector3Int location)
	{
		if (previousNoisePix == null)
			return new Color(0.0f, 0.0f, 0.0f); // May not be initialized in editor

		return previousNoisePix[location.y * noiseTex.width + location.x];
	}

	public Weather GetWeather(Vector3Int location)
	{
		Weather forecast = Weather.Clear;
		float value = SamplePixData(location).r;

		if (value > lightningTresh)
		{
			forecast = Weather.Lightning;
		}
		else if (value > stormyThresh)
		{
			forecast = Weather.Stormy;
		}
		else if (value > cloudyThresh)
		{
			forecast = Weather.Cloudy;
		}

		return forecast;
	}

	public void Step()
	{
		noiseOrigin.x += noiseStep.x;
		noiseOrigin.y += noiseStep.y;
		ComputeNoise();
	}

	public void StartForecasting()
	{
		forecasting = true;
	}

	public bool IsForecasting()
	{
		return forecasting;
	}
}
