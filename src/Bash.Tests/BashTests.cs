using System;

using Bash.Commands;

using Xunit;

namespace Bash.Tests
{
    using static Command;
    using static BashExpression;

    public class BashTests
    {
        [Fact]
        public void CreateScript()
        {
            var script = new BashScript
            {                
                Comment("Setup environment"),

                Set("INSTANCE_ID", "$(curl -s http://169.254.169.254/latest/meta-data/instance-id)"),

                new IfStatement(IsDefined("$1"), Set("APP_NAME",    "$1")),
                new IfStatement(IsDefined("$2"), Set("APP_VERSION", "$2")),

                // TODO: Use shorter syntax...
                new IfStatement(IsDefined("$3"),
                    then     : new[] { Set("APP_PACKAGE_URL", "$3") },
                    elseThen : new[] { Set("APP_PACKAGE_URL", "s3://folder/$APP_NAME/$APP_VERSION.tar.gz") }
                ),

                Set("APP_ROOT", "/var/apps/$APP_NAME/$APP_VERSION"),

                Empty, Echo("Configuring $APP_NAME/$APP_VERSION"),

                // Install depedencies
                Apt.Install("awscli", "nginx", "libunwind8", "libcurl4-openssl-dev"),

                // Configure S3 to allow encrypted package downloads
                "sudo aws configure set s3.signature_version s3v4",

                // Install SSM  agent
                "cd /tmp",

                Wget.Download(
                    new Uri("https://s3.amazonaws.com/ec2-downloads-windows/SSMAgent/latest/debian_amd64/amazon-ssm-agent.deb")
                ),

                "sudo dpkg -i amazon-ssm-agent.deb",

                Systemctl.Enable("amazon-ssm-agent"),

                // Configure Ngnix ------------------------------------------------------------------------------------------------
                Empty, Echo("Configuring nginx"),

                Wget.Download(
                    url: new Uri("https://unknown/nginx.config"),
                    destination: "/etc/nginx/sites-available/default",
                    sudo: true
                ),

                
                Nginx.Reload(),

                // Setup program directories
                "mkdir -p $APP_ROOT",

                // Install the app ------------------------------------------------------------------------------------------------

                // Create a working directory for download
                "mkdir -r /tmp/programs/$APP_NAME/$APP_VERSION",

                // Download the app
                Aws.S3.Copy(
                    source  : "$APP_PACKAGE_URL",
                    target  : "/tmp/programs/$APP_NAME/$APP_VERSION",
                    options : AwsOptions.Quiet,
                    sudo    : true
                ),

                // Extract the app
                Tar.Extract(
                    file    : "/tmp/programs/$APP_NAME/$APP_VERSION",
                    directory  : "$APP_ROOT",
                    stripFirstLevel: true
                ),

                // Create a symbolic link
                CreateSymbolicLink(
                    "$APP_ROOT", 
                    link    : "/var/apps/$APP_NAME/latest", 
                    options : SymbolicLinkOptions.Symbolic | SymbolicLinkOptions.Force, // update if it already exists
                    sudo:    true
                ),
                
                // Give permissions to www-data
                Chown(owner: "www-data", path: "/var/apps",     recursive: true, sudo: true),

                // Configure the service ------------------------------------------------------------------------------------------
                Empty, Echo("Configuring $APP_NAME.service"),

                // Setup the service
                Aws.S3.Copy(
                    source  : "s3://folder/$APP_NAME/$APPNAME.service",
                    target  : "/etc/systemd/system/$APP_NAME.service",
                    options : AwsOptions.Quiet,
                    sudo    : true
                ),

                Systemctl.Enable("$APP_NAME.service"),
                Systemctl.Start("$APP_NAME.service"),

                // Cleanup -------------------------------------------------------------------------------------------------------
                Empty, Echo("Cleaning up"),

                new Command("rm -r /tmp/programs/$APP_NAME/$APP_VERSION"),
                
                // Report to homebase ---------------------------------------------------------------------------------------------
                Empty, Echo("Phoning home $INSTANCE_ID"),

                "curl --data \"status=running\" https://cloud/hosts/aws:$INSTANCE_ID"
            };


            Assert.Equal(@"#!/bin/bash
# Setup environment
INSTANCE_ID=$(curl -s http://169.254.169.254/latest/meta-data/instance-id)
if [ -n ""$1"" ]; then APP_NAME=$1; fi
if [ -n ""$2"" ]; then APP_VERSION=$2; fi
if [ -n ""$3"" ]; then APP_PACKAGE_URL=$3; else APP_PACKAGE_URL=s3://folder/$APP_NAME/$APP_VERSION.tar.gz; fi
APP_ROOT=/var/apps/$APP_NAME/$APP_VERSION

echo ""Configuring $APP_NAME/$APP_VERSION""
sudo apt install -y awscli nginx libunwind8 libcurl4-openssl-dev
sudo aws configure set s3.signature_version s3v4
cd /tmp
wget -v ""https://s3.amazonaws.com/ec2-downloads-windows/SSMAgent/latest/debian_amd64/amazon-ssm-agent.deb""
sudo dpkg -i amazon-ssm-agent.deb
sudo systemctl enable amazon-ssm-agent

echo ""Configuring nginx""
sudo wget -v ""https://unknown/nginx.config"" -O /etc/nginx/sites-available/default
sudo nginx -s reload
mkdir -p $APP_ROOT
mkdir -r /tmp/programs/$APP_NAME/$APP_VERSION
sudo aws s3 cp --quiet $APP_PACKAGE_URL /tmp/programs/$APP_NAME/$APP_VERSION
tar -xf /tmp/programs/$APP_NAME/$APP_VERSION --strip 1 -C $APP_ROOT
sudo ln -sfn $APP_ROOT /var/apps/$APP_NAME/latest
sudo chown -R www-data /var/apps

echo ""Configuring $APP_NAME.service""
sudo aws s3 cp --quiet s3://folder/$APP_NAME/$APPNAME.service /etc/systemd/system/$APP_NAME.service
sudo systemctl enable $APP_NAME.service
sudo systemctl start $APP_NAME.service

echo ""Cleaning up""
rm -r /tmp/programs/$APP_NAME/$APP_VERSION

echo ""Phoning home $INSTANCE_ID""
curl --data ""status=running"" https://cloud/hosts/aws:$INSTANCE_ID", script.ToString());
        }
    }
        
}