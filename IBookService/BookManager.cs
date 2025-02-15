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

		public BookManager(IRepositoryManager manager)
		{
			_manager = manager;
		}

		public void CreateOneBook(Book book)
		{
			if (book is null)
			{
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
				throw new Exception(nameof(entity));
			}
			_manager.Book.DeleteOneBook(entity);
			_manager.Save();
		}

		public IEnumerable<Book> GetAllBooks(bool trackChanges)
		{
			return _manager.Book.GetAllBooks(trackChanges).ToList();
		}

		public Book GetOneBookById(int bookId, bool trackChanges)
		{
			return _manager.Book.GetOneBookById(bookId, trackChanges);
		}

		public void UpdateOneBook(int id, Book book, bool trackChanges)
		{
			var entity = _manager.Book.GetOneBookById(id, trackChanges);
			if (entity is null)
			{
				throw new Exception(nameof(entity));
			}
			if (book is null)
			{
				throw new ArgumentNullException(nameof(book));
			}
			entity.Title = book.Title;
			entity.Price = book.Price;

			_manager.Book.UpdateOneBook(entity);
			_manager.Save();
		}
	}
}
