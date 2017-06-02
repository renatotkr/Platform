NOTE: we prematurally versioned Carbon.Platform and Carbon.Platform.Abstractions at 1.0.0
We'll continue to make breaking API changes in minor releases until we reach 2.0.
Services & management libraries will stabalize at v1.

1.43.0
- Breakout CI

1.42.0
- Breakout EncryptionKeys and DataEncryptionKey into KMS

1.40.0
- Add package details to Programs & Website releases.
- Add Urls & Runtime to Programs & ProgramReleases
- Simplify Deployment Record
- Add Version to IProgram contract

1.39.0
- Add Id to RepositoryBranch
- Remove BranchCount from Repository
- Change RepositoryFile key to (branchId, path)

1.38.0
- Add SecurityGroupIds to NetworkInterface
- Rename MachineImageInfo -> Image
- Add Details to: Build, Deployment, Program, ProgramRelease, Image, LoadBalancer, Volume, Channel, and Queue
- Add OwnerId to Image & HealthCheck
- Add InitiatorId to Build & Deployment
- Introduce Cluster and consolidate Environment Locations, Host Groups, and DatabaseClusters
- Add BuildProject

1.37.0
- Change ResourceType from a structure to a class 
- Add Build
- Simplify Activity model
- Move User to Iam namespace and add details and suspended properties
- Rename Apps to Programs to support Services and Tasks
- Add OwnerId to remaining top level objects 
- Make host addresses a string[] to to support names in the future.
- Move Repositories to Storage layer

1.36.0
- Introduce Slugs
- Add optional unique slug to environments 
- Add HostGroupProperties
- Add HostTemplateProperties
 
- BREAKING ------------------------------------------------------------
  To transition to IPV6, we need to add support for mutiple address blocks on networks & subnets
 
- NetworkInfo : Change CidrBlock(string) to AddressBlocks(string[])
- SubnetInfo  : Change Cidr(string)      to AddressBlocks(string[])

1.35.0
- Decouple Apps from their environments
- Decouple Load Balancers from environments (they may service multiple environments)

1.36.0
- Move Disruptions to Diagnostics & rename to Issue

1.37.0
- Add default sequence names...

1.32.0
- Add Expires to DataEncryptionKey

1.31.0
- Add DataEncryptionKeys
- Add Domains

- Ensure all keys are under 767 bytes

1.30.0
- Add Users

1.29.0
- Add Channels

1.27.0
- Add Queues

1.26.0
- Add Buckets

1.25.0
- Refactor Locations

1.23.0
- Add Database, DatabaseBackup, DatabaseCluster, & DatabaseInstance

1.21.0
- Add Deployment & DeploymentTarget

1.20.0
- Add Environment & EnvironmentLocation

1.19.0
- Rename NetworkProxy -> LoadBalancer
- Rename NetworkAcl -> NetworkSecurityGroup

1.18.0
- Add Grant, RepositoryFile, & RepositoryCommit

1.10.0
- Add HealthCheck & HostTemplate

1.9.0
- Implement abstractions
- Move Net primitives to primitives (future: Carbon.Net.Primitives)

1.8.0
- Add Stats & Heartbeat to NetworkInterafaces & Volumes

1.7.0
- Add NetworkAddress
- Add Terminated to Hosts, and DatabaseInstances
- Add deleted markers to Apps, Backends, Networks, NetworkInterfaces, Images, and Volumes

1.6
- Add NetworkProxy
- Add NetworkRule
- Add NetworkLayer
- Add NetworkListener

1.5
- Add Subnets
- Add LoadBalancers
- Add Certificates

1.4
- Rename Host -> HostInfo

1.3
- Add Outages
- Add MachineTypes

1.1
- Add errorCount
- Add regions