using UnityEngine;
using System.Collections;

public class ControlledSelfDestruct : MonoBehaviour {

	public GameObject deathParticleEffect;

	private GameObject fxObject;

	public float delay = 4;

	public float fxDelay = 2;

	public float objectHelath = 1;
	private bool destroyed = false;

	/// <summary>
	/// Damages the object. This method is called by the sword when it collides with an object tagged "Destructible".
	/// </summary>
	/// <para>If objectHelath reaches 0, initiates object's destruction.</para>
	/// <param name="damage">float corresponding to damage dealt.</param>
	public void DamageObject(float damage){
		objectHelath -= damage;
		if (objectHelath <= 0 && !destroyed) {
			destroyed = true;
			InitiateRemove();
		}
	}

	/// <summary>
	/// Initiates the removal of object by invoking functions after a certain delay.
	/// </summary>
	public void InitiateRemove () {
		Invoke ("SelfRemove", delay);
		Invoke ("SpawnFX", fxDelay);
	}

	/// <summary>
	/// Sets the FX object's parent to null so that the FX isn't removed, then removes the object.
	/// </summary>
	void SelfRemove(){
		fxObject.transform.parent = null;
		Destroy (gameObject);
	}

	/// <summary>
	/// Spawns the destruction FX at the centre of the object's mesh.
	/// </summary>
	void SpawnFX(){
		Mesh objectMesh = GetComponent<Mesh>();
		Collider objectCollider = GetComponent<Collider> ();
		fxObject = (GameObject) Instantiate (deathParticleEffect, transform.position, Quaternion.Euler (0, 0, 0));
		fxObject.transform.parent = gameObject.transform;
		fxObject.transform.position = objectCollider.bounds.center;	// Makes the particle appear from the centre of object's mesh, reguradless of object's centre node position
	}
}
