using System.Collections;
using System.Collections.Generic;

public class DataBase {

	private static DataBase mInstance;

	private string fileName;
	private string[] tags;
	private float[,,] data;

	private DataBase() {
		
	}

	public static DataBase Instance {
		get {
			if (mInstance == null)
				mInstance = new DataBase ();
			return mInstance;
		}
	}

	public void Init() {
		
	}
}
