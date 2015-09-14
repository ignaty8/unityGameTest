//using UnityEngine;
//using System.Collections;
//using Leap;
//
//public class HandPinch : MonoBehaviour
//{
//
//
//	void UpdatePinch (Frame frame)
//	{
//
//		bool trigger_pinch = false;
//
//		Hand hand = frame.Hands [handIndex];
//
//			
//			
//		// Thumb tip is the pinch position.
//				
//		Vector3 thumb_tip = hand.Fingers [0].TipPosition.ToUnityScaled ();
//
//			
//			
//		// Check thumb tip distance to joints on all other fingers.
//				
//		// If it's close enough, start pinching.
//				
//		for (int i = 1; i < NUM_FINGERS && !trigger_pinch; ++i) {
//
//			Finger finger = hand.Fingers [i];
//		
//				
//				
//			for (int j = 0; j < NUM_JOINTS && !trigger_pinch; ++j) {
//
//				Vector3 joint_position = finger.JointPosition ((Finger.FingerJoint)(j)).ToUnityScaled ();
//
//				Vector3 distance = thumb_tip - joint_position;
//
//				if (distance.magnitude < THUMB_TRIGGER_DISTANCE)
//						
//					trigger_pinch = true;
//
//			}
//
//		}
//
//			
//			
//		// Only change state if it's different.
//				
//		if (trigger_pinch && !pinching_)
//					
//			OnPinch (pinch_position);
//
//	}
//
//}
