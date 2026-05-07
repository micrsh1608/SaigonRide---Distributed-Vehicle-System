using System;

namespace SaigonBus.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public string Username
        {
            get;
            set;
        }

        public string Note
        {
            get;
            set;
        }

        public int Rating
        {
            get;
            set;
        }

        public DateTime CreatedAt
        {
            get;
            set;
        }
        = DateTime.Now;
    }
}
