using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            //var partsstringxml = File.ReadAllText("../../../datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, partsstringxml));

            //var suppliersStringXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSupplier(context, suppliersStringXml));

            var partsStringXml = File.ReadAllText("../../../Datasets/cars.xml");
            Console.WriteLine(ImportCars(context, partsStringXml));

        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(PartsImportModel[]),
                new XmlRootAttribute("Parts"));

            PartsImportModel[] importDtos;
            using (var stringReader =  new StringReader(inputXml))
            {
                importDtos = (PartsImportModel[])xmlSerializer.Deserialize(stringReader);
            };

            var supplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToArray();

            Part[] parts = importDtos
                .Where(p => supplierIds.Contains(p.SupplierId))
                .Select(dto => new Part()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId,
                })
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }

        public static string ImportSupplier(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(SuppliersImportModel[]),
                new XmlRootAttribute("Suppliers"));

            SuppliersImportModel[] importDtos;
            using (var stringReader = new StringReader(inputXml))
            {
                importDtos = (SuppliersImportModel[])xmlSerializer.Deserialize(stringReader);
            };

            Supplier[] suppliers = importDtos
                .Select(dto => new Supplier()
                {
                    Name = dto.Name,
                    IsImporter = dto.IsImporter
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CarsImportModel[]),
                new XmlRootAttribute("Cars"));

            CarsImportModel[] importDtos;
            using (var stringReader = new StringReader(inputXml))
            {
                importDtos = (CarsImportModel[])xmlSerializer.Deserialize(stringReader);
            };

            var existingParts = context.Parts
                .Select(p => p.Id)
                .ToArray();

            List<Car> cars = new List<Car>();

            foreach (var model in importDtos)
            {
                Car car = new Car()
                {
                    Make = model.Make,
                    Model = model.Model,
                    TraveledDistance = model.TraveledDistance,
                };

                int[] carPartsids = model.PartIds
                    .Select(p => p.Id)
                    .Distinct()
                    .Where(p => existingParts.Contains(p))
                    .ToArray();

                var carParts = new List<PartCar>();

                foreach(var partid in carPartsids)
                {
                    carParts.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partid
                    });
                }

                car.PartsCars = carParts;

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
    }
}