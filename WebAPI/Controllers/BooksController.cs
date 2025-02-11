using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController(RepositoryContext repositoryContext) : ControllerBase
	{
		private readonly RepositoryContext _repositoryContext = repositoryContext;

		[HttpGet]
		public IActionResult GetAllBooks()
		{
			var books = _repositoryContext.Books;
			return Ok(books);
		}

		[HttpGet("{id:int}")]
		public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
		{
			try
			{
				var book = _repositoryContext
						.Books
						.Where(i => i.Id == id)
						.SingleOrDefault();
				return Ok(book);

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		[HttpPost]
		public IActionResult AddBook([FromBody] Book book)
		{
			try
			{
				if (book == null)
				{
					return BadRequest();
				}
				_repositoryContext.Books.Add(book);
				_repositoryContext.SaveChanges();
				//return 201;
				return StatusCode(201, book);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		[HttpPut("{id:int}")]
		public IActionResult UpdateBook([FromRoute(Name = "id")] int id , [FromBody] Book bookToUpdate)
		{
			try
			{
				var book = _repositoryContext.Books.Where(i => i.Id == id).SingleOrDefault();

				if (book is null)
				{
					return NoContent();
				}

				if (id != bookToUpdate.Id)
				{
					return BadRequest();
				}

				book.Title = bookToUpdate.Title;
				book.Price = bookToUpdate.Price;

				_repositoryContext.SaveChanges();
				return Ok(book);
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		[HttpDelete("{id:int}")]
		public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
		{
			try
			{
				var book = _repositoryContext.Books.Where(i => i.Id == id).SingleOrDefault();

				if (book is null)
				{
					return NotFound();
				}

				_repositoryContext.Books.Remove(book);
				_repositoryContext.SaveChanges();
				return NoContent();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		[HttpPatch("{id:int}")]
		public IActionResult PartiallyUpdateBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> patchDoc)
		{
			try
			{
				var book = _repositoryContext.Books.Where(i => i.Id == id).SingleOrDefault();

				if (book is null)
				{
					return NotFound();
				}

				patchDoc.ApplyTo(book, ModelState);

				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				_repositoryContext.SaveChanges();
				return Ok(book);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
