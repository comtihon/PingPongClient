using System;
using System.Configuration;
using System.Net.Sockets;

namespace PingPongClient
{
	public class Client
	{
		private int port;
		private string host;
		private int protocolVersion;
		private int apiVersion;
		private Socket socket;

		public Client ()
		{
			host = ConfigurationManager.AppSettings ["ServerHost"];
			port = int.Parse (ConfigurationManager.AppSettings ["ServerPort"]);

			apiVersion = int.Parse (ConfigurationManager.AppSettings ["APIVersion"]);
			protocolVersion = int.Parse (ConfigurationManager.AppSettings ["ProtocolVersion"]);
		}

		public bool Connect ()
		{
			Console.WriteLine ("Connecting to host {0}:{1}. Api:{2}, Protocol:{3}", host, port, apiVersion, protocolVersion);
			try {
				socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.Connect (host, port);
				Console.WriteLine("OK");
				return true;
			} catch {
				Console.WriteLine ("Error occurred while connecting to server!");
				return false;
			}
		}

		public void Disconnect() 
		{
			if (socket.Connected)
				socket.Disconnect(false);
		}

		public void SendMessage (byte[]message)
		{
			FullPacket packet = new FullPacket {apiVersion = this.apiVersion,
				protocol = protocolVersion,
				type = PacketType.ping,
				packet = message
			};
			byte[] raw = packet.Serialize ();

			int bytesSent = socket.Send (raw);
			if (bytesSent != raw.Length)
				Console.WriteLine ("Packet was send corrupted");
		}

		public byte[] RecvMessage ()
		{
			byte[] raw = new byte[1024];
			int bytesRec = socket.Receive (raw);

			FullPacket header = FullPacket.Deserialize (raw);
			if (header == null)
				return null;
			Console.WriteLine ("Got packet from server: protocol {0}, api {1}, type {2}", header.protocol, header.apiVersion, header.type);
			return header.packet;
		}
	}
}

