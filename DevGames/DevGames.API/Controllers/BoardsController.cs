using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly DevGameContext context;

        public BoardsController(DevGameContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(context.Boards);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var board = context.Boards.FirstOrDefault(b => b.Id == id);

            if (board == null)
                return NotFound();

            return Ok(board);
        }

        [HttpPost]
        public IActionResult Post(AddBoardInputModel model)
        {
            var board = new Board(model.Id, model.GameTitle, model.Description, model.Rules);
            context.Boards.Add(board);

            return CreatedAtAction("GetById", new { id = model.Id}, model);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateBoardInputModel model)
        {
            var board = context.Boards.SingleOrDefault(b => b.Id == id);

            if (board == null)
                return NotFound();

            board.Update(model.Description, model.Rules);
            return NoContent();
        }
    }
}
