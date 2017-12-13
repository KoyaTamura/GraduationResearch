using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class WebCamMirror : MonoBehaviour
{

    private WebCamTexture webcamtex;
    public Quaternion direction;

    // Use this for initialization
    void Start()
    {

        WebCamDevice[] devices = WebCamTexture.devices;


        webcamtex = new WebCamTexture(devices[1].name);

        Renderer rend = GetComponent<Renderer>();
        rend.material.mainTexture = webcamtex;
        webcamtex.Play();

    }

    // Update is called once per frame
    void Update()
    {

        //del live mirror
        direction = InputTracking.GetLocalRotation(VRNode.Head);

        if (0.4f <= direction[0] && direction[0] < 0.50f)
        {
            gameObject.SetActive(false);
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
            mirrorPos.y = mirrorPos.y- 0.01f;
            gameObject.transform.position = mirrorPos;

        }
    }
}
