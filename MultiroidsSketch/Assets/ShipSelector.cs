using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ShipSelector : MonoBehaviour
{
	public List<GameObject> SelectedShips;

	public Dictionary<KeyCode, List<GameObject>> ControlGroups;
	
	private Vector2 _selectionEndPoint;
	private Vector2 _selectionStartPoint;
	public List<GameObject> _highlightedShips;
	private KeyCode[] ControlGroupKeys =
	{
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
		KeyCode.Alpha0
	};

	private LineRenderer _lineRenderer;
	private BoxCollider2D _selectionCollider;

	private void Start ()
	{
		SelectedShips = new List<GameObject>();
		ControlGroups = new Dictionary<KeyCode, List<GameObject>>();
		_highlightedShips = new List<GameObject>();
		_lineRenderer = GetComponent<LineRenderer>();
		_selectionCollider = GetComponent<BoxCollider2D>();
		
		_lineRenderer.positionCount = 4;
		_lineRenderer.startWidth = 0.05f;
		_lineRenderer.endWidth = 0.05f;
		_lineRenderer.startColor = Color.green;
		_lineRenderer.endColor = Color.green;
	}
	
	private void Update ()
	{
		if(!SetControlGroup())
            SelectControlGroup();	
		
		if (Input.GetMouseButtonDown(0))
		{
            ClearShips();
			
			_lineRenderer.enabled = true;
			_selectionCollider.enabled = true;
			_selectionStartPoint = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
			_lineRenderer.SetPosition(0, _selectionStartPoint);
		}

		if (Input.GetMouseButton(0))
		{
			_selectionEndPoint = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
			_lineRenderer.SetPosition(1, new Vector3(_selectionStartPoint.x, _selectionEndPoint.y, 0));
			_lineRenderer.SetPosition(2, _selectionEndPoint);
			_lineRenderer.SetPosition(3, new Vector3(_selectionEndPoint.x, _selectionStartPoint.y, 0));

			_selectionCollider.offset = _selectionStartPoint + (_selectionEndPoint - _selectionStartPoint) / 2;
			var size = _selectionEndPoint - _selectionStartPoint;
			_selectionCollider.size = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
		}
		else
		{
			SelectShips(_highlightedShips);
			
			_lineRenderer.enabled = false;
			_selectionCollider.enabled = false;

			foreach (var ship in SelectedShips)
			{
				ship.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			}
		}
	}
	
	private bool SetControlGroup()
	{
		if (Input.GetKey(KeyCode.LeftControl))
		{
			foreach (var key in ControlGroupKeys)
			{
				if (Input.GetKey(key))
				{
					Debug.Log("Setting new control group " + key);
					
					ControlGroups[key] = new List<GameObject>();
					
                    foreach (var ship in SelectedShips)
                    {
	                    ControlGroups[key].Add(ship);
                    }

					return true;
				}
			}
		}

		return false;
	}
	
	private bool SelectControlGroup()
	{
        foreach (var key in ControlGroupKeys)
        {
            if (Input.GetKey(key) && ControlGroups.ContainsKey(key))
            {
                Debug.Log("Selecting control group " + key);
	            
	            ClearShips();
                SelectShips(ControlGroups[key]);
                return true;
            }
        }

		return false;
	}

	private void ClearShips()
	{
		foreach (var ship in SelectedShips)
		{
            ship.GetComponent<Movement>().DisableControls(); 
            ship.GetComponent<Shooting>().DisableControls();
			ship.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
		}
		
		SelectedShips.Clear();
	}

	private void SelectShips(List<GameObject> ships)
	{
        foreach (var ship in ships)
        {
	        Debug.Log("Adding ship: " + ship.name);
	        ship.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            ship.GetComponent<Movement>().EnableControls(); 
            ship.GetComponent<Shooting>().EnableControls();
	        
            SelectedShips.Add(ship);
        }
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (!_highlightedShips.Contains(other.gameObject))
			{
				other.gameObject.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = true;
				_highlightedShips.Add(other.gameObject);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		other.gameObject.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = false;
		_highlightedShips.Remove(other.gameObject);
	}
}
