using myshop.DataAccess.Implementation;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Services
{
    public class IssueRepository : GenericRepository<Issue>, IIssueLogRepository
    {
        private readonly ApplicationDbContext _context;
        public IssueRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Issue issue)
        {
            var IssueInDb = _context.Issues.FirstOrDefault(x => x.Id == issue.Id);
            if (IssueInDb != null)
            {
                IssueInDb.Title = issue.Title;
                IssueInDb.Description = issue.Description;
                IssueInDb.CreatedAt = DateTime.Now;
            }
        }
    }
}
