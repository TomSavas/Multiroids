using System.IO.Compression;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Movement : MonoBehaviour
{
	private float _speed = 100;
	private float _rotationSpeed = 7;
	private float _squareTerminalVelocity = 10;
	private bool _controlable;
	private Rigidbody2D _rigidbody2D;
	private ParticleSystem _particleSystem;

	private void Start()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_particleSystem = GetComponent<ParticleSystem>();
		_particleSystem.enableEmission = false;
	}

	private void Update() 
	{
		if (!_controlable)
			return;
	
		if (Input.GetKey(KeyCode.W))
		{
			_rigidbody2D.AddForce(transform.up * _speed * Time.deltaTime);

			if (_rigidbody2D.velocity.sqrMagnitude > _squareTerminalVelocity)
			{
				_rigidbody2D.velocity = _rigidbody2D.velocity.normalized * Mathf.Sqrt(_squareTerminalVelocity);
			}

			_particleSystem.enableEmission = true;
		}
		else
		{
			_particleSystem.enableEmission = false;
		}

		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(new Vector3(0, 0, _rotationSpeed));
		}
		
		if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(new Vector3(0, 0, -_rotationSpeed));
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
}
