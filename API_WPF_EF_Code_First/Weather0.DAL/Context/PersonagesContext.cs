using ApiIntro0.DAL.Context;
using System;
using System.Data.Entity;
using System.Linq;

namespace Weather0.DAL.Context
{
    public class PersonagesContext : DbContext
    {
        public PersonagesContext() : base("name=PersonagesContext") { }
        public virtual DbSet<PersonageCls> WeatherData { get; set; }
        public virtual DbSet<FilmCls> Films { get; set; }
        public virtual DbSet<SpeciesCls> Species { get; set; }
        public virtual DbSet<StarshipCls> Starships { get; set; }
        public virtual DbSet<VehicleCls> Vehicles { get; set; }
    }
}