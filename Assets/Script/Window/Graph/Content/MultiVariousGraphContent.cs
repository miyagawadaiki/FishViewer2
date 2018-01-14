using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiVariousGraphContent : GraphContent {

	public List<GraphManager> graphManList;

	void Awake() {
		graphManList = new List<GraphManager> ();
	}

	public override void Init () {
		foreach (GraphManager gm in graphManList) {
			gm.Init ();
		}

		ShowView ();
	}

	public override void Set(string parameters) {
		RemoveGraphManager ();

		if (parameters.Equals (""))
			return;

		char[] fishSeparator = {'\t'}, typeSeparator = { ' ' };
		string[] fishParameters = parameters.Split (fishSeparator, System.StringSplitOptions.RemoveEmptyEntries);


	}

	public override void RemoveGraphManager() {
		int num = graphManList.Count;
		for (int i = 0; i < num; i++) {
			GraphManager gm = graphManList [0];
			Simulation.Remove (gm);
			graphManList.RemoveAt (0);
			Destroy (gm.gameObject);
		}
	}
}
