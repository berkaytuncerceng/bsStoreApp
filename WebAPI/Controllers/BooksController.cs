using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController: ControllerBase
	{
		private readonly IRepositoryManager _manager;

		public BooksController(IRepositoryManager manager)
		{
			_manager = manager;
		}

		[HttpGet]
		public IActionResult GetAllBooks()
		{
			var books = _manager.Book.GetAllBooks(false);
			return Ok(books);
		}

		[HttpGet("{id:int}")]
		public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
		{
			try
			{
				var book = _manager.Book.GetOneBookById(id, false);
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
				_manager.Book.CreateOneBook(book);
				_manager.Save();
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
				var book = _manager.Book.GetOneBookById(id, false);

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

				_manager.Save();
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
				var book = _manager.Book.GetOneBookById(id , true);

				if (book is null)
				{
					return NotFound();
				}

				_manager.Book.DeleteOneBook(book);
				_manager.Save();
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
				var book = _manager.Book.GetOneBookById(id,true);

				if (book is null)
				{
					return NotFound();
				}

				patchDoc.ApplyTo(book, ModelState);

				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				_manager.Save();
				return Ok(book);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
