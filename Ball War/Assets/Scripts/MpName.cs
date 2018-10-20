using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpName : MonoBehaviour {
//comentario
	public enum Index {First,Second};
	public Index tipo;
	public Transform camera;
	public GameObject FirstPlayer;
	public GameObject SecondPlayer;
	void Start () {
		if (tipo == Index.First)
		{
			this.gameObject.transform.position = new Vector3(FirstPlayer.transform.position.x,FirstPlayer.transform.position.y + 3f,FirstPlayer.transform.position.z);
		}
	else
		{
			this.gameObject.transform.position = new Vector3(SecondPlayer.transform.position.x,SecondPlayer.transform.position.y + 3f,SecondPlayer.transform.position.z);
		}
		this.gameObject.transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
