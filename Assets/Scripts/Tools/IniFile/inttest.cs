using UnityEngine;
using System.Collections;


public class inttest : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        IniFile ini = new IniFile("T1");
        print(ini .GetInt ("a/1"));
        ini.Set("a/1",69);
        ini.Save("T1");
        
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
