using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PickemBot.Models;
using PickemBot.Modules;

namespace PickemBot.Data
{
    public class PickemContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Picker> Pickers { get; set; }

        // public DbSet<Player> Players { get; set; }
        
        public string DbPath { get; }
        
        public PickemContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            Console.WriteLine("Database path: " + path + "/pickem.db");
            DbPath = System.IO.Path.Join(path, "pickem.db");
            
            if (Teams.Count() == 0)
            {
                DBSeeding.SeedTeams(this);
            }
        }
        
        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set advancepick navigation
            modelBuilder.Entity<AdvancePicks>()
                .HasMany(c => c.AdvanceTeams)
                .WithMany(t => t.PickedByAdvance);
            modelBuilder.Entity<Team>()
                .HasMany<AdvancePicks>()
                .WithOne(c => c.NoWin);
            modelBuilder.Entity<Team>()
                .HasMany<AdvancePicks>()
                .WithOne(c => c.Undefeated);
            
            
            // Set champion navigation
            modelBuilder.Entity<ChampionPicks>()
                .HasMany(c => c.QuarterFinalists)
                .WithMany(t => t.PickedQuarterFinals);
            modelBuilder.Entity<ChampionPicks>()
                .HasMany(c => c.SemiFinalists)
                .WithMany(t => t.PickedSemiFinals);
            modelBuilder.Entity<Team>()
                .HasMany<ChampionPicks>()
                .WithOne(c => c.Winner);
            

            // Set picker navigation
            modelBuilder.Entity<Picker>()
                .HasOne(p => p.ChallengerPicks);
            
            modelBuilder.Entity<Picker>()
                .HasOne(p => p.LegendPicks);
            
            modelBuilder.Entity<Picker>()
                .HasOne(p => p.ChampionPicks);
        }
        #endregion
        
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

       
    }
}