using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyWindowManager : MonoBehaviour {

	private List<MyWindowController> windowList;

	private Vector2 start;
	private bool isMoveMode, isExpMode, multiSelect;

	void Awake() {
		windowList = new List<MyWindowController> ();
	}

	// Use this for initialization
	void Start () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Window")) {
			windowList.Add (obj.GetComponent<MyWindowController> ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			multiSelect = true;
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			multiSelect = false;
		}

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Down");
			OnMouseDown ();
		} else if (Input.GetMouseButton (0)) {
			Debug.Log ("Drag");
			OnMouseDrag ();
		} else if(Input.GetMouseButtonUp(0)) {
			OnMouseUp ();
		}
	}

	void OnMouseDown() {
		int max = -1;
		MyWindowController clicked = null;
		Vector2 pos = Input.mousePosition;
		foreach (MyWindowController mwc in windowList) {
			if(!multiSelect) mwc.SetNormalMode ();
			if (mwc.Contains(pos)) {
				if (mwc.GetSiblingIndex () > max) {
					clicked = mwc;
					max = mwc.GetSiblingIndex ();
				}
			}
		}

		if(clicked != null) {
			clicked.SetAsLastSibling ();
			clicked.SetSelectMode ();

			if (clicked.IsInMenu (pos)) {
				Debug.Log ("Translate");
				isMoveMode = true;
				start = (Vector2)Input.mousePosition;
			} else if (clicked.IsOnBottomRight (pos)) {
				Debug.Log ("Expand");
				isExpMode = true;
				start = (Vector2)Input.mousePosition;
			}
		}
	}

	void OnMouseDrag() {
		Vector2 now = (Vector2)Input.mousePosition;
		if (isMoveMode) {
			foreach (MyWindowController mwc in windowList) {
				if (mwc.IsSelect) {
					mwc.Translate (now - start);
				}
			}
		} else if(isExpMode) {
			foreach (MyWindowController mwc in windowList) {
				if (mwc.IsSelect) {
					Vector2 ex = now - start;
					mwc.Expand (new Vector2(ex.x, -ex.y));
				}
			}
		}
		start = now;
	}

	void OnMouseUp() {
		isMoveMode = false;
		isExpMode = false;
	}
}
