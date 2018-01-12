using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour {

	[SerializeField]
	private Sprite[] expandMarker = null;

	private Image image;

	void Awake() {
		image = this.GetComponent<Image> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.position = 
	}

	public void ChangeExpand(Vector2 expandDir) {
		this.gameObject.SetActive (true);
		Cursor.visible = false;

		float a = expandDir.x * expandDir.y;
		if (a > 0f) {
			image.sprite = expandMarker [0];
		} else if (a < 0f) {
			image.sprite = expandMarker [1];
		} else {
			if (expandDir.x == 0f) {
				image.sprite = expandMarker [2];
			} else if(expandDir.y == 0f) {
				image.sprite = expandMarker [3];
			}
		}
	}
}
