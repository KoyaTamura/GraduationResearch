using System.Collections;
using UnityEngine;
//using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;

public class LateMovie : MonoBehaviour {

	//webcam数値
	private WebCamTexture webcamTexture;
	public int camNum = 0;
	public int width = 1920;
	public int height = 1080;
	public int fps = 30;

	//代入する云々
	private Texture2D setTexture;
	private Texture2DArray textureArray;
	private bool webcamIsPlaying = true;

	//何秒遅らせるか
	public int lateSec = 3;
	public float lateSpeed = 0.01f;

	//遅延を減らすスイッチ
	private bool realTime = false;
	public bool swichLate = true;
	private int GoBack;
	private float fixTime;




	// Use this for initialization
	void Start () {

		WebCamInit ();
		//Application.targetFrameRate = fps; //FPS設定

	}
		

	// Update is called once per frame
	void Update () {

		//最初の一回で代入先のtextureを初期化
		if(webcamTexture.width > 16 && webcamTexture.height > 16 && webcamIsPlaying){
			SetTextureInit ();
			webcamIsPlaying = false;
		}

		//全て初期化されたらここに入る
		if(webcamIsPlaying == false){
			
			if(Input.GetKeyDown(KeyCode.Return)){
				realTime = true;
				GoBack = 1 - GoBack;
			}

			textureArray.SetPixels (webcamTexture.GetPixels() , Time.frameCount % (lateSec * fps));
            //setTexture.SetPixels(textureArray.GetPixels(Time.frameCount % (lateSec * fps)));
            //setTexture.Apply();
            StartCoroutine ("realTimeMovie",Time.frameCount % (fps * lateSec));

        }

		Debug.Log (fixTime);

	}

	void WebCamInit(){
		//遅延した状態で始めるならGoBackは0でfixTimeは0
		//リアルタイムで始める場合はGoBackは1でfixTimeはlateSec
		if (swichLate) {
			GoBack = 0;
			fixTime = 0;
		} else {
			GoBack = 1;
			fixTime = lateSec;
		}

		//webcamをスタートさせる
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length > camNum)
		{
			webcamTexture = new WebCamTexture(devices[camNum].name, width, height, fps);
			webcamTexture.Play();
		}
		else
		{
			Debug.Log("no camera");
		}

		//デバイスの名前一覧表示
		for (int i = 0; i < devices.Length; i++)
		{
			Debug.Log(devices[i].name);
		}
	}

	void SetTextureInit(){

		//テクスチャー配列、カラー、表示テクスチャーの初期化
		textureArray = new Texture2DArray(webcamTexture.width, webcamTexture.height,lateSec * fps, TextureFormat.RGB565,false);
		setTexture = new Texture2D(webcamTexture.width , webcamTexture.height,TextureFormat.RGB565,false);
		GetComponent<Renderer> ().material.mainTexture = setTexture;
	}

	IEnumerator realTimeMovie(int i){

		if(realTime && GoBack == 1){
			if (lateSec - fixTime > 0) {
				fixTime += lateSpeed;
			} else {
				fixTime = lateSec;
				realTime = false;
			}
		}else if (realTime && GoBack == 0){
			if (fixTime > 0) {
				fixTime -= lateSpeed;
			} else {
				fixTime = 0;
				realTime = false;
			}
		}

		yield return  new WaitForSeconds ((float)lateSec - fixTime);
		setTexture.SetPixels (textureArray.GetPixels (i));
		setTexture.Apply ();
	}
		
}
