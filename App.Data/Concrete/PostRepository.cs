﻿using App.Data.Abstract;
using App.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Concrete
{
	public class PostRepository : Repository<Post>, IPostRepository
	{
		public PostRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<List<Post>> GetAllPostsByIncludeAsync()
		{
			return await _context.Posts.GroupJoin()
		}


		public Task<User> GetPostByIncludeAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Post>> GetSomePostsByIncludeAsync(Expression<Func<Post, bool>> expression)
		{
			throw new NotImplementedException();
		}
	}
}
