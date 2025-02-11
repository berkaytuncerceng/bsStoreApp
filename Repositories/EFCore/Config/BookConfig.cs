using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Repositories.EFCore.Config
{
	public class BookConfig : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.HasData(
				new Book
				{
					Id = 1,
					Title = "Devlet",
					Price = 100.2m
				},
				new Book
				{
					Id = 2,
					Title = "Suç ve Ceza",
					Price = 150
				},
				new Book
				{
					Id = 3,
					Title = "Yaprak Dökümü",
					Price = 200
				});
		}
	}
}
