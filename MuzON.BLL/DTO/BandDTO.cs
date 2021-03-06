﻿using MuzON.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzON.BLL.DTO
{
    public class BandDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }
        public CountryDTO Country { get; set; }
        public List<Guid> SelectedArtists { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<ArtistDTO> Artists { get; set; }
        public ICollection<SongDTO> Songs { get; set; }
    }
}
