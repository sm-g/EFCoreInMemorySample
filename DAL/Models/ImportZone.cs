using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;

namespace DAL.Models
{
    public class ImportZone : IEquatable<ImportZone>
    {
        public int Id { get; set; }

        // Uniq

        public int ImportId { get; set; }

        public int ZoneId { get; set; }

        // End Uniq

        [Required]
        public string Name { get; set; }

        [IgnoreMap]
        public virtual Import _Import { get; set; }

        bool IEquatable<ImportZone>.Equals(ImportZone other)
        {
            return other != null
                && Id == other.Id
                && ImportId == other.ImportId
                && ZoneId == other.ZoneId
                && Name == other.Name
                ;
        }

        public override string ToString() => $"{Id}:{ImportId},{ZoneId},{Name}";
    }
}