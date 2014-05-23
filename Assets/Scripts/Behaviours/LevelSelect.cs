using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	void Start () {
	
	}
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Application.LoadLevel(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Application.LoadLevel(1);
	}
}
