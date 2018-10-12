﻿using AutoMapper;
using MuzON.BLL.DTO;
using MuzON.BLL.Interfaces;
using MuzON.Domain.Entities;
using MuzON.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MuzON.BLL.Services
{
    public class ArtistService : IArtistService
    {
        private IUnitOfWork _unitOfWork;

        public ArtistService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void AddArtist(ArtistDTO artistDTO, List<HttpPostedFileBase> songs)
        {

            Artist artist = Mapper.Map<ArtistDTO, Artist>(artistDTO);
            Country country = _unitOfWork.Countries
                .SearchFor(x => x.Id == artist.CountryId).First();

            if (artistDTO.SelectedBands != null)
            {
                artist.Bands = new List<Band>();

                foreach (var c in _unitOfWork.Bands.SearchFor(co => artistDTO.SelectedBands.Contains(co.Id)))
                {
                    artist.Bands.Add(c);
                }
            }

            if (songs != null)
            {
                artist.Songs = new List<BandSong>();
                foreach (var song in AddSong(songs, artist))
                {
                    artist.Songs.Add(song);
                }
            }
            artist.CountryId = artistDTO.Country.Id;
            artist.Country = country;
            _unitOfWork.Artists.Create(artist);
            _unitOfWork.Save();
        }

        public List<BandSong> AddSong(List<HttpPostedFileBase> songs, Artist artist)
        {
            List<BandSong> bandSongs = new List<BandSong>();
            
                foreach (var songItem in songs)
                {
                    BandSong bandSong = new BandSong();
                    Song song = new Song()
                    {
                        Id = Guid.NewGuid(),
                        Name = Path.GetFileName(songItem.FileName),
                        FileName = songItem.FileName
                    };
                    bandSong.Song = song;
                    bandSong.SongId = song.Id;
                    bandSong.Artist = artist;
                    bandSong.ArtistId = artist.Id;

                    bandSongs.Add(bandSong);
                }
            
            return bandSongs;
        }

        public void DeleteArtist(ArtistDTO artistDTO)
        {
            Artist artist = Mapper.Map<ArtistDTO, Artist>(artistDTO);
            _unitOfWork.Artists.Delete(artist.Id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public ArtistDTO GetArtistById(Guid Id)
        {
            Artist artist = _unitOfWork.Artists.Get(Id);
            return Mapper.Map<Artist, ArtistDTO>(artist);
        }

        public IEnumerable<ArtistDTO> GetArtists()
        {
            var artists = _unitOfWork.Artists.GetAll().ToList();
            return Mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistDTO>>(artists);
        }

        public IEnumerable<SongDTO> GetSongs()
        {
            var songs = _unitOfWork.Songs.GetAll().ToList();
            return Mapper.Map<IEnumerable<SongDTO>>(songs);
        }

        public void UpdateArtist(ArtistDTO artistDTO, Guid[] selectedBands)
        {
            Artist artist = _unitOfWork.Artists.Get(artistDTO.Id);
            var countryId = artist.CountryId;

            Mapper.Map(artistDTO, artist);

            if (selectedBands != null)
            {
                if (artist.Bands == null)
                    artist.Bands = new List<Band>();
                artist.Bands.Clear();
                foreach (var c in _unitOfWork.Bands.SearchFor(co => selectedBands.Contains(co.Id)))
                {
                    artist.Bands.Add(c);
                }
            }

            if (artist.Id != countryId)
            {
                Country country = _unitOfWork.Countries
                    .SearchFor(x => x.Id == artist.CountryId).First();
                artist.Country = country;
            }

            _unitOfWork.Artists.Update(artist);
            _unitOfWork.Save();
        }
    }
}
