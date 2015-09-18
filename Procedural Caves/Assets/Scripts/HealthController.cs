using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	public float objectHealthMax;
	public float objectHealth;
	
	// Use this for initialization
	void Start () {
		objectHealth = objectHealthMax;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Damages the object. This method is called by the sword when it collides with an object tagged "Destructible".
	/// </summary>
	/// <para>If objectHelath reaches 0, initiates object's destruction.</para>
	/// <param name="damage">float corresponding to damage dealt.</param>
	public void TakeDamage(float damage){
		objectHealth -= damage;
	}
}
