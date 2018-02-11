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
	[SerializeField]
	private WindowListPanelController wlpc = null;

	private bool isCommandMode = false;
	private KeyCode[] commands = {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
		KeyCode.Alpha0,
	};

	void Awake() {
		spc.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (!mbc.isActive && !mwm.isDraging && Input.mousePosition.y > Screen.height - 10f && Input.mousePosition.y <= Screen.height) {
			mbc.SlideIn ();
			return;
		}
		*/

		if (Input.GetKeyDown (KeyCode.W)) {
			wlpc.Slide ();
			return;
		}

		if (Simulation.isEnabled && Input.GetKeyDown (KeyCode.A)) {
			spc.Slide ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			isCommandMode = true;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			isCommandMode = false;
		}

		if (isCommandMode) {
			if (Simulation.isEnabled) {
				for (int i = 0; i < ProjectData.DefaultData.shortcutTexts.Count; i++) {
					if (Input.GetKeyDown (commands [i])) {
						mwm.AddWindow (ProjectData.DefaultData.shortcutTexts [i]);
						return;
					}
				}
			}
		}

		//if (mbc.isActive) {
			
		if (!mbc.IsMouseInArea ()) {
			mbc.HidePanel ();
			//mbc.SlideOut ();
			//if (Input.GetMouseButtonDown (0))
			//	mbc.OnMouseLeftDown ();
				
		} else {
			return;
		}

		if (!wlpc.IsMouseInArea () && (!spc.gameObject.activeSelf || !spc.IsMouseInArea ())) {
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				mwm.multiSelect = true;
			} else if (Input.GetKeyUp (KeyCode.LeftControl)) {
				mwm.multiSelect = false;
			}

			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				mwm.squareExpand = true;
			} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
				mwm.squareExpand = false;
			}

			if (Input.GetKeyDown (KeyCode.K)) {
				mwm.MakeClone ();
			}

			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("LeftDown");
				mwm.OnMouseLeftDown ();
			} else if (Input.GetMouseButton (0)) {
				Debug.Log ("LeftDrag");
				mwm.OnMouseLeftDrag ();
			} else if(Input.GetMouseButtonUp(0)) {
				mwm.OnMouseLeftUp ();
			}

			if (Input.GetMouseButtonDown (1)) {
				mwm.OnMouseRightDown ();
			} else if (Input.GetMouseButton (1)) {
				mwm.OnMouseRightDrag ();
			} else if (Input.GetMouseButtonUp (1)) {
				mwm.OnMouseRightUp ();
			}

			if (Input.GetAxis ("Mouse ScrollWheel") != 0f) {
				mwm.OnWheelChange (Input.GetAxis ("Mouse ScrollWheel"));
			}
		}
	}

	public void ActivateSimuPanel() {
		spc.gameObject.SetActive (true);

		mbc.Interactivate ();
	}
}
