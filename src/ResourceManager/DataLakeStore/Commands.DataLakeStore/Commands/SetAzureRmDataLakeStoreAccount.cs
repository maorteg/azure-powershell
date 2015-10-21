﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Azure.Commands.DataLakeStore.Models;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace Microsoft.Azure.Commands.DataLakeStore
{
    [Cmdlet(VerbsCommon.Set, "AzureRmDataLakeStoreAccount"), OutputType(typeof (DataLakeStoreAccount))]
    public class SetAzureDataLakeStoreAccount : DataLakeStoreCmdletBase
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 0, Mandatory = true,
            HelpMessage = "Name of the account.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Position = 1, Mandatory = false,
            HelpMessage = "Name of default group to use for all newly created files and folders in the Data Lake Store."
            )]
        [ValidateNotNullOrEmpty]
        public string DefaultGroup { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Position = 2, Mandatory = false,
            HelpMessage =
                "A string,string dictionary of tags associated with this account that should replace the current set of tags"
            )]
        [ValidateNotNull]
        public Hashtable Tags { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Position = 3, Mandatory = false,
            HelpMessage = "Name of resource group under which you want to update the account.")]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        protected override void ProcessRecord()
        {
            // TODO: Define what can be updated for DataLakeStore accounts
            var currentAccount = DataLakeStoreClient.GetAccount(ResourceGroupName, Name);
            var location = currentAccount.Location;

            if (string.IsNullOrEmpty(DefaultGroup))
            {
                DefaultGroup = currentAccount.Properties.DefaultGroup;
            }

            var tags = new Dictionary<string, string>();
            if (Tags != null && Tags.Count > 0)
            {
                foreach (string entry in Tags.Keys)
                {
                    tags.Add(entry, (string) Tags[entry]);
                }
            }
            else
            {
                tags = (Dictionary<string, string>) currentAccount.Tags;
            }

            WriteObject(DataLakeStoreClient.CreateOrUpdateAccount(ResourceGroupName, Name, DefaultGroup, location, tags));
        }
    }
}