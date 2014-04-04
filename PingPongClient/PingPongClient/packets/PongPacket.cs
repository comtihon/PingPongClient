using System;
using ProtoBuf;

namespace PingPongClient
{
	[ProtoContract]
	public class PongPacket : BasePacket
	{
		[ProtoMember(1)]
		public string response { get; set; }
	}
}

