using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL.Initializer
{
    public static class Seeder
    {
        public static async Task SeedAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            var genres = new List<GenreEntity>();

            if (!context.Genres.Any())
            {
                genres.AddRange(
                    new GenreEntity { Name = "Фентезі" },
                    new GenreEntity { Name = "Фантастика" },
                    new GenreEntity { Name = "Детективи" },
                    new GenreEntity { Name = "Романтична проза" },
                    new GenreEntity { Name = "Трилери та жахи" },
                    new GenreEntity { Name = "Класична література" },
                    new GenreEntity { Name = "Комікси та манґи" }
                );

                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
            }

            if (!context.Authors.Any())
            {
                if(genres.Count == 0)
                {
                    genres = await context.Genres.ToListAsync();
                }

                var authors = new List<AuthorEntity>()
                {
                    // 1. Сергій Жадан
                    new AuthorEntity {
                        Name = "Сергій Жадан",
                        BirthDate = new DateTime(1974, 8, 23).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Інтернат", Pages = 336, PublishYear = 2017, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Ворошиловград", Pages = 440, PublishYear = 2010, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Месопотамія", Pages = 368, PublishYear = 2014, Genres = new List<GenreEntity> { genres[3], genres[5] } },
                            new BookEntity { Title = "Депеш Мод", Pages = 230, PublishYear = 2004, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Антена", Pages = 304, PublishYear = 2018, Genres = new List<GenreEntity> { genres[5] } }
                        }
                    },
                    // 2. Макс Кідрук
                    new AuthorEntity {
                        Name = "Макс Кідрук",
                        BirthDate = new DateTime(1984, 8, 11).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Колонія", Pages = 904, PublishYear = 2023, Genres = new List<GenreEntity> { genres[1] } },
                            new BookEntity { Title = "Де немає Бога", Pages = 480, PublishYear = 2018, Genres = new List<GenreEntity> { genres[4] } },
                            new BookEntity { Title = "Доки світло не згасне назавжди", Pages = 560, PublishYear = 2019, Genres = new List<GenreEntity> { genres[1], genres[4] } },
                            new BookEntity { Title = "Бот", Pages = 480, PublishYear = 2012, Genres = new List<GenreEntity> { genres[1], genres[4] } },
                            new BookEntity { Title = "Зазирни у мої сни", Pages = 528, PublishYear = 2016, Genres = new List<GenreEntity> { genres[1], genres[4] } }
                        }
                    },
                    // 3. Оксана Забужко
                    new AuthorEntity {
                        Name = "Оксана Забужко",
                        BirthDate = new DateTime(1960, 9, 19).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Польові дослідження з українського сексу", Pages = 160, PublishYear = 1996, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Музей покинутих секретів", Pages = 832, PublishYear = 2009, Genres = new List<GenreEntity> { genres[5], genres[3] } },
                            new BookEntity { Title = "Казка про калинову сопілку", Pages = 96, PublishYear = 2000, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Тут могла б бути ваша реклама", Pages = 320, PublishYear = 2014, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Найдовша подорож", Pages = 176, PublishYear = 2022, Genres = new List<GenreEntity> { genres[5] } }
                        }
                    },
                    // 4. Андрій Кокотюха
                    new AuthorEntity {
                        Name = "Андрій Кокотюха",
                        BirthDate = new DateTime(1970, 11, 17).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Адвокат із Личаківської", Pages = 288, PublishYear = 2015, Genres = new List<GenreEntity> { genres[2] } },
                            new BookEntity { Title = "Червоний", Pages = 320, PublishYear = 2012, Genres = new List<GenreEntity> { genres[2], genres[5] } },
                            new BookEntity { Title = "Темна вода", Pages = 280, PublishYear = 2018, Genres = new List<GenreEntity> { genres[2], genres[4] } },
                            new BookEntity { Title = "Вигнанець і грішниця", Pages = 304, PublishYear = 2020, Genres = new List<GenreEntity> { genres[2] } },
                            new BookEntity { Title = "Останній рік карнавалу", Pages = 320, PublishYear = 2021, Genres = new List<GenreEntity> { genres[2] } }
                        }
                    },
                    // 5. Люко Дашвар
                    new AuthorEntity {
                        Name = "Люко Дашвар",
                        BirthDate = new DateTime(1957, 10, 3).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Село не люди", Pages = 272, PublishYear = 2007, Genres = new List<GenreEntity> { genres[3], genres[5] } },
                            new BookEntity { Title = "Молоко з кров'ю", Pages = 288, PublishYear = 2008, Genres = new List<GenreEntity> { genres[3], genres[5] } },
                            new BookEntity { Title = "Рай.Центр", Pages = 304, PublishYear = 2009, Genres = new List<GenreEntity> { genres[3] } },
                            new BookEntity { Title = "Мати все", Pages = 336, PublishYear = 2010, Genres = new List<GenreEntity> { genres[3] } },
                            new BookEntity { Title = "На запах м'яса", Pages = 368, PublishYear = 2013, Genres = new List<GenreEntity> { genres[3] } }
                        }
                    },
                    // 6. Юрій Андрухович
                    new AuthorEntity {
                        Name = "Юрій Андрухович",
                        BirthDate = new DateTime(1960, 3, 13).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Рекреації", Pages = 160, PublishYear = 1992, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Московіада", Pages = 256, PublishYear = 1993, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Перверзія", Pages = 320, PublishYear = 1996, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Дванадцять обручів", Pages = 352, PublishYear = 2003, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Радіо Ніч", Pages = 448, PublishYear = 2020, Genres = new List<GenreEntity> { genres[5] } }
                        }
                    },
                    // 7. Володимир Лис
                    new AuthorEntity {
                        Name = "Володимир Лис",
                        BirthDate = new DateTime(1950, 10, 26).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Століття Якова", Pages = 240, PublishYear = 2010, Genres = new List<GenreEntity> { genres[5], genres[3] } },
                            new BookEntity { Title = "Соло для Соломії", Pages = 368, PublishYear = 2013, Genres = new List<GenreEntity> { genres[3], genres[5] } },
                            new BookEntity { Title = "Діва Млинза", Pages = 352, PublishYear = 2015, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Острів Сильвестра", Pages = 224, PublishYear = 2016, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Стара холера", Pages = 272, PublishYear = 2018, Genres = new List<GenreEntity> { genres[3], genres[5] } }
                        }
                    },
                    // 8. Василь Шкляр
                    new AuthorEntity {
                        Name = "Василь Шкляр",
                        BirthDate = new DateTime(1951, 6, 10).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Чорний ворон", Pages = 384, PublishYear = 2009, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Маруся", Pages = 320, PublishYear = 2014, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Троща", Pages = 416, PublishYear = 2017, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Характерник", Pages = 304, PublishYear = 2019, Genres = new List<GenreEntity> { genres[0], genres[5] } },
                            new BookEntity { Title = "Ключ", Pages = 256, PublishYear = 1999, Genres = new List<GenreEntity> { genres[2], genres[5] } }
                        }
                    },
                    // 9. Дара Корній
                    new AuthorEntity {
                        Name = "Дара Корній",
                        BirthDate = new DateTime(1970, 9, 20).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Гонихмарник", Pages = 336, PublishYear = 2010, Genres = new List<GenreEntity> { genres[0], genres[3] } },
                            new BookEntity { Title = "Зворотний бік світла", Pages = 320, PublishYear = 2012, Genres = new List<GenreEntity> { genres[0] } },
                            new BookEntity { Title = "Зворотний бік темряви", Pages = 352, PublishYear = 2013, Genres = new List<GenreEntity> { genres[0] } },
                            new BookEntity { Title = "Місяцівна", Pages = 352, PublishYear = 2019, Genres = new List<GenreEntity> { genres[0], genres[3] } },
                            new BookEntity { Title = "Сузір'я Дів", Pages = 320, PublishYear = 2018, Genres = new List<GenreEntity> { genres[3], genres[0] } }
                        }
                    },
                    // 10. Ілларіон Павлюк
                    new AuthorEntity {
                        Name = "Ілларіон Павлюк",
                        BirthDate = new DateTime(1980, 5, 14).ToUniversalTime(),
                        Books = new List<BookEntity> {
                            new BookEntity { Title = "Я бачу, вас цікавить пітьма", Pages = 664, PublishYear = 2020, Genres = new List<GenreEntity> { genres[2], genres[4] } },
                            new BookEntity { Title = "Білий попіл", Pages = 352, PublishYear = 2018, Genres = new List<GenreEntity> { genres[2], genres[4] } },
                            new BookEntity { Title = "Танець недоумка", Pages = 680, PublishYear = 2019, Genres = new List<GenreEntity> { genres[1], genres[4] } },
                            new BookEntity { Title = "Вакуум", Pages = 250, PublishYear = 2023, Genres = new List<GenreEntity> { genres[4], genres[1] } },
                            new BookEntity { Title = "Пітьма над Світом", Pages = 300, PublishYear = 2024, Genres = new List<GenreEntity> { genres[4] } }
                        }
                    }
                };

                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
            }
        }
    }
}
