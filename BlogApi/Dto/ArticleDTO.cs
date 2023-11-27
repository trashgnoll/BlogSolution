﻿using Blog.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogApi.Dto;

public class ArticleDto
{
    public string Title { get; set; }
    public String Content { get; set; }
    public ICollection<string>? Tags { get; set; }

}
