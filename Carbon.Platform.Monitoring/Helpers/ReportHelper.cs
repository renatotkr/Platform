namespace Carbon.Platform
{
    using System;
    using System.Diagnostics;

    public  class ReportHelper
    {
        public static ProcessorReport Generate(ProcessorObservation one, ProcessorObservation two)
        {
            float processingTime = CounterSample.Calculate(one.ProcessorTimeSample, two.ProcessorTimeSample);

            return new ProcessorReport {
                Start = one.Date,
                End = two.Date,
                ProcessingTime = (float)Math.Round(processingTime, 3)
            };
        }

        public static MachineReport Generate(MachineObservation one, MachineObservation two)
        {
            var processor = Generate(one.Processor, two.Processor);

            var memory = LocalMemory.Observe();

            var report = new MachineReport
            {
                ReportId = ReportIdentity.Create(one.Date, two.Date),
                MachineId = one.MachineId,
                CpuTime = processor.ProcessingTime,
                MemoryUsed = memory.Used
            };

            if (one.NetworkInterfaces != null && one.NetworkInterfaces.Length > 0)
            {
                var len = one.NetworkInterfaces.Length;

                for (int i = 0; i < len; i++)
                {
                    var r = Generate(one.NetworkInterfaces[i], two.NetworkInterfaces[i]);

                    report.SendRate += r.SendRate;
                    report.ReceiveRate += r.ReceiveRate;
                }
            }

            return report;
        }

        public static AppInstanceReport Generate(AppInstanceObservation one, AppInstanceObservation two)
        {
            return new AppInstanceReport {
                AppInstanceId = one.AppInstance.GetId(),
                ReportId = ReportIdentity.Create(one.Date, two.Date),
                RequestRate = CounterSample.Calculate(one.RequestRateSample, two.RequestRateSample),
                ErrorRate = CounterSample.Calculate(one.ErrorRateSample, two.ErrorRateSample),
                TotalRequestCount = two.TotalRequestsSample.RawValue
            };
        }

        public static NetworkInterfaceReport Generate(NetworkInterfaceObservation o1, NetworkInterfaceObservation o2)
        {
            int receiveRate = (int)CounterSample.Calculate(o1.ReceiveRateSample, o2.ReceiveRateSample); // bytes per second
            int sendRate = (int)CounterSample.Calculate(o1.SendRateSample, o2.SendRateSample);          // bytes per second

            return new NetworkInterfaceReport
            {
                ReportId = ReportIdentity.Create(o1.Date, o2.Date),
                MacAddress = o1.NetworkInterface.MacAddress,
                ReceiveRate = receiveRate,
                SendRate = sendRate
            };
        }

        public static VolumeReport Generate(VolumeObservation one, VolumeObservation two)
        {
            var report = new VolumeReport {
                VolumeId = one.Volume.FullName,
                ReportId = ReportIdentity.Create(one.Date, two.Date),
                Available = two.Available,
                Used = two.Used,
                Size = two.Size,
                ReadTime = CounterSample.Calculate(one.ReadTimeSample, two.ReadTimeSample),
                WriteTime = CounterSample.Calculate(one.WriteTimeSample, two.WriteTimeSample),
                ReadRate = (int)CounterSample.Calculate(one.ReadRateSample, two.ReadRateSample),
                WriteRate = (int)CounterSample.Calculate(one.WriteRateSample, two.WriteRateSample)
            };

            return report;
        }
    }
}
