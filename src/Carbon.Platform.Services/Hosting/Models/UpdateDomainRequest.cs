namespace Carbon.Platform.Hosting
{
    public class UpdateDomainRequest
    {
        public UpdateDomainRequest(
            long id, 
            long? registrationId = null, 
            long? certificateId = null)
        {
            Id             = id;
            RegistrationId = registrationId;
            CertificateId  = certificateId;
        }

        public long Id { get; }

        public long? CertificateId { get; }

        public long? RegistrationId { get; }
    }
}