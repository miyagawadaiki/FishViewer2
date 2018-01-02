using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyAppManager : MonoBehaviour {
	
	[SerializeField]
	private MyWindowManager mwm = null;
	[SerializeField]
	private MenuBarController mbc = null;
	[SerializeField]
	private SimuPanelController spc = null;


	void Awake() {
		spc.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!mbc.isActive && Input.mousePosition.y > Screen.height - 10f) {
			mbc.SlideIn ();
		}

		if (Simulation.isEnabled && Input.GetKeyDown (KeyCode.Space)) {
			spc.Slide ();
		}

		if (mbc.isActive || spc.isActive) {
			
			if (mbc.isActive) {
				if (!mbc.IsMouseInArea ())
					mbc.SlideOut ();
				//if (Input.GetMouseButtonDown (0))
				//	mbc.OnMouseLeftDown ();
			}

			if (spc.isActive) {

			}
		} else {
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				mwm.multiSelect = true;
			} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
				mwm.multiSelect = false;
			}

			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("Down");
				mwm.OnMouseLeftDown ();
			} else if (Input.GetMouseButton (0)) {
				Debug.Log ("Drag");
				mwm.OnMouseLeftDrag ();
			} else if(Input.GetMouseButtonUp(0)) {
				mwm.OnMouseLeftUp ();
			}

			if (Input.GetMouseButtonDown (1)) {
				mwm.OnMouseRightDown ();
			} else if (Input.GetMouseButton (1)) {

			} else if (Input.GetMouseButtonUp (1)) {

			}
		}
	}

	public void ActivateSimuPanel() {
		spc.gameObject.SetActive (true);
	}
}
