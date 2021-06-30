using UnityEngine;
using System.Collections;
using NetWorklib;
using System.Collections.Generic;
using Bjoy;

namespace BjoyProto
{
	public class BjoyNetMain : MonoBehaviour
	{
		public static BjoyNetMain m_sInstance;
	//public List <BjoyProto.SyncDataRespone> PlayersData = new List<BjoyProto.SyncDataRespone>();

		public int m_nChannelID=0;
		public string m_strIp="";
		public int m_nPort=0;

		void Awake()
		{
			m_sInstance = this;
		}

		// Use this for initialization
		void Start () {
			m_sInstance.m_nChannelID = 1;
			RegistNetCallback(false);
			BjoyNetMain.MakeConnect(1,"10.10.10.108", 4002);
		}
		
		// Update is called once per frame
		void Update () {
			if(m_nChannelID>0)
			{
				SocketManager.Instance.DoRecvData(m_nChannelID);
			}
		}

		void OnDestroy() {
			print("Script was destroyed");
			if(m_nChannelID>0)
			{
				CloseConnect(m_nChannelID);
			}
		}

		public static void RegistNetCallback(bool bGameServer){
			///注册事件处理方法
			PacketManager.Instence.RegistDoProcess (0x10, OnClose);
			PacketManager.Instence.RegistDoProcess (0x11, OnConnected);
			// 登录服还是游戏服
			if(bGameServer) 
			{
				PacketManager.Instence.RegistDoProcess (0x102, OnMachineHeartbeatRespone);
				PacketManager.Instence.RegistDoProcess (0x103, OnUserLoginResponse);
				PacketManager.Instence.RegistDoProcess (0x104, OnUserLogoutRespone);
				PacketManager.Instence.RegistDoProcess (0x105, OnMachineLoginResponse);
				PacketManager.Instence.RegistDoProcess (0x106, OnSyncDataResponse);
				PacketManager.Instence.RegistDoProcess (0x107, OnUpdateUserDataResponse);
				PacketManager.Instence.RegistDoProcess (0x108, OnUserBuyNuJianResponse);
				///注册解析数据包方法
				PacketManager.Instence.RegistParserProcess (0x102, CDecoder.ProcessParserPacket<BjoyProto.MachineHeartbeatRespone>);
				PacketManager.Instence.RegistParserProcess (0x103, CDecoder.ProcessParserPacket<BjoyProto.UserLoginResponse>);
				PacketManager.Instence.RegistParserProcess (0x104, CDecoder.ProcessParserPacket<BjoyProto.UserLogoutRespone>);
				PacketManager.Instence.RegistParserProcess (0x105, CDecoder.ProcessParserPacket<BjoyProto.MachineLoginResponse>);
				PacketManager.Instence.RegistParserProcess (0x106, CDecoder.ProcessParserPacket<BjoyProto.SyncDataResponse>);
				PacketManager.Instence.RegistParserProcess (0x107, CDecoder.ProcessParserPacket<BjoyProto.UpdateUserDataResponse>);
				PacketManager.Instence.RegistParserProcess (0x108, CDecoder.ProcessParserPacket<BjoyProto.UserBuyNuJianResponse>);

			}
			else
			{
				PacketManager.Instence.RegistDoProcess (0x101, OnMachineLoginRsp);
				PacketManager.Instence.RegistParserProcess (0x101, CDecoder.ProcessParserPacket<BjoyProto.MachineLoginRsp>);
			}
		}
		
		public static bool MakeConnect(int nHandleId,string host,int port){
			bool connected = SocketManager.Instance.Connect(nHandleId,host,port);
			return connected;
		}

		public static void CloseConnect(int nHandleId){
			SocketManager.Instance.Disconnect(nHandleId);
		}

		public static int OnClose(DataUnit dataUnit)
		{
			Debuger.Log(string.Format("BjoyNetMain:OnClose m_nChannelID={0} : m_nCmd={1}",dataUnit.m_nChannelID,dataUnit.m_nCmd));
			if(dataUnit.m_nChannelID==1)
			{
				m_sInstance.m_nChannelID=2;
				RegistNetCallback(true);
				MakeConnect(2,m_sInstance.m_strIp,m_sInstance.m_nPort);
			}
			else 
			{

			}
			return 0;
		}

		public static int OnConnected(DataUnit dataUnit)
		{
			Debuger.Log(string.Format("BjoyNetMain:OnConnected m_nChannelID={0} : m_nCmd={1}",dataUnit.m_nChannelID,dataUnit.m_nCmd));
			if(dataUnit.m_nChannelID==1)
			{
				SendMachineLoginReq();
			}
			else 
			{
				SendMachineLoginRequest();
				SendUserLoginRequest();
			}
//			SendMachineHeartbeatRequest();
//			SendMachineLoginRequest();
//			SendUserLoginRequest();
//			SendMachineLoginRequest();
//			SendUserLogoutRequest();
//			SendSyncDataRequest();
//			SendUpdateUserDataRequest();
//			SendUserBuyNuJianRequest();
			return 0;
		}

