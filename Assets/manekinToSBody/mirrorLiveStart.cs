using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class mirrorLiveStart : MonoBehaviour {

	private WebCamTexture webcamtex;
	public Quaternion direction;
	private bool playFlag = true;

	// Use this for initialization
	void Start () {
		
		WebCamDevice[] devices = WebCamTexture.devices;
		webcamtex = new WebCamTexture(devices[1].name);
		Renderer rend = GetComponent<Renderer>();
		rend.material.mainTexture = webcamtex;
	
	}
	
	// Update is called once per frame
	void Update () {

		direction = InputTracking.GetLocalRotation(VRNode.Head);

		//上を向いたタイミングでInSphereを消す
		if (0.4f <= direction[0] && direction[0] < 0.50f && playFlag)
		{
			webcamtex.Play ();
			playFlag = false;
			//Debug.Log("hoge");
		}

		else if(Input.GetKeyDown(KeyCode.Space) && playFlag){
			webcamtex.Play ();
			playFlag = false;
		}

        //key set mirror
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 mirrorPos = gameObject.transform.position;
            mirrorPos.x = mirrorPos.x + 0.01f;
            gameObject.transform.position = mirrorPos;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 mirrorPos = gameObject.transform.position;
            mirrorPos.x = mirrorPos.x - 0.01f;
            gameObject.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 mirrorPos = gameObject.transform.position;
            mirrorPos.y = mirrorPos.y + 0.01f;
            gameObject.transform.position = mirrorPos;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 mirrorPos = gameObject.transform.position;
            mirrorPos.y = mirrorPos.y - 0.01f;
            gameObject.transform.position = mirrorPos;

        }
    }
}
