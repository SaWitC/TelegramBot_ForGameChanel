using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot_GamesPc.Models
{
    class GameLinkModel
    {
        public int Id { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string GameLink { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public string Title { get; set;}
    }
}
