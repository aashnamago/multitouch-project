using UnityEngine;
using System.Collections.Generic;

public class OldMenuCode : MonoBehaviour {
	private Dictionary<GameObject, List<float>> beingTouched = new Dictionary<GameObject, List<float>>(); // maps a GameObject that is being touched to a list of fingerIds (fingers touching it)
	private Dictionary<GameObject, float> originalY = new Dictionary<GameObject, float>(); // maps a GameObject to its original y-coordinates
	private Dictionary<GameObject, Vector3> offsets = new Dictionary<GameObject, Vector3>(); // maps a GameObject to its offset from the fingers moving it (right now, only moves if one finger touching it) 
	
	private Dictionary<GameObject, List<List<float>>> touchHistory = new Dictionary<GameObject, List<List<float>>>(); // keeps track of the previous and current list of fingerIds associated with a given GameObject


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	
	
	
	
	// Delete old entries and add new entries based on finger positions in the new frame
	void updateDictionary() {
	}
	
	
	// Delete fingerIds from lists in beingTouched if they are no longer touching the associated object 
	void deleteOldEntries() {
//		List<GameObject> keys = new List<GameObject>();
//		foreach (GameObject key in beingTouched.Keys) {
//			keys.Add (key);
//		}
//		
//		for (int i = 0; i < keys.Count; i++) {
//			GameObject currentKey = keys[i];
//			List<float> fingerList = beingTouched[currentKey];
//			bool stillTouched = false;
//			for (int j = 0; j < fingerList.Count; j++) {
//				if (InputProxy.touches.Contains(fingerList[j])) {
//					//						for (int k = 0; k < InputProxy.touches.Length; k++) {
//					//							if (beingTouched[currentKey].Contains(InputProxy.touches[k].fingerId)) {
//					//								stillTouched = true;
//					//							}
//					//						}
//				}
//			}
//			
//			
//			
//		}
		
		// if no longer touching the associated object, remove the fingerId from value position of beingTouched
		// at the end, delete any objects that have no touches associated
	} 
	
	
	
	// Add any GameObjects that are now being touched or any new fingers that are now touching a GameObject to beingTouched
	void addNewEntries() {
	}
	
	// Move current touch list to become the previous list and update the current touch list 
	void updateTouchHistory() {
		// initialize with empty lists
		// current list becomes previous list
		// beingTouched[key] becomes new current list 
	}
	
	
	// Perform actions on GameObjects that are being touched based on previous and current associated fingerIds
	void updateMovements() {
	}

	
	
	
	Vector3 touchedPosition(Ray touchRay, Transform cameraTransform, float distFromOrigin) { // copied from Thai
		float angle;
		angle = Vector3.Angle(cameraTransform.forward, touchRay.direction);
		float hypotenuse;
		hypotenuse = distFromOrigin / Mathf.Cos(angle * Mathf.Deg2Rad);
		return cameraTransform.position + touchRay.direction * hypotenuse;
	}
	
	//	void Update() {
	//		updateDictionary(); // check which objects are still being touched 
	//		contactObject();
	//	}
	
