﻿using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    public class PublisherServer : MarshalByRefObject, IGeneralControlServices,
        IPublisherControlServices
    {
        private string name;

        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        private IPuppetMasterLog logServer;

        public PublisherServer(string name,string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // General test and control methods.

        public void Publish(string topicName, int numberOfEvents, int interval)
        {
            IBroker broker;
            for(int i = 0; i < numberOfEvents; i++)
            {
                broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
                broker.Diffuse(new Event(this.name, topicName, "content"));
                logServer.LogAction("PubEvent " + name + ", " + "???????" + ", " +
                    topicName + ", " + i);
                Thread.Sleep(interval);
            }
        }

        public void Crash()
        {
            System.Environment.Exit(-1);
        }

        public void Freeze()
        {
            //TODO
        }

        public void Status()
        {
            //TODO
        }

        public void Unfreeze()
        {
            //TODO
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        string GetName()
        {
            return this.name;
        }
    }
}
