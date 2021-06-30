using UnityEngine;
using System.Collections;
using NetWorklib;
using System.Collections.Generic;

namespace Bjoy
{
	public class NetMain : MonoBehaviour
	{
		public static NetMain m_sInstance;

	//public List <Bjoy.SyncDataRespone> PlayersData = new List<Bjoy.SyncDataRespone>();

		//public int Playersnum=0;

		void Awake()
		{
			m_sInstance = this;
		}

		// Use this for initialization
		void Start () {
			//MakeConnect();
			//RegistNetCallback();
		}
		
		// Update is called once per frame
		void Update () {
			//SocketManager.Instance.DoRecvData(1);
		}

		void OnDestroy() {
			//print("Script was destroyed");
			//CloseConnect();
		}

		public void RegistNetCallback(){
			///注册事件处理方法
			PacketManager.Instence.RegistDoProcess (0x10, OnClose);
			PacketManager.Instence.RegistDoProcess (0x11, OnConnected);
			PacketManager.Instence.RegistDoProcess (0x101, OnMachineLoginRepone);
			PacketManager.Instence.RegistDoProcess (0x102, OnMachineHeartbeatRespone);
			PacketManager.Instence.RegistDoProcess (0x105, OnLoginRespone);
			PacketManager.Instence.RegistDoProcess (0x106, OnSyncDataRespone);
			PacketManager.Instence.RegistDoProcess (0x107, OnUpdateUserDataRespone);
			PacketManager.Instence.RegistDoProcess (0x108, OnUserBuyNuJianRespone);
			PacketManager.Instence.RegistDoProcess (0x1234,OnUserChat);
			///注册解析数据包方法
			PacketManager.Instence.RegistParserProcess (0x101, CDecoder.ProcessParserPacket<Bjoy.MachineLoginRepone>);
			PacketManager.Instence.RegistParserProcess (0x102, CDecoder.ProcessParserPacket<Bjoy.MachineHeartbeatRespone>);
			PacketManager.Instence.RegistParserProcess (0x105, CDecoder.ProcessParserPacket<Bjoy.LoginRespone>);
			PacketManager.Instence.RegistParserProcess (0x106, CDecoder.ProcessParserPacket<Bjoy.SyncDataRespone>);
			PacketManager.Instence.RegistParserProcess (0x107, CDecoder.ProcessParserPacket<Bjoy.UpdateUserDataRespone>);
			PacketManager.Instence.RegistParserProcess (0x108, CDecoder.ProcessParserPacket<Bjoy.UserBuyNuJianRespone>);
			PacketManager.Instence.RegistParserProcess (0x1234, CDecoder.ProcessParserPacket<chat.UserChat>);
		}
		
		public static bool MakeConnect(){
			bool connected = SocketManager.Instance.Connect(1, "10.10.10.108", 4000);
			return connected;	
		}

		public static void CloseConnect(){
			SocketManager.Instance.Disconnect(1);
		}

		public static int OnClose(DataUnit dataUnit)
		{
			Debuger.Log("NetMain:OnClose m_nCmd="+dataUnit.m_nCmd);
			return 0;
		}

		public static int OnConnected(DataUnit dataUnit)
		{
			Debuger.Log("NetMain:OnConnected m_nCmd="+dataUnit.m_nCmd);
//			SendUserChat();
//			SendMachineLoginRequest();
//			SendMachineHeartbeatRequest();
//			SendLoginRequest();
//			SendSyncDataRequest();
//			SendUpdateUserDataRequest();
//			SendUserBuyNuJianRequest();
			return 0;
		}

#if true
		public static int OnUserChat(DataUnit dataUnit)
		{
			Debuger.Log("OnProcessPacket m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( chat.UserChat ) )
				{
					chat.UserChat rsp = (chat.UserChat)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("chat.UserChat {0} : {1}", rsp.m_strNickName, rsp.m_strMessage));
				}
			}
			
			return 0;
		}

		public static void SendUserChat()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x1234;
			dataUnit1.m_nChannelID = 1;
			chat.UserChat req = new chat.UserChat();
			req.m_strNickName = "han韩";
			req.m_strMessage = "test";
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<chat.UserChat>(ms,req);  
			byte[] result = ms.ToArray();  
			ms.Close();
			Debuger.Log("chat.UserChat长度："+ result.Length);
		}
