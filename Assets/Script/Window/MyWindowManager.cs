using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyWindowManager : MonoBehaviour {

	public bool multiSelect, squareExpand, isDraging;

	[SerializeField]
	private GameObject windowObj = null;

	private List<MyWindowController> windowList;
	private RectTransform recTra;

	private Vector2 leftOrigin, rightOrigin, leftStart, rightStart;
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
		GameObject contentObj = Instantiate(Resources.Load ("Content/" + System.Enum.GetName (typeof(ContentType), content) + "Content"), obj.transform) as GameObject;
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



	public MyWindowController GetLastWindowController() {
		return windowList [windowList.Count - 1];
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
				Debug.Log ("<color=green>Remove : " + mwc.name + "</color>");
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

			//leftOrigin = (Vector2)Input.mousePosition;
			leftStart = (Vector2)Input.mousePosition;

			/*
			if (clicked.IsOnLeftSide (pos))
				expandDir.x = -1f;
			if (clicked.IsOnRightSide (pos))
				expandDir.x = 1f;
			if (clicked.IsOnBottomSide (pos))
				expandDir.y = -1f;
			if (clicked.IsOnTopSide (pos))
				expandDir.y = 1f;
			*/
			expandDir = clicked.GetExpandDir (pos);

			if(!expandDir.Equals (new Vector2())) {
				isExpMode = true;
			} else if(clicked.IsInMenu(pos)) {
				isMoveMode = true;
			}

			isDraging = true;

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

			//rightOrigin = (Vector2)Input.mousePosition;
			rightStart = (Vector2)Input.mousePosition;

			isDraging = true;

			clicked.content.OnRightClick (pos);
		}
	}

	// 左ドラッグ時の動作
	public void OnMouseLeftDrag() {
		Vector2 now = (Vector2)Input.mousePosition;
		if (isExpMode) {
			foreach (MyWindowController mwc in windowList) {
				if (mwc.isSelected) {
					if (squareExpand) {
						mwc.SquareExpand (now - leftStart, expandDir);
						//if (!mwc.IsInWindowManager ())
						//	mwc.Expand (now - start, expandDir);
					} else {
						mwc.Expand (now - leftStart, expandDir);
						//if (!mwc.IsInWindowManager ())
						//	mwc.Expand (start - now, expandDir);
					}

					if (!expandDir.Equals (new Vector2 ()))
						mwc.content.OnExpand (now - leftStart, expandDir);
				}
			}
		} else if (isMoveMode) {
			foreach (MyWindowController mwc in windowList) {
				if (mwc.isSelected) {
					mwc.Translate (now - leftStart);
					//if (!mwc.IsInWindowManager ())
					//	mwc.Translate (start - now);

					mwc.content.OnTranslate (now - leftStart);
				}
			}
		} else {
			foreach (MyWindowController mwc in windowList) {
				if (mwc.isSelected) {
					mwc.content.OnLeftDrag (leftStart, now);
				}
			}
		}

		leftStart = now;
	}

	public void OnMouseRightDrag() {
		Vector2 now = (Vector2)Input.mousePosition;
		foreach (MyWindowController mwc in windowList) {
			if (mwc.isSelected) {
				mwc.content.OnRightDrag (rightStart, now);
			}
		}

		rightStart = now;
	}


	// 左ドロップ時の動作
	public void OnMouseLeftUp() {
		isMoveMode = false;
		isDraging = false;
		expandDir = new Vector2 ();
		isExpMode = false;
	}


	// 右ドロップ時の動作
	public void OnMouseRightUp() {
		isDraging = false;
	}


	// ホイール操作時の動作
	public void OnWheelChange(float value) {
		foreach (MyWindowController mwc in windowList) {
			if (mwc.isSelected && mwc.Contains(Input.mousePosition)) {
				mwc.content.OnWheelChange(value);
			}
		}
	}


	/*
	// カーソルの画像を変える
	public void WatchCursor() {
		Vector2 expdir, pos = Input.mousePosition;
		foreach (MyWindowController mwc in windowList) {
			expdir = mwc.GetExpandDir (pos);
			if (isExpMode || !expdir.Equals(new Vector2())) {
				Cursor.visible = false;
				float a = expdir.x * expdir.y;

			}
		}
	}
	*/



	public int Count() {
		return windowList.Count;
	}

	public enum ContentType {
		FileSelect,
		FileRead,
		FileMake,
		SingleGraph,
		MultiEvenGraph,
		MultiVariousGraph,
		SingleGraphSetting,
		MultiEvenGraphSetting,
		MultiVariousGraphSetting,
		MultiVariousParameterSetting,
		Parameter,
		Sample,
	}
}
