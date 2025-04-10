using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Services
{
    public class IssueLogger
    {
        private readonly ApplicationDbContext _context;

        public IssueLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Log(string title, string description)
        {
            var issue = new Issue
            {
                Title = title,
                Description = description
            };

            _context.Issues.Add(issue);
            _context.SaveChanges();
        }
    }
}
