using System;
using System.Net;
using System.Net.Sockets;
using Bjoy;
using System.Collections.Generic;

namespace NetWorklib
{
     
#if UNITY_STANDALONE_WIN
    public enum ERROR_TYPE
    {
        WOULDBLOCK = 10035,
        ALREADY = 10037
    }
#else
    public enum ERROR_TYPE
    {
        WOULDBLOCK = 10035,
        ALREADY = 10037
    }
#endif


    public class SocketHandler 
    {
		static private SocketHandler handle = null;
		static public SocketHandler Instence
		{
			get
			{
				if (handle == null) 
				{
					handle = new SocketHandler();
				}

				return handle;
			}

		}

		public static SocketHandler CreateHandle(int nHandleId)
		{
			SocketHandler handle = new SocketHandler (nHandleId);
			handle.Init ();
			return handle;
		}


        /// <summary>
        /// soket 当前状态
        /// </summary>
        public enum CONN_STATUS
        {
            CONN_IDLE,
            CONN_FATAL_ERROR = -1,
            CONN_DATA_ERROR = -2,
            CONN_CONNECTING = -3,
            CONN_DISCONNECT = -4,
            CONN_CONNECTED = -5,
            CONN_DATA_SENDING = -6,
            CONN_DATA_RECVING = -7,
            CONN_SEND_DONE = -8,
            CONN_RECV_DONE = -9,
            CONN_APPEND_SENDING = -10,
            CONN_APPEND_DONE = -11,
            CONN_XML_POLICY = -12
        };

		public SocketHandler()          
		{
			_nHandleID = 0;
			_socket = null;
			endPoint = null;
			_writeBuf = new Buffer();
			_readBuf = new Buffer();
			_readStatus = CONN_STATUS.CONN_DISCONNECT;
			_writeStatus = CONN_STATUS.CONN_DISCONNECT;

			_outLock = new object ();
			//_inLock = new object ();

			Debuger.LogError("SocketHandler构造函数");
		}
        /// <summary>
        /// function
        /// </summary>
		public SocketHandler(int nHandlID):this()
        {
			_nHandleID = nHandlID;
			//Debuger.LogError("SocketHandler构造函数2");
        }

        public bool Init()
        {
            _decoder = CreateDecoder();
			_decoder.Init ();
            return true;
        }

        public bool IsConnected
        {
			get { 
				if (_socket != null)
					return _socket.Connected;
				return false;
			}
            
        }

        public bool Connect(string strHost, Int32 nPort)
        {
            try
            {
				//Debuger.LogError(string.Format("SocketHandler:Connect函数step1 {0}:{1}",strHost,nPort));
				if (_readStatus == CONN_STATUS.CONN_DISCONNECT || _readStatus == CONN_STATUS.CONN_IDLE)
                {
                    IPAddress ipadd = ParseIdAndHost(strHost);
                    endPoint = new IPEndPoint(ipadd, nPort);
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(endPoint, new AsyncCallback(_OnConnCallBack), socket);
                    _readStatus = CONN_STATUS.CONN_CONNECTING;
					//Debuger.LogError(string.Format("SocketHandler:Connect函数step2 {0}:{1}",strHost,nPort));
                }
            }
            catch (SocketException e)
            {
				Debuger.LogError(" socket errno:" + e.SocketErrorCode);
                return false;
            }
            catch (Exception e)
            {
                /// error log
				Debuger.LogError("connect error msg:" + e.Message);
                return false;
            }

            return true;
        }

        public bool Reconnect()
        {
            try
            {
                if (_readStatus == CONN_STATUS.CONN_DISCONNECT || _readStatus == CONN_STATUS.CONN_IDLE)
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(endPoint, new AsyncCallback(_OnConnCallBack), socket);
                    _readStatus = CONN_STATUS.CONN_CONNECTING;
                }
            }
            catch (Exception e)
            {
                /// error log
                Debuger.LogError("ReConnect error msg:" + e.Message);
                return false;
            }

            return true;
        }


        public void Close()
        {
            OnClose();

            if(_socket.Connected)
                _socket.Close();
            _readStatus = CONN_STATUS.CONN_DISCONNECT;
            _writeStatus = CONN_STATUS.CONN_DISCONNECT;
        }	

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="nIndex"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public int Send(byte[] buf, int nIndex,int nLen)
        {	
            try
            {   	
				lock(_outLock)
				{
					_writeBuf.Append (buf, nIndex, nLen);
				}

				if (_writeStatus == CONN_STATUS.CONN_SEND_DONE || _writeStatus == CONN_STATUS.CONN_CONNECTED)
				{
					_writeStatus = CONN_STATUS.CONN_DATA_SENDING;
					_socket.BeginSend(_writeBuf.Data, 0, _writeBuf.DataSize, SocketFlags.None, new AsyncCallback(_OnSendCallBack) , this);

				}
				else 
				{
					///sock还正在发送数据
					Debuger.Log("socket is sending data ! channelId: " + m_nHandleID );
				}	

				return nLen;
            }
            catch (SocketException e)
            {
				Debuger.LogError("connect  SocketErrorCode:" + e.SocketErrorCode);
            }
            catch (Exception e)
            {
                _readStatus = CONN_STATUS.CONN_DISCONNECT;
				Debuger.LogError("Exception:" + e.Message);
            }


            return -1;
        }

