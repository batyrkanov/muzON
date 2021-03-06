﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using MuzON.BLL.DTO;
using MuzON.BLL.Interfaces;
using MuzON.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MuzON.Web.Controllers
{
    public class PlaylistsController : BaseController
    {
        public PlaylistsController(IGenreService genre, IPlayListService playList, ISongService song, ICommentService comment)
            : base(genre, playList, song, comment)
        { }

        // GET: Playlists
        public ActionResult Index()
        {
            var playlistDTOs = playListService.GetPlaylists();
            var playlists = Mapper.Map<IEnumerable<PlaylistViewModel>>(playlistDTOs);
            foreach (var item in playlists)
            {
                foreach (var item2 in playlistDTOs.Where(x => x.Id == item.Id).ToList().Select(x => x.Songs).ToList())
                {
                    item.Songs = Mapper.Map<List<SongViewModel>>(item2.ToList());
                    item.Rating = playListService.PlaylistRating(item.Id);
                }
            }
            return View(playlists);
        }

        public ActionResult Details(Guid id)
        {
            var playlistDTO = playListService.GetPlaylistById(id);
            var playlist = Mapper.Map<PlaylistViewModel>(playlistDTO);
            playlist.Rating = playListService.PlaylistRatingFromUser(playlist.Id, Guid.Parse(User.Identity.GetUserId()));
            playlist.Songs = GetAllSongs(playlistDTO.Songs.Select(x => x.Id)).Where(x => x.IsSelected).ToList();
            return PartialView("_Details", playlist);
        }

        public ActionResult Create()
        {
            var playlist = new PlaylistViewModel
            {
                Image = String.Empty,
                Songs = GetAllSongs()
            };
            return PartialView("_PlaylistAction", playlist);
        }

        public ActionResult Edit(Guid id)
        {
            var playlist = playListService.GetPlaylistById(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            PlaylistViewModel playlistViewModel = Mapper.Map<PlaylistViewModel>(playlist);
            playlistViewModel.Songs = GetAllSongs(playlist.Songs.Select(x => x.Id));
            return PartialView("_PlaylistAction", playlistViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateEdit(PlaylistViewModel playlistViewModel)
        {
            if (ModelState.IsValid)
            {
                playlistViewModel.Comments = playlistViewModel.Id == Guid.Parse("00000000-0000-0000-0000-000000000000") 
                                             ? null 
                                             : GetAllComments(playlistViewModel.Id);
                playlistViewModel.Songs = GetAllSongs(playlistViewModel.SelectedSongs).Where(x => x.IsSelected).ToList();
                var playlist = Mapper.Map<PlaylistDTO>(playlistViewModel);
                if(playlist.Id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    playlist.Id = Guid.NewGuid();
                    playlist.Image = util.SetImage(Request.Files["uploadImage"], playlist.Image);
                    playListService.AddPlayList(playlist);
                    logger.InfoLog("Playlist",
                                "added",
                                playlist.Name,
                                playlist.Id,
                                User.Identity.Name);
                    return Json(new { data = "success" });
                }
                else
                {
                    playlist.Image = util.SetImage(Request.Files["uploadImage"], playlist.Image, Request.Form["image"]);
                    playListService.UpdatePlaylist(playlist);
                    logger.InfoLog("Playlist",
                                "edited",
                                playlist.Name,
                                playlist.Id,
                                User.Identity.Name);
                    return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { playlistViewModel, errorMessage = util.GetErrorList(ModelState.Values) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddComment(Guid id)
        {
            var playlist = playListService.GetPlaylistById(id);
            if (playlist != null)
            {
                var userId = User.Identity.GetUserId();
                var text = Request.Form["text"];
                SaveComment(Guid.Parse(userId), text, null, id);
                GetComments(id);
                return Json(new { data = "success", id });
            }
            return Json(new { playlist, errorMessage = util.GetErrorList(ModelState.Values) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComments(Guid id)
        {
            var playlist = playListService.GetPlaylistById(id);
            return PartialView("_Comments", Mapper.Map<PlaylistViewModel>(playlist));
        }

        public JsonResult RatePlaylist(Guid id, string ratingFromUser)
        {
            var playlist = playListService.GetPlaylistById(id);
            if (playlist != null)
            {
                var userId = User.Identity.GetUserId();
                var rate = double.Parse(ratingFromUser.Replace('.', ','));
                var rating = new RatingViewModel
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(userId),
                    Value = rate,
                    SongId = null,
                    PlaylistId = playlist.Id
                };
                var ratingDto = Mapper.Map<RatingDTO>(rating);
                playListService.RatePlayList(ratingDto);
                return Json(new { data = "success", id });
            }
            return Json(new { playlist, errorMessage = util.GetErrorList(ModelState.Values) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(Guid id)
        {
            var playlist = playListService.GetPlaylistById(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            PlaylistViewModel playlistViewModel = Mapper.Map<PlaylistViewModel>(playlist);
            return PartialView("_Delete", playlistViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(Guid id)
        {
            var playlist = playListService.GetPlaylistById(id);
            playListService.DeletePlaylist(playlist);
            logger.InfoLog("Playlist",
                            "deleted",
                            playlist.Name,
                            playlist.Id,
                            User.Identity.Name);
            return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
        }

        protected List<SongViewModel> GetAllSongs(IEnumerable<Guid> selectedIds = null)
        {
            IEnumerable<SongDTO> songDTOs = songService.GetSongs();

            IEnumerable<SongViewModel> songViewModels = Mapper.Map<IEnumerable<SongViewModel>>(songDTOs);

            if (selectedIds != null)
            {
                foreach (SongViewModel model in songViewModels)
                {
                    model.IsSelected = selectedIds.Contains(model.Id);
                }
            }

            return songViewModels.OrderBy(x => x.Name).ToList();
        }

        protected List<CommentViewModel> GetAllComments(Guid? playlistId = null)
        {
            IEnumerable<CommentDTO> commentDTOs = playlistId == null
                                        ? commentService.GetComments()
                                        : commentService.GetCommentsByPlaylistId(playlistId.Value);

            IEnumerable<CommentViewModel> commentViewModels = Mapper.Map<IEnumerable<CommentViewModel>>(commentDTOs);

            return commentViewModels.ToList();
        }
    }
}