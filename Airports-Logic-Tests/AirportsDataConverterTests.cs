namespace Airports_Logic_Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Airports_IO.Models;
    using Airports_Logic.Services;
    using FluentAssertions;
    using NUnit.Framework;

    public class AirportsDataConverterTests
    {
        [Test]
        public void Convert_ListIsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            List<AirportsParseResult> input = new List<AirportsParseResult>();
            AirportsDataConverter sut = new AirportsDataConverter();

            // Act
            var convertedInput = sut.ConvertToModel(input);

            // Assert
            convertedInput.Airports.Should().BeEmpty();
            convertedInput.Cities.Should().BeEmpty();
            convertedInput.Countries.Should().BeEmpty();
        }

        [Test]
        public void Convert_ListIsNull_ShouldReturnEmptyList()
        {
            // Arrange
            AirportsDataConverter sut = new AirportsDataConverter();

            // Act
            var convertedInput = sut.ConvertToModel(null);

            // Assert
            convertedInput.Airports.Should().BeEmpty();
            convertedInput.Cities.Should().BeEmpty();
            convertedInput.Countries.Should().BeEmpty();
        }

        [Test]
        public void Convert_ListHasAFewElements_ShouldReturnWithTheCorrectBundle()
        {
            // Arrange
            AirportsDataConverter sut = new AirportsDataConverter();
            List<AirportsParseResult> input = new List<AirportsParseResult>
            {
                new AirportsParseResult()
                {
                    Id = "1",
                    AirportName = "Totally not fake airport",
                    Altitude = "999",
                    CityName = "Fakepest",
                    CountryName = "Notacountrystan",
                    IATA = "NTA",
                    ICAO = "OOOO",
                    Latitude = "333.33",
                    Longitude = "64.33",
                },
                new AirportsParseResult()
                {
                    Id = "2",
                    AirportName = "Viktorhegyi repuloter",
                    Altitude = "1111",
                    CityName = "Nádasbesenyö",
                    CountryName = "Fungary",
                    IATA = "PPB",
                    ICAO = "AABB",
                    Latitude = "435.31",
                    Longitude = "38.16",
                },
                new AirportsParseResult()
                {
                    Id = "3",
                    AirportName = "Idezuhanj",
                    Altitude = "4433",
                    CityName = "Nádasbesenyö",
                    CountryName = "Fungary",
                    IATA = "SDE",
                    ICAO = "AFVD",
                    Latitude = "135.15",
                    Longitude = "78.34",
                },
            };

            // Act
            var conversionResult = sut.ConvertToModel(input);

            // Assert
            conversionResult.Airports.Count().Should().Be(3);
            conversionResult.Airports.Where(a => a.Name == input.First().AirportName).Count().Should().Be(1);

            conversionResult.Cities.Count().Should().Be(2);
            conversionResult.Cities.Where(c => c.Name == input.First().CityName).Count().Should().Be(1);

            conversionResult.Countries.Count().Should().Be(2);
            conversionResult.Countries.Where(c => c.Name == input.First().CountryName).Count().Should().Be(1);
        }
    }
}