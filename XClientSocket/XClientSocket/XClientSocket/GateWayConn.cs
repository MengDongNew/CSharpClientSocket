using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace XClientSocket.XClientSocket
{
    class GateWayConn
    {
    }

    public class ServerPacket
    {
        public bool err;
        public ushort eventId;
        public ushort len;
        public byte state;
        public ArrByteReader arrByteReader = new ArrByteReader();

        public static ServerPacket Create(ArrByte64K arrByte64K)
        {
            return new ServerPacket(arrByte64K);
        }
        private ServerPacket(ArrByte64K arrByte64K)
        {
            //int offset = 0;
            //ushort count;
            len = (ushort)(arrByte64K.arrByte64K[0] * 256 + arrByte64K.arrByte64K[1]);
            eventId = (ushort)(arrByte64K.arrByte64K[2] * 256 + arrByte64K.arrByte64K[3]);
            Console.WriteLine("length　=　{0}, eventId = {1}", len, eventId);
            //ArrByte64K arr64k = new ArrByte64K();
            arrByteReader.SetArrByte(arrByte64K);
        }
    }



    public class ArrByte64K
    {
        public const int ByteSize = 1024 * 10;
        public ushort len = 0;
        public byte[] arrByte64K = new byte[ByteSize];
        public ArrByte64K()
        {

        }
        public ArrByte64K(ArrByte64K bytes64k)
        {
            Array.Copy(bytes64k.arrByte64K, 0, arrByte64K, 0, bytes64k.arrByte64K.Length);
        }
        public static ArrByte64K Create()
        {
            return new ArrByte64K();
        }
        public static ArrByte64K Create(ArrByte64K bytes64k)
        {
            return new ArrByte64K(bytes64k);
        }
        public void AppendBytes(byte[] appendBytes, int length)
        {
            lock (arrByte64K)
            {
                len += (ushort)length;
                if (len > ByteSize)
                {//太大了

                    byte[] bytes = arrByte64K;
                    arrByte64K = new byte[len];
                    Array.Copy(bytes, arrByte64K, len - (ushort)length);
                    //					Debug.Log ("太长了XXXXXXXXXXXXXXXXXXXXX");
                    return;
                }
                Array.Copy(appendBytes, 0, arrByte64K, len - (ushort)length, length);
            }

        }
        public void DelBytes(ushort _len, int index = 0)
        {
            _len = len < _len ? len : _len;
            //byte[] tmps = new byte[_len];
            int endIndex = index + _len;
            endIndex = ByteSize < endIndex ? ByteSize : endIndex;
            lock (arrByte64K)
            {
                for (int i = endIndex; i < ByteSize; i++)
                {
                    arrByte64K[i - endIndex] = arrByte64K[i];
                    arrByte64K[i] = 0;
                }
                if (len >= _len)
                    len -= _len;
            }

        }
        /// <summary>
        /// Resets the arr byte64k.
        /// </summary>
        /// <param name="newLength">New length.</param>
        public void ReLengthArrByte64k(int newLength)
        {
            newLength = 1024 * (newLength / 1024 + 1);
            byte[] bytes = arrByte64K;
            arrByte64K = new byte[newLength];
            Array.Copy(bytes, arrByte64K, bytes.Length);
        }
        public void Clear()
        {
            for (int i = 0; i < ByteSize; i++)
            {
                arrByte64K[i] = 0;
            }
            len = 0;
        }
    }



    public class PacketSend
    {
        public ArrByte64K _arrByte64K;
        private ushort _i = 4;// 8 个字节 

        public PacketSend Write(ushort v)
        {
            if (_i + 2 >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(byte[] v)
        {
            int length = v.Length;
            if (_i + length + sizeof(ushort) >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                LengthenArrByte64k(_i + length + 2);
            }
            Write((ushort)length);
            for (int i = _i; i < _i + length; i++)
            {
                _arrByte64K.arrByte64K[i] = v[i - _i];
            }
            _i += (ushort)length;
            return this;
        }
        public PacketSend Write(byte[] v, int length)
        {

            if (_i + length + sizeof(ushort) >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                LengthenArrByte64k(_i + length + 2);
            }
            Write((ushort)length);
            for (int i = _i; i < _i + length; i++)
            {
                _arrByte64K.arrByte64K[i] = v[i - _i];
            }
            _i += (ushort)length;
            return this;
        }
        private PacketSend()
        {
            _arrByte64K = new ArrByte64K();

        }
        public static PacketSend Create(EEvents eventId)
        {
            var pk = new PacketSend();
            //pk._arrByte64K.arrByte64K[0] = (byte)(15 >> 8);
            //pk._arrByte64K.arrByte64K[1] = (byte)(15);//第1、2位存储id
            pk._arrByte64K.arrByte64K[2] = (byte)((ushort)eventId >> 8);
            pk._arrByte64K.arrByte64K[3] = (byte)((ushort)eventId);//第3、4位存储id
            return pk;
        }

        public PacketSend Write(bool v)
        {
            if (v)
                return Write((byte)1);
            else
                return Write((byte)0);
        }
        public PacketSend Write(byte v)
        {
            if (_i + 1 >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (v); _i++;
            return this;
        }
        public PacketSend Write(short v)
        {
            if (_i + 2 >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(int v)
        {
            if (_i + 4 >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 24); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 16); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(ulong v)
        {
            if (_i + 8 >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 56); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 48); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 40); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 32); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 24); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 16); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(long v)
        {
            if (_i + 8 >= _arrByte64K.arrByte64K.Length)
            {
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 56); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 48); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 40); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 32); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 24); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 16); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(string value)
        {
            return WriteUTF8(value);
        }
        public PacketSend WriteUTF8(string value)
        {
            int length = Encoding.UTF8.GetByteCount(value);
            if (_i + length + 2 >= _arrByte64K.arrByte64K.Length)
            {
                LengthenArrByte64k(_i + length + 2);
                //Debug.Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                //return this;
            }
            Write((short)length);
            Encoding.UTF8.GetBytes(value, 0, value.Length, _arrByte64K.arrByte64K, (int)_i);
            _i += (ushort)length;
            return this;
        }
        public void LengthenArrByte64k(int newLength)
        {
            _arrByte64K.ReLengthArrByte64k(newLength);
            /*
            newLength = 1024* (newLength/1024+1);
            byte[] bytes = _arrByte64K.arrByte64K;
            _arrByte64K.arrByte64K = new byte[newLength];
            Array.Copy (bytes, _arrByte64K.arrByte64K,bytes.Length);
            */
        }
        public ArrByte64K ExportArrByte64K()
        {
            _arrByte64K.arrByte64K[0] = (byte)((_i) >> 8);
            _arrByte64K.arrByte64K[1] = (byte)((_i));//0，1字节存储字节流的长度
            _arrByte64K.len = _i;
            var ar = _arrByte64K;
            _arrByte64K = null;
            return ar;
        }
    }


    public class ArrByteReader
    {
        private ArrByte64K _arrByte;
        private int _readLen;
        public void SetArrByte(ArrByte64K arrByte)
        {
            // Array.Copy(_arrByte.arrByte64K, arrByte.arrByte64K, arrByte.arrByte64K.Length);
            _arrByte = arrByte;
            _readLen = 4;//默认从第byte[4]读取数据
            if (_arrByte != null)
            {
                _arrByte.len = (ushort)(_arrByte.arrByte64K[0] * 256 + _arrByte.arrByte64K[1]);
            }

        }

        public byte[] ReadBytes()
        {
            int len = ReaduShort();
            _readLen += len;

            if (_readLen > _arrByte.len) return null;

            byte[] bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                bytes[i] = _arrByte.arrByte64K[_readLen - len + i];
            }

            return bytes;
        }
        public byte ReadByte()
        {
            _readLen += 1;
            if (_readLen > _arrByte.len)
                return 0;
            return _arrByte.arrByte64K[_readLen - 1];
        }
        public ushort ReaduShort()
        {
            _readLen += 2;
            if (_readLen > _arrByte.len)
                return 0;
            return GetuShort(_arrByte.arrByte64K, _readLen - 2);
        }
        public int ReadInt()
        {
            _readLen += 4;
            if (_readLen > _arrByte.len)
                return 0;
            return GetInt(_arrByte.arrByte64K, _readLen - 4);
        }
        public ulong ReaduLong()
        {
            _readLen += 8;
            if (_readLen > _arrByte.len)
                return 0;

            return (((ulong)_arrByte.arrByte64K[_readLen - 8]) << 56)
                | (((ulong)_arrByte.arrByte64K[_readLen - 7]) << 48)
                | (((ulong)_arrByte.arrByte64K[_readLen - 6]) << 40)
                | (((ulong)_arrByte.arrByte64K[_readLen - 5]) << 32)
                | (((ulong)_arrByte.arrByte64K[_readLen - 4]) << 24)
                | (((ulong)_arrByte.arrByte64K[_readLen - 3]) << 16)
                | (((ulong)_arrByte.arrByte64K[_readLen - 2]) << 8)
                | (((ulong)_arrByte.arrByte64K[_readLen - 1]))
                ;
        }
        public long ReadLong()
        {
            _readLen += 8;
            if (_readLen > _arrByte.len)
                return 0;

            return (((long)_arrByte.arrByte64K[_readLen - 8]) << 56)
                | (((long)_arrByte.arrByte64K[_readLen - 7]) << 48)
                | (((long)_arrByte.arrByte64K[_readLen - 6]) << 40)
                | (((long)_arrByte.arrByte64K[_readLen - 5]) << 32)
                | (((long)_arrByte.arrByte64K[_readLen - 4]) << 24)
                | (((long)_arrByte.arrByte64K[_readLen - 3]) << 16)
                | (((long)_arrByte.arrByte64K[_readLen - 2]) << 8)
                | (((long)_arrByte.arrByte64K[_readLen - 1]))
                ;
        }
        public string ReadUTF8String(bool safeCheck = true)
        {
            ushort l = ReaduShort();
            return ReadUTF8StringSafe(l, safeCheck);
        }
        private bool IsSafeChar(int c)
        {
            return (c >= 0x20 && c < 0xFFFE);
        }
        private string ReadUTF8StringSafe(int fixedLength, bool safeCheck = true)
        {
            if (_readLen + fixedLength > _arrByte.len)
            {
                _readLen += fixedLength;
                return String.Empty;
            }

            int bound = _readLen + fixedLength;

            int count = 0;
            int index = _readLen;
            int start = _readLen;

            while (index < bound && _arrByte.arrByte64K[index++] != 0)
                ++count;

            index = 0;

            byte[] buffer = new byte[count];
            int value = 0;

            while (_readLen < bound && (value = _arrByte.arrByte64K[_readLen++]) != 0)
                buffer[index++] = (byte)value;

            string s = Utility.UTF8.GetString(buffer);

            bool isSafe = true;

            for (int i = 0; isSafe && i < s.Length; ++i)
                isSafe = IsSafeChar((int)s[i]);

            _readLen = start + fixedLength;

            if (isSafe || !safeCheck)
                return s;

            StringBuilder sb = new StringBuilder(s.Length);

            for (int i = 0; i < s.Length; ++i)
                if (IsSafeChar((int)s[i]))
                    sb.Append(s[i]);

            return sb.ToString();
        }

        public static ushort GetuShort(byte[] arrByte, int index)
        {
            return (ushort)(((ushort)(arrByte[index]) << 8)
                | ((ushort)(arrByte[index + 1])));
        }
        public static int GetInt(byte[] arrByte, int index)
        {
            return (((int)arrByte[index]) << 24)
                | (((int)arrByte[index + 1]) << 16)
                | (((int)arrByte[index + 2]) << 8)
                | (((int)arrByte[index + 3]));
        }
    }



    public class Utility
    {
        private static Encoding m_UTF8, m_UTF8WithEncoding;

        public static Encoding UTF8
        {
            get
            {
                if (m_UTF8 == null)
                    m_UTF8 = new UTF8Encoding(false, false);

                return m_UTF8;
            }
        }

        public static Encoding UTF8WithEncoding
        {
            get
            {
                if (m_UTF8WithEncoding == null)
                    m_UTF8WithEncoding = new UTF8Encoding(true, false);

                return m_UTF8WithEncoding;
            }
        }

        public static void FormatBuffer(TextWriter output, Stream input, int length)
        {
            output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

            int byteIndex = 0;

            int whole = length >> 4;
            int rem = length & 0xF;

            for (int i = 0; i < whole; ++i, byteIndex += 16)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(16);

                for (int j = 0; j < 16; ++j)
                {
                    int c = input.ReadByte();

                    bytes.Append(c.ToString("X2"));

                    if (j != 7)
                    {
                        bytes.Append(' ');
                    }
                    else
                    {
                        bytes.Append("  ");
                    }

                    if (c >= 0x20 && c < 0x80)
                    {
                        chars.Append((char)c);
                    }
                    else
                    {
                        chars.Append('.');
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                output.Write("  ");
                output.WriteLine(chars.ToString());
            }

            if (rem != 0)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(rem);

                for (int j = 0; j < 16; ++j)
                {
                    if (j < rem)
                    {
                        int c = input.ReadByte();

                        bytes.Append(c.ToString("X2"));

                        if (j != 7)
                        {
                            bytes.Append(' ');
                        }
                        else
                        {
                            bytes.Append("  ");
                        }

                        if (c >= 0x20 && c < 0x80)
                        {
                            chars.Append((char)c);
                        }
                        else
                        {
                            chars.Append('.');
                        }
                    }
                    else
                    {
                        bytes.Append("   ");
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                output.Write("  ");
                output.WriteLine(chars.ToString());
            }
        }
    }

}