        public virtual Int32 OnConnected()
        {
			Debuger.Log("SocketHandler:OnConnected");
			SocketManager.Instance.RegistSocketHandle (this);
			return _decoder.OnSocketConnected (this);
        }

		public virtual int OnComplete(byte[] buf,int nIndex,int nLen)
		{
			Debuger.Log("SocketHandler:OnComplete");
			return _decoder.OnPacketComplete (buf, nIndex, nLen, this);
		}

        public virtual Int32 OnClose()
        {
			Debuger.Log("SocketHandler:OnClose");
			_decoder.OnSocketClose (this);
			SocketManager.Instance.UnRegistSocketHandl (_nHandleID,this);
			return 0;
        }


        /// <summary>
        /// property
        /// </summary>


        

        public string GetIpPort()
        {
            if(endPoint != null)
                return endPoint.ToString();
            return "";
        }

        protected virtual CDecoder CreateDecoder()
        {
            return new CDecoder();
        }

        private bool handle_input()
        {
            try
            {
                if (_readStatus == CONN_STATUS.CONN_CONNECTED || _readStatus == CONN_STATUS.CONN_RECV_DONE)
                {
					byte[] buf = new byte[16*1024];
					_readStatus = CONN_STATUS.CONN_DATA_RECVING;
                    _socket.BeginReceive(buf, 0,buf.Length, SocketFlags.None, new AsyncCallback(_OnRecvCallBack), buf);
                    return true;
                }
            }
            catch (SocketException e)
            {
                _readStatus = CONN_STATUS.CONN_DISCONNECT;
                Console.Write("connect  SocketErrorCode:" + e.SocketErrorCode);
                
            }
            catch (Exception e)
            {

                _readStatus = CONN_STATUS.CONN_DISCONNECT;
                Console.Write("Exception:" + e.Message);
            }

            if (_readStatus == CONN_STATUS.CONN_DISCONNECT)
            {
                Close();
                _socket.Close();
                
                _socket = null;
            }
            return false;
        }
	

