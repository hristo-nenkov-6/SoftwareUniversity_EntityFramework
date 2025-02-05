namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 300));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumInfo = context.Producers?
                .FirstOrDefault(p => p.Id == producerId)
                .Albums?
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString(),
                    ProducerName = a.Producer?.Name,
                    Songs = a.Songs?
                        .Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price,
                            Writer = s.Writer?.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.Writer),
                    AlbumPrice = a.Songs?
                        .Select(s => s.Price)
                        .Sum()
                })
                .OrderByDescending(a => a.AlbumPrice);

            var sb = new StringBuilder();

            foreach(var album in albumInfo)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs: ");

                int br = 1;
                foreach(var song in album.Songs)
                {
                    sb.AppendLine($"--{br}");
                    sb.AppendLine($"--SongName: {song.SongName}");
                    sb.AppendLine($"--Price: {song.Price:f2}");
                    sb.AppendLine($"--Writer: {song.Writer}");
                    br++;
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return sb.ToString();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsInfo = context.Songs
                .ToList()
                .Where(s => s.Duration.TotalSeconds > (double)duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    WriterName = s.Writer.Name,
                    PerformersNames = 
                        s.SongPerforemrs
                        .Select(s => s.Performer.FirstName
                                       + " "
                                       + s.Performer.LastName)
                        .ToList(),
                    AlbumProducerName = s.Album.Producer.Name,
                    DurationString = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToList();

            var sb = new StringBuilder();

            int br = 1;
            foreach(var song in songsInfo)
            {
                sb.AppendLine($"-Song #${br}");
                sb.AppendLine($"--SongName: {song.SongName}");
                sb.AppendLine($"--Writer: {song.WriterName}");
                if (song.PerformersNames.Any())
                {
                    sb.AppendLine($"--Performer: {String.Join(", ", song.PerformersNames)}");
                }
                sb.AppendLine($"--AlbumProducer: {song.AlbumProducerName}");
                sb.AppendLine($"--Duration: {song.DurationString}");

                br++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
