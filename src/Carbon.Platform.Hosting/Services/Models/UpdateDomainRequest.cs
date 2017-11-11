namespace Carbon.Platform.Hosting
{
    public class UpdateDomainRequest
    {
        public UpdateDomainRequest(
            long id, 
            long? registrationId = null, 
            long? certificateId = null,
            long? environmentId = null)
        {
            Validate.Id(id);

            Id             = id;
            RegistrationId = registrationId;
            CertificateId  = certificateId;
            EnvironmentId  = environmentId;
        }

        public long Id { get; }

        public long? CertificateId { get; }
        
        public long? EnvironmentId { get; }

        public long? RegistrationId { get; }
    }
}