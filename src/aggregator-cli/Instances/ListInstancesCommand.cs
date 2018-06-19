﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aggregator.cli
{
    [Verb("list.instances", HelpText = "Lists Aggregator instances.")]
    class ListInstancesCommand : CommandBase
    {
        internal override Task<int> RunAsync()
        {
            var azure = AzureLogon.Load()?.Logon();
            if (azure == null)
            {
                WriteError($"Must logon.azure first.");
                return Task.Run(() => 2);
            }
            var instances = new AggregatorInstances(azure);
            bool any = false;
            foreach (var item in instances.List())
            {
                WriteOutput(
                    item,
                    (data) => $"Instance {item.name} in {item.region}");
                any = true;
            }
            if (!any)
            {
                WriteInfo("No aggregator instances found.");
            }
            return Task.Run(() => 0);
        }
    }
}