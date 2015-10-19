using System;
using System.Collections.Generic;
using System.IO;
using Helvetica.Common.Logging;
using Helvetica.NntpClient.Interfaces;
using log4net;

namespace Helvetica.NntpClient.Models
{
    public class GroupCollection : NntpModelBase
    {
        public IEnumerable<Group> Groups { get { return GetPayload(); } }

        private string _tempFilePath;

        private static readonly ILog Logger = new Log("GroupCollection");

        public GroupCollection(NntpResponse response)
        {
            Parse(response);
        }

        public override void Dispose()
        {
            try
            {
                if (File.Exists(_tempFilePath))
                    File.Delete(_tempFilePath);
            }
            catch (Exception)
            {
                Logger.WarnFormat("Could not delete temp file '{0}'", _tempFilePath);
            }
        }

        protected override void Parse(NntpResponse response)
        {
            ThrowIfUnexpectedResponseCode(response, NntpResponseCode.ListOfNewsgroupsFollows);

            _tempFilePath = GetOwnedPayload(response);
        }

        private IEnumerable<Group> GetPayload()
        {
            return LazyReadPayload(_tempFilePath, LineToGroup);
        }

        private Group LineToGroup(string line)
        {
            //alt.dreams 0000023459 0000020620 y

            var parts = line.Split(new[] {' '}, 4);

            string name = null;
            string startIdVal = null;
            string endIdVal = null;
            long startId = 0;
            long endId = 0;
            string canPostVal = null;
            bool canPost = false;
            try
            {
                name = parts[0];
                endIdVal = parts[1];
                startIdVal = parts[2];
                canPostVal = parts[3];

                if (!string.IsNullOrWhiteSpace(startIdVal))
                    startId = Convert.ToInt64(startIdVal);
                if (!string.IsNullOrWhiteSpace(endIdVal))
                    endId = Convert.ToInt64(endIdVal);
                if (!string.IsNullOrWhiteSpace(canPostVal))
                {
                    canPost = canPostVal.Equals("y");
                }

                return new Group(name, startId, endId, canPost);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}