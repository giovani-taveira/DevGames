using AutoMapper;
using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence;
using DevGames.API.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DevGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBoardRepository repository;

        public BoardsController(IMapper mapper, IBoardRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <summary>
        /// Recuperar todos os boards
        /// </summary>
        /// <returns>Board</returns>
        /// <response code="200">Sucess</response>
        /// <response code="404">Boards não encontrados</response>

        [HttpGet] 
        public IActionResult GetAll()
        {
            var boards = repository.GetAll();
            Log.Information($"{boards.Count()} boards retrived");

            if (boards.Count() == 0)
                return NotFound();

            return Ok(boards);
        }

        /// <summary>
        /// Recuperar um board através do id
        /// </summary>
        /// <remarks>
        /// Exemplo da requisição de um board
        /// {
        /// "id" : "1",
        /// }
        /// </remarks>
        /// <param name="model">Id do board</param>
        /// <returns>Board encontrado pelo id</returns>
        /// <response code="200">Board Encontrado</response>
        /// <response code="404">Board Inexistente</response>

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var board = repository.GetById(id);

            if (board == null)
                return NotFound();

            return Ok(board);
        }

        /// <summary>
        /// Publicar um novo board
        /// </summary>
        /// <remarks>
        /// Exemplo da criação de um board
        /// {
        /// "gameTitle" : "Red Dead Redemption 2",
        /// "Description" : "Um jogo de velho oeste",
        /// "rules" : "1. Sem SPAM."
        /// }
        /// </remarks>
        /// <param name="model">Informações do board</param>
        /// <returns>Objeto Criado</returns>
        /// <response code="201">Board Criado</response>
        /// <response code="400">Informações Inválidas</response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(AddBoardInputModel model)
        {
            var board = mapper.Map<Board>(model);
            repository.Add(board);

            return CreatedAtAction("GetById", new { id = board.Id}, model);
        }

        /// <summary>
        /// Atualizar as informações de um board
        /// </summary>
        /// <remarks>
        /// Exemplo da atualização de um board
        /// {
        /// "Description" : "Boards para ajudar os jogadores",
        /// "rules" : "1. Proibido falar de um jogo diferente."
        /// }
        /// </remarks>
        /// <param name="model">Informações do board</param>
        /// <returns>Objeto Criado</returns>
        /// <response code="204">Board Atualizado</response>
        /// <response code="404">Board Inexistente</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, UpdateBoardInputModel model)
        {
            var board = repository.GetById(id);

            if (board == null)
                return NotFound();

            board.Update(model.Description, model.Rules);
            repository.Update(board);

            return NoContent();
        }

        /// <summary>
        /// Remover um board através do id
        /// </summary>
        /// <remarks>
        /// Exemplo da requisição de um board
        /// {
        /// "id" : "1",
        /// }
        /// </remarks>
        /// <param name="model">Id do board</param>
        /// <returns>Board removido pelo id</returns>
        /// <response code="204">Board Removido</response>
        /// <response code="404">Board Inexistente</response>

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var board = repository.GetById(id);
            if (board == null)
                NotFound();

            repository.Delete(board);
            return NoContent();
        }
    }
}
