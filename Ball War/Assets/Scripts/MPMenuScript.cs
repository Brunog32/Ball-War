using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MPMenuScript : MonoBehaviour { 
	public MultiplayerConnection gp;
	public Button LoginButton;
	public Button MatchButton;
	public Button ExitButton;
	// Use this for initialization
	void Start () {
		  gp = new MultiplayerConnection();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		
	}

	public void IsSignedInThanUpdateIt(bool IsSignedIn)
	{
		if (IsSignedIn == true)
		{
			LoginButton.interactable = false;
			ExitButton.interactable = true;
			MatchButton.interactable = true;
		}
		else
		{
			LoginButton.interactable = true;
			ExitButton.interactable = false;
			MatchButton.interactable = false;
		}
	} 
}