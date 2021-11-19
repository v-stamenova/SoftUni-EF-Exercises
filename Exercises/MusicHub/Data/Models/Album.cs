﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicHub.Data.Models
{
	public class Album
	{
		public Album()
		{
			Songs = new List<Song>();
			Price = Songs.Sum(x => x.Price);
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime ReleaseDate { get; set; }

		public decimal Price { get; set; }

		public int? ProducerId { get; set; }
		public Producer Producer { get; set; }

		public ICollection<Song> Songs { get; set; }
	}
}
