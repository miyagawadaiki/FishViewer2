using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiEvenGraphSettingContent : MyWindowContent {

	private MultiEvenGraphContent megc;

	[SerializeField]
	private Button doneButton = null;
	[SerializeField]
	private GraphTypeSetting gts = null;
	[SerializeField]
	private FishMultiSelectSetting fmss = null;
	[SerializeField]
	private SourceSetting ss = null;
	[SerializeField]
	private PointerSetting ps = null;

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.MoveTo (new Vector2 ());
		mwc.canExpand = false;

		doneButton.onClick.AddListener (() => mwc.Destroy ());

		ss.CoverElement (0);
		ps.CoverElement (1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RegisterGraphContent(MultiEvenGraphContent megc) {
		this.megc = megc;

		foreach (Setting s in this.GetComponentsInChildren<Setting>()) {
			s.type = GraphContentType.MultiEven;
			s.gc = megc;
		}
	}

	public void UpdateGraphContent() {
		if(fmss.GetData().Equals("")) {
			megc.Set (gts.GetGraphType (), null);
			return;
		}

		char[] separator = { ',' };
		string[] hoge = fmss.GetData ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		int num = hoge.Length;
		string[] ret = new string[num];

		string[] ssElement = ss.GetData ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		string[] psElement = ps.GetData ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < num; i++) {
			ret[i] = 	hoge [i] + "," + 
						ssElement[1] + "," + 
						ssElement[2] + "," + 
						psElement [0] + "," +
						int.Parse (hoge [i]) + "," +
						psElement [2] + "," +
						psElement [3] + "," +
						psElement [4] + "," +
						psElement [5];

		}

		megc.Set (gts.GetGraphType (), ret);
	}
}
