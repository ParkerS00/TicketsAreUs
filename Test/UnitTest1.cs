using System.Net.Http;
using System.Net.Http.Json;
using FluentAssertions;
using RazorClassLib.Data;
using WebApp.Exceptions;

namespace Test
{
    public class UnitTest1 : IClassFixture<TicketsApiFactory>
    {
        private HttpClient httpClient;
        public UnitTest1(TicketsApiFactory factory)
        {
            httpClient = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task CanAddNewOccasion()
        {
            //Arrange
            Occasion newOccasion = new Occasion()
            {
                OccasionName = "Test Occasion",
                TimeOfDay = new DateTime()
            };

            //Act
            await httpClient.PostAsJsonAsync<Occasion>($"ocassion/{newOccasion}", newOccasion);
            var grabOccasion = await httpClient.GetFromJsonAsync<Occasion>($"occasion/{1}");

            //Assert
            newOccasion.OccasionName.Equals(grabOccasion.OccasionName);
            newOccasion.Id.Equals(grabOccasion.Id);
        }

        [Fact]
        public async Task CanAddNewTicket()
        {
            //Arrange
            Occasion newOccasion = new Occasion()
            {
                OccasionName = "Test Occasion",
                TimeOfDay = new DateTime()
            };

            Ticket newTicket = new Ticket()
            {
                Guid = Guid.NewGuid(),
                IsUsed = false,
                OccasionId = 1
            };

            //Act
            await httpClient.PostAsJsonAsync<Occasion>($"ocassion/{newOccasion}", newOccasion);
            await httpClient.PostAsJsonAsync<Ticket>($"ticket/add/{newTicket}", newTicket);

            var grabTicket = await httpClient.GetFromJsonAsync<Ticket>($"ticket/{1}");

            //Assert
            newTicket.OccasionId.Equals(grabTicket.OccasionId);
            newTicket.Guid.Equals(grabTicket.Guid);
            newTicket.Id.Equals(grabTicket.Id);
        }

        [Fact]
        public async Task CanUpdateAndUseTicket()
        {
            //Arrange
            Occasion newOccasion = new Occasion()
            {
                OccasionName = "Test Occasion",
                TimeOfDay = new DateTime()
            };

            Ticket newTicket = new Ticket()
            {
                Guid = Guid.NewGuid(),
                IsUsed = false,
                OccasionId = 1
            };

            //Act
            await httpClient.PostAsJsonAsync<Occasion>($"ocassion/{newOccasion}", newOccasion);
            await httpClient.PostAsJsonAsync<Ticket>($"ticket/add/{newTicket}", newTicket);

            await httpClient.PatchAsJsonAsync<Ticket>($"ticket/update/{newTicket}", newTicket);
            var updateTicket = await httpClient.GetFromJsonAsync<Ticket>($"ticket/{1}");

            //Assert
            updateTicket.Guid.Equals(newTicket.Guid);
            updateTicket.Id.Equals(newTicket.Id);
            updateTicket.IsUsed.Equals(true);
        }

        [Fact]
        public async Task CantUseAlreadyUsedTicket()
        {
            //Arrange
            Occasion newOccasion = new Occasion()
            {
                OccasionName = "Test Occasion",
                TimeOfDay = new DateTime()
            };

            Ticket newTicket = new Ticket()
            {
                Guid = Guid.NewGuid(),
                IsUsed = false,
                OccasionId = 1
            };

            //Act
            await httpClient.PostAsJsonAsync<Occasion>($"ocassion/{newOccasion}", newOccasion);
            await httpClient.PostAsJsonAsync<Ticket>($"ticket/add/{newTicket}", newTicket);

            await httpClient.PatchAsJsonAsync<Ticket>($"ticket/update/{newTicket}", newTicket);
            var updateTicket = await httpClient.GetFromJsonAsync<Ticket>($"ticket/{1}");

            var response = await httpClient.PatchAsJsonAsync<Ticket>($"ticket/update/{updateTicket}", updateTicket);

            //Assert
            response.Content.ReadFromJsonAsync<TicketAlreadyScannedException>().Should().NotBe(null);
        }

        [Fact]
        public async Task NotCorrectOccasionForTicket()
        {
            //Arrange
            Occasion newOccasion = new Occasion()
            {
                OccasionName = "Test Occasion",
                TimeOfDay = new DateTime()
            };

            Ticket newTicket = new Ticket()
            {
                Guid = Guid.NewGuid(),
                IsUsed = false,
                OccasionId = 2
            };

            //Act
            await httpClient.PostAsJsonAsync<Occasion>($"ocassion/{newOccasion}", newOccasion);
            await httpClient.PostAsJsonAsync<Ticket>($"ticket/add/{newTicket}", newTicket);

            var response = await httpClient.PatchAsJsonAsync<Ticket>($"ticket/update/{newTicket}", newTicket);

            //Assert
            response.Content.ReadFromJsonAsync<OccasionNotCorrectException>().Should().NotBe(null);
        }
    }
}
