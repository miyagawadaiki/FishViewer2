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
	private List<Button> buttonList = null;


	void Awake() {
		spc.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		foreach (Button b in buttonList)
			b.interactable = false;
			//b.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!mbc.isActive && !mwm.isDraging && Input.mousePosition.y > Screen.height - 10f && Input.mousePosition.y <= Screen.height) {
			mbc.SlideIn ();
			return;
		}

		if (Simulation.isEnabled && Input.GetKeyDown (KeyCode.Space)) {
			spc.Slide ();
			return;
		}

		if (mbc.isActive) {
			
			if (!mbc.IsMouseInArea ())
					mbc.SlideOut ();
				//if (Input.GetMouseButtonDown (0))
				//	mbc.OnMouseLeftDown ();
				
		} else if (!spc.gameObject.activeSelf || !spc.IsMouseInArea ()) {
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
		foreach (Button b in buttonList)
			b.interactable = true;
			//b.gameObject.SetActive (true);
	}
}
