﻿using Entities.Exceptions;
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
			var book = _manager.BookService.GetOneBookById(id, false);
			return Ok(book);
		}

		[HttpPost]
		public IActionResult AddBook([FromBody] Book book)
		{

			if (book == null)
			{
				return BadRequest();
			}
			_manager.BookService.CreateOneBook(book);
			//return 201;
			return StatusCode(201, book);
		}


		[HttpPut("{id:int}")]
		public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book bookToUpdate)
		{

			if (bookToUpdate is null)
			{
				return BadRequest();
			}
			_manager.BookService.UpdateOneBook(id, bookToUpdate, true);
			return NoContent();


		}

		[HttpDelete("{id:int}")]
		public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
		{

			_manager.BookService.DeleteOneBook(id, false);

			return NoContent();
		}



		[HttpPatch("{id:int}")]
		public IActionResult PartiallyUpdateBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> patchDoc)
		{

			var book = _manager.BookService.GetOneBookById(id, true);
	

			patchDoc.ApplyTo(book);
			_manager.BookService.UpdateOneBook(id, book, true);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return NoContent();

		}
	}
}



