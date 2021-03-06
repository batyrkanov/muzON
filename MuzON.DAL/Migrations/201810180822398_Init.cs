namespace MuzON.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Image = c.Binary(),
                        CountryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: false)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Bands",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Image = c.Binary(),
                        CountryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: false)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(),
                        SongId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.SongId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Value = c.Int(nullable: false),
                        SongId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.SongId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Image = c.Binary(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.BandArtists",
                c => new
                    {
                        Band_Id = c.Guid(nullable: false),
                        Artist_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Band_Id, t.Artist_Id })
                .ForeignKey("dbo.Bands", t => t.Band_Id, cascadeDelete: false)
                .ForeignKey("dbo.Artists", t => t.Artist_Id, cascadeDelete: false)
                .Index(t => t.Band_Id)
                .Index(t => t.Artist_Id);
            
            CreateTable(
                "dbo.SongArtists",
                c => new
                    {
                        Song_Id = c.Guid(nullable: false),
                        Artist_Id = c.Guid(nullable: true),
                    })
                .PrimaryKey(t => new { t.Song_Id, t.Artist_Id })
                .ForeignKey("dbo.Songs", t => t.Song_Id, cascadeDelete: false)
                .ForeignKey("dbo.Artists", t => t.Artist_Id, cascadeDelete: false)
                .Index(t => t.Song_Id)
                .Index(t => t.Artist_Id);
            
            CreateTable(
                "dbo.SongBands",
                c => new
                    {
                        Song_Id = c.Guid(nullable: false),
                        Band_Id = c.Guid(nullable: true),
                    })
                .PrimaryKey(t => new { t.Song_Id, t.Band_Id })
                .ForeignKey("dbo.Songs", t => t.Song_Id, cascadeDelete: false)
                .ForeignKey("dbo.Bands", t => t.Band_Id, cascadeDelete: false)
                .Index(t => t.Song_Id)
                .Index(t => t.Band_Id);
            
            CreateTable(
                "dbo.SongGenres",
                c => new
                    {
                        SongId = c.Guid(nullable: false),
                        GenreId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.GenreId })
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: false)
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: false)
                .Index(t => t.SongId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.PlaylistSongs",
                c => new
                    {
                        Playlist_Id = c.Guid(nullable: false),
                        Song_Id = c.Guid(nullable: true),
                    })
                .PrimaryKey(t => new { t.Playlist_Id, t.Song_Id })
                .ForeignKey("dbo.Playlists", t => t.Playlist_Id, cascadeDelete: false)
                .ForeignKey("dbo.Songs", t => t.Song_Id, cascadeDelete: false)
                .Index(t => t.Playlist_Id)
                .Index(t => t.Song_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PlaylistSongs", "Song_Id", "dbo.Songs");
            DropForeignKey("dbo.PlaylistSongs", "Playlist_Id", "dbo.Playlists");
            DropForeignKey("dbo.SongGenres", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.SongGenres", "SongId", "dbo.Songs");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "SongId", "dbo.Songs");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "SongId", "dbo.Songs");
            DropForeignKey("dbo.SongBands", "Band_Id", "dbo.Bands");
            DropForeignKey("dbo.SongBands", "Song_Id", "dbo.Songs");
            DropForeignKey("dbo.SongArtists", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.SongArtists", "Song_Id", "dbo.Songs");
            DropForeignKey("dbo.Bands", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Artists", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.BandArtists", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.BandArtists", "Band_Id", "dbo.Bands");
            DropIndex("dbo.PlaylistSongs", new[] { "Song_Id" });
            DropIndex("dbo.PlaylistSongs", new[] { "Playlist_Id" });
            DropIndex("dbo.SongGenres", new[] { "GenreId" });
            DropIndex("dbo.SongGenres", new[] { "SongId" });
            DropIndex("dbo.SongBands", new[] { "Band_Id" });
            DropIndex("dbo.SongBands", new[] { "Song_Id" });
            DropIndex("dbo.SongArtists", new[] { "Artist_Id" });
            DropIndex("dbo.SongArtists", new[] { "Song_Id" });
            DropIndex("dbo.BandArtists", new[] { "Artist_Id" });
            DropIndex("dbo.BandArtists", new[] { "Band_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Ratings", new[] { "UserId" });
            DropIndex("dbo.Ratings", new[] { "SongId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "SongId" });
            DropIndex("dbo.Bands", new[] { "CountryId" });
            DropIndex("dbo.Artists", new[] { "CountryId" });
            DropTable("dbo.PlaylistSongs");
            DropTable("dbo.SongGenres");
            DropTable("dbo.SongBands");
            DropTable("dbo.SongArtists");
            DropTable("dbo.BandArtists");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Playlists");
            DropTable("dbo.Genres");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Ratings");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Songs");
            DropTable("dbo.Countries");
            DropTable("dbo.Bands");
            DropTable("dbo.Artists");
        }
    }
}
