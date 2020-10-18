using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using NHibernateDemo.Model;
using Serilog;

namespace NHibernateDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private ISession _nhibernateSession;

        public BooksController(ILogger<BooksController> logger, ISession session)
        {
            _logger = logger;
            _nhibernateSession = session;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            // serilog 
            Log.Logger.Information("Getting books serilog logging");
            // .net core logging
            _logger.LogInformation("Getting books .net core logging");
            var books = await _nhibernateSession.Query<Book>().ToListAsync();
            return books;
        }

        [HttpPost]
        public async Task Post(Book b)
        {
            await _nhibernateSession.SaveAsync(b);
            await _nhibernateSession.FlushAsync();
        }
    }
}
