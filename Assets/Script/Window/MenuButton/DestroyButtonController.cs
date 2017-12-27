using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyButtonController : MonoBehaviour {

	private Button button;

	// Use this for initialization
	void Start () {
		button = this.GetComponent<Button> ();
		button.onClick.AddListener (() => this.GetComponentInParent<MyWindowController> ().Destroy ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
