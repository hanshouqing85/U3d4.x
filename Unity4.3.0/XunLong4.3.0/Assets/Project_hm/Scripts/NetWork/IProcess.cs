namespace NetWorklib
{
	/// <summary>
	/// 解析数据委托函数类型.
	/// </summary>
	public delegate DataUnit OnParserPacket(CCNetworkPacket package);
	/// <summary>
	/// 处理dataUnit数据 委托函数类型
	/// </summary>
	public delegate int OnProcessPacket(DataUnit dataUnit);


	/// <summary>
	///  数据包处理回调 
	/// </summary>
	public struct PacketProcessStruct
	{
		public PacketProcessStruct(OnParserPacket parserProcess,OnProcessPacket doProcess)
		{
			ParserProcess = parserProcess;
			DoProcess = doProcess;
		}
		/// <summary>
		/// The parser process.
		/// 解析网络数据包句柄
		/// </summary>
		public OnParserPacket ParserProcess;

		/// <summary>
		/// 处理数据包句柄
		/// </summary>
		public OnProcessPacket DoProcess;

	}





	public class DataUnit
	{
		public DataUnit()
		{
			m_nChannelID = 0;
			m_nPacketLen = 0;
			m_bCode = 0;
			m_bVS = 0x11;
			m_nSeq = 0;
			m_nCmd = 0;
			m_nDest = 0;
			m_nSrc = 0;
			m_oBodyUnit = null;
		}

		public int m_nChannelID;
		/// <summary>
		/// The length of the  packet.
		/// </summary>
		public ushort m_nPacketLen;
		/// <summary>
		/// The m_b code.
		/// </summary>
		public byte m_bCode;
		/// <summary>
		/// The m_b V.
		/// </summary>
		public byte m_bVS;
		/// <summary>
		/// The m_n seq.
		/// </summary>
		public ushort m_nSeq;
		/// <summary>
		/// The m_n cmd.
		/// </summary>
		public ushort m_nCmd;
		/// <summary>
		/// The m_n destination.
		/// </summary>
		public int m_nDest;
		/// <summary>
		/// The m_n source.
		/// </summary>
		public int m_nSrc;

		/// <summary>
		/// The m_o body unit.
		/// </summary>
		public global::ProtoBuf.IExtensible m_oBodyUnit;


	}
}

