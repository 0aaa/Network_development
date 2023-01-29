using ApiIntro0.BLL.DTO;
using ApiIntro0.DAL.Context;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Weather0.DAL.Repositories;

namespace Weather0.BLL.Services
{
    public class WeatherService
    {
        public GenericRepository<PersonageCls> PersonagesRpstry { get; set; }
        public GenericRepository<FilmCls> FilmsRpstry { get; set; }
        public GenericRepository<VehicleCls> VehiclesRpstry { get; set; }
        public GenericRepository<StarshipCls> StarshipsRpstry { get; set; }
        public GenericRepository<SpeciesCls> SpeciesRpstry { get; set; }
        private string RequestStr { get; set; }
        private WebClient WbClnt { get; set; }
        private JObject[] ResultJsn { get; set; }
        private PersonageCls[] PersonagesInArr { get; set; }
        private PersonageDTO[] PersonagesOutArr { get; set; }
        public WeatherService()
        {
            PersonagesRpstry = new GenericRepository<PersonageCls>();
            SpeciesRpstry = new GenericRepository<SpeciesCls>();
            StarshipsRpstry = new GenericRepository<StarshipCls>();
            VehiclesRpstry = new GenericRepository<VehicleCls>();
            FilmsRpstry = new GenericRepository<FilmCls>();

            ResultJsn = new JObject[5];
            WbClnt = new WebClient();
        }
        public PersonageDTO[] GetPersonages(int demandedQuantity)
        {
            if (PersonagesRpstry.GetAll().Count() < demandedQuantity)
            {
                for (int i = 1; i <= demandedQuantity; i++)
                {
                    if (!PersonagesRpstry.GetAll().Any(prsnge => prsnge.Id == i))
                    {
                        RequestStr = "https://swapi.dev/api/people/" + i;
                        try
                        {
                            ResultJsn[0] = JObject.Parse(WbClnt.DownloadString(RequestStr));
                        }
                        catch (Exception excptn)
                        {
                            MessageBox.Show(excptn.Message, "Caramba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return null;
                        }
                        AddDBEntities("films", 1, "title");
                        AddDBPersonage();
                        //AddDBEntities("species", 2, SpeciesRpstry);
                        //AddDBEntities("starships", 3, StarshipsRpstry);
                        //AddDBEntities("vehicles", 4, VehiclesRpstry); 
                    }
                }
            }
            return GetDBPersonage(demandedQuantity);
        }
        private void AddDBPersonage()
        {
            string[] idUrlArr = $"{ResultJsn[0]["url"]}".Split('/');
            PersonagesRpstry.AddUpdate(new PersonageCls()
            {
                Id = Convert.ToInt32(idUrlArr[idUrlArr.Length - 2]),
                Name = $"{ResultJsn[0]["name"]}",
                BirthYear = $"{ResultJsn[0]["birth_year"]}",
                EyeColor = $"{ResultJsn[0]["eye_color"]}",
                Gender = $"{ResultJsn[0]["gender"]}",
                HairColor = $"{ResultJsn[0]["hair_color"]}",
                Height = $"{ResultJsn[0]["height"]}",
                Mass = $"{ResultJsn[0]["mass"]}",
                SkinColor = $"{ResultJsn[0]["skin_color"]}",
                Homeworld = $"{JObject.Parse(WbClnt.DownloadString($"{ResultJsn[0]["homeworld"]}"))["name"]}",
            });
            PersonagesRpstry.Save();

            idUrlArr = $"{ResultJsn[0]["films"][0]}".Split('/');

            PersonageCls testPers = PersonagesRpstry.GetAll().Last();

            testPers.Films=(FilmsRpstry.GetAll().Where(flm => flm.Id == Convert.ToInt32(idUrlArr[idUrlArr.Length - 2])).ToHashSet());

            PersonagesRpstry.AddUpdate(testPers);
            PersonagesRpstry.Save();
            //for (int i = 0; i < ResultJsn[0]["films"].Count(); i++)
            //{
            //    PersonagesRpstry.GetAll().LastOrDefault()?.Films.Add(FilmsRpstry.GetAll().FirstOrDefault(flm => flm.Id == Convert.ToInt32(idUrlArr[idUrlArr.Length - 2])));
            //}
        }
        //HashSet<int> addF()
        //{
        //    HashSet<int> test = new HashSet<int>();
        //    string[] idUrlArr;
        //    for (int i = 0; i < ResultJsn[0]["films"].Count(); i++)
        //    {
        //        ResultJsn[1] = JObject.Parse(WbClnt.DownloadString($"{ResultJsn[0]["films"][i]}"));
        //        idUrlArr = $"{ResultJsn[1]["url"]}".Split('/');
        //        test.Add(Convert.ToInt32(idUrlArr[idUrlArr.Length - 2]));
        //    }
        //    return test;
        //}
        private void AddDBEntities(string entitiesSourceArray, int destinationIndex, string property = "name")
        {
            string[] idUrlArr;
            for (int i = 0; i < ResultJsn[0][entitiesSourceArray].Count(); i++)
            {
                ResultJsn[destinationIndex] = JObject.Parse(WbClnt.DownloadString($"{ResultJsn[0][entitiesSourceArray][i]}"));
                switch (destinationIndex)
                {
                    case 1:
                        //PersonagesRpstry.GetAll().Last().Films.Add(filmToAdd);
                        if (!FilmsRpstry.GetAll().Any(entty => entty.Name == $"{ResultJsn[destinationIndex][property]}"))
                        {
                            idUrlArr = $"{ResultJsn[1]["url"]}".Split('/');
                            FilmCls filmToAdd = new FilmCls()
                            {
                                Id = Convert.ToInt32(idUrlArr[idUrlArr.Length - 2]),
                                Name = $"{ResultJsn[destinationIndex][property]}"
                            };
                            FilmsRpstry.AddUpdate(filmToAdd);
                            FilmsRpstry.Save();
                        }
                        break;
                    case 2:
                        if (!SpeciesRpstry.GetAll().Any(entty => entty.Name == $"{ResultJsn[destinationIndex][property]}"))
                        {
                            SpeciesCls speciesToAdd = new SpeciesCls() { Name = $"{ResultJsn[destinationIndex][property]}" };
                            SpeciesRpstry.AddUpdate(speciesToAdd);
                            PersonagesRpstry.GetAll().Last().Species.Add(speciesToAdd);
                            SpeciesRpstry.Save();
                        }
                        break;
                    case 3:
                        if (!StarshipsRpstry.GetAll().Any(entty => entty.Name == $"{ResultJsn[destinationIndex][property]}"))
                        {
                            StarshipCls starshipToAdd = new StarshipCls() { Name = $"{ResultJsn[destinationIndex][property]}" };
                            StarshipsRpstry.AddUpdate(starshipToAdd);
                            PersonagesRpstry.GetAll().Last().Starships.Add(starshipToAdd);
                            StarshipsRpstry.Save();
                        }
                        break;
                    case 4:
                        if (!VehiclesRpstry.GetAll().Any(entty => entty.Name == $"{ResultJsn[destinationIndex][property]}"))
                        {
                            VehicleCls vehicleToAdd = new VehicleCls() { Name = $"{ResultJsn[destinationIndex][property]}" };
                            VehiclesRpstry.AddUpdate(vehicleToAdd);
                            PersonagesRpstry.GetAll().Last().Vehicles.Add(vehicleToAdd);
                            VehiclesRpstry.Save();
                        }
                        break;
                }
            }
            PersonagesRpstry.Save();
        }

        public PersonageDTO[] GetDBPersonage(int quantity)
        {
            PersonagesOutArr = new PersonageDTO[quantity];
            PersonagesInArr = PersonagesRpstry.GetAll().Take(quantity).ToArray();
            for (int i = 0; i < quantity; i++)
            {
                PersonagesOutArr[i] = new PersonageDTO()
                {
                    Name = PersonagesInArr[i].Name,
                    BirthYear = PersonagesInArr[i].BirthYear,
                    EyeColor = PersonagesInArr[i].EyeColor,
                    Gender = PersonagesInArr[i].Gender,
                    HairColor = PersonagesInArr[i].HairColor,
                    Height = PersonagesInArr[i].Height,
                    Mass = PersonagesInArr[i].Mass,
                    SkinColor = PersonagesInArr[i].SkinColor,
                    Homeworld = PersonagesInArr[i].Homeworld
                };
                for (int j = 0; j < 4; j++)
                {
                    //PersonagesOutArr[i].FSSV[j] = GetDBEntities(PersonagesInArr[i], j);
                }
            }
            return PersonagesOutArr;
        }
        private List<string> GetDBEntities(PersonageCls currentPersonage, int mode)
        {
            ParentCls[] entitiesTempArr = null;
            List<string> entityNamesLst = new List<string>();
            int listSize = 0;
            switch (mode)
            {
                case 0:
                    listSize = currentPersonage.FilmId.Count;
                    //entitiesTempArr = currentPersonage.Films.ToArray();
                    break;
                case 1:
                    listSize = currentPersonage.Species.Count;
                    entitiesTempArr = currentPersonage.Species.ToArray();
                    break;
                case 2:
                    listSize = currentPersonage.Starships.Count;
                    entitiesTempArr = currentPersonage.Starships.ToArray();
                    break;
                case 3:
                    listSize = currentPersonage.Vehicles.Count;
                    entitiesTempArr = currentPersonage.Vehicles.ToArray();
                    break;
            }
            for (int i = 0; i < listSize; i++)
            {
                entityNamesLst.Add(entitiesTempArr[i].Name);
            }
            return entityNamesLst;
        }
    }
}
