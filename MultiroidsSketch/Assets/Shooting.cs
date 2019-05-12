using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shooting : MonoBehaviour
{
	public GameObject BulletPrefab;
	public ShipSelector ShipSelector;

	private float _speed = 15000;
	private float _cooldown = 0.5f;
	private float _lastShotTime;
	private bool _controlable;

	private void Update()
	{
		if (!_controlable)
			return;

		if (_lastShotTime + _cooldown < Time.time && Input.GetKey(KeyCode.Space))
		{
			_lastShotTime = Time.time;
			var bullet = Instantiate(BulletPrefab, transform.position + transform.up * 0.5f, transform.rotation);
			bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * _speed * Time.deltaTime);
			
			Destroy(bullet, 10);
		}
	}

	public void EnableControls()
	{
		_controlable = true;
	}
	
	public void DisableControls()
	{
		_controlable = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Bullet"))
		{
			Destroy(other.gameObject);

			if (ShipSelector.SelectedShips.Contains(gameObject))
				ShipSelector.SelectedShips.Remove(gameObject);
			Destroy(gameObject);
		}
	}
}
