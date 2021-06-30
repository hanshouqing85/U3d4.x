
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NetWorklib
{
	
	public  class  CCNetworkPacket
	{	
		public	enum PACKET_POS
		{
			PACKAGE_SIZE = 8192,
			PACKAGE_HEADER_SIZE = 18,
			
			PACKAGE_LEN_POS = 0,
			PACKAGE_CODE_POS = 2,
			PACKAGE_VS_POS = 3,
			PACKAGE_SEQ_POS = 4,
			PACKAGE_SUB_CMD_POS = 6,
			PACKAGE_MAIN_CMD_POS = 7,
			PACKAGE_DEST_POS = 8,
			PACKAGE_SOURCE_POS = 12,
			PACKAGE_HEADER_FLAGS_POS = 16,
			PACKAGE_BODY_POS = 18
		};
		
		
		public static CCNetworkPacket CreatePacketByDataUnit(DataUnit dataUnit)
		{
			byte[] maske = new byte[2];
			maske [0] = 79;
			maske [1] = 68;
			if (dataUnit.m_oBodyUnit != null) {
				System.IO.MemoryStream mem = new System.IO.MemoryStream ();
				ProtoBuf.Serializer.Serialize(mem ,dataUnit.m_oBodyUnit);
				COutputPacket package = new COutputPacket();
				package.WriteCmd(dataUnit.m_nCmd);
				package.WriteCode(dataUnit.m_bCode);
				package.WriteVersion(dataUnit.m_bVS);
				package.WriteSeqNum(dataUnit.m_nSeq);
				package.WriteCmd(dataUnit.m_nCmd);
				package.WriteDst(dataUnit.m_nDest);
				package.WriteSrc(dataUnit.m_nSrc);
				package.WriteMask(maske);
				package.WriteBody(mem.GetBuffer(),0,(ushort)mem.Length);
				package.End();

				
				return package;
			} else {
				COutputPacket package = new COutputPacket();
				package.WriteCmd(dataUnit.m_nCmd);
				package.WriteCode(dataUnit.m_bCode);
				package.WriteVersion(dataUnit.m_bVS);
				package.WriteSeqNum(dataUnit.m_nSeq);
				package.WriteCmd(dataUnit.m_nCmd);
				package.WriteDst(dataUnit.m_nDest);
				package.WriteSrc(dataUnit.m_nSrc);
				package.WriteMask(maske);
				package.End();
				
				return package;
			}
		}	
		
		protected  int _ReadInt32(byte[] buf, int nPos)
		{
			int n = BitConverter.ToInt32(buf, nPos);
			return (int)System.Net.IPAddress.NetworkToHostOrder((int)n);
		}
		
		protected ushort _ReadInt16(byte[] buf, int nPos)
		{
			short n = BitConverter.ToInt16(buf,nPos);
			return (ushort)System.Net.IPAddress.NetworkToHostOrder((short)n);
		}
		
		protected void _WriteInt16(ushort n, ushort nIndex)
		{
			n = (ushort)System.Net.IPAddress.HostToNetworkOrder((short)n);
			_WriteHeader(BitConverter.GetBytes(n), 2, nIndex);
		}
		
		protected void _WriteInt32(int n, ushort nIndex)
		{
			n = (int)System.Net.IPAddress.HostToNetworkOrder((int)n);
			_WriteHeader(BitConverter.GetBytes(n), 4, nIndex);
		}
		
		protected void _WriteInt64(long n, ushort nIndex)
		{
			n = (long)System.Net.IPAddress.HostToNetworkOrder((long)n);
			_WriteHeader(BitConverter.GetBytes(n), 8, nIndex);
		}
		
		/************************************************************************/
		/* nDataLen 位数据包体长度，用户只需要天上自己需要搭载的包长度          */
		/************************************************************************/
		
		public CCNetworkPacket()
		{
			_buffer = new byte[(int)PACKET_POS.PACKAGE_SIZE];
			this._memSize = (int)PACKET_POS.PACKAGE_SIZE;
			_packetLen = (int)PACKET_POS.PACKAGE_HEADER_SIZE;
		}
		
		public CCNetworkPacket(ushort nDataLen)
		{
			_buffer = new byte[nDataLen + (int)PACKET_POS.PACKAGE_HEADER_SIZE];
			this._memSize = (ushort)(nDataLen + (ushort)PACKET_POS.PACKAGE_HEADER_SIZE);
			_packetLen = (ushort)((ushort)PACKET_POS.PACKAGE_HEADER_SIZE + nDataLen);
		}
		
		public CCNetworkPacket(byte[] pBuf, int nIndex, int nBufLen)
		{
			_buffer = new byte[nBufLen];
			Array.Copy(pBuf, nIndex, _buffer, 0, nBufLen);
			_packetLen = PacketSize;
		}
		
		
		
		
		/// 加密解密接口
		public virtual bool EncryptBuffer() { return true;}
		public virtual bool CrevasseBuffer() { return true; }
		
		///压缩接口
		public virtual bool CompressBuffer() { return true; }
		public virtual bool UnComprecessBuffer() { return true; }
		
		////返回内存基址
		public byte[] PacketBuf
		{
			get { return _buffer;}
		}
		////返回数据包长度
		public ushort PacketSize
		{
			get{ return  _ReadInt16 (_buffer, (int)PACKET_POS.PACKAGE_LEN_POS);}
		}
		
		public void WriteCode(byte vs)
		{
			_buffer[(int)PACKET_POS.PACKAGE_CODE_POS] = vs;
		}
		////协议主版本号
		public byte ReadCode()
		{
			return _buffer[(int)PACKET_POS.PACKAGE_CODE_POS];
		}
		
		public void WriteVersion(byte vs)
		{
			_buffer[(int)PACKET_POS.PACKAGE_VS_POS] = vs;
		}
		////协议主版本号
		public byte ReadVersion()
		{
			return _buffer[(int)PACKET_POS.PACKAGE_VS_POS];
		}
		
		
		////读取序列号
		public ushort ReadSeqNum()
		{
			return (ushort)_ReadInt16(_buffer, (int)PACKET_POS.PACKAGE_SEQ_POS);
		}
		////设置序列号 
		public void WriteSeqNum(ushort seqNum)
		{
			_WriteInt16((ushort)seqNum, (ushort)PACKET_POS.PACKAGE_SEQ_POS);
		}
		
		////写入命令
		public void WriteCmd(ushort nCmd)
		{
			_WriteInt16((ushort)nCmd, (ushort)PACKET_POS.PACKAGE_SUB_CMD_POS);
		}
		////读取令字
		public ushort ReadCmd()
		{
			return (ushort)_ReadInt16(_buffer, (int)PACKET_POS.PACKAGE_SUB_CMD_POS);
		}
		
		public void WriteDst(int nDst)
		{
			_WriteInt32((int)nDst, (ushort)PACKET_POS.PACKAGE_DEST_POS);
		}
		
		public int ReadDst()
		{
			return (int)_ReadInt32(_buffer, (int)PACKET_POS.PACKAGE_SUB_CMD_POS);
		}
		
		public void WriteSrc(int nSrc)
		{
			_WriteInt32((int)nSrc, (ushort)PACKET_POS.PACKAGE_SOURCE_POS);
		}
		
		
		public int ReadSrc()
		{ 
			return (int)_ReadInt32(_buffer, (int)PACKET_POS.PACKAGE_SOURCE_POS);
		}
		
		public void WriteMask(byte[] mask)
		{
			_buffer[(int)PACKET_POS.PACKAGE_HEADER_FLAGS_POS] = mask[0];
			_buffer[(int)PACKET_POS.PACKAGE_HEADER_FLAGS_POS+1] = mask[1];
		}
		
		public byte[] ReadMask()
		{
			byte[] mask = new byte[2];
			mask[0] = _buffer[(int)PACKET_POS.PACKAGE_HEADER_FLAGS_POS] ;
			mask[1] = _buffer[(int)PACKET_POS.PACKAGE_HEADER_FLAGS_POS+1] ;
			return mask;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="nIndex"></param>
		/// <param name="nLen"></param>
		public void WriteBody(byte[] data,int nIndex, ushort nLen)
		{
			if ((int)PACKET_POS.PACKAGE_HEADER_SIZE + nLen + 2 > _memSize)
			{
				byte[] buf = new byte[_memSize * 2];
				_memSize *= 2;
				Array.Copy(_buffer, 0, buf, 0, _packetLen);
				_buffer = buf;
			}
			
			Array.Copy(data, nIndex, _buffer, (int)PACKET_POS.PACKAGE_BODY_POS, nLen);
			_packetLen += nLen;
		}
		
		public byte[] GetBodyBufer()
		{
			int nLen = ReadBodyLen();
			if (nLen == 0)
				return null;
			byte[] bBody  = new byte[nLen];
			Array.Copy(_buffer, (int)PACKET_POS.PACKAGE_BODY_POS, bBody,0 , nLen);
			return bBody;
		}
		
		public ushort ReadBodyLen()
		{
			return (ushort)(_ReadInt16(_buffer, (int)PACKET_POS.PACKAGE_LEN_POS) - (ushort)PACKET_POS.PACKAGE_HEADER_SIZE);
		}
		
		
		
		
		protected void _Begin(ushort nCmd, ushort nSeq, ushort nSrc, ushort nDes, byte vs)
		{   
			WriteCmd(nCmd);
			WriteSeqNum(nSeq);
			WriteSrc(nSrc);
			WriteDst(nDes);
			WriteVersion(vs);
			WriteMask(System.Text.Encoding.Default.GetBytes("OD"));
			_packetLen = (ushort)PACKET_POS.PACKAGE_HEADER_SIZE;//_WriteHeader(BitConverter.GetBytes((ushort)PACKET_POS.PACKAGE_HEADER_SIZE), 2, 0);
		}
		
		protected void _End()
		{
			_WriteInt16(_packetLen, (ushort)PACKET_POS.PACKAGE_LEN_POS);
		}
		
		void _WriteHeader(byte[] buf,ushort nLen,ushort nPos)
		{
			for (int i=0;i<nLen;i++)
			{
				_buffer[nPos+i] = buf[i];
			}       
		}
		
		protected	byte[] _buffer ;
		protected ushort _memSize;
		protected ushort _packetLen;
	};
	
	
	class  CInputPacket: CCNetworkPacket
	{
		
		public  CInputPacket(byte[] pBuf,int nIndex,int nBufLen)
			: base(pBuf, nIndex, nBufLen)
		{
			
		}
		
		
		public override bool EncryptBuffer() { return true;}
		public override bool CrevasseBuffer() { return true;}
		public override bool CompressBuffer() {return true;}
		public override bool UnComprecessBuffer(){ return true;}
		
	};
	
	class   COutputPacket: CCNetworkPacket
	{
		public COutputPacket()
			:base()
		{ 
			
		}
		
		public void Begin(byte[] buf,int nIndex,ushort nLen, ushort nCmd, ushort nSeq = 0, ushort nSrc = 0, ushort nDes = 0, byte vs = 0x11)
		{
			base._Begin(nCmd, nSeq, nSrc, nDes, vs);
			if (buf != null)
			{
				base.WriteBody(buf, nIndex, nLen);
			}
		}
		
		public void End()
		{
			base._End();
		}
		
		
		
		public override bool EncryptBuffer() { return true; }
		public override bool CrevasseBuffer() { return true; }
		public override bool CompressBuffer() { return true; }
		public override bool UnComprecessBuffer() { return true; }
	};
	
}



