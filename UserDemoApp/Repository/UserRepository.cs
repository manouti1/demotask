using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserDemoApp.Models;

namespace UserDemoApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;
        private readonly DbConfiguration _settings;

        public UserRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<User>(_settings.CollectionName);
        }


        public async Task SeedDataIfEmpty()
        {
            long count = await _collection.CountDocumentsAsync(FilterDefinition<User>.Empty);

            if (count == 0)
            {
                await SeedInitialData();
            }
        }

        private async Task SeedInitialData()
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Contact, f => f.Person.Phone)
                .RuleFor(u => u.Country, f => f.Address.Country())
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.ConsentGiven, f => f.Random.Bool());

            var usersToInsert = faker.Generate(3); 

            await _collection.InsertManyAsync(usersToInsert);
        }

        //create use async
        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            try
            {
                await _collection.InsertOneAsync(user, cancellationToken);
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //delete by id
        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _collection.DeleteOneAsync(c => c.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //get all users asynchronous, cancellation token is to abort gracefully in case of any issue in async call
        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _collection.Find(c => true).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateAsync(string id, User user, CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(c => c.Id, id);
                var options = new ReplaceOptions { IsUpsert = false };

                await _collection.ReplaceOneAsync(filter, user, options, cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
