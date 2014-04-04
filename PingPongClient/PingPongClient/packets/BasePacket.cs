using System;
using System.IO;
using ProtoBuf;

namespace PingPongClient
{
	public class BasePacket
	{
		public byte[] Serialize ()
		{
			byte[] serialized;
			using (var ms = new MemoryStream ()) {
				Serializer.Serialize (ms, this);
				serialized = ms.ToArray ();
			}
			return serialized;
		}
	}
}

