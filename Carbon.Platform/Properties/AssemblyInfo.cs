﻿using System.Reflection;

[assembly: AssemblyTitle("Carbon Platform")]
[assembly: AssemblyProduct("Carbon")]
[assembly: AssemblyCopyright("© 2012 - 2016 Jason Nelson")]

[assembly: AssemblyVersion("1.0.0")]

/*
  1.0.0 (2016-09-01)
  - Breakout packaging
  - Move semver to data
  - Move data model to SQL (remove depedency from Dynamo)

  0.4.0 (2015-10-15)
  - Add BitBucket
  - Add GitHub

  0.3.0 (2015-10-01)
 -----------------------------------------------------
 - Update to .NET 4.6
 - Remove Carbon.Core depedency

  0.1.1 (2014-04-10)
 -----------------------------------------------------
 - Breakout observerations

  0.1.1 (2014-04-10)
 -----------------------------------------------------
 - Change application version to an Int32 (The version record can keep track of details)
 - Add AppError
 
 0.1.0 (2014-01-10)
 -----------------------------------------------------
 - Remove App & Frontend Environments (Each app should have a single environment and always move forward)
 
 0.0.9 (2013-02-16)
 -----------------------------------------------------
 - Flatten namespaces [Breaking]
 
 0.0.8 (2013-02-15)
 -----------------------------------------------------
 - Overall restructuring [Breaking]
 
 - Added IPackage
 
 0.0.6 (2013-01-10)
 ----------------------------------------------------- 
 - Bundle inherts Collection<FileInfo>
 
 - Checksum -> Sha256
 - Carbon.Core
 - Removed depedency on Carbon.Media
 
 0.0.3 (2012-10-01)
 -----------------------------------------------------
 * Hosting *
 - Added IAppHost Contract 
 
 * Scm *
 - Added Repository Class Stubs
 
 * Infrastructure *
 - Renamed DriveInfo to VolumeInfo
 - Added VolumeStatus
 - Added InstanceName Helper
 - Added NetworkAddress
 
 0.0.2 (2012-09-30)
 -----------------------------------------------------
 - Added Bundle
 - Added Package
 - Added PackageStore

 0.0.1 (2012-09-01)
 -----------------------------------------------------
 - Initial
 
*/
