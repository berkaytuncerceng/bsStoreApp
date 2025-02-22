using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BooksController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public BooksController(IServiceManager manager)
		{
			_manager = manager;
		}

		[HttpGet]
		public IActionResult GetAllBooks()
		{
			var books = _manager.BookService.GetAllBooks(false);
			return Ok(books);
		}

		[HttpGet("{id:int}")]
		public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
		{
			try
			{
				var book = _manager.BookService.GetOneBookById(id, false);
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
				_manager.BookService.CreateOneBook(book);
				//return 201;
				return StatusCode(201, book);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		[HttpPut("{id:int}")]
		public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book bookToUpdate)
		{
			try
			{
				if (bookToUpdate is null)
				{
					return BadRequest();
				}
				_manager.BookService.UpdateOneBook(id, bookToUpdate, true);
				return NoContent();
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
				_manager.BookService.DeleteOneBook(id, false);

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
				var book = _manager.BookService.GetOneBookById(id, true);
				if (book == null)
				{
					return NotFound();
				}

				patchDoc.ApplyTo(book );
				_manager.BookService.UpdateOneBook(id, book, true);
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				return NoContent();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}

}
