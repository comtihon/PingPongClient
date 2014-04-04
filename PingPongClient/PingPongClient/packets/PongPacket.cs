using System;
using ProtoBuf;
using System.IO;

namespace PingPongClient
{
	[ProtoContract]
	public class PongPacket : BasePacket
	{
		[ProtoMember (1)]
		public string response { get; set; }

		public static PongPacket Deserialize (byte[]raw)
		{
			try {
				PongPacket deserialized;
				using (MemoryStream ms = new MemoryStream (raw)) {
					deserialized = Serializer.Deserialize<PongPacket> (ms);
				}
				return deserialized;
			} catch {
				Console.Write ("Unknown packet format! [");
				return null;
			}
		}
	}
}

