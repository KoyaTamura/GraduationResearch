using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.VR;

public class fadeOutScript : MonoBehaviour {

    //fadeout用の黒いパネル
    public CanvasGroup fadeout;
    public Quaternion direction;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //ブラックアウトのコルーチン分岐
        direction = InputTracking.GetLocalRotation(VRNode.Head);

        if (0.4f <= direction[0] && direction[0] < 0.50f)
        {

            StartCoroutine("blackOut");
        }


    }

    //ブラックアウトのコルーチン
    private IEnumerator blackOut()
    {
        DOTween.To(() => fadeout.alpha, (x) => fadeout.alpha = x, 1.0f, 1.0f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(2.0f);
        DOTween.To(() => fadeout.alpha, (x) => fadeout.alpha = x, 0.0f, 1.0f).SetEase(Ease.InCubic);
    }

}