#endif

		public static int OnMachineLoginRepone(DataUnit dataUnit)
		{
			Debuger.Log("OnMachineLoginRepone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( Bjoy.MachineLoginRepone) )
				{
					Bjoy.MachineLoginRepone rsp = (Bjoy.MachineLoginRepone)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("Bjoy.MachineLoginRepone {0} : {1}: {2}: {3}", rsp.m_nMachineID ,rsp.m_nHostID, rsp.m_nProxyIp, rsp.m_nProxyPort));
				}
			}
			
			return 0;
		}
		
		public static int OnMachineHeartbeatRespone(DataUnit dataUnit)
		{
			Debuger.Log("OnMachineHeartbeatRespone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( Bjoy.MachineHeartbeatRespone) )
				{
					Bjoy.MachineHeartbeatRespone rsp = (Bjoy.MachineHeartbeatRespone)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("Bjoy.MachineHeartbeatRespone {0}", rsp.m_nSeq));
				}
			}
			
			return 0;
		}
		
		public static int OnUserBuyNuJianRespone(DataUnit dataUnit)
		{
			Debuger.Log("OnUserBuyNuJianRespone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( Bjoy.UserBuyNuJianRespone) )
				{
					Bjoy.UserBuyNuJianRespone rsp = (Bjoy.UserBuyNuJianRespone)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("Bjoy.UserBuyNuJianRespone {0}:{1}:{2}", rsp.m_strCardID,rsp.m_nCoins,rsp.m_nNuJianShuLiang));
				}
			}
			
			return 0;
		}
		
		public static int OnLoginRespone(DataUnit dataUnit)
		{
			Debuger.Log("OnLoginRespone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( Bjoy.LoginRespone) )
				{
					Bjoy.LoginRespone rsp = (Bjoy.LoginRespone)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("Bjoy.LoginRespone {0}", rsp.m_strKey));
				}
			}
			
			return 0;
		}
		
		public static int OnSyncDataRespone(DataUnit dataUnit)
		{
			Debuger.Log("OnSyncDataRespone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( Bjoy.SyncDataRespone) )
				{
					Bjoy.SyncDataRespone rsp = (Bjoy.SyncDataRespone)dataUnit.m_oBodyUnit;
				//	Debuger.Log(string.Format("Bjoy.SyncDataRespone {0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", rsp.m_mUserData.m_nUserID, rsp.m_mUserData.m_nUserCoins,rsp.m_mUserData.m_nUserIntegrals,rsp.m_mXunLongData.m_nLevel,rsp.m_mXunLongData.m_nExp,rsp.m_mXunLongData.m_nNuJian,rsp.m_mXunLongData.m_nNu,rsp.m_mXunLongData.m_nLinPian));

					//	GameObject.Find ("Script").GetComponent<GameManager> ().Players[i].LongCoinnum = rsp.m_mUserData.m_nUserCoins;
					//	GameObject.Find ("Script").GetComponent<GameManager> ().Players[i].Hooknum =rsp.m_mXunLongData.m_nNuJian;
					//	GameObject.Find ("Script").GetComponent<GameManager> ().Players[i].m_score =rsp.m_mUserData.m_nUserIntegrals;
				
//					GameManager.Instance.PlayersData.Add(rsp);
					GameManager.Instance.initPlayer();
					GameManager.Instance.initSelectedPlayer();
				//	GameManager.Instance.Players[0].LongCoinnum = rsp.m_mUserData.m_nUserCoins;
				//	GameManager.Instance.Players[0].Hooknum =rsp.m_mXunLongData.m_nNuJian;
			
				}
			}
			
			return 0;
		}
		
		public static int OnUpdateUserDataRespone(DataUnit dataUnit)
		{
			Debuger.Log("OnUpdateUserDataRespone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( Bjoy.UpdateUserDataRespone) )
				{
					Bjoy.UpdateUserDataRespone rsp = (Bjoy.UpdateUserDataRespone)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("Bjoy.UpdateUserDataRespone {0} : {1}", rsp.m_mUpdataRet.Count, rsp.m_mUpdataRet.ToString()));
					for(int i=0;i<rsp.m_mUpdataRet.Count;i++)
					{
						Bjoy.UpdateUserDataRespone.UpdateRet t=rsp.m_mUpdataRet[i];
						Debuger.Log(string.Format("i={0} : {1}: {2}", i, t.m_strCardID,t.status));

					}
				}
			}
			
			return 0;
		}
		
		public static void SendMachineLoginRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x101;
			dataUnit1.m_nChannelID = 1;
			Bjoy.MachineLoginRequest req = new Bjoy.MachineLoginRequest();
			req.m_strUUID = "han123456";
			req.m_strMacIP = "E0-DB-55-CC-84-51";
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<Bjoy.MachineLoginRequest>(ms,req);  
			byte[] result = ms.ToArray();  
			ms.Close();
			Debuger.Log("Bjoy.MachineLoginReques长度："+ result.Length);
		}
		
		public static void SendMachineHeartbeatRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x102;
			dataUnit1.m_nChannelID = 1;
			Bjoy.MachineHeartbeatRequest req = new Bjoy.MachineHeartbeatRequest();
			req.m_nSeq = 123456;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<Bjoy.MachineHeartbeatRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("Bjoy.MachineHeartbeatRequest长度："+ result.Length);
		}
		
		public static void SendUserBuyNuJianRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x108;
			dataUnit1.m_nChannelID = 1;
			Bjoy.UserBuyNuJianRequest req = new Bjoy.UserBuyNuJianRequest();
			req.m_strCardID = "111222";
			req.m_nGameType = 1;
			req.m_nTime =642;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<Bjoy.UserBuyNuJianRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("Bjoy.UserBuyNuJianRequest长度："+ result.Length);
		}
		
		public static void SendLoginRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x105;
			dataUnit1.m_nChannelID = 1;
			Bjoy.LoginRequest req = new Bjoy.LoginRequest();
			req.m_nMachineID = 999222;
			req.m_nHostID = 642;
			req.m_strUUID = "han123456";
			req.m_nGameType = 1;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<Bjoy.LoginRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("Bjoy.LoginRequest长度："+ result.Length);
		}
		
		public static void SendSyncDataRequest(string CardID)
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x106;
			dataUnit1.m_nChannelID = 1;
			Bjoy.SyncDataRequest req = new Bjoy.SyncDataRequest();
		//	req.m_strUserCardID="111222";
			req.m_strUserCardID=CardID;
			req.m_nGameType = 1;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<Bjoy.SyncDataRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("Bjoy.SyncDataRequest长度："+ result.Length);
		}

		public static void SendUpdateUserDataRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x107;
			dataUnit1.m_nChannelID = 1;
			Bjoy.UpdateUserDataRequest req = new Bjoy.UpdateUserDataRequest();
			Bjoy.UpdateUserDataRequest.GameData t = new Bjoy.UpdateUserDataRequest.GameData();

			t.m_strCardId = "111222";
			t.m_nIntegrals = 50;
			t.m_nExp = 100;
			t.m_nNuJian = 10;
			t.m_nLinPian = 200;

			req.m_mGameDatas.Add(t);

			req.m_nTime = 642;

			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<Bjoy.UpdateUserDataRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("Bjoy.UpdateUserDataRequest长度："+ result.Length);
		}



		public static void SendUpdateUserDataRequest1(GameManager obj)
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x107;
			dataUnit1.m_nChannelID = 1;
			Bjoy.UpdateUserDataRequest req = new Bjoy.UpdateUserDataRequest();
			Bjoy.UpdateUserDataRequest.GameData t = new Bjoy.UpdateUserDataRequest.GameData();
		     

			t.m_strCardId = obj.Players[0].PlayerID;
			t.m_nIntegrals = obj.Players[0].m_scoreadd;
			t.m_nExp = obj.Players[0].m_expadd;
			t.m_nNuJian = 	obj.Players[0].Hooknumadd;
			t.m_nLinPian = 200;


			print( "obj.Players[0].Hooknumadd="+obj.Players[0].Hooknumadd );
			print ("obj.Players[0].PlayerID="+obj.Players[0].PlayerID);
			print ("obj.Players[0].m_scoreadd="+obj.Players[0].m_scoreadd);

			req.m_mGameDatas.Add(t);
			
			req.m_nTime = 642;
			
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<Bjoy.UpdateUserDataRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("Bjoy.UpdateUserDataRequest长度："+ result.Length);

		}
	

	}
}
