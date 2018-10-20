using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpSecondPlayer : MonoBehaviour 
{
	private Vector3 NewPosition;
	private Vector3 OldPosition;
	public Vector3 ReceivedPosition;

	// Use this for initialization
	void Start () {
		MultiplayerConnection script = MultiplayerConnection.Instance;
		gameObject.name = script.getItself().DisplayName;
		GameObject.Find("SecondPlayerName").GetComponent<TextMesh>().text = script.OtherPlayer().DisplayName;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//GameObject.Find("LogPos").GetComponent<Text>().text = ReceivedPosition.ToString();
		MultiplayerConnection script = MultiplayerConnection.Instance;
		OldPosition = NewPosition;
		NewPosition = ReceivedPosition;
		if(OldPosition != NewPosition)
		{
			gameObject.GetComponent<Rigidbody>().MovePosition(NewPosition);
		}

	}
}
