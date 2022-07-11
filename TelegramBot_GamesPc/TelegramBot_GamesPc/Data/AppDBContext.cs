using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TelegramBot_GamesPc.Models;

namespace TelegramBot_GamesPc.Data
{
    class AppDBContext:DbContext
    {
        public AppDBContext()
        {
            //Database.EnsureDeleted();

            Database.EnsureCreated();
        }

        public DbSet<GameLinkModel> gameLinkModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TelegramBotDB;Trusted_Connection=True;");
        }
    }
}
