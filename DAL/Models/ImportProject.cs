using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace DAL.Models
{
    public class ImportProject : IEquatable<ImportProject>
    {
        public int Id { get; set; }

        // Uniq

        public int ImportId { get; set; }

        public int ProjectId { get; set; }

        // End Uniq

        [Required]
        public string Name { get; set; }

        public int ImportCustomerId { get; set; }

        [IgnoreMap]
        public virtual Import _Import { get; set; }

        [IgnoreMap]
        public virtual ImportCustomer ImportCustomer { get; set; }

        bool IEquatable<ImportProject>.Equals(ImportProject other)
        {
            return other != null
                && Id == other.Id
                && ImportId == other.ImportId
                && ProjectId == other.ProjectId
                && Name == other.Name
                && ImportCustomerId == other.ImportCustomerId
                ;
        }

        public override string ToString() => $"{Id}:{ImportId},{ProjectId},{Name},{ImportCustomer}";
    }
}