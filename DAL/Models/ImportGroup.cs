using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace DAL.Models
{
    public class ImportGroup : IEquatable<ImportGroup>
    {
        public int Id { get; set; }

        // Uniq

        public int ImportId { get; set; }

        public int GroupId { get; set; }

        // End Uniq

        [Required]
        public string Name { get; set; }

        [IgnoreMap]
        public virtual Import _Import { get; set; }

        bool IEquatable<ImportGroup>.Equals(ImportGroup other)
        {
            return other != null
                && Id == other.Id
                && ImportId == other.ImportId
                && GroupId == other.GroupId
                && Name == other.Name
                ;
        }

        public override string ToString() => $"{Id}:{ImportId},{GroupId},{Name}";
    }
}