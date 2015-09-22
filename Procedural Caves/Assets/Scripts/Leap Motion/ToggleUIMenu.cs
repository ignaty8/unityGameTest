using UnityEngine;
using System.Collections;
using Leap;

public class ToggleUIMenu : MonoBehaviour {

	Controller controller = new Controller();

	public float minSwipeVelocity = 200f;
	public float minSwipeLength = 750f;

	bool isMenuVisible = false;

	float timer;
	public float timeBetweenToggles = .8f;

	GameObject uiHolder;

	// Use this for initialization
	void Start () {
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);

		// Configure the gesture sensitivity.
		controller.Config.SetFloat ("Gesture.Swipe.MinLength", minSwipeLength);
		controller.Config.SetFloat ("Gesture.Swipe.MinVelocity", minSwipeVelocity);
		controller.Config.Save ();

		uiHolder = transform.FindChild ("UIHolder").gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (timer < Time.time) {
			CheckGesture ();
		}
	}

	void CheckGesture(){
		Frame frame = controller.Frame ();
		GestureList gestures = frame.Gestures();
		foreach (Gesture gesture in gestures) {
			if(gesture.Type == Gesture.GestureType.TYPE_SWIPE){
				SwipeGesture swipeGesture = new SwipeGesture (gesture);
				//Debug.Log ("Success");
				if (Mathf.Abs(swipeGesture.Direction.y) > 10 * Mathf.Abs (swipeGesture.Direction.z) && Mathf.Abs (swipeGesture.Direction.y) > 10 * Mathf.Abs (swipeGesture.Direction.x)){
					ToggleMenuState();
				}

			}
		}
	}

	void ToggleMenuState(){
		isMenuVisible = !isMenuVisible;
		Debug.Log ("MenuVisibility" + isMenuVisible.ToString ());
		timer = Time.time + timeBetweenToggles;
		uiHolder.SetActive(isMenuVisible);
	}
}
