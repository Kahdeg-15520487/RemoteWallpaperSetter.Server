using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace RosenHCMC.VPN.AppServices
{
    public class DiscoveryService : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                UdpClient Server = new UdpClient(8888);

                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                byte[] ResponseData;

                using (MemoryStream stream = new MemoryStream())
                {
                    JsonWriterOptions options = new JsonWriterOptions
                    {
                        Indented = true
                    };
                    using (Utf8JsonWriter writer = new Utf8JsonWriter(stream, options))
                    {
                        writer.WriteStartArray();
                        foreach (IPAddress ip in localIPs)
                        {
                            writer.WriteStringValue(ip.ToString());
                        }
                        writer.WriteEndArray();
                    }

                    string json = Encoding.UTF8.GetString(stream.ToArray());
                    Console.WriteLine(json);

                    ResponseData = Encoding.ASCII.GetBytes(json);
                }

                while (!cancellationToken.IsCancellationRequested)
                {
                    UdpClient server = new UdpClient(9099);
                    IPEndPoint clientEp = new IPEndPoint(IPAddress.Any, 0);
                    //byte[] clientRequestData = server.Receive(ref clientEp);
                    Task<byte[]> task = Task.Run(() => server.Receive(ref clientEp));
                    byte[] clientRequestData = await task;
                    //UdpReceiveResult clientRequestData = await server.ReceiveAsync();
                    string clientRequest = Encoding.ASCII.GetString(clientRequestData);

                    Console.WriteLine($"Recived {clientRequest} from {clientEp.Address}, sending response: { ResponseData} ");
                    await server.SendAsync(ResponseData, ResponseData.Length, clientEp);
                    server.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