		public static int OnMachineLoginRsp(DataUnit dataUnit)
		{
			Debuger.Log("OnMachineLoginRsp m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.MachineLoginRsp) )
				{
					BjoyProto.MachineLoginRsp rsp = (BjoyProto.MachineLoginRsp)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.MachineLoginRsp {0} : {1}: {2}: {3}", rsp.m_nMachineID ,rsp.m_nHostID, rsp.m_strProxyIp, rsp.m_nProxyPort));

					m_sInstance.m_strIp=rsp.m_strProxyIp;
					m_sInstance.m_nPort=rsp.m_nProxyPort;
					CloseConnect(1);
				}
			}
			
			return 0;
		}
		
		public static int OnMachineHeartbeatRespone(DataUnit dataUnit)
		{
			Debuger.Log("OnMachineHeartbeatRespone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.MachineHeartbeatRespone) )
				{
					BjoyProto.MachineHeartbeatRespone rsp = (BjoyProto.MachineHeartbeatRespone)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.MachineHeartbeatRespone {0}", rsp.m_nSeq));
				}
			}
			
			return 0;
		}
		
		public static int OnUserBuyNuJianResponse(DataUnit dataUnit)
		{
			Debuger.Log("OnUserBuyNuJianResponse m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.UserBuyNuJianResponse) )
				{
					BjoyProto.UserBuyNuJianResponse rsp = (BjoyProto.UserBuyNuJianResponse)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.UserBuyNuJianResponse {0}:{1}:{2}", rsp.m_strCardID,rsp.m_nCoins,rsp.m_nNuJianShuLiang));
				}
			}
			
			return 0;
		}
		
		public static int OnUserLoginResponse(DataUnit dataUnit)
		{
			Debuger.Log("OnUserLoginResponse m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.UserLoginResponse) )
				{
					BjoyProto.UserLoginResponse rsp = (BjoyProto.UserLoginResponse)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.UserLoginResponse {0}:{1}", rsp.m_nRet,rsp.m_strUserCardId));
				}
			}
			
			return 0;
		}

		public static int OnUserLogoutRespone(DataUnit dataUnit)
		{
			Debuger.Log("OnUserLogoutRespone m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.UserLogoutRespone) )
				{
					BjoyProto.UserLogoutRespone rsp = (BjoyProto.UserLogoutRespone)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.UserLogoutRespone {0}:{1}", rsp.m_nRet,rsp.m_strUserCardId));
				}
			}
			
			return 0;
		}

		public static int OnMachineLoginResponse(DataUnit dataUnit)
		{
			Debuger.Log("OnMachineLoginResponse m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.MachineLoginResponse) )
				{
					BjoyProto.MachineLoginResponse rsp = (BjoyProto.MachineLoginResponse)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.MachineLoginResponse {0}:{1}:{2}", rsp.m_nRet,rsp.m_nHostId,rsp.m_strKey));
				}
			}
			
			return 0;
		}
		
		public static int OnSyncDataResponse(DataUnit dataUnit)
		{
			Debuger.Log("OnSyncDataResponse m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.SyncDataResponse) )
				{
					BjoyProto.SyncDataResponse rsp = (BjoyProto.SyncDataResponse)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.SyncDataResponse {0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", rsp.m_mUserData.m_nUserID, rsp.m_mUserData.m_nUserCoins,rsp.m_mUserData.m_nUserIntegrals,rsp.m_mXunLongData.m_nLevel,rsp.m_mXunLongData.m_nExp,rsp.m_mXunLongData.m_nNuJian,rsp.m_mXunLongData.m_nNu,rsp.m_mXunLongData.m_nLinPian));

					//	GameObject.Find ("Script").GetComponent<GameManager> ().Players[i].LongCoinnum = rsp.m_mUserData.m_nUserCoins;
					//	GameObject.Find ("Script").GetComponent<GameManager> ().Players[i].Hooknum =rsp.m_mXunLongData.m_nNuJian;
					//	GameObject.Find ("Script").GetComponent<GameManager> ().Players[i].m_score =rsp.m_mUserData.m_nUserIntegrals;

					//这里需要修改
					//GameManager.Instance.PlayersData.Add(rsp);
					//GameManager.Instance.initPlayer();
					//GameManager.Instance.initSelectedPlayer();

				//	GameManager.Instance.Players[0].LongCoinnum = rsp.m_mUserData.m_nUserCoins;
				//	GameManager.Instance.Players[0].Hooknum =rsp.m_mXunLongData.m_nNuJian;
			
				}
			}
			
			return 0;
		}
		
		public static int OnUpdateUserDataResponse(DataUnit dataUnit)
		{
			Debuger.Log("OnUpdateUserDataResponse m_nCmd="+dataUnit.m_nCmd);
			if (dataUnit.m_oBodyUnit != null) {
				if(dataUnit.m_oBodyUnit.GetType() == typeof( BjoyProto.UpdateUserDataResponse) )
				{
					BjoyProto.UpdateUserDataResponse rsp = (BjoyProto.UpdateUserDataResponse)dataUnit.m_oBodyUnit;
					Debuger.Log(string.Format("BjoyProto.UpdateUserDataResponse {0} : {1}", rsp.m_mUpdataRet.Count, rsp.m_mUpdataRet.ToString()));
					for(int i=0;i<rsp.m_mUpdataRet.Count;i++)
					{
						BjoyProto.UpdateUserDataResponse.UpdateRet t=rsp.m_mUpdataRet[i];
						Debuger.Log(string.Format("i={0} : {1}: {2}", i, t.m_nRet,t.m_strCardID));

					}
				}
			}
			
			return 0;
		}
		
		public static void SendMachineLoginReq()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x101;
			dataUnit1.m_nChannelID = 1;
			BjoyProto.MachineLoginReq req = new BjoyProto.MachineLoginReq();
			req.m_strUUID = "han123456";
			req.m_strMacIP = "E0-DB-55-CC-84-51";
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<BjoyProto.MachineLoginReq>(ms,req);  
			byte[] result = ms.ToArray();  
			ms.Close();
			Debuger.Log("BjoyProto.MachineLoginReq长度："+ result.Length);
		}

