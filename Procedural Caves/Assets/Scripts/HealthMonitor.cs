using UnityEngine;
using System.Collections;

public class HealthMonitor : MonoBehaviour {

	public float parentObjectHealth;
	float parentHealthMax;
	float oldParentObjectHealth;
	int healthPercentage = 100;
	float displayedHealth = 100;
	public float uiUpdateSpeed;

	Texture uiTexture;

	GameObject parentObject;
	Renderer parentRenderer;
	HealthController parentHealthController;

	// Use this for initialization
	void Start () {
		//parentObject = gameObject.GetComponentInParent<GameObject> ();
		parentRenderer = GetComponentInParent<Renderer> ();
		parentHealthController = gameObject.GetComponentInParent<HealthController>();
		if (parentHealthController != null) {
			//parentObjectHealth = parentHealthController.objectHealth;
			parentHealthMax = parentHealthController.objectHealthMax;
			displayedHealth = 100f;
		}
		//HealthUpdater();
	}
	
	// Update is called once per frame
	void Update () {
		if (parentHealthController != null) {
			HealthUpdater ();
		}
	}

	void HealthUpdater(){
		oldParentObjectHealth = parentObjectHealth;
		parentObjectHealth = parentHealthController.objectHealth;

		if (healthPercentage < displayedHealth && !(displayedHealth<=0)) {
			float healthDisplayDifference = displayedHealth - healthPercentage;

			displayedHealth -= Time.deltaTime * uiUpdateSpeed * healthDisplayDifference;
			
			int intDisplayedHealth = Mathf.CeilToInt(displayedHealth);
			
			ChangeUITexture(intDisplayedHealth);
		}

		if (parentObjectHealth != oldParentObjectHealth) {	
			//Debug.Log (parentObjectHealth);
			float tmp = parentObjectHealth / parentHealthMax * 100f;
			//Debug.Log ("tmp" + tmp.ToString());
			healthPercentage = Mathf.CeilToInt (tmp);
			//Debug.Log (healthPercentage + "%");


			if (displayedHealth <= 0){
				ChangeUITexture(0);
			} else {
			}
		}
	}

	void ChangeUITexture(int textureNumber){
		textureNumber = (int)Mathf.Clamp (textureNumber, 0f, 100f);
//		if (textureNumber == 10) {
//			uiTexture = Resources.Load ("UI/Textures/Health/HealthBar0") as Texture;
//
//			parentRenderer.material.mainTexture = uiTexture;
//		} else {
			uiTexture = Resources.Load ("UI/Textures/Health/HealthBar" + textureNumber.ToString()) as Texture;
			
			parentRenderer.material.mainTexture = uiTexture;
//		}
	}
}
