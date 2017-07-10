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

            Context        = context ?? throw new ArgumentNullException(nameof(context));

            Clients        = new Dataset<Client, (Uid, byte[])>(context);

            Exceptions     = new Dataset<ExceptionInfo, Uid>(context);

            Requests       = new Dataset<Request, Uid>(context);
            RequestTimings = new Dataset<RequestTiming, (Uid, string)>(context);

            Events         = new Dataset<Event, Uid>(context);
        }

        public IDbContext Context { get; }

        public Dataset<Client, (Uid, byte[])>        Clients        { get; }
        public Dataset<Event, Uid>                   Events         { get; }
        public Dataset<ExceptionInfo, Uid>           Exceptions     { get; }
        public Dataset<Request, Uid>                 Requests       { get; }
        public Dataset<RequestTiming, (Uid, string)> RequestTimings { get; }
    }
}