		public static void SendMachineHeartbeatRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x102;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.MachineHeartbeatRequest req = new BjoyProto.MachineHeartbeatRequest();
			req.m_nSeq = 123456;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<BjoyProto.MachineHeartbeatRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.MachineHeartbeatRequest长度："+ result.Length);
		}
		
		public static void SendUserBuyNuJianRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x108;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.UserBuyNuJianRequest req = new BjoyProto.UserBuyNuJianRequest();
			req.m_strCardID = "111222";
			req.m_nGameType = 1;
			req.m_nTime =642;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<BjoyProto.UserBuyNuJianRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.UserBuyNuJianRequest长度："+ result.Length);
		}
		
		public static void SendUserLoginRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x103;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.UserLoginRequest req = new BjoyProto.UserLoginRequest();
			req.m_strUserCardId = "test1";//1~13
			req.m_nHostId =642;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<BjoyProto.UserLoginRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.UserLoginRequest长度："+ result.Length);
		}

		public static void SendUserLogoutRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x104;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.UserLogoutRequest req = new BjoyProto.UserLogoutRequest();
			req.m_strUserCardId = "999222";
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<BjoyProto.UserLogoutRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.UserLogoutRequest长度："+ result.Length);
		}

		public static void SendMachineLoginRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x105;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.MachineLoginRequest req = new BjoyProto.MachineLoginRequest();
			req.m_strUUID = "han123456";
			req.m_nGameType = 1;
			req.m_strMd5Key = "202cb962ac59075b964b07152d234b70";
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<BjoyProto.MachineLoginRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.MachineLoginRequest长度："+ result.Length);
		}
		
		public static void SendSyncDataRequest(string CardID)
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x106;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.SyncDataRequest req = new BjoyProto.SyncDataRequest();
		//	req.m_strUserCardID="111222";
			req.m_strUserCardID=CardID;
			req.m_nGameType = 1;
			dataUnit1.m_oBodyUnit = req;
			SocketManager.Instance.SendDataUnit(dataUnit1);
			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();  
			ProtoBuf.Serializer.Serialize<BjoyProto.SyncDataRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.SyncDataRequest长度："+ result.Length);
		}

		public static void SendUpdateUserDataRequest()
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x107;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.UpdateUserDataRequest req = new BjoyProto.UpdateUserDataRequest();
			BjoyProto.UpdateUserDataRequest.GameData t = new BjoyProto.UpdateUserDataRequest.GameData();

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
			ProtoBuf.Serializer.Serialize<BjoyProto.UpdateUserDataRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.UpdateUserDataRequest长度："+ result.Length);
		}



		public static void SendUpdateUserDataRequest1(GameManager obj)
		{
			DataUnit dataUnit1 =  new DataUnit();
			dataUnit1.m_nCmd = 0x107;
			dataUnit1.m_nChannelID = 2;
			BjoyProto.UpdateUserDataRequest req = new BjoyProto.UpdateUserDataRequest();
			BjoyProto.UpdateUserDataRequest.GameData t = new BjoyProto.UpdateUserDataRequest.GameData();
		     

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
			ProtoBuf.Serializer.Serialize<BjoyProto.UpdateUserDataRequest>(ms,req);  
			byte[] result = ms.ToArray();
			ms.Close();
			Debuger.Log("BjoyProto.UpdateUserDataRequest长度："+ result.Length);

		}
	

	}
}
