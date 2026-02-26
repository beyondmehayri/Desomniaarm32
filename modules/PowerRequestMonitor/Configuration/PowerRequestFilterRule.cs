using System.Text.RegularExpressions;

namespace MadWizard.Desomnia.PowerRequest.Configuration
{
    public class PowerRequestFilterRule
    {
        public required string Name { get; set; }

        private string? Text { get; set; }

        public Regex Pattern => Text != null ? new Regex(Text) : throw new ArgumentNullException("pattern");

        public FilterRuleType Type { get; set; } = FilterRuleType.MustNot;
    }

    public enum FilterRuleType
    {
        MustNot = 0,
        Must
    }
}
