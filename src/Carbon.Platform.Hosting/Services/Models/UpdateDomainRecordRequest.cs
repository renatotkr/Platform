﻿using System;

using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public readonly struct UpdateDomainRecordRequest
    {
        public UpdateDomainRecordRequest(long id, IRecord value)
        {
            Validate.Id(id);

            Id = id;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public long Id { get; } // RecordId
        
        public IRecord Value { get; } 
    }
}