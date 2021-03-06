﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuzON.BLL.DTO;

namespace MuzON.BLL.Interfaces
{
    public interface IPlayListService
    {
        IEnumerable<PlaylistDTO> GetPlaylists();
        PlaylistDTO GetPlaylistById(Guid id);
        void AddPlayList(PlaylistDTO playlistDTO);
        void RatePlayList(RatingDTO ratingDTO);
        double PlaylistRating(Guid id);
        double PlaylistRatingFromUser(Guid playlistId, Guid userId);
        void DeletePlaylist(PlaylistDTO playlist);
        void UpdatePlaylist(PlaylistDTO playlistDTO);
    }
}