        private void _OnConnCallBack(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            try
            {                
                ///通知异步线程已经链接完成完成
                s.EndConnect(ar);
				//Debuger.LogError(string.Format("_OnConnCallBack {0}",s.Connected));
                if (s.Connected)
                {
                    _socket = s;
                    _readStatus = CONN_STATUS.CONN_CONNECTED;
                    _writeStatus = CONN_STATUS.CONN_CONNECTED;
                    this.Init();
					//Debuger.LogError(string.Format("this.OnConnected() {0}",this.OnConnected()));
                    if (this.OnConnected() >= 0)
                    {
                        handle_input();
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    Close();
                }
            }
            catch (SocketException e)
            {
                Debuger.LogError("connect  SocketErrorCode:" + e.SocketErrorCode);
				Close();
            }
            catch (Exception e)
            {
                _readStatus = CONN_STATUS.CONN_DISCONNECT;
				Debuger.LogError("Exception:"+e.Message);
				Close();
            }
        }

        private void _OnRecvCallBack(IAsyncResult ar)
        {
            byte[] buf = (byte[])ar.AsyncState;

            try
            {
                ////接收到的字节数
                int recvLen = _socket.EndReceive(ar);
				Debuger.Log("recvLen:"+recvLen);
                if (recvLen > 0)
                {            
					///有缓冲数据，需要拼包
					if(_readBuf.DataSize > 0)
					{
						_readBuf.Append(buf,0,recvLen);

						byte[] tempBuf = _readBuf.Data;
						int nTempLen = _readBuf.DataSize;
						int nIndex = 0;



						///处理数据包
						while(nTempLen > 0)
						{
							int ret = _decoder.OnParserPacket(tempBuf, nIndex, nTempLen);
							if (ret > 0)
							{
								if (OnComplete(buf, nIndex, ret) >= 0)
								{
									nTempLen -= ret;
									nIndex += ret;
									_readStatus = CONN_STATUS.CONN_DATA_RECVING;
								}
								else
								{
									_readStatus = CONN_STATUS.CONN_DATA_ERROR;
									break;
								}
							}
							else if(ret == 0)
							{
								///继续接收数据
								_readBuf.Skip(nIndex);
								_readStatus = CONN_STATUS.CONN_DATA_RECVING;
								//_socket.BeginReceive(buf, 0,buf.Length, SocketFlags.None, new AsyncCallback(_OnRecvCallBack), buf);
								break;
							}
							else
							{
								///数据处理失败，需要断开
								_readStatus = CONN_STATUS.CONN_DATA_ERROR;
								break;
							}
						}

					}
					else{ 
						///
						byte[] tempBuf = buf;
						int nTempLen = recvLen;
						int nIndex = 0;
						while (nTempLen > 0)
						{
							int ret = _decoder.OnParserPacket(buf, nIndex, nTempLen);
							if (ret > 0)
							{
								if (OnComplete(buf, nIndex, ret) >= 0)
								{
									nTempLen -= ret;
									nIndex += ret;
								}
								else
								{
									_readStatus = CONN_STATUS.CONN_DATA_ERROR;
									break;
								}
							}
							else if(ret == 0 )
							{
								///继续接收数据
								_readStatus = CONN_STATUS.CONN_DATA_RECVING;
								_readBuf.Append(tempBuf,nIndex,nTempLen);
								//_socket.BeginReceive(buf, 0,buf.Length, SocketFlags.None, new AsyncCallback(_OnRecvCallBack), buf);
								break;
							}
							else
							{
								///数据处理失败，需要断开
								_readStatus = CONN_STATUS.CONN_DATA_ERROR;
								break;
							}
						}
					}
					

					if(_readStatus == CONN_STATUS.CONN_DATA_RECVING)
					{
						_socket.BeginReceive(buf, 0,buf.Length, SocketFlags.None, new AsyncCallback(_OnRecvCallBack), buf);
					}

                }
                else
                {
                    _readStatus = CONN_STATUS.CONN_DATA_ERROR;
                }
            }
            catch (SocketException e)
            {
                _readStatus = CONN_STATUS.CONN_DATA_ERROR;
                Console.Write("recv  SocketErrorCode:" + e.SocketErrorCode);
            }
            catch (Exception e)
            {
                _readStatus = CONN_STATUS.CONN_DATA_ERROR;
                Console.Write("Exception:" + e.Message);
            }

			if (_readStatus == CONN_STATUS.CONN_DATA_ERROR)
            {
                _readBuf.Skip(_readBuf.DataSize);
                Close(); 
            }
                      
        }
	

        private void _OnSendCallBack(IAsyncResult ar)
        {
            SocketHandler handler = (SocketHandler)ar.AsyncState;
            try
            {
                int nLen = handler._socket.EndSend(ar);

				Debuger.Log("send len:" + nLen);
                ////发送的字节数
                if (nLen > 0)
                {
					lock(_outLock)
					{
						_writeBuf.Skip(nLen);
					}

                    if (_writeBuf.DataSize > 0)
                    {
						_socket.BeginSend(_writeBuf.Data
						                  , 0
						                  , _writeBuf.DataSize
						                  , SocketFlags.None
						                  , new AsyncCallback(_OnSendCallBack)
						                  , handler);

						Debuger.Log("write buf size > 0 when OnSendCallBack ! bufsize:" + _writeBuf.DataSize);
                    }
					else{
						_writeStatus = CONN_STATUS.CONN_SEND_DONE;
					}
                    
				}
            }
            catch (SocketException e)
            {
                Console.Write("send  SocketErrorCode:" + e.SocketErrorCode);
            }
            catch (Exception e)
            {
                _readStatus = CONN_STATUS.CONN_DISCONNECT;
                Console.Write("Exception:" + e.Message);
            }         
        }

        private IPAddress ParseIdAndHost(string host)
        {
            IPAddress add;
            try{
                add = IPAddress.Parse(host);
                return add;
            }
            catch(Exception e)
            {
                Console.Write("IPAddress ip parse err:" + e.Message);

                try
                {
                    IPAddress[] adds = Dns.GetHostAddresses(host);
                    return adds[0];
                }
                catch (Exception ee) 
                {
                    Console.Write("Dns parse err:" + ee.Message);
                }
            }

            return null;
        }


        private Int32 _nHandleID;
        public Int32 m_nHandleID
        {
			get { return _nHandleID; }
			set { _nHandleID = value; }
        }

		public CONN_STATUS m_WriteStatus{
			get { return _writeStatus; }
		}

        System.Net.IPEndPoint endPoint;
        private Socket _socket;
        private Buffer _writeBuf;
        private Buffer _readBuf;
        private CDecoder _decoder;
        private CONN_STATUS _readStatus;
        private CONN_STATUS _writeStatus; 

		private System.Object _outLock;
		//private System.Object _inLock;
    }
}
