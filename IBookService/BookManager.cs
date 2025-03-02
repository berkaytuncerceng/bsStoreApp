using AutoMapper;
using Entities.DTOs;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class BookManager : IBookService
	{
		private readonly IRepositoryManager _manager;
		private readonly ILoggerService _logger;
		private readonly IMapper _mapper;

		public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
		{
			_manager = manager;
			_logger = logger;
			_mapper = mapper;
		}

		public void CreateOneBook(Book book)
		{
			if (book is null)
			{
				var logMessage = "Eklenecek kitap boş";
				_logger.LogInfo(logMessage);
				throw new ArgumentNullException(nameof(book));
			}
			_manager.Book.CreateOneBook(book);
			_manager.Save();
		}

		public void DeleteOneBook(int id, bool trackChanges)
		{
			var entity = _manager.Book.GetOneBookById(id, trackChanges);
			if (entity is null)

			{
				//var logMessage = "Bu id'de kitap bulunamadı";
				//_logger.LogInfo(logMessage);
				throw new BookNotFoundException(id);
			}
			_manager.Book.DeleteOneBook(entity);
			_manager.Save();
		}

		public IEnumerable<Book> GetAllBooks(bool trackChanges)
		{
			var bookList = _manager.Book.GetAllBooks(trackChanges).ToList();
			if (bookList.Count == 0)
			{
				var logMessage = "Kitaplar bulunamadı";
				_logger.LogDebug(logMessage);
				throw new Exception("Sistemde hiç kitap yok.");
			}
			return bookList;
		}

		public Book GetOneBookById(int bookId, bool trackChanges)
		{
			var book = _manager.Book.GetOneBookById(bookId, trackChanges);
			if (book is null)
			{
				throw new BookNotFoundException(bookId);
			}
			return book;
		}

		public void UpdateOneBook(int id, 
			BookDtoToUpdate bookDto, 
			bool trackChanges)
		{
			var entity = _manager.Book.GetOneBookById(id, trackChanges);
			if (entity is null)
			{
				throw new BookNotFoundException(id);
			}
			if (bookDto is null)
			{
				throw new BookNotFoundException(id);
			}
			//entity.Title = book.Title;
			//entity.Price = book.Price;
			entity = _mapper.Map<Book>(bookDto); // üstteki ifadeleri tek tek yazmamıza gerek kalmadı

			_manager.Book.UpdateOneBook(entity);
			_manager.Save();
		}
	}
}
