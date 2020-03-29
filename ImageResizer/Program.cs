using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        async static Task Main(string[] args)
        {

            CancellationTokenSource cts = new CancellationTokenSource();


            #region 等候使用者輸入 取消 c 按鍵
            ThreadPool.QueueUserWorkItem(x =>
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.C)
                {
                    cts.Cancel();
                }
            });
            #endregion
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;
            long oldProcessTime;
            long newProcessTime;
            float improvement;
            ImageProcess imageProcess = new ImageProcess();

            imageProcess.Clean(destinationPath);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            imageProcess.ResizeImages(sourcePath, destinationPath, 2.0);
            sw.Stop();
            Console.WriteLine($"舊花費時間: {sw.ElapsedMilliseconds} ms");
            oldProcessTime = sw.ElapsedMilliseconds;

            sw.Reset();

            sw.Start();
            await imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0, cts.Token);
            sw.Stop();
            Console.WriteLine($"新花費時間: {sw.ElapsedMilliseconds} ms");

            newProcessTime = sw.ElapsedMilliseconds;
            improvement = ((float)(oldProcessTime - newProcessTime) / oldProcessTime)*100;
            Console.WriteLine($"效能提升: {improvement} %");

        }
    }
}
