using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            //string stringedJson = File.ReadAllText("D:\\Entity Framework\\08_09 - JSON and XML Processing\\ProductsShop\\CarDealer\\Datasets\\suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, stringedJson));

            //string stringedjson = File.ReadAllText("d:\\entity framework\\08_09 - json and xml processing\\productsshop\\cardealer\\datasets\\parts.json");
            //Console.WriteLine(ImportParts(context, stringedjson));

            string stringedjson = File.ReadAllText("d:\\entity framework\\08_09 - json and xml processing\\productsshop\\cardealer\\datasets\\cars.json");
            Console.WriteLine(ImportCars(context, stringedjson));

            //string stringedjson = File.ReadAllText("d:\\entity framework\\08_09 - json and xml processing\\productsshop\\cardealer\\datasets\\customers.json");
            //Console.WriteLine(ImportCustomers(context, stringedjson));

            //string stringedjson = File.ReadAllText("d:\\entity framework\\08_09 - json and xml processing\\productsshop\\cardealer\\datasets\\sales.json");
            //Console.WriteLine(ImportSales(context, stringedjson));

            //Console.WriteLine(GetOrderedCustomers(context));

            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //Console.WriteLine(GetLocalSuppliers(context));
            
            Console.WriteLine(GetCarsWithTheirListOfParts(context));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            List<Supplier> suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var validSupplierIds = context.Suppliers.Select(s => s.Id).ToList();

            var filteredParts = parts.Where(p => validSupplierIds.Contains(p.SupplierId)).ToList();

            context.Parts.AddRange(filteredParts);

            context.SaveChanges();

            return $"Successfully imported {filteredParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            List<CarPartsDTO> cars = JsonConvert.DeserializeObject<List<CarPartsDTO>>(inputJson);

            foreach (var car in cars)
            {
                Car currentCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TraveledDistance = car.TraveledDistance
                };

                foreach (var part in car.PartsIds)
                {
                    bool isValid = currentCar.PartsCars.FirstOrDefault(x => x.PartId == part) == null;
                    bool isPartValid = context.Parts.FirstOrDefault(p => p.Id == part) != null;

                    if (isValid && isPartValid)
                    {
                        currentCar.PartsCars.Add(new PartCar()
                        {
                            PartId = part
                        });
                    }
                }

                context.Cars.Add(currentCar);
            }

            context.SaveChanges();

            return $"Successfully imported {context.Cars.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var filteredCustomers = context
                .Customers
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver,
                })
                .Distinct()
                .ToList()
                .OrderBy(c => c.BirthDate)
                .ThenByDescending(c => !c.IsYoungDriver);

            var wantedObject = JsonConvert.SerializeObject(filteredCustomers, Formatting.Indented);

            return wantedObject.ToString();
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var filteredCars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                })
                .ToList()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToList();

            var wantedObject = JsonConvert.SerializeObject(filteredCars, Formatting.Indented);

            return wantedObject.ToString();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                });

            var wantedObject = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);

            return wantedObject.ToString();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    parts = c.PartsCars.Select(p => new
                    {
                        Name = p.Part.Name,
                        Price = decimal.Round(p.Part.Price, 2)
                    })
                })
                .ToList();

            var wantedObject = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return wantedObject.ToString();
        }
    }
}