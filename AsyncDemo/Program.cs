using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class Program
    {
        //await关键字本身不会开启新的线程，但是await等待的任务有可能会开启额外的线程
        //await不会阻塞当前线程，当线程执行到await标记点时，就会退出当前方法，继续执行其他的任务
        //当await标记的任务执行完成后，继续执行await所在的方法中的后续的逻辑
        //异步任务在调用的时候已经开始执行，在await时标记需要等待任务完成
        static void Main(string[] args)
        {
            OutPut("before test");
            AsyncTest();
            OutPut("after test");
            Console.ReadLine();
        }

        static async Task AsyncTest()
        {
            OutPut("before call ValueTaskAsync");
            Task<string> valueTask = ValueTaskAsync();
            OutPut("after call ValueTaskAsync");
            Thread.Sleep(1000);
            OutPut("before call VoidTaskAsync");
            await VoidTaskAsync();
            ApplyTask();
            OutPut("after call VoidTaskAsync");


            var value = await valueTask;
            OutPut("waiting value of ValueTaskAsync");
            OutPut($"ValueTaskAsync returned:{value}");
            Console.ReadLine();
        }

        static async Task VoidTaskAsync()
        {
            OutPut("VoidTaskAsync calling ");
            await Task.Delay(1000);
            OutPut("VoidTaskAsync called");
        }

        static async Task<string> ValueTaskAsync()
        {
            OutPut("ValueTaskAsync calling");
            int i = 0;
            while (i < 20)
            {
                OutPut($"before i++：{i}");
                await Task.Delay(100);
                i++;
                OutPut($"after i++：{i}");
            }


            OutPut("ValueTaskAsync delayComplete");
            return "hello word";

        }

        static void ApplyTask()
        {
            for (int i = 0; i < 50; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                });
            }
        }


        private static void OutPut(string message)
        {
            Console.WriteLine($"{message}，当前线程id:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
