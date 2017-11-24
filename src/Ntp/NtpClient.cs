namespace Vurdalakov
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class NtpClient
    {
        private String _ntpServer = "pool.ntp.org"; // default server

        public NtpClient()
        {
        }

        public NtpClient(String ntpServer)
        {
            this._ntpServer = ntpServer;
        }

        public DateTime GetUtcTime()
        {
            // prepare data buffer
            var data = new Byte[48];
            data[0] = 0x1B; // LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            // get NTP server endpoint
            var address = this.GetIpV4Address();
            var ipEndPoint = new IPEndPoint(address, 123); // NTP port is 123

            // call NTP server
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)) // NTP uses UDP
            {
                socket.SendTimeout = 3000;
                socket.ReceiveTimeout = 3000;

                socket.Connect(ipEndPoint);
                socket.Send(data);
                socket.Receive(data);
                socket.Close();
            }

            // extract full seconds and seconds fraction
            var seconds = BitConverter.ToUInt32(data, 40);
            var fraction = BitConverter.ToUInt32(data, 44);

            // convert to little-endian
            seconds = this.SwapEndianness(seconds);
            fraction = this.SwapEndianness(fraction);

            // get milliseconds since 01.01.1900
            var milliseconds = Convert.ToUInt64(seconds) * 1000 + Convert.ToUInt64(fraction) * 1000 / 0x100000000L;

            // return UTC time
            return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(milliseconds);
        }

        private IPAddress GetIpV4Address()
        {
            var addresses = Dns.GetHostEntry(_ntpServer).AddressList;
            if (0 == addresses.Length)
            {
            }

            foreach (var address in addresses)
            {
                if (AddressFamily.InterNetwork == address.AddressFamily)
                {
                    return address;
                }
            }

            throw new ApplicationException($"IPv4 address not found for {_ntpServer}");
        }

        private UInt32 SwapEndianness(UInt32 value)
        {
            return ((value & 0x000000FF) << 24) | ((value & 0x0000FF00) << 8) | ((value & 0x00FF0000) >> 8) | ((value & 0xFF000000) >> 24);
        }
    }
}
