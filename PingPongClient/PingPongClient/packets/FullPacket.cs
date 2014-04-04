using System;
using ProtoBuf;
using System.IO;

namespace PingPongClient
{
	public enum PacketType
	{
		ping,
		pong,
		error
	}

	[ProtoContract]
	public class FullPacket : BasePacket
	{
		[ProtoMember (1)]
		public PacketType type { get; set; }

		[ProtoMember (2)]
		public int protocol { get; set; }

		[ProtoMember (3)]
		public int apiVersion { get; set; }

		[ProtoMember (4)]
		public byte[] packet { get; set; }

		public static FullPacket Deserialize (byte[]raw)
		{
			try {
				FullPacket deserialized;
				using (MemoryStream ms = new MemoryStream (raw)) {
					deserialized = Serializer.Deserialize<FullPacket> (ms);
				}
				return deserialized;
			} catch {
				Console.Write ("Unknown header format! [");
				foreach (byte b in raw)
					Console.Write (b);
				Console.WriteLine ("], {0}", raw.Length);
				return null;
			}
		}
	}
}

