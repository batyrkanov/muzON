﻿using Microsoft.AspNet.Identity.EntityFramework;
using MuzON.DAL.Configurations;
using MuzON.Domain.Entities;
using MuzON.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzON.DAL.EF
{
    public class MuzONContext : IdentityDbContext<User, Role, Guid, UserLogin, UserRole, UserClaim>
    {
        public DbSet<Band> Bands { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BandSong> BandSongs { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Member> Members { get; set; }

        static MuzONContext() { Database.SetInitializer<MuzONContext>(new DbInitializer());  }

        public MuzONContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            
            // BandSongs Configuarations
            modelBuilder.Configurations.Add(new BandSongConfig());

            // Members Configuarations
            modelBuilder.Configurations.Add(new MemberConfig());

            // User Configuarations
            modelBuilder.Configurations.Add(new UserConfig());

            // Role Configuarations
            modelBuilder.Configurations.Add(new CustomRoleConfig());

            // Song Configuration
            modelBuilder.Configurations.Add(new SongConfig());
        }
    }

    public class ContextFactory : IDbContextFactory<MuzONContext>
    {
        public MuzONContext Create()
        {
            return new MuzONContext("DefaultConnection");
        }
    }
}
