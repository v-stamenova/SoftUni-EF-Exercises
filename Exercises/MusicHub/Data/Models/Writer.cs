﻿using System.Collections.Generic;

namespace MusicHub.Data.Models
{
	public class Writer
	{
		public Writer()
		{
			Songs = new List<Song>();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public string Pseudonym { get; set; }
		
		public ICollection<Song> Songs { get; set; }
	}
}
