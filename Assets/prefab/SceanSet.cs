using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			SceneManager.LoadScene ("BodyToManekin");
		}else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SceneManager.LoadScene ("ChromaKeyMirror");
		}else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SceneManager.LoadScene ("LateMovie");
		}else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SceneManager.LoadScene ("ManekinToBody");
		}
	}
}
