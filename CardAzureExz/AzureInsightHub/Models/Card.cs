using System.ComponentModel.DataAnnotations;

namespace AzureInsightHub.Models
{
    public class Card
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public bool Visible { get; set; }
    }
}
