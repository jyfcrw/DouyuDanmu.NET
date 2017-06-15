using System;
using System.Text;
using SuperSocket.ProtoBase;
using System.IO;

namespace DouyuDanmu
{
    public class DouyuPackageInfo : IPackageInfo
    {
        public static int HeaderSize = 8;
        public static int HeaderFullSize = 12;

        public enum MessageType : short
        {
            CLIENT_TO_SERVER = 689,
            SERVER_TO_CLIENT = 690
        }

        public Int32 Size;
        public Int16 Type = (Int16)MessageType.CLIENT_TO_SERVER;
        public byte Cipher = 0;
        public byte Reserve = 0;
        public byte[] Body;
        public string Data;

        public byte[] Encode(string data)
        {
            Body = new byte[Encoding.UTF8.GetBytes(data).Length + 1];
            Encoding.UTF8.GetBytes(data).CopyTo(Body, 0);
            char c = (char)'\0';
            Body[Body.Length - 1] = (byte)c;
            Size = Body.Length + HeaderSize;

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write((Int32)Size); // 长度出现两遍
            writer.Write((Int32)Size);
            writer.Write((Int16)Type);
            writer.Write(Cipher);
            writer.Write(Reserve);
            writer.Write(Body);
            return stream.ToArray();
        }

        public string Decode(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);

            reader.ReadInt32(); // 读取长度
            reader.ReadInt32(); // 再次读取长度

            Type = (Int16)reader.ReadInt16();
            Cipher = reader.ReadByte();
            Reserve = reader.ReadByte();
            Body = reader.ReadBytes(data.Length - HeaderSize - 4);
            Data = Encoding.UTF8.GetString(Body).TrimEnd('\0');
            return Data;
        }

    }
}
