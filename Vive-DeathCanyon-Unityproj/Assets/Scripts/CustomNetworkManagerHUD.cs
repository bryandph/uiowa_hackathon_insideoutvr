#if ENABLE_UNET

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class CustomNetworkManagerHUD : MonoBehaviour
	{
		public float scale;
		public int spacing;
		public NetworkManager manager;
		private GUIStyle labelStyle;
		private GUIStyle buttonStyle;
		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;

		bool showServer = false;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			if (!showGUI)
				return;

			labelStyle = new GUIStyle ();
			labelStyle.fontSize = 20;
			labelStyle.normal.textColor = Color.white;

			buttonStyle = new GUIStyle ("button");
			buttonStyle.fontSize = 20;

			GUI.skin.textField.fontSize = 50;
			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			//int spacing = 24;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "LAN Host(H)", buttonStyle))
				{
					manager.StartHost();
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 105*scale, 20*scale), "LAN Client(C)", buttonStyle))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos + 100*(scale) + 20, ypos, 95*scale, 20*scale), manager.networkAddress);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "LAN Server Only(S)", buttonStyle))
				{
					manager.StartServer();
				}
				ypos += spacing;
			}
			else
			{
				if (NetworkServer.active)
				{
					//GUI.Label(new Rect(0, ypos, 300*scale, 20*scale), "Server: port=" + manager.networkPort, style);
					//ypos += spacing;
				}
				if (NetworkClient.active)
				{
					//GUI.Label(new Rect(0, ypos, 300*scale, 20*scale), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort, style);
					//ypos += spacing;
				}
			}

			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "Client Ready"))
				{
					ClientScene.Ready(manager.client.connection);
				
					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += spacing;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (GUI.Button(new Rect(0, 0, 50*scale, 20*scale), "Stop (X)"))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}

			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null)
				{
					if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "Enable Match Maker (M)", buttonStyle))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "Create Internet Match"))
							{
								//manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
								manager.matchMaker.CreateMatch (manager.matchName, manager.matchSize, true, "", "", "", 1, 1, manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, 100*scale, 20*scale), "Room Name:", labelStyle);
							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, 100*scale, 20*scale), manager.matchName);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "Find Internet Match"))
							{
								//manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
								manager.matchMaker.ListMatches (0, 20, "", true, 1, 1, manager.OnMatchList);
							}
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "Join Match:" + match.name))
								{
									manager.matchName = match.name;
									manager.matchSize = (uint)match.currentSize;
									//manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
									manager.matchMaker.JoinMatch (match.networkId, "", "", "", 1, 1, manager.OnMatchJoined);
								}
								ypos += spacing;
							}
						}
					}

					if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "Change MM server"))
					{
						showServer = !showServer;
					}
					if (showServer)
					{
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100*scale, 20*scale), "Local"))
						{
							manager.SetMatchHost("localhost", 1337, false);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100*scale, 20*scale), "Internet"))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100*scale, 20*scale), "Staging"))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
					}

					ypos += spacing;

					GUI.Label(new Rect(xpos, ypos, 300*scale, 20*scale), "MM Uri: " + manager.matchMaker.baseUri, labelStyle);
					ypos += spacing;

					if (GUI.Button(new Rect(xpos, ypos, 200*scale, 20*scale), "Disable Match Maker"))
					{
						manager.StopMatchMaker();
					}
					ypos += spacing;
				}
			}
		}
	}
};
#endif //ENABLE_UNET
