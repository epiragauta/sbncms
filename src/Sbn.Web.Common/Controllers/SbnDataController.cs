using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Web.Common.Controllers
{
    public class SbnDataController : SbnApiController
    {

        private IScopeProvider ScopeProvider { get; set; }
        public SbnDataController(IScopeProvider scopeProvider) => ScopeProvider = scopeProvider;

        //public IEnumerable<string> GetAllTiposAsociatividad() => new[] { "Área Metropolitana", "Asociación de Municipios", "Provincias Administrativas y de Planificación", "Regiones de Planeación y Gestión" };

        public List<KeyValueDto> GetAllUsers()
        {
            var scope = ScopeProvider.CreateScope();
            List<KeyValueDto> dbResult = scope.Database.Fetch<KeyValueDto>("SELECT * FROM sbnKeyValue where [key] like '%@%'");
            return dbResult;
        }

        public StatusOperation SaveUser(string email, string organization, string sector, string fullUserName, string key){
            IScope scope = ScopeProvider.CreateScope();
            short count = scope.Database.ExecuteScalar<short>("SELECT count(*) FROM sbnKeyValue where [key] = @0", email);
            StatusOperation status = new StatusOperation();
            String hash = "P1o2i3u4y5t6";
            status.Status = "False";
            if (hash != key)
            {
                status.Message = "No puede ingresar información";
                return status;
            }
            
            if (count != 0)
            {
                
                status.Message = "Usuario ya existe";
            }
            else
            {
                KeyValueDto dto = new KeyValueDto();
                dto.Key = email;
                UserGuest guest = new UserGuest();
                guest.Sector = sector;
                guest.FullName = fullUserName;
                guest.Organization = organization;
                dto.Value = JsonSerializer.Serialize(guest);
                dto.UpdateDate = DateTime.Now;

                scope.Database.Execute("INSERT INTO sbnKeyValue VALUES (@0, @1,@2)", email, dto.Value, DateTime.Now);
                scope.Database.CompleteTransaction();
                //object obj = scope.Database.Insert(dto);
                //scope.Database.Save(dto);
                
                status.Status = "True";
                status.Message = string.Format("El {0} usuario ha sido creado", email);
            }
            return status;
            
        }
    }

    public class StatusOperation
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class UserGuest
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
        [JsonPropertyName("organization")]
        public string Organization { get; set; }
        [JsonPropertyName("sector")]
        public string Sector { get; set; }

    }
}
