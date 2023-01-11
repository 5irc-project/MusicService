using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Exceptions
{
    [Serializable]
    public class PlaylistNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Playlist not found";
        private int PlaylistId { get; }
        public PlaylistNotFoundException(int playlistId) : base(DefaultMessage) {
            PlaylistId = playlistId;
        }
    }
}