using Ecommerce.Models.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

public class SchedulePdfGenerator
{
    public static byte[] Generate(List<Schedule> schedules)
    {
        var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        var timeSlots = Enumerable.Range(0, 7)
            .Select(i => TimeSpan.FromHours(8 + i * 2))
            .ToList();

        var generationDate = DateTime.Now;
        var startOfWeek = generationDate.AddDays(-(int)generationDate.DayOfWeek + (generationDate.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
        var endOfWeek = startOfWeek.AddDays(6);
        var schoolName = "BitBot";
        var className = schedules.Any() ? schedules.First().Classe?.Name ?? "N/A" : "N/A";
        var culture = new CultureInfo("en-US");
        var title = $"{schoolName} Schedule: {startOfWeek.ToString("MMMM dd", culture)} - {endOfWeek.ToString("MMMM dd, yyyy", culture)} ({className})";

        byte[] logoBytes = null;
        try
        {
            using (var httpClient = new HttpClient())
            {
                logoBytes = httpClient.GetByteArrayAsync("https://i.imgur.com/luvAL8B.png").Result;
            }
        }
        catch { /* Handle exception */ }

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(5);
                page.Size(PageSizes.A4.Landscape());

                // Header Section
                page.Header().Column(column =>
                {
                    if (logoBytes != null)
                    {
                        column.Item().AlignCenter().Width(100).Image(logoBytes);
                        column.Item().PaddingTop(10);
                    }
                    column.Item().Text(title)
                        .FontSize(8)
                        .Bold()
                        .AlignCenter();
                });

                // Table Content
                page.Content().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(0.5f);
                        foreach (var _ in daysOfWeek)
                            columns.RelativeColumn(2);
                    });

                    // Table Header
                    foreach (var header in new[] { "Time" }.Concat(daysOfWeek))
                    {
                        table.Cell()
                            .Background("#f0f0f0")
                            .MinHeight(30)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(header)
                            .Bold()
                            .FontSize(6);
                    }

                    var remainingRowSpans = new Dictionary<int, int>();
                    foreach (var time in timeSlots)
                    {
                        var endTime = time.Add(TimeSpan.FromHours(2));

                        // Time Column Cell
                        table.Cell()
                            .Padding(0)
                            .Border(1)
                            .BorderColor("#ddd")
                            .MinHeight(50)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text($"{time:hh\\:mm} - {endTime:hh\\:mm}")
                            .FontSize(6);

                        foreach (var day in daysOfWeek)
                        {
                            var dayIndex = Array.IndexOf(daysOfWeek, day);

                            if (remainingRowSpans.TryGetValue(dayIndex, out var remaining))
                            {
                                if (remaining > 0)
                                {
                                    remainingRowSpans[dayIndex] = remaining - 1;
                                    continue;
                                }
                                remainingRowSpans.Remove(dayIndex);
                            }

                            // Key Change: Map array index (0-5) to day numbers (1-6)
                            var cellSchedules = schedules
                                .Where(s => s.Day == dayIndex + 1 &&  // Monday=1, Tuesday=2, etc.
                                          s.StartTime >= time &&
                                          s.StartTime < endTime)
                                .ToList();

                            if (cellSchedules.Any())
                            {
                                var lesson = cellSchedules.First();
                                var lessonEnd = lesson.EndTime;
                                var startSlotIndex = timeSlots.FindIndex(t => t <= lesson.StartTime && lesson.StartTime < t.Add(TimeSpan.FromHours(2)));
                                var endSlotIndex = timeSlots.FindIndex(t => t <= lessonEnd && lessonEnd <= t.Add(TimeSpan.FromHours(2)));
                                var rowSpan = Math.Max(1, endSlotIndex - startSlotIndex + 1);

                                var content = $"{lesson.Course?.CourseName ?? "N/A"}\n" +
                                              $"{lesson.Course?.User?.FirstName} {lesson.Course?.User?.LastName}\n" +
                                              $"{lesson.Location?.Name ?? "N/A"}";

                                table.Cell()
                                    .RowSpan((uint)rowSpan)
                                    .Padding(0)
                                    .Border(1)
                                    .BorderColor("#ddd")
                                    .Background("#E6F0FA")
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Column(c => c.Item().Text(content).LineHeight(1.2f).FontSize(12));

                                remainingRowSpans[dayIndex] = rowSpan - 1;
                            }
                            else
                            {
                                table.Cell()
                                    .Padding(0)
                                    .Border(1)
                                    .BorderColor("#ddd")
                                    .MinHeight(50)
                                    .Background(Colors.White)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text("");
                            }
                        }
                    }
                });

                // Footer Section
                page.Footer()
                    .PaddingTop(10)
                    .AlignCenter()
                    .Text($"Generated on {DateTime.Now:dddd, MMMM dd, yyyy}")
                    .FontSize(6);
            });
        }).GeneratePdf();
    }
}