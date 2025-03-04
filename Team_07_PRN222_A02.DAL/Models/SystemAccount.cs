using System;
using System.Collections.Generic;

namespace Team_07_PRN222_A02.DAL.Models;

public partial class SystemAccount
{
    public int AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public string AccountEmail { get; set; } = null!;

    public byte AccountRole { get; set; }

    public string AccountPassword { get; set; } = null!;

    public virtual ICollection<NewsArticle> NewsArticleCreatedBies { get; set; } = new List<NewsArticle>();

    public virtual ICollection<NewsArticle> NewsArticleUpdatedBies { get; set; } = new List<NewsArticle>();
}
