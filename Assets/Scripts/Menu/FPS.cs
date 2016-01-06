using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour
{
	[SerializeField] UILabel uiLabel;
	[SerializeField] float updateInterval = 0.5f;
	float accum = 0f; // FPS accumulated over the interval
	int frames = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval

	void Start()
	{
		if (!GetComponent<UILabel>())
		{
			print("Requires NGUI UILabel component!");
			enabled = false;
			return;
		}

		timeleft = updateInterval;  
	}

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0f)
		{
			// display two fractional digits (f2 format)
			uiLabel.text = "FPS: " + (accum/frames).ToString("f2");
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
