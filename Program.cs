using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBackgroundTask
{
	class Program
	{
		public static async Task Main(string[] args)
		{
			ServiceX x = new ServiceX();
			await x.SomeServiceEntryMethod();
			Console.Read();
		}
	}

	public class ServiceX
	{
		public Task SomeServiceEntryMethod()
		{
			Console.WriteLine("Multithreading started");
			Console.WriteLine();
			Thread.Sleep(1);

			List<Task> tasks = new List<Task>();
			for (int i = 0; i < 8; i++)
			{
				//I can use this version 
				var t = DoSomeAsyncStuff(i);       

				//or I can use this version both seems to have same results
				//var t = DoSomeAsyncStuffV2(i);
				tasks.Add(t);
			}
			
			Console.WriteLine("I'm the main thread and if any method was synchronous I should not execute before that method");
			Console.WriteLine();

			//in the calling method I can await for all task to complete the for loop spins a few async tasks
			return Task.WhenAll(tasks);
		}

		//private async Task DoSomeAsyncStuffV2(int taskNumber)
		//{
		//	await Task.Run(() =>
		//	{
		//		if (taskNumber == 0)
		//			Thread.Sleep(1000);
		//		Console.WriteLine("test Task No: " + taskNumber.ToString() + " ManagedThreadId:" + Thread.CurrentThread.ManagedThreadId.ToString());

		//	});
		//}

		private Task DoSomeAsyncStuff(int taskNumber)
		{
			return Task.Run(() =>
			{
				if (taskNumber == 0)
				{
					Thread.Sleep(1000);
					Console.WriteLine($"I was the longest running task/method not marked as async.{Environment.NewLine}Task No: {taskNumber.ToString()} ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId.ToString());
				}
				else
				{
					Console.WriteLine("test Task No: " + taskNumber.ToString() + " ManagedThreadId:" + Thread.CurrentThread.ManagedThreadId.ToString());
				}
			});
		}
	}


}
