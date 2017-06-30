using System;
using Carbon.Data;
using Carbon.Data.Sequences;
using Carbon.Platform.Diagnostics;

namespace Carbon.Cloud.Logging
{
    public class LoggingDb
    {
        public LoggingDb(IDbContext context)
        {
            // Ensure the db type handlers are registered
   
            // context.Types.TryAdd(new JsonObjectHandler());
            context.Types.TryAdd(new UidHandler());

            Context = context ?? throw new ArgumentNullException(nameof(context));

            Clients        = new Dataset<Client, (Uid, byte[])>(context);

            Exceptions = new Dataset<ExceptionInfo, Uid>(context);

            // UserAgents     = new Dataset<UserAgent, byte[]>();
            // Referrers      = new Dataset<Referrer, byte[>(context);

            Requests = new Dataset<Request, Uid>(context);
            RequestTimings = new Dataset<RequestTiming, (Uid, string)>(context);

            // Logging -----------------------------------------------------------------
            Events = new Dataset<Event, long>(context);
        }

        public IDbContext Context { get; }


        public Dataset<Event, long>                 Events        { get; }

        public Dataset<Client, (Uid, byte[])> Clients { get; }

        public Dataset<Request, Uid>                 Requests { get; }
        public Dataset<RequestTiming, (Uid, string)> RequestTimings { get; }

        public Dataset<ExceptionInfo, Uid> Exceptions { get; }
    }
}