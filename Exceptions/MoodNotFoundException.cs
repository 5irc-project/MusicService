using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Exceptions
{
    [Serializable]
    public class MoodNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Mood not found";
        private int MoodId { get; }
        public MoodNotFoundException(int moodId) : base(DefaultMessage) {
            MoodId = moodId;
        }
    }
}