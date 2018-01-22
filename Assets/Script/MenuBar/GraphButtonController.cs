using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphButtonController : MonoBehaviour {

	[SerializeField]
	private RectTransform templete = null;

	private Button button;
	private MenuBarController mbc;
	private RectTransform recTra;

	void Awake () {
		button = this.GetComponent<Button> ();
		mbc = this.GetComponentInParent<MenuBarController> ();
		recTra = this.GetComponent<RectTransform> ();
	}

	// Use this for initialization
	void Start () {
		button.onClick.AddListener (() => mbc.RegisterPanel (templete));
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (recTra.rect.Contains (Input.mousePosition - this.transform.position)) {
			templete.gameObject.SetActive (true);
			mbc.RegisterPanel (templete);
		} else {
			templete.gameObject.SetActive (false);
		}
		*/
	}
}
