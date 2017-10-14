using EmailReminderWJ.BE;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Authentication;
using System.Text;

namespace NS.ERWJ.DA
{
    class Dal : IDisposable
    {
        //private MongoServer mongoServer = null;
        private bool disposed = false;

        // To do: update the connection string with the DNS name
        // or IP address of your server. 
        //For example, "mongodb://testlinux.cloudapp.net
        private string userName = ConfigurationManager.AppSettings["mongoUserName"];
        private string host = ConfigurationManager.AppSettings["mongoHost"];
        private string password = ConfigurationManager.AppSettings["mongoPass"];

        // This sample uses a database named "Tasks" and a 
        //collection named "TasksList".  The database and collection 
        //will be automatically created if they don't already exist.
        private string dbName = ConfigurationManager.AppSettings["mongoDbName"];
        private string collectionName = ConfigurationManager.AppSettings["mongoCollectionName"];

        // Default constructor.        
        public Dal()
        {
        }

        // Creates a Task and inserts it into the collection in MongoDB.
        public void CreatePlanAlert(PlanAlertBE alert)
        {
            var collection = GetPlanAlertsCollectionForEdit();
            try
            {
                collection.InsertOne(alert);
            }
            catch (MongoCommandException exc)
            {
                Console.WriteLine("ERROR-CreatePlanAlert: " + exc.Message);
            }
        }

        public void DeletePlanAlert(string id)
        {
            var collection = GetPlanAlertsCollectionForEdit();
            try
            {
                collection.DeleteOne(x => x.Id == new Guid(id));
            }
            catch (MongoCommandException exc)
            {
                Console.WriteLine("ERROR-DeletePlanAlert: " + exc.Message);
            }
        }



        // Gets all Task items from the MongoDB server.        
        public List<PlanAlertBE> GetAllPlanAlerts()
        {
            try
            {
                var collection = GetPlanAlertsCollection();
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException exc)
            {
                Console.WriteLine("ERROR-GetAllPlanAlerts: " + exc.Message);
                return new List<PlanAlertBE>();
            }
        }

        // Gets all Task items from the MongoDB server.        
        public List<PlanAlertBE> GetAllEnabledPlanAlerts()
        {
            try
            {
                var collection = GetPlanAlertsCollection();

                var filter = Builders<PlanAlertBE>.Filter.Eq("Enabled", true);
                return collection.Find(filter).ToList();
            }
            catch (MongoConnectionException exc)
            {
                Console.WriteLine("ERROR-GetAllEnabledPlanAlerts: " + exc.Message);
                return new List<PlanAlertBE>();
            }
        }

        private IMongoCollection<PlanAlertBE> GetPlanAlertsCollection()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credentials = new List<MongoCredential>()
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };

            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var todoPlanAlertCollection = database.GetCollection<PlanAlertBE>(collectionName);

            return todoPlanAlertCollection;
        }

        private IMongoCollection<PlanAlertBE> GetPlanAlertsCollectionForEdit()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credentials = new List<MongoCredential>()
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var todoPlanAlertCollection = database.GetCollection<PlanAlertBE>(collectionName);
            return todoPlanAlertCollection;
        }

        # region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }

            this.disposed = true;
        }

        # endregion
    }
}
