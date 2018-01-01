using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyWindowManager : MonoBehaviour {

	public bool multiSelect;

	[SerializeField]
	private GameObject windowObj = null;

	private List<MyWindowController> windowList;
	private RectTransform recTra;

	private Vector2 start;
	private Vector2 expandDir = new Vector2();
	private bool isMoveMode, isExpMode, removeFlag;
	private float canvasScale;

	void Awake() {
		windowList = new List<MyWindowController> ();
	}

	// Use this for initialization
	void Start () {
		recTra = GameObject.Find("Canvas").GetComponent<RectTransform>();
		canvasScale = recTra.localScale.x;

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Window")) {
			windowList.Add (obj.GetComponent<MyWindowController> ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (removeFlag) {
			Remove ();
		}

		/*
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			multiSelect = true;
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			multiSelect = false;
		}

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Down");
			OnMouseLeftDown ();
		} else if (Input.GetMouseButton (0)) {
			Debug.Log ("Drag");
			OnMouseLeftDrag ();
		} else if(Input.GetMouseButtonUp(0)) {
			OnMouseLeftUp ();
		}

		if (Input.GetMouseButtonDown (1)) {
			OnMouseRightDown ();
		} else if (Input.GetMouseButton (1)) {

		} else if (Input.GetMouseButtonUp (1)) {

		}
		*/
	}

	public void AddWindow() {
		Debug.Log ("Add window");
		GameObject obj = Instantiate (windowObj, this.transform) as GameObject;
		RectTransform wrt = obj.GetComponent<RectTransform>();
		wrt.position = new Vector3 (wrt.rect.width / 2 + windowList.Count * 20f + 10f, recTra.rect.height - wrt.rect.height / 2 - windowList.Count * 20f - 10f, 0f) * canvasScale;
		MyWindowController mwc = obj.GetComponent<MyWindowController> ();
		windowList.Add (mwc);
	}


	// WindowにContentを指定して追加する
	public void AddWindow(ContentType content, string type) {
		Debug.Log ("Add window");

		// MyWindowManagerに新規Windowを追加
		GameObject obj = Instantiate (windowObj, this.transform) as GameObject;
		RectTransform wrt = obj.GetComponent<RectTransform>();
		//wrt.position = new Vector3 (wrt.rect.width / 2 + windowList.Count * 20f + 10f, recTra.rect.height - wrt.rect.height / 2 - windowList.Count * 20f - 10f, 0f) * canvasScale;
		MyWindowController mwc = obj.GetComponent<MyWindowController> ();
		windowList.Add (mwc);

		// 追加したWindowにContentを設定する
		GameObject contentObj = Instantiate(Resources.Load (System.Enum.GetName (typeof(ContentType), content) + "Content"), obj.transform) as GameObject;
		contentObj.GetComponent<MyWindowContent>().typeName = type;
		//mwc.content = contentObj.GetComponent<MyWindowContent> ();
	}



	public void AddWindow(string name) {
		char[] sep = { '/' };
		string[] tmp = name.Split (sep);
		if (tmp.Length == 1)
			AddWindow ((ContentType)System.Enum.Parse (typeof(ContentType), name), "");
		else
			AddWindow ((ContentType)System.Enum.Parse (typeof(ContentType), tmp[0]), tmp[1]);
	}


	// Remove()をCallする（次のUpdate()のときにRemove()を実行させる）
	public void CallRemove() {
		removeFlag = true;
	}


	// Windowを削除する
	public void Remove() {
		for (int i = 0;i<windowList.Count;i++) {
			if (windowList[i].isDestroyed) {
				MyWindowController mwc = windowList [i];
				windowList.RemoveAt (i);
				Destroy (mwc.gameObject, 0.1f);
			}
		}
		removeFlag = false;
	}


	// 左クリック時の動作
	public void OnMouseLeftDown() {
		int max = -1;
		MyWindowController clicked = null;
		Vector2 pos = Input.mousePosition;
		foreach (MyWindowController mwc in windowList) {
			if (!multiSelect && this.GetComponent<RectTransform>().rect.Contains (pos / canvasScale - this.GetComponent<RectTransform>().rect.size / 2)) {
				Debug.Log ("in window");
				mwc.SetNormalMode ();
			}

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
			} else {
				Debug.Log ("Expand");
				isExpMode = true;
				start = (Vector2)Input.mousePosition;

				if (clicked.IsOnLeftSide (pos))
					expandDir.x = -1f;
				if (clicked.IsOnRightSide (pos))
					expandDir.x = 1f;
				if (clicked.IsOnBottomSide (pos))
					expandDir.y = -1f;
				if (clicked.IsOnTopSide (pos))
					expandDir.y = 1f;
			}

			clicked.content.OnLeftClick (pos);
		}
	}


	// 右クリック時の操作
	public void OnMouseRightDown() {
		int max = -1;
		MyWindowController clicked = null;
		Vector2 pos = Input.mousePosition;
		foreach (MyWindowController mwc in windowList) {
			if (!multiSelect && this.GetComponent<RectTransform>().rect.Contains (pos / canvasScale - this.GetComponent<RectTransform>().rect.size / 2)) {
				Debug.Log ("in window");
				mwc.SetNormalMode ();
			}

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

			clicked.content.OnRightClick (pos);
		}
	}

	// 左ドラッグ時の動作
	public void OnMouseLeftDrag() {
		Vector2 now = (Vector2)Input.mousePosition;
		if (isMoveMode) {
			foreach (MyWindowController mwc in windowList) {
				if (mwc.isSelected) {
					mwc.Translate (now - start);
					if (!mwc.IsInWindowManager ())
						mwc.Translate (start - now);
				}
			}
		} else if(isExpMode) {
			foreach (MyWindowController mwc in windowList) {
				if (mwc.isSelected) {
					mwc.Expand (now - start, expandDir);
					if (!mwc.IsInWindowManager ())
						mwc.Expand (start - now, expandDir);
				}
			}
		}

		/*
		foreach (MyWindowController mwc in windowList) {
			if (mwc.isSelected) {
				mwc.content.OnLeftDrag (start, now);
			}
		}
		*/

		start = now;
	}


	// 左ドロップ時の動作
	public void OnMouseLeftUp() {
		isMoveMode = false;
		isExpMode = false;
		expandDir = new Vector2 ();
	}



	public int Count() {
		return windowList.Count;
	}

	public enum ContentType {
		FileSelect,
		FileRead,
		SingleGraph,
		MultiEvenGraph,
		MultiVariousGraph,
		Sample,
	}
}
