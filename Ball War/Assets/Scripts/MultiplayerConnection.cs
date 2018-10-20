using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Com.Google.Android.Gms.Games;

public class MultiplayerConnection : MonoBehaviour, RealTimeMultiplayerListener {

	static MultiplayerConnection sInstance= new MultiplayerConnection();
	private List<Participant> Players;
	public GameObject SecondPlayer;
	private bool showingWaitingRoom = false;

	private Invitation mFirstInvite=null,mIncomingInvitation = null;
	
	public Rect labelRect, acceptButtonRect,declineButtonRect;
	// Use this for initialization 
	void Start () {

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().WithInvitationDelegate(OnInvitationReceived).Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		GameObject.Find("Multiplayer").GetComponent<MPMenuScript>().IsSignedInThanUpdateIt(Social.localUser.authenticated);
		SignIn();
		//PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(sInstance);
	}

	public List<Participant> getPlayers(){
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();

	}

	public Participant OtherPlayer(){
		
		List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
		participants.Remove(PlayGamesPlatform.Instance.RealTime.GetSelf());
		Participant Rival = participants[0];
		return Rival;
		}

	public Participant getParticipant(string  IdParticipante){

		return PlayGamesPlatform.Instance.RealTime.GetParticipant(IdParticipante);
	
	}

	public Participant getItself(){

		return PlayGamesPlatform.Instance.RealTime.GetSelf ();

	}

	public void SignIn(){
		Social.localUser.Authenticate((bool success) => {
			
			if (success) 
				{
					GameObject.Find("Multiplayer").GetComponent<MPMenuScript>().IsSignedInThanUpdateIt(true);
					
				}
			else
				{
				     GameObject.Find("Multiplayer").GetComponent<MPMenuScript>().IsSignedInThanUpdateIt(false);
				}

		});
	}

	public void obtenerInvitaciones()
	{
		getInvitations();
	}
	private void getInvitations(){
	PlayGamesPlatform.Instance.RealTime.GetAllInvitations(
        (invites) =>
        {
            Debug.Log("Got " + invites.Length + " invites");
            string logMessage = "";
            foreach(Invitation invite in invites)
            {
                logMessage += " " + invite.InvitationId + " (" +
                    invite.InvitationType + ") from " +
                    invite.Inviter + "\n";
                if (mFirstInvite == null) {
                    mFirstInvite = invite;
                }
            }
            Debug.Log(logMessage);
    });
	}

	public void FindMatch(){
	
		sInstance = new MultiplayerConnection ();
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(1, 1, 0, sInstance);
		GameObject.Find("Log").GetComponent<Text>().text = "Buscando partida";
		
	
	}

	public void OnRoomConnected(bool IsConected)
	{
		if (IsConected) 
		{
			//EMPEZAR JUEGO
			SceneManager.LoadScene("Prueba");
			Players = getPlayers();
			MpOrganizer.FirstPlayerID = getPlayers().First().ParticipantId;
			MpOrganizer.SecondPlayerID = getPlayers().Last().ParticipantId;
			MpOrganizer.FirstPlayerName = getPlayers().First().DisplayName;
			MpOrganizer.SecondPlayerName = getPlayers().Last().DisplayName;
		}
		else
		{
			//
			PlayGamesPlatform.Instance.RealTime.LeaveRoom();
		}
	
	}


	public void OnRoomSetupProgress(float porcentage)
	{
		if (!showingWaitingRoom) {
            showingWaitingRoom = true;
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        }
			
	}

	public void OnPeersConnected(string[] participantIds) {
       foreach (string ParticipantId in participantIds)
	   {
	   	// Ese ParticipantId esta conectado a la sala
	   }

    }
    public void OnPeersDisconnected(string[] participantIds) {
          foreach (string ParticipantId in participantIds)
	   {
	   	// Ese ParticipantId esta desconectado de la sala
	   }
    }
	
	public void OnLeftRoom()
	{
		//DEJO LA SALA DE ESPERA
	}
	
	public void OnParticipantLeft(Participant player)
	{

		//El jugador player dejo la sala 
	}

	public void SignOut()
	{
		PlayGamesPlatform.Instance.SignOut();
		GameObject.Find("Multiplayer").GetComponent<MPMenuScript>().IsSignedInThanUpdateIt(false);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			SceneManager.LoadScene("Menu");
		//	SceneManager.UnloadSceneAsync("Prueba");
		}			
		
	}

	public static MultiplayerConnection Instance{	
		get 
		{
			return sInstance;		
		}	
	}

	public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept=false) {
        GameObject.Find("Invitacion").GetComponent<Text>().text = "en el onInvitationRecieved";
		if (shouldAutoAccept) {
            // Invitation should be accepted immediately. This happens if the user already
            // indicated (through the notification UI) that they wish to accept the invitation,
            // so we should not prompt again.
            //ShowWaitScreen();
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, sInstance);
        } else {
            // The user has not yet indicated that they want to accept this invitation.
            // We should *not* automatically accept it. Rather we store it and 
            // display an in-game popup:
            mIncomingInvitation = invitation;
			GameObject.Find("Invitacion").GetComponent<Text>().text = invitation.InvitationId.ToString();
        }
    }

	void OnGUI() {
        if (mIncomingInvitation != null) {
            // show the popup
			GameObject.Find("Invitacion").GetComponent<Text>().text = "Invitacion Recibida";
            string who = (mIncomingInvitation.Inviter != null && 
                mIncomingInvitation.Inviter.DisplayName != null) ?
                    mIncomingInvitation.Inviter.DisplayName: " A player,";
            GUI.Label(labelRect, who + " is challenging you to a race!");
            if (GUI.Button(acceptButtonRect, "Accept!")) {
                // user wants to accept the invitation!
                //ShowWaitScreen();
                PlayGamesPlatform.Instance.RealTime.AcceptInvitation(mIncomingInvitation.InvitationId, sInstance);
				mIncomingInvitation = null;
            }
            if (GUI.Button(declineButtonRect, "Decline")) {
                // user wants to decline the invitation
                PlayGamesPlatform.Instance.RealTime.DeclineInvitation(mIncomingInvitation.InvitationId);
				mIncomingInvitation = null;
            }
		
		}
    }

	public void OnRealTimeMessageReceived(bool reliable, string senderId,byte[] data)
	{
		char Tipo = (char) data[0];
		if (Tipo == 'P')
		{
			Vector3 SecondPlayerNewPosition;
			float positionX =  System.BitConverter.ToSingle(data, 1);
			float positionY =  System.BitConverter.ToSingle(data, 5);
			float positionZ =  System.BitConverter.ToSingle(data, 9);
			SecondPlayerNewPosition = new Vector3(positionX,positionY,positionZ);
			GameObject.FindGameObjectWithTag("Player").gameObject.transform.position = SecondPlayerNewPosition;
			//SecondPlayer.GetComponent<MpSecondPlayer>().ReceivedPosition = SecondPlayerNewPosition;
		}
		if (Tipo == 'R')
		{
			Vector3 SecondPlayerNewRotation;
			float rotationX =  System.BitConverter.ToSingle(data, 1);
			float rotationY =  System.BitConverter.ToSingle(data, 5);
			float rotationZ =  System.BitConverter.ToSingle(data, 9);
			SecondPlayerNewRotation = new Vector3(rotationX,rotationY,rotationZ);
			GameObject.FindGameObjectWithTag("Player").transform.eulerAngles = SecondPlayerNewRotation;
		}
	}

}
