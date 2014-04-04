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
				return true;
			} catch {
				Console.WriteLine ("Error occurred while connecting to server!");
				return false;
			}
		}

		public void Disconnect ()
		{
			if (socket.Connected)
				socket.Disconnect (false);
		}

		private byte[] ComposePacket (byte[] message)
		{
			byte[] intBytes = BitConverter.GetBytes (message.Length);
			if (BitConverter.IsLittleEndian)
				Array.Reverse (intBytes);

			byte[] result = new byte[ message.Length + intBytes.Length ];
			Buffer.BlockCopy (intBytes, 0, result, 0, intBytes.Length);
			Buffer.BlockCopy (message, 0, result, intBytes.Length, message.Length);
			return result;
		}

		public void SendMessage (byte[]message)
		{
			FullPacket packet = new FullPacket {apiVersion = this.apiVersion,
				protocol = protocolVersion,
				type = PacketType.ping,
				packet = message
			};
			byte[] raw = packet.Serialize ();
			byte[] packetRaw = ComposePacket (raw);
			Send (packetRaw);
		}

		private void Send (byte[]pack)
		{
			int bytesSend = socket.Send (pack);
			int packetLen = pack.Length;

			if (bytesSend < pack.Length) {
				byte[] chunk = new byte[pack.Length - bytesSend];
				Buffer.BlockCopy (pack, bytesSend, chunk, 0, pack.Length - bytesSend);
				Send (chunk);
			}
		}

		private byte[] Resv ()
		{
			byte[] len = new byte[4];
			socket.Receive (len);

			if (BitConverter.IsLittleEndian)
				Array.Reverse (len);

			int packetLen = BitConverter.ToInt32 (len, 0);

			byte[] packet = new byte[packetLen];
			int read = socket.Receive (packet);

			while (read < packetLen) {
				byte[] chunk = new byte[packetLen - read];
				int chunkRead = socket.Receive (packet);
				Buffer.BlockCopy (chunk, 0, packet, read, packetLen - read);
				read += chunkRead;
			}
			return packet;
		}

		public FullPacket RecvMessage ()
		{
			byte[] raw = Resv ();
			FullPacket header = FullPacket.Deserialize (raw);
			if (header == null)
				return null;
			return header;
		}
	}
}

