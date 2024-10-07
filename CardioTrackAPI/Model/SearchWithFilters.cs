namespace CardioTrackAPI.Model
{
    public class SearchWithFilters<T>
    {
        public IEnumerable<T>? Results { get; set; }
        public int Count { get; set; }
        public SearchWithFilters(IEnumerable<T> results, int count)
        {
            this.Results = results;
            this.Count = count;
        }
    }
}
