using UnityEngine;
using System.Collections.Generic;

public class MultiTouchMenu : MonoBehaviour {
	public struct fingerPair { // stores pair of fingerIds
		public float finger1;
		public float finger2;
	}

	private Dictionary<GameObject, List<float>> beingTouched = new Dictionary<GameObject, List<float>>(); // maps a GameObject that is being touched to a list of fingerIds associated with it
	private Dictionary<float, Vector3> offsets = new Dictionary<float, Vector3>(); // maps a fingerId to its offset from the object it is associated with
	private List<GameObject> alreadyExpanded = new List<GameObject>(); // records each GameObject that has already been expanded to display its submenus
	private Dictionary<float, TouchPhase> touchPhases = new Dictionary<float, TouchPhase>(); // maps each existing fingerId to its current touch phase -- updated every frame
	private Dictionary<fingerPair, float> fingerDistances = new Dictionary<fingerPair, float>(); // maps a finger pair to the distance between them

	private const float originalY = 0f; // all objects should have the same original y-coordinate
	private const float yScale = 0.0005f; // all objects should have the same y-scale
	private const int numSubmenus = 6;

	void Update() {
		updateDictionary();
		updateMovement();
	}

	void updateDictionary() {
		// Remove old touches 
		List<GameObject> touchedObjects = new List<GameObject>();
		foreach (GameObject key in beingTouched.Keys) {
			touchedObjects.Add (key);
		}
		for (int i = 0; i < touchedObjects.Count; i++) { // essentially looping through beingTouched
			GameObject button = touchedObjects[i];
			deleteOldEntries(button);
			if (beingTouched[button].Count == 0) { // if no associated fingerIds remaining
				beingTouched.Remove (button);
			}
		}
		// Add any new touches
		List<Touch> newTouches = new List<Touch>();
		foreach (Touch touch in InputProxy.touches) {
			if (touch.phase == TouchPhase.Began) {
				newTouches.Add (touch);
			}
			// also update touch phases here 
			float ID = touch.fingerId; 
			touchPhases[ID] = touch.phase;
		}
		for (int i = 0; i < newTouches.Count; i++) {
			Touch currentTouch = newTouches[i];
			int layerToHit = 1 << 8; // only want to touch objects that are put in this layer
			Ray touchRay = camera.ScreenPointToRay(currentTouch.position);
			Debug.DrawRay(touchRay.origin, touchRay.direction * 200, Color.yellow); // figure out what camera.ScreenPointToRay does
			RaycastHit hitCheck;
			bool hitTrue = Physics.Raycast(touchRay, out hitCheck, 60f, layerToHit);
			GameObject hitObject = null;
			if (hitTrue) { // if you hit an object
				hitObject = hitCheck.transform.gameObject;
				if (!beingTouched.ContainsKey (hitObject)) {
					List<float> fingerIds = new List<float>();
					beingTouched.Add (hitObject, fingerIds);
				}
				beingTouched[hitObject].Add (currentTouch.fingerId);
				float distanceFromCamera = camera.transform.position.y - originalY;
				Vector3 touchPosition = touchedPosition(touchRay, camera.transform, distanceFromCamera); // calculate touch position
				Vector3 offset = hitObject.transform.position - touchPosition; // calculate offset
				offsets.Add (currentTouch.fingerId, offset);
			}
		}
	}

	void updateMovement() {
		foreach (GameObject button in beingTouched.Keys) {
			int began = 0;
			int ended = 0;
			int moved = 0;
			int stationary = 0;
			foreach (float fingerID in beingTouched[button]) {
				if (touchPhases[fingerID] == TouchPhase.Began) {
					began++;
				} else if (touchPhases[fingerID] == TouchPhase.Ended) {
					ended++;
				} else if (touchPhases[fingerID] == TouchPhase.Moved) {
					moved++;
				} else if (touchPhases[fingerID] == TouchPhase.Stationary) {
					stationary++;
				}
			}
			int numTouches = calculateTouches (button);
			// START GOING THROUGH CASES:

			// WHENEVER NUMTOUCHES = 2, RECALCULATE THE DISTANCE BETWEEN THOSE TWO AND STORE WITH GAMEOBJECT
//			if (numTouches == 2) {
//				fingerPair pair = new fingerPair();
//				pair.finger1 = 0f;
//				pair.finger2 = 0f;
//				Vector3 position1 = new Vector3();
//				Vector3 position2 = new Vector3();
//				foreach (Touch currentTouch in InputProxy.touches) {
//					if (beingTouched[button].Contains (currentTouch.fingerId)) {
//						int layerToHit = 1 << 8; // only want to touch objects that are put in this layer
//						Ray touchRay = camera.ScreenPointToRay(currentTouch.position);
//						Debug.DrawRay(touchRay.origin, touchRay.direction * 200, Color.yellow); // figure out what camera.ScreenPointToRay does
//						RaycastHit hitCheck;
//						bool hitTrue = Physics.Raycast(touchRay, out hitCheck, 60f, layerToHit);
//						GameObject hitObject = null;
//						if (hitTrue) {
//							hitObject = hitCheck.transform.gameObject;
//							if (hitObject == button) {
//								if (pair.finger1 == 0f) {
//									pair.finger1 = currentTouch.fingerId;
//									position1 = currentTouch.position;
//								} else {
//									pair.finger2 = currentTouch.fingerId;
//									position2 = currentTouch.position;
//								}
//							}
//						}
//					}
//				}
//				Vector3 difference = position2 - position1;
//				float distance = Vector3.magnitude(difference);
//				if (!fingerDistances.ContainsKey(pair)){
//					fingerDistances.Add(pair, distance);
//				} else {
//				}
//
//			} 
			if (beingTouched[button].Count == 1 && moved == 1) {
				foreach (Touch currentTouch in InputProxy.touches) {
					if (currentTouch.fingerId == beingTouched[button][0]) {
						float distanceFromCamera = camera.transform.position.y - originalY;
						Ray touchRay = camera.ScreenPointToRay(currentTouch.position);
						Vector3 touchPosition = touchedPosition(touchRay, camera.transform, distanceFromCamera);
						button.transform.position = touchPosition + offsets[currentTouch.fingerId]; 
					}
				}
			} else if  (beingTouched[button].Count == 3) {
				if (numTouches == 1 && began == 0) { // EXPAND
					if (!alreadyExpanded.Contains (button)) {
						generateButtons(button.transform);
						alreadyExpanded.Add (button);
					}
				}
				if (began == 1 && numTouches != 3) {
					beingTouched[button].Clear ();
					Debug.Log("happening");
				}
			}
			if (beingTouched[button].Count > 1 && numTouches == 0) {
				beingTouched[button].Clear ();
			} 
		}
	}

