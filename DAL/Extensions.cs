using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore
{
    internal static class ModelBuilderExtensions
    {
        public static void SetSimpleUnderscoreTableNameConvention(this ModelBuilder modelBuilder,
            bool preserveAcronyms,
            IDictionary<string, string> propertyMap = null,
            IDictionary<string, string> entityMap = null
            )
        {
            var propMap = propertyMap ?? new Dictionary<string, string>();
            var entMap = entityMap ?? new Dictionary<string, string>();
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    if (propMap.ContainsKey(prop.Name))
                    {
                        prop.Relational().ColumnName = propMap[prop.Name];
                    }
                    else
                    {
                        var underscoredProp = AddUndercoresToSentence(prop.Name, preserveAcronyms);
                        prop.Relational().ColumnName = underscoredProp.ToLowerInvariant();
                    }
                }

                var entName = entity.DisplayName();
                if (entMap.ContainsKey(entName))
                {
                    entity.Relational().TableName = entMap[entName];
                }
                else
                {
                    var underscored = AddUndercoresToSentence(entity.DisplayName(), preserveAcronyms);
                    entity.Relational().TableName = underscored.ToLowerInvariant();
                }
            }
        }

        public static string AddUndercoresToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) &&
                    (
                        (text[i - 1] != '_' && !char.IsUpper(text[i - 1]))
                    ||
                        (preserveAcronyms &&
                        char.IsUpper(text[i - 1]) &&
                        i < text.Length - 1 &&
                        !char.IsUpper(text[i + 1]))
                    )
                )
                {
                    newText.Append('_');
                }
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}