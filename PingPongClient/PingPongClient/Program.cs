using System;
using ProtoBuf;
using System.IO;
using System.Configuration;

namespace PingPongClient
{
	class MainClass
	{
		private static int pingCount;

		public static void Main (string[] args)
		{
			pingCount = int.Parse (ConfigurationManager.AppSettings ["PingCount"]);

			while (pingPong () && pingCount-- > 0)
				;

		}

		private static bool pingPong ()
		{
			Client client = new Client ();

			if (!client.Connect ())
				return false;	
			PingPacket pingPacket = new PingPacket { request = "Ping" };
			client.SendMessage (pingPacket.Serialize ());
			Console.WriteLine ("Ping");
			FullPacket message = client.RecvMessage ();
			if (message == null) {
				Console.WriteLine ("Error recieving packet!");
				client.Disconnect ();
			}

			switch (message.type) {
			case PacketType.pong:
				PongPacket pong = PongPacket.Deserialize (message.packet);
				if (pong != null)
					Console.WriteLine (pong.response);
				client.Disconnect ();
				return true;
			case PacketType.error:
				ErrorPacket error = ErrorPacket.Deserialize (message.packet);
				if (error != null)
					Console.WriteLine ("Got error: {0} - {1}", error.code, error.message);
				break;
			default :
				break;
			}
			client.Disconnect ();	
			return false;
		}
	}
}
