using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTypes
{
    public delegate void ThreadWork();

    public class ThreadPool
    {

        private Queue<ThreadWork> requestedTask;

        private int maxQueueTask;


        public int MaxQueueTask
        {
            get
            {
                return maxQueueTask;
            }
        }

        public ThreadPool(int thrNum, int bufSize)
        {
            requestedTask = new Queue<ThreadWork>();
            maxQueueTask = bufSize;
            for (int i = 0; i < thrNum; i++)
            {
                ThreadStart ts = new ThreadStart(ThreadGetWork);
                Thread thread = new Thread(ts);
                thread.Start();
            }
        }

        public void AssyncInvoke(ThreadWork action)
        {
            lock (this)
            {
                if (requestedTask.Count != MaxQueueTask)
                {
                    requestedTask.Enqueue(action);
                    Monitor.Pulse(this);
                }
            }
        }

        // Main function for threads.
        public void ThreadGetWork()
        {
            ThreadWork work = null;
            while (true)
            {
                lock (this)
                {
                    while (requestedTask.Count == 0)
                        Monitor.Wait(this);
                    work = requestedTask.Dequeue();
                }
                work();
            }
        }

    }
}
