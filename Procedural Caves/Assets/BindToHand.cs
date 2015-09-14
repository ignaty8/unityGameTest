using UnityEngine;
using System.Collections;
using Leap;

public class BindToHand : MonoBehaviour {

	
	Controller controller = new Controller ();
	GameObject handController, mainCamera;

	public float movementScaleX, movementScaleY, movementScaleZ, offsetX, offsetY, offsetZ;
	
	// Use this for initialization
	void Start () {
		handController = GameObject.FindGameObjectWithTag ("HandController");
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {


		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		foreach (Hand hand in hands) {
			if(hand.IsValid){
				if(hand.IsRight){
					MoveToHand(hand.Id,frame);
				}
			}
		}
	}

	void MoveToHand(int handID, Frame frame){
		Hand targetHand = frame.Hand (handID);

		//HandController handControllerScript = handController.GetComponent<HandController> ();
		//handControllerScript.

		Vector3 handPosition = targetHand.PalmPosition.ToUnityScaled ();
		handPosition = new Vector3 (handPosition.x * movementScaleX + offsetX, handPosition.y * movementScaleY + offsetY, handPosition.z * movementScaleZ + offsetZ);
			
		//float distance = handPosition.magnitude;
		//Vector3 distanceVector = new Vector3(0, 0, -distance);



		//
		Vector3 handDirection = targetHand.Direction.ToUnity();
		transform.localRotation = Quaternion.LookRotation(handDirection, Vector3.Cross (targetHand.PalmNormal.ToUnity(), handDirection))// * mainCamera.transform.rotation;
			;//

		//transform.localRotation = transform.rotation * handController.transform.rotation; 

		//Vector3 relativePosition = handController.transform.rotation * distanceVector + handController.transform.position;



		transform.localPosition = handPosition// + handController.transform.position;
		                                           ;
	}
}
