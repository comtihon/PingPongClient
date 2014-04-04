using System;
using ProtoBuf;
using System.IO;

namespace PingPongClient
{
	[ProtoContract]
	public class PingPacket : BasePacket
	{
		[ProtoMember(1)]
		public string request { get; set; }
	}
}

