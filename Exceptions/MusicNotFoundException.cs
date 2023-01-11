using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Exceptions
{
    [Serializable]
    public class MusicNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Music not found";
        private int MusicId { get; }
        public MusicNotFoundException(int musicId) : base(DefaultMessage) {
            MusicId = musicId;
        }
    }
}