using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMovieApi.Data;
using WebMovieApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebMovieApi.Controllers
{
    

    [Route("api/[controller]")]
    public class MovieController : Controller
    {

        private readonly MovieDbContext _movieDbContext;

        public MovieController(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
            if (_movieDbContext.Movies.Count() == 0)
            {
                _movieDbContext.Movies.Add(new Models.Movies { Title = "Jason" });
                _movieDbContext.SaveChanges();
            }
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Movies> Get(String releaseDate)
        {
            List<Movies> list = new List<Movies>();
            list = _movieDbContext.Movies.Where(t=> t.ReleaseDate>= Convert.ToDateTime(releaseDate)).ToList();
            if (list.Count==0)
            {
                return Enumerable.Empty<Movies>();
            }
            return list.AsEnumerable();
            
        }

        [HttpPost("List")]
        public IEnumerable<Movies> Get([FromBody] Movies movie)
        {
            List<Movies> list = new List<Movies>();
            list = _movieDbContext.Movies.Where(t => t.ReleaseDate >= movie.ReleaseDate && t.Title.Contains(movie.Title)).ToList();
            if (list.Count == 0)
            {
                return Enumerable.Empty<Movies>();
            }
            return list.AsEnumerable();

        }

        // GET api/<controller>/5
        //[HttpGet("{id}"), Name= "GetTodo")]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            Console.Write("asdf=" + _movieDbContext);
            var movie = _movieDbContext.Movies.Where(t => t.ID >= id);
            if (movie == null)
            {
                return NotFound();
            }
            return new ObjectResult(movie);
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] Movies movies)
        {
            if (movies== null)
            {
                return BadRequest();
            }
            _movieDbContext.Movies.Add(movies);
            _movieDbContext.SaveChanges();
            return CreatedAtRoute("Get", new { id = movies.ID }, movies);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _movieDbContext.Movies.FirstOrDefault(t => t.ID == id);
            if (movie == null)
                return NotFound();
            else
            {
                _movieDbContext.Movies.Remove(movie);
                _movieDbContext.SaveChanges();
                return new NoContentResult();
            }
        }
    }
}
