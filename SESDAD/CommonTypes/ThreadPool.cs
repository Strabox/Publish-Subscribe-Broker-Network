using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTypes
{
    
    /// <summary>
    ///  Thread pool that guarantee fifo order in calls,
    ///  only if you use one thread.
    /// </summary>
    public class ThreadPool
    {
        private class WorkStruct
        {
            private WaitCallback callbackFunction;

            public WaitCallback CallbackFunction
            {
                get
                {
                    return callbackFunction;
                }
            }

            private Object callbackArgument;

            public Object CallbackArgument
            {
                get
                {
                    return callbackArgument;
                }
            }

            public WorkStruct(WaitCallback callback,Object argument)
            {
                this.callbackArgument = argument;
                this.callbackFunction = callback;
            }
        }

        private Queue<WorkStruct> requestedTask;

        public ThreadPool(int thrNum)
        {
            requestedTask = new Queue<WorkStruct>();
            for (int i = 0; i < thrNum; i++)
            {
                ThreadStart ts = new ThreadStart(ThreadGetWork);
                Thread thread = new Thread(ts);
                thread.Start();
            }
        }

        public void AssyncInvoke(WaitCallback callback,Object argument)
        {
            lock (this)
            {
                requestedTask.Enqueue(new WorkStruct(callback,argument));
                Monitor.Pulse(this);
            }
        }

        // Main function for threads.
        public void ThreadGetWork()
        {
            WorkStruct work = null;
            while (true)
            {
                lock (this)
                {
                    while (requestedTask.Count == 0)
                        Monitor.Wait(this);
                    work = requestedTask.Dequeue();
                }
                work.CallbackFunction(work.CallbackArgument);
            }
        }

    }

}
