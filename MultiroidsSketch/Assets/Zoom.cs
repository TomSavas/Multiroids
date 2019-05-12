using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
	private float _zoomSpeed = 5;
	
	private void Update ()
	{
		var potentialSize = Camera.main.orthographicSize - Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;

		if (potentialSize > 5 && potentialSize < 15)
		{
			Camera.main.orthographicSize = potentialSize;
		}
	}
}
