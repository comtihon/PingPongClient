using System;
using ProtoBuf;
using System.IO;

namespace PingPongClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Client client = new Client ();

			PingPacket pingPacket = new PingPacket { request = "Ping" };
			if (!client.Connect ())
				return;
			client.SendMessage (pingPacket.Serialize ());
			byte[] message = client.RecvMessage ();
			if (message == null) {
				Console.WriteLine ("Error recieving packet!");
				client.Disconnect ();
			}
		}
	}
}
