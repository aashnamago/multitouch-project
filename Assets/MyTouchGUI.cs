using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//	public float buttonScale = 0.15f;
	//	public float zDistance = 3.88f;
	//	public float sphereScale = 15f;
	//	public Vector3 sphereSpawnPosition = new Vector3 (0, 0, 0);
	//	private GameObject originalMenu;	
	//	private Dictionary <int, GameObject> fingersPresent = new Dictionary<int, GameObject>(); // maps fingerIds to the touchGUIs they go with
	
	//	void createFirstMenu() {
	//		originalMenu = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
	//		originalMenu.name = "firstIcon";
	//		originalMenu.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
	//		float randX, randY, randZ;
	//		randX = Random.Range (-5f, 5f);
	//		randZ = Random.Range (-5f, 5f);
	//		randY = Random.Range (-5f, 5f);
	//		originalMenu.transform.position = sphereSpawnPosition + new Vector3(randX, randY, randZ);
	//		originalMenu.GetComponent<SphereCollider>().radius = 0.5f;
	//		originalMenu.renderer.material.color = Color.blue;
	//		originalMenu.layer = 9;
	//	}
	//
	//	void touchGUI() {
	//		Touch[] currentTouches = InputProxy.touches; 
	//		foreach (Touch fingerTouch in currentTouches) {
	//			if (fingersPresent.ContainsKey(fingerTouch.fingerId)) {
	//				followSpot(fingerTouch);
	//			} else {
	//				if (fingersPresent.Count < 14) {
	//					GameObject spot = spawnObject(fingerTouch);
	//					fingersPresent.Add (fingerTouch.fingerId, spot);
	//				}
	//			}
	//		}
	//		
	//		foreach (int fingerId in fingersPresent.Keys) {
	//			bool stillExists = false;
	//			for (int i = 0; i < currentTouches.Length; i++) {
	//				if (currentTouches[i].fingerId == fingerId) {
	//					stillExists = true;
	//				}
	//			}
	//			if (!stillExists) {
	//				Destroy(fingersPresent[fingerId]);
	//				fingersPresent.Remove(fingerId);
	//			}
	//		}
	//	}
	//	
	//	// Move gameobject based on fingerID
	//	void followSpot(Touch fingerTouch) {
	//		GameObject spot = fingersPresent[fingerTouch.fingerId];
	//		float xPos = fingerTouch.position.x;
	//		float yPos = fingerTouch.position.y;
	//		spot.transform.position = camera.ScreenToWorldPoint(new Vector3 (xPos, yPos, zDistance)); // place a certain distance away from camera 
	//	}
	//
	//	// Create a cylinder object at the location of fingerTouch
	//	GameObject spawnObject(Touch fingerTouch) {
	//		// USE A RING FROM ILLUSTRATOR INSTEAD?
	//		GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Cylinder); 
	//		spot.name = "spot";
	//		spot.transform.localScale = new Vector3(buttonScale, 0, buttonScale);
	//		float xPos = fingerTouch.position.x;
	//		float yPos = fingerTouch.position.y;
	//		spot.transform.position = camera.ScreenToWorldPoint(new Vector3 (xPos, yPos, zDistance)); // place a certain distance away from camera  
	//		spot.renderer.material.color = Color.black;
	//		spot.renderer.material.shader = Shader.Find("Somian/Unlit/Transparent"); 
	//		return spot;
	//	}
}
