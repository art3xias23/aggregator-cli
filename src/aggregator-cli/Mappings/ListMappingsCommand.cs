﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aggregator.cli
{

    [Verb("list.mappings", HelpText = "Lists mappings from existing VSTS Projects to Aggregator Rules.")]
    class ListMappingsCommand : CommandBase
    {
        internal override async Task<int> RunAsync()
        {
            var vsts = await VstsLogon.Load()?.LogonAsync();
            if (vsts == null)
            {
                WriteError($"Must logon.vsts first.");
                return 2;
            }
            var mappings = new AggregatorMappings(vsts);
            bool any = false;
            foreach (var item in mappings.List())
            {
                WriteOutput(
                    item,
                    (data) => $"Rule {item.rule} in {item.project} for {item.events}");
                any = true;
            }
            if (!any)
            {
                WriteInfo("No rule mappings found.");
            }
            return 0;
        }
    }
}