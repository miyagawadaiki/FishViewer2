using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {

	public static bool isEnabled = false, playing = false;
	public static int step = 0;

	private static List<GraphContent> graphConList;
	//private static List<GraphManager> graphList;
	private static int memo = 0;

	public static void Init() {
		step = -1;
		memo = -2;

		DataBase.SetDataBase ();

		graphConList = new List<GraphContent> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GraphContent")) {
			obj.GetComponent<GraphContent> ().Init ();
			graphConList.Add (obj.GetComponent<GraphContent> ());
		}

		/*
		graphList = new List<GraphManager> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GraphContent")) {
			GraphManager gm = obj.GetComponent<GraphManager>();
			//gm.Init ();
			graphList.Add (gm);
		}
		*/

		if (!isEnabled)
			GameObject.Find ("MyAppManager").GetComponent<MyAppManager> ().ActivateSimuPanel ();
		
		GameObject.Find ("SimuPanel").GetComponent<SimulationController> ().Init ();
		
		isEnabled = true;
	}

	/*
	public static void Register(GraphManager gm) {
		graphList.Add (gm);
	}

	public static void Register(GraphManager[] gmArray) {
		foreach (GraphManager gm in gmArray)
			graphList.Add (gm);
	}
	*/

	public static void Register (GraphContent gc) {
		if (graphConList.Contains (gc))
			return;
		graphConList.Add (gc);
	}

	/*
	public static void Remove(GraphManager gm) {
		graphList.Remove (gm);
	}
	*/

	public static void Remove (GraphContent gc) {
		if (graphConList == null)
			return;
		graphConList.Remove (gc);
	}

	public static void Reset() {
		step = -1;
	}

	public static bool IsEnd() {
		return step >= DataBase.step - 1;
	}

	public static void Execute() {
		if (step == memo)
			return;
		foreach (GraphContent gc in graphConList) {
			gc.Plot (step);
		}
		/*
		foreach (GraphManager gm in graphList) {
			gm.Plot (step);
		}
		*/
		memo = step;
	}

	public static void Execute(int s) {
		if (s == memo)
			return;
		foreach (GraphContent gc in graphConList) {
			gc.Plot (s);
		}
		/*
		foreach (GraphManager gm in graphList) {
			gm.Plot (s);
		}
		*/
		step = s;
		memo = s;
	}
}
