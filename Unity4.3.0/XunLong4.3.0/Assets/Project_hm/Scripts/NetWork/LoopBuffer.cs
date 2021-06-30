using System;


namespace NetWorklib
{
    class LoopBuffer
    {
        public LoopBuffer(Int32 nLen)
        {
            this.buffer = new byte[nLen];
            writeIndex = 0;
            readIndex = 0;
            dataLen = 0;
            bufLen = nLen;
        }

        public void Init()
        {
            writeIndex = 0;
            readIndex = 0;
            dataLen = 0;
        }
        public Int32 WriteData(byte[] buf,Int32 nIndex, Int32 nLen)
        {
            if (nLen < 0) return 0;
            if (nLen > bufLen - dataLen) nLen = bufLen - dataLen;
            
            for (Int32 i = 0; i < nLen; i++)
            {
                this.buffer[writeIndex++] = buf[nIndex + i];
                if (writeIndex >= dataLen) writeIndex = 0;
            }

            dataLen += nLen;
            return nLen;
        }

        public byte[] ReadData()
        {
            byte[] buf = new byte[dataLen];

            for (Int32 i = 0; i < dataLen; i++)
            {
                buf[i] = this.buffer[readIndex++];
                if (readIndex >= bufLen) readIndex = 0;
            }

            return buf;
        }

        public byte[] ReadData(Int32 nLen)
        {

            if (nLen < 0 || nLen > dataLen) nLen = dataLen;
            byte[] buf = new byte[nLen];

            for (Int32 i = 0; i < nLen; i++)
            {
                buf[i] = this.buffer[readIndex++];
                if (readIndex >= bufLen) readIndex = 0;
            }

            return buf;
        }


        public Int32 EraseBuf(Int32 nLen)
        {
            if (nLen < 0) return 0;
            if (nLen >= dataLen) nLen = dataLen;

            readIndex = (readIndex + nLen) % dataLen;

            dataLen = dataLen - nLen;
            return nLen;
        }

        public Int32 DataLen
        {
            get { return this.dataLen; }
        }

        public Int32 EmptyBuferLen
        {
            get { return (bufLen - dataLen); }
        }


        public byte this[int index]
        {
            get { return buffer[(index + readIndex) % bufLen]; }
            set { buffer[(index + readIndex) % bufLen] = value; }
        }


        private byte[] buffer;
        private Int32 writeIndex;
        private Int32 readIndex;
        private Int32 bufLen;
        private Int32 dataLen;
    }


    public class Buffer
    {
		/// <summary>
		/// 申请缓存 buffer 
		/// </summary>
		/// <param name="nLen">默认申请16k的空间.</param>
		public Buffer()
        {
			_buffer = new byte[8192];
            _dataSize = 0;
        }

		public void Append(byte[] buf,int nIndex,int nLen)
		{
			if (nLen + _dataSize > _buffer.Length) {
				int len = _buffer.Length;
				while(nLen > len)
				{
					len += 8192;
				}
				byte[] newbuf = new byte[_buffer.Length + len];

				Array.Copy(_buffer, _dataSize, newbuf, 0, _dataSize);
				_buffer = newbuf;
			} 

			Array.Copy (buf, nIndex, _buffer, _dataSize, nLen);
			_dataSize += nLen;
		}

        public void Skip(int nLen)
        {
			if (_dataSize == nLen) {
				_dataSize = 0;
			} else if (_dataSize > nLen) {
				//
				_dataSize -= nLen;
				for(int i = 0;i< _dataSize;i++,nLen++)
				{
					_buffer[i] = _buffer[nLen];
				}

			} else {
				UnityEngine.Debug.LogError(" Buffer dataisze < skip data len ! datasize:"+_dataSize + "  skiplen:"+ nLen);
				_dataSize = 0;
			}
        }

        public Int32 DataSize
        {
            get { return _dataSize; }
        }

        public byte this[ Int32 index]{
            get { return  _buffer[index]; }
            set { _buffer[index] = value; }
        }

        public byte[] Data
        {
            get { return _buffer; }
        }
        private byte[] _buffer;
		private Int32 _dataSize;
    }
}
