using System;
using System.Runtime.Serialization;
using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Hosting
{
    [Dataset("DomainContacts")]
    public class DomainContact
    {
        public DomainContact() { }

        public DomainContact(
            long id,
            DomainContactType type,
            long domainId,
            string firstName,
            string lastName,
            PhysicalAddress address,
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.NotNullOrEmpty(firstName, nameof(firstName));
            Ensure.NotNullOrEmpty(lastName, nameof(lastName));
            Ensure.NotNull(address, nameof(address));

            Id         = id;
            Type       = type;
            DomainId   = domainId;
            FirstName  = firstName;
            LastName   = lastName;
            Address    = JsonObject.FromObject(address);
            Properties = properties ?? new JsonObject();
        }

        [Member("id"), Key(sequenceName: "domainContactId")] // domainId | #
        public long Id { get; }
        
        [Member("type")]
        public DomainContactType Type { get; set; }

        [Member("domainId"), Indexed]
        public long DomainId { get; }

        [Member("firstName")]
        [StringLength(100)]
        public string FirstName { get; }

        [Member("lastName")]
        [StringLength(100)]
        public string LastName { get; }

        [Member("organizationName")]
        [StringLength(100)]
        public string OrganizationName { get; set; }

        [Member("email")]
        [StringLength(253)]
        public string Email { get; set; }
        
        [Member("phone")]
        [StringLength(100)]
        public string Phone { get; set; }

        [Member("fax")]
        [StringLength(100)]
        public string Fax { get; set; }

        [Member("address")] // PhysicalAddress
        public JsonObject Address { get; set; }
        
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Timestamps

        [Member("validated")]
        public DateTime? Validated { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    public class PhysicalAddress
    {
        public PhysicalAddress() { }

        public PhysicalAddress(
            string line1,
            string line2, 
            string city, 
            string region, 
            string country,
            string postalCode)
        {
            Lines      = new[] { line1, line2 };
            City       = city;
            Region     = region;
            Country    = country;
            PostalCode = postalCode;
        }

        // address1, address2, address3
        [DataMember(Name = "lines", Order = 1)]
        public string[] Lines { get; set; }
        
        [DataMember(Name = "city", Order = 2)]
        public string City { get; set; } // DependentLocality

        [DataMember(Name = "region", Order = 3)]
        public string Region { get; set; }

        [DataMember(Name = "country", Order = 4)]
        public string Country { get; set; }

        [DataMember(Name = "postalCode", Order = 5)]
        public string PostalCode { get; set; }
    }
}