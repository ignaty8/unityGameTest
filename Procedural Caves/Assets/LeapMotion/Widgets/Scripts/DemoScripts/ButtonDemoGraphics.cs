using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ButtonDemoGraphics : MonoBehaviour 
{
	public void SetActive(bool status)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		Text[] texts = GetComponentsInChildren<Text>();
		Image[] GUIimages = GetComponentsInChildren<Image>();
		foreach (Renderer renderer in renderers)
		{
			renderer.enabled = status;
		}
		foreach(Text text in texts){
			text.enabled = status;
		}
		foreach(Image image in GUIimages){
			image.enabled = status;
		}

		// User added
		Collider[] colliders = GetComponentsInChildren<Collider> ();
		foreach (Collider collider in colliders) {
			collider.enabled = status;
		}

		if (status) {
//			GameObject[] onGraphicsOfChildren = GameObject.FindGameObjectsWithTag ("OnGraphics");
//			foreach (GameObject candidateObject in onGraphicsOfChildren) {
//				if (!candidateObject.transform.parent.gameObject.GetComponent<Renderer>().enabled){
//
//				}
//			}

			foreach (Transform childTransform in transform){
				foreach (Transform child2Transform in childTransform){
					foreach (Transform child3Transform in child2Transform){
				Debug.Log (childTransform.name);
				if(child3Transform.tag == "OnGraphics"){
					
					//Debug.Log ("Success");
					Renderer[] childRenderers = child3Transform.GetComponentsInChildren<Renderer>();
					Text[] childTexts = child3Transform.GetComponentsInChildren<Text>();
					Image[] childGUIimages = child3Transform.GetComponentsInChildren<Image>();
					foreach (Renderer renderer in childRenderers){
						renderer.enabled = false;
					}
					foreach(Text text in childTexts){
						text.enabled = false;
					}
					foreach(Image image in childGUIimages){
						image.enabled = false;
					}
				}
					}
				}
			}
		}
	}
	
	public void SetColor(Color color)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		Text[] texts = GetComponentsInChildren<Text>();
		Image[] GUIimages = GetComponentsInChildren<Image>();
		foreach (Renderer renderer in renderers)
		{
			renderer.material.color = color;
		}
		foreach (Text text in texts){
			text.color = color;
		}
		foreach(Image image in GUIimages){
			image.color = color;
		}
	}
}
