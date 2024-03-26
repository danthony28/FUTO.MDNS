﻿using System.Text;
using FUTO.MDNS;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, _) => cts.Cancel();

        var serviceDiscoverer = new ServiceDiscoverer("_googlecast._tcp.local", "_airplay._tcp.local", "_fcast._tcp.local");
        //await serviceDiscoverer.BroadcastServiceAsync("SomeDevice", "_googlecast._tcp.local", 8009);
        serviceDiscoverer.OnServicesUpdated += (services) =>
        {
            var builder = new StringBuilder();
            foreach (var service in services)
            {
                builder.Clear();
                builder.AppendLine($"Service {service.Name} ({string.Join(", ", service.Pointers)}) on port {service.Port}:");
                foreach (var address in service.Addresses)
                    builder.AppendLine($"  - {address}");
                Console.Write(builder.ToString());
            }
        };

        await serviceDiscoverer.RunAsync(cts.Token);
    }
}