using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence;
using DevGames.API.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevGames.API.Controllers
{
    [Route("api/boards/{id}/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository repository;

        public PostsController(IPostRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Recuperar todos os posts
        /// </summary>
        /// <remarks>
        /// Exemplo da requisição de um post
        /// {
        /// "Id" : "1",
        /// }
        /// </remarks>
        /// <returns>Todos os posts encontrados</returns>
        /// <response code="200">Posts encontrados</response>
        /// <response code="404">Posts Inexistentes</response>
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            var post = repository.GetAllByBoard(id);

            return Ok(post);
        }

        /// <summary>
        /// Recuperar um post pelo id 
        /// </summary>
        /// <remarks>
        /// Exemplo da requisição de um board e um post
        /// {
        /// "Id" : "1",
        /// "PostId" : "1",
        /// }
        /// </remarks>
        /// <param name="model">Id do board e do post</param>
        /// <returns>Post encontrado pelo id</returns>
        /// <response code="200">Post encontrado</response>
        /// <response code="404">Post Inexistente</response>
        [HttpGet("{postId})")]
        public IActionResult GetById(int id, int postId)
        {
            var post = repository.GetById(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        /// <summary>
        /// Publicar um novo Post
        /// </summary>
        /// <remarks>
        /// Exemplo da criação de um post
        /// {
        /// "Id" : "1",
        /// "Title" : "Preciso de ajuda",
        /// "Description" : "não consigo achar uma pantera para caçar",
        /// "User" : "FeijãoGamer05"
        /// }
        /// </remarks>
        /// <param name="model">Informações do post</param>
        /// <returns>Objeto Criado</returns>
        /// <response code="201">Post Criado</response>
        /// <response code="400">Informações Inválidas</response>
        /// <resposnse code="404">Board não encontrado</resposnse>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post(int id, AddPostInputModel model)
        {
            var post = new Post(model.Title, model.Description, id);
            repository.Add(post);
            return CreatedAtAction(nameof(GetById), new { id = post.Id, postId = post.Id}, model);
        }

        /// <summary>
        /// Publicar um novo comentario de um post
        /// </summary>
        /// <remarks>
        /// Exemplo da criação de um comentario
        /// {
        /// "Id" : "1",
        /// "Title" : "Eu sei como te ajudar",
        /// "Description" : "A panteras apareçem na região de Saint Denis",
        /// "User" : "Ronaldinho"
        /// }
        /// </remarks>
        /// <param name="model">Informações do comentario</param>
        /// <returns>Objeto Criado</returns>
        /// <response code="204">Comentário Criado</response>
        /// <response code="404">Post não encontrado</response>
        
        [HttpPost("{postId}/comments")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PostComment(int id, int postId, AddCommentInputModel model)
        {
            var postExists = repository.PostExists(postId);

            if (!postExists)
                return NotFound();

            var comment = new Comment(model.Title, model.Description, model.User, postId);
            repository.AddComment(comment);

            return NoContent();
        }
    }
}