	//	void contactObject() {
	//		foreach (Touch currentTouch in InputProxy.touches) { 
	//			int layerToHit = 1 << 8; // only want to touch objects that are put in this layer
	//			Ray touchRay = camera.ScreenPointToRay(currentTouch.position);
	//			Debug.DrawRay(touchRay.origin, touchRay.direction * 200, Color.yellow); // figure out what camera.ScreenPointToRay does
	//			RaycastHit hitCheck;
	//			bool hitTrue = Physics.Raycast(touchRay, out hitCheck, 60f, layerToHit);
	//			GameObject hitObject = null;
	//			if (hitTrue) {
	//				performAction(hitObject, hitCheck, touchRay, currentTouch, camera);
	//			}
	//		}
	//	}
	//
	//	// FIX THIS METHOD 
	//	void performAction(GameObject hitObject, RaycastHit hitCheck, Ray touchRay, Touch currentTouch, Camera camera) {
	//		hitObject = hitCheck.transform.gameObject;
	//		if (hitObject.name != "Cylinder") { // make sure I'm only hitting the button for now
	//			Debug.Log("hitting something else: " + hitObject.name);
	//		}
	//		float distanceFromCamera;
	//		Vector3 touchPosition;
	//
	//		if (beingTouched.ContainsKey(hitObject)) { // Object already being touched or has multiple fingers added during this frame
	//			bool waitToMove = false;
	//			foreach (Touch touch in InputProxy.touches) { // check if there are any other fingers on the object
	//				int layerToHit = 1 << 8;
	//				Ray ray = camera.ScreenPointToRay(touch.position);
	//				RaycastHit hit;
	//				bool collision = Physics.Raycast(ray, out hitCheck, 60f, layerToHit); 
	//				if (collision) {
	////						GameObjecttouchedObject = hit.transform.gameObject; // WHY DOESNT THIS WORK??
	//					if (hitObject.transform.gameObject == hitObject && touch.fingerId != currentTouch.fingerId) { // if a different finger is touching the same object
	//						if (!beingTouched[hitObject].Contains(touch.fingerId)) {
	//							waitToMove = true; // wait to move until that finger is recorded
	//						} 
	//					}
	//				}	
	//			}
	//			if (!waitToMove) { // all fingers currently on the object have been recorded
	//				if (beingTouched[hitObject].Count == 1) { // if there's only one finger touching, drag normally
	//					distanceFromCamera = camera.transform.position.y - originalY[hitObject];
	//					touchPosition = touchedPosition(touchRay, camera.transform, distanceFromCamera);
	//					hitObject.transform.position = touchPosition + offsets[hitObject]; 
	//					Debug.Log("drag just one finger");
	//				} else if (beingTouched[hitObject].Count == 4) {
	//					// expand menu if the fingers move outwards
	//					Debug.Log("expand menu!!");
	//				} else {
	//					Debug.Log("wrong number of fingers");
	//				}
	//			} else { // if you have to wait to move
	//				if (!beingTouched[hitObject].Contains(currentTouch.fingerId)) {
	//					beingTouched[hitObject].Add (currentTouch.fingerId); // don't really need to update offsets or originalY
	//					Debug.Log("multiple fingers touching");
	//				}
	//			}
	//		} else { // Object being touched for first time 
	//			Debug.Log("first touch");
	//			List<float> touchList = new List<float>();
	//			touchList.Add (currentTouch.fingerId);
	//			beingTouched.Add (hitObject, touchList);
	//			distanceFromCamera = camera.transform.position.y - hitObject.transform.position.y;
	//			originalY.Add (hitObject, hitObject.transform.position.y);// add to originalY
	//			touchPosition = touchedPosition (touchRay, camera.transform, distanceFromCamera); // calculate touch position 
	//			Vector3 offset = hitObject.transform.position - touchPosition; // calculate offset 
	//			offsets.Add (hitObject, offset);
	//		}
	//	}
	//
	//	// FIX THIS METHOD ALSO
	//
	//	void updateDictionary() {
	//		List<GameObject> keys = new List<GameObject>();
	//		foreach (GameObject key in beingTouched.Keys) {
	//			keys.Add (key);
	//		}
	//		for(int i = 0; i < keys.Count; i++) {
	//			GameObject currentKey = keys[i];
	//			bool stillTouched = false;
	//			for (int j = 0; j < InputProxy.touches.Length; j++) {
	//				if (beingTouched[currentKey].Contains(InputProxy.touches[j].fingerId)) {
	//					stillTouched = true;
	//				}
	//			}
	//			if (!stillTouched) {
	//				beingTouched.Remove (currentKey);
	//				originalY.Remove (currentKey);
	//				offsets.Remove (currentKey);
	//			}
	//		}
	//	}
	//
	//	void expandMenu() {
	//		foreach (GameObject key in beingTouched.Keys) {
	//			if (beingTouched[key].Count == 4) {
	//
	//			}
	//		}
	//	}
	//
	
	
	//	void updateDictionary() {
	//		foreach (GameObject key in beingTouched.Keys) {
	//			bool stillTouching = false;
	//			foreach (int fingerID in beingTouched[key]) {
	//				foreach (Touch touch in InputProxy.touches) {
	//					if (touch.fingerId == fingerID) {
	//					}
	//				}
	//			}
	//
	//			if (beingTouched[key].Count == 0) {
	//				beingTouched.Remove (key);
	//				originalY.Remove (key);
	//				offsets.Remove (key);
	//			}
	//		}
	//	}
}
