using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeatherControl : MonoBehaviour
{
	// Noise generation
	public int noiseWidth;
	public int noiseHeight;
	public Vector2 noiseOrigin;
	public float noiseScale = 1.0F;

	private Texture2D noiseTex;
	private Color[] noisePix;
	private Renderer noiseRenderer;

	void Awake()
	{
		noiseRenderer = GetComponent<Renderer>();
		noiseTex = new Texture2D(noiseWidth, noiseHeight);
		noisePix = new Color[noiseTex.width * noiseTex.height];
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

	public Color SampleTex(Vector3Int location)
	{
		if (noisePix == null)
			return new Color(0.0f, 0.0f, 0.0f); // May not be initialized in editor

		return noisePix[location.y * noiseTex.width + location.x];
	}

	public void Step()
	{
		// TODO: Smoother stepping?
		noiseOrigin.x += 1;
		noiseOrigin.y += 1;
		ComputeNoise();
	}
}
