using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ParticleSystem))]
public class UnpauseParticleSystem : MonoBehaviour 
{
	private ParticleSystem _particleSystem;
	
	public void Awake() {
		_particleSystem = gameObject.GetComponent<ParticleSystem>();
	}

	public void Update() {
		if (Time.timeScale == 0 ) {
			_particleSystem.Simulate(Time.unscaledDeltaTime,false,false);
			_particleSystem.Play();
		}
	}
}
