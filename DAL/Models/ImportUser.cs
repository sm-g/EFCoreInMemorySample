using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace DAL.Models
{
    public class ImportUser : IEquatable<ImportUser>
    {
        public int Id { get; set; }

        // Uniq

        public int ImportId { get; set; }

        public int UserId { get; set; }

        // End Uniq

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int? ImportGroupId { get; set; }

        public int? ImportZoneId { get; set; }

        [IgnoreMap]
        public virtual Import _Import { get; set; }

        [IgnoreMap]
        public virtual ImportGroup ImportGroup { get; set; }

        [IgnoreMap]
        public virtual ImportZone ImportZone { get; set; }

        bool IEquatable<ImportUser>.Equals(ImportUser other)
        {
            return other != null
                && Id == other.Id
                && ImportId == other.ImportId
                && UserId == other.UserId
                && FirstName == other.FirstName
                && MiddleName == other.MiddleName
                && LastName == other.LastName
                && ImportGroupId == other.ImportGroupId
                && ImportZoneId == other.ImportZoneId
                ;
        }

        public override string ToString() => $"{Id}:{ImportId},{UserId},{FirstName},{MiddleName},{LastName},{ImportGroup},{ImportZone}";
    }
}