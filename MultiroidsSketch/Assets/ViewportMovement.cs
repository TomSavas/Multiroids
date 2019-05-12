using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ViewportMovement : MonoBehaviour
{
	public ShipSelector ShipSelector;
	
	private float _speed = 10;
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (ShipSelector == null || ShipSelector.SelectedShips == null || ShipSelector.SelectedShips.Count == 0)
				return;
			
			var avgPos = Vector3.zero;

			foreach (var ship in ShipSelector.SelectedShips)
			{
				avgPos += ship.gameObject.transform.position;
			}

			avgPos /= ShipSelector.SelectedShips.Count;
			avgPos.z = Camera.main.gameObject.transform.position.z;
			
			Camera.main.gameObject.transform.position = avgPos;
		}
		else
		{
            var viewportMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            if(viewportMousePosition.x < 0.025)
                Camera.main.transform.position += new Vector3(-_speed * Time.deltaTime, 0, 0);
                
            if(viewportMousePosition.x > 0.975)
                Camera.main.transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
            
            if(viewportMousePosition.y < 0.025)
                Camera.main.transform.position += new Vector3(0, -_speed * Time.deltaTime, 0);
                
            if(viewportMousePosition.y > 0.975)
                Camera.main.transform.position += new Vector3(0, _speed * Time.deltaTime, 0);
			
		}
	}
}
