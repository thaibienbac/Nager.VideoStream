﻿using System;
using System.IO;
using System.Threading;

namespace Nager.VideoStream.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists("frames"))
            {
                Directory.CreateDirectory("frames");
            }

            var streamUrl = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";

            var cancellationTokenSource = new CancellationTokenSource();

            var client = new VideoStreamClient();
            client.NewImageReceived += NewImageReceived;
            var task = client.StartFrameReaderAsync(streamUrl, OutputImageFormat.Bmp, cancellationTokenSource.Token);
            Console.WriteLine("Video Stream Frame handling started");
            Console.ReadLine();
            client.NewImageReceived -= NewImageReceived;
            cancellationTokenSource.Cancel();
            Console.WriteLine("Video Stream Frame handling stopped");
            Console.ReadLine();
        }

        private static void NewImageReceived(byte[] imageData)
        {
            Console.WriteLine($"New image received, bytes:{imageData.Length}");
            File.WriteAllBytes($@"frames\{DateTime.Now.Ticks}.bmp", imageData);
        }
    }
}
