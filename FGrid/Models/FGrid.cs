namespace FGrid.Models
{
    public class FGrid<T> where T : class
    {
        public string SourceUrl { get; }
        public FGridColumns<T> Columns { get; set; }

        public FGrid(string sourceUrl)
        {
            SourceUrl = sourceUrl;
            Columns = new FGridColumns<T>(this);
        }
    }
}