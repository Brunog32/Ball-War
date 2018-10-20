using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi;
public class MultiplayerMovement : MonoBehaviour {

	private float  positionX, positionY, positionZ,rotationX,rotationY,rotationZ;
	private byte[] DataWillDeliver_Sent;
	private List<byte> DataList;

	// Use this for initialization
	void Start () {
		DataList = new List<byte>();
		MultiplayerConnection script = MultiplayerConnection.Instance;
		gameObject.name = script.getItself().DisplayName;
		GameObject.Find("FirstPlayerName").GetComponent<TextMesh>().text = script.getItself().DisplayName;
	}

	public void SendPosition(float positionX, float positionY, float positionZ)
	{
		DataList.Clear();
		DataList.Add((byte) 'P');
		DataList.AddRange(System.BitConverter.GetBytes(positionX));
		DataList.AddRange(System.BitConverter.GetBytes(positionY));
		DataList.AddRange(System.BitConverter.GetBytes(positionZ));
		DataWillDeliver_Sent = DataList.ToArray();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false,DataWillDeliver_Sent);
	}

		public void SendRotation(float rotationX, float rotationY, float rotationZ)
	{
		DataList.Clear();
		DataList.Add((byte) 'R');
		DataList.AddRange(System.BitConverter.GetBytes(rotationX));
		DataList.AddRange(System.BitConverter.GetBytes(rotationY));
		DataList.AddRange(System.BitConverter.GetBytes(rotationZ));
		DataWillDeliver_Sent = DataList.ToArray();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false,DataWillDeliver_Sent);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		SendPosition(this.gameObject.transform.position.x,this.gameObject.transform.position.y,this.gameObject.transform.position.z);
		SendRotation(this.gameObject.transform.rotation.eulerAngles.x,this.gameObject.transform.rotation.eulerAngles.y,this.gameObject.transform.rotation.eulerAngles.z);
		

	}
}
