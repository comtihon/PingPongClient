using System;
using ProtoBuf;

namespace PingPongClient
{
	[ProtoContract]
	public class ErrorPacket : BasePacket
	{
		[ProtoMember(1)]
		public int code { get; set; }

		[ProtoMember(2)]
		public string message { get; set; }
	}
}

