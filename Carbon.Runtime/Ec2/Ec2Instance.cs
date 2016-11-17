﻿using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Carbon.Computing
{
    using Json;

    public class Ec2Instance
    {
        const string baseUri = "http://169.254.169.254";

        // http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/AESDG-chapter-instancedata.html

        public string InstanceId { get; set; }

        public string ImageId { get; set; }

        public string InstanceType { get; set; }

        public string Region { get; set; }

        public string AvailabilityZone { get; set; }

        public IPAddress PrivateIp { get; set; }

        private static readonly HttpClient httpClient = new HttpClient {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public static Task<string> GetInstanceId()
            => httpClient.GetStringAsync(baseUri + "/latest/meta-data/instance-id");

        public static async Task<IPAddress> GetPublicIpAsync()
        {
            var result = await httpClient.GetStringAsync(baseUri + "/latest/meta-data/public-ipv4");

            if (result.Length == 0) return null;

            return IPAddress.Parse(result);
        }

        public static Task<byte[]> GetUserData()
            => httpClient.GetByteArrayAsync(baseUri + "/latest/user-data");

        public static Task<string> GetUserDataString()
            => httpClient.GetStringAsync(baseUri + "/latest/user-data");

        public static Task<string> GetAvailabilityZone()
            => httpClient.GetStringAsync(baseUri + "/latest/meta-data/placement/availability-zone");

        public static async Task<Ec2Instance> FetchAsync()
        {
            var text = await httpClient.GetStringAsync(baseUri + "/latest/dynamic/instance-identity/document").ConfigureAwait(false);

            return JsonObject.Parse(text).As<Ec2Instance>();
        }
    }
}

/*
{
  "instanceId" : "i-bf7e3395",
  "billingProducts" : [ "bp-6ba54002" ],
  "version" : "2010-08-31",
  "imageId" : "ami-f647b19e",
  "architecture" : "x86_64",
  "pendingTime" : "2014-07-05T01:59:45Z",
  "instanceType" : "c3.large",
  "accountId" : "416372880389",
  "kernelId" : null,
  "ramdiskId" : null,
  "region" : "us-east-1",
  "availabilityZone" : "us-east-1d",
  "privateIp" : "10.0.3.20",
  "devpayProductCodes" : null
}
*/
