using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectData;

public class FileInputFieldManager : MonoBehaviour {

	[SerializeField]
	private InputField pathIf = null;
	[SerializeField]
	private InputField nameIf = null;

	public FileKey key;

	private List<string> address;
	private char[] separator = { '/', '\\' };
	private int viewIndex;
	private string nextName = null;

	void Awake() {
		address = new List<string> ();
	}

	// Use this for initialization
	void Start () {
		SetPath (FileName.GetPath(key));
		pathIf.text = GetPath (pathIf.characterLimit);
		nameIf.text = FileName.GetName (key);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// リストにパスを分けて登録する
	public void SetPath(string path) {
		address.Clear ();
		string[] tmp = path.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (string s in tmp)
			address.Add (s);
	}


	// 編集終了時の処理　書き込まれたパスを登録する
	public void EndEdit() {
		string buf = "";
		for (int i = 0; i < viewIndex; i++)
			buf += address [i] + "/";

		string[] tmp = pathIf.text.Split (separator);
		for (int i = 1; i < tmp.Length; i++)
			buf += tmp [i] + "/";
		
		SetPath (buf);
	}


	// パスを文字列で取得
	public string GetPath() {
		string path = address [0];
		for (int i = 1; i < address.Count; i++)
			path += "/" + address [i];

		return path;
	}


	// Text渡し用に字数を制限してパスを文字列化する
	public string GetPath(int limit) {
		if (address [address.Count - 1].Length > limit) {
			viewIndex = address.Count - 1;
			return ".../" + address [address.Count - 1];
		}

		int sum = address [address.Count - 1].Length;
		for (int i = address.Count - 2; i >= 1; i--) {
			sum += address [i].Length + 1;
			if (sum + 3 > limit) {
				viewIndex = i + 1;
				string buf = "...";
				for (int j = i + 1; j < address.Count; j++)
					buf += "/" + address [j];

				return buf;
			}
		}

		if (sum + address [0].Length <= limit) {
			viewIndex = 0;
			return GetPath ();
		} else {
			viewIndex = 1;
			string buf = "...";
			for (int i = 1; i < address.Count; i++)
				buf += "/" + address [i];

			return buf;
		}
	}


	// ファイル名を取得する
	public string GetName() {
		return nameIf.text;
	}


	// フォルダ構造を1つ戻る
	public void GoBack() {
		nextName = address [address.Count - 1];
		address.RemoveAt (address.Count - 1);
		pathIf.text = GetPath (pathIf.characterLimit);
	}


	// 指定したフォルダに進む
	public void GoNext(string name) {
		address.Add (name);
		pathIf.text = GetPath (pathIf.characterLimit);
	}


	// 以前にいたフォルダに戻る
	public void GoNext() {
		if (nextName == null)
			return;
		address.Add (nextName);
		pathIf.text = GetPath (pathIf.characterLimit);
	}
}
