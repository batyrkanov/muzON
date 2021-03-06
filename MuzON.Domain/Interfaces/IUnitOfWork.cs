﻿using MuzON.Domain.Entities;
using MuzON.Domain.Identity;
using System;
using System.Threading.Tasks;

namespace MuzON.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Artist> Artists { get; }
        IRepository<Band> Bands { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Rating> Ratings { get; }
        IRepository<Country> Countries { get; }
        IRepository<Genre> Genres { get; }
        IRepository<Playlist> Playlists { get; }
        IRepository<Song> Songs { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        Task SaveAsync();
        void Save();
    }
}
