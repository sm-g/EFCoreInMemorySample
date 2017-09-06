using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace DAL.Models
{
    public class ImportCustomer : IEquatable<ImportCustomer>
    {
        public int Id { get; set; }

        // Uniq

        public int ImportId { get; set; }

        public int CustomerId { get; set; }

        // End Uniq

        [Required]
        public string Name { get; set; }

        [IgnoreMap]
        public virtual Import _Import { get; set; }

        bool IEquatable<ImportCustomer>.Equals(ImportCustomer other)
        {
            return other != null
                && Id == other.Id
                && ImportId == other.ImportId
                && CustomerId == other.CustomerId
                && Name == other.Name
                ;
        }

        public override string ToString() => $"{Id}:{ImportId},{CustomerId},{Name}";
    }
}