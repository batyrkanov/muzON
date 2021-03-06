﻿using AutoMapper;
using MuzON.BLL.DTO;
using MuzON.BLL.Interfaces;
using MuzON.Domain.Entities;
using MuzON.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzON.BLL.Services
{
    public class BandService : IBandService
    {
        private IUnitOfWork _unitOfWork;

        public BandService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void AddBand(BandDTO bandDTO)
        {
            var band = Mapper.Map<Band>(bandDTO);
            Country country = _unitOfWork.Countries.SearchFor(x => x.Id == band.CountryId).First();
            if (bandDTO.SelectedArtists != null)
            {
                band.Artists = new List<Artist>();

                foreach (var c in _unitOfWork.Artists.SearchFor(co => bandDTO.SelectedArtists.Contains(co.Id)))
                {
                    band.Artists.Add(c);
                }
            }
            band.CountryId = country.Id;
            band.Country = country;
            _unitOfWork.Bands.Create(band);
            _unitOfWork.Save();
        }

        public void DeleteBand(BandDTO bandDTO)
        {
            Band band = Mapper.Map<Band>(bandDTO);
            _unitOfWork.Bands.Delete(band.Id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public BandDTO GetBandById(Guid id)
        {
            var band = _unitOfWork.Bands.Get(id);
            return Mapper.Map<BandDTO>(band);
        }

        public IEnumerable<BandDTO> GetBands()
        {
            var bandsDTO = _unitOfWork.Bands.GetAll().ToList();
            return Mapper.Map<IEnumerable<BandDTO>>(bandsDTO);
        }

        public void UpdateBand(BandDTO bandDTO)
        {
            var band = _unitOfWork.Bands.Get(bandDTO.Id);
            var countryId = band.CountryId == bandDTO.Country.Id ? band.CountryId : bandDTO.Country.Id;
            List<Song> songs = new List<Song>();
            foreach (var song in band.Songs)
            {
                songs.Add(song);
            }
            bandDTO.Country = null;
            Mapper.Map(bandDTO, band);
            Country country = _unitOfWork.Countries
                    .SearchFor(x => x.Id == countryId).First();
            band.Country = country;
            band.CountryId = country.Id;

            if (bandDTO.SelectedArtists.Count > 0)
            {
                if (band.Artists == null)
                    band.Artists = new List<Artist>();
                band.Artists.Clear();
                foreach (var c in _unitOfWork.Artists
                            .SearchFor(co => bandDTO.SelectedArtists.Contains(co.Id)))
                {
                    band.Artists.Add(c);
                }
            }
            band.Songs = songs;
            _unitOfWork.Bands.Update(band);
            _unitOfWork.Save();
        }
    }
}
