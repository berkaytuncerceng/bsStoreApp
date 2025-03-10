﻿using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
	public class BookRepository : RepositoryBase<Book> , IBookRepository
	{
		public BookRepository(RepositoryContext repositoryContext) : base(repositoryContext)
		{
		}

		public void CreateOneBook(Book book) => Create(book);
		public void DeleteOneBook(Book book) => Delete(book);

		public IQueryable<Book> GetAllBooks(bool trackChanges) => FindAll(trackChanges);

		public Book GetOneBookById(int bookId, bool trackChanges) => 
			FindByCondition(b => b.Id.Equals(bookId), trackChanges).SingleOrDefault();

		public void UpdateOneBook(Book book) => Update(book);
	}
}
