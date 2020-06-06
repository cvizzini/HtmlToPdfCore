namespace HtmlToPdfCore
{
    public class QuoteTableObject
    {
        public QuoteTableObject(string description, string quantity, string cost)
        {
            Description = description;
            Quantity = quantity;
            Cost = cost;
        }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Cost { get; set; }
    }
}
