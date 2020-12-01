namespace FGrid.Models
{
    public class FGridFilterOption
    {
        public FGridFilterOption(string value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
        
        public string Value { get; set; }
        public string DisplayText { get; set; }
    }
}