	void generateButtons(Transform hitObjectTransform) {
		float cylinderScale = hitObjectTransform.localScale.x;
		float newDiameter = 1.3f * cylinderScale;

		// update GUI labels positions

		// get angle and constant for number of submenus
		float theta = (2 * Mathf.PI) / numSubmenus;

		// general case 
		for (int i = 1; i <= numSubmenus; i++) {
			GameObject newButton = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			newButton.name = "Submenu " + i;
			newButton.transform.localScale = new Vector3(cylinderScale, originalY, cylinderScale);
			newButton.transform.position = hitObjectTransform.position + new Vector3(newDiameter * Mathf.Cos(theta * i), 0 , newDiameter * Mathf.Sin(theta * i));
			newButton.renderer.material.color = Color.black;
			newButton.layer = 8;

			GameObject buttonLabel = new GameObject ("label");
			buttonLabel.AddComponent("GUIText");
			buttonLabel.guiText.text = "Submenu";
			Vector3 viewPos = camera.WorldToViewportPoint(newButton.transform.position);
			buttonLabel.guiText.transform.position = viewPos;
		}

//		// in the x direction
//		GameObject newButton = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
//		newButton.name = "Submenu";
//
//		newButton.transform.position = hitObjectTransform.position + new Vector3(radius * 2, 0, 0);
//		newButton.renderer.material.color = Color.black;
//		newButton.layer = 8;
//
//		// in the z direction 
//		GameObject secondButton = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
//		secondButton.name = "Submenu";
//		secondButton.transform.localScale = new Vector3(cylinderScale / 2.5f, originalY, cylinderScale / 2.5f);
//		secondButton.transform.position = hitObjectTransform.position + new Vector3(0, 0, radius * 2);
//		secondButton.renderer.material.color = Color.black;
//		secondButton.layer = 8;
	}

	int calculateTouches(GameObject button) {
		int numTouches = 0;
		foreach (Touch currentTouch in InputProxy.touches) {
			if (beingTouched[button].Contains (currentTouch.fingerId)) {
				int layerToHit = 1 << 8; // only want to touch objects that are put in this layer
				Ray touchRay = camera.ScreenPointToRay(currentTouch.position);
				Debug.DrawRay(touchRay.origin, touchRay.direction * 200, Color.yellow); // figure out what camera.ScreenPointToRay does
				RaycastHit hitCheck;
				bool hitTrue = Physics.Raycast(touchRay, out hitCheck, 60f, layerToHit);
				GameObject hitObject = null;
				if (hitTrue) {
					hitObject = hitCheck.transform.gameObject;
					if (hitObject == button) {
						numTouches++;
					}
				}
			}
		}
		return numTouches;
	}

	void deleteOldEntries(GameObject button) {
		List<float> associatedFingers = beingTouched[button];
		for (int j = 0; j < associatedFingers.Count; j++) {
			float finger = associatedFingers[j];
			Touch currentTouch = new Touch ();
			bool stillExists = false;
			foreach (Touch touch in InputProxy.touches) {
				if (touch.fingerId == finger) {
					stillExists = true;
					break;
				}
			} 
			if (!stillExists) {
				beingTouched[button].Remove(finger); // update beingTouched dictionary
				offsets.Remove(finger); // update offsets dictionary as well
			}
		}
	}

//	void deleteOldEntries(GameObject button) {
//		List<float> associatedFingers = beingTouched[button];
//		for (int j = 0; j < associatedFingers.Count; j++) {
//			float finger = associatedFingers[j];
//			Touch currentTouch = new Touch ();
//			foreach (Touch touch in InputProxy.touches) {
//				if (touch.fingerId == finger) {
//					currentTouch = touch;
//					break;
//				}
//			} 
//			if (currentTouch.phase == TouchPhase.Ended) {
//				beingTouched[button].Remove(finger); // update beingTouched dictionary
//				offsets.Remove(finger); // update offsets dictionary as well
//			}
//		}
//	}


		 
	Vector3 touchedPosition(Ray touchRay, Transform cameraTransform, float distFromOrigin) { // copied from Thai
		float angle;
		angle = Vector3.Angle(cameraTransform.forward, touchRay.direction);
		float hypotenuse;
		hypotenuse = distFromOrigin / Mathf.Cos(angle * Mathf.Deg2Rad);
		return cameraTransform.position + touchRay.direction * hypotenuse;
	}
}






