using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Avatar.App.Final.Models
{
    public class FinalVote
    {
        public long Id { get; set; }
        public long FinalistId { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
