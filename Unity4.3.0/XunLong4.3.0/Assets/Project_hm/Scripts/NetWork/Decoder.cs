using System;

namespace NetWorklib
{
    public class CDecoder
    {
		static public DataUnit ProcessParserPacket<T>(CCNetworkPacket package) where T: ProtoBuf.IExtensible
		{
			DataUnit dataUnit = new DataUnit ();
			
			///数据包长度
			dataUnit.m_nPacketLen = package.PacketSize;
			///数据包特殊标志位
			dataUnit.m_bCode = package.ReadCode ();
			///数据包 版本号
			dataUnit.m_bVS = package.ReadVersion ();
			///数据包序列号
			dataUnit.m_nSeq = package.ReadSeqNum ();
			///数据包 命令字
			dataUnit.m_nCmd = package.ReadCmd ();
			///数据包目标
			dataUnit.m_nDest = package.ReadDst ();
			///数据包源
			dataUnit.m_nSrc = package.ReadSrc ();
			
			
			
			ushort nBodyLen = package.ReadBodyLen ();
			if (nBodyLen > 0) {
				byte[] buf = package.GetBodyBufer ();
				try{
					dataUnit.m_oBodyUnit =  ProtoBuf.Serializer.Deserialize<T> (new System.IO.MemoryStream( buf));
				}catch(System.Exception ec)
				{
					UnityEngine.Debug.LogException(ec);
					UnityEngine.Debug.LogError("protobuf Deserialize faild ! cmd:"+dataUnit.m_nCmd);
					return null;
				}
			}
			
			return dataUnit;
		}

		protected enum PACKET_STATUS
		{
			PACKET_PARSER_ERR,
			PACKET_PARSER_HANDSHAKE,
			PACKET_PACKET_LEN,
			PACKET_PARSER_FLAGS,
			PAKCET_PARSER_VS,
			PACKET_PARSER_SEQ,
			PACKET_PARSER_CMD,
			PACKET_PARSER_DEST,
			PACKET_PARSER_SRC,
			PACKET_PARSER_BODY           
		}

        public void Init()
        {
            _readStatus = PACKET_STATUS.PACKET_PARSER_HANDSHAKE;
        }


        /// <summary>
        /// 数据包解析
        /// </summary>
        /// <param name="buf">网络到的数据</param>
        /// <param name="nLen">数据长度</param>
        /// <returns> 解析成功返回包长度，数据不够返回0，解析失败返回-1</returns>
        public virtual int OnParserPacket(byte[] buf ,Int32 nIndex, Int32 nLen)
        {
            if (_readStatus == PACKET_STATUS.PACKET_PARSER_ERR) 
				return -1;

            if (_readStatus == PACKET_STATUS.PACKET_PARSER_HANDSHAKE)
                _readStatus =  PACKET_STATUS.PACKET_PACKET_LEN;


            if (_readStatus == PACKET_STATUS.PACKET_PACKET_LEN)
            {
                if (nLen < (int)CCNetworkPacket.PACKET_POS.PACKAGE_HEADER_SIZE)
                {
                    ///数据接收未完成
                    return 0;
                }

                _readStatus++;
            }

            if (_readStatus == PACKET_STATUS.PACKET_PARSER_FLAGS)
            {
                if (buf[(int)CCNetworkPacket.PACKET_POS.PACKAGE_HEADER_FLAGS_POS + nIndex] != 'O'
                || buf[(int)CCNetworkPacket.PACKET_POS.PACKAGE_HEADER_FLAGS_POS + 1 + nIndex] != 'D')
                {
                    ///数据解析失败，底层将会断掉链接
                    return -1;
                }

                _readStatus++;
            }

            if (_readStatus == PACKET_STATUS.PAKCET_PARSER_VS)
            {
                if (buf[(int)CCNetworkPacket.PACKET_POS.PACKAGE_VS_POS + nIndex ] != 0x11)
                {
                    return -1;
                }

                _readStatus = PACKET_STATUS.PACKET_PARSER_BODY;
            }

            //if (_readStatus == PACKET_STATUS.PACKET_PARSER_SEQ) _readStatus++;

            //if (_readStatus == PACKET_STATUS.PACKET_PARSER_CMD) _readStatus++;
            //if (_readStatus == PACKET_STATUS.PACKET_PARSER_DEST) _readStatus++;
            //if (_readStatus == PACKET_STATUS.PACKET_PARSER_SRC) _readStatus++;
            if (_readStatus == PACKET_STATUS.PACKET_PARSER_BODY)
            {
                UInt16 packetLen = BitConverter.ToUInt16(buf, (int)CCNetworkPacket.PACKET_POS.PACKAGE_LEN_POS + nIndex);
                packetLen = (UInt16)System.Net.IPAddress.NetworkToHostOrder((Int16)packetLen);

				UnityEngine.Debug.Log ("nLen:"+nLen + "; packetLen:"+packetLen);
                ///数据包未读完
                if (nLen < packetLen) return 0;

                ///返回包长，底层将会删掉这个长度的数据
                _readStatus = PACKET_STATUS.PACKET_PACKET_LEN;
				            
				///解析成功，返回包长
                return packetLen;
            }
          
            return nLen;
        }

        /// <summary>
        /// 数据处理错误返回-1，处理成功返回0或者>0 
		/// </summary>
        /// <param name="buf">Buffer.</param>
        /// <param name="nIndex">N index.</param>
        /// <param name="nLen">N length.</param>
        public virtual int OnPacketComplete(byte[] buf, Int32 nIndex, Int32 nLen ,SocketHandler socket)
        {
            CCNetworkPacket package = new CCNetworkPacket (buf, nIndex, nLen);
			return PacketManager.Instence.ParserPacketProcess (package,socket);
		
        }

		public virtual int OnSocketConnected(SocketHandler socket)
		{
			DataUnit dataUnit = new DataUnit ();
			dataUnit.m_nChannelID = socket.m_nHandleID;
			dataUnit.m_nCmd = 0x11;
			PacketManager.Instence.AddInputData (dataUnit);
			return 0;
		}

		public virtual int OnSocketClose( SocketHandler socket)
		{
			DataUnit dataUnit = new DataUnit ();
			dataUnit.m_nChannelID = socket.m_nHandleID;
			dataUnit.m_nCmd = 0x10;
			PacketManager.Instence.AddInputData (dataUnit);
			return 0;
		}
        
        protected PACKET_STATUS _readStatus;
    }
}
