using System;
using ProtoBuf;
using System.IO;

namespace PingPongClient
{
	[ProtoContract]
	public class ErrorPacket : BasePacket
	{
		[ProtoMember (1)]
		public int code { get; set; }

		[ProtoMember (2)]
		public string message { get; set; }

		public static ErrorPacket Deserialize (byte[]raw)
		{
			try {
				ErrorPacket deserialized;
				using (MemoryStream ms = new MemoryStream (raw)) {
					deserialized = Serializer.Deserialize<ErrorPacket> (ms);
				}
				return deserialized;
			} catch {
				Console.Write ("Unknown packet format! [");
				return null;
			}
		}
	}
}

