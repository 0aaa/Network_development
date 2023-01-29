using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntro0.DAL.Context
{
    public class PersonageCls
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string BirthYear { get; set; }
        public string EyeColor { get; set; }
        public string Gender { get; set; }
        public string HairColor { get; set; }
        public string Height { get; set; }
        public string Mass { get; set; }
        public string SkinColor { get; set; }
        public string Homeworld { get; set; }
        public virtual HashSet<int> FilmId { get; set; }
        public virtual HashSet<FilmCls> Films { get; set; }
        public virtual HashSet<SpeciesCls> Species { get; set; }
        public virtual HashSet<StarshipCls> Starships { get; set; }
        public virtual HashSet<VehicleCls> Vehicles { get; set; }
        public PersonageCls()
        {
            FilmId = new HashSet<int>();
            Films = new HashSet<FilmCls>();
            Species = new HashSet<SpeciesCls>();
            Starships = new HashSet<StarshipCls>();
            Vehicles = new HashSet<VehicleCls>();
        }
    }
}
