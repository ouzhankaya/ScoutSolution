using Scout.DataAccessLayer.EntityFramework;
using Scout.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.DataAccessLayer
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            for (int j = 0; j < 80; j++)
            {
                Country country = new Country()
                {
                    CountryName = FakeData.PlaceData.GetCountry()

                };
                context.Countries.Add(country);
                for (int m = 0; m < 20; m++)
                {
                    Province province = new Province()
                    {
                        ProvinceName = FakeData.PlaceData.GetCity()
                    };
                    country.Provinces.Add(province);

                }
            }
            context.SaveChanges();

            Foot sagAyak = new Foot()
            {
                FootName = "Sağ Ayak"
            };
            Foot solAyak = new Foot()
            {
                FootName = "Sol Ayak"
            };
            Foot herIkiAyak = new Foot()
            {
                FootName = "Her İki Ayak"
            };
            context.Feet.Add(sagAyak);
            context.Feet.Add(solAyak);
            context.Feet.Add(herIkiAyak);
            context.SaveChanges();


            Position kaleci = new Position()
            {
                PositionName = "Kaleci"
            };
            Position sagBek = new Position()
            {
                PositionName = "Sağ Bek"
            };
            Position solBek = new Position()
            {
                PositionName = "Sol Bek"
            };
            Position stoper = new Position()
            {
                PositionName = "Stoper"
            };
            Position defOrtaSaha = new Position()
            {
                PositionName = "Defansif Ortaha"
            };
            Position mrkOrtaSaha = new Position()
            {
                PositionName = "Merkez Orta Saha"
            };
            Position ofOrtaSaha = new Position()
            {
                PositionName = "Ofansif Orta Saha"
            };
            Position sagKanat = new Position()
            {
                PositionName = "Sağ Kanat"
            };
            Position solKanat = new Position()
            {
                PositionName = "Sol kanat"
            };
            Position forvet = new Position()
            {
                PositionName = "Forvet"
            };
            context.Positions.Add(kaleci);
            context.Positions.Add(sagBek);
            context.Positions.Add(solBek);
            context.Positions.Add(stoper);
            context.Positions.Add(defOrtaSaha);
            context.Positions.Add(mrkOrtaSaha);
            context.Positions.Add(ofOrtaSaha);
            context.Positions.Add(sagKanat);
            context.Positions.Add(solKanat);
            context.Positions.Add(forvet);
            context.SaveChanges();


            OtherPosition kaleci1 = new OtherPosition()
            {
                OtherPositionName = "Kaleci"
            };
            OtherPosition sagBek1 = new OtherPosition()
            {
                OtherPositionName = "Sağ Bek"
            };
            OtherPosition solBek1 = new OtherPosition()
            {
                OtherPositionName = "Sol Bek"
            };
            OtherPosition stoper1 = new OtherPosition()
            {
                OtherPositionName = "Stoper"
            };
            OtherPosition defOrtaSaha1 = new OtherPosition()
            {
                OtherPositionName = "Defansif Ortaha"
            };
            OtherPosition mrkOrtaSaha1 = new OtherPosition()
            {
                OtherPositionName = "Merkez Orta Saha"
            };
            OtherPosition ofOrtaSaha1 = new OtherPosition()
            {
                OtherPositionName = "Ofansif Orta Saha"
            };
            OtherPosition sagKanat1 = new OtherPosition()
            {
                OtherPositionName = "Sağ Kanat"
            };
            OtherPosition solKanat1 = new OtherPosition()
            {
                OtherPositionName = "Sol kanat"
            };
            OtherPosition forvet1 = new OtherPosition()
            {
                OtherPositionName = "Forvet"
            };
            context.OtherPositions.Add(kaleci1);
            context.OtherPositions.Add(sagBek1);
            context.OtherPositions.Add(solBek1);
            context.OtherPositions.Add(stoper1);
            context.OtherPositions.Add(defOrtaSaha1);
            context.OtherPositions.Add(mrkOrtaSaha1);
            context.OtherPositions.Add(ofOrtaSaha1);
            context.OtherPositions.Add(sagKanat1);
            context.OtherPositions.Add(solKanat1);
            context.OtherPositions.Add(forvet1);
            context.SaveChanges();



            Admin admin = new Admin()
            {
                Name = "Oğuzhan",
                Lastname = "Kaya",
                ActivateGuid = Guid.NewGuid(),
                Email = "oguuzhankaya@gmail.com",
                Username = "oguzhankaya",
                ProfileImageFileName = "user.png",
                Password = "123456",
                DateOfBirth = Convert.ToDateTime("20.08.1994"),
                IsActive = true
            };
            context.Admins.Add(admin);

            List<Foot> footList = context.Feet.ToList();
            List<Position> positionList = context.Positions.ToList();
            List<OtherPosition> otherPositionList = context.OtherPositions.ToList();
            List<Country> countryList = context.Countries.ToList();
            List<Province> provinceList = context.Provinces.ToList();
            for (int i = 0; i < 20; i++)
            {
                Foot footUser = footList[FakeData.NumberData.GetNumber(0, 1)];
                Position positionUser = positionList[FakeData.NumberData.GetNumber(0, 1)];
                Country countryUser = countryList[FakeData.NumberData.GetNumber(0, 1)];
                Province provinceUser = provinceList[FakeData.NumberData.GetNumber(0, 1)];
                OtherPosition otherPositionUser = otherPositionList[FakeData.NumberData.GetNumber(0, 1)];
                Footballer footballer = new Footballer()
                {

                    Name = FakeData.NameData.GetFirstName(),
                    Lastname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    Username = $"footballer{i}",
                    ProfileImageFileName = "user.png",
                    Password = "123456",
                    PhoneNumber = FakeData.PhoneNumberData.GetPhoneNumber(),
                    DateOfBirth = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-8)),
                    Height = FakeData.NumberData.GetNumber(155, 200),
                    Weight = FakeData.NumberData.GetNumber(40, 100),
                    Country = countryUser,
                    Province = provinceUser,
                    Position = positionUser,
                    ActivateGuid = Guid.NewGuid(),
                    OtherPosition = otherPositionUser,
                    Foot = footUser,
                    IsActive = true
                };
                context.Footballers.Add(footballer);
                context.SaveChanges();
            }

            for (int j = 0; j < 15; j++)
            {
                Manager manager = new Manager()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Lastname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    Username = $"manager{j}",
                    ProfileImageFileName = "manager.png",
                    Password = "123456",
                    ActivateGuid = Guid.NewGuid(),
                    DateOfBirth = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-70), DateTime.Now.AddYears(-25)),
                    CountOfViewProfile = FakeData.NumberData.GetNumber(50, 70),
                    IsActive = true
                };
                context.Managers.Add(manager);
                context.SaveChanges();
            }




            List<Footballer> footballerList = context.Footballers.ToList();
            List<Manager> managerList = context.Managers.ToList();
            //Adding Shares

            for (int i = 0; i < FakeData.NumberData.GetNumber(20, 30); i++)
            {
                Footballer owner = footballerList[FakeData.NumberData.GetNumber(0, footballerList.Count - 1)];
                Share share = new Share()
                {

                    ShareText = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                    Owner = owner,
                    LikeCount = FakeData.NumberData.GetNumber(1, 15),
                    CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ShareImageFileName = "footballer.jpg",
                };
                context.Shares.Add(share);
                //adding fake comments
                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 20); j++)
                {
                    Manager comment_owner = managerList[FakeData.NumberData.GetNumber(0, managerList.Count - 1)];

                    Comment comment = new Comment()
                    {
                        CommentText = FakeData.TextData.GetSentence(),
                        Manager = comment_owner,
                        CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now)
                    };

                    share.Comments.Add(comment);
                }
                // adding fake likes
                for (int m = 0; m < share.LikeCount; m++)
                {
                    Liked liked = new Liked()
                    {
                        LikedUser = managerList[m]
                    };

                    share.Likes.Add(liked);
                }
            }
            context.SaveChanges();
            for (int i = 0; i < FakeData.NumberData.GetNumber(7, 15); i++)
            {
                Footballer owner = footballerList[FakeData.NumberData.GetNumber(0, footballerList.Count - 1)];
                Share share = new Share()
                {

                    ShareText = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                    Owner = owner,
                    LikeCount = FakeData.NumberData.GetNumber(1, 15),
                    CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ShareVideoFileName = "totti.mp4",
                };
                context.Shares.Add(share);

                
                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 20); j++)
                {
                    Manager comment_owner = managerList[FakeData.NumberData.GetNumber(0, managerList.Count - 1)];

                    Comment comment = new Comment()
                    {
                        CommentText = FakeData.TextData.GetSentence(),
                        Manager = comment_owner,
                        CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now)

                    };

                    share.Comments.Add(comment);
                }
                // adding fake likes
                for (int m = 0; m < share.LikeCount; m++)
                {
                    Liked liked = new Liked()
                    {
                        LikedUser = managerList[m]
                    };

                    share.Likes.Add(liked);
                }
            }
            context.SaveChanges();

        }

    }

}
