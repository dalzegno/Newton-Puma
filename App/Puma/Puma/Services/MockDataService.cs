using Bogus;
using Puma.MockDataModels;
using Puma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Puma.Services
{
    internal class MockDataService
    {
        readonly IUserApiService _userApiService = DependencyService.Get<IUserApiService>();
        readonly IPoiService _poiService = DependencyService.Get<IPoiService>();

        Faker<AddUserDto> _user;
        //Faker _pois;

        List<User> _addedUsers;
        List<AddressAndPosition> _addressesAndPositions;
        Random _rng;

        PositionPoi positionPoi;

        public MockDataService()
        {
            _addedUsers = new List<User>();
            _addressesAndPositions = new List<AddressAndPosition>();
        }


        public async Task GenerateUsers(int count)
        {
            var _user = new Faker<AddUserDto>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Password, () => "123456");

            for (int i = 0; i < count; i++)
            {
                var generated = _user.Generate();
                var addedUser = await _userApiService.CreateUserAsync(generated);

                _addedUsers.Add(addedUser);
            }
        }


        public async Task GeneratePois(int count)
        {
            var pois = new Faker<AddPoiDto>()
                .RuleFor(poi => poi.UserId, () => _addedUsers[GetRandomIndex(_addedUsers.Count)].Id)
                .RuleFor(poi => poi.Name, f => f.Company.CompanyName())
                .RuleFor(poi => poi.Position, f => _addressesAndPositions[GetRandomIndex(_addressesAndPositions.Count)].Position)
                .RuleFor(poi => poi.Address, (_, poi) =>
                _addressesAndPositions.FirstOrDefault(ap => ap.Position.Latitude == poi.Position.Latitude &&
                                                            ap.Position.Longitude == poi.Position.Longitude).Address)
                .RuleFor(poi => poi.Description, f => f.Lorem.Sentence())
                .RuleFor(poi => poi.TagIds, (_, poi) => new List<int> { _rng.Next(1, 5), _rng.Next(1, 5) });

            for (int i = 0; i < count; i++)
            {
                var generatedPoi = pois.Generate();
                await _poiService.CreatePoiAsync(generatedPoi);
            }
        }

        public async Task GenerateComments(int count)
        {
            var pois = await _poiService.GetAllAsync();
            var users = await _userApiService.GetAll();

            var comments = new Faker<AddCommentDto>()
                .RuleFor(c => c.PointOfInterestId, () => pois[GetRandomIndex(pois.Count)].Id)
                .RuleFor(c => c.Body, f => f.Lorem.Sentence())
                .RuleFor(c => c.UserId, () => users[GetRandomIndex(users.Count)].Id);

            for (int i = 0; i < count; i++)
            {
                var comment = comments.Generate();
                await _poiService.AddCommentAsync(comment);
            }
        }

        public async Task GenerateGrades(int count)
        {
            var pois = await _poiService.GetAllAsync();
            var users = await _userApiService.GetAll();

            var grades = new Faker<AddGradeDto>()
                .RuleFor(g => g.Grade, () => GetRandomGrade())
                .RuleFor(g => g.UserId, () => users[GetRandomIndex(users.Count)].Id)
                .RuleFor(g => g.PoiId, () => pois[GetRandomIndex(pois.Count)].Id);

            for (int i = 0; i < count; i++)
            {
                var grade = grades.Generate();
                await _poiService.AddGradeAsync(grade);
            }
        }

        private int GetRandomGrade()
        {
            var random = new Random();
            var grade = random.Next(1, 3);
            return grade;
        }

        private int GetRandomIndex(int count)
        {
            Random random = new Random();
            int index = random.Next(count);
            return index;
        }
        public PositionPoi GetRandomPosition(Random random)
        {


            int lat = random.Next(3000000, 3400000);
            int lon = random.Next(300000, 700000);

            var randomPosition = new Position(Convert.ToDouble("59." + lat), Convert.ToDouble("18.0" + lon));
            var positionPoi = new PositionPoi(randomPosition.Latitude, randomPosition.Longitude);

            return positionPoi;
        }

        public async Task<Address> GetRandomAddress(Geocoder geoCoder, PositionPoi position)
        {
            var pos = new Position(position.Latitude, position.Longitude);
            var addresses = await geoCoder.GetAddressesForPositionAsync(pos);
            var returnedAddress = addresses.FirstOrDefault();
            return GetAddress(returnedAddress);
        }

        private Address GetAddress(string address)
        {

            var words = address?.Split('\n') ?? Array.Empty<string>();
            var newAddress = new Address();
            if (words.Length == 1)
            {
                newAddress.StreetName = "";
                newAddress.Area = "";
                newAddress.Country = words[0];
            }
            else if (words.Length == 2)
            {
                newAddress.StreetName = "";
                newAddress.Area = words[0];
                newAddress.Country = words[1];
            }
            else if (words.Length == 3)
            {
                newAddress.StreetName = words[0];
                newAddress.Area = words[1];
                newAddress.Country = words[2];
            }

            return newAddress;
        }


        public async Task GetRandomAddressAndPosition(Geocoder geoCoder, int count)
        {
            _rng = new Random();
            for (int i = 0; i < count; i++)
            {
                var pos = GetRandomPosition(_rng);
                var address = await GetRandomAddress(geoCoder, pos);

                while (address.StreetName == "" || !address.StreetName.Any(c => char.IsDigit(c)))
                {
                    pos = GetRandomPosition(_rng);
                    address = await GetRandomAddress(geoCoder, pos);
                }

                var adAndPos = new AddressAndPosition()
                {
                    Address = address,
                    Position = pos
                };

                _addressesAndPositions.Add(adAndPos);
            }
        }
    }
}
