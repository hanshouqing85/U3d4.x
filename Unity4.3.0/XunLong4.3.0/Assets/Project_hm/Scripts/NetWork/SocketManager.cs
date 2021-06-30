
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using Bjoy;

namespace NetWorklib
{   


    public class SocketManager
    {
		static SocketManager __socketManager;
		static public SocketManager Instance
		{
			get{
				if(__socketManager == null) __socketManager = new SocketManager();
				return __socketManager;
			}
		}

		public SocketManager()
		{
			__SocketMap = new Dictionary<int, SocketHandler>();
		}

		public void RegistSocketHandle(SocketHandler socket)
		{
			if (__SocketMap.ContainsKey (socket.m_nHandleID)) {
				__SocketMap[socket.m_nHandleID].Close();
				__SocketMap[socket.m_nHandleID] = socket;
			} else {
				__SocketMap.Add(socket.m_nHandleID,socket);
			}
		}

		public void UnRegistSocketHandl(int nHandleId,SocketHandler socket)
		{
			if (__SocketMap.ContainsKey (nHandleId)) {
				if(__SocketMap[nHandleId].Equals(socket))
					__SocketMap.Remove(nHandleId);
			} 
		}

		public void DestroySocketHandle(int nHandleId)
		{
			if (__SocketMap.ContainsKey (nHandleId)){
				__SocketMap.Remove(nHandleId);
			}
		}


    	public bool DoWrite(int nHandleId)
		{
			return false;
		}


		public bool Connect(int nHandleId,string host,int port)
		{
			if (__SocketMap.ContainsKey (nHandleId)) {
				if (__SocketMap [nHandleId].IsConnected) {
					return true;
				} else {
					return __SocketMap [nHandleId].Connect (host, port);
				}
			} else {
				return  SocketHandler.CreateHandle (nHandleId).Connect (host, port);
			}
		}

		// add by hxh
		public SocketHandler GetSocketHandle(int nHandleId)
		{
			SocketHandler handle=null;
			if (__SocketMap.ContainsKey (nHandleId)) {
				handle=__SocketMap [nHandleId];
			} 
			return handle;
		}

		// add by hxh
		public void Disconnect(int nHandleId)
		{
			SocketHandler handle=null;
			if (__SocketMap.ContainsKey (nHandleId)) {
				handle=__SocketMap [nHandleId];
				if(handle.IsConnected){
					handle.Close();
				}
			} 
		}
		
		/// <summary>
		/// 发送数据 
		/// </summary>
		/// <returns>The data unit.</returns>
		/// <param name="dataUnit">Data unit.</param>
		public int SendDataUnit(DataUnit dataUnit)
		{
			SocketHandler sock = null;
			if ( __SocketMap.TryGetValue ( dataUnit.m_nChannelID , out sock ) ) {
				CCNetworkPacket packet =  CCNetworkPacket.CreatePacketByDataUnit(dataUnit);
				return sock.Send(packet.PacketBuf,0,packet.PacketSize);
			} 
			///
			Debuger.LogError (" socket not exist ! ChannelId :" + dataUnit.m_nChannelID);
			return -1;
		}

		/// <summary>
		/// 处理收到的数据包 ，本函数由UI线程调用.
		/// </summary>
		/// <param name="nChannelId">N channel identifier.</param>
		public void DoRecvData(int nChannelId)
		{
			UnityEngine.Profiler.BeginSample ("Network");
			PacketManager.Instence.DoInputPackage (nChannelId);
			UnityEngine.Profiler.EndSample ();
		}


		private Dictionary<int ,SocketHandler> __SocketMap ;

    }
}
	

