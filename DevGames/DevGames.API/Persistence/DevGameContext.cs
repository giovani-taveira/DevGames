using DevGames.API.Entities;

namespace DevGames.API.Persistence
{
    public class DevGameContext
    {
        public DevGameContext()
        {
            Boards = new List<Board>();
        }
        public List<Board> Boards { get; private set; }
    }


